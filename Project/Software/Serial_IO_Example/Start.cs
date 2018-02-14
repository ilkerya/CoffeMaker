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
    public partial class Start : Form
    {
        static int time;
        public Start()
        {
            InitializeComponent();
            label1.Text = "Çalışma Sürücüsü : " + Okyanus.Definitions.WorkDrive + Okyanus.Definitions.nl;
            label1.Text += "Çalışma Klasörü : " + Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Log_Directory;
            time = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            if (time > 4)
            {
                Close();

            }
        }

    }
}
