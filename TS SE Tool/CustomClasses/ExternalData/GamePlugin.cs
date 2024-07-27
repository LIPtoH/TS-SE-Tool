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
    public class GamePlugin {
        public bool Enabled {
            get => !File32bit.IsDisabled() && !File64bit.IsDisabled();
            set {
                if (x86) File32bit.Toggle(value);
                if (x64) File64bit.Toggle(value);
            }
        }
        public string Name { get => File32bit?.FileNameWithoutExtension().RemoveAll("_win32", "_x86").Trim() ?? File64bit?.FileNameWithoutExtension().RemoveAll("_win64", "_x64").Trim(); }
        public DateTime? InstallDate {
            get {
                if (x86) return File32bit.LastWriteTime;
                else if (x64) return File64bit.LastWriteTime;
                return null;
            }
        }

        [JsonIgnore]
        public bool x86 { get => File32bit != null && File32bit.Exists; }
        [JsonIgnore]
        public bool x64 { get => File64bit != null && File64bit.Exists; }

        [JsonIgnore]
        [Browsable(false)]
        public FileInfo File32bit { get; set; }
        [Browsable(false)]
        public string Path32bit { get => File32bit?.FullName; }

        [JsonIgnore]
        [Browsable(false)]
        public FileInfo File64bit { get; set; }
        [Browsable(false)]
        public string Path64bit { get => File64bit?.FullName; }

        [JsonIgnore]
        [Browsable(false)]
        public List<FileInfo> Files {
            get {
                var list = new List<FileInfo>();
                if (x86) list.Add(File32bit);
                if (x64) list.Add(File64bit);
                return list;
            }
        }

        public bool Delete() {
            try {
                if (x86) File32bit.Delete();
            } catch (Exception ex) {
                IO_Utilities.LogWriter($"Failed to delete {File32bit.FullString()}: {ex}");
                return false;
            }
            try {
                if (x64) File64bit.Delete();
            } catch (Exception ex) {
                IO_Utilities.LogWriter($"Failed to delete {File64bit.FullString()}: {ex}");
                return false;
            }
            return true;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            GamePlugin other = (GamePlugin)obj;
            return Path32bit == other.Path32bit && Path64bit == other.Path64bit;
        }

        public override int GetHashCode() {
            // Combine hashes of Name, Path32bit, and Path64bit
            unchecked {
                int hash = 17;
                //hash = hash * 23 + (Name ?? "").GetHashCode();
                hash = hash * 23 + (Path32bit ?? "").GetHashCode();
                hash = hash * 23 + (Path64bit ?? "").GetHashCode();
                return hash;
            }
        }
    }
    public static class GamePluginExtensions {
        public static IEnumerable<GamePlugin> AddSafe(this IEnumerable<GamePlugin> plugins, GamePlugin pluginToAdd) {
            List<GamePlugin> pluginsList = new List<GamePlugin>(plugins);
            if (!pluginsList.Any(plugin => plugin.Path32bit == pluginToAdd.Path32bit || plugin.Path64bit == pluginToAdd.Path64bit)) {
                pluginsList.Add(pluginToAdd);
            }
            return pluginsList;
        }
        public static GamePlugin? GetDirectMatch(this IEnumerable<GamePlugin> plugins, FileInfo file) {
            return plugins.FirstOrDefault(f => f.Name == file.FileNameWithoutExtension().RemoveAll("_win32", "_x86", "_win64", "_x64").Trim());
        }
        public static GamePlugin? GetFuzzyMatch(this IEnumerable<GamePlugin> plugins, FileInfo file, int ratio = 70) {
            return plugins.FirstOrDefault(f => Fuzz.Ratio(file.Name, f.Name) > 70);
        }
    }
}
