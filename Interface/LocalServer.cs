using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace SendImagesTool
{
    public class LocalServer
    {
        public LocalServer()
        {
            GetServer();
        }
       public string  LocalIP
        {
            set;
            get;
        }
       public int LocalPort
        {
            set;
            get;
        }
       public  string LocalAE
        {
            set;
            get;
        }
         
        public void  GetServer()
        {
            this.LocalAE = ConfigurationManager.AppSettings["LocalAE"];
            this.LocalIP = ConfigurationManager.AppSettings["LocalIP"];
            this.LocalPort = Convert.ToInt16(ConfigurationManager.AppSettings["LocalPort"]);

        }
        public void SetServer()
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["LocalAE"].Value = this.LocalAE;
            config.AppSettings.Settings["LocalIP"].Value = this.LocalIP;
            config.AppSettings.Settings["LocalPort"].Value = this.LocalPort.ToString();
           

        }

    }
    public class RemoteServer
    {
        public RemoteServer()
        {
            GetServer(); ;
        }
        
        
        public  string RemoteIP
        {
            set;
            get;
        }
        public int RemotePort
        {
            set;
            get;
        }
        public string RemoteAE
        {
            set;
            get;
        }
        public void GetServer()
        {
            this.RemoteAE = ConfigurationManager.AppSettings["AE"];
            this.RemoteIP = ConfigurationManager.AppSettings["IP"];
            this.RemotePort = Convert.ToInt16(ConfigurationManager.AppSettings["Port"]);
        }
        public void SetServer()
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["AE"].Value = this.RemoteAE;
            config.AppSettings.Settings["IP"].Value = this.RemoteIP;
            config.AppSettings.Settings["Port"].Value = this.RemotePort.ToString();
           
        }
    }
   public  enum SendState
    {
        Waiting=0,
        Sending=1,
        End=2,
        Expert=3
    }
    
}
