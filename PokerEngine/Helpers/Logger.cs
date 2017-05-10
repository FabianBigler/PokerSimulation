using PokerEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PokerEngine.Helpers
{
    public class Logger : ILogger
    {
        private const string LogFileName = "log.txt";
               
        public void Log(string message)
        {
            var logFilePath = Path.Combine(HttpContext.Current.Server.MapPath("~"), LogFileName);            

            using (var sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(String.Format("{0:dd.MM.yy H:mm:ss}:{1}", DateTime.Now, message));
            }
        }

        public void LogException(Exception ex)
        {
            this.Log(ex.ToString());
        }
    }
}
