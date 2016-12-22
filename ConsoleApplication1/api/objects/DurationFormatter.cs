using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.api.objects
{
    public class DurationFormatter
    {
        public static string ConvertSecondsToWorklogFormat(int sec) {
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, 0);
            string formatted = string.Format("{0:D1}h {1:D2}m {2:D2}s", duration.Hours, duration.Minutes, duration.Seconds);
            Console.WriteLine(" ss " + formatted);
            return formatted;
        } 
    }
}
