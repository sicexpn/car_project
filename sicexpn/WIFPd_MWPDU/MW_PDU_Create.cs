using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WIFPd_MWPDU
{
    public class MW_PDU_Create
    {
        private byte ver;
        private int num;
        private Crc32 crc = new Crc32();

        public MW_PDU_Create(byte ver)
        {
            // TODO: Complete member initialization
            this.ver = ver;
            num = 1;
        }

        /// <summary>
        /// 构造PDU头部
        /// </summary>
        /// <param name="GwID">设备ID</param>
        /// <param name="SendCount">重发次数，默认为0</param>
        /// <param name="p">数据长度？</param>
        /// <param name="segment">段号，默认为0</param>
        /// <returns>20字节的PDU头部</returns>
        private byte[] BuildHead(long GwID, int SendCount, int p, int segment)
        {
            byte[] Head = new byte[PublicConst.WIFP_MW_PDU_HEADER_SIZE]; //20字节的PDU头部
            Head[PublicConst.WIFP_MW_PDU_VERSION_OFFSET] = ver; //版本
            Head[PublicConst.WIFP_MW_PDU_TIMES_OFFSET] = (byte)SendCount; //发送次数

            byte[] AllLength = BitConverter.GetBytes(PublicConst.WIFP_MW_PDU_HEADER_SIZE + PublicConst.WIFP_MW_PDU_CENTER_SIZE + p + PublicConst.WIFP_MW_PDU_CRC_SIZE); //PDU总长度=头部+中间部分+数据+CRC
            for (int i = PublicConst.WIFP_MW_PDU_LENGTH_SIZE - 1; i >= 0; i--) //总长度只有2字节
            {
                Head[PublicConst.WIFP_MW_PDU_LENGTH_OFFSET + PublicConst.WIFP_MW_PDU_LENGTH_SIZE - 1 - i] = AllLength[i];
            }

            byte[] ID = BitConverter.GetBytes(GwID); //设备ID
            for (int i = PublicConst.WIFP_MW_PDU_DEVID_SIZE - 1; i >= 0; i--)
            {
                Head[PublicConst.WIFP_MW_PDU_DEVID_OFFSET + PublicConst.WIFP_MW_PDU_DEVID_SIZE - 1 - i] = ID[i];
            }

            byte[] NUM = BitConverter.GetBytes(num); //序列号
            for (int i = PublicConst.WIFP_MW_PDU_SEQUENCE_SIZE - 1; i >= 0; i--)
            {
                Head[PublicConst.WIFP_MW_PDU_SEQUENCE_OFFSET + PublicConst.WIFP_MW_PDU_SEQUENCE_SIZE - 1 - i] = NUM[i];
            }

            byte[] Segment = BitConverter.GetBytes(segment); //段号
            for (int i = PublicConst.WIFP_MW_PDU_SEGMENT_SIZE - 1; i >= 0; i--)
            {
                Head[PublicConst.WIFP_MW_PDU_SEGMENT_OFFSET + PublicConst.WIFP_MW_PDU_SEGMENT_SIZE - 1 - i] = Segment[i];
            }

            return Head;
        }

        /// <summary>
        /// 构造PDU中间部分（1字节命令标识+3字节保留+4字节时间戳）
        /// </summary>
        /// <param name="label">命令标识：数据读取-0x01;数据上传-0x02;下行控制-0x03</param>
        /// <returns>8字节的PDU中间部分</returns>
        private byte[] BuildCenter(byte label)
        {
            byte[] Center = new byte[PublicConst.WIFP_MW_PDU_CENTER_SIZE];
            Center[0] = label;

            DateTime dt = DateTime.Now;
            int timeStamp = UTCTime.DateTimeToUTC(dt);
            byte[] time = BitConverter.GetBytes(timeStamp);
            for (int i = time.Length-1; i >= 0; i--)
            {
                Center[PublicConst.WIFP_MW_PDU_CENTER_SIZE - 1 - i] = time[i];
            }

            return Center;
        }

        /// <summary>
        /// 根据浮点数据生成PDUData段
        /// </summary>
        /// <param name="ListFloat"></param>
        /// <returns></returns>
        private byte[] BuildData(List<float> ListFloat)
        {
            byte[] data = new byte[ListFloat.Count * PublicConst.WIFP_MW_PDU_RESOURCE_SIZE];
            int z = 0;
            for (int i = 0; i < ListFloat.Count; i++)
            {
                byte[] value = BitConverter.GetBytes(ListFloat[i]);
                for (int x = value.Length-1; x >= 0; x--)
                {
                    data[z] = value[x];
                    z++;
                }
            }
            return data;
        }

        /// <summary>
        /// 生成PDU
        /// </summary>
        /// <param name="GwID">网关ID</param>
        /// <param name="SendCount">发送次数</param>
        /// <param name="label">数据标示</param>
        /// <param name="ListValue">数据</param>
        /// <param name="segment">段</param>
        /// <returns></returns>
        public byte[] CreatePDU(long GwID, int SendCount, byte label, List<Entity.Value> ListValue, int segment)
        {
            //对ListValue中的值按照屏蔽位从低到高排序
            ListValue.Sort(new CompareValue());

            DateTime dt = DateTime.Now;

            byte[] Center = BuildCenter(label);

            byte[] Data = ConstructData(ListValue);

            byte[] Head = BuildHead(GwID, SendCount, Data.Length, segment);

            byte[] _pdu = new byte[Head.Length + Center.Length  + Data.Length];

            #region 组合数据
            for (int i = 0; i < Head.Length; i++)
            {
                _pdu[i] = Head[i];
            }
            for (int i = 0; i < Center.Length; i++)
            {
                _pdu[PublicConst.WIFP_MW_PDU_COMMAND_OFFSET + i] = Center[i];
            }
            for (int i = 0; i < Data.Length; i++)
            {
                _pdu[PublicConst.WIFP_MW_PDU_MASK_OFFSET + i] = Data[i];
            }

            byte[] pdu = new byte[_pdu.Length + 4];

            for (int i = 0; i < _pdu.Length; i++)
            {
                pdu[i] = _pdu[i];
            }

            byte[] byteCrc = crc.ByteCrcAdd(_pdu);

            for (int i = 0; i < byteCrc.Length; i++)
            {
                pdu[_pdu.Length + i] = byteCrc[i];
            }

            #endregion

            if (num < PublicConst.WIFP_MW_PDU_MAX_SEQUENCE)
            {
                num++;
            }
            else
            {
                num = 0;
            }

            Debug.Print("run time:" + (DateTime.Now - dt));
            return pdu;
        }

        private byte[] ConstructData(List<Entity.Value> ListValue)
        {
            List<float> lf = new List<float>();
            List<int> li = new List<int>();
            foreach(Entity.Value v in ListValue)
            {
                lf.Add(v.value);
                li.Add(v.num);
            }

            byte[] Data = BuildData(lf);
            byte[] Mask = BuildMask(li);
            byte[] result = new byte[Data.Length + Mask.Length];

            for (int i = 0; i < Mask.Length; i++)
            {
                result[i] = Mask[i];
            }

            for (int i = 0; i < Data.Length; i++)
            {
                result[PublicConst.WIFP_MW_PDU_MASK_SIZE + i] = Data[i];
            }

            return result;
        }

        /// <summary>
        /// 构造PDU屏蔽位字段
        /// </summary>
        /// <param name="li"></param>
        /// <returns></returns>
        private byte[] BuildMask(List<int> li)
        {
            long maskDec = 0;
            long original = 1;
            byte[] maskbit = new byte[PublicConst.WIFP_MW_PDU_MASK_SIZE]; //8字节的屏蔽位
            foreach (int step in li)
            {
                maskDec += original << step; //将对应屏蔽位移位置1
            }
            maskbit = BitConverter.GetBytes(maskDec); //将屏蔽位数值转成字节数组
            Array.Reverse(maskbit); //调整字节序(Big-endian)
            return maskbit;
        }
    }
    
    public class CompareValue : IComparer<Entity.Value>
    {
        public int Compare(Entity.Value a, Entity.Value b)
        {
            if (a.num > b.num)
                return 1;
            else if (a.num == b.num)
                return 0;
            else
                return -1;
        }
    }
}