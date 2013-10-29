using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace MvcApplication界面设计
{
    public class PDU
    {
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));

            return (int)(time - startTime).TotalSeconds;

        }
        /// <summary>
        /// Udp 封装
        /// </summary>
        /// <returns></returns>
        public static byte[] Udp(String serial_number,String cmd_id, String mask, params String[] resources)
        {

            #region Head
            //byte[] UDP_head = new byte[20];
            ////WIFP_d 头 定长20字节
            byte[] GW_ID = new byte[8];//网关ID
            String gw_id = "0001000023cbd64a";
            GW_ID = getStrHex(gw_id);

            byte[] Length = new byte[2];//总长度
            int resource_number = resources.Length;
            int length = 20 + 20 + 4 * resource_number;
            Length[0] = (byte)(length%256);
            Length[1] = (byte)(length / 256);
            //Length = MathUtils.GetbytesInt16(length);

            byte[] Version = new byte[1];//协议版本00
            Version = getStrHex("00");
            byte[] Counts = new byte[1];//重发次数
            Counts = getStrHex("00");
            byte[] SerialNumber = new byte[2];//序列号
            SerialNumber = getStrHex(serial_number);
            byte[] Segment = new byte[2];//段
            Segment = getStrHex("0000");
            byte[] ReseverdField = new byte[4];//保留字段
            
            List<byte> head = new List<byte>();
            head.AddRange(GW_ID);
            head.AddRange(Length);
            head.AddRange(Version);
            head.AddRange(Counts);
            head.AddRange(SerialNumber);
            head.AddRange(Segment);
            head.AddRange(ReseverdField);
            #endregion

            #region data
            //WIFP_d 数据（每段支持0-64个资源）
            
            byte[] Cmd_ID = new byte[1];//命令标识
            Cmd_ID = getStrHex(cmd_id);
            byte[] ReseverdField2 = new byte[3];//保留字段
            byte[] TimeStamp = new byte[4];//时间戳
            DateTime dt = DateTime.Now;
            int epoch = ConvertDateTimeInt(dt);
            TimeStamp = MathUtils.GetbytesInt32(epoch);

            byte[] Mask = new byte[8];//屏蔽位
            Mask = getStrHex(mask);
            List<byte> data = new List<byte>();
            data.AddRange(Cmd_ID);
            data.AddRange(ReseverdField2);
            data.AddRange(TimeStamp);
            data.AddRange(Mask);

            byte[] res = new byte[4];//资源：数目1-64,可变
            for (int i = 0; i < resource_number; i++)
            { 
                res = MathUtils.GetbytesInt32(int.Parse(resources[i]));
                data.AddRange(res);
            }
            #endregion
            List<byte> pdu = new List<byte>();
            pdu.AddRange(head);
            pdu.AddRange(data);
            
            byte[] CRC = new byte[4];//CRC校验
            CRC = CRCck32(pdu.ToArray());
            pdu.AddRange(CRC);
            

            return pdu.ToArray();
            //#endregion

           
            //head.AddRange(data);

            //return null;
        }
        /// <summary>
        /// CRCck16
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CRCck16(byte[] data)
        {
            byte CRC_L = 0xFF;
            byte CRC_H = 0xFF;   //CRC寄存器 
            byte SH;
            byte SL;
            byte[] temp = data;
            int j;

            for (int i = 0; i < temp.Length; i++)
            {
                CRC_L = (byte)(CRC_L ^ temp[i]); //每一个数据与CRC寄存器进行异或 
                for (j = 0; j < 8; j++)
                {
                    SH = (byte)(CRC_H & 0x01);
                    SL = (byte)(CRC_L & 0x01);

                    CRC_H = (byte)(CRC_H >> 1);      //高位右移一位
                    CRC_H = (byte)(CRC_H & 0x7F);
                    CRC_L = (byte)(CRC_L >> 1);      //低位右移一位 
                    CRC_L = (byte)(CRC_L & 0x7F);

                    if (SH == 0x01) //如果高位字节最后一位为1 
                    {
                        CRC_L = (byte)(CRC_L | 0x80);   //则低位字节右移后前面补1 
                    }             //否则自动补0 
                    if (SL == 0x01) //如果LSB为1，则与多项式码进行异或 
                    {
                        CRC_H = (byte)(CRC_H ^ 0xA0);
                        CRC_L = (byte)(CRC_L ^ 0x01);
                    }
                }
            }
            byte[] result = new byte[2];
            result[0] = CRC_H;       //CRC高位 
            result[1] = CRC_L;       //CRC低位 
            return result;
        }
        /// <summary>
        /// crc check
        /// </summary>
        /// <param name="sInputString"></param>
        /// <returns></returns>
        public static byte[] CRCck32(byte[] buffer)
        {
            ulong[] Crc32Table;
            //生成CRC32码表
            ulong Crc;
            Crc32Table = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                Crc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                        Crc = (Crc >> 1) ^ 0xEDB88320;
                    else
                        Crc >>= 1;
                }
                Crc32Table[i] = Crc;
            }
            //获取字符串的CRC32校验值

            //byte[] buffer = GetBytes(sInputString);//ASCIIEncoding.ASCII.
            //Encoding ascII = Encoding.GetEncoding("ASCII");
            //byte[] buffer = ascII.GetBytes(sInputString);
            ulong value = 0xffffffff;
            int len = buffer.Length;
            for (i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }
            ulong data = value ^ 0xffffffff;

            byte[] bytes = new byte[8];
            byte[] result = new byte[4];
            bytes[0] = (byte)(data >> 56);
            bytes[1] = (byte)((data >> 48) & 255);
            bytes[2] = (byte)((data >> 40) & 255);
            bytes[3] = (byte)((data >> 32) & 255);
            result[0] = bytes[4] = (byte)((data >> 24) & 255);
            result[1] = bytes[5] = (byte)((data >> 16) & 255);
            result[2] = bytes[6] = (byte)((data >> 8) & 255);
            result[3] = bytes[7] = (byte)(data & 255);
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.AppendFormat("data:{0}\n ", data);
            //sb.AppendFormat("data:{0:x}\n ", data);
            //sb.AppendFormat("bytes:{0:x},{1:x},{2:x},{3:x},{4:x},{5:x},{6:x},{7:x}\n ", (int)bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], (int)bytes[6], (int)bytes[7]);

            return result;
        }
        /// <summary>
        /// 字节数组转为字符串
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string getHexStr(byte[] buffer, int count)//字节数组转为字符串
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                if (Regex.Match(Convert.ToString(buffer[i], 16), "[a-f0-9]{2}").Value == "")
                    sb.Append("0" + Convert.ToString(buffer[i], 16) + "");//去掉中间的空格
                else
                    sb.Append(Convert.ToString(buffer[i], 16) + "");
            }
            return sb.ToString().ToUpper();
        }
        /// <summary>
        /// 字符串转换为字节数组，只限数字和字母
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static byte[] getStrHex(string cache)//字符串转为字节数组:16进制数转换为字节数组
        {
            int length = cache.Length;
            //if (len % 2 == 1)
            //    len++;
            int len = length / 2;

            string[] temp = new string[len];
            for (int j = 0; j < length; j += 2)//2个char字符为一组，作为字符串进行转换
            {
                temp[j / 2] = cache[j].ToString() + cache[j + 1].ToString();
            }
            byte[] buffer = new byte[len];

            for (int i = 0; i < len; i++)
            {
                buffer[i] = Convert.ToByte(temp[i], 16);
            }
            return buffer;
        }

    }
}