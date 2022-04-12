using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    internal class SiiNBlockCore
    {
        internal List<string> UnidentifiedLines = new List<string>();

        internal void removeWritenBlock(string _input)
        {
            FormMain.SiiNunitData.NamelessControlList.Remove(_input);
        }

        internal string WriteUnidentifiedLines()
        {
            return String.Join(Environment.NewLine, UnidentifiedLines);
        }

        internal string WriteErrorMsg(string _message, string _tagLine, string _dataLine)
        {
            return _message + Environment.NewLine + this.GetType().Name.ToLower() + " | " + _tagLine + " = " + _dataLine;
        }
    }
}
