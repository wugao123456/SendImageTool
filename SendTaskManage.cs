using Interface.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Interface
{
    public class SendTaskManage
    {
        //  public System.Collections.Concurrent.ConcurrentDictionary<string, DownloadItem> downloadItems;

        private System.Threading.ManualResetEvent mr = null;

        private System.Threading.ManualResetEventSlim slim;//主线程调用后为了判断不会第二次再进入
        public System.ComponentModel.BindingList<DownloadItem> DownLoadItemList;
        public  int  MaxDownCount;
        public SendTaskManage()
        {

            slim = new System.Threading.ManualResetEventSlim(true);
            //  bRuning = false;
            //   downloadItems = new ConcurrentDictionary<string, DownloadItem>(System.Environment.ProcessorCount * 2, 500);
            mr = new ManualResetEvent(true);
            MaxDownCount = 100;

        }


        public void Start()
        {
            try
            {
                slim.Wait();
                slim.Reset();
            }
            finally
            {
                slim.Set();
            }
        }
        /// <summary>
        /// 启动发送任务
        /// </summary>
        /// <param name="studyInstUID"></param>
        /// <param name="accepteNums"></param>
        public bool StartDownload(string studyInstUID)
        {
            var ok = false;
            try
            {

                var old = DownLoadItemList.First(x => x.StudyInstanceUID == studyInstUID);
                if (old == null || string.IsNullOrEmpty(old.StudyInstanceUID))
                    return false;
                //   var cd = DateTime.Now;
                DownloadItem di = new DownloadItem()
                {
                    Completed = 0,
                    SendStatus = E_SendStatus.Start,
                    StudyInstanceUID = studyInstUID
                };
                DownLoadItemList.Add(di);
                mr.WaitOne();
                mr.Reset();

            }
            catch (Exception e)
            {

            }
            finally
            {
                mr.Set();
            }
            return ok;
        }

        public void Progress(string studyInstUID, int completed)
        {
            try
            {
                mr.WaitOne();
                mr.Reset();
                //if (downloadItems.TryGetValue(studyInstUID, out oldItem))
                var old = DownLoadItemList.First(x => x.StudyInstanceUID == studyInstUID);
                if (old == null || string.IsNullOrEmpty(old.StudyInstanceUID))
                {
                  old = new DownloadItem()
                    {
                        Completed = 0,
                        StudyInstanceUID = studyInstUID,
                        SendStatus = E_SendStatus.Running
                    };

                  DownLoadItemList.Add(old);
                  return;
                }
                old.SendStatus = E_SendStatus.Running;
                old.Completed += 1;

            }
            catch (Exception e)
            {

            }
            finally
            {
                mr.Set();
            }
        }


        public void DownloadSuccess(string studyInstUID)
        {

            try
            {

                mr.WaitOne();
                mr.Reset();
                // DownloadItem oldItem = null;

                var old = DownLoadItemList.First(x => x.StudyInstanceUID == studyInstUID);

                if (old == null || string.IsNullOrEmpty(old.StudyInstanceUID))
                    return;
                old.SendStatus = E_SendStatus.Success;
                // if (downloadItems.TryGetValue(studyInstUID, out oldItem))
                //DownloadItem newItem = new DownloadItem()
                //{
                //    Completed = oldItem.Completed,
                //    StudyInstanceUID = studyInstUID,
                //    SendStatus = E_SendStatus.Success
                //};


            }
            catch (Exception e)
            {

            }
            finally
            {


                mr.Set();
            }
        }
        public void RemoveList()
        {

            try
            {
                if (DownLoadItemList.Count <= MaxDownCount)
                    return;

                for (int i = 0; i < DownLoadItemList.Count - MaxDownCount; i++)
                {
                    DownLoadItemList.RemoveAt(i);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                mr.Set();
            }


        }
    }
}
