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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Resources;

namespace TS_SE_Tool
{
    class PlainTXTResourceReader : IResourceReader
    {

        //private string dsn;
        private string language;

        public PlainTXTResourceReader (CultureInfo culture) //(string _dsn, CultureInfo culture)
        {
            //dsn = _dsn;
            language = culture.Name;
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            //Base
            try
            {
                StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + @"\lang\Default\lngfile.txt", Encoding.UTF8);

                while (!reader.EndOfStream)
                {
                    string[] linechunk;
                    string line = reader.ReadLine();

                    if (line != "" && !line.StartsWith("["))
                    {
                        linechunk = line.Split(new char[] { ';' }, 2);
                        dict.Add(linechunk[0], linechunk[1]);
                    }
                }

                reader.Close();
            }
            catch  // ignore
            {
            }
            //LNG
            try
            {
                StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + @"\lang\" + language + @"\lngfile.txt", Encoding.UTF8);
                                
                while (!reader.EndOfStream)
                {
                    string[] linechunk;
                    string line = reader.ReadLine();

                    if (line != "" && !line.StartsWith("["))
                    {
                        linechunk = line.Split(new char[] { ';' }, 2);
                        //dict.Add(linechunk[0], linechunk[1]);
                        if (dict.ContainsKey(linechunk[0]))
                            dict[linechunk[0]] = linechunk[1];
                        else
                            dict.Add(linechunk[0], linechunk[1]);
                    }
                }

                reader.Close();
            }
            catch  // ignore
            {
            }

            return dict.GetEnumerator();
        }

        public void Close()
        {

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IDisposable.Dispose()
        {
        }

    }
}
