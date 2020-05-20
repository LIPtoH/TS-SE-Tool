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

namespace TS_SE_Tool.Utilities
{
    public class DateTimeUtilities
    {
        private static DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime UnixTimeStampToDateTime(double _unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = EpochTime.AddSeconds(_unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        public static uint DateTimeToUnixTimeStamp(DateTime _dateTime)
        {
            // Unix timestamp is seconds past epoch
            uint unixTimeStamp = Convert.ToUInt32(Math.Floor(_dateTime.ToUniversalTime().Subtract(EpochTime).TotalSeconds));

            return unixTimeStamp;
        }

        public static uint DateTimeToUnixTimeStamp()
        {
            // Unix timestamp is seconds past epoch
            DateTime _dateTime = DateTime.Now;
            return DateTimeToUnixTimeStamp(_dateTime);
        }
    }
}
