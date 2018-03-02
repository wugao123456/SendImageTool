﻿// Copyright (c) 2012-2015 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

namespace Dicom
{
    using Dicom.Network;
    using System;

    public class DicomFileMetaInformation : DicomDataset
    {
        public DicomFileMetaInformation()
        {
            Version = new byte[] { 0x00, 0x01 };
            ImplementationClassUID = DicomImplementation.ClassUID;
            ImplementationVersionName = DicomImplementation.Version;

            var machine = NetworkManager.MachineName;
            if (machine.Length > 16) machine = machine.Substring(0, 16);
            SourceApplicationEntityTitle = machine;
        }

        public DicomFileMetaInformation(DicomDataset dataset)
            : this()
        {
            MediaStorageSOPClassUID = dataset.Get<DicomUID>(DicomTag.SOPClassUID);
            //部分工作站含有DicomTag.SOPInstanceUID但是值为null 需要重新生成
            if (dataset.Contains(DicomTag.SOPInstanceUID) && !string.IsNullOrEmpty(dataset.Get<string>(DicomTag.SOPInstanceUID)))
            {
                MediaStorageSOPInstanceUID = dataset.Get<DicomUID>(DicomTag.SOPInstanceUID);
            }
            else
            {
                DicomUIDGenerator uid = new DicomUIDGenerator();
                dataset.Add<string>(DicomTag.SOPInstanceUID, uid.Generate().ToString());
                MediaStorageSOPInstanceUID = dataset.Get<DicomUID>(DicomTag.SOPInstanceUID);
            }
            TransferSyntax = dataset.InternalTransferSyntax;
        }

        public byte[] Version
        {
            get
            {
                return Get<byte[]>(DicomTag.FileMetaInformationVersion);
            }
            set
            {
                Add(DicomTag.FileMetaInformationVersion, value);
            }
        }

        public DicomUID MediaStorageSOPClassUID
        {
            get
            {
                return Get<DicomUID>(DicomTag.MediaStorageSOPClassUID);
            }
            set
            {
                Add(DicomTag.MediaStorageSOPClassUID, value);
            }
        }

        public DicomUID MediaStorageSOPInstanceUID
        {
            get
            {
                return Get<DicomUID>(DicomTag.MediaStorageSOPInstanceUID);
            }
            set
            {
                Add(DicomTag.MediaStorageSOPInstanceUID, value);
            }
        }

        public DicomTransferSyntax TransferSyntax
        {
            get
            {
                return Get<DicomTransferSyntax>(DicomTag.TransferSyntaxUID);
            }
            set
            {
                Add(DicomTag.TransferSyntaxUID, value.UID);
            }
        }

        public DicomUID ImplementationClassUID
        {
            get
            {
                return Get<DicomUID>(DicomTag.ImplementationClassUID);
            }
            set
            {
                Add(DicomTag.ImplementationClassUID, value);
            }
        }

        public string ImplementationVersionName
        {
            get
            {
                return Get<string>(DicomTag.ImplementationVersionName);
            }
            set
            {
                Add(DicomTag.ImplementationVersionName, value);
            }
        }

        public string SourceApplicationEntityTitle
        {
            get
            {
                return Get<string>(DicomTag.SourceApplicationEntityTitle);
            }
            set
            {
                Add(DicomTag.SourceApplicationEntityTitle, value);
            }
        }

        public override string ToString()
        {
            return "DICOM File Meta Info";
        }
    }
}