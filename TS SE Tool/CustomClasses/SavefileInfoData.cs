using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS_SE_Tool
{
    public class SavefileInfoData
    {
        FormMain MainForm = Application.OpenForms.OfType<FormMain>().Single();

        public string SaveName { get; set; }
        public int FileTime { get; set; } 
    }
}
