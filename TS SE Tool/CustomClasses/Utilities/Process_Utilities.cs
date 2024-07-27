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
using System.IO;
using System.Diagnostics;

namespace TS_SE_Tool.Utilities {
    class Process_Utilities {
        public static Process StartProcess(FileInfo file, params string[] args) => StartProcess(file.FullName, file.DirectoryName, args);
        public static Process StartProcess(string file, string workDir = null, params string[] args) {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.FileName = file;
            proc.Arguments = string.Join(" ", args);
            if (workDir != null) {
                proc.WorkingDirectory = workDir;
            }
            IO_Utilities.LogWriter($"Starting Process: {proc.FileName} {proc.Arguments}");
            return Process.Start(proc);
        }
    }
}
