using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcApplication界面设计
{
    public class MathUtils
    {
        /// <summary>
        /// int16 to byte[2]
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static byte[] GetbytesInt16(short argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        /// <summary>
        /// int32 byte[4]
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static byte[] GetbytesInt32(int argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        /// <summary>
        /// int64 byte[8]
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static byte[] GetbytesInt64(long argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        public static byte[] GetbytesUInt16(ushort argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        public static byte[] GetbytesUInt32(uint argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        public static byte[] GetbytesUInt64(ulong argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        public static byte[] GetbytesDouble(double argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        public static byte[] GetbytesChar(char argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
        public static byte[] GetbytesBoolean(bool argument)
        {
            byte[] byteArray = BitConverter.GetBytes(argument);
            return byteArray;
        }
    }
}
