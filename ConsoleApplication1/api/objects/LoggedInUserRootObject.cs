using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.api.objects
{
    public class LoggedInUserRootObject
    {
        public string self { get; set; }
        public string name { get; set; }
        public LoginInfo loginInfo { get; set; }

        public class LoginInfo
        {
            public int failedLoginCount { get; set; }
            public int loginCount { get; set; }
            public string lastFailedLoginTime { get; set; }
            public string previousLoginTime { get; set; }
        }
    }
}
