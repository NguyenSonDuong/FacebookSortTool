using FacebookGetLink.ControllerAction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace FacebookGetLink.view
{
    public partial class Notifi : Form
    {
        public static String PATH_KEY = "key.ini";
        public Notifi()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
