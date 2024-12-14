using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
            panel1.Controls.Clear();
            qr qr = new qr(label1.Text,label2.Text,label3.Text);
            qr.MdiParent = this;
            panel1.Controls.Add(qr);
            qr.Show();
        }

    }
}
