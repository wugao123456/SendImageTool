using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{

    public enum E_ServerState
    {
        NotRunning = 0,
        Running = 1,
        Stopped = 2,
    }

    public interface IDicomServerHandle
    {
        E_ServerState ServerState { get; }
        event Action<E_ServerState> ServerStateChanged;
        void Start(int timeout = 5000); 
        void Stop();
        Exception error { get; }
    }
}
