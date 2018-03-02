using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dicom;
using Dicom.Network;
using Dicom.Imaging;
using System.Threading;
using System.IO;
using Dicom.IO.Buffer;
using System.Net;
using System.Configuration;

using Interface;
using Interface.Model;
namespace SendImagesTool
{
    public partial class   Form1 : Form
    {
        private int SendState;
        private IDicomServerHandle mStorageHandle = null;
        Type CreaateObject;
        SendTask sendtaskinstance;
        private Action<StudySendArgs> OnStudyState;
        private List<StudySendArgs> StudyList;
        public Form1()
        {
            InitializeComponent();
            GetLocalServer();
            sendtaskinstance = new SendTask(localserver, remoteserver);
            sendtaskinstance.NotifyChange += OnNotifyChange;
            sendtaskinstance.StudySendMessage += OnReceiveDicomMessage;

          StudyList = new List<StudySendArgs>();
       
          StudyTableDGV.DataSource = StudyList;//new BindingList<StudySendArgs>(StudyList);
            

        }
        private void OnReceiveDicomMessage(Object sender)
        {
            try
            {
               
                this.BeginInvoke(new Action(() =>
                {
                    StudyTableDGV.DataSource = null;
                    StudyList.RemoveAll(j => (j.SendState == "Success"));
                    //foreach (var i in StudyList)
                    //{
                    //    if (i.SendState == "Success")
                    //        StudyList.Remove(i);
                    //}
                    StudyList.Add((StudySendArgs)sender);
                    StudyTableDGV.DataSource = StudyList;
                 

                }));

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
          
        }
        private void SetText(bool textbool)
        {
            //  foreach(var i in this.)

        }
        private void OnNotifyChange(string str)
        {

            //  SendStatusInfo.Text = str;
            this.BeginInvoke((Action)(() => SendStatusInfo.Text = str));
         }
        private LocalServer localserver = new LocalServer();

        private RemoteServer remoteserver = new RemoteServer();
        private void GetLocalServer()
        {
            LocalIPText.Text = localserver.LocalIP;
            LocalPortText.Value = Convert.ToInt32(localserver.LocalPort);
            LocalAEText.Text = localserver.LocalAE;
            RemoteAETitle.Text = remoteserver.RemoteAE;
            RemotePortText.Value = Convert.ToInt32(remoteserver.RemotePort);
            RemoteIPText.Text = remoteserver.RemoteIP;
        }
        private void SetLocalServer()
        {
            localserver.LocalIP = LocalIPText.Text;
            localserver.LocalPort = Convert.ToInt16(LocalPortText.Value);
            localserver.LocalAE = LocalAEText.Text;
            remoteserver.RemoteAE = RemoteAETitle.Text;
            remoteserver.RemotePort = Convert.ToInt16(RemotePortText.Value);
            RemoteIPText.Text = remoteserver.RemoteIP;
            localserver.SetServer();
            remoteserver.SetServer();
        }

        private void CallBackEcho(DicomCEchoRequest creq, DicomCEchoResponse cres)
        {


            if (cres.Status == Dicom.Network.DicomStatus.Success)
            {
                MessageBox.Show("Pacs 在线");

            }
            else
            {
                MessageBox.Show("Pacs 不在线");
            }

        }
        private void Ping_Button_Click(object sender, EventArgs e)
        {
            try
            {
                DicomCEchoRequest req = new DicomCEchoRequest();
                req.OnResponseReceived += CallBackEcho;
                var client = new DicomClient();
                client.AddRequest(req);
                client.Send(remoteserver.RemoteIP, remoteserver.RemotePort, false, localserver.LocalAE, remoteserver.RemoteAE);

            }
            catch (Exception ex)
            {
                MessageBox.Show("链接不到对方主机");
            }


        }

        private void Sure_Button_Click(object sender, EventArgs e)
        {
            if (Sure_Button.Text == "修改")
            {
                //是修改 所有按键变可修改
                foreach (var i in this.Controls)
                {

                    if (i is TextBox)
                    {
                        TextBox x = i as TextBox;
                        x.ReadOnly = false;
                        x.Enabled = true;
                    }
                    else if (i is NumericUpDown)
                    {
                        NumericUpDown x = i as NumericUpDown;
                        x.ReadOnly = false;
                        x.Enabled = true;

                    }
                }

                Sure_Button.Text = "确认";
                return;
            }
            // 所有按键变灰 设置了

            foreach (var i in this.Controls)
            {
                if (i is TextBox)
                {
                    TextBox x = i as TextBox;
                    if (x.Name == "FileTextPath")
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(x.Text))
                    {
                        MessageBox.Show("所有的信息必须填");
                        return;
                    }

                }

            }

            foreach (var i in this.Controls)
            {

                if (i is TextBox)
                {

                    TextBox x = i as TextBox;
                    x.ReadOnly = true;
                    x.Enabled = false;

                }
                else if (i is NumericUpDown)
                {
                    NumericUpDown x = i as NumericUpDown;
                    x.ReadOnly = true;
                    x.Enabled = false;

                }

            }
            SetLocalServer();
            GetLocalServer();
            Sure_Button.Text = "修改";

        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {

            OnNotifyChange("Waiting Sending");
            //OpenFileDialog dialog = new OpenFileDialog();
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.FilePathText.Text = dialog.SelectedPath;
            }

        }

        private void Send_Button_Click(object sender, EventArgs e)
        {
            if (Sure_Button.Text == "确认")
            {

                MessageBox.Show("请按确认键");
                return;
            }
            if (string.IsNullOrEmpty(FilePathText.Text))
            {
                MessageBox.Show("请填图片路径");

            }

            // sendtaskinstance = new SendTask();
            sendtaskinstance.Start(FilePathText.Text);

        }

        private void Log_button_Click(object sender, EventArgs e)
        {


            System.Diagnostics.Process.Start(@"d:\a.txt");
        }
        
        private const int WM_ACTIVATEAPP = 0x001C;
        private bool appActive = true;
        private string MsgString = null;
        //protected override void WndProc(ref Message m)
        //{

        //    switch (m.Msg)
        //    {
        //        // The WM_ACTIVATEAPP message occurs when the application
        //        // becomes the active application or becomes inactive.
        //        case WM_ACTIVATEAPP:



        //            // The WParam value identifies what is occurring.
        //            appActive = (((int)m.WParam != 0));

        //            // Invalidate to get new text painted.
        //            this.Invalidate();

        //            break;
        //    }
        //    MsgString += m.Msg.ToString() + " ";
        //    base.WndProc(ref m);

        //}

        private void button1_Click(object sender, EventArgs e)
        {

            var s = MsgString;
        }



        #region Event
      
        #endregion



       
    }
}
