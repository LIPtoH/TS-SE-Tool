using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS_SE_Tool.Save.Items
{
    internal class SiiNBlockCore
    {
        internal List<string> UnidentifiedLines = new List<string>();

        internal void removeWritenBlock(string _input)
        {
            TSSET_Help.fmRemoveWritenBlock(_input);
        }

        internal string WriteUnidentifiedLines()
        {
            //Must terminate with a newline, otherwise the closing brace the caller appends
            //right after would end up glued to the last preserved line.
            if (UnidentifiedLines.Count == 0)
                return "";

            return String.Join(Environment.NewLine, UnidentifiedLines) + Environment.NewLine;
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
