using ConsoleApplication1.api.objects;
using ConsoleApplication1.University4Industry_UI_Testing.utils;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Console.WriteLine("|| " + args[0]);


            restClientsInit();
            
            string line = "";
            List<Worklog> u4iWorklogsList;
            while (true) {
                Console.WriteLine("Enter 'exit' to exit");
                Console.WriteLine("Enter the date in YYYY-MM-D format:");
                line = Utils.GetConsoleValue();
                if (Utils.IsDateFormatCorrect(line))
                {
                    u4iWorklogsList = u4iJiraRestApi.GetWorklogsList(line); 

                    Console.WriteLine("Copying...");
                    JiraRestApi.CopyWorkLogs(u4iWorklogsList, ainJiraRestApi, TARGET_TICKET);

                    Console.WriteLine(" ");
                    Console.WriteLine(String.Format("Copying successfully completed! (for '{0}' date)", line));
                    Console.WriteLine(" ");
                }
                else if(line.Equals("exit")){
                    break;
                }
            }       
            Console.ReadKey();
        }

        public static void restClientsInit() {
            u4iJiraRestApi = new JiraRestApi(U4I_JIRA);
            u4iJiraRestApi.Login(U4I_LOGIN, U4I_PASSWORD);

            ainJiraRestApi = new JiraRestApi(AIN_JIRA);
            ainJiraRestApi.Login(AIN_LOGIN, AIN_PASSWORD);
        }
    }
}
