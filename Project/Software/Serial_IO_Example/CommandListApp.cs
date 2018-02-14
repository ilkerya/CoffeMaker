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
    public partial class CommandListApp : Form
    {
        public CommandListApp()
        {
            InitializeComponent();
            textBox3.Text = "                Komut Sutununa Numarasini yada ismini girin";
            textBox3.Enabled = false;
       //     String str = "  Komut Sutununa Numarasini " + System.Environment.NewLine + "      yada ismini girin" + System.Environment.NewLine;
            String str = " ";
            int Counter = 0;
            str  = Counter.ToString() + "  = " + Okyanus.Definitions.Komut00.ToString() + System.Environment.NewLine;Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut01.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut02.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut03.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut04.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut05.ToString() + System.Environment.NewLine;Counter++;//5
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut06.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut07.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut08.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut09.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut10.ToString() + System.Environment.NewLine;Counter++;//10
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut11.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut12.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut13.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut14.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut15.ToString() + System.Environment.NewLine;Counter++;//15
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut16.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut17.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut18.ToString() + System.Environment.NewLine;Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut19.ToString() + System.Environment.NewLine; Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut20.ToString() + System.Environment.NewLine; Counter++;//20
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut21.ToString() + System.Environment.NewLine; Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut22.ToString() + System.Environment.NewLine; Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut23.ToString() + System.Environment.NewLine; Counter++;
            str += Counter.ToString() + " = " + Okyanus.Definitions.Komut24.ToString() + System.Environment.NewLine; Counter++;
            
            textBox1.Text = str;
            textBox1.Enabled = false;


       //     Counter = 0;
            str  = Counter.ToString() + "  = " + Okyanus.Definitions.Komut25.ToString() + System.Environment.NewLine; Counter++;
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut26.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut27.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut28.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut29.ToString() + System.Environment.NewLine; Counter++;//0
 

            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut30.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut31.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut32.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut33.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut34.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut35.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut36.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut37.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut38.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut39.ToString() + System.Environment.NewLine; Counter++;//0

            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut40.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut41.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut42.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut43.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut44.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut45.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut46.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut47.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut48.ToString() + System.Environment.NewLine; Counter++;//0
            str += Counter.ToString() + "  = " + Okyanus.Definitions.Komut49.ToString() + System.Environment.NewLine; 

            textBox2.Text = str;
            textBox2.Enabled = false;



        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }




    }
}
