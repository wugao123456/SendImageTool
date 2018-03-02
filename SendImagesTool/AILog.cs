using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using Dicom.Log;
namespace SendImagesTool
{
    public sealed class AILog
    {
        private AILog()
        {

        }
        public static readonly ILog Log = log4net.LogManager.GetLogger("logApp");

        public static readonly ILog PerfermenceLog = log4net.LogManager.GetLogger("logPerfermence");
    }

    public class AILogger : Dicom.Log.Logger
    {
        private readonly log4net.ILog _logger;

        public AILogger()
            : base()
        {
            _logger = log4net.LogManager.GetLogger("logFodicom");
        }


        /// <summary>
        /// Log a message to the _logger.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="msg">Log message (format string).</param>
        /// 
        /// 
        /// 
        /// <param name="args">Log message arguments.</param>
        public override void Log(LogLevel level, string msg, params object[] args)
        {

            var ordinalFormattedMessage = NameFormatToPositionalFormat(msg);

            switch (level)
            {
                case LogLevel.Debug:
                    this._logger.DebugFormat(ordinalFormattedMessage, args);
                    break;
                case LogLevel.Info:
                    this._logger.InfoFormat(ordinalFormattedMessage, args);
                    break;
                case LogLevel.Warning:
                    this._logger.WarnFormat(ordinalFormattedMessage, args);
                    break;
                case LogLevel.Error:
                    this._logger.ErrorFormat(ordinalFormattedMessage, args);
                    break;
                case LogLevel.Fatal:
                    this._logger.FatalFormat(ordinalFormattedMessage, args);
                    break;
                default:
                    this._logger.InfoFormat(ordinalFormattedMessage, args);
                    break;
            }
        }
    }

    public class Log4NetImplement : Dicom.Log.LogManager
    {

        public static readonly Dicom.Log.LogManager Instance = new Log4NetImplement();

        private Log4NetImplement()
            : base()
        {

        }

        protected override Logger GetLoggerImpl(string name)
        {
            return new AILogger();
        }
    }
  
}
