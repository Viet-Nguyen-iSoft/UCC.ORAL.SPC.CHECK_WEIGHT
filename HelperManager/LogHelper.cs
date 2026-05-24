using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HelperManager
{
  public static class LogHelper
  {
    private static readonly object _lock = new object();
    public static void LogErrorToFileLog(Exception exception, string folderFileLog)
    {
      //DateTime dateTime = DateTime.Now;
      //string fileName = $"Filelog_{DateTime.Now.ToString("yyyyMMdd")}" + ".txt";
      //string nameFileLog = Path.Combine(folderFileLog, fileName);
      //if (!File.Exists(nameFileLog))
      //{
      //  File.Create(nameFileLog);
      //}
      //string contentLog = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\r\n" + $"Content: {exception.Message} && {exception.StackTrace} \r\n";
      //File.AppendAllText(nameFileLog, contentLog);

      lock (_lock)
      {
        string fileName = $"Filelog_{DateTime.Now:yyyyMMdd}.txt";
        string nameFileLog = Path.Combine(folderFileLog, fileName);

        string contentLog =
            $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}\r\n" +
            $"Content: {exception.Message} && {exception.StackTrace}\r\n";

        File.AppendAllText(nameFileLog, contentLog);
      }
    }
  }


}
