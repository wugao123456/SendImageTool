using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dicom;
using Dicom.Network;
using Dicom.Log;
using Dicom.Imaging;
using Dicom.IO.Buffer;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Interface.Model;
namespace SendImagesTool
{
    public class CStoreSCP : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider,
           IDicomNServiceProvider
    {

        #region syntaxes
        private readonly static List<DicomTransferSyntax> DefaultAcceptedTransferSyntaxes = new List<DicomTransferSyntax>(){
				DicomTransferSyntax.ExplicitVRLittleEndian,
				DicomTransferSyntax.ExplicitVRBigEndian,
				DicomTransferSyntax.ImplicitVRLittleEndian
			};

        private readonly static List<DicomTransferSyntax> DefaultAcceptedImageTransferSyntaxes = new List<DicomTransferSyntax>() {
				// Lossless
				DicomTransferSyntax.JPEGLSLossless,
				DicomTransferSyntax.JPEG2000Lossless,
				DicomTransferSyntax.JPEGProcess14SV1,
				DicomTransferSyntax.JPEGProcess14,
				DicomTransferSyntax.RLELossless,
                DicomTransferSyntax.MPEG2 ,
				// Lossy
				DicomTransferSyntax.JPEGLSNearLossless,
				DicomTransferSyntax.JPEG2000Lossy,
				DicomTransferSyntax.JPEGProcess1,
				DicomTransferSyntax.JPEGProcess2_4,
				// Uncompressed
				DicomTransferSyntax.ExplicitVRLittleEndian,
				DicomTransferSyntax.ExplicitVRBigEndian,
				DicomTransferSyntax.ImplicitVRLittleEndian,
                //_-added by dhz 
                DicomTransferSyntax.DeflatedExplicitVRLittleEndian  
			};

        private List<DicomTransferSyntax> AcceptedTransferSyntaxes;
        private List<DicomTransferSyntax> AcceptedImageTransferSyntaxes;
        public void AddAcceptedTransferSyntaxes(DicomTransferSyntax TS)
        {
            if (!AcceptedTransferSyntaxes.Contains(TS))
            {
                AcceptedTransferSyntaxes.Add(TS);
            }
        }
        public void AddAcceptedImageTransferSyntaxes(DicomTransferSyntax TS)
        {
            if (!AcceptedImageTransferSyntaxes.Contains(TS))
            {
                AcceptedImageTransferSyntaxes.Add(TS);
            }
        }


        #endregion

        public CStoreSCP(Stream stream, Logger log)
            : base(stream, log)
        {
            AcceptedTransferSyntaxes = new List<DicomTransferSyntax>();
            AcceptedImageTransferSyntaxes = new List<DicomTransferSyntax>();
            AcceptedTransferSyntaxes.AddRange(DefaultAcceptedTransferSyntaxes);
            AcceptedImageTransferSyntaxes.AddRange(DefaultAcceptedImageTransferSyntaxes);
            //CalledAeList = new List<string>();
            //CallingAeList = new List<string>();
            //VerifyCalledAe = VerifyAeMode.AcceptAll;
            //VerifyCallingAe = VerifyAeMode.AcceptAll;
            //_aeCommparer = new AEComparte();
        } 


        public event Action<StudySendArgs> OnDicomNEevent;

        #region Dicom
        public void OnReceiveAssociationRequest(DicomAssociation association)
        {
            //if (VerifyCalledAe != VerifyAeMode.AcceptAll && !CalledAeList.Contains(association.CalledAE, _aeCommparer))
            //{
            //    SendAssociationReject(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CalledAENotRecognized);
            //    return;
            //}
            //if (VerifyCallingAe != VerifyAeMode.AcceptAll && !CallingAeList.Contains(association.CallingAE, _aeCommparer))
            //{
            //    SendAssociationReject(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CallingAENotRecognized);
            //    return;
            //}
            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification)
                {
                    pc.AcceptTransferSyntaxes(AcceptedTransferSyntaxes.ToArray());
                }
                else if (pc.AbstractSyntax == DicomUID.StorageCommitmentPushModelSOPClass)
                {
                    pc.AcceptTransferSyntaxes(AcceptedTransferSyntaxes.ToArray());
                }
                else if (pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelMOVE
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelFIND
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelFIND

                    )
                {
                    pc.AcceptTransferSyntaxes(AcceptedTransferSyntaxes.ToArray());
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    pc.AcceptTransferSyntaxes(AcceptedImageTransferSyntaxes.ToArray());
                }
            }

            SendAssociationAccept(association);
        }

        public void OnReceiveAssociationReleaseRequest()
        {
            SendAssociationReleaseResponse();
        }

        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
        }

        public void OnConnectionClosed(Exception exception)
        {
        }

        public DicomCStoreResponse OnCStoreRequest(DicomCStoreRequest request)
        {

            return new DicomCStoreResponse(request, DicomStatus.Success);
        }

        public void OnCStoreRequestException(string tempFileName, Exception e)
        {
            // let library handle logging and error response
        }

        public DicomCEchoResponse OnCEchoRequest(DicomCEchoRequest request)
        {
            return new DicomCEchoResponse(request, DicomStatus.Success);
        }

        public DicomNActionResponse OnNActionRequest(DicomNActionRequest request)
        {
            throw new NotImplementedException();
        }

        public DicomNCreateResponse OnNCreateRequest(DicomNCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public DicomNDeleteResponse OnNDeleteRequest(DicomNDeleteRequest request)
        {
            throw new NotImplementedException();
        }

        public DicomNEventReportResponse OnNEventReportRequest(DicomNEventReportRequest request)
        {
            //Console.WriteLine("OnNEventReportRequest:{0}", request.EventTypeID);

            //   string OnNEventReportRequest = request.EventTypeID.ToString();
           // string studyInstanceUId = request.Dataset.Get<string>(DicomTag.StudyInstanceUID);
             DicomSequence seq = null;

             seq = request.Dataset.Get<DicomSequence>(DicomTag.ReferencedSOPSequence);
             DicomDataset d = seq.Items.First();
             string studyInstanceUId = d.Get<string>(DicomTag.StudyInstanceUID);
             string sendState = "success";

             OnDicomNEevent.BeginInvoke(new StudySendArgs()
                 {
                     StudyInstanceUID = studyInstanceUId,
                     SendState = sendState

                 }, null, null);
            return new DicomNEventReportResponse(request, DicomStatus.Success);
        }

        public DicomNGetResponse OnNGetRequest(DicomNGetRequest request)
        {
            throw new NotImplementedException();
        }

        public DicomNSetResponse OnNSetRequest(DicomNSetRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion



    }
}
