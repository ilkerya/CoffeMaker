using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;




using NPlot.Bitmap;
using System.Collections;


using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using System.IO.Ports;
using System.IO;
using GridWork;
using System.Threading;
using LumenWorks.Framework.IO.Csv;
using Okyanus;
using System.Diagnostics;
using System.Drawing.Printing;

using Serial_IO_Example;

namespace Okyanus
{
    public partial class ClearChart : Form
    {
        public ClearChart()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Okyanus.Chart.ClearChart = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
