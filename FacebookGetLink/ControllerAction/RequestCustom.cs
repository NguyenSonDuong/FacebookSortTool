using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace FacebookGetLink.ControllerAction
{
    public class RequestCustom
    {
        public static HttpRequest GetRequets(String cookie, String user_agent)
        {
            HttpRequest http = new HttpRequest();
            http.Cookies = new CookieDictionary();
            if (!String.IsNullOrEmpty(cookie))
            {
                AddCookie(http, cookie);
            }
            if (!String.IsNullOrEmpty(user_agent))
            {
                http.UserAgent = user_agent;
            }
            return http;
        }
        public static void AddCookie(HttpRequest http, string cookie)
        {
            var temp = cookie.Split(';');
            foreach (var item in temp)
            {
                var temp2 = item.Split('=');
                if (temp2.Count() > 1)
                {
                    http.Cookies.Add(temp2[0], temp2[1]);
                }
            }
        }
    }
}
