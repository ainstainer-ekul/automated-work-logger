using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using ConsoleApplication1.api.objects;

namespace ConsoleApplication1
{
    public class Utils
    {
        public static string ConvertSecondsToWorklogFormat(string sec)
        {
            TimeSpan duration = new TimeSpan(0, 0, 0, Int32.Parse(sec), 0);
            return string.Format("{0:D1}h {1:D2}m", duration.Hours, duration.Minutes);
        }

        public static string GetCurrentDateAndTime()
        {
            return string.Format("{0:yyyy-MM-d}T{1:HH:mm:ss}.{2:fff}{3:zz}00", DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

        public static string GetConfigValue(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                throw new Exception("Error reading app settings");
            }
        }

        public static string GetConsoleValue()
        {
            return Console.ReadLine();
        }

        public static bool IsDateFormatCorrect(string dates) {
            string datePattern = @"([\d]{4}-[\d]{1,2}-[\d]{1,2}:[\d]{4}-[\d]{1,2}-[\d]{1,2})";
            Regex r = new Regex(datePattern, RegexOptions.IgnoreCase);
            if (r.Match(dates).Success)
            {
                string[] dateItems = dates.Split(':');
                return IsDateExists(dateItems[0]) & IsDateExists(dateItems[1]);
            }
            else {
                return false;
            }
        }

        private static bool IsDateExists(string date) {
            string[] dateItems = date.Split('-');
            if ((Int32.Parse(dateItems[0]) <= DateTime.Now.Year) &
                (Int32.Parse(dateItems[1]) <= 12) &
                 (Int32.Parse(dateItems[2]) <= 31))
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static int CompareTwoDates(string startDate, string endDate)
        {
            return DateTime.Compare(Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));  
        }

        public static List<Worklog> GetWorkloglistFromTxtFile()
        {
            string line;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\txt-logs\\oneDayLogs.txt");
            List<Worklog> worklogsList = new List<Worklog>();
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);

            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
                worklogsList.Add(GetWorklogFromTxtLine(line));
                
            }
            file.Close();


            return worklogsList;
        }

        private static Worklog GetWorklogFromTxtLine(string line)
        {
            string[] numberLineSeparatedItems = line.Split(' ');
            line = line.Substring(numberLineSeparatedItems[0].Length+1, line.Length-1- numberLineSeparatedItems[0].Length);

            string[] worklogData = line.Split(new string[] { " - " }, StringSplitOptions.None);
            Worklog worklog = new Worklog();

            worklog.key = worklogData[0];
            worklog.timeSpentSeconds = worklogData[1];
            worklog.comment = worklogData[2];
            worklog.dateStarted = string.Format("{0}{1:zz}00", DateTime.Now, DateTime.Now);

            return worklog;
        }

        public void vvv()
        {
            string oneDay = string.Format("{0}-{1}-{2}T{3}:{4}:{5}.{6}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                         DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);

        }
    }
}
