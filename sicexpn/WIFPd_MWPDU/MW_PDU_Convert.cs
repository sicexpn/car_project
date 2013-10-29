using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIFPd_MWPDU
{
    public class MW_PDU_Convert
    {
        /// <summary>
        /// PDU解析
        /// </summary>
        /// <param name="RecData">接收到的数据</param>
        /// <returns></returns>
        public Entity.PDU PDUConvert(byte[] RecData)
        {
            Entity.PDU pdu = new Entity.PDU();
            pdu.GwID = GetGwID(RecData);

            pdu.PduLength = GetLength(RecData);

            pdu.label = RecData[PublicConst.WIFP_MW_PDU_COMMAND_OFFSET];

            pdu.Timestamp = GetTimeStamp(RecData);

            pdu.ver = RecData[PublicConst.WIFP_MW_PDU_VERSION_OFFSET];

            pdu.SendCount = RecData[PublicConst.WIFP_MW_PDU_TIMES_OFFSET];

            pdu.DataNum = GetDataNum(RecData);

            pdu.paragraph = GetSegment(RecData);

            pdu.crc = checkCRC(RecData, pdu.PduLength);

            pdu.DataList = GetDataList(RecData, pdu.PduLength);

            return pdu;
        }

        /// <summary>
        /// 获取数据LIST
        /// </summary>
        /// <param name="RecData"></param>
        /// <param name="PduLength"></param>
        /// <returns></returns>
        private List<Entity.Value> GetDataList(byte[] RecData, int PduLength)
        {
            List<int> mask = GetMaskList(RecData,PduLength);

            List<Entity.Value> ev = new List<Entity.Value>();
            int datacount = (PduLength - PublicConst.WIFP_MW_PDU_CRC_SIZE - PublicConst.WIFP_MW_PDU_DATA_OFFSET) / PublicConst.WIFP_MW_PDU_RESOURCE_SIZE;

            for (int i = 0; i < datacount; i++)
            {
                Entity.Value v = new Entity.Value(); 
                byte[] data = new byte[PublicConst.WIFP_MW_PDU_RESOURCE_SIZE];
                int x = 0;
                for (int z = PublicConst.WIFP_MW_PDU_DATA_OFFSET + (i * PublicConst.WIFP_MW_PDU_RESOURCE_SIZE); z < (PublicConst.WIFP_MW_PDU_DATA_OFFSET + (i + 1) * PublicConst.WIFP_MW_PDU_RESOURCE_SIZE); z++)
                {
                    data[x] = RecData[z];
                    x++;
                }
                Array.Reverse(data);
                v.value = (BitConverter.ToSingle(data, 0));
                v.num = mask[i];
                ev.Add(v);
            }

            return ev;
        }

        private List<int> GetMaskList(byte[] RecData, int PduLength)
        {
            int segment = GetSegment(RecData);
            long maskInt = GetMaskInt(RecData);
            List<int> maskList = new List<int>();
            string maskStr = Convert.ToString(maskInt, 2);
            int count = maskStr.Length-1;
            foreach (char c in maskStr)
            {
                if (c == '1')
                {
                    maskList.Add(count + segment * PublicConst.WIFP_MW_PDU_MAX_RESOURCE_COUNT);
                 }
                count--;
            }

            maskList.Reverse();

            return maskList;
        }

        /// <summary>
        /// 获取屏蔽位Int
        /// </summary>
        /// <param name="RecData"></param>
        /// <returns></returns>
        private long GetMaskInt(byte[] RecData)
        {
            byte[] result = new byte[PublicConst.WIFP_MW_PDU_MASK_SIZE];
            int i = 0;
            for (int z = PublicConst.WIFP_MW_PDU_MASK_OFFSET; z < PublicConst.WIFP_MW_PDU_DATA_OFFSET; z++)
            {
                result[i] = RecData[z];
                i++;
            }
            Array.Reverse(result);
            return BitConverter.ToInt64(result, 0);
        }

        /// <summary>
        /// 校验CRC
        /// </summary>
        /// <param name="RecData"></param>
        /// <param name="PduLength"></param>
        /// <returns></returns>
        private bool checkCRC(byte[] RecData, int PduLength)
        {
            byte[] originalData = new byte[PduLength - PublicConst.WIFP_MW_PDU_CRC_SIZE];
            for (int i = 0; i < PduLength - PublicConst.WIFP_MW_PDU_CRC_SIZE; i++)
            {
                originalData[i] = RecData[i];
            }

            byte[] originalCRC = new byte[PublicConst.WIFP_MW_PDU_CRC_SIZE];
            int z = 0;
            for (int i = PduLength - PublicConst.WIFP_MW_PDU_CRC_SIZE; i < PduLength; i++)
            {
                originalCRC[z] = RecData[i];
                z++;
            }
            Crc32 crc = new Crc32();
            byte[] checkcrc = crc.ByteCrcAdd(originalData);
            if (checkcrc[0] == originalCRC[0] & checkcrc[1] == originalCRC[1] & checkcrc[2] == originalCRC[2] & checkcrc[3] == originalCRC[3])
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 获取段
        /// </summary>
        /// <param name="RecData"></param>
        /// <returns></returns>
        private int GetSegment(byte[] RecData)
        {
            byte[] result = new byte[PublicConst.WIFP_MW_PDU_SEGMENT_SIZE];
            int i = 0;
            for (int z = PublicConst.WIFP_MW_PDU_SEGMENT_OFFSET; z < (PublicConst.WIFP_MW_PDU_SEGMENT_OFFSET + PublicConst.WIFP_MW_PDU_SEGMENT_SIZE); z++)
            {
                result[i] = RecData[z];
                i++;
            }
            Array.Reverse(result);
            return BitConverter.ToInt16(result, 0);
        }

        /// <summary>
        /// 获取序列号
        /// </summary>
        /// <param name="RecData"></param>
        /// <returns></returns>
        private int GetDataNum(byte[] RecData)
        {
            byte[] result = new byte[PublicConst.WIFP_MW_PDU_SEQUENCE_SIZE];
            int i = 0;
            for (int z = PublicConst.WIFP_MW_PDU_SEQUENCE_OFFSET; z < (PublicConst.WIFP_MW_PDU_SEQUENCE_OFFSET + PublicConst.WIFP_MW_PDU_SEQUENCE_SIZE); z++)
            {
                result[i] = RecData[z];
                i++;
            }
            Array.Reverse(result);
            return BitConverter.ToInt16(result, 0);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="RecData"></param>
        /// <returns></returns>
        private DateTime GetTimeStamp(byte[] RecData)
        {
            byte[] result = new byte[PublicConst.WIFP_MW_PDU_TIMESTAMP_SIZE];
            int i = 0;
            for (int z = PublicConst.WIFP_MW_PDU_TIMESTAMP_OFFSET; z < (PublicConst.WIFP_MW_PDU_TIMESTAMP_OFFSET + PublicConst.WIFP_MW_PDU_TIMESTAMP_SIZE); z++)
            {
                result[i] = RecData[z];
                i++;
            }
            Array.Reverse(result);
            return UTCTime.UTCToDateTime(BitConverter.ToInt32(result, 0));
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        /// <param name="RecData"></param>
        /// <returns></returns>
        private int GetLength(byte[] RecData)
        {
            byte[] result = new byte[PublicConst.WIFP_MW_PDU_LENGTH_SIZE];
            int i = 0;
            for (int z = PublicConst.WIFP_MW_PDU_LENGTH_OFFSET; z < (PublicConst.WIFP_MW_PDU_LENGTH_OFFSET + PublicConst.WIFP_MW_PDU_LENGTH_SIZE); z++)
            {
                result[i] = RecData[z];
                i++;
            }
            Array.Reverse(result);
            int length = BitConverter.ToInt16(result, 0);
            return length;
        }

        /// <summary>
        /// 获取网关ID
        /// </summary>
        /// <param name="RecData"></param>
        /// <returns></returns>
        private long GetGwID(byte[] RecData)
        {
            byte[] ByteGwID = new byte[PublicConst.WIFP_MW_PDU_DEVID_SIZE];
            for (int i = PublicConst.WIFP_MW_PDU_DEVID_OFFSET; i < (PublicConst.WIFP_MW_PDU_DEVID_OFFSET + PublicConst.WIFP_MW_PDU_DEVID_SIZE); i++)
            {
                ByteGwID[i] = RecData[i];
            }
            Array.Reverse(ByteGwID);
            Int64 GwID = BitConverter.ToInt64(ByteGwID, 0);
            return GwID;
        }
    }
}
