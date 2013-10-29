using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIFPd_MWPDU
{
    public class UTCTime
    {
        /// <summary>
        /// UTC转DataTime
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static DateTime UTCToDateTime(int seconds)
        {
            DateTime dtZone = new DateTime(1970, 1, 1, 0, 0, 0);
            dtZone = dtZone.AddSeconds(seconds);
            return dtZone;
        }

        /// <summary>
        /// DataTime转UTC时间
        /// </summary>
        /// <param name="vDate"></param>
        /// <returns></returns>
        public static int DateTimeToUTC(DateTime vDate)
        {
            DateTime dtZone = new DateTime(1970, 1, 1, 0, 0, 0);
            return (int)vDate.Subtract(dtZone).TotalSeconds;
        }
    }
}
