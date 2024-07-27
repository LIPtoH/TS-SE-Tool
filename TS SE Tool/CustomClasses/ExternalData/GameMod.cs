/*
   Copyright 2016-2020 LIPtoH <liptoh.codebase@gmail.com>

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
using System.IO;
using TS_SE_Tool.Utilities;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using FuzzySharp;

namespace TS_SE_Tool {
    public class GameMod {
        public bool Enabled {
            get => !File.IsDisabled();
            set {
                if (hasFile) File.Toggle(value);
            }
        }

        [Browsable(false)]
        public string Id { get; set; }
        public string Name { get; set; } // { get => File?.FileNameWithoutExtension().Trim(); }
        public DateTime? InstallDate {
            get {
                if (hasFile) return File.LastWriteTime;
                return null;
            }
        }

        [JsonIgnore]
        public bool hasFile { get => File != null && File.Exists; }

        [JsonIgnore]
        [Browsable(false)]
        public FileInfo File { get; set; }
        [Browsable(false)]
        public string FilePath { get => File?.FullName; }

        public bool Delete() {
            try {
                if (hasFile) File.Delete();
            } catch (Exception ex) {
                IO_Utilities.LogWriter($"Failed to delete {File.FullString()}: {ex}");
                return false;
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = obj as GameMod;
            return FilePath == other.FilePath;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 23 + (FilePath ?? "").GetHashCode();
                return hash;
            }
        }
    }
    public static class GameModExtensions {
        public static IEnumerable<GameMod> AddSafe(this IEnumerable<GameMod> mods, GameMod pluginToAdd) {
            List<GameMod> modList = new List<GameMod>(mods);
            if (!modList.Any(plugin => plugin.FilePath == pluginToAdd.FilePath)) {
                modList.Add(pluginToAdd);
            }
            return modList;
        }
        public static GameMod? GetDirectMatch(this IEnumerable<GameMod> mods, FileInfo file) {
            return mods.FirstOrDefault(f => f.Name == file.FileNameWithoutExtension().Trim());
        }
        public static GameMod? GetFuzzyMatch(this IEnumerable<GameMod> mods, FileInfo file, int ratio = 70) {
            return mods.FirstOrDefault(f => Fuzz.Ratio(file.Name, f.Name) > 70);
        }
    }
}
