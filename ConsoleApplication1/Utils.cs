using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    public class Utils
    {
        public static string ConvertSecondsToWorklogFormat(int sec)
        {
            TimeSpan duration = new TimeSpan(0, 0, 0, sec, 0);
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
    }
}
