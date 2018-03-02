using Dicom.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dicom;
using SendImagesTool;

namespace Interface
{
    public class DicomServerBuilder : IDicomServerBuilder
    {
        public DicomServerBuilder()
        {

        }

       public  Action<string> OnDicomServerMessage;
        private void DicomServerMessage(string sender)
        {

            if (OnDicomServerMessage != null)
                OnDicomServerMessage.BeginInvoke(sender,null,null);

        }
        public IDicomServerHandle StartCStore(int port, string CalledAe)
        {
            var server = new JpDicomServer<CStoreSCP>(port);
            server.ScpCreated += (scp) =>
            {
                //scp.DicomStorage = DbDicomStorage;
                //scp.FileStorage = StorageFileProvider;
                //scp.PSSIQuery = DbPSSIQuery;
                //scp.WorkModel = workModel;
                //scp.DownloadMgr = DownloadMgr;

                //scp.InstallDicomFileSavehandler(new EventHandler<DicomFileSaveArgs>(DicomFileSaved));
                //scp.Cache = DbCache;
                //scp.Notify = NotifyBgService;
               // scp.AddCalledAe(CalledAe);
                //foreach (var cc in callingAes)
                //{
                //    scp.AddCallingAe(cc);
                //}
                //scp.VerifyCalledAe = VerifyAeMode.AcceptList;
                //scp.VerifyCallingAe = VerifyAeMode.AcceptList;
              //  scp.OnDicomNEevent += DicomServerMessage;
               
            };
            return new DicomServerHandle<CStoreSCP>(server);
        }
    }
}
