using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Text;

namespace PhoneAppTcp
{
    public partial class MainPage : PhoneApplicationPage
    {
        String str { set;get;}
        // 构造函数
        private Socket sock;
        //建立一个Socket异步事件参数
        private SocketAsyncEventArgs socketEventArg;
        public MainPage()
        {
            InitializeComponent();
        }
        //private Socket sock;
        //private SocketAsyncEventArgs socketEventArg;
        private void send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //String host = args[0];
                //Int32 port = Convert.ToInt32(args[1]);
                //String host = "10.108.217.118";
                //String host = "10.205.3.133";
                String host = "192.168.0.136";
                Int32 port = 9900;

                Int16 iterations = 3;
                //if (args.Length == 3)
                //{
                //    iterations = Convert.ToInt16(args[2]);
                //}

                using (SocketClient sa = new SocketClient(host, port))
                {
                    sa.Connect();

                    for (Int32 i = 0; i < iterations; i++)
                    {
                        //Console.WriteLine(sa.SendReceive("Message #" + i.ToString()));
                        string t = sa.SendReceive("xpn");
                    }
                    sa.Disconnect();

                    //Console.WriteLine("Press any key to terminate the client process...");
                    //Console.Read();
                }


                using (Socket cc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPAddress ip = IPAddress.Parse(host);
                    // Instantiates the endpoint and socket.
                    //this.hostEndPoint = new IPEndPoint(addressList[addressList.Length - 1], port);
                    IPEndPoint hostEndPoint = new IPEndPoint(ip, port);

                }
            }
            catch (IndexOutOfRangeException)
            {
                //Console.WriteLine("Usage: SocketAsyncClient <host> <port> [iterations]");
            }
            catch (FormatException)
            {
                //Console.WriteLine("Usage: SocketAsyncClient <host> <port> [iterations]." +
                //    "\r\n\t<host> Name of the host to connect." +
                //    "\r\n\t<port> Numeric value for the host listening TCP port." +
                //    "\r\n\t[iterations] Number of iterations to the host.");
            }
            catch (Exception ex)
            {
                //Console.WriteLine("ERROR: " + ex.Message);
            }
            //Console.ReadKey();
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            sendwifi("mmmm");
            //string str = "xpning";
            //byte[] buffer = Encoding.UTF8.GetBytes(str);
            ////将发送内内容信息存放进Socket异步事件参数中
            //socketEventArg.SetBuffer(buffer, 0, buffer.Length);

            
        }
        void socketEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            //检测Socket是否发送出差，当前最后的一个操作

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
                        string str = get();
                        byte[] buffer = Encoding.UTF8.GetBytes(str);
                        
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
                        Socket socket = e.UserToken as Socket;

                        socket.Dispose();//释放连接
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
        void set(string s)
        {
            this.str = s;
        }
        String get()
        {
            return this.str;
        }
        void sendwifi(string s)
        {
            set(s);
            String host = "192.168.0.136";
            Int32 port = 9900;
            //String host = "192.168.0.254";
            //Int32 port = 8080;
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint hostEndPoint = new IPEndPoint(ip, port);

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //建立一个Socket异步事件参数
            socketEventArg = new SocketAsyncEventArgs();
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

                throw new SocketException((int)ex.ErrorCode);
            }
            //sock.Close();
        }
    }
}