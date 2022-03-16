using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS_SE_Tool.Save.Items
{
    internal class SiiNBlockCore
    {

        internal void removeWritenBlock(string _input)
        {
            FormMain.SiiNunitData.NamelessControlList.Remove(_input);
        }
    }
}
