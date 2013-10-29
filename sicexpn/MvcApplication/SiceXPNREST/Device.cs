using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SiceXPNREST
{
    [DataContract]
    public class Device
    {
        public string MWID { get; set; }
        [DataMember]
        public string  Name="" ;
        [DataMember]
        public string DID = "";
        List<Operation> _OperationList = new List<Operation>();
        [DataMember]
        public List<Operation> OperationList
        {
            get { return _OperationList; }
            set { _OperationList = value; }
        }
    }
}