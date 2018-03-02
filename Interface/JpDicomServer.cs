using Dicom.Log;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Interface
{
    /// <summary>
    /// DICOM server class.
    /// 功能等同于Dicom.Network.DicomServer<T>
    /// 更改的部分有：
    /// 1.构造函数不再自动启动服务，新增start函数用于启动
    /// 2.构造T类型的scp服务后，引发ScpCreated事件，用于外部进行一些特定设置
    /// </summary>
    /// <typeparam name="T">DICOM service that the server should manage.</typeparam>
    public class JpDicomServer<T> : IDisposable
        where T : DicomService, IDicomServiceProvider
    {
        #region EVENTS

        /// <summary>
       
        /// </summary>
        public event Action<T> ScpCreated;

        #endregion
        #region FIELDS

        private bool disposed;

        private readonly int port;

        private readonly string certificateName;

        private CancellationTokenSource cancellationSource;

        private readonly List<T> clients;

        private ManualResetEvent WaitHandler = new ManualResetEvent(false);

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Initializes an instance of <see cref="DicomServer{T}"/>, that starts listening for connections in the background.
        /// </summary>
        /// <param name="port">Port to listen to.</param>
        /// <param name="certificateName">Certificate name for authenticated connections.</param>
        /// <param name="options">Service options.</param>
        /// <param name="logger">Logger.</param>
        public JpDicomServer(int port, string certificateName = null, DicomServiceOptions options = null, Logger logger = null)
        {
            this.port = port;
            this.certificateName = certificateName;
            this.clients = new List<T>();

            this.Options = options;
            this.Logger = logger ?? LogManager.GetLogger("Dicom.Network");
            this.IsListening = false;
            this.Exception = null;
            this.disposed = false;
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gets the logger used by <see cref="DicomServer{T}"/>
        /// </summary>
        public Logger Logger { get; private set; }

        /// <summary>
        /// Gets the options to control behavior of <see cref="DicomService"/> base class.
        /// </summary>
        public DicomServiceOptions Options { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the server is actively listening for client connections.
        /// </summary>
        public bool IsListening { get; private set; }

        /// <summary>
        /// Gets the exception that was thrown if the server failed to listen.
        /// </summary>
        public Exception Exception { get; private set; }

        #endregion

        #region METHODS

        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start(int timeout)
        {
            if (IsListening) { return; }
            WaitHandler.Reset();
            this.cancellationSource = new CancellationTokenSource();
            Task.Factory.StartNew(
                this.OnTimerTickAsync,
                this.cancellationSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            Task.Factory.StartNew(
                this.ListenAsync,
                this.cancellationSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            WaitHandler.WaitOne(timeout);
        }
        /// <summary>
        /// Stop server from further listening.
        /// </summary>
        public void Stop()
        {

            if (this.cancellationSource != null && !this.cancellationSource.IsCancellationRequested)
            {
                this.cancellationSource.Cancel();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Execute the disposal.
        /// </summary>
        /// <param name="disposing">True if called from <see cref="Dispose"/>, false otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Stop();
                this.cancellationSource.Dispose();
                this.clients.Clear();
            }

            this.disposed = true;
        }

        /// <summary>
        /// Create an instance of the DICOM service class.
        /// </summary>
        /// <param name="stream">Network stream.</param>
        /// <returns>An instance of the DICOM service class.</returns>
        protected virtual T CreateScp(Stream stream)
        {
            var scp = (T)Activator.CreateInstance(typeof(T), stream, this.Logger);
            if (ScpCreated != null)
            {
                ScpCreated(scp);
            }
            return scp;
        }

        /// <summary>
        /// Listen indefinitely for network connections on the specified port.
        /// </summary>
        private async void ListenAsync()
        {
            try
            {
                var noDelay = this.Options != null ? this.Options.TcpNoDelay : DicomServiceOptions.Default.TcpNoDelay;

                var listener = NetworkManager.CreateNetworkListener(this.port);


                ///---
                //----The process was terminated due to an unhandled exception. Exception Info: System.NullReferenceException Stack: 
                //at System.Runtime.CompilerServices.AsyncMethodBuilderCore.<ThrowAsync>b__1(System.Object) at System.Threading.QueueUserWorkItemCallback.WaitCallback_Context(System.Object) 
                //at System.Threading.ExecutionContext.RunInternal(System.Threading.ExecutionContext, System.Threading.ContextCallback, System.Object, Boolean) 
                //at System.Threading.ExecutionContext.Run(System.Threading.ExecutionContext, System.Threading.ContextCallback, System.Object, Boolean) 
                //at System.Threading.QueueUserWorkItemCallback.System.Threading.IThreadPoolWorkItem.ExecuteWorkItem() 
                //at System.Threading.ThreadPoolWorkQueue.Dispatch() 
                //at System.Threading._ThreadPoolWaitCallback.PerformWaitCallback()  
                //------------http://stackoverflow.com/questions/16056016/nullreferenceexception-in-system-threading-tasks-calling-httpclient-getasyncurl

                //---原始代码  BEGIN
                //await listener.StartAsync().ConfigureAwait(false);
                //---原始代码  END

                //----Bug Fixed  BEGIN
#pragma warning disable 4014 // Fire and forget.



                var task = Task.Run(async () =>
                {
                    await listener.StartAsync().ConfigureAwait(false);
                });

                Task.WaitAll(new Task[] { task });



                //---Bug  Fixed  END 


                this.IsListening = true;
                WaitHandler.Set();
                while (!this.cancellationSource.IsCancellationRequested)
                {
                    var networkStream =
                        await
                        listener.AcceptNetworkStreamAsync(this.certificateName, noDelay, this.cancellationSource.Token)
                            .ConfigureAwait(false);

                    if (networkStream != null)
                    {
                        var scp = this.CreateScp(networkStream.AsStream());
                        if (this.Options != null)
                        {
                            scp.Options = this.Options;
                        }

                        this.clients.Add(scp);
                    }
                }

                listener.Stop();
                this.IsListening = false;
                this.Exception = null;
            }
            catch (OperationCanceledException)
            {
                this.Logger.Info("Listening manually terminated");

                this.IsListening = false;
                this.Exception = null;
            }
            catch (Exception e)
            {
                this.Logger.Error("Exception listening for clients, {@error}", e);

                this.Stop();
                this.IsListening = false;
                this.Exception = e;
            }
            finally
            {
                WaitHandler.Set();
            }
        }

        /// <summary>
        /// Remove no longer used client connections.
        /// </summary>
        private async void OnTimerTickAsync()
        {
            while (!this.cancellationSource.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(1000, this.cancellationSource.Token).ConfigureAwait(false);
                    this.clients.RemoveAll(client => !client.IsConnected);
                }
                catch (OperationCanceledException)
                {
                    this.Logger.Info("Disconnected client cleanup manually terminated.");
                }
                catch (Exception e)
                {
                    this.Logger.Warn("Exception removing disconnected clients, {@error}", e);
                }
            }
        }

        #endregion
    }
}
