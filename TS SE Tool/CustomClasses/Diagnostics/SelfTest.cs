/*
   Headless diagnostic harness added during the 2026 save-format investigation.

   Usage:  "TS SE Tool.exe" --selftest <pathToSaveFolder> [outputFolder]

   It runs the exact production pipeline
       game.sii -> decode -> SiiNunit parse -> PrintOut -> text
   without the WinForms UI, dumps every intermediate artefact and reports the
   *real* exception (type, message, stack, inner) instead of the generic
   "Something went wrong during Writing Save file" message box.

   It never writes into the save folder.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using TS_SE_Tool.Save.Items;

namespace TS_SE_Tool.Diagnostics
{
    internal static class SelfTest
    {
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private static StringBuilder _report = new StringBuilder();
        private static string _outDir;

        private static void Say(string line)
        {
            Console.WriteLine(line);
            _report.AppendLine(line);
        }

        internal static int Run(string[] args)
        {
            if (!AttachConsole(-1))
                AllocConsole();

            if (args.Length < 2)
            {
                Console.WriteLine("usage: \"TS SE Tool.exe\" --selftest <saveFolder> [outFolder]");
                return 2;
            }

            string saveDir = args[1].TrimEnd('\\', '/');
            _outDir = args.Length > 2 && !args[2].StartsWith("--")
                ? args[2]
                : Path.Combine(Path.GetTempPath(), "tsset_selftest");

            int setLevel = -1;
            for (int i = 2; i < args.Length - 1; i++)
                if (args[i] == "--set-level")
                    int.TryParse(args[i + 1], out setLevel);

            Directory.CreateDirectory(_outDir);

            Say("=== TS SE Tool self-test ===");
            Say("save folder : " + saveDir);
            Say("output      : " + _outDir);
            Say("");

            int exit = 0;

            try
            {
                exit = RunPipeline(saveDir, setLevel);
            }
            catch (Exception ex)
            {
                Say("");
                Say("###### UNHANDLED EXCEPTION IN HARNESS ######");
                Say(Describe(ex));
                exit = 1;
            }

            File.WriteAllText(Path.Combine(_outDir, "selftest_report.txt"), _report.ToString());
            Console.WriteLine();
            Console.WriteLine("report written to " + Path.Combine(_outDir, "selftest_report.txt"));
            return exit;
        }

        private static string Describe(Exception ex)
        {
            var sb = new StringBuilder();
            int depth = 0;
            for (Exception e = ex; e != null; e = e.InnerException, depth++)
            {
                string pad = new string(' ', depth * 2);
                sb.AppendLine(pad + "Type    : " + e.GetType().FullName);
                sb.AppendLine(pad + "Message : " + e.Message);
                if (e.TargetSite != null)
                    sb.AppendLine(pad + "Site    : " + e.TargetSite.DeclaringType + "." + e.TargetSite.Name);
                sb.AppendLine(pad + "Stack   :");
                sb.AppendLine(e.StackTrace ?? "  <none>");
            }
            return sb.ToString();
        }

        private static int RunPipeline(string saveDir, int setLevel)
        {
            string infoPath = Path.Combine(saveDir, "info.sii");
            string gamePath = Path.Combine(saveDir, "game.sii");

            //--- 1. info.sii -------------------------------------------------
            Say("[1] decoding info.sii");
            string[] infoLines = Decode(infoPath);
            if (infoLines == null) { Say("    FAILED to decode info.sii"); return 1; }
            Say("    lines = " + infoLines.Length + " ; first = " + infoLines[0]);
            File.WriteAllLines(Path.Combine(_outDir, "info.decoded.sii"), infoLines);

            SaveFileInfoData info = new SaveFileInfoData();
            info.ProcessData(infoLines);
            ushort version = info.Version;
            Say("    savefile version = " + version);
            CompareRoundTrip("info.sii", infoLines, info.PrintOut());
            Say("");

            //--- 1b. profile.sii (written too whenever the profile tab is edited) ----
            string profilePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(saveDir)) ?? "", "profile.sii");

            if (File.Exists(profilePath))
            {
                Say("[1b] decoding profile.sii");
                string[] profileLines = Decode(profilePath);

                if (profileLines != null)
                {
                    SaveFileProfileData profile = new SaveFileProfileData();
                    profile.ProcessData(profileLines);
                    File.WriteAllLines(Path.Combine(_outDir, "profile.decoded.sii"), profileLines);
                    CompareRoundTrip("profile.sii", profileLines, profile.PrintOut());
                }
                Say("");
            }

            //--- 2. game.sii decode ------------------------------------------
            Say("[2] decoding game.sii");
            string[] gameLines = Decode(gamePath);
            if (gameLines == null) { Say("    FAILED to decode game.sii"); return 1; }
            Say("    lines = " + gameLines.Length + " ; first = " + gameLines[0]);
            string decodedPath = Path.Combine(_outDir, "game.decoded.sii");
            File.WriteAllText(decodedPath, string.Join(Environment.NewLine, gameLines));
            Say("    written " + decodedPath);
            Say("");

            //--- 3. parse -----------------------------------------------------
            Say("[3] parsing into SiiNunit");
            SiiNunit unit;
            try
            {
                unit = new SiiNunit(gameLines);
            }
            catch (Exception ex)
            {
                Say("    ###### PARSE FAILED ######");
                Say(Describe(ex));
                return 1;
            }

            Say("    blocks parsed            = " + unit.SiiNitems.Count);
            Say("    unidentified block types = " + unit.UnidentifiedBlocks.Count);
            foreach (string b in unit.UnidentifiedBlocks.Take(40))
                Say("        " + b);

            // per-block unparsed lines
            var unknownLines = new List<string>();
            foreach (var kv in unit.SiiNitems)
            {
                if (kv.Value == null) continue;
                SiiNBlockCore core = kv.Value as SiiNBlockCore;
                if (core == null) continue;
                foreach (string l in core.UnidentifiedLines)
                    unknownLines.Add(((object)kv.Value).GetType().Name + " | " + kv.Key + " | " + l.Trim());
            }
            Say("    unrecognised lines inside known blocks = " + unknownLines.Count);
            File.WriteAllLines(Path.Combine(_outDir, "unrecognised_lines.txt"), unknownLines);

            var summary = unknownLines
                .Select(x => x.Split('|'))
                .Select(p => p[0].Trim() + " :: " + p[2].Trim().Split(':')[0].Trim())
                .Distinct().OrderBy(x => x).ToList();
            foreach (string l in summary.Take(120))
                Say("        " + l);
            Say("");

            //--- 3b. optional edit ---------------------------------------------
            //FormMethods.SetDefaultValues normally fills this in; the harness has no form.
            if (Globals.PlayerLevelUps.Length == 0)
                Globals.PlayerLevelUps = new int[] {200, 500, 700, 900, 1100, 1300, 1500, 1700, 1900, 2100,
                    2300, 2500, 2700, 2900, 3100, 3300, 3500, 3700, 4000, 4300,
                    4600, 4900, 5200, 5500, 5800, 6100, 6400, 6700, 7000, 7300}; //ATS

            if (setLevel >= 0)
            {
                Say("[3b] editing player level");
                Say("     experience_points before = " + unit.Economy.experience_points +
                    " (level " + unit.Economy.getPlayerLvl()[0] + ")");

                unit.Economy.setPlayerExp(setLevel);

                Say("     requested level          = " + setLevel);
                Say("     experience_points after  = " + unit.Economy.experience_points +
                    " (level " + unit.Economy.getPlayerLvl()[0] + ")");
                Say("");
            }

            //--- 4. serialize --------------------------------------------------
            Say("[4] serialising with PrintOut(version=" + version + ")");
            string output;
            try
            {
                output = unit.PrintOut(version);
            }
            catch (Exception ex)
            {
                Say("    ###### PRINTOUT FAILED - this is the real exception ######");
                Say(Describe(ex));
                return 1;
            }

            string outPath = Path.Combine(_outDir, "game.roundtrip.sii");
            File.WriteAllText(outPath, output);
            Say("    written " + outPath + " (" + output.Length + " chars)");
            Say("");

            //--- 5. compare ----------------------------------------------------
            Say("[5] comparing block inventory original vs round-trip");
            string[] origLines = File.ReadAllLines(decodedPath);
            string[] rtLines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            var before = BlockInventory(origLines);
            var after = BlockInventory(rtLines);

            Say("    blocks original   = " + before.Values.Sum() + " (" + before.Count + " types)");
            Say("    blocks round-trip = " + after.Values.Sum() + " (" + after.Count + " types)");

            foreach (string tag in before.Keys.Union(after.Keys).OrderBy(x => x))
            {
                int b = before.ContainsKey(tag) ? before[tag] : 0;
                int a = after.ContainsKey(tag) ? after[tag] : 0;
                if (a != b)
                    Say("    DIFF " + tag.PadRight(34) + " original=" + b + " roundtrip=" + a);
            }

            var namesBefore = BlockNames(origLines);
            var namesAfter = BlockNames(rtLines);
            var lost = namesBefore.Except(namesAfter).ToList();
            var dup = namesAfter.GroupBy(x => x).Where(g => g.Count() > 1)
                                .Select(g => g.Key + " x" + g.Count()).ToList();
            Say("    lost block instances       = " + lost.Count);
            foreach (string s in lost.Take(40)) Say("        LOST " + s);
            Say("    duplicated block instances = " + dup.Count);
            foreach (string s in dup.Take(40)) Say("        DUP  " + s);

            //--- 6. reload what we just produced --------------------------------
            Say("");
            Say("[6] re-parsing the produced file (simulated reload)");

            SiiNunit reloaded;
            try
            {
                reloaded = new SiiNunit(rtLines);
            }
            catch (Exception ex)
            {
                Say("    ###### RELOAD FAILED ######");
                Say(Describe(ex));
                return 1;
            }

            Say("    blocks parsed     = " + reloaded.SiiNitems.Count);
            Say("    experience_points = " + reloaded.Economy.experience_points +
                " (level " + reloaded.Economy.getPlayerLvl()[0] + ")");

            if (reloaded.Economy.experience_points != unit.Economy.experience_points)
            {
                Say("    ###### XP DID NOT SURVIVE THE ROUND-TRIP ######");
                return 1;
            }

            try
            {
                string second = reloaded.PrintOut(version);

                Say("    second serialisation ok (" + second.Length + " chars, " +
                    (second == output ? "byte identical to the first" : "DIFFERS from the first") + ")");

                if (second != output)
                {
                    File.WriteAllText(Path.Combine(_outDir, "game.roundtrip2.sii"), second);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Say("    ###### SECOND SERIALISATION FAILED ######");
                Say(Describe(ex));
                return 1;
            }

            Say("");
            Say("=== pipeline completed ===");
            return 0;
        }

        /// <summary>
        /// Line level comparison for the small single-block files (info.sii, profile.sii).
        /// </summary>
        private static void CompareRoundTrip(string label, string[] original, string produced)
        {
            var before = original.Select(x => x.Trim()).Where(x => x.Length > 0).OrderBy(x => x, StringComparer.Ordinal).ToList();
            var after = produced.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                                .Select(x => x.Trim()).Where(x => x.Length > 0).OrderBy(x => x, StringComparer.Ordinal).ToList();

            var lost = before.Except(after).ToList();
            var added = after.Except(before).ToList();

            if (lost.Count == 0 && added.Count == 0)
            {
                Say("    " + label + " round-trip OK (" + before.Count + " lines)");
                return;
            }

            Say("    " + label + " round-trip DIFFERS - lost=" + lost.Count + " added=" + added.Count);
            foreach (string l in lost.Take(15)) Say("        LOST  " + l);
            foreach (string l in added.Take(15)) Say("        ADDED " + l);
        }

        private static Dictionary<string, int> BlockInventory(string[] lines)
        {
            var d = new Dictionary<string, int>();
            foreach (string raw in lines)
            {
                string l = raw.Trim();
                if (!l.Contains(':') || !l.Contains('{')) continue;
                string tag = l.Split(new[] { ':', '{' }, 3)[0].Trim();
                if (tag.Length == 0) continue;
                d[tag] = d.ContainsKey(tag) ? d[tag] + 1 : 1;
            }
            return d;
        }

        private static List<string> BlockNames(string[] lines)
        {
            var list = new List<string>();
            foreach (string raw in lines)
            {
                string l = raw.Trim();
                if (!l.Contains(':') || !l.Contains('{')) continue;
                string[] p = l.Split(new[] { ':', '{' }, 3);
                list.Add(p[0].Trim() + " " + p[1].Trim());
            }
            return list;
        }

        //================== decoding (same DLL as the app) ==================
        private static unsafe string[] Decode(string path)
        {
            if (!File.Exists(path)) { Say("    missing file " + path); return null; }

            byte[] data = File.ReadAllBytes(path);
            if (data.Length == 0) { Say("    EMPTY file " + path); return null; }

            uint size = (uint)data.Length;
            int format;
            fixed (byte* p = data) format = FormMain.SIIGetMemoryFormat(p, size);

            Say("    format code = " + format);

            if (format == 1)
                return Encoding.UTF8.GetString(data).Split(new[] { "\r\n" }, StringSplitOptions.None);

            uint outSize = 0;
            int result;

            if (format == 2)
            {
                fixed (byte* p = data) result = FormMain.SIIDecryptAndDecodeMemory(p, size, null, &outSize);
                if (result != 0) { Say("    decrypt probe failed: " + result); return null; }
                byte[] outData = new byte[outSize];
                fixed (byte* p = data)
                fixed (byte* q = outData)
                    result = FormMain.SIIDecryptAndDecodeMemory(p, size, q, &outSize);
                if (result != 0) { Say("    decrypt failed: " + result); return null; }
                return Encoding.UTF8.GetString(outData).Split(new[] { "\r\n" }, StringSplitOptions.None);
            }

            if (format == 3 || format == 4)
            {
                fixed (byte* p = data) result = FormMain.SIIDecodeMemory(p, size, null, &outSize);
                if (result != 0) { Say("    decode probe failed: " + result); return null; }
                byte[] outData = new byte[outSize];
                fixed (byte* p = data)
                fixed (byte* q = outData)
                    result = FormMain.SIIDecodeMemory(p, size, q, &outSize);
                if (result != 0) { Say("    decode failed: " + result); return null; }
                return Encoding.UTF8.GetString(outData).Split(new[] { "\r\n" }, StringSplitOptions.None);
            }

            Say("    unsupported format code " + format);
            return null;
        }
    }
}
