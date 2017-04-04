using ConsoleApplication1.api.objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AWLconsole
{
    public class FileLogger
    {
        private static string logsFileName = "daily_logs_list.txt";

        public static List<Worklog> GetLogsFromFile() {
            List<Worklog> worklogList = new List<Worklog>();
      
            string fileContent = File.ReadAllText(GetLogsTxtFilePath(logsFileName));

            string[] items = fileContent.Split(new string[] { "---\r\n" }, StringSplitOptions.None);
            foreach (string item in items)
            {
                worklogList.Add(StringToWorkLogDictionary(item));
            }

            return worklogList;
        }

        private static string GetLogsTxtFilePath(string txtFileName) {
            string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string filePath = projectPath +
                Path.DirectorySeparatorChar + logsFileName;

            return filePath;
        }

        private static Worklog StringToWorkLogDictionary(string textLine) {
            string[] worklogItems = textLine.Split(new string[] { " - " }, StringSplitOptions.None);

            Worklog worklog = new Worklog();
            worklog.key = worklogItems[0];
            worklog.timeSpeendString = worklogItems[1];
            worklog.comment = worklogItems[2];

            return worklog;
        }
    }
}
