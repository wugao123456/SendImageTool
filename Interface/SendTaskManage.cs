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
        public System.Collections.Concurrent.ConcurrentDictionary<string, DownloadItem> downloadItems;
        
        private System.Threading.ManualResetEvent mr = null;

        private System.Threading.ManualResetEventSlim slim;//主线程调用后为了判断不会第二次再进入

        public SendTaskManage()
        {

            slim = new System.Threading.ManualResetEventSlim(true);
          //  bRuning = false;
            downloadItems = new ConcurrentDictionary<string, DownloadItem>(System.Environment.ProcessorCount * 2, 500);
            mr = new ManualResetEvent(true);

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

                var cd = DateTime.Now;

                DownloadItem di = new DownloadItem()
                {
                    Completed = 0,
                    SendStatus=E_SendStatus.Start,
                    StudyInstanceUID=studyInstUID
                  //  Completed = 0,
                   // Fails = 0,
                   // Warnings = 0,
                  //  AddTime = cd,
                    //TokenTime = cd,
                 //   PatientId = patientId,
                  //  Remaings = 0


                };
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

                DownloadItem oldItem = null;

                if (downloadItems.TryGetValue(studyInstUID, out oldItem))
                {
                    
                    DownloadItem newItem = new DownloadItem()
                    {
                        Completed = oldItem.Completed+1,
                    };
                }
                else
                {
                  
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


        public void DownloadSuccess(string studyInstUID, int completed, int remaings, int fails, int warnings)
        {

            try
            {

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



        }
    }
}
