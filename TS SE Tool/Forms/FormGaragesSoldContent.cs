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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TS_SE_Tool.Save.Items;

namespace TS_SE_Tool
{
    public partial class FormGaragesSoldContent : Form
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();
        int SpareDrvSpaces = 0, SpareVhcSpaces = 0;

        List<Garages> thisGarageList = new List<Garages>();
        List<string> thisExtraVehicles = new List<string>();
        List<string> thisExtraDrivers = new List<string>();

        public FormGaragesSoldContent()
        {
            InitializeComponent();
            
            PrepareForm();
        }

        private void PrepareForm()
        {
            this.Icon = Properties.Resources.MainIco;

            // Clone
            foreach(Garages garage in MainForm.GaragesList)
            {
                thisGarageList.Add((Garages)garage.DeepClone());
            }

            thisExtraVehicles = new List<string>(MainForm.extraVehicles);
            thisExtraDrivers = new List<string>(MainForm.extraDrivers);

            //dialog result
            buttonSave.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            FillTreeView();
        }

        private void FillTreeView()
        {
            FillSavedDriversTreeView();
            FillSavedTrucksTreeView();

            FillSortingDriversTreeView();
            FillSortingTrucksTreeView();
        }

        private void FillSavedDriversTreeView()
        {
            int TotalDrv = 0;
            SpareDrvSpaces = 0;

            treeViewSavedDrivers.BeginUpdate();
            treeViewSavedDrivers.Nodes.Clear();

            foreach (Garages tempG in thisGarageList)
            {
                if (tempG.GarageStatus > 0)
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
                    TotalDrv += curDr;

                    //Drivers tree
                    bool childes = false;

                    treeViewSavedDrivers.Nodes.Add(tempG.GarageName, "[ " + curDr + " | " + tempG.Drivers.Count + " ] " + tempG.GarageNameTranslated);
                    foreach (string tempD in tempG.Drivers)
                    {
                        if (tempD != null)
                        {
                            treeViewSavedDrivers.Nodes[tempG.GarageName].Nodes.Add(tempD, GetDriverName(tempD));

                            childes = true;
                        }
                        else
                        {
                            treeViewSavedDrivers.Nodes[tempG.GarageName].Nodes.Add(null, "---");
                        }

                    }

                    if (!childes)
                        treeViewSavedDrivers.Nodes[tempG.GarageName].Nodes.Clear();
                }
            }

            treeViewSavedDrivers.EndUpdate();

            //Label
            labelSavedDrivers.Text = "Drivers [ " + TotalDrv + " | " + (SpareDrvSpaces + TotalDrv).ToString() + " ]";

            if (TotalDrv == (SpareDrvSpaces + TotalDrv))
                labelSavedDrivers.ForeColor = Color.Red;
            else
                labelSavedDrivers.ForeColor = DefaultForeColor;

        }

        private void FillSavedTrucksTreeView()
        {
            int TotalVhc = 0;
            SpareVhcSpaces = 0;

            treeViewSavedTrucks.BeginUpdate();
            treeViewSavedTrucks.Nodes.Clear();

            foreach (Garages tempG in thisGarageList)
            {
                if (tempG.GarageStatus > 0)
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

                    SpareVhcSpaces = SpareVhcSpaces + tempG.Vehicles.Count - curVeh;
                    TotalVhc += curVeh;

                    bool childes = false;

                    //Trucks tree
                    childes = false;

                    treeViewSavedTrucks.Nodes.Add(tempG.GarageName, "[ " + curVeh + " | " + tempG.Vehicles.Count + " ] " + tempG.GarageNameTranslated);
                    foreach (string tempV in tempG.Vehicles)
                    {
                        if (tempV != null)
                        {
                            treeViewSavedTrucks.Nodes[tempG.GarageName].Nodes.Add(tempV, GetTruckName(tempV));

                            treeViewSavedTrucks.Nodes[tempG.GarageName].Nodes[tempV].ToolTipText = tempV;

                            childes = true;
                        }
                        else
                        {
                            treeViewSavedTrucks.Nodes[tempG.GarageName].Nodes.Add(null, "---");

                        }
                    }

                    if (!childes)
                        treeViewSavedTrucks.Nodes[tempG.GarageName].Nodes.Clear();
                }
            }

            treeViewSavedTrucks.EndUpdate();

            //Label
            labelSavedTrucks.Text = "Trucks [ " + TotalVhc + " | " + (SpareVhcSpaces + TotalVhc).ToString() + " ]";
            if (TotalVhc == (SpareVhcSpaces + TotalVhc))
                labelSavedTrucks.ForeColor = Color.Red;
            else
                labelSavedTrucks.ForeColor = DefaultForeColor;
        }

        private void FillSortingDriversTreeView()
        {
            treeViewSortingDrivers.BeginUpdate();

            treeViewSortingDrivers.Nodes.Clear();

            foreach (string tempD in thisExtraDrivers)
            {
                if (tempD != null)
                {
                    treeViewSortingDrivers.Nodes.Add(tempD, GetDriverName(tempD));
                }
            }

            treeViewSortingDrivers.EndUpdate();

            //Label
            labelSortingDrivers.Text = "Drivers " + thisExtraDrivers.Count(x => x != null).ToString();
            if (SpareDrvSpaces < thisExtraDrivers.Count)
                labelSortingDrivers.ForeColor = Color.Red;
            else
                labelSortingDrivers.ForeColor = DefaultForeColor;

        }
        
        private void FillSortingTrucksTreeView()
        {
            treeViewSortingTrucks.BeginUpdate();

            treeViewSortingTrucks.Nodes.Clear();

            foreach (string tempV in thisExtraVehicles)
            {
                if (tempV != null)
                {
                    treeViewSortingTrucks.Nodes.Add(tempV, GetTruckName(tempV));

                    treeViewSortingTrucks.Nodes[tempV].ToolTipText = tempV;
                }
            }

            treeViewSortingTrucks.EndUpdate();

            //Label
            labelSortingTrucks.Text = "Trucks " + thisExtraVehicles.Count(x => x != null).ToString();
            if (SpareVhcSpaces < thisExtraVehicles.Count)
                labelSortingTrucks.ForeColor = Color.Red;
            else
                labelSortingTrucks.ForeColor = DefaultForeColor;
        }

        private string GetTruckName(string _tmptruckname)
        {
            string tmpTruckName = "", TruckName = "";

            //Brand
            foreach (string accLink in MainForm.UserTruckDictionary[_tmptruckname].TruckMainData.accessories)
            {
                Type t = MainForm.SiiNunitData.SiiNitems[accLink].GetType();

                if (t.Name == "Vehicle_Accessory")
                {
                    Save.Items.Vehicle_Accessory tmp = (Save.Items.Vehicle_Accessory)MainForm.SiiNunitData.SiiNitems[accLink];
                    if (tmp.accType == "basepart")
                    {
                        tmpTruckName = tmp.data_path.Split(new char[] { '"' })[1].Split(new char[] { '/' })[4];
                    }
                }
            }

            MainForm.TruckBrandsLngDict.TryGetValue(tmpTruckName, out string trucknamevalue);

            if (trucknamevalue != null && trucknamevalue != "")
                TruckName = trucknamevalue;
            else
                TruckName = tmpTruckName;

            return TruckName;
        }

        private string GetDriverName(string _drivername)
        {
            if (MainForm.SiiNunitData.Player.drivers[0] == _drivername)
            {
               return Utilities.TextUtilities.FromHexToString(Globals.SelectedProfile);
            }
            else
            {
                MainForm.DriverNames.TryGetValue(_drivername, out string _resultvalue);

                if (_resultvalue != null && _resultvalue != "")
                {
                    return _resultvalue.TrimStart(new char[] { '+' });
                }

                return _drivername;
            }
        }

        private void buttonMoveDriversOut_Click(object sender, EventArgs e)
        {
            // Get the checked nodes.
            List<TreeNode> source_nodes = CheckedNodes(treeViewSavedDrivers);

            if (source_nodes.Count() != 0)
            {
                foreach (TreeNode tempNode in source_nodes)
                {
                    if (tempNode != null && tempNode.Name != "")
                    {
                        string grgName = "";
                        List<string> DriverList = new List<string>();

                        if (tempNode.Parent == null)
                        {
                            grgName = tempNode.Name;

                            foreach (TreeNode node in tempNode.Nodes)
                                DriverList.Add(node.Name);
                        }
                        else
                        {
                            grgName = tempNode.Parent.Name;

                            DriverList.Add(tempNode.Name);
                        }

                        Garages tempGarage = thisGarageList[thisGarageList.FindIndex(x => x.GarageName == grgName)];

                        foreach (string entry in DriverList)
                        {
                            if (entry != "" && entry != MainForm.SiiNunitData.Player.drivers[0])
                            {
                                tempGarage.Drivers[tempGarage.Drivers.IndexOf(entry)] = null;

                                thisExtraDrivers.Add(entry);
                                thisExtraVehicles.Add(null);
                            }
                        }
                    }
                }

                FillSavedDriversTreeView();

                FillSortingDriversTreeView();
            }

            buttonMoveDriversOut.Enabled = false;
        }

        private void buttonMoveTrucksOut_Click(object sender, EventArgs e)
        {
            // Get the checked nodes.
            List<TreeNode> source_nodes = CheckedNodes(treeViewSavedTrucks);

            if (source_nodes.Count() != 0)
            {
                foreach (TreeNode tempNode in source_nodes)
                {
                    if (tempNode != null && tempNode.Name != "")
                    {
                        string grgName = "";
                        List<string> TruckList = new List<string>();

                        if (tempNode.Parent == null)
                        {
                            grgName = tempNode.Name;

                            foreach (TreeNode node in tempNode.Nodes)                            
                                TruckList.Add(node.Name);
                            
                        }   
                        else
                        {
                            grgName = tempNode.Parent.Name;

                            TruckList.Add(tempNode.Name);
                        }

                        Garages tempGarage = thisGarageList[thisGarageList.FindIndex(x => x.GarageName == grgName)];

                        foreach(string entry in TruckList)
                        {
                            if (entry == "")
                                continue;

                            tempGarage.Vehicles[tempGarage.Vehicles.IndexOf(entry)] = null;

                            thisExtraVehicles.Add(entry);
                            thisExtraDrivers.Add(null);
                        }
                    }
                }

                FillSavedTrucksTreeView();

                FillSortingTrucksTreeView();
            }

            buttonMoveTrucksOut.Enabled = false;

        }

        private void buttonMoveDriversIn_Click(object sender, EventArgs e)
        {
            // Get the checked nodes.
            List<TreeNode> source_nodes = CheckedNodes(treeViewSortingDrivers);

            //Target garage
            List<TreeNode> target_nodes = CheckedNodes(treeViewSavedDrivers);

            if (source_nodes.Count() != 0)
            {
                if (target_nodes.Count() != 0)
                {
                    foreach (TreeNode tempTreeNode in target_nodes)
                    {
                        if (tempTreeNode != null && tempTreeNode.Parent == null)
                        {
                            string grgName = tempTreeNode.Name;
                            Garages tempGrg = thisGarageList[thisGarageList.FindIndex(x => x.GarageName == grgName)];

                            if (tempGrg.GarageStatus != 0)
                            {
                                for (int i = 0; i < tempGrg.Drivers.Count; i++)
                                {
                                    if (tempGrg.Drivers[i] == null)
                                    {
                                        string driverNL = source_nodes[0].Name;
                                        tempGrg.Drivers[i] = driverNL;

                                        thisExtraDrivers[thisExtraDrivers.FindIndex(x => x == driverNL)] = null;

                                        //==

                                        source_nodes.RemoveAt(0);

                                        if (source_nodes.Count == 0)
                                            goto exitloop;
                                    }
                                }
                            }
                        }

                    }

                    exitloop:;
                }
                else
                {
                    foreach (Garages tempGrg in thisGarageList)
                    {
                        if (tempGrg.GarageStatus != 0)
                        {
                            for (int i = 0; i < tempGrg.Drivers.Count; i++)
                            {
                                if (tempGrg.Drivers[i] == null)
                                {
                                    string driverNL = source_nodes[0].Name;
                                    tempGrg.Drivers[i] = driverNL;

                                    thisExtraDrivers[thisExtraDrivers.FindIndex(x => x == driverNL)] = null;
                                    
                                    //==

                                    source_nodes.RemoveAt(0);

                                    if (source_nodes.Count == 0)
                                        goto exitloop;
                                }
                            }
                        }
                    }
                    exitloop:;
                }

                FillSavedDriversTreeView();

                FillSortingDriversTreeView();
            }

            buttonMoveDriversIn.Enabled = false;
        }

        private void buttonMoveTrucksIn_Click(object sender, EventArgs e)
        {
            // Get the checked nodes.
            List<TreeNode> source_nodes = CheckedNodes(treeViewSortingTrucks);

            //Target garage
            List<TreeNode> target_nodes = CheckedNodes(treeViewSavedTrucks);

            if (source_nodes.Count() != 0)
            {
                if (target_nodes.Count() != 0)
                {
                    foreach (TreeNode tempTreeNode in target_nodes)
                    {
                        if (tempTreeNode != null && tempTreeNode.Parent == null)
                        {
                            string grgName = tempTreeNode.Name;
                            Garages tempGrg = thisGarageList[thisGarageList.FindIndex(x => x.GarageName == grgName)];

                            if (tempGrg.GarageStatus != 0)
                            {
                                for (int i = 0; i < tempGrg.Vehicles.Count; i++)
                                {
                                    if (tempGrg.Vehicles[i] == null)
                                    {
                                        string truckNL = source_nodes[0].Name;
                                        tempGrg.Vehicles[i] = truckNL;

                                        thisExtraVehicles[thisExtraVehicles.FindIndex(x => x == truckNL)] = null;

                                        source_nodes.RemoveAt(0);

                                        if (source_nodes.Count == 0)
                                            goto exitloop;
                                    }
                                }
                            }  
                        }
                    }

                    exitloop:;
                }
                else
                {
                    foreach (Garages tempGrg in thisGarageList)
                    {
                        if (tempGrg.GarageStatus != 0)
                        {
                            for (int i = 0; i < tempGrg.Vehicles.Count; i++)
                            {
                                if (tempGrg.Vehicles[i] == null)
                                {
                                    string truckNL = source_nodes[0].Name;
                                    tempGrg.Vehicles[i] = truckNL;

                                    thisExtraVehicles[thisExtraVehicles.FindIndex(x => x == truckNL)] = null;

                                    source_nodes.RemoveAt(0);

                                    if (source_nodes.Count == 0)
                                     goto exitloop;
                                }
                            }
                        }
                    }

                    exitloop:;
                }

                FillSavedTrucksTreeView();

                FillSortingTrucksTreeView();
            }

            buttonMoveTrucksIn.Enabled = false;
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //Apply changes
            MainForm.GaragesList = thisGarageList;

            MainForm.extraVehicles = thisExtraVehicles;
            MainForm.extraDrivers = thisExtraDrivers;
        }

        //
        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeView sourceTreeView = sender as TreeView;

            string n = sourceTreeView.Name;

            List<TreeNode> selected_nodes = CheckedNodes(sourceTreeView);

            bool state = selected_nodes.Count() != 0;

            switch (sourceTreeView.Name)
            {
                case "treeViewSavedDrivers":
                    {
                        buttonMoveDriversOut.Enabled = state;
                        break;
                    }
                case "treeViewSavedTrucks":
                    {
                        buttonMoveTrucksOut.Enabled = state;
                        break;
                    }
                case "treeViewSortingDrivers":
                    {
                        buttonMoveDriversIn.Enabled = state;
                        break;
                    }
                case "treeViewSortingTrucks":
                    {
                        buttonMoveTrucksIn.Enabled = state;
                        break;
                    }
            }
        }

    }
}
