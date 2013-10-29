using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIFPd_MWPDU
{
    /// <summary>
    /// PDU中的常量定义
    /// </summary>
    public class PublicConst
    {
        //PDU头部长度 20字节
        public const int WIFP_MW_PDU_HEADER_SIZE = 20;
        //PDU中段长度（1字节命令标识+3字节保留字段+4字节时间戳） 8字节
        public const int WIFP_MW_PDU_CENTER_SIZE = 8;
        
        #region PDU头部各字段起始位及长度
        //设备ID
        public const int WIFP_MW_PDU_DEVID_OFFSET = 0;
        public const int WIFP_MW_PDU_DEVID_SIZE = 8;
        //PDU长度
        public const int WIFP_MW_PDU_LENGTH_OFFSET = 8;
        public const int WIFP_MW_PDU_LENGTH_SIZE = 2;
        //版本号
        public const int WIFP_MW_PDU_VERSION_OFFSET = 10;
        //发送次数
        public const int WIFP_MW_PDU_TIMES_OFFSET = 11;
        //PDU数据包序列号
        public const int WIFP_MW_PDU_SEQUENCE_OFFSET = 12;
        public const int WIFP_MW_PDU_SEQUENCE_SIZE = 2;
        //PDU数据段号
        public const int WIFP_MW_PDU_SEGMENT_OFFSET = 14;
        public const int WIFP_MW_PDU_SEGMENT_SIZE = 2;
        #endregion

        #region PDU数据部分各字段起始位及长度
        //命令标识
        public const int WIFP_MW_PDU_COMMAND_OFFSET = 20;
        public const int WIFP_MW_PDU_COMMAND_SIZE = 1;
        //时间戳
        public const int WIFP_MW_PDU_TIMESTAMP_OFFSET = 24;
        public const int WIFP_MW_PDU_TIMESTAMP_SIZE = 4;
        //屏蔽位
        public const int WIFP_MW_PDU_MASK_OFFSET = 28;
        public const int WIFP_MW_PDU_MASK_SIZE = 8;
        //数据起始位
        public const int WIFP_MW_PDU_DATA_OFFSET = 36;
        //每资源数据长度
        public const int WIFP_MW_PDU_RESOURCE_SIZE = 4;
        //CRC校验码长度
        public const int WIFP_MW_PDU_CRC_SIZE = 4;
        #endregion
       
        //最大发送序列号
        public const int WIFP_MW_PDU_MAX_SEQUENCE = 65535;
        //每PDU能承载的最大资源数量
        public const int WIFP_MW_PDU_MAX_RESOURCE_COUNT = 64;
    }
}
