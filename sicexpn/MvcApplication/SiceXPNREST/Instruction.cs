using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Runtime.Serialization;
namespace SiceXPNREST
{
    [DataContract]
    public class Instruction
    {
        [DataMember]
        public DateTime OPTime;

        private List<Device> _DeviceList = new List<Device>();
        [DataMember]
        public List<Device> DeviceList
        {
            get { return _DeviceList; }
            set { _DeviceList = value; }
        }
         [DataMember]
        public string MWID { get; set; }
    }
}