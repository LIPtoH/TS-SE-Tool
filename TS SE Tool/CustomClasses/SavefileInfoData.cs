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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TS_SE_Tool
{
    public class SavefileInfoData
    {
        private string SaveContainerNameless { get; set; } = "";
        public string Name { get; set; } = "\"\"";
        public uint Time { get; set; } = 0;
        public uint FileTime { get; set; } = 0;
        public short Version { get; set; } = 1;

        public List<string> Dependencies { get; set; }

        public string GetText()
        {
            string OutputText = "SiiNunit\r\n{\r\n";

            OutputText += "save_container : " + SaveContainerNameless + " {\r\n";
            OutputText += " name: " + Name + "\r\n";
            OutputText += " time: " + Time.ToString() + "\r\n";
            OutputText += " file_time: " + FileTime.ToString() + "\r\n";
            OutputText += " version: " + Version.ToString() + "\r\n";
            OutputText += " dependencies: " + Dependencies.Count.ToString() + "\r\n";

            byte DepCount = 0;
            foreach(string tDep in Dependencies)
            {
                OutputText += " dependencies[" + DepCount + "]: \"" + tDep + "\"\r\n";
                DepCount++;
            }

            OutputText += "}\r\n\r\n}";

            return OutputText;
        }

        public void WriteToStream (StreamWriter _streamWriter)
        {
            _streamWriter.Write(GetText());
        }

        public void PrepareInfo(string[] _InfoFileLines)
        {
            string[] chunkOfline;
            List<string> SavefileDependencies = new List<string>();

            for (int line = 0; line < _InfoFileLines.Length; line++)
            {
                if (_InfoFileLines[line].StartsWith("save_container :"))
                {
                    chunkOfline = _InfoFileLines[line].Split(new char[] { ' ' });
                    SaveContainerNameless = chunkOfline[2];
                    continue;
                }

                if (_InfoFileLines[line].StartsWith(" name:"))
                {
                    chunkOfline = _InfoFileLines[line].Split(new char[] { ' ' });
                    Name = chunkOfline[2];
                    continue;
                }

                if (_InfoFileLines[line].StartsWith(" time:"))
                {
                    chunkOfline = _InfoFileLines[line].Split(new char[] { ' ' });
                    Time = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_InfoFileLines[line].StartsWith(" file_time:"))
                {
                    chunkOfline = _InfoFileLines[line].Split(new char[] { ' ' });
                    FileTime = uint.Parse(chunkOfline[2]);
                    continue;
                }

                if (_InfoFileLines[line].StartsWith(" version:"))
                {
                    chunkOfline = _InfoFileLines[line].Split(new char[] { ' ' });
                    Version = short.Parse(chunkOfline[2]);
                    continue;
                }

                if (_InfoFileLines[line].StartsWith(" dependencies:"))
                {
                    chunkOfline = _InfoFileLines[line].Split(new char[] { ' ' });
                    Dependencies = new List<string>();
                    Dependencies.Capacity = short.Parse(chunkOfline[2]);
                    continue;
                }

                if (_InfoFileLines[line].StartsWith(" dependencies["))
                {
                    chunkOfline = _InfoFileLines[line].Split(new char[] { '"' });
                    Dependencies.Add(chunkOfline[1]);
                    continue;
                }

                if (_InfoFileLines[line].StartsWith("}"))
                {
                    break;
                }
            }
        }
    }
}
