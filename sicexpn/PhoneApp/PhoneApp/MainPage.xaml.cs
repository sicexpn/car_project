using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Net.Sockets;
using System.Threading;
using PhoneApp.Utils;
using WIFPd_MWPDU;
using WIFPd_MWPDU.Entity;

using System.Device.Location;
using Microsoft.Phone.Reactive;
using System.Xml;
using System.IO;

namespace PhoneApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        //String Gw_Id="0001000023cbd64a";
        String url="http://www.cloudsensing.cn:9071/WIFPa/AllMWID.xml";
        String url2 = "http://www.cloudsensing.cn:9071/WIFPa/RegisterMW";
        String url3 = "http://www.cloudsensing.cn:9071/WIFPa/DeleteMW/";
        String postString = "";
        String UpdateDeployInfo="http://www.cloudsensing.cn:9071/WIFPa/UpdateDeployInfo/0001000023cbd64a?lang=zh";
        String getCmdUrl = "http://10.205.3.143:8080/CommandService/GetMW/0001000023cbd64a";
        //private Socket socket;
        //private string wifiIP = string.Empty;
        //private int wifiPort;
        //private ManualResetEvent MyEvent = new ManualResetEvent(true);
       
        //private DnsEndPoint hostEntry;
        WebClient webClient = new WebClient();
        // 客户端 Socket
        private Socket _socket;
        // 用于发送数据的 Socket 异步操作对象
        private SocketAsyncEventArgs _socketAsyncSend;
        // 用于接收数据的 Socket 异步操作对象
        private SocketAsyncEventArgs _socketAsyncReceive;
        // 是否已发送过数据
        private bool _sent = false;
        private ManualResetEvent _signalSend = new ManualResetEvent(false);
        

        private Socket _socketWifi;
        private SocketAsyncEventArgs _socketWifiAsync;

        IPAddress wifiIp = IPAddress.Parse("192.168.0.254");
        int wifiPort = 8080;
        IPEndPoint wifiEp;
        public MainPage()
        {
            InitializeComponent();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socketAsyncSend = new SocketAsyncEventArgs();
            //IPAddress remoteIp = IPAddress.Parse("192.168.0.102");
            //int remotePort = 33456;
            IPAddress remoteIp = IPAddress.Parse("223.4.152.92");
            int remotePort = 34566;
            IPEndPoint remoteEp = new IPEndPoint(remoteIp, remotePort);
            _socketAsyncSend.RemoteEndPoint = remoteEp;
            _socketAsyncSend.Completed += new EventHandler<SocketAsyncEventArgs>(_socketAsyncSend_Completed);
            
            //wifi init
            
        }
        public void SendData()
        {
            long GwId = 0x0001000023cbd64a;
            byte ver = 0xEE;
            MW_PDU_Create pdu = new MW_PDU_Create(ver);
            List<WIFPd_MWPDU.Entity.Value> listValue = new List<Value>();
            WIFPd_MWPDU.Entity.Value v1 = new Value();
            WIFPd_MWPDU.Entity.Value v2 = new Value();
            WIFPd_MWPDU.Entity.Value v3 = new Value();
            WIFPd_MWPDU.Entity.Value v5 = new Value();

            v1.num = 1;
            v1.value = 11;
            v2.num = 2;
            v2.value = 2;
            v3.num = 3;
            v3.value = 3;

            listValue.Add(v1);
            listValue.Add(v2);
            listValue.Add(v3);
            byte[] sendBytes = pdu.CreatePDU(GwId, 00, 0x01, listValue, 0);//0x01:read ; 0x02: write ;0x03: control
            //byte[] payload = new byte[1] { 222 };//Encoding.UTF8.GetBytes(txtName.Text + "：" + txtInput.Text);
            // 设置需要发送的数据的缓冲区
            _socketAsyncSend.SetBuffer(sendBytes, 0, sendBytes.Length);

            _signalSend.Reset(); // 无信号

            // 异步地向服务端发送信息（SendToAsync - UDP；SendAsync - TCP）

            _socket.SendToAsync(_socketAsyncSend);

            _signalSend.WaitOne(3000); // 阻塞
        }

        void _socketAsyncSend_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            {
                //OutputMessage(e.SocketError.ToString());
            }

            _signalSend.Set(); // 有信号

            if (!_sent)
            {
                _sent = true;
                // 注：只有发送过数据，才能接收数据，否则 ReceiveFromAsync() 时会出现异常
                ReceiveData();
            }
        }

        // 接收信息
        public void ReceiveData()
        {
            // 实例化 SocketAsyncEventArgs，并指定端口，以接收数据
            _socketAsyncReceive = new SocketAsyncEventArgs();

            IPAddress remoteIp = IPAddress.Parse("192.168.0.102");
            int remotePort = 33456;
            //IPAddress remoteIp = IPAddress.Parse("223.4.152.92");
            //int remotePort = 33456;
            IPEndPoint remoteEp = new IPEndPoint(remoteIp, remotePort);
            _socketAsyncReceive.RemoteEndPoint = remoteEp;

            // 设置接收数据的缓冲区，udp 报文（Datagram）的最大长度为 65535（包括报文头）
            _socketAsyncReceive.SetBuffer(new Byte[65535], 0, 65535);
            _socketAsyncReceive.Completed += new EventHandler<SocketAsyncEventArgs>(_socketAsyncReceive_Completed);

            // 异步地接收数据（ReceiveFromAsync - UDP；ReceiveAsync - TCP）
            _socket.ReceiveFromAsync(_socketAsyncReceive);
        }

        void _socketAsyncReceive_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // 接收数据成功，将接收到的数据转换成字符串，并去掉两头的空字节
                //var response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                //response = response.Trim('\0');
                int length=e.BytesTransferred;
                byte[] recbytes = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    recbytes[i] = e.Buffer[i];
                }
                
                MW_PDU_Convert pdu_convert = new MW_PDU_Convert();
                PDU pdu_rec = pdu_convert.PDUConvert(recbytes);
                
            }
            else
            {
                
            }

            // 继续异步地接收数据
            _socket.ReceiveFromAsync(e);
        }
        /// <summary>
        /// Get Rest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetRest_Click(object sender, RoutedEventArgs e)
        {
            //webClient.Headers[HttpRequestHeader.ContentType] = "text/xml";
            webClient.DownloadStringCompleted+=new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            
            webClient.DownloadStringAsync(new Uri(url));
        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            textBlock.Text = e.Result;//get Rest服务得到的数据
        }
        /// <summary>
        /// Post Rest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostRest_Click(object sender, RoutedEventArgs e)
        {
            webClient.UploadStringCompleted+=new UploadStringCompletedEventHandler(webClient_UploadStringCompleted);
            
            webClient.UploadStringAsync(new Uri(url2),postString);
        }
        void webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            textBlock.Text = e.Result;//获取post后rest服务返回的xml文件
        }

        private void DeleteRest_Click(object sender, RoutedEventArgs e)
        {
            //设置请求的内容格式为text/xml。
            webClient.Headers[HttpRequestHeader.ContentType] = "text/xml";
            postString= "<string xmlns="+'\"'+"http://schemas.microsoft.com/2003/10/Serialization/"+'\"'+">String content</string>";
            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UploadStringCompleted);
            url3 += "00010000751087e7";
            webClient.UploadStringAsync(new Uri(url3), postString);
            //textBlock.Text = postString+url3;
        }
        //socket send
        //private void Send_Click(object sender, RoutedEventArgs e)
        //{
        //    wifiIP = "192.168.0.102";
        //    wifiPort = 3001;
        //    //建立一个终结点对像            
        //    hostEntry = new DnsEndPoint(wifiIP, wifiPort);
        //    //创建一个Socket对象   
        //    try
        //    {
        //        sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    }
        //    catch (Exception ex)
        //    {
        //        // sysInfo.Text = "与路由器连接失败";
        //    }
        //    if (sock != null)
        //    {
        //        connectsocket();
        //    }
        //}
        //private void connectsocket()
        //{
        //    if (sock != null)
        //    {
        //        SocketAsyncEventArgs connArg = new SocketAsyncEventArgs();
        //        // 要连接的远程服务器  
        //        connArg.RemoteEndPoint = new DnsEndPoint(wifiIP, wifiPort);
        //        // 操作完成后的回调  
        //        connArg.Completed += (sendObj, arg) =>
        //        {
        //            if (arg.SocketError == SocketError.Success) //连接成功  
        //            {
        //                //Dispatcher.BeginInvoke(() => sysInfo.Text = "连接成功。");
        //                SendData("xpn");//xpn add 

        //            }
        //            else
        //            {
        //                Dispatcher.BeginInvoke(() =>
        //                {
        //                    // sysInfo.Text = "连接失败，错误：" + arg.SocketError.ToString();
        //                });
        //            }
        //            // 向调用线程报告操作结束  
        //            MyEvent.Set();
        //        };
        //        // 重置线程等待事件  
        //        MyEvent.Reset();
        //        // sysInfo.Text = "正在连接，请等候……";
        //        // 开始异连接  
        //        sock.ConnectAsync(connArg);
        //        // 等待连接完成  
        //        MyEvent.WaitOne(6000);
        //    }

        //}
        //private void SendData(string data)
        //{

            
        //    if (sock != null && sock.Connected)
        //    {
        //        SocketAsyncEventArgs sendArg = new SocketAsyncEventArgs();
        //        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(data);
        //        sendArg.SetBuffer(buffer, 0, buffer.Length);
        //        // 发送完成后的回调  
        //        sendArg.Completed += (objSender, mArg) =>
        //        {
        //            // 如果操作成功  
        //            if (mArg.SocketError == SocketError.Success)
        //            {
        //                //Dispatcher.BeginInvoke(() => sysInfo.Text = "发送成功。");

        //            }
        //            else
        //            {
        //                Dispatcher.BeginInvoke(() =>
        //                {
        //                    //this.sysInfo.Text = "发送失败，错误：" + mArg.SocketError.ToString();
        //                });
        //            }
        //            // 报告异步操作结束  
        //            MyEvent.Set();
        //        };
        //        // 重置信号  
        //        MyEvent.Reset();
        //        // Dispatcher.BeginInvoke(() => sysInfo.Text = "正在发送，请等候……");
        //        // 异步发送  
        //        sock.SendAsync(sendArg);
        //        // 等待操作完成  
        //        MyEvent.WaitOne(6000);
        //    }

        //}
        //test button
        private void BitConvert_Click(object sender, RoutedEventArgs e)
        {
            //char l ='A';
            //byte[] bytes = MathUtils.GetbytesChar(l);
            ////textBlock.Text = bytes.Length.ToString();
            //textBlock.Text = bytes[0].ToString();
            ////test1
            //string cmd = "AA1";
            //byte[] temp = System.Text.Encoding.UTF8.GetBytes(cmd);//string to byte[] ？
            //int len = cmd.Length;
            //byte[] sendBytes = new byte[len + 1];
            //int sum = 0;
            //for (int i = 0; i < len; i++)
            //{
            //    sum += temp[i];
            //    sendBytes[i] = temp[i];
            //}
            //sum %= 128;
            //sendBytes[len] = (byte)sum;

            //float tf = 1.233f;
            //byte[] recbytes = BitConverter.GetBytes(tf);
            //float f = BitConverter.ToSingle(recbytes, 0);
            //textBlock.Text = "oo";
            SendWifiData("xpn");
        }
        /// <summary>
        /// socket send test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            //IPAddress remoteIp = IPAddress.Parse("10.205.3.143");
            //int remotePort = 45336;
            //IPEndPoint remoteEp = new IPEndPoint(remoteIp, remotePort);

            SendData();
        }

        private void PostGps_Click(object sender, RoutedEventArgs e)
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 0;

            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            //watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.Start();
        }
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //获取手机的gps信息
            String latitude = e.Position.Location.Latitude.ToString();
            String longitude = e.Position.Location.Longitude.ToString();
            String altitude = e.Position.Location.Altitude.ToString();
            StringBuilder sb = new StringBuilder();
            //创建xml文件，向网关post gps信息
            #region 创建xml文件
            using (XmlWriter writer = XmlWriter.Create(sb))//不支持创建文件的形式
            {
                writer.WriteStartElement("DeployInfo");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("部署信息");

                writer.WriteStartElement("Owner");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("所有者");
                writer.WriteEndAttribute();
                writer.WriteString("行盼宁");
                writer.WriteEndElement();

                writer.WriteStartElement("Location");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("位置");
                writer.WriteEndAttribute();
                writer.WriteString("DRC507");
                writer.WriteEndElement();

                writer.WriteStartElement("Address");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("地址");
                writer.WriteEndAttribute();
                writer.WriteString("DRC");
                writer.WriteEndElement();

                writer.WriteStartElement("Manager");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("管理者");
                writer.WriteEndAttribute();
                writer.WriteString("行盼宁");
                writer.WriteEndElement();

                writer.WriteStartElement("GPS");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("GPS信息");

                writer.WriteStartElement("Latitude");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("纬度");
                writer.WriteEndAttribute();
                writer.WriteString(latitude);//纬度
                writer.WriteEndElement();


                writer.WriteStartElement("Longtitude");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("经度");
                writer.WriteEndAttribute();
                writer.WriteString(longitude);//经度
                writer.WriteEndElement();


                writer.WriteStartElement("Altitude");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("海拔");
                writer.WriteEndAttribute();
                writer.WriteString(altitude);//海拔 
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteStartElement("LocalCoordinate");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("本地位置信息");

                writer.WriteStartElement("LocalX");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("本地X坐标");
                writer.WriteEndAttribute();
                writer.WriteString("无");//x坐标
                writer.WriteEndElement();


                writer.WriteStartElement("LocalY");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("本地Y坐标");
                writer.WriteEndAttribute();
                writer.WriteString("无");//y坐标
                writer.WriteEndElement();


                writer.WriteStartElement("LocalZ");
                writer.WriteStartAttribute("Name");
                writer.WriteValue("本地Z坐标");
                writer.WriteEndAttribute();
                writer.WriteString("无");//z坐标
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
            }
            #endregion
            sb.Remove(0, 39);//去掉前面多余的xml说明
            //textBlock.Text = sb.ToString();
            postString = sb.ToString();
            WebClient webClient = new WebClient();//避免并发冲突
            webClient.Headers[HttpRequestHeader.ContentType] = "text/xml";

            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UpdateDeployInfoCompleted);

            webClient.UploadStringAsync(new Uri(UpdateDeployInfo), postString);
        }
        void webClient_UpdateDeployInfoCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            textBlock.Text = e.Result;
        }

        private void Command_Click(object sender, RoutedEventArgs e)
        {
            
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadCommandCompleted);

            webClient.DownloadStringAsync(new Uri(getCmdUrl));
            
        }
        
        void webClient_DownloadCommandCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //xml 解析获取deviceID和对应的OpId 以及时间
            String cmd_rec = e.Result;

            XmlReader reader = XmlReader.Create(new StringReader(cmd_rec));
            reader.ReadToFollowing("DID");
            int deviceId = int.Parse(reader.ReadInnerXml());

            reader.ReadToFollowing("Value");
            int OpValue = int.Parse(reader.ReadInnerXml());
            if (deviceId == 2)
            {
                int num = OpValue;//采集人编号，从value中获取
            }
            
            //通过wifi连接接Arduino板子
            switch (deviceId)
            { 
                case 01://Car
                    switch (OpValue)
                    { 
                        case 01://前进
                            SendWifiData("AA1");
                            break;
                        case 02://后退
                            SendWifiData("AA2");
                            break;
                        case 03://向左
                            SendWifiData("AA3");
                            break;
                        case 04://向右
                            SendWifiData("AA4");
                            break;
                        case 05://停止
                            SendWifiData("AA5");
                            break;
                        default:
                            break;
                    }
                    break;
                case 02://采集指令
                    SendWifiData("AA6");
                    break;
                default:
                    break;
            }
        }
        public void SendWifiData(string cmd)
        {
            byte[] temp = System.Text.Encoding.Unicode.GetBytes(cmd);//string to byte[]
            int len = cmd.Length;
            byte[] sendBytes=new byte[len+1];
            int sum = 0;
            for (int i = 0; i < len; i++)
            {
                sum += temp[i];
                sendBytes[i] = temp[i];
            }
            sum %= 256;
            sendBytes[len] = (byte)sum;

            _socketWifiAsyncSend.SetBuffer(sendBytes, 0, sendBytes.Length);
            _signalWifiSend.Reset();
            //_socketWifi.SendToAsync(_socketWifiAsyncSend);
            _socketWifi.ConnectAsync(_socketWifiAsyncSend);
            _socketWifi.SendAsync(_socketWifiAsyncSend);
            _signalWifiSend.WaitOne(3000);//阻塞


        }
        void _socketWifiAsyncSend_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
            { 
            
            }
            _signalWifiSend.Set();
            if (!_sendWifi)
            {
                _sendWifi = true;
                ReceiveWifiData();
            }
        }
        //接受wifi回传数据
        public void ReceiveWifiData()
        {
            _socketWifiAsyncReceive = new SocketAsyncEventArgs();
            _socketWifiAsyncReceive.RemoteEndPoint = wifiEp;

            _socketWifiAsyncReceive.SetBuffer(new Byte[500], 0, 500);
            _socketWifiAsyncReceive.Completed+=new EventHandler<SocketAsyncEventArgs>(_socketWifiAsyncReceive_Completed);
            _socketWifi.ReceiveFromAsync(_socketWifiAsyncReceive);
        }
        void _socketWifiAsyncReceive_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                int length = e.BytesTransferred;
                byte[] recbytes = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    recbytes[i] = e.Buffer[i];
                }
                //解析接受到的数据，并将其以pdu的形式上传到平台
            }
        }
        public void SendPduData(List<Value> listValue)
        {
            long GwId = 0x0001000023cbd64a;
            byte ver = 0xEE;
            MW_PDU_Create pdu = new MW_PDU_Create(ver);
            byte[] sendBytes = pdu.CreatePDU(GwId, 00, 0x01, listValue, 0);
            _socketAsyncSend.SetBuffer(sendBytes, 0, sendBytes.Length);

        }
    }
}