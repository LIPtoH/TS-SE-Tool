using System;

namespace TS_SE_Tool.Utilities
{
    public class DateTimeUtilities
    {
        private static DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime UnixTimeStampToDateTime(double _unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            //DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            DateTime dtDateTime = EpochTime.AddSeconds(_unixTimeStamp).ToLocalTime();

            return dtDateTime;
        }

        public static double DateTimeToUnixTimeStamp(DateTime _dateTime)
        {
            // Unix timestamp is seconds past epoch
            //DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            double unixTimeStamp = _dateTime.ToUniversalTime().Subtract(EpochTime).TotalSeconds;

            return unixTimeStamp;
        }
    }
}
