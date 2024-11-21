using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TEPL.QDMS.WindowsService.Log
{
    public static class LoggerBlock
    {
        public static void WriteTraceLog(Exception ex)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.EventId = 100;
            logEntry.Priority = 2;
            logEntry.Message = ex.Message;
            Logger.Write(logEntry);
            //  LogWriter.Write(message, "General", 5, 2000, TraceEventType.Error);
        }
        public static void WriteLog(string strMessage)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.EventId = 100;
            logEntry.Priority = 2;
            logEntry.Message = strMessage;
            Logger.Write(logEntry);
        }
    }
}
