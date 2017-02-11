using ConsoleApplication1.api.objects;
using ConsoleApplication1.University4Industry_UI_Testing.utils;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ConsoleApplication1.api.objects.LoggedInUserRootObject;

namespace ConsoleApplication1
{
    class JiraWorkLogger
    {
        private static String U4I_JIRA = Utils.GetConfigValue("u4i-jira");
        private static String U4I_LOGIN = Utils.GetConfigValue("u4i-login");
        private static String U4I_PASSWORD = Utils.GetConfigValue("u4i-password");
        private static String AIN_JIRA = Utils.GetConfigValue("ain-jira");
        private static String AIN_LOGIN = Utils.GetConfigValue("ain-login");
        private static String AIN_PASSWORD = Utils.GetConfigValue("ain-password");
        private static String TARGET_TICKET = Utils.GetConfigValue("target-ain-ticket");

        private static JiraRestApi ainJiraRestApi;
        private static JiraRestApi u4iJiraRestApi;
        
        static void Main(string[] args)
        {
                restClientsInit();
                string command = "";
                while (true)
                {
                    Console.WriteLine("Enter 'exit' to exit");
                    Console.WriteLine("Enter a specific period in YYYY-MM-D:YYYY-MM-D format " +
                                      "to copy worklogs:");

                    command = Utils.GetConsoleValue();
                    if (Utils.IsDateFormatCorrect(command))
                    {
                        string[] dateItems = command.Split(':');

                        switch (Utils.CompareTwoDates(dateItems[0], dateItems[1]))
                        {
                            case 0:
                            case -1:
                                Console.WriteLine("Copying...");
                                Console.WriteLine(" ");
                                Dictionary<string, List<Worklog>> worklogsListForPeriod =
                                    u4iJiraRestApi.GetWorklogsListForPeriod(dateItems[0], dateItems[1]);
                                foreach (KeyValuePair<string, List<Worklog>> listlogsForOneDay in worklogsListForPeriod)
                                {
                                    JiraRestApi.CopyWorkLogs(listlogsForOneDay.Value, ainJiraRestApi, TARGET_TICKET);
                                }
                                Console.WriteLine(
                                    String.Format("Copying successfully completed! (for '{0}' - '{1}' dates)",
                                        dateItems[0], dateItems[1]));
                                break;

                            default:
                                Console.WriteLine("'{0}' {1} '{2}'. Worklogs were not copied!", dateItems[0],
                                    "is later than", dateItems[1]);
                                break;
                        }
                    }
                    else if (command.Equals("exit"))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(String.Format("'{0}' - unsupported command", command));
                    }
                    Console.ReadKey();
                }
         }

        public static void restClientsInit() {
            u4iJiraRestApi = new JiraRestApi(U4I_JIRA);
            u4iJiraRestApi.Login(U4I_LOGIN, U4I_PASSWORD);
            ainJiraRestApi = new JiraRestApi(AIN_JIRA);
            ainJiraRestApi.Login(AIN_LOGIN, AIN_PASSWORD);
        }
    }
}
