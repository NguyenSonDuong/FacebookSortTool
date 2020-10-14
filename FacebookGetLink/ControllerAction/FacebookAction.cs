using FacebookGetLink.ControllerAction;
using FacebookGetLink.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;

namespace FacebookGetLink
{
    public delegate void ProcessHandler(object obj, object data);
    public delegate void CompleteHandler();
    class FacebookAction
    {
        #region
        public static String COMMENTS = "1";
        public static String POST = "2";
        public static String GROUPS = "2";
        #endregion

        #region Khai báo const Parameter
        public const String KEY_GROUPS_USER = "me/groups?fields=administrator,created_time,id,name,member_count&limit=100";
        public const String KEY_GROUPS_POST = "feed?fields=message,id,created_time&limit=100";
        public const String KEY_GROUPS_POST_COMMENTS = "/comments?fields=message,id,created_time&limit=100";
        public const String KEY_REACTIONS_HAHA = "?fields=reactions.type(HAHA).limit(0).summary(total_count)";
        public const String KEY_REACTIONS = "?fields=reactions.summary(total_count)";
        public const String PATH_COOKIE = "cookie.ini";
        public const String PATH_TOKEN = "token.ini";
        #endregion

        #region Khai báo sự kiện

        private event ProcessHandler processLoading;
        private event CompleteHandler complateLoading;
        public event CompleteHandler ComplateLoading
        {
            add
            {
                this.complateLoading += value;
            }
            remove
            {
                this.complateLoading -= value;
            }
        }
        public event ProcessHandler ProcessLoading {
            add
            {
                this.processLoading += value;
            }
            remove
            {
                this.processLoading -= value;
            }
        }
        #endregion

        #region Khai báo const Link
        public String URL_GET_TOKEN = "https://m.facebook.com/composer/ocelot/async_loader/?publisher=feed";
        public String HOST = "https://graph.facebook.com/v6.0/";
        #endregion

        #region Khai báo biến Thuộc tính getter setter
        private string accesstoken;
        private string cookie;

        public string Accesstoken
        {
            get
            {
                return accesstoken;
            }

            set
            {
                accesstoken = value;
            }
        }
        public string Cookie
        {
            get
            {
                return cookie;
            }

            set
            {
                cookie = value;
            }
        }
        #endregion

        #region Phương thức
        public String GetUrl(string para)
        {
            return  HOST + para + "&access_token=" + Accesstoken;
        }
        
        public void GetTokenCookieForFile()
        {
            try
            {
                accesstoken = File.ReadAllText(PATH_TOKEN);
                cookie = File.ReadAllText(PATH_COOKIE);
                if (String.IsNullOrEmpty(accesstoken))
                    throw new Exception("Lỗi token file rỗng vui lòng cập nhật lại");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void AddTokenCookieToFile(String token, String cookie)
        {
            try
            {
                this.cookie = cookie;
                this.accesstoken = token;
                if (String.IsNullOrEmpty(token) && !String.IsNullOrEmpty(cookie))
                {
                    token = UpdateToken("");
                }
                
                File.WriteAllText(PATH_TOKEN, token);
                File.WriteAllText(PATH_COOKIE, cookie);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void GetGroupsUser(string url)
        {
            HttpRequest http = RequestCustom.GetRequets("","");
            try
            {
                string reponsive = http.Get(url).ToString();
                ObjectGroupsUser.Group groups = JsonConvert.DeserializeObject<ObjectGroupsUser.Group>(reponsive);
                if (processLoading != null)
                    processLoading(FacebookAction.GROUPS, groups);
                if (groups.paging != null)
                    if (groups.paging.next != null)
                        GetGroupsUser(groups.paging.next);
                    else
                    {
                        if(complateLoading!=null)
                        complateLoading();
                    }
                else
                {
                    if (complateLoading != null)
                        complateLoading();
                }
                    
            }
            catch (Exception ex)
            {
                if (complateLoading != null)
                    complateLoading();
                throw ex;
            }
        }
        
        public void GetPostComments(string url)
        {
            HttpRequest http = RequestCustom.GetRequets("", "");
            try
            {
                string reponsive = http.Get(url).ToString();
                ObjectGroupsPostComments.Comments groups = JsonConvert.DeserializeObject<ObjectGroupsPostComments.Comments>(reponsive);
                if(processLoading != null)
                    processLoading(FacebookAction.COMMENTS, groups);
                if (groups.paging != null)
                    if (groups.paging.next != null)
                        GetPostComments(groups.paging.next);
                    else
                    {
                        if (complateLoading != null)
                            complateLoading();
                    }
                else
                {
                    if (complateLoading != null)
                        complateLoading();
                }
            }
            catch (Exception ex)
            {
                if (complateLoading != null)
                    complateLoading();
                throw ex;
            }
        }
        public void GetGroupPosts(string url)
        {
            HttpRequest http = RequestCustom.GetRequets("","");
            try
            {
                string reponsive = http.Get(url).ToString();
                ObjectGroupsPost.Post groups = JsonConvert.DeserializeObject<ObjectGroupsPost.Post>(reponsive);
                if (processLoading != null)
                    processLoading(FacebookAction.POST, groups);
                if (groups.paging != null)
                    if (groups.paging.next != null)
                        GetGroupPosts(groups.paging.next);
                    else
                    {
                        if (complateLoading != null)
                            complateLoading();
                    }
                else
                {
                    if (complateLoading != null)
                        complateLoading();
                }
            }
            catch (Exception ex)
            {
                if (complateLoading != null)
                    complateLoading();
                throw ex;
            }
        }

        public String UpdateToken( String user_agent)
        {
            if (string.IsNullOrEmpty(this.cookie))
            {
                throw new Exception("Cookie không được bỏ trống");
            }
            if (String.IsNullOrEmpty(user_agent))
            {
                user_agent = Http.ChromeUserAgent();
            }
            try
            {
                HttpRequest http = RequestCustom.GetRequets(Cookie, user_agent);
                String dataAccessToken = http.Get(URL_GET_TOKEN).ToString();
                if (dataAccessToken.Contains("EAAA"))
                {
                    String endString = "\\\",\\\"useLocalFilePreview\\\"";
                    int start = dataAccessToken.IndexOf("EAAA");
                    int end = dataAccessToken.IndexOf(endString);
                    String t = dataAccessToken.Substring(start, end - start);
                    return t;
                }
                else
                {
                    throw new Exception("Cookie có thể đã hết hạn vui lòng thử lại");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

    }
}
