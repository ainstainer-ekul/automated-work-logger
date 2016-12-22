using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.api.objects
{
    public class WorklogRootObject
    {
        public int timeSpentSeconds { get; set; }
        public int billedSeconds { get; set; }
        public string dateStarted { get; set; }
        public string comment { get; set; }
        public string self { get; set; }
        public int id { get; set; }
        public Author author { get; set; }
        public Issue issue { get; set; }
        public List<object> worklogAttributes { get; set; }
        public List<object> workAttributeValues { get; set; }

        public class Author
        {
            public string self { get; set; }
            public string name { get; set; }
            public string displayName { get; set; }
            public string avatar { get; set; }
        }

        public class IssueType
        {
            public string name { get; set; }
            public string iconUrl { get; set; }
        }

        public class Issue
        {
            public string self { get; set; }
            public int id { get; set; }
            public int projectId { get; set; }
            public string key { get; set; }
            public int remainingEstimateSeconds { get; set; }
            public IssueType issueType { get; set; }
            public string summary { get; set; }
        }
    }
}
