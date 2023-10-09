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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TS_SE_Tool.Utilities
{
    class IO_Utilities
    {
        internal static void DirectoryCopy(string _sourceDirName, string _destDirName, bool _copySubDirs)
        {
            DirectoryCopy(_sourceDirName, _destDirName, _copySubDirs, null);
        }

        internal static void DirectoryCopy(string _sourceDirName, string _destDirName, bool _copySubDirs, string[] _fileList)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dirInfo = new DirectoryInfo(_sourceDirName);

            if (!dirInfo.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + _sourceDirName);
            }
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(_destDirName))
            {
                Directory.CreateDirectory(_destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dirInfo.GetFiles();
            string tempPath = "";

            foreach (FileInfo file in files)
            {
                if ( _fileList != null )
                    if ( !_fileList.Contains(file.Name) )
                        continue;

                tempPath = Path.Combine(_destDirName, file.Name);

                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (_copySubDirs)
            {
                DirectoryInfo[] dirInfoArray = dirInfo.GetDirectories();

                foreach (DirectoryInfo subdir in dirInfoArray)
                {
                    tempPath = Path.Combine(_destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, _copySubDirs, _fileList);
                }
            }
        }

        internal static void LogWriter(string _error)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\log.log", true))
                {
                    writer.WriteLine(DateTime.Now + " " + _error);
                }
            }
            catch
            { }
        }

        internal static void ErrorLogWriter(string _error)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + @"\errorlog.log", true))
                {
                    writer.WriteLine(DateTime.Now + " | " + AssemblyData.AssemblyProduct + " - " + AssemblyData.AssemblyVersion + " | " + 
                                    Globals.SelectedProfileName + " [ " + Globals.SelectedProfile + " ] >> " + 
                                    Globals.SelectedSaveName + " [ " + Globals.SelectedSave + " ] ");
                    writer.WriteLine(_error + Environment.NewLine);
                }
            }
            catch
            { }
        }

        internal static void WritePreviewTOBJ(string _path, string _name, string _pathToTGA)
        {
            WritePreviewTOBJ(_path + "\\" + _name + ".tobj", _pathToTGA);
        }

        internal static void WritePreviewTOBJ(string _pathToTOBJ, string _pathToTGA)
        {
            using (BinaryWriter binWriter = new BinaryWriter(File.Open(_pathToTOBJ, FileMode.Create)))
            {
                byte[] preview_tobj = new byte[] { 1, 10, 177, 112, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 3, 3, 2, 0, 2, 2, 2, 1, 0, 0, 0, 1, 0, 0 };

                binWriter.Write(preview_tobj);

                byte filePathLength = (byte)_pathToTGA.Length;
                binWriter.Write(filePathLength);

                binWriter.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0 });

                binWriter.Write(Encoding.UTF8.GetBytes(_pathToTGA));
            }
        }
    }
}
