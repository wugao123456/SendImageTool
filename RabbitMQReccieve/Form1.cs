using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ.Client;

namespace RabbitMQReccieve
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "";
            factory.Port = 110;
            factory.UserName = "";
            factory.Password = "";
            using (IConnection conn = factory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //在MQ上定义一个持久化队列，如果名称相同不会重复创建
                    channel.QueueDeclare("MyFirstQueue", true, false, false, null);
                    while (true)
                    //{
                    //   // string customStr = Console.ReadLine();
                    //   // RequestMsg requestMsg = new RequestMsg();
                    // //   requestMsg.Name = string.Format("Name_{0}", customStr);
                    //  //  requestMsg.Code = string.Format("Code_{0}", customStr);
                    //    string jsonStr = JsonConvert.SerializeObject(requestMsg);
                    //    byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);

                    //    //设置消息持久化
                    //    IBasicProperties properties = channel.CreateBasicProperties();
                    //    properties.DeliveryMode = 2;
                    //    channel.BasicPublish("", "MyFirstQueue", properties, bytes);

                        //channel.BasicPublish("", "MyFirstQueue", null, bytes);

                     //   Console.WriteLine("消息已发送：" + requestMsg.ToString());
                    }
                }
            }
        }
    }
}
