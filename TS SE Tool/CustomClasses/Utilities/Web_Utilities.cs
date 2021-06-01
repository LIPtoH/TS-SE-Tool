using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace TS_SE_Tool.Utilities
{
    public class Web_Utilities
    {
        public static Web_Utilities External = new Web_Utilities();

        internal string linkCheckVersion = "https://liptoh.now.im/TS-SET-CheckVersion";
        internal string linkDownloadVersion = "https://liptoh.now.im/TS-SET-Download";
        internal string linkYoutubeTutorial = "https://liptoh.now.im/TS-SET-Tutorial";

        internal string linkHelpDeveloper = "https://www.paypal.me/LIPtoHCode";
        internal string linkMailDeveloper = "liptoh.codebase@gmail.com";

        internal string linkSCSforum = "https://forum.scssoft.com/viewtopic.php?f=34&t=266092";
        internal string linTMPforum = "https://forum.truckersmp.com/index.php?/topic/79561-ts-saveeditor-tool";
        internal string linGithub = "https://github.com/LIPtoH/TS-SE-Tool";

        internal string linGithubReleases = "https://github.com/LIPtoH/TS-SE-Tool/releases";
        internal string linGithubReleasesLatest = "https://github.com/LIPtoH/TS-SE-Tool/releases/latest";

        private static System.Timers.Timer aTimer;
        private static byte atimerCounter = 0;
        private static string aTimerControlText = "";

        //Check for new version
        internal string[] CheckNewVersionAvailability(Control _control)
        {
            aTimerControlText = _control.Text;
            SetTimer(_control);

            string[] NewVersion = { "", "" };
            string newversionData = GetLatestVersionData(linkCheckVersion);

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

        private string GetLatestVersionData(string url)
        {
            string result = null;

            using (WebClientWithTO client = new WebClientWithTO())
            {
                try
                {
                    client.Timeout = 5;
                    result = client.DownloadString(url);
                }
                catch { }

                client.Dispose();
            }

            return result;
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

        private class WebClientWithTO : WebClient
        {
            public UInt16 Timeout { get; set; }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout * 1000;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;

                return lWebRequest;
            }
        }
    }
}
