using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Model
{


    public   enum E_SendStatus:int
    {
        Start=0,
        Running=1,
        Success=2,
        Exception=3
    }
  
   
    public class DownloadItem : IEqualityComparer<DownloadItem>, INotifyPropertyChanged
    {

         public string StudyInstanceUID { get; set; }
         public int Completed { get; set; }

         public E_SendStatus SendStatus
         {
             set;
             get;
         }
        public bool Equals(DownloadItem x, DownloadItem y)
        {
            return (x.StudyInstanceUID == y.StudyInstanceUID  );
        }

        public int GetHashCode(DownloadItem obj)
        {
            return obj.StudyInstanceUID.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }

        }
    }


   
    //public class StudySendArgs
    //{
    //    public StudySendArgs(int count)
    //    {
    //        Index = count;
    //    }
    //    public int Index { get; set; }
    //    public string StudyInstanceUID { get; set; }
    //    public int Completed
    //    {
    //        set;
    //        get;
    //    }

    //    public E_SendStatus SendStatus
    //    {
    //        set;
    //        get;
    //    }

    //}

}
