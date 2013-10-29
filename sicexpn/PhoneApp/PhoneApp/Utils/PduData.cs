using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WIFPd_MWPDU;
using WIFPd_MWPDU.Entity;
using System.Threading;

namespace PhoneApp.Utils
{
    public class PduData
    {
        private IPEndPoint ipLocalPoint;      //本地IP和端口号  
        private EndPoint RemotePoint;         //远程网络地址标识  
        private Socket mySocket;              //Socket  
        private bool RunningFlag = false;     //接收线程运行标志  

        public PduData()
        { 
            IPAddress localIp = IPAddress.Parse("192.168.33.204");
            //IPAddress localIp = IPAddress.Parse("192.168.0.102");
            int localPort = 30001;
            IPAddress remoteIp = IPAddress.Parse("223.4.152.92");//平台IP 端口
                    int remotePort = 33456;
                    //IPAddress remoteIp = IPAddress.Parse("192.168.33.204");//平台IP 端口
                    //int port = 30002;
            IPEndPoint remoteIpEndPoint = new IPEndPoint(remoteIp, remotePort);

            IPEndPoint localIpEndPoint = new IPEndPoint(localIp, localPort);

            this.ipLocalPoint = localIpEndPoint;
            this.RemotePoint = remoteIpEndPoint;
            //定义网络类型，数据连接类型和网络协议UDP  
            this.mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //绑定网络地址  
            //mySocket.ReceiveFromAsync(new SocketAsyncEventArgs());
           
        }
        //定义一个委托  
        public delegate void MyInvoke(string strRecv);  
        
    }
}
