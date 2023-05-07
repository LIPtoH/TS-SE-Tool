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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Net.Mime;
using System.Security.Cryptography;
using TS_SE_Tool.Utilities;
using System.Security.Policy;

namespace TS_SE_Tool.Utilities
{
    public class Web_Utilities
    {
        internal static Web_Utilities External = new Web_Utilities();

        internal string linkCheckVersion    = "https://rebrand.ly/TS-SET-CheckVersion";
        internal string linkDownloadVersion = "https://rebrand.ly/TS-SET-Download";
        internal string linkYoutubeTutorial = "https://rebrand.ly/TS-SET-Tutorial";

        internal string linkCheckVersion2 = "https://liptoh.twilightparadox.com/TS-SET-CheckVersion";
        internal string linkDownloadVersion2 = "https://liptoh.twilightparadox.com/TS-SET-Download";

        internal string linkHelpDeveloper = "https://www.paypal.me/LIPtoHCode";
        internal string linkMailDeveloper = "liptoh.codebase@gmail.com";

        internal string linkSCSforum = "https://forum.scssoft.com/viewtopic.php?f=34&t=266092";
        internal string linkTMPforum = "https://forum.truckersmp.com/index.php?/topic/79561-ts-saveeditor-tool";
        internal string linkGithub = "https://github.com/LIPtoH/TS-SE-Tool";

        internal string linkGithubReleases = "https://github.com/LIPtoH/TS-SE-Tool/releases";
        internal string linkGithubReleasesLatest = "https://github.com/LIPtoH/TS-SE-Tool/releases/latest";

        private static System.Timers.Timer aTimer;
        private static byte atimerCounter = 0;
        private static string aTimerControlText = "";

        //Check for new version
        internal bool CheckNewVersionTimeElapsed(DateTime _lastTime)
        {
            TimeSpan timeDifference = DateTime.Now.Subtract(_lastTime);

            if (timeDifference.TotalHours > 24)
                return true;
            else
                return false;
        }

        internal string[] CheckNewVersionAvailability(Control _control)
        {
            aTimerControlText = _control.Text;
            SetTimer(_control);

            string[] NewVersion = { "", "" };
            string newversionData = GetLatestVersionData();

            if (newversionData != null)
            {
                NewVersion = newversionData.Split(new char[] { '\t' });

                if (NewVersion[0] != null && NewVersion[0].Contains("TS.SE.Tool"))                
                    NewVersion[0] = NewVersion[0].Replace("TS.SE.Tool.", "").Replace(".zip", "");

                aTimer.Stop();
                aTimer.Dispose();

                return NewVersion;
            }
            else
            {
                aTimer.Stop();
                aTimer.Dispose();

                return null;
            }
        }

        private string GetLatestVersionData()
        {
            string result = null;

            foreach(string url in new[] { linkCheckVersion, linkCheckVersion2 })
            {
                
                bool available = RemoteFileExists(url);

                if (available)
                {
                    using (WebClientWithTO client = new WebClientWithTO())
                    {
                        client.Timeout_S = 10;

                        try
                        {
                            result = client.DownloadString(url);
                        }
                        catch { }

                        client.Dispose();
                    }
                }
            }

            return result;
        }

        internal bool? CheckNewVersionStatus(string[] _NewVersion)
        {
            bool? betterVersion = false;

            string[] newArr = _NewVersion[0].Split(new char[] { '.' });
            string[] currArr = AssemblyData.AssemblyVersion.Split(new char[] { '.' });

            for (byte i = 0; i < newArr.Length; i++)
            {
                if (byte.Parse(newArr[i]) > byte.Parse(currArr[i]))
                {
                    betterVersion = true;
                    break;
                }
                else if (byte.Parse(newArr[i]) < byte.Parse(currArr[i]))
                {
                    betterVersion = null;
                    break;
                }
            }

            return betterVersion;
        }
        //
        private static void SetTimer(Control _control)
        {
            aTimer = new System.Timers.Timer(200);
            aTimer.Elapsed += (Object source, ElapsedEventArgs e) => OnTimedEvent(source, e, _control);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e, Control _control)
        {
            _control.BeginInvoke((MethodInvoker)delegate ()
            {
                atimerCounter++;

                if (atimerCounter < 5)
                    _control.Text += ".";
                else
                {
                    atimerCounter = 0;
                    _control.Text = aTimerControlText;
                }
            });
        }

        internal class WebClientWithTO : WebClient
        {
            public UInt16 Timeout_S { get; set; } = 60;

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout_S * 1000;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout_S;

                return lWebRequest;
            }
        }

        internal bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                request.Timeout = 5000;
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
    }
}
