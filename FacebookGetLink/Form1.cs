using FacebookGetLink.ControllerAction;
using FacebookGetLink.model;
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
        public static bool isSelect = false;
        public static string pattern = @"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])?";
        public bool isComplete = false;
        private FacebookAction face;
        private bool isRegex;
        public String textFormat;
        public int countCommentsLoad = 0;

        public Form1()
        {
            face = new FacebookAction();
            face.ProcessLoading += ProcessLoadingEvent;
            face.ComplateLoading += ComplateLoadingProcess;
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
                    MessageBox.Show("Quá trình lấy token lỗi vui lòng cập nhật thủ công ở trình duyệt");
                    Process.Start(face.URL_GET_TOKEN);
                    Application.Exit();
                }
                
            }
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataViewTable.Columns.Clear();
            CreateCollumTable(HEADER_TABLE_GROUPS);
            ThreadCustom.StartThread(() =>
            {
                isComplete = false;
                String url = "";
                url = face.GetUrl(FacebookAction.KEY_GROUPS_USER);
                try
                {
                    face.GetGroupsUser(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi quá trình lấy comments: " + ex.Message);
                }

            });

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
            textFormat = tbFormat.Text;
            String textID = tbIDPost.Text;
            if(String.IsNullOrEmpty(textFormat) || String.IsNullOrEmpty(textID))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }
            CreateCollumTable(HEADER_TABLE_POSTCOMENT);
            ThreadCustom.StartThread(() =>
            {
                isComplete = false;
                String url = "";
                url = face.GetUrl(textID+FacebookAction.KEY_GROUPS_POST_COMMENTS);
                try
                {
                    face.GetPostComments(url);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Lỗi quá trình lấy comments: " + ex.Message);
                }
                
            });
            
        }
        private void ProcessLoadingEvent(Object obj, Object obj2)
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
            else {

            }
        }

        private void ComplateLoadingProcess()
        {
            isComplete = true;
        }
        private String GetTextOFFormat(String text, String regex)
        {
            StringBuilder output = new StringBuilder();
            try
            {
                RegexOptions options = RegexOptions.Multiline;
                MatchCollection maths = Regex.Matches(text,regex, options);
                if(maths.Count <=0)
                {
                    return "";
                }else if(maths.Count == 1)
                {
                    return maths[0].Value;
                }
                else
                {
                    foreach (Match m in maths)
                    {
                        output.Append(m.Value + "|");
                    }
                    return output.ToString();
                }
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            String id = dataViewTable.SelectedRows[0].Cells[0].Value.ToString();
        }
        
        private void dataViewTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                            ExportExcel(save.FileName);
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

        public void ExportExcel(String path)
        {
                ExcelPackage excel = new ExcelPackage();
                try
                {
                    excel.Workbook.Properties.Title = "Danh sách comment";
                    excel.Workbook.Worksheets.Add("Danh Sach link");

                    ExcelWorksheet es = excel.Workbook.Worksheets[1];
                    es.Name = "Comments";
                    int row = 1;
                    int cou = 1;
                    foreach (DataGridViewTextBoxColumn item in dataViewTable.Columns)
                    {
                        es.Cells[1, cou].Value = item.HeaderCell.Value;
                        es.Cells[1, cou].Style.Font.Size = 14;
                        es.Cells[1, cou].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        es.Cells[1, cou].Style.Font.Color.SetColor(Color.Red);
                        cou++;
                    }
                    row++;
                    for (int j = 0; j < dataViewTable.RowCount; j++)
                    {
                        cou = 1;
                        foreach (DataGridViewTextBoxColumn item in dataViewTable.Columns)
                        {
                            es.Cells[row, cou].Value = dataViewTable.Rows[j].Cells[cou - 1].Value;
                            cou++;
                        }
                        row++;

                    }
                    byte[] by = excel.GetAsByteArray();
                    File.WriteAllBytes(path, by);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        private void button3_Click_1(object sender, EventArgs e)
        {
            textFormat = tbFormmatGroups.Text;
            String textID = tbIDGroups.Text;
            if (String.IsNullOrEmpty(textFormat) || String.IsNullOrEmpty(textID))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }
            CreateCollumTable(HEADER_TABLE_POSTCOMENT);
            ThreadCustom.StartThread(() =>
            {
                isComplete = false;
                String url = "";
                url = face.GetUrl(textID + FacebookAction.KEY_GROUPS_POST);
                try
                {
                    face.GetGroupPosts(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi quá trình lấy comments: " + ex.Message);
                }

            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.Delete(FacebookAction.PATH_COOKIE);
            File.Delete(FacebookAction.PATH_TOKEN);
            Application.Exit();
        }
    }
    

    
}
