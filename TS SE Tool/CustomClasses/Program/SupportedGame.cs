using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool.CustomClasses.Program {
    public class SupportedGame {
        public long SteamAppId { get; internal set; }
        public string Type { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string ExecutableName { get; internal set; }

        public Version MinSupportedGameVersion { get; internal set; } = new();
        public Version MaxSupportedGameVersion { get; internal set; } = new();
        [JsonIgnore]
        [Browsable(false)]
        public string SupportedGameVersionString { get => $"{MinSupportedGameVersion}-{MaxSupportedGameVersion}"; }
        [Browsable(false)]
        public List<long> SupportedSaveFileVersions { get; internal set; } = new();
        [JsonIgnore]
        [Browsable(false)]
        public string SupportedSaveFileVersionString { get => SupportedSaveFileVersions.Count < 1 ? string.Empty : string.Join(", ", SupportedSaveFileVersions); }
        //[JsonIgnore]
        //[Browsable(false)]
        //public string SupportedGameVersionsString { get => string.Join(", ", SupportedGameVersions); }
        //[JsonIgnore]
        //[Browsable(false)]
        //public string SupportedSaveFileVersionsString { get => string.Join(", ", SupportedSaveFileVersions.Select(v => v.ToString())); }

        public bool Installed {
            get {
                try { var _ = Globals.SteamGameLocator.getGameInfoByFolder(Name).steamGameLocation; return true; } catch { return false; }
            }
        }
        public DirectoryInfo SteamUserDataDir { get => Globals.GetLatestSteamUserDataDir().Combine(SteamAppId.ToString()); }
        public DirectoryInfo SteamRemoteDir { get => SteamUserDataDir.Combine("remote"); }

        public DirectoryInfo? GameDir { get => Installed ? new DirectoryInfo(Globals.SteamGameLocator.getGameInfoByFolder(Name).steamGameLocation) : null; }
        public FileInfo? Executable { get => Installed ? GameDir.CombineFile("bin", "win_x64", ExecutableName) : null; }
        public DateTime? LastUpdated { get => Installed ? Executable.LastWriteTime : null; }
        public Version? Version { get => Installed ? new Version(Executable.GetVersionInfo().ProductVersion.TrimEnd('0')) : null; }
        public bool? IsSupported { get => Installed ? (Version > MinSupportedGameVersion && Version < MaxSupportedGameVersion) : null; }
        public List<DirectoryInfo> PluginDirs { get => new() { GameDir.Combine("bin", "win_x64", "plugins"), GameDir.Combine("bin", "win_x86", "plugins") }; }

        public DirectoryInfo DocumentsDir { get => Globals.DocumentsDir.Combine(Name); }
        public DirectoryInfo ModsDir { get => DocumentsDir.Combine("mod"); }

        public List<Process> Processes { get => Installed ? Process.GetProcessesByName(Executable.Name).ToList() : new(); }
        public bool IsRunning { get => Processes.Count > 0; }

        public int LicensePlateWidth { get; internal set; }
        public List<int> PlayerLevelUps { get; internal set; } = new();
        public List<Currency> Currencies { get; internal set; } = new();

        //public SupportedGame(long steamAppId, string type, string name, string description, string supportedGameVersions, List<long> supportedSaveFileVersions) =>
        //    new SupportedGame(steamAppId, type, name, description, supportedGameVersions.ParseVersions(), supportedSaveFileVersions);
        //public SupportedGame(long steamAppId, string type, string name, string description, List<Version> supportedGameVersions, List<long> supportedSaveFileVersions) {
        //    SteamAppId = steamAppId;
        //    Type = type;
        //    Name = name;
        //    Description = description;
        //    SupportedGameVersions = supportedGameVersions;
        //    SupportedSaveFileVersions = supportedSaveFileVersions;
        //}
    }
    public static class SupportedGameExtensions {
        public static SupportedGame Get(this IEnumerable<SupportedGame> games, string type) => games.FirstOrDefault(g => g.Type == type);
        public static Currency Get(this IEnumerable<Currency> currencies, string name) => currencies.FirstOrDefault(g => g.Name == name);
    }
    public class Currency {
        public string Name { get; set; }
        public double ConversionRate { get; set; }
        public List<string> FormatSymbols { get; set; }

        public Currency(string name, double conversionRate, List<string> formatSymbols) {
            Name = name;
            ConversionRate = Math.Round(conversionRate, 2);
            FormatSymbols = formatSymbols ?? throw new ArgumentNullException(nameof(formatSymbols));
        }
    }

}
