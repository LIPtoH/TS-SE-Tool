﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS_SE_Tool.Save.Items
{
    internal class SiiNBlockCore
    {
        private FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        internal List<string> UnidentifiedLines = new List<string>();

        internal void removeWritenBlock(string _input)
        {
            MainForm.SiiNunitData.NamelessControlList.Remove(_input);
        }

        internal string WriteUnidentifiedLines()
        {
            return String.Join(Environment.NewLine, UnidentifiedLines);
        }

        internal string WriteErrorMsg(string _message, string _tagLine, string _dataLine)
        {
            return _message + Environment.NewLine + this.GetType().Name.ToLower() + " | " + _tagLine + " = " + _dataLine;
        }
        internal string WriteErrorMsg( string _tagLine, string _dataLine)
        {
            return this.GetType().Name.ToLower() + " | " + _tagLine + " = " + _dataLine;
        }
    }
}
