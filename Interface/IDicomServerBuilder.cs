using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Interface
{
  public  interface IDicomServerBuilder
    {

       //event Action<string> OnDicomServerMessage; 
      //  void DicomFileSaved(object sender, DicomFileSaveArgs arg);

        IDicomServerHandle StartCStore(int port, string CalledAe);

        //void DicomCommandHandler(object sender, DicomCommandArgs cmdSet);
      
       // IDicomServerHandle StartCCommand(int port, string CalledAe, string[] callingAes);

    }
}
