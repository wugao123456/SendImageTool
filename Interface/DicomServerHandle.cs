using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dicom;
using Dicom.Network;
using Interface;

namespace SendImagesTool
{
    public class DicomServerHandle<T> : IDicomServerHandle where T : DicomService, IDicomServiceProvider
    {
        JpDicomServer<T> _server;

        private E_ServerState _state;

        public E_ServerState ServerState
        {
            get
            {
                return _state;
            }
            protected set
            {
                _state = value;
                if (ServerStateChanged != null)
                {
                    ServerStateChanged.BeginInvoke(value, null, null);
                }
            }
        }

        public event Action<E_ServerState> ServerStateChanged;
        
        public Exception error
        {
            get
            {
                return _server.Exception;
            }
        }

        public DicomServerHandle(JpDicomServer<T> server)
        {
            _server = server;
            ServerState = E_ServerState.NotRunning;
        }

        public void Start(int timeout = 5000)
        {
            if (timeout <= 0)
            {
                timeout = 5000;
            }
            _server.Start(timeout);
            if (_server.IsListening)
            {
                ServerState = E_ServerState.Running;
            }
            else
            {
                Stop();
            }
        }
        public void Stop()
        {

            _server.Stop();
            ServerState = E_ServerState.Stopped;
        }

    }
}
