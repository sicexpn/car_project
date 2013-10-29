using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;

namespace MvcApplication界面设计
{
    public class SendDataUtil
    {
        static public void sendUdp(byte[] dgram)
        {
            string ip = "223.4.152.92";
            IPAddress HostIp = IPAddress.Parse(ip);
            int port = 34566;
            IPEndPoint host = new IPEndPoint(HostIp, port);
            UdpClient udpClient = new UdpClient();
            int bytes = dgram.Length;
            udpClient.Send(dgram, bytes);
        }
        static public byte[] receiveUdp()
        {
            UdpClient server = new UdpClient();
            string ip = "223.4.152.92";
            IPAddress HostIp = IPAddress.Parse(ip);
            int port = 34566;
            IPEndPoint receivePoint = new IPEndPoint(HostIp, port);
            byte[] recData = server.Receive(ref receivePoint);
            return recData;
        }
        
    }
}