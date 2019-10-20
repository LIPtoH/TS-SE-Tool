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
using System.Resources;

namespace TS_SE_Tool
{
    class PlainTXTResourceSet : ResourceSet
    {

        internal PlainTXTResourceSet(CultureInfo culture) : base(new PlainTXTResourceReader(culture))//(dsn, culture))
        {
        }

        public override Type GetDefaultReader()
        {
            return typeof(PlainTXTResourceReader);
        }

    }
}
