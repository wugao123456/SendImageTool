using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace Interface
{
   public class SocketTest
    {
        #region socket
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // //当点击开始监听的时候 在服务器端创建一个负责监IP地址跟端口号的Socket  
                // Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // IPAddress ip = IPAddress.Any;//IPAddress.Parse(txtServer.Text);  
                // //创建端口号对象  
                // IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(Port.Text));
                // //监听  
                // socketWatch.Bind(point);
                //// ShowMsg("监听成功");  
                // MessageBox.Show("监听成功");
                // socketWatch.Listen(10);

                // Thread th = new Thread(Listen);
                // th.IsBackground = true;
                // th.Start(socketWatch);
            }
            catch (Exception er)
            {
            }

        }

        /// <summary>
        /// 等待客户端的连接 并且创建与之通信用的Socket
        /// </summary>
        /// 
        Socket socketSend;
        void Listen(object o)
        {
            //    Socket socketWatch = o as Socket;
            //    //等待客户端的连接 并且创建一个负责通信的Socket
            //    while (true)
            //    {
            //        try
            //        {
            //            //负责跟客户端通信的Socket
            //            socketSend = socketWatch.Accept();
            //            //将远程连接的客户端的ip地址和socket存入集合中
            //            dicSocket.Add(socketSend.RemoteEndPoint.ToString(), socketSend);
            //            //将远程连接的客户端的IP地址和端口号存储下拉框中
            //            // cboUsers.Items.Add(socketSend.RemoteEndPoint.ToString());
            //            this.BeginInvoke(new Action(() => { RemoteTextIP.Text += socketSend.RemoteEndPoint.ToString(); }));
            //            //192.168.11.78：连接成功
            //            // ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功");
            //            //开启 一个新线程不停的接受客户端发送过来的消息
            //            Thread th = new Thread(Recive);
            //            th.IsBackground = true;
            //            th.Start(socketSend);
            //        }
            //        catch
            //        { }
            //    }
        }

        void Recive(object o)
        {
            Socket socketSend = o as Socket;
            while (true)
            {
                try
                {
                    //客户端连接成功后，服务器应该接受客户端发来的消息  
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    //实际接受到的有效字节数  
                    int r = socketSend.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    //  string str = Encoding.UTF8.GetString(buffer, 0, r);
                    // ShowMsg(socketSend.RemoteEndPoint + ":" + str);  
                }
                catch
                { }
            }
        }
        #endregion

    }
}
