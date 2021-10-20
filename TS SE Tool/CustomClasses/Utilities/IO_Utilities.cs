/*
   Copyright 2016-2020 LIPtoH <liptoh.codebase@gmail.com>

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
    }
}
