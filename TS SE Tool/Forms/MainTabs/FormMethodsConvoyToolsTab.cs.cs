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
using System.Collections.Generic;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public partial class FormMain
    {
        //Convoy tools tab
        private void buttonGPSCurrentPositionCopy_Click(object sender, EventArgs e)
        {
            string tempString = "GPS_TruckPosition\r\n";

            tempString += PlayerDataData.UserCompanyAssignedTruckPlacement;
            string asd = BitConverter.ToString(Utilities.ZipDataUtilitiescs.zipText(tempString)).Replace("-", "");
            Clipboard.SetText(asd);
            MessageBox.Show("Truck GPS position has been copied.");
        }

        private void buttonGPSCurrentPositionPaste_Click(object sender, EventArgs e)
        {
            //UserCompanyAssignedTruckPlacement
            try
            {
                string inputData = Utilities.ZipDataUtilitiescs.unzipText(Clipboard.GetText());
                string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (Lines[0] == "GPS_TruckPosition")
                {
                    List<string> tempstr = new List<string>();
                    for (int i = 1; i < Lines.Length; i++)
                    {
                        tempstr.Add(Lines[i]);
                    }

                    PlayerDataData.UserCompanyAssignedTruckPlacement = tempstr[0];
                    //PlayerProfileData.UserCompanyAssignedTrailerPlacement = "(0, 0, 0) (1; 0, 0, 0)";

                    MessageBox.Show("Truck GPS position has been inserted.");
                    UserCompanyAssignedTruckPlacementEdited = true;
                }
                else
                    MessageBox.Show("Wrong data. Expected Truck GPS data but\r\n" + Lines[0] + "\r\nwas found.");
            }
            catch
            {
                MessageBox.Show("Something gone wrong with decoding.");
            }
        }

        private void buttonGPSStoredGPSPathCopy_Click(object sender, EventArgs e)
        {
            string tempData = "GPS_Path\r\n";
            //GPS Behind
            if (GPSbehind.Count > 0)
            {
                tempData += "GPSbehind\r\n";
                foreach (KeyValuePair<string, List<string>> temp in GPSbehind)
                {
                    tempData += "waypoint\r\n";
                    foreach (string tempLines in temp.Value)
                    {
                        tempData += tempLines + "\r\n";
                    }
                }
            }

            //GPS Ahead
            if (GPSahead.Count > 0)
            {
                tempData += "GPSahead\r\n";
                foreach (KeyValuePair<string, List<string>> temp in GPSahead)
                {
                    tempData += "waypoint\r\n";
                    foreach (string tempLines in temp.Value)
                    {
                        tempData += tempLines + "\r\n";
                    }
                }
            }

            //GPS Avoid
            if (GPSAvoid != null)
            {
                tempData += "GPSavoid\r\n";
                foreach (KeyValuePair<string, List<string>> temp in GPSAvoid)
                {
                    tempData += "waypoint\r\n";
                    foreach (string tempLines in temp.Value)
                    {
                        tempData += tempLines + "\r\n";
                    }
                }
            }

            //MessageBox.Show(tempPaint);
            string asd = BitConverter.ToString(Utilities.ZipDataUtilitiescs.zipText(tempData)).Replace("-", "");
            Clipboard.SetText(asd);
            MessageBox.Show("GPS Path data has been copied.");
        }

        private void buttonGPSStoredGPSPathPaste_Click(object sender, EventArgs e)
        {
            try
            {
                string inputData = Utilities.ZipDataUtilitiescs.unzipText(Clipboard.GetText());
                string[] Lines = inputData.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (Lines[0] == "GPS_Path")
                {
                    Dictionary<int, List<string>> tempGPSbehind, tempGPSahead, tempGPSavoid;

                    tempGPSbehind = new Dictionary<int, List<string>>();
                    tempGPSahead = new Dictionary<int, List<string>>();
                    tempGPSavoid = new Dictionary<int, List<string>>();

                    bool tagGPSbehind = false, tagGPSahead = false, tagGPSavoid = false;

                    for (int i = 1; i < Lines.Length; i++)
                    {
                        //GPSbehind
                        if (Lines[i].StartsWith("GPSbehind"))
                        {
                            tagGPSbehind = true;
                            continue;
                        }

                        if (tagGPSbehind)
                        {
                            int wp = 0;
                            do
                            {
                                if (Lines[i].StartsWith("waypoint"))
                                {
                                    i++;
                                    List<string> tmpList = new List<string>();

                                    while (!Lines[i].StartsWith("waypoint") && !Lines[i].StartsWith("GPS") && Lines[i] != "" && i < Lines.Length)
                                    {
                                        tmpList.Add(Lines[i]);
                                        i++;
                                    }

                                    tempGPSbehind.Add(wp, tmpList);
                                    wp++;
                                }
                            }
                            while (!Lines[i].StartsWith("GPS") && Lines[i] != "" && i < Lines.Length);

                            tagGPSbehind = false;
                        }

                        //GPSahead
                        if (Lines[i].StartsWith("GPSahead"))
                        {
                            tagGPSahead = true;
                            continue;
                        }

                        if (tagGPSahead)
                        {
                            int wp = 0;
                            do
                            {
                                if (Lines[i].StartsWith("waypoint"))
                                {
                                    i++;
                                    List<string> tmpList = new List<string>();

                                    while (!Lines[i].StartsWith("waypoint") && !Lines[i].StartsWith("GPS") && Lines[i] != "" && i < Lines.Length)
                                    {
                                        tmpList.Add(Lines[i]);
                                        i++;
                                    }

                                    tempGPSahead.Add(wp, tmpList);
                                    wp++;
                                }
                            }
                            while (!Lines[i].StartsWith("GPS") && Lines[i] != "" && i < Lines.Length);

                            tagGPSahead = false;
                        }

                        //GPS Avoid
                        if (Lines[i].StartsWith("GPSavoid"))
                        {
                            tagGPSavoid = true;
                            continue;
                        }

                        if (tagGPSavoid)
                        {
                            int wp = 0;
                            do
                            {
                                if (Lines[i].StartsWith("waypoint"))
                                {
                                    i++;
                                    List<string> tmpList = new List<string>();

                                    while (!Lines[i].StartsWith("waypoint") && !Lines[i].StartsWith("GPS") && Lines[i] != "" && i < Lines.Length)
                                    {
                                        tmpList.Add(Lines[i]);
                                        i++;
                                    }

                                    tempGPSavoid.Add(wp, tmpList);
                                    wp++;
                                }
                            }
                            while (!Lines[i].StartsWith("GPS") && Lines[i] != "" && i < Lines.Length);

                            tagGPSavoid = false;
                        }
                    }

                    //GPSbehind = tempGPSbehind
                    if (tempGPSbehind.Count > 0)
                    {
                        GPSbehind.Clear();
                        foreach (KeyValuePair<int, List<string>> temp in tempGPSbehind)
                        {
                            GPSbehind.Add(GetSpareNameless(), temp.Value);
                        }
                    }

                    //GPSahead = tempGPSahead
                    if (tempGPSahead.Count > 0)
                    {
                        GPSahead.Clear();
                        foreach (KeyValuePair<int, List<string>> temp in tempGPSahead)
                        {
                            GPSahead.Add(GetSpareNameless(), temp.Value);
                        }
                    }

                    //GPSavoid = tempGPSavoid
                    if (tempGPSavoid.Count > 0)
                    {
                        GPSAvoid.Clear();
                        foreach (KeyValuePair<int, List<string>> temp in tempGPSavoid)
                        {
                            GPSAvoid.Add(GetSpareNameless(), temp.Value);
                        }
                    }

                    MessageBox.Show("GPS Path data has been inserted.");
                }
                else
                    MessageBox.Show("Wrong data. Expected GPS Path data but\r\n" + Lines[0] + "\r\nwas found.");
            }
            catch
            {
                MessageBox.Show("Something gone wrong with decoding.");
            }
        }

        private void buttonConvoyToolsGPSTruckPositionMultySaveCopy_Click(object sender, EventArgs e)
        {
            FormConvoyControlPositions FormWindow = new FormConvoyControlPositions(true);
            FormWindow.ShowDialog();
        }

        private void buttonConvoyToolsGPSTruckPositionMultySavePaste_Click(object sender, EventArgs e)
        {
            FormConvoyControlPositions FormWindow = new FormConvoyControlPositions(false);
            FormWindow.ShowDialog();
        }
        //end Convoy Tools tab
    }
}