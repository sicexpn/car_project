using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WIFPd_MWPDU.Entity
{
    public class PDU
    {
        public long GwID { get; set; }

        public int PduLength { get; set; }

        public byte label { get; set; }

        public DateTime Timestamp { get; set; }

        public byte ver { get; set; }

        public byte SendCount { get; set; }

        public int DataNum { get; set; }

        public int paragraph { get; set; }

        public bool crc { get; set; }

        public List<Entity.Value> DataList { get; set; }
    }
}
