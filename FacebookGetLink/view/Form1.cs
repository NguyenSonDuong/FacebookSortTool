using FacebookGetLink.ControllerAction;
using FacebookGetLink.model;
using FacebookGetLink.model.reponsive;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace FacebookGetLink
{
    public partial class Form1 : Form
    {

        
        public static String[] HEADER_TABLE_GROUPS = { "ID", "Tên nhóm", "Số lượng thành viên", "Trạng thái quản trị", "Thời gian tạo nhóm" };
        public static String[] HEADER_TABLE_POSTCOMENT = { "ID comments", "Nội dung comments", "Chuỗi tìm kiếm", "Thời gian comments"};
        public static String[] HEADER_TABLE_POSTGROUPS = { "ID Post", "Nội dung post", "Chuỗi tìm kiếm", "Thời gian post","COUNT REACTION" };
        public static bool isSelect = false;
        public static string pattern = @"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])?";
        public bool isComplete = false;
        private FacebookAction face;
        private bool isRegex;
        public String textFormat;
        public int countCommentsLoad = 0;
        private bool isAddColum = false;
        private Random rand = new Random();
        Random rad = new Random();
        public Form1()
        {
            face = new FacebookAction();
            face.ProcessLoading += ProcessLoadingEvent;
            face.ComplateLoading += ComplateLoadingProcess;
            face.ErrorLoading += Face_ErrorLoading;
            try
            {
                face.GetTokenCookieForFile();
            }catch(Exception ex)
            {
                AddTokenCookie add = new AddTokenCookie();
                add.ShowDialog();
                try
                {
                    if(String.IsNullOrEmpty(add.cookie) && String.IsNullOrEmpty(add.token))
                    {
                        MessageBox.Show("Vui lòng nhập lại");
                        add.ShowDialog();
                    }
                    face.AddTokenCookieToFile(add.token, add.cookie);
                }
                catch(Exception e)
                {
                    Application.Exit();
                }
                
            }
            InitializeComponent();
        }

        private void Face_ErrorLoading(string message)
        {
            isComplete = true;
            MessageBox.Show("Lỗi: " + message);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            

        }
        public void CreateCollumTable(String[] data)
        {
            int i = 0;
            foreach (String item in data)
            {
                dataViewTable.Columns.Add("Col" + i, item);
                i++;
            }
        }
       
        
        private void button2_Click(object sender, EventArgs e)
        {
            

        }
        private async void ProcessLoadingEvent(Object obj, Object obj2)
        {
            if (obj.Equals(FacebookAction.COMMENTS))
            {
                CustomInvoker.RunInvoker(dataViewTable, (sender) =>
                {
                    DataGridView data = sender as DataGridView;
                    ObjectGroupsPostComments.Comments comments = obj2 as ObjectGroupsPostComments.Comments;
                    foreach(ObjectGroupsPostComments.Datum item in comments.data)
                    {
                        countCommentsLoad++;
                        String stringOutFormat = GetTextOFFormat(item.message, textFormat);
                        if (String.IsNullOrEmpty(stringOutFormat))
                            continue;
                        data.Rows.Add(item.id, item.message, stringOutFormat, item.created_time);
                    }
                    CustomInvoker.RunInvoker(lbStatus, (sen) =>
                    {
                        lbStatus.Text = countCommentsLoad + " Comments loader";
                    });
                });
                
            }
            else 
            if(obj.Equals(FacebookAction.GROUPS)){
                CustomInvoker.RunInvoker(dataViewTable, (sender) =>
                {
                    DataGridView data = sender as DataGridView;
                    ObjectGroupsUser.Group groups = obj2 as ObjectGroupsUser.Group;
                    foreach (ObjectGroupsUser.Datum item in groups.data)
                    {
                        countCommentsLoad++;
                        data.Rows.Add(item.id, item.name, item.member_count,item.administrator ,item.created_time);

                    }
                    CustomInvoker.RunInvoker(lbStatus, (sen) =>
                    {
                        lbStatus.Text = countCommentsLoad + " Groups loader";
                    });
                });
            }
            else if(obj.Equals(FacebookAction.POST)){
                CustomInvoker.RunInvoker(dataViewTable, (sender) =>
                {
                    DataGridView data = sender as DataGridView;
                    ObjectGroupsPost.Post groups = obj2 as ObjectGroupsPost.Post;
                    foreach (ObjectGroupsPost.Datum item in groups.data)
                    {
                        
                        String stringOutFormat = GetTextOFFormat(item.message, textFormat);
                        if (String.IsNullOrEmpty(stringOutFormat))
                            continue;
                        countCommentsLoad++;
                        List<String> dataRows = new List<string>();
                        dataRows.Add(item.id);
                        dataRows.Add(item.message);
                        dataRows.Add(stringOutFormat);
                        dataRows.Add(item.created_time.ToShortDateString());
                        dataRows.Add(item.reactions.summary.total_count+"");
                        int count = 0;
                        data.Rows.Add(dataRows.ToArray());
                        Thread.Sleep(rad.Next(100, 1000));
                        
                    }
                    CustomInvoker.RunInvoker(lbStatus, (sen) =>
                    {
                        lbStatus.Text = countCommentsLoad + " Post loader";
                    });
                });
            }
            else if (obj.Equals(FacebookAction.REACIONS))
            {
                CustomInvoker.RunInvoker(dataViewTable, (sender) =>
                {
                    DataGridView data = sender as DataGridView;
                    ReponsiveReactions.Rootobject reaction = obj2 as ReponsiveReactions.Rootobject;
                    
                    CustomInvoker.RunInvoker(lbStatus, (sen) =>
                    {
                        lbStatus.Text = countCommentsLoad + " Groups loader";
                    });
                });
            }
        }
        private void ComplateLoadingProcess()
        {
            isComplete = true;
            MessageBox.Show("Hoàn tất load dữ liệu");
        }
        private String GetTextOFFormat(String text, String regex)
        {
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }
            StringBuilder output = new StringBuilder();
            try
            {
                if (Regex.IsMatch(text.ToLower(), regex.ToLower()))
                {
                    RegexOptions options = RegexOptions.Multiline;
                    MatchCollection maths = Regex.Matches(text.ToLower(), regex.ToLower(), options);
                    foreach (Match m in maths)
                    {
                        output.Append(m.Groups[1].Value + "|");
                    }
                    return output.ToString();
                }
                else
                {
                    return "";
                }
                
            }
            catch(Exception ex)
            {
                return "";
            }
        }
        
        
        private void ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isComplete)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Excel|*.xlsx|All type|*.*";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    ThreadCustom.StartThread(() => {
                        try
                        {
                            Controller.ExportExcel(dataViewTable,save.FileName);
                            MessageBox.Show("Lưu thành công file");
                            try
                            {
                                Process.Start(save.FileName);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lưu file thất bại");
                        }
                    });
                }
            }
        }

        

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            textFormat = tbFormmatGroups.Text;
            long timeStart = 0;
            long timeEnd = 0;
            int timeDelay = 3000;
            if(!Int64.TryParse(txtTimeStart.Text, out timeStart) || !Int64.TryParse(txtTimeEnd.Text, out timeEnd))
            {
                MessageBox.Show("Vui Lòng nhập time phù hợp");
                return;
            }
            Int32.TryParse(txtTimeEnd.Text,out timeDelay);
            String textID = tbIDGroups.Text;

            if (String.IsNullOrEmpty(textFormat) || String.IsNullOrEmpty(textID))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            dataViewTable.Columns.Clear();
            CreateCollumTable(HEADER_TABLE_POSTGROUPS);
            ThreadCustom.StartThread(() =>
            {
                isComplete = false;
                String url = "";
                url = face.GetUrl(textID + FacebookAction.KEY_GROUPS_POST.Replace("{%timestart%}",""+ timeStart).Replace("{%timeend%}", ""+ timeEnd));
                try
                {
                    face.GetGroupPosts(url, timeDelay);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi quá trình lấy comments: " + ex.Message);
                    return;
                }

            });
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            textFormat = tbFormat.Text;
            String textID = tbIDPost.Text;
            if (String.IsNullOrEmpty(textFormat) || String.IsNullOrEmpty(textID))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            dataViewTable.Columns.Clear();
            CreateCollumTable(HEADER_TABLE_POSTCOMENT);
            isComplete = false;
            Controller.SearchCommnets(face, textFormat, textID,
                (mess) =>
                {
                    MessageBox.Show("Lỗi lấy comments: " + mess);
                    return;
                });
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            dataViewTable.Columns.Clear();
            CreateCollumTable(HEADER_TABLE_GROUPS);
            isComplete = false;
            Controller.GetGroupsOfUser(face,
                (mess) => {
                    MessageBox.Show("Lỗi quá trình lấy thong tin Groups: " + mess);
                });
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            File.Delete(FacebookAction.PATH_COOKIE);
            File.Delete(FacebookAction.PATH_COOKIE);
            File.Delete(FacebookAction.PATH_TOKEN);
            Application.Exit();
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            ThreadCustom.StartThread(() =>
            {
                face.UpdateToken("");
            });
        }
        

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            String[] idUsers = null;
            if(cbUser.Checked)
            {
                idUsers = tbIdUserReceiver.Text.Split('|');
            }    
            String idUser = tbIdUserReceiver.Text;
            String message = tbMessage.Text;
            ThreadCustom.StartThread(() => {
                if (cbUser.Checked)
                {
                    foreach(String id in idUsers)
                    {
                        face.SendMessage(message, id);
                        Thread.Sleep(rand.Next(500, 2000));
                    }
                }
                else
                {
                    face.SendMessage(message , idUser);
                    Thread.Sleep(1000);
                }

            });
            
        }

        private void tbFormmatGroups_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //String firstMacAddress = NetworkInterface
            //                .GetAllNetworkInterfaces()
            //                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            //                .Select(nic => nic.GetPhysicalAddress().ToString())
            //                .FirstOrDefault();
            //HttpRequest http = RequestCustom.GetRequets("", "");
            //try
            //{
            //    String data = http.Post("https://apipixivcustom.herokuapp.com/api/v1/active/active", "deviceID=" + firstMacAddress, "application/x-www-form-urlencoded").ToString();
            //    if (data.Equals("ok"))
            //    {
            //    }
            //    else
            //    {
            //        //String sign = http.Post("https://apipixivcustom.herokuapp.com/api/v1/active/SignActive", "deviceID=" + firstMacAddress, "application/x-www-form-urlencoded").ToString();
            //        //if (sign.Equals("ok"))
            //        //{

            //        //}
            //        MessageBox.Show("Ứng dụng chưa được active vui lòng active");
            //        Application.Exit();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Active Fail");
            //    Application.Exit();
            //}
        }
    }
    

    
}
