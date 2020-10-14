using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacebookGetLink
{
    public partial class AddTokenCookie : Form
    {
        public String token;
        public String cookie;

        public AddTokenCookie()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cookie = textBox1.Text;
            token = textBox2.Text;
            if (String.IsNullOrEmpty(token) && String.IsNullOrEmpty(cookie))
            {
                MessageBox.Show("Vui lòng nhập 1 trong 2 trường sau");
                return;
            }
            this.Close();
            
        }

        private void AddTokenCookie_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (String.IsNullOrEmpty(token) && String.IsNullOrEmpty(cookie))
            {
                Application.Exit();
            }
        }
    }
}
