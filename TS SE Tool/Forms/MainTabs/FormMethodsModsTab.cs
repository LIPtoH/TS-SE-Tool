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
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using TS_SE_Tool.Utilities;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TS_SE_Tool {
    public partial class FormMain {
        public static BindingList<ProfileMod> ProfileMods = new BindingList<ProfileMod>();
        #region ProfileTab
        private void FillModList() {
            //Text = $"Manage Plugins for {MainForm.SelectedGame.Type}";
            tableMods.Columns.Clear();
            tableMods.Rows.Clear();
            tableMods.DataSource = ProfileMods;
            tableMods.AllowUserToAddRows = true;
            tableMods.AllowUserToDeleteRows = true;
            tableMods.AllowUserToOrderColumns = true;
            tableMods.MultiSelect = true;
            tableMods.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableMods.CellContentClick += tableMods_CellContentClick;
            tableMods.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //tableMods.RowPostPaint += TableMods_RowPostPaint;
            //tableMods.RowHeadersVisible = true;

            //tableMods.AutoGenerateColumns = false;
            //DataGridViewTextBoxColumn rowNumberColumn = new DataGridViewTextBoxColumn();
            //rowNumberColumn.HeaderText = "#";
            //rowNumberColumn.ReadOnly = true;
            //rowNumberColumn.DisplayIndex = 0;
            //rowNumberColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //var columns = GenerateDataGridViewColumns(tableMods, typeof(ProfileMod));
            //tableMods.Columns.Add(rowNumberColumn);
            //tableMods.Columns.AddRange(columns);
            PopulateMods();

            foreach (DataGridViewColumn column in tableMods.Columns) {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(ProfileMods.FirstOrDefault());
                PropertyDescriptor property = properties.Find(column.DataPropertyName, false);
                var attribute = property.Attributes[typeof(AutoSizeColumnMode)] as AutoSizeColumnMode;
                if (attribute != null) column.AutoSizeMode = attribute.Mode;
            }
        }

        #region RowSorting
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private void tableMods_MouseDown(object sender, MouseEventArgs e) {
            rowIndexFromMouseDown = tableMods.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1) {
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            } else {
                dragBoxFromMouseDown = Rectangle.Empty;
            }
        }
        private void tableMods_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    DragDropEffects dropEffect = tableMods.DoDragDrop(tableMods.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }
        private void tableMods_DragOver(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }
        private void tableMods_DragDrop(object sender, DragEventArgs e) {
            Point clientPoint = tableMods.PointToClient(new Point(e.X, e.Y));
            rowIndexOfItemUnderMouseToDrop = tableMods.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Effect == DragDropEffects.Move) {
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                tableMods.Rows.RemoveAt(rowIndexFromMouseDown);
                tableMods.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
            }
        }
        #endregion RowSorting

        private DataGridViewColumn[] GenerateDataGridViewColumns(DataGridView dgv, Type type) {
            var ret = new DataGridViewColumn[] { };
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props) {
                DataGridViewColumn column = new DataGridViewColumn();
                column.DataPropertyName = prop.Name;
                column.HeaderText = prop.Name;
                column.ValueType = prop.PropertyType;
                if (prop.PropertyType == typeof(bool)) {
                    column.CellTemplate = new DataGridViewCheckBoxCell();
                }
                ret.Append(column);
            }
            return ret;
        }

        private void TableMods_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat() {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void PopulateMods() {
            ProfileMods.Clear();
            for (int i = 0; i < MainSaveFileProfileData.ActiveMods.Count; i++) { // foreach (var mod in MainSaveFileProfileData.ActiveMods) {
                ProfileMods.Add(new ProfileMod(MainSaveFileProfileData.ActiveMods[i], i));
            }
        }

        private List<ProfileMod> GetModsFromSelected() {
            var list = new List<ProfileMod>();
            foreach (DataGridViewRow row in tableMods.SelectedRows) {
                list.Add(row.DataBoundItem as ProfileMod);
            }
            return list;
        }

        private void tableMods_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            tableMods.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void tableMods_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e) {
            //var grid = sender as DataGridView;
            //if (sender is null || grid is null || e.RowIndex < 0 || e.RowIndex >= grid.Rows.Count) return;
            //var strip = new ContextMenuStrip();
            //strip.Items.Add(new ToolStripSeparator());
            //strip.Items.Add(new ToolStripButton("Show Debug Info", null, (object sender, EventArgs args) => { MessageBox.Show(GetModsFromSelected().ToJson(true)); }));
            //e.ContextMenuStrip = strip;
        }

        private void profileModsRemove_Click(object sender, EventArgs e) {
            throw new NotImplementedException("lol");
        }

        private void profileModsDebug_Click(object sender, EventArgs e) {
            MessageBox.Show(GetModsFromSelected().ToJson(true));
        }
        #endregion ProfileTab
    }
}