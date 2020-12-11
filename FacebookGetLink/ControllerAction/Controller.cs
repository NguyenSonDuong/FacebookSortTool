using FacebookGetLink.ControllerAction;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacebookGetLink
{
    public delegate void ErrorHandler(String message);
    public delegate void SucessHandler(String message);
    public delegate void RunHandler();
    public class Controller
    {
        public static void ExportExcel(DataGridView dataViewTable, String path)
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
        public static void SearchCommnets(FacebookAction face,String textFormat, String textID, ErrorHandler error)
        {
            if (String.IsNullOrEmpty(textFormat) || String.IsNullOrEmpty(textID))
            {
                error("Null or Empty error");
            }
            
            ThreadCustom.StartThread(() =>
            {
                
                String url = "";
                url = face.GetUrl(textID + FacebookAction.KEY_GROUPS_POST_COMMENTS);
                try
                {
                    face.GetPostComments(url);
                }
                catch (Exception ex)
                {
                    error(ex.Message);
                }

            });
        }
        public static void GetGroupsOfUser(FacebookAction face,ErrorHandler error)
        {
            ThreadCustom.StartThread(() =>
            {
                String url = "";
                url = face.GetUrl(FacebookAction.KEY_GROUPS_USER);
                try
                {
                    face.GetGroupsUser(url);
                }
                catch (Exception ex)
                {
                    error(ex.Message);
                }

            });
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

    }
}
