/*
   Added during the 2026 save-format investigation (ATS/ETS2 savefile version 97).

   TS SE Tool does not rewrite the save file it read - it *reconstructs* it from a
   fixed set of hand written fields per block type. Every attribute SCS adds in a
   newer game version is therefore silently dropped on write, and every attribute
   SCS removes keeps being written out.

   For savefile v97 that meant, among others:
       lost   : player.my_vehicles / assigned_vehicles / cars / buses,
                economy.screen_visit_list, economy.total_*_by_mode,
                vehicle.trip_recuperation*, vehicle.sliding_axle_offset,
                vehicle_addon_accessory.paint_color
       invented: company.state_change_time, player.sleeping_count,
                economy.stored_tutorial_state

   This class merges the reconstructed text back onto the block bodies as they were
   read from the save, so that:
     * every original line survives, in its original position;
     * values the tool actually edited win over the original ones;
     * array entries the tool added are kept;
     * attributes the tool invents which this save version does not use are dropped.

   Blocks the tool created from scratch have no original and pass through untouched.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.Save.Items
{
    internal static class OriginalBlockMerge
    {
        //"tag : name {"
        private static readonly Regex BlockHeader =
            new Regex(@"^(?<tag>[A-Za-z_][A-Za-z_0-9]*)\s*:\s*(?<name>\S+)\s*\{\s*$", RegexOptions.Compiled);

        //trailing array index, e.g. "companies[17]" -> base "companies"
        private static readonly Regex ArrayIndex =
            new Regex(@"\[\d+\]$", RegexOptions.Compiled);

        /// <summary>
        /// Tag of an attribute line, or null when the line carries no attribute.
        /// </summary>
        private static string TagOf(string _line)
        {
            int colon = _line.IndexOf(':');

            if (colon <= 0)
                return null;

            string tag = _line.Substring(0, colon).Trim();

            return tag.Length == 0 ? null : tag;
        }

        private static string BaseTagOf(string _tag)
        {
            return ArrayIndex.Replace(_tag, "");
        }

        /// <summary>
        /// Rewrites <paramref name="_generated"/> so that each block keeps everything the
        /// original block body had. <paramref name="_originalBodies"/> maps a block's
        /// nameless id to its body lines as read from the save (header and closing brace
        /// excluded).
        /// </summary>
        internal static string Apply(string _generated, Dictionary<string, List<string>> _originalBodies)
        {
            if (_originalBodies == null || _originalBodies.Count == 0 || string.IsNullOrEmpty(_generated))
                return _generated;

            string[] lines = _generated.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            StringBuilder output = new StringBuilder();

            List<string> droppedTags = new List<string>();
            int mergedBlocks = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                Match header = BlockHeader.Match(lines[i].Trim());

                if (!header.Success)
                {
                    output.AppendLine(lines[i]);
                    continue;
                }

                //collect the generated body
                List<string> generatedBody = new List<string>();
                int j = i + 1;

                while (j < lines.Length && lines[j].Trim() != "}")
                {
                    generatedBody.Add(lines[j]);
                    j++;
                }

                bool closed = j < lines.Length;

                string name = header.Groups["name"].Value;

                output.AppendLine(lines[i]);

                List<string> originalBody;

                if (_originalBodies.TryGetValue(name, out originalBody))
                {
                    mergedBlocks++;

                    foreach (string line in MergeBody(header.Groups["tag"].Value, originalBody, generatedBody, droppedTags))
                        output.AppendLine(line);
                }
                else
                {
                    foreach (string line in generatedBody)
                        output.AppendLine(line);
                }

                if (closed)
                    output.AppendLine(lines[j]);

                i = closed ? j : lines.Length;
            }

            if (droppedTags.Count > 0)
            {
                IO_Utilities.ErrorLogWriter(
                    "Save write | attributes not present in the loaded save were dropped (" + droppedTags.Count + "):" +
                    Environment.NewLine +
                    string.Join(Environment.NewLine, droppedTags.Distinct().OrderBy(x => x)));
            }

            IO_Utilities.LogWriter("Save write | original-content merge applied to " + mergedBlocks + " blocks");

            //Split() above dropped the trailing newline handling; AppendLine already
            //re-added one per line, so trim the extra blank tail the last AppendLine made.
            string result = output.ToString();

            if (result.EndsWith(Environment.NewLine))
                result = result.Substring(0, result.Length - Environment.NewLine.Length);

            return result;
        }

        private static List<string> MergeBody(string _blockTag,
                                              List<string> _originalBody,
                                              List<string> _generatedBody,
                                              List<string> _droppedTags)
        {
            //Generated attributes, first occurrence wins - the writers never emit a tag twice.
            Dictionary<string, string> generated = new Dictionary<string, string>();
            List<string> generatedOrder = new List<string>();

            foreach (string line in _generatedBody)
            {
                string tag = TagOf(line);

                if (tag == null || generated.ContainsKey(tag))
                    continue;

                generated.Add(tag, line);
                generatedOrder.Add(tag);
            }

            HashSet<string> originalTags = new HashSet<string>();
            HashSet<string> originalBaseTags = new HashSet<string>();

            foreach (string line in _originalBody)
            {
                string tag = TagOf(line);

                if (tag == null)
                    continue;

                originalTags.Add(tag);
                originalBaseTags.Add(BaseTagOf(tag));
            }

            List<string> merged = new List<string>();
            HashSet<string> consumed = new HashSet<string>();

            //1. walk the original body, substituting values the tool re-emitted
            foreach (string line in _originalBody)
            {
                string tag = TagOf(line);

                if (tag != null && generated.ContainsKey(tag))
                {
                    merged.Add(generated[tag]);
                    consumed.Add(tag);
                }
                else if (tag != null || line.Trim().Length > 0)
                {
                    //unknown / newly added attribute, or a comment - keep verbatim
                    merged.Add(line);
                }
            }

            //2. anything the tool produced that the original did not have
            foreach (string tag in generatedOrder)
            {
                if (consumed.Contains(tag))
                    continue;

                if (originalBaseTags.Contains(BaseTagOf(tag)))
                {
                    //an array the tool grew (extra job offers, colours, accessories, ...)
                    merged.Add(generated[tag]);
                }
                else
                {
                    //an attribute this savefile version does not use - writing it would
                    //push an unknown attribute into the save
                    _droppedTags.Add(_blockTag + " | " + tag);
                }
            }

            return merged;
        }
    }
}
