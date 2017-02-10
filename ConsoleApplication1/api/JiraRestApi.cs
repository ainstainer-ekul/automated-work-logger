using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    using api.objects;
    using RestSharp;
    using RestSharp.Authenticators;
    using RestSharp.Deserializers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    namespace University4Industry_UI_Testing.utils
    {
        public class JiraRestApi
        {
            private RestClient restClient;

            public JiraRestApi(string url)
            {
                restClient = new RestClient(url);
            }

            public void Login(string email, string password)
            {
                string loginEntryPoint = "/rest/auth/1/session";
                var loginRequest = new RestRequest(loginEntryPoint, Method.POST);
                restClient.Authenticator = new HttpBasicAuthenticator(email, password);
                IRestResponse response = restClient.Execute(loginRequest);
                CookieContainer cookiecon = new CookieContainer();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var cookie = response.Cookies.FirstOrDefault();
                    cookiecon.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
                }
                restClient.CookieContainer = cookiecon;
            }

            public List<Worklog> GetWorklogsList(string date)
            {
                var worklogsListRequest = new RestRequest(GetDailyWorklogsListEntryPoint(date), Method.GET);
                IRestResponse worklogsListResponse = restClient.Execute(worklogsListRequest);
           
                List<WorklogRootObject> worklogRootObjectList = new JsonDeserializer().Deserialize<List<WorklogRootObject>>(worklogsListResponse);

                List<Worklog> worklogList = new List<Worklog>();
                Worklog worklog;

                
                foreach (WorklogRootObject rootObject in worklogRootObjectList)
                {

                    Console.WriteLine("------");
                    worklog = new Worklog();
                    worklog.key = rootObject.issue.key;
                    worklog.timeSpentSeconds = rootObject.timeSpentSeconds;

                    Console.WriteLine(rootObject.dateStarted + " " + rootObject.issue.key + " " + rootObject.timeSpentSeconds + "  " + rootObject.comment);


                    worklog.comment = rootObject.comment;
                    worklog.dateStarted = rootObject.dateStarted;
                    worklogList.Add(worklog);
                    Console.WriteLine("------");

                }
                return worklogList;
            }

            public Dictionary<string, List<Worklog>> GetWorklogsListForPeriod(string startDate, string endDate )
            {
                Dictionary<string, List<Worklog>> resultLogsDictionary = new Dictionary<string, List<Worklog>>();

                for (var date = Convert.ToDateTime(startDate); date <= Convert.ToDateTime(endDate); date = date.AddDays(1))
                {
                    string oneDay = string.Format("{0}-{1}-{2}", date.Year, date.Month, date.Day);
                    resultLogsDictionary.Add(oneDay, GetWorklogsList(oneDay));
                }


                return resultLogsDictionary;
            }

            public int GetWorklogsSum(List<Worklog> worklogList) {
                int dailyWorklogs = 0;
                foreach (Worklog worklog in worklogList) {
                    dailyWorklogs += worklog.timeSpentSeconds;
                }
                return dailyWorklogs;
            }

            private string GetDailyWorklogsListEntryPoint(string yyyy_MM_dd) {
                return string.Format("/rest/tempo-timesheets/3/worklogs?dateFrom={0}&dateTo={0}", yyyy_MM_dd);
            }

            private string GetIssueEntryPoint(string key) {
                return string.Format("/rest/api/2/search?jql=key={0}", key);
            }

            private string AddWorklogEntryPoint(string key) {
                return string.Format("/rest/api/2/issue/{0}/worklog", key);
            }

            public void AddWorklog(string issueKey, string ticketComment, string duration, string startedDate) {
                var addWorklogRequest = new RestRequest(AddWorklogEntryPoint(issueKey), Method.POST);               
                addWorklogRequest.AddHeader("Accept", "application/json");
                addWorklogRequest.AddHeader("Content-type", "application/json");
                addWorklogRequest.Parameters.Clear();
                addWorklogRequest.AddJsonBody(
                new
                {
                    timeSpent = duration,
                    comment = ticketComment,
                    started = startedDate
                });
                restClient.Execute(addWorklogRequest);
            }

            public static void CopyWorkLogs(List<Worklog> worklogList, JiraRestApi targetJiraRestApi, string targetTicket) {
                foreach (Worklog oneWorklog in worklogList) { 
                    Console.WriteLine("-----------");
                    Console.WriteLine(oneWorklog.key + "  "
                        + Utils.ConvertSecondsToWorklogFormat(oneWorklog.timeSpentSeconds));

                    Console.WriteLine("|| targetTicket: " + targetTicket);
                    Console.WriteLine("|| timeSpentSeconds: " + Utils.ConvertSecondsToWorklogFormat(oneWorklog.timeSpentSeconds));
                    Console.WriteLine("|| dateStarted: " + string.Format("{0}{1:zz}00", oneWorklog.dateStarted, DateTime.Now));



//                    targetJiraRestApi.AddWorklog(targetTicket,
//                        oneWorklog.key,
//                        Utils.ConvertSecondsToWorklogFormat(oneWorklog.timeSpentSeconds),
//                        string.Format("{0}{1:zz}00", oneWorklog.dateStarted, DateTime.Now)                      
//                    );
                }
            }
        }
    }
}
