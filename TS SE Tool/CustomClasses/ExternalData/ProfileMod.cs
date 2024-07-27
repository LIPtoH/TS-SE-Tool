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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using FuzzySharp;
using System.Windows.Forms;

namespace TS_SE_Tool {
    public class ProfileMod {
        [Browsable(false)]
        public int LoadOrder { get; set; }
        [AutoSizeColumnMode(DataGridViewAutoSizeColumnMode.ColumnHeader)]
        [DisplayName("#")]
        [JsonIgnore]
        public int LoadOrderFriendly { get => LoadOrder + 1; }
        [Browsable(false)]
        public string Id { get; set; }
        public string Name { get; set; } // { get => File?.FileNameWithoutExtension().Trim(); }

        [JsonIgnore]
        //[Browsable(false)]
        [AutoSizeColumnMode(DataGridViewAutoSizeColumnMode.ColumnHeader)]
        public bool Exists { get => GameMod != null && GameMod.File != null && GameMod.File.Exists; }

        [Browsable(false)]
        public GameMod? GameMod { get; set; }

        public ProfileMod(string profileEntry, int loadOrder) {
            var split = profileEntry.Trim('\"').Split('|');
            Id = split.First();
            Name = split.Last();
            LoadOrder = loadOrder;
        }

        public bool Delete() {
            return GameMod.Delete();
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = obj as ProfileMod;
            return Id == other.Id;
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 23 + (Id ?? "").GetHashCode();
                return hash;
            }
        }
    }
    public static class ProfileModExtensions {
        public static IEnumerable<ProfileMod> AddSafe(this IEnumerable<ProfileMod> mods, ProfileMod modToAdd) {
            List<ProfileMod> modList = new List<ProfileMod>(mods);
            if (!modList.Any(mod => mod.Id == modToAdd.Id)) {
                modList.Add(modToAdd);
            }
            return modList;
        }
        public static ProfileMod? GetDirectMatch(this IEnumerable<ProfileMod> mods, FileInfo file) {
            return mods.FirstOrDefault(f => f.Name == file.FileNameWithoutExtension().Trim());
        }
        public static ProfileMod? GetFuzzyMatch(this IEnumerable<ProfileMod> mods, FileInfo file, int ratio = 70) {
            return mods.FirstOrDefault(f => Fuzz.Ratio(file.Name, f.Name) > 70);
        }
    }
}
