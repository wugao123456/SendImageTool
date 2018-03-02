using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace TestDLL
{
    
    public class LogTest
    {
        public Action<LogModel> LogEvent;

        public string FileName
        {
            set;
            get;
        }
        public string FilePath
        {
            set;
            get;
        }
        public void GetLogInstace()
        {

            MessageBox.Show("nihao");
            
        }

        public void WriteLog()
        {

            
        }
       public  class LogModel
       {
           public string LogRow
           {
               set;
               get;
           }



       }


    }
}
