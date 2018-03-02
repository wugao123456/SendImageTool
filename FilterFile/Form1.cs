using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using Dicom;
using Dicom.Network;
using Dicom.Imaging;
using System.Threading;
namespace FilterFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           sm = new List<studymodel>();
            ReadExtable(@"C:\Users\Administrator\Desktop\excel\normal.xls");
        }
        List<studymodel> sm;
        public class studymodel
        {
            public string patientid
            {
                set;
                get;
            }
            public string stutydate
            {
                set;
                get;
            }
        }
        private string studyInstanceUID;
        private string patientid;
        private string studytime;
        

        private void button1_Click(object sender, EventArgs e)
        {

            Task.Factory.StartNew<bool>(new Func<Object, bool>(ListFiles), @"\\192.168.198.220\Share", CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default
                          ).ContinueWith(t =>
                          {
                              var ok = t.Result;
                              MessageBox.Show("OK");

                          }).ConfigureAwait(false);


        }
        public bool ListFiles(Object info1)
        {

            string x = info1 as string;
            var dir = new DirectoryInfo(x);
            if (!dir.Exists) return false;
            var DirInfos = dir.GetDirectories();
            foreach(var studyfile in DirInfos)
            {

               // var seriesfiles = studyfile.GetDirectories();
                //if (seriesfiles.Length <= 0)
                //    break;
                //int Max=0; DirectoryInfo  series=null;
                //foreach (var seriesfile  in seriesfiles)
                //{
                //    var dcmFiles = System.IO.Directory.GetFiles(seriesfile.FullName, "*.DCM", System.IO.SearchOption.TopDirectoryOnly);
                //    if(dcmFiles.Length>Max)
                //    {
                //        Max = dcmFiles.Length; series = seriesfile;
                //    }
                //}
                var dcm = System.IO.Directory.GetFiles(studyfile.FullName, "*.DCM", System.IO.SearchOption.AllDirectories);
                if (dcm.Count() <= 0)
                    continue;
                //var sopIds = new List<KeyValuePair<string, string>>();
                var file = DicomFile.Open(dcm[0]);
                studyInstanceUID = file.Dataset.Get<string>(DicomTag.StudyInstanceUID);
                string se = file.Dataset.Get<string>(DicomTag.SeriesInstanceUID);
                studytime = file.Dataset.Get<string>(DicomTag.StudyDate);
                 patientid = file.Dataset.Get<string>(DicomTag.PatientID);

                //if (patientid == "11253780" || patientid == "11353824" ||patientid=="1195967"||patientid=="11088118" || patientid=="11304033")
                 if (IsExist(patientid,studytime))
                {
                    foreach (var df in dcm)
                    {
                        var fi = DicomFile.Open(df);
                        string sop = fi.Dataset.Get<string>(DicomTag.SOPInstanceUID);
                        var di = System.IO.Path.Combine(@"D:\shengzhongtu\More", patientid, studyInstanceUID, se);
                        var saveTo = System.IO.Path.Combine(di, sop + ".DCM");

                        var dirt = System.IO.Path.GetDirectoryName(saveTo);
                        if (!System.IO.Directory.Exists(dirt))
                        {
                            System.IO.Directory.CreateDirectory(dirt);
                        }
                        fi.Save(saveTo);
                        //  request.File.Save(saveTo);//---有可能无权写入或是磁盘空间不足导入写入失败 
                    }


                }

               
            }
          
            return true;
        }
        public bool ReadExtable(string filename)
        {
            object Nothing = System.Reflection.Missing.Value;
            var app = new Excel.Application();
            app.Visible = false;
            app.DisplayAlerts = false;
            string sheetname = filename;
            Excel.Workbook workBook = app.Workbooks.Open(filename, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);
            //for (int i = 0; i < app.Workbooks[1].Worksheets.Count; i++)
            {
                Excel.Worksheet worksheet = (Excel.Worksheet)app.Workbooks[1].Worksheets[3];
                
               string id = ((Excel.Range)worksheet.Cells[2, 2]).Text.ToString();
               string time = ((Excel.Range)worksheet.Cells[2, 1]).Text.ToString();
               time=time.Replace("\\","");
               int i = 3;
                   while(true)
                   {
                       sm.Add(new studymodel()
                           {
                               patientid = id,
                               stutydate = time
                           });
                       //if(patientid==id && time==studytime)
                       //{
                       //    return true;
                       //}
                       id = ((Excel.Range)worksheet.Cells[i, 2]).Text.ToString();
                       time = ((Excel.Range)worksheet.Cells[i, 1]).Text.ToString();
                       i++;
                       time=time.Replace("\\","");
                       if(string.IsNullOrEmpty(id))
                           break;
                   }
                 

            }
        
            return false;
        }


        bool IsExist(string st,string date)
        {

            foreach(var i in sm)
            {
                if(i.patientid==st)
                {
                    return true;
                }

            }

            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Task.Factory.StartNew<bool>(new Func<Object, bool>(TestBone), @"F:\Boneold\Images", CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default
                         ).ContinueWith(t =>
                         {
                             var ok = t.Result;

                         }).ConfigureAwait(false);

        }


        public bool TestBone(Object info1)
        {

            string x = info1 as string;
            var dir = new DirectoryInfo(x);
            if (!dir.Exists) return false;
            var DirInfos = dir.GetDirectories();
            foreach (var studyfile in DirInfos)
            {

                var seriesfiles = studyfile.GetDirectories();
                if (seriesfiles.Length <= 0)
                    break;
                DirectoryInfo series = null;
                foreach (var seriesfile in seriesfiles)
                {
                    var dcmFiles = System.IO.Directory.GetFiles(seriesfile.FullName, "*.DCM", System.IO.SearchOption.TopDirectoryOnly);
                    if (dcmFiles.Length >1)
                    {
                        continue;
                    }
                    var file = DicomFile.Open(dcmFiles[0]);
                    string st = file.Dataset.Get<string>(DicomTag.StudyInstanceUID);
                    string se = file.Dataset.Get<string>(DicomTag.SeriesInstanceUID);
                 
                    if(file.Dataset.Get<string>(DicomTag.BodyPartExamined)=="HAND")
                    {
                       
                        string sop = file.Dataset.Get<string>(DicomTag.SOPInstanceUID);
                        var di = System.IO.Path.Combine(@"F:\bone", st, se);
                        var saveTo = System.IO.Path.Combine(di, sop + ".DCM");

                        var dirt = System.IO.Path.GetDirectoryName(saveTo);
                        if (!System.IO.Directory.Exists(dirt))
                        {
                            System.IO.Directory.CreateDirectory(dirt);
                        }
                        file.Save(saveTo);
                    }
                   
                }
             

             

            }

            return true;
        }
    }
}
