using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Okyanus
{
    public partial class EndApp : Form
    {
        public EndApp()
        {
            InitializeComponent();

         //   Form.ActiveForm = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           // this->Close();
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

 
    }
}
