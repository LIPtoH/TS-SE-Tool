/*
   Copyright 2016-2022 LIPtoH <liptoh.codebase@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace TS_SE_Tool.Utilities {
    static class IO_Extensions {
        public enum MachineType : ushort {
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,
            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
            IMAGE_FILE_MACHINE_ARM64 = 0xaa64
        }
        #region DirectoryInfo
        public static DirectoryInfo Combine(this DirectoryInfo dir, params string[] paths) {
            var final = dir.FullName;
            foreach (var path in paths) {
                final = Path.Combine(final, path);
            }
            return new DirectoryInfo(final);
        }
        public static bool IsEmpty(this DirectoryInfo directory) {
            return !Directory.EnumerateFileSystemEntries(directory.FullName).Any();
        }
        public static string FullString(this DirectoryInfo directory) {
            return $"\"{directory}\"{directory.StatusString()}";
        }
        public static string StatusString(this DirectoryInfo directory, bool existsInfo = false) {
            if (directory is null) return " (is null ❌)";
            if (File.Exists(directory.FullName)) return " (is file ❌)";
            if (!directory.Exists) return " (does not exist ❌)";
            if (directory.IsEmpty()) return " (is empty ⚠️)";
            return existsInfo ? " (exists ✅)" : string.Empty;
        }
        public static void Copy(this DirectoryInfo source, DirectoryInfo target, bool overwrite = false) {
            Directory.CreateDirectory(target.FullName);
            foreach (FileInfo fi in source.GetFiles())
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), overwrite);
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                Copy(diSourceSubDir, target.CreateSubdirectory(diSourceSubDir.Name));
        }
        public static bool Backup(this DirectoryInfo directory, bool overwrite = false) {
            if (!directory.Exists) return false;
            var backupDirPath = directory.FullName + ".bak";
            if (Directory.Exists(backupDirPath) && !overwrite) return false;
            Directory.CreateDirectory(backupDirPath);
            foreach (FileInfo fi in directory.GetFiles()) fi.CopyTo(Path.Combine(backupDirPath, fi.Name), overwrite);
            foreach (DirectoryInfo diSourceSubDir in directory.GetDirectories()) {
                diSourceSubDir.Copy(Directory.CreateDirectory(Path.Combine(backupDirPath, diSourceSubDir.Name)), overwrite);
            }
            return true;
        }
        public static void OpenInExplorer(this DirectoryInfo dir) {
            if (!dir.Exists) dir.Create();
            Process_Utilities.StartProcess("explorer.exe", args: dir.FullName);
        }
        //public static void ShowInExplorer(this DirectoryInfo dir) => Process_Utilities.StartProcess("explorer.exe", args: dir.Parent.FullName);
        #endregion
        #region FileInfo
        public static FileInfo CombineFile(this DirectoryInfo dir, params string[] paths) {
            var final = dir.FullName;
            foreach (var path in paths) {
                final = Path.Combine(final, path);
            }
            return new FileInfo(final);
        }
        public static FileInfo Combine(this FileInfo file, params string[] paths) {
            var final = file.DirectoryName;
            foreach (var path in paths) {
                final = Path.Combine(final, path);
            }
            return new FileInfo(final);
        }
        public static FileVersionInfo GetVersionInfo(this FileInfo file) => FileVersionInfo.GetVersionInfo(file.FullName);
        public static string FileNameWithoutExtension(this FileInfo file) {
            return Path.GetFileNameWithoutExtension(file.Name);
        }
        /*public static string Extension(this FileInfo file) {
            return Path.GetExtension(file.Name);
        }*/
        public static string FullString(this FileInfo file) {
            return $"\"{file}\"{file.StatusString()}";
        }
        public static string StatusString(this FileInfo file, bool existsInfo = false) {
            if (file is null) return "(is null ❌)";
            if (Directory.Exists(file.FullName)) return "(is directory ❌)";
            if (!file.Exists) return "(does not exist ❌)";
            if (file.Length < 1) return "(is empty ⚠️)";
            return existsInfo ? "(exists ✅)" : string.Empty;
        }
        public static void AppendLine(this FileInfo file, string line) {
            try {
                if (!file.Exists) file.Create();
                File.AppendAllLines(file.FullName, new string[] { line });
            } catch { }
        }
        public static void WriteAllText(this FileInfo file, string text) => File.WriteAllText(file.FullName, text);
        public static string ReadAllText(this FileInfo file) => File.ReadAllText(file.FullName);
        public static List<string> ReadAllLines(this FileInfo file) => File.ReadAllLines(file.FullName).ToList();
        public static bool Backup(this FileInfo file, bool overwrite = false) {
            if (!file.Exists) return false;
            var backupFilePath = file.FullName + ".bak";
            if (File.Exists(backupFilePath) && !overwrite) return false;
            File.Copy(file.FullName, backupFilePath, overwrite);
            return true;
        }
        public static MachineType GetDllMachineType(FileInfo file) {
            // See http://www.microsoft.com/whdc/system/platform/firmware/PECOFF.mspx
            // Offset to PE header is always at 0x3C.
            // The PE header starts with "PE\0\0" =  0x50 0x45 0x00 0x00,
            // followed by a 2-byte machine type field (see the document above for the enum).
            //
            using (var fs = file.OpenRead())
            using (var br = new BinaryReader(fs)) {
                fs.Seek(0x3c, SeekOrigin.Begin);
                Int32 peOffset = br.ReadInt32();
                fs.Seek(peOffset, SeekOrigin.Begin);
                UInt32 peHead = br.ReadUInt32();
                if (peHead != 0x00004550) // "PE\0\0", little-endian
                    return MachineType.IMAGE_FILE_MACHINE_UNKNOWN;
                //throw new Exception("Can't find PE header");
                return (MachineType)br.ReadUInt16();
            }
        }
        public static bool Is64Bit(this FileInfo file, bool _default = false) {
            //try {
            //var peFile = new PeFile(fileInfo.FullName);
            //return peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Import].VirtualAddress == 0;
            //} catch (Exception ex) {
            //IO_Utilities.ErrorLogWriter(ex.Message);
            var matches64 = Regex.IsMatch(file.FileNameWithoutExtension(), @"(x64|x86_64|64bit|64)$", RegexOptions.IgnoreCase);
            var matches32 = Regex.IsMatch(file.FileNameWithoutExtension(), @"(x86|32bit|86|32)$", RegexOptions.IgnoreCase);
            return matches64 || (_default && !matches32);
            //}
        }
        public static bool Is64BitDll(this FileInfo file, bool _default = false) {
            switch (GetDllMachineType(file)) {
                case MachineType.IMAGE_FILE_MACHINE_AMD64:
                case MachineType.IMAGE_FILE_MACHINE_IA64:
                    return true;
                case MachineType.IMAGE_FILE_MACHINE_I386:
                    return false;
                default:
                    return _default;
            }
        }

        internal static bool IsDisabled(this FileInfo file) => file != null && file.Extension.ToLowerInvariant() == IO_Utilities.DisabledFileExtension;
        internal static void Toggle(this FileInfo file, bool? enable = null) {
            var disabled = file.IsDisabled();
            if (disabled && (!enable.HasValue || enable.Value)) file.Enable();
            else if (!disabled && (!enable.HasValue || !enable.Value)) file.Disable();
        }
        internal static void Enable(this FileInfo file) {
            if (file.IsDisabled())
                file.MoveTo(file.FullName.Replace(IO_Utilities.DisabledFileExtension, string.Empty));
        }
        internal static void Disable(this FileInfo file) {
            if (!file.IsDisabled())
                file.MoveTo(file.FullName + ".disabled");
        }
        internal static void CopyTo(this FileInfo source, FileInfo target) => source.CopyTo(target.FullName);
        public static void OpenWithDefaultApp(this FileInfo file) => System.Diagnostics.Process.Start(file.FullName);
        public static void OpenInExplorer(this FileInfo file) {
            if (!file.Directory.Exists) file.Directory.Create();
            Process_Utilities.StartProcess("explorer.exe", args: file.ToString());
        }
        public static void ShowInExplorer(this FileInfo file) {
            if (!file.Directory.Exists) file.Directory.Create();
            Process_Utilities.StartProcess("explorer.exe", args: file.Directory.ToString());
        }

        #endregion
    }
    class IO_Utilities {
        internal const string DisabledFileExtension = ".disabled";
        internal static void DirectoryCopy(string _sourceDirName, string _destDirName, bool _copySubDirs) {
            DirectoryCopy(_sourceDirName, _destDirName, _copySubDirs, null);
        }

        internal static void DirectoryCopy(string _sourceDirName, string _destDirName, bool _copySubDirs, string[] _fileList) {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dirInfo = new DirectoryInfo(_sourceDirName);

            if (!dirInfo.Exists) {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + _sourceDirName);
            }
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(_destDirName)) {
                Directory.CreateDirectory(_destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dirInfo.GetFiles();
            string tempPath = "";

            foreach (FileInfo file in files) {
                if (_fileList != null)
                    if (!_fileList.Contains(file.Name))
                        continue;

                tempPath = Path.Combine(_destDirName, file.Name);

                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (_copySubDirs) {
                DirectoryInfo[] dirInfoArray = dirInfo.GetDirectories();

                foreach (DirectoryInfo subdir in dirInfoArray) {
                    tempPath = Path.Combine(_destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, _copySubDirs, _fileList);
                }
            }
        }

        internal static void LogWriter(string _error) {
            try {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\log.log", true)) {
                    writer.WriteLine(DateTime.Now + " " + _error);
                }
            } catch { }
        }

        internal static void ErrorLogWriter(string _error) {
            try {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\errorlog.log", true)) {
                    writer.WriteLine(DateTime.Now + " | " + AssemblyData.AssemblyProduct + " - " + AssemblyData.AssemblyVersion + " | " +
                                    Globals.SelectedProfileName + " [ " + Globals.SelectedProfile + " ] >> " +
                                    Globals.SelectedSaveName + " [ " + Globals.SelectedSave + " ] ");
                    writer.WriteLine(_error + Environment.NewLine);
                }
            } catch { }
        }

        internal static void WritePreviewTOBJ(string _path, string _name, string _pathToTGA) {
            WritePreviewTOBJ(_path + "\\" + _name + ".tobj", _pathToTGA);
        }

        internal static void WritePreviewTOBJ(string _pathToTOBJ, string _pathToTGA) {
            using (BinaryWriter binWriter = new BinaryWriter(File.Open(_pathToTOBJ, FileMode.Create))) {
                byte[] preview_tobj = new byte[] { 1, 10, 177, 112, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 3, 3, 2, 0, 2, 2, 2, 1, 0, 0, 0, 1, 0, 0 };

                binWriter.Write(preview_tobj);

                byte filePathLength = (byte)_pathToTGA.Length;
                binWriter.Write(filePathLength);

                binWriter.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0 });

                binWriter.Write(Encoding.UTF8.GetBytes(_pathToTGA));
            }
        }
    }
}
