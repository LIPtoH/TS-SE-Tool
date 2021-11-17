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
using System;
using System.Windows.Forms;
using Microsoft.Win32;
using TS_SE_Tool.Utilities;

namespace TS_SE_Tool
{
    public class DetectEnviroment
    {
        internal static void Get45PlusFromRegistry()
        {
            try
            {
                const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

                using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    {
                        IO_Utilities.LogWriter($"Installed .NET Framework {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))} version");
                        //Console.WriteLine($".NET Framework Version: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}");
                    }
                    else
                    {
                        IO_Utilities.LogWriter(".NET Framework Version 4.5 or later is not detected.");
                        //Console.WriteLine(".NET Framework Version 4.5 or later is not detected.");
                    }
                }

                // Checking the version using >= enables forward compatibility.
                string CheckFor45PlusVersion(int releaseKey)
                {
                    if (releaseKey >= 528040)
                        return "4.8 or later";
                    if (releaseKey >= 461808)
                        return "4.7.2";
                    if (releaseKey >= 461308)
                        return "4.7.1";
                    if (releaseKey >= 460798)
                        return "4.7";
                    if (releaseKey >= 394802)
                        return "4.6.2";
                    if (releaseKey >= 394254)
                        return "4.6.1";
                    if (releaseKey >= 393295)
                        return "4.6";
                    if (releaseKey >= 379893)
                        return "4.5.2";
                    if (releaseKey >= 378675)
                        return "4.5.1";
                    if (releaseKey >= 378389)
                        return "4.5";
                    // This code should never execute. A non-null release key should mean
                    // that 4.5 or later is installed.
                    return "No 4.5 or later version detected";
                }
            }
            catch
            {
                IO_Utilities.LogWriter(".NET Framework Version detection FAILED");
            }
        }

        internal static void DetectOS()
        {
            try
            {
                string RunningOS = "", Details = "";

                PlatformID pID = Environment.OSVersion.Platform;

                switch (pID)
                {
                    case PlatformID.Win32NT:
                        {
                            RunningOS = "Windows";
                            Details = "TESTED";

                            if (Registry.LocalMachine.OpenSubKey("Software\\Wine") != null)
                            {
                                RunningOS = "Windows with Wine";
                                Details = "NOT TESTED";
                            }
                            break;
                        }
                    case PlatformID.Unix:
                        {
                            RunningOS = "Unix";
                            Details = "NOT TESTED";
                            break;
                        }
                    case PlatformID.MacOSX:
                        {
                            RunningOS = "MacOSX";
                            Details = "NOT TESTED";
                            break;
                        }
                    default:
                        IO_Utilities.LogWriter("OS detection FAILED");
                        break;
                }

                IO_Utilities.LogWriter("Enviroment - " + RunningOS + " (" + Details + ")");
            }
            catch
            {
                IO_Utilities.LogWriter("OS detection FAILED");
            }
        }
    }
}
