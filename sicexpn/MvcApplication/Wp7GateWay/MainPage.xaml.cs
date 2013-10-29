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
using WIFPd_MWPDU;
using WIFPd_MWPDU.Entity;

using System.Device.Location;
using Microsoft.Phone.Reactive;
using System.Xml;
using System.IO;

namespace Wp7GateWay
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region 常量初始化以及各种变量的声明
        private String latitude = "0";//保存gps信息
        private String longitude = "0";
        private String altitude = "0";
        String HeartBeatUrl = "http://www.cloudsensing.cn:9071/WIFPd/HeartBeat/358618041020139";//心跳消息地址
        private static char cmd_flag = '0';//0:指令数据取走；1:post数据保存未取走
        String UpdateDeployInfo = "http://www.cloudsensing.cn:9071/WIFPa/UpdateDeployInfo/0001000023cbd64a?lang=zh";
        //String getCmdUrl = "http://10.205.3.133:8080/CommandService/GetMW/0001000023cbd64a";
        
        //String getCmdUrl = "http://192.168.0.136:8080/CommandService/GetMW/0001000023cbd64a";
        //String getCmdUrl = "http://192.168.105:8080/CommandService/GetMW/0001000023cbd64a";
        String getCmdUrl = "http://www.cloudsensing.cn:9071/WIFPa/extension/command?hwid=358618041020139";
        private WebClient webClient = new WebClient();
        
        // 客户端 Socket
        private Socket _socket;
        // 用于发送数据的 Socket 异步操作对象
        private SocketAsyncEventArgs _socketAsyncSend;
        // 用于接收数据的 Socket 异步操作对象
        //private SocketAsyncEventArgs _socketAsyncReceive;
        //// 是否已发送过数据
        //private bool _sent = false;
        private ManualResetEvent _signalSend = new ManualResetEvent(false);
        IPAddress remoteIp = IPAddress.Parse("223.4.152.92");
        int remotePort = 34566;
        IPEndPoint remoteEp;
        //wifi 连接相关参数初始化
        byte[] bytes { set; get; }
        void setBytes(Byte[] bytes)
        {
            this.bytes = bytes;
        }
        byte[] getBytes()
        {
            return this.bytes;
        }
        
        #endregion
        static Thread cmd_thread;
        static Thread gps_thread;
        static Thread heart_thread;
        static bool flag = false;
        //static Thread 
        
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            #region 变量赋值
            //与平台连接的socket的参数初始化
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socketAsyncSend = new SocketAsyncEventArgs();

            remoteEp = new IPEndPoint(remoteIp, remotePort);
            _socketAsyncSend.RemoteEndPoint = remoteEp;
            _socketAsyncSend.Completed += new EventHandler<SocketAsyncEventArgs>(_socketAsyncSend_Completed);

            //wifi 连接的socket的参数的初始化
            //_socketWifi = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //_socketWifiAsyncSend = new SocketAsyncEventArgs();
            //wifiEp = new IPEndPoint(wifiIp, wifiPort);
            //_socketWifiAsyncSend.RemoteEndPoint = wifiEp;
            //_socketWifiAsyncSend.Completed += new EventHandler<SocketAsyncEventArgs>(_socketWifiAsyncSend_Completed);
            #endregion

            #region 线程调用
            if (flag == false)
            {
                cmd_thread = new Thread(new ThreadStart(command_thread));
                //cmd_thread.IsBackground = true;
                cmd_thread.Start();


                gps_thread = new Thread(new ThreadStart(gpsSend_thread));
                gps_thread.Start();

                //heart_thread = new Thread(new ThreadStart(heartGet_thread));
                //heart_thread.Start();

                flag = true;
            }
            #endregion
        }
        #region 线程调用函数
        public void command_thread()
        {
            while (true)
            {

                //if (cmd_flag == '1')
                {
                    getCommand();
                    Thread.Sleep(1000);//可修改
                }
               
            }
        }
        public void gpsSend_thread()
        {
            while (true)
            {
                SendGps();
                Thread.Sleep(15000);
            }
        }
        public void heartGet_thread()
        {
            while (true)
            {
                getHeart();
                Thread.Sleep(500);
            }
        }
        #endregion
        #region 与平台通信
        public void getHeart()
        {
            
            webClient = new WebClient();
            string hbUrl = GetRandomUri(HeartBeatUrl);
            webClient.DownloadStringAsync(new Uri(HeartBeatUrl));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_HeartBeatCompleted);

        }
        void webClient_HeartBeatCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try {
                string heartBeat_xml = e.Result;
                XmlReader reader = XmlReader.Create(new StringReader(heartBeat_xml));
                if (reader.ReadToFollowing("Content"))
                {
                    string hBflag = reader.ReadInnerXml().ToString();
                    cmd_flag = hBflag[2];
                    if (cmd_flag == '1')
                    {
                        getCommand();
                    }
                }

            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    //MessageBox.Show(ex.Message);
                    listBox1.Items.Add(ex.Message);
                    int count = listBox1.Items.Count - 1;
                    listBox1.SelectedIndex = count;
                });
            }
        }
        #region 1、获取平台指令
        public void getCommand()
        {
            webClient = new WebClient();
            webClient.AllowReadStreamBuffering = false;
            webClient.AllowWriteStreamBuffering = false;
            //string str = GetRandomUri(getCmdUrl);
            webClient.DownloadStringAsync(new Uri(getCmdUrl));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadCommandCompleted);

            
        }
        private int num;//采集人编号
        void webClient_DownloadCommandCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            //xml 解析获取deviceID和对应的OpId 以及时间 to be continued
            
            try
            {

                String cmd_rec = e.Result;
                cmd_rec = cmd_rec.Replace(@"^", @"<").Replace(@"$", @">");

                XmlReader reader = XmlReader.Create(new StringReader(cmd_rec));
                if (reader.ReadToFollowing("DID"))
                {
                    int deviceId = int.Parse(reader.ReadInnerXml());

                    reader.ReadToFollowing("Value");
                    int OpValue = int.Parse(reader.ReadInnerXml());
                    if (deviceId == 2)
                    {
                        num = OpValue;//采集人编号，从value中获取
                    }
                    //通过wifi连接接Arduino板子

                    #region 解析指令，将其发送至wifi模块
                    switch (deviceId)
                    {
                        case 01://Car
                            switch (OpValue)
                            {
                                case 01://前进
                                    SendWifiData("AA1");
                                    Dispatcher.BeginInvoke(() =>
                                    {
                                        listBox1.Items.Add("forward");
                                        int count = listBox1.Items.Count - 1;
                                        listBox1.SelectedIndex = count;
                                    });
                                    break;
                                case 02://后退
                                    SendWifiData("AA2");
                                    Dispatcher.BeginInvoke(() =>
                                    {
                                        listBox1.Items.Add("backward");
                                        int count = listBox1.Items.Count - 1;
                                        listBox1.SelectedIndex = count;
                                    });
                                    break;
                                case 03://向左
                                    SendWifiData("AA3");
                                    Dispatcher.BeginInvoke(() =>
                                    {
                                        listBox1.Items.Add("left");
                                        int count = listBox1.Items.Count - 1;
                                        listBox1.SelectedIndex = count;
                                    });
                                    break;
                                case 04://向右
                                    SendWifiData("AA4");
                                    Dispatcher.BeginInvoke(() =>
                                    {
                                        listBox1.Items.Add("right");
                                        int count = listBox1.Items.Count - 1;
                                        listBox1.SelectedIndex = count;
                                    });
                                    break;
                                case 05://停止
                                    SendWifiData("AA5");
                                    Dispatcher.BeginInvoke(() =>
                                    {
                                        listBox1.Items.Add("stop");
                                        int count = listBox1.Items.Count - 1;
                                        listBox1.SelectedIndex = count;
                                    });
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 02://采集指令
                            SendWifiData("AA6");
                            Dispatcher.BeginInvoke(() =>
                            {
                                listBox1.Items.Add("cocllect data");
                                int count = listBox1.Items.Count - 1;
                                listBox1.SelectedIndex = count;
                            });
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        listBox1.Items.Add(ex.Message);
                        int count = listBox1.Items.Count - 1;
                        listBox1.SelectedIndex = count;
                        
                    });
            }
        }
       
        #endregion
        #region 2、上传数据至平台
        //上传PDU数据至平台
        public void SendPduData(List<Value> listValue)
        {
            long GwId = 0x0001000023cbd64a;
            byte ver = 0xEE;
            MW_PDU_Create pdu = new MW_PDU_Create(ver);
            byte[] sendBytes = pdu.CreatePDU(GwId, 00, 0x01, listValue, 0);
            _socketAsyncSend.SetBuffer(sendBytes, 0, sendBytes.Length);
            // 异步地向服务端发送信息（SendToAsync - UDP；SendAsync - TCP）
            _socket.SendToAsync(_socketAsyncSend);
        }
        void _socketAsyncSend_Completed(object sender, SocketAsyncEventArgs e)
        {
        }

        //上传GPS信息属性至平台
        public void SendGps()
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
            latitude = e.Position.Location.Latitude.ToString();
            longitude = e.Position.Location.Longitude.ToString();
            altitude = e.Position.Location.Altitude.ToString();

            

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
            String postString = sb.ToString();
            WebClient webClient = new WebClient();//避免并发冲突
            webClient.Headers[HttpRequestHeader.ContentType] = "text/xml";

            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UpdateDeployInfoCompleted);

            webClient.UploadStringAsync(new Uri(UpdateDeployInfo), postString);
        }
        void webClient_UpdateDeployInfoCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            //textBlock.Text = e.Result;
        }
        #endregion
        #endregion

        #region 与wifi模块通信

        #region 1、下发指令到wifi模块
        //发送指令至wifi模块
        public void SendWifiData(string cmd)
        {
            byte[] temp = System.Text.Encoding.UTF8.GetBytes(cmd);//string to byte[] ？
            int len = cmd.Length;
            byte[] sendBytes = new byte[len + 1];
            int sum = 0;
            for (int i = 0; i < len; i++)
            {
                sum += temp[i];
                sendBytes[i] = temp[i];
            }
            sum %= 128;
            sendBytes[len] = (byte)sum;//校验和

            setBytes(sendBytes);
            //String host = "192.168.0.136";
            //Int32 port = 9900;
            //String host = "192.168.0.157";
            //Int32 port = 9900;
            String host = "192.168.0.254";
            Int32 port = 8080;
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint hostEndPoint = new IPEndPoint(ip, port);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //建立一个Socket异步事件参数
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            //注册socket完成事件
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(socketEventArg_Completed);
            socketEventArg.RemoteEndPoint = hostEndPoint;
            socketEventArg.UserToken = sock;
            try
            {
                //运行Socket
                sock.ConnectAsync(socketEventArg);
            }
            catch (SocketException ex)
            {

                //throw new SocketException((int)ex.ErrorCode); 
                Dispatcher.BeginInvoke(() =>//socket异常打印
                    {
                        //MessageBox.Show(ex.Message);
                        listBox1.Items.Add(ex.Message);
                        int count = listBox1.Items.Count - 1;
                        listBox1.SelectedIndex = count;
                    });
            }
        }
        //发送数据至wifi模块完成，然后接受回传数据
        #endregion
        void socketEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            //检测Socket是否发送出差，当前最后的一个操作
            try
            {

                if (e.SocketError != SocketError.Success)
                {
                    if (e.SocketError == SocketError.ConnectionAborted)
                    {
                        Dispatcher.BeginInvoke(() => MessageBox.Show("连接超时请重试!" + e.SocketError));
                    }
                    else if (e.SocketError == SocketError.ConnectionRefused)
                    {
                        Dispatcher.BeginInvoke(() => MessageBox.Show("服务器端启动" + e.SocketError));
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() => MessageBox.Show("出差了 " + e.SocketError));
                    }
                    //关闭连接清理资源
                    if (e.UserToken != null)
                    {
                        Socket socks = e.UserToken as Socket;
                        socks.Shutdown(SocketShutdown.Both);
                        socks.Close();
                    }
                    return;
                }
                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Connect:
                        if (e.UserToken != null)
                        {
                            Socket socket = e.UserToken as Socket;
                            //string str = get();
                            byte[] buffer = getBytes();//Encoding.UTF8.GetBytes(str);

                            //将发送内内容信息存放进Socket异步事件参数中
                            e.SetBuffer(buffer, 0, buffer.Length);
                            bool completesAsynchronously = socket.SendAsync(e);
                            //检测Socket发送是否被挂起，如果被挂起将继续进行处理
                            if (!completesAsynchronously)
                            {
                                socketEventArg_Completed(e.UserToken, e);
                            }
                        }
                        break;
                    case SocketAsyncOperation.None:

                        break;
                    case SocketAsyncOperation.Receive:

                        if (e.UserToken != null)
                        {
                            string dataFromServer = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred);
                            //h获取运行中socket对象
                            #region 发送采集信息到数据平台
                            if (e.SocketError == SocketError.Success)
                            {
                                int length = e.BytesTransferred;
                                byte[] recbytes = new byte[length];

                                for (int i = 0; i < length; i++)
                                {
                                    recbytes[i] = e.Buffer[i];
                                    //int j;
                                }
                                if (recbytes[0] == '0')
                                {
                                    if (recbytes[3] == '0')
                                    {
                                        recbytes[3] = (byte)'1';
                                    }
                                    int ivalue = (int)recbytes[3] - 48;

                                    float fvalue = (float)0.067 * ivalue;
                                    int ii;
                                    //解析接受到的数据，并将其以pdu的形式上传到平台，valueList中num如何赋值。to be continued
                                    Value v_latitude = new Value();//Gps 数据呈现给平台
                                    Value v_longitude = new Value();
                                    Value v_altitude = new Value();

                                    v_latitude.num = 0;
                                    v_longitude.num = 1;
                                    v_altitude.num = 2;
                                    v_latitude.value = float.Parse(latitude);
                                    v_longitude.value = float.Parse(longitude);
                                    v_altitude.value = float.Parse(altitude);



                                    Value v = new Value();
                                    Value v_collect = new Value();
                                    List<Value> valueList = new List<Value>();
                                    valueList.Add(v_latitude);
                                    valueList.Add(v_longitude);
                                    valueList.Add(v_altitude);

                                    v.num = num;//采集人对应的编号
                                    v.value = fvalue;
                                    valueList.Add(v);

                                    v_collect.num = 3;//平台侧显示采集的信息
                                    v_collect.value = fvalue;
                                    valueList.Add(v_collect);

                                    SendPduData(valueList);
                                    int count = 0;
                                    Dispatcher.BeginInvoke(() =>
                                        {
                                            count = listBox1.Items.Count - 1;
                                            if (count >= 0)
                                            {
                                                listBox1.SelectedIndex = count;
                                                listBox1.Items[count] += "   Successful";
                                            }
                                        });
                                }
                                else
                                {
                                    int count = 0;
                                    Dispatcher.BeginInvoke(() =>
                                    {

                                        count = listBox1.Items.Count - 1;
                                        if (count >= 0)
                                        {
                                            listBox1.SelectedIndex = count;
                                            listBox1.Items[count] += "   Successful";
                                        }
                                        else
                                        {
                                            listBox1.Items.Add("error");
                                        }
                                    });


                                }


                            }
                            #endregion
                            Socket socket = e.UserToken as Socket;
                            socket.Dispose();//释放socket连接
                            //收到信息内容绑定到Listbox控件中
                            Dispatcher.BeginInvoke(() =>
                            {
                                //listboxsendText.Items.Add("客户端" + DateTime.Now.ToShortTimeString() + "传来的信息:" + dataFromServer);
                            });
                        }
                        break;
                    case SocketAsyncOperation.ReceiveFrom:
                        break;
                    case SocketAsyncOperation.Send:
                        Dispatcher.BeginInvoke(() =>
                        {
                            //listboxsendText.Items.Add("客户端" + DateTime.Now.ToShortTimeString() + "发送的信息:" + ContentText.Text);
                        });

                        if (e.UserToken != null)
                        {

                            Socket socket = e.UserToken as Socket;
                            //开始接受服务器端的消息
                            bool completesAsynchronously = socket.ReceiveAsync(e);
                            //检测Socket发送是否呗挂起，如果被挂起将继续进行处理
                            if (!completesAsynchronously)
                            {
                                socketEventArg_Completed(e.UserToken, e);
                            }

                        }
                        break;
                    case SocketAsyncOperation.SendTo:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        //MessageBox.Show(exception.Message);
                        listBox1.Items.Add(ex.Message);
                        int count = listBox1.Items.Count - 1;
                        listBox1.SelectedIndex = count;
                    });
            }

        }

       
        //为了防止webclient缓存带来的问题，才去动态uri的方法，实现对数据的实时获取
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetRandomUri(string uri)
        {
            Random rd = new Random();
            string rdStr = rd.Next(10).ToString() + rd.Next(10).ToString() + rd.Next(10).ToString();
            string rdUri = uri + "&" + rdStr;
            return rdUri;
        }
        
        #endregion

        String getGpsUrl = "http://www.cloudsensing.cn:9071/WIFPa/GatewayAbstract.xml/0001000023cbd64a?lang=zh";
        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            //SendWifiData("AA1");
            //getGpsUrl = "";
            //Value v = new Value();
            //v.num = 3;
            //float f = (float)1.2;
            //v.value = f;
            //List<Value> listValue = new List<Value>();
            //listValue.Add(v);
            //SendPduData(listValue);

            WebClient wc1 = new WebClient();
            getGpsUrl = "http://www.cloudsensing.cn:9071/WIFPa/GatewayAbstract.xml/0001000023cbd64a?lang=zh";
            wc1.DownloadStringAsync(new Uri(getGpsUrl));
            wc1.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc1_DownloadStringCompleted);
        }
        void wc1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            String result = e.Result;
            Dispatcher.BeginInvoke(() =>
                {
                    listBox1.Items.Add(result);
                });
        }
        //手机直接操作小车
        private void buttonFore_Click(object sender, RoutedEventArgs e)
        {
            SendWifiData("AA1");
            listBox1.Items.Add("forward");
            int count = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = count;
        }

        private void buttonRight_Click(object sender, RoutedEventArgs e)
        {
            SendWifiData("AA4");
            listBox1.Items.Add("right");
            int count = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = count;
        }

        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            SendWifiData("AA3");
            listBox1.Items.Add("left");
            int count = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = count;
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            SendWifiData("AA5");
            listBox1.Items.Add("stop");
            int count = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = count;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            SendWifiData("AA2");
            listBox1.Items.Add("back");
            int count = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = count;
        }
    }
}
