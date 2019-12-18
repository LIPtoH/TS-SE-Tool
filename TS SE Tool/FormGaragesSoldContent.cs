/*
   Copyright 2016-2019 LIPtoH <liptoh.codebase@gmail.com>

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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public partial class FormGaragesSoldContent : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public FormGaragesSoldContent()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.MainIco;
            FillTreeView();
        }

        private void FillTreeView()
        {
            treeViewSavedDrivers.Nodes.Clear();
            treeViewSavedTrucks.Nodes.Clear();
            treeViewSortingDrivers.Nodes.Clear();
            treeViewSortingTrucks.Nodes.Clear();


            int SpareDrvSpaces = 0, SpareVhcSpaces = 0, TotalDrv = 0, TotalVhc = 0;
            //Saved Drivers and Trucks
            foreach (Garages tempG in MainForm.GaragesList)
            {
                if(tempG.GarageStatus > 0)
                {
                    int curVeh = 0, curDr = 0;
                    foreach (string temp in tempG.Drivers)
                    {
                        if (temp != null)
                            curDr++;
                    }
                    foreach (string temp in tempG.Vehicles)
                    {
                        if (temp != null)
                            curVeh++;
                    }

                    SpareDrvSpaces = SpareDrvSpaces + tempG.Drivers.Count - curDr;
                    SpareVhcSpaces = SpareVhcSpaces + tempG.Vehicles.Count - curVeh;
                    TotalDrv += curDr;
                    TotalVhc += curVeh;
                    //Drivers tree
                    treeViewSavedDrivers.Nodes.Add(tempG.GarageName, "[ " + curDr + " | " + tempG.Drivers.Count + " ] " + tempG.GarageNameTranslated);
                    foreach (string tempD in tempG.Drivers)
                    {
                        if (tempD != null)
                        {
                            string DriverName = tempD;

                            if (MainForm.PlayerDataV.UserDriver == DriverName)
                            {
                                DriverName = MainForm.FromHexToString(Globals.SelectedProfile);
                            }
                            else
                            {
                                MainForm.DriverNames.TryGetValue(DriverName, out string _resultvalue);

                                if (_resultvalue != null && _resultvalue != "")
                                {
                                    DriverName = _resultvalue.TrimStart(new char[] { '+'});
                                }
                            }

                            treeViewSavedDrivers.Nodes[tempG.GarageName].Nodes.Add(tempD, DriverName);
                        }                            
                    }
                    //Trucks tree
                    treeViewSavedTrucks.Nodes.Add(tempG.GarageName, "[ " + curVeh + " | " + tempG.Vehicles.Count + " ] " + tempG.GarageNameTranslated);
                    foreach (string tempV in tempG.Vehicles)
                    {
                        if (tempV != null)
                        {
                            string TruckName = "";
                            try
                            {   
                                string templine = MainForm.UserTruckDictionary[tempV].Parts.Find(x => x.PartType == "truckbrandname").PartData.Find(xline => xline.StartsWith(" data_path:"));
                                TruckName = templine.Split(new char[] { '"' })[1].Split(new char[] { '/' })[4];
                            }
                            catch { }
                            MainForm.TruckBrandsLngDict.TryGetValue(TruckName, out string trucknamevalue);

                            if (trucknamevalue != null && trucknamevalue != "")
                            {
                                TruckName = trucknamevalue;
                            }

                            treeViewSavedTrucks.Nodes[tempG.GarageName].Nodes.Add(tempV, TruckName);
                        }   
                    }
                }
            }
            //Labels
            labelSavedDrivers.Text = "Drivers [ " + TotalDrv + " | " + (SpareDrvSpaces + TotalDrv).ToString() + " ]";

            if (TotalDrv == (SpareDrvSpaces + TotalDrv))
                labelSavedDrivers.ForeColor = Color.Red;
            else
                labelSavedDrivers.ForeColor = DefaultForeColor;

            labelSavedTrucks.Text = "Trucks [ " + TotalVhc + " | " + (SpareVhcSpaces + TotalVhc).ToString() + " ]";
            if (TotalVhc == (SpareVhcSpaces + TotalVhc))
                labelSortingTrucks.ForeColor = Color.Red;
            else
                labelSortingDrivers.ForeColor = DefaultForeColor;

            //Drivers and Trucks to sort
            foreach (string tempD in MainForm.extraDrivers)
            {
                if (tempD != null)
                    treeViewSortingDrivers.Nodes.Add(tempD, tempD);
            }

            foreach (string tempV in MainForm.extraVehicles)
            {
                if (tempV != null)
                {
                    //treeViewSortingTrucks.Nodes.Add(tempV);
                    string TruckName = "";
                    try
                    {
                        string templine = MainForm.UserTruckDictionary[tempV].Parts.Find(x => x.PartType == "truckbrandname").PartData.Find(xline => xline.StartsWith(" data_path:"));
                        TruckName = templine.Split(new char[] { '"' })[1].Split(new char[] { '/' })[4];
                    }
                    catch { }
                    MainForm.TruckBrandsLngDict.TryGetValue(TruckName, out string trucknamevalue);

                    if (trucknamevalue != null && trucknamevalue != "")
                    {
                        TruckName = trucknamevalue;
                    }

                    treeViewSortingTrucks.Nodes.Add(tempV, TruckName);
                }
            }
            //Labels
            labelSortingDrivers.Text = "Drivers " + MainForm.extraDrivers.Count(x => x != null).ToString();
            if (SpareDrvSpaces < MainForm.extraDrivers.Count)
                labelSortingDrivers.ForeColor = Color.Red;
            else
                labelSortingDrivers.ForeColor = DefaultForeColor;

            labelSortingTrucks.Text = "Trucks " + MainForm.extraVehicles.Count(x =>x != null).ToString();
            if (SpareVhcSpaces < MainForm.extraVehicles.Count)
                labelSortingTrucks.ForeColor = Color.Red;
            else
                labelSortingDrivers.ForeColor = DefaultForeColor;
        }

        private void buttonMoveDriversOut_Click(object sender, EventArgs e)
        {
            // Get the checked nodes.
            List<TreeNode> checked_nodes = CheckedNodes(treeViewSavedDrivers, false);

            if (checked_nodes.Count() != 0)
            {
                foreach (TreeNode tempD in checked_nodes)
                {
                    if (tempD != null)
                    {
                        Garages tempG = MainForm.GaragesList[MainForm.GaragesList.FindIndex(x => x.GarageName == tempD.Parent.Name)];
                        string driverNL = tempD.Name;

                        if(driverNL != MainForm.PlayerDataV.UserDriver)
                        {
                            tempG.Drivers[tempG.Drivers.FindIndex(x => x == driverNL)] = null;
                            MainForm.extraDrivers.Add(driverNL);
                            MainForm.extraVehicles.Add(null);
                        }

                        checked_nodes.RemoveAt(0);
                        if (checked_nodes.Count() == 0)
                            goto NoMoreDrivers;
                    }
                }
                NoMoreDrivers:
                FillTreeView();
            }
        }

        private void buttonMoveTrucksOut_Click(object sender, EventArgs e)
        {
            // Get the checked nodes.
            List<TreeNode> checked_nodes = CheckedNodes(treeViewSavedTrucks);

            if (checked_nodes.Count() != 0)
            {
                foreach (TreeNode tempT in checked_nodes)
                {
                    if (tempT != null)
                    {
                        Garages tempG = MainForm.GaragesList[MainForm.GaragesList.FindIndex(x => x.GarageName == tempT.Parent.Name)];
                        string truckNL = tempT.Name;

                        tempG.Vehicles[tempG.Vehicles.FindIndex(x => x == truckNL)] = null;
                        MainForm.extraVehicles.Add(truckNL);
                        MainForm.extraDrivers.Add(null);

                        checked_nodes.RemoveAt(0);
                        if (checked_nodes.Count() == 0)
                            goto NoMoreTrucks;
                    }
                }
                NoMoreTrucks:
                FillTreeView();
            }
        }

        private void buttonMoveDriversIn_Click(object sender, EventArgs e)
        {
            // Get the checked nodes.
            List<TreeNode> checked_nodes = CheckedNodes(treeViewSortingDrivers);

            if (checked_nodes.Count() != 0)
            {
                foreach (Garages tempG in MainForm.GaragesList)
                {
                    if (tempG.GarageStatus != 0)
                    {
                        for (int i = 0; i < tempG.Drivers.Count; i++)
                        {
                            if (tempG.Drivers[i] == null)
                            {
                                string driverNL = checked_nodes[0].Name;
                                tempG.Drivers[i] = driverNL;
                                MainForm.extraDrivers[MainForm.extraDrivers.FindIndex(x => x == driverNL)] = null;
                                checked_nodes.RemoveAt(0);
                                if (checked_nodes.Count() == 0)
                                    goto NoMoreDrivers;
                            }
                        }
                    }
                }
                NoMoreDrivers:
                FillTreeView();
            }
        }

        private void buttonMoveTrucksIn_Click(object sender, EventArgs e)
        {
            // Get the checked nodes.
            List<TreeNode> checked_nodes = CheckedNodes(treeViewSortingTrucks);

            if (checked_nodes.Count() != 0)
            {
                foreach (Garages tempG in MainForm.GaragesList)
                {
                    if (tempG.GarageStatus != 0)
                    {
                        for (int i = 0; i < tempG.Vehicles.Count; i++)
                        {
                            if (tempG.Vehicles[i] == null)
                            {
                                string truckNL = checked_nodes[0].Name;
                                tempG.Vehicles[i] = truckNL;
                                MainForm.extraVehicles[MainForm.extraVehicles.FindIndex(x => x == truckNL)] = null;
                                checked_nodes.RemoveAt(0);
                                if (checked_nodes.Count() == 0)
                                    goto NoMoreTrucks;
                            }
                        }
                    }
                }
                NoMoreTrucks:
                FillTreeView();
            }
        }

        // Return a list of the TreeNodes that are checked.
        private void FindCheckedNodes(List<TreeNode> checked_nodes, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                // Add this node.
                if (node.Checked)
                    checked_nodes.Add(node);

                // Check the node's descendants.
                FindCheckedNodes(checked_nodes, node.Nodes);
            }
        }

        private void FindCheckedNodes(List<TreeNode> checked_nodes, TreeNodeCollection nodes, bool FindParentsOrChild)
        {
            foreach (TreeNode node in nodes)
            {
                // Add this node.
                if (FindParentsOrChild)
                    if (node.Checked && node.Parent == null)
                        checked_nodes.Add(node);

                // Check the node's descendants.
                if (!FindParentsOrChild)
                {
                    if (node.Checked && node.Parent != null)
                        checked_nodes.Add(node);
                    FindCheckedNodes(checked_nodes, node.Nodes, FindParentsOrChild);
                }
                    
            }
        }

        // Return a list of the checked TreeView nodes.
        private List<TreeNode> CheckedNodes(TreeView trv)
        {
            List<TreeNode> checked_nodes = new List<TreeNode>();
            FindCheckedNodes(checked_nodes, trv.Nodes);
            return checked_nodes;
        }

        private List<TreeNode> CheckedNodes(TreeView trv, bool FindParentsOrChild)
        {
            List<TreeNode> checked_nodes = new List<TreeNode>();

            FindCheckedNodes(checked_nodes, trv.Nodes, FindParentsOrChild);

            return checked_nodes;
        }
    }
}
