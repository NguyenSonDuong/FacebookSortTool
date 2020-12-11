using FacebookGetLink.ControllerAction;
using FacebookGetLink.model;
using FacebookGetLink.model.bodyvariable;
using FacebookGetLink.model.reponsive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xNet;

namespace FacebookGetLink
{
    public delegate void ProcessHandler(object obj, object data);
    public delegate void CompleteHandler();
    public delegate void CompleteRetrurnHandler(object value);
    public class FacebookAction
    {
        #region
        public static String COMMENTS = "1";
        public static String POST = "2";
        public static String GROUPS = "3";
        public static String REACIONS = "4";
        #endregion

        #region Khai báo const Parameter
        public const String KEY_GROUPS_USER = "me/groups?fields=administrator,created_time,id,name,member_count&limit=100";
        public const String KEY_GROUPS_POST = "/feed?fields=message,id,created_time&limit=25";
        public const String KEY_GROUPS_POST_COMMENTS = "/comments?fields=message,id,created_time&limit=100";
        public const String KEY_REACTIONS_HAHA = "?fields=reactions.type(HAHA).limit(0).summary(total_count)";
        public const String KEY_REACTIONS = "?fields=reactions.summary(total_count)";
        public const String PATH_COOKIE = "cookie.ini";
        public const String PATH_TOKEN = "token.ini";
        #endregion

        #region Khai báo sự kiện

        private event ProcessHandler processLoading;
        private event CompleteHandler complateLoading;
        private event ErrorHandler errorLoding;
        private event CompleteRetrurnHandler completeReturn;
        public event CompleteRetrurnHandler CompleteReturn
        {
            add
            {
                this.completeReturn += value;
            }
            remove
            {
                this.completeReturn -= value;
            }
        }
        public event ErrorHandler ErrorLoading
        {
            add
            {
                this.errorLoding += value;
            }
            remove
            {
                this.errorLoding -= value;
            }
        }
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
        public String URL_GRAPH = "https://www.facebook.com/api/graphql/";

        #endregion

        #region Khai báo biến Thuộc tính getter setter
        private string accesstoken;
        private string cookie;
        private String fb_dtsg = "";
        private String id;
        private String username;

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
        public string Fb_dtsg
        {
            get
            {
                return fb_dtsg;
            }

            set
            {
                fb_dtsg = value;
            }
        }
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }
        #endregion

        #region Phương thức
        public String GetUrl(string para)
        {
            return  HOST + para + "&access_token=" + Accesstoken;
        }

        public String GetFbDtsgFromContent(String dataAccessToken)
        {
            dataAccessToken = dataAccessToken.Replace("\\", "").Replace(" ","");
            string pattern = "name=\"fb_dtsg\"value=\"[a-zA-Z0-9-_:]+\"";
            string patGetFBDTSG = @"""[a-zA-Z0-9-_:]+""";
            Match fb = Regex.Match(dataAccessToken, pattern);
            String fb_dtsg = Regex.Matches(fb.Value, patGetFBDTSG)[1].Value;
            return fb_dtsg.Replace('"', ' ').Trim();
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
                    if (errorLoding != null)
                        errorLoding("Token có thể đã bị lỗi vui lòng thửu lại");
                }
                    
            }
            catch (Exception ex)
            {
                if (errorLoding != null)
                    errorLoding(ex.Message);
                throw ex;
            }
        }
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public ReactionCount.Top_Reactions GetReactionCount(String ID, String reactionType)
        {
            if(String.IsNullOrEmpty(ID) || String.IsNullOrEmpty(fb_dtsg))
            {
                errorLoding("Dữ liệu chưa cập nhật đầy đủ (ID,FB_DTSG), vui lòng cập nhật lại cookie,");
                return null;
            }
            try
            {
                String body = BodyReaction(Base64Encode("feedback:"+ID),  reactionType);
                HttpRequest http = RequestCustom.GetRequets(cookie, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
                String output = http.Post(URL_GRAPH, body, "application/x-www-form-urlencoded").ToString(); 
                ReactionCount.Rootobject reponsive = JsonConvert.DeserializeObject<ReactionCount.Rootobject>(output);
                return reponsive.data.node.top_reactions;
            }
            catch(Exception ex)
            {
                errorLoding(ex.Message);
                return null;
            }
        }
        public String BodyReaction(String feedbackID, int count, String cursor,String reactionType)
        {
            ReactionsVariable variable = new ReactionsVariable();
            variable.count = count;
            variable.cursor = cursor;
            variable.feedbackTargetID = feedbackID;
            variable.id = feedbackID;
            variable.scale = 1;
            variable.reactionType = reactionType;
            String body = "__user=" + Id+ "&__a=1&fb_dtsg=" + fb_dtsg + "&variables="+JsonConvert.SerializeObject(variable) + "&server_timestamps=true&doc_id=3785345958187365";
            return body;
        }
        public String BodyReaction(String feedbackID,  String reactionType)
        {
            RactionCount variable = new RactionCount();
            variable.feedbackTargetID = feedbackID;
            variable.scale = 1;
            variable.reactionType = reactionType;
            String body = "av="+Id+"__user=" + Id + "&__a=1&fb_dtsg=" + fb_dtsg + "&variables=" + JsonConvert.SerializeObject(variable) + "&server_timestamps=true&doc_id=3785345958187365";
            return body;
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
                    if (errorLoding != null)
                        errorLoding("Token có thể đã lỗi vui lòng lấy lại");
                }
            }
            catch (Exception ex)
            {
                if (errorLoding != null)
                    errorLoding(ex.Message);
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
                    if (errorLoding != null)
                        errorLoding("Token có thể đã lỗi vui lòng thử lại");
                }
            }
            catch (Exception ex)
            {
                if (errorLoding != null)
                    errorLoding(ex.Message);
            }
        }
        public String GetID( string cookie)
        {
            var temp = cookie.Split(';');
            foreach (var item in temp)
            {
                var temp2 = item.Trim().Split('=');
                if (temp2.Length > 1)
                {
                    if (temp2[0].Equals("c_user"))
                    {
                        return temp2[1];
                    }

                }
            }
            return null;
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
                    accesstoken = t;
                    this.fb_dtsg = GetFbDtsgFromContent(dataAccessToken);
                    this.id = GetID(Cookie);
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
        public static void GetCookie(String username, String password)
        {
            String url = "https://mbasic.facebook.com/login/device-based/regular/login/?refsrc=https%3A%2F%2Fmbasic.facebook.com%2F&lwv=100&ref=dbl";
            String body = "lsd=AVpFPQboc4o&jazoest=2965&m_ts=1602739649&li=wd2HX-Giuclk1-hjGTVzUu46&try_number=0&unrecognized_tries=0&email=hunterdemon9x99&pass=NguyenDuong130999&login=%C4%90%C4%83ng+nh%E1%BA%ADp";
            HttpRequest http = RequestCustom.GetRequets("", Http.FirefoxUserAgent());
            http.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            http.Referer = "https://mbasic.facebook.com/login/?next&ref=dbl&fl&refid=8";
            HttpResponse output = http.Post(url, body, "application/x-www-form-urlencoded");
            CookieDictionary cookie = output.Cookies;
            String a = "";
            Console.WriteLine(""); 
        }
        #endregion

    }
}
