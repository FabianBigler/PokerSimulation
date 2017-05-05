using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Helpers
{
    public class Logger
    {
        private const string LogFileName = "log.txt";
        public static void Log(string message)
        {
            var logFilePath = Path.Combine(Environment.CurrentDirectory, LogFileName);
            using (var sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(String.Format("{0:dd.MM.yy H:mm:ss}:{1}", DateTime.Now, message));
            }
        }
    }
}
