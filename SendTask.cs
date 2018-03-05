using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dicom;
using Dicom.Network;
using Dicom.Imaging;
using System.Threading;
using System.IO;
using Dicom.IO.Buffer;
using System.Configuration;
using Interface.Model;
using Interface;
namespace SendImagesTool
{
    public class SendTask
    {
        public Task TaskInstance;
        private bool IsRuning;
        public Action<string> NotifyChange;
        public LocalServer localserver;
        public RemoteServer remoteserver;
        public Action<DownloadItem> StudySendMessage;
       
        public SendTaskManage SendTaskManageInstance;
        public SendTask(LocalServer LS, RemoteServer RS)
        {
            remoteserver = RS;
            localserver = LS;
            SendTaskManageInstance = new SendTaskManage();
        }
        

        public bool Start(string Directory)
        {
            NotifyChange.Invoke("Sending");
            Task.Factory.StartNew<bool>(new Func<Object, bool>(ListFiles), Directory, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default
                          ).ContinueWith(t =>
                          {
                              var ok = t.Result;
                              if (NotifyChange != null)
                              {
                                  NotifyChange.Invoke(ok == true ? "Success" : "Failed");
                              }

                          }).ConfigureAwait(false);
            return true;
        }

        public bool SendStore(Object directory)
        {

             List<string> StudyInstanceList=new List<string>();
           
            var client = new DicomClient();
            string Directory = directory as string;
            var dcmFiles = System.IO.Directory.GetFiles(Directory, "*.DCM", System.IO.SearchOption.AllDirectories);
            var sopIds = new List<KeyValuePair<string, string>>();
            if (dcmFiles == null || dcmFiles.Count() == 0)
                return false;
            var first = dcmFiles.ElementAt(0);
            var fi = DicomFile.Open(first);
            string temp = fi.Dataset.Get<string>(DicomTag.StudyInstanceUID);

            foreach (var df in dcmFiles)
            {

                var file = DicomFile.Open(df);
                sopIds.Add(new KeyValuePair<string, string>(file.Dataset.Get<string>(DicomTag.SOPClassUID),
                                                            file.Dataset.Get<string>(DicomTag.SOPInstanceUID)));
                string st = file.Dataset.Get<string>(DicomTag.StudyInstanceUID);
                var dicomCStoreReQuest = new DicomCStoreRequest(df);
                dicomCStoreReQuest.OnResponseReceived = (DicomCStoreRequest request, DicomCStoreResponse response) =>
                    {
                        if(response.Status==DicomStatus.Success)
                        {
                            if(!StudyInstanceList.Exists(x=>x==st))
                            {
                                StudyInstanceList.Add(st);
                            }
                            SendTaskManageInstance.Progress(st, 1);
                        }

                    };
                client.AddRequest(dicomCStoreReQuest);
              
              //  string se = file.Dataset.Get<string>(DicomTag.SeriesInstanceUID);
            }
            
            client.Send(remoteserver.RemoteIP, remoteserver.RemotePort, false, localserver.LocalAE, remoteserver.RemoteAE);
            var cmdDataset = new DicomDataset();
            var refImgSeq = new DicomSequence(DicomTag.ReferencedSOPSequence);

            foreach (var ax in sopIds)
            {
                var rImg = new DicomDataset();
                rImg.Add(DicomTag.ReferencedSOPClassUID, ax.Key);
                rImg.Add(DicomTag.ReferencedSOPInstanceUID, ax.Value);
                refImgSeq.Items.Add(rImg);
            }
            var cmb = new DicomUIDGenerator();
            cmdDataset.Add(refImgSeq);
            cmdDataset.Add<string>(DicomTag.TransactionUID, cmb.Generate().UID);

            var nActionReq = new DicomNActionRequest(DicomUID.StorageCommitmentPushModelSOPClass, cmb.Generate(), 0x0001);
            nActionReq.Dataset = cmdDataset;
            nActionReq.OnResponseReceived = (DicomNActionRequest request, DicomNActionResponse response) =>
            {
                if(response.Status==DicomStatus.Success)
                {
                    foreach(var i in StudyInstanceList)
                    {
                        SendTaskManageInstance.DownloadSuccess(i);
                        StudyInstanceList.Remove(i);
                    }
                }
            };
            client.AddRequest(nActionReq);

            client.Send(remoteserver.RemoteIP, remoteserver.RemotePort, false, localserver.LocalAE, remoteserver.RemoteAE);

            return true;
        }
        public bool ListFiles(Object info1)
        {

            string x = info1 as string;
            var dir = new DirectoryInfo(x);
            if (!dir.Exists) return false;
            var DirInfos = dir.GetDirectories();
            ///判断有没有dcm文件
            var dcmFiles = System.IO.Directory.GetFiles(dir.FullName, "*.DCM", System.IO.SearchOption.TopDirectoryOnly);
            if (dcmFiles.GetLength(0) > 0)
            {
                SendStore(dir.FullName);
                return true;

            }

            for (int i = 0; i < DirInfos.Length; i++)
            {

                ListFiles(DirInfos[i].FullName);
            }
            Thread.Sleep(1000);
            return true;
        }


    }
}
