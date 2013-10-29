using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SiceXPNREST
{
    [DataContract]
    public class Operation
    {
        [DataMember]
        public string  Name="" ;
        [DataMember]
        public string OpID = "";
        [DataMember]
        public string Value = "";
    }
}