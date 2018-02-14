using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Printing;
namespace Okyanus
{
    public partial class LOGData : Form
    {

       
        public LOGData()
        {
            InitializeComponent();

            this.Text = Okyanus.Chart.VeriFormBasligi;

            label_DataAl.Visible = false;
     //       textBox1.Text = Okyanus.Variables.RawIndex.ToString();
      //      Okyanus.Chart.Renk1 = Color.Blue;
        //      textBox1.Text = 
            textBox_Colour_01.ForeColor = Okyanus.Chart.Renk0;
            textBox_Colour_02.ForeColor = Okyanus.Chart.Renk1;
            textBox_Colour_03.ForeColor = Okyanus.Chart.Renk2;
            textBox_Colour_04.ForeColor = Okyanus.Chart.Renk3;
            textBox_Colour_05.ForeColor = Okyanus.Chart.Renk4;

            textBox_Colour_06.ForeColor = Okyanus.Chart.Renk5;
            textBox_Colour_07.ForeColor = Okyanus.Chart.Renk6;
            textBox_Colour_08.ForeColor = Okyanus.Chart.Renk7;
            textBox_Colour_09.ForeColor = Okyanus.Chart.Renk8;
            textBox_Colour_10.ForeColor = Okyanus.Chart.Renk9;

            textBox_Colour_11.ForeColor = Okyanus.Chart.Renk10;
            textBox_Colour_12.ForeColor = Okyanus.Chart.Renk11;
            textBox_Colour_13.ForeColor = Okyanus.Chart.Renk12;
            textBox_Colour_14.ForeColor = Okyanus.Chart.Renk13;
            textBox_Colour_15.ForeColor = Okyanus.Chart.Renk14;

            textBox_Colour_16.ForeColor = Okyanus.Chart.Renk15;
            textBox_Colour_17.ForeColor = Okyanus.Chart.Renk16;
            textBox_Colour_18.ForeColor = Okyanus.Chart.Renk17;
            textBox_Colour_19.ForeColor = Okyanus.Chart.Renk18;
            textBox_Colour_20.ForeColor = Okyanus.Chart.Renk19;

            textBox_Colour_21.ForeColor = Okyanus.Chart.Renk20;
            textBox_Colour_22.ForeColor = Okyanus.Chart.Renk21;
            textBox_Colour_23.ForeColor = Okyanus.Chart.Renk22;
            textBox_Colour_24.ForeColor = Okyanus.Chart.Renk23;
            textBox_Colour_25.ForeColor = Okyanus.Chart.Renk24;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
       //     fer++
      //      textBox1.Text = Okyanus.Variables.RawIndex.ToString(); 
            LOG_PopulatetexboxandCheckBoxes();
            if (Okyanus.Variables.Log_Status == false) return;

            if (Okyanus.Variables.LOg_ViewTimer != 0)
            {
                Okyanus.Variables.LOg_ViewTimer--;
                if (Okyanus.Variables.LOg_ViewTimer == 0)
                {
                    label_DataAl.Visible = false;
                }
                else label_DataAl.Visible = true;
            }



            if (Okyanus.Variables.UpdateVariables == false) return;
            Okyanus.Variables.UpdateVariables = false;
            String str;
            for (int j = 0; j < Okyanus.Variables.RawIndex; j++)
            {
                switch (j)
                {
                    case 0:
                        if (Okyanus.Chart.TES1307_Enabled == true)
                        {
                            str = (Okyanus.Chart.TES1307_01_Raw / 10).ToString();
                            textBox_Colour_01.Text = "TES1307 1." + " -> " + str;
                        }
                        else
                        {
                            str = Okyanus.Variables.Variables_Data[j];
                            textBox_Colour_01.Text = Okyanus.Variables.Variables_Name[j] + " -> " + str;
                        }
                        break;
                    case 1:
                        if (Okyanus.Chart.TES1307_Enabled == true)
                        {
                            str = (Okyanus.Chart.TES1307_02_Raw / 10).ToString();
                            textBox_Colour_02.Text = "TES1307 2." + " -> " + str;
                        }
                        else
                        {
                            str = Okyanus.Variables.Variables_Data[j];
                            textBox_Colour_02.Text = Okyanus.Variables.Variables_Name[j] + " -> " + str;
                        }                  
                   //     textBox_Colour_02.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; 
                        break;
                    case 2: textBox_Colour_03.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 3: textBox_Colour_04.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 4: textBox_Colour_05.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 5: textBox_Colour_06.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 6: textBox_Colour_07.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 7: textBox_Colour_08.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 8: textBox_Colour_09.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 9: textBox_Colour_10.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 10: textBox_Colour_11.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 11: textBox_Colour_12.Text = Okyanus.Variables.Variables_Name[j] + " -> " + Okyanus.Variables.Variables_Data[j]; break;
                    case 12: textBox_Colour_13.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 13: textBox_Colour_14.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 14: textBox_Colour_15.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 15: textBox_Colour_16.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 16: textBox_Colour_17.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 17: textBox_Colour_18.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 18: textBox_Colour_19.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 19: textBox_Colour_20.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 20: textBox_Colour_21.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 21: textBox_Colour_22.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 22: textBox_Colour_23.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 23: textBox_Colour_24.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                    case 24: textBox_Colour_25.Text = Okyanus.Variables.Variables_Name[j] + "->" + Okyanus.Variables.Variables_Data[j]; break;
                }
            }



        }



        public void LOG_PopulatetexboxandCheckBoxes()
        {
            if (Okyanus.Chart.LogPropUpdate == false) return;

            Okyanus.Chart.LogPropUpdate = false;

            Size = new System.Drawing.Size(163, (60 + (Okyanus.Variables.RawIndex * 24)));
            label_DataAl.Location = new System.Drawing.Point(16, 8 + (Okyanus.Variables.RawIndex * 24));
            //      LOGData.
          

            textBox_Colour_01.Visible = false;
            textBox_Colour_02.Visible = false;
            textBox_Colour_03.Visible = false;
            textBox_Colour_04.Visible = false;
            textBox_Colour_05.Visible = false;
            textBox_Colour_06.Visible = false;
            textBox_Colour_07.Visible = false;
            textBox_Colour_08.Visible = false;
            textBox_Colour_09.Visible = false;
            textBox_Colour_10.Visible = false;
            textBox_Colour_11.Visible = false;
            textBox_Colour_12.Visible = false;
            textBox_Colour_13.Visible = false;
            textBox_Colour_14.Visible = false;
            textBox_Colour_15.Visible = false;
            textBox_Colour_16.Visible = false;
            textBox_Colour_17.Visible = false;
            textBox_Colour_18.Visible = false;
            textBox_Colour_19.Visible = false;
            textBox_Colour_20.Visible = false;
            textBox_Colour_21.Visible = false;
            textBox_Colour_22.Visible = false;
            textBox_Colour_23.Visible = false;
            textBox_Colour_24.Visible = false;
            textBox_Colour_25.Visible = false;


            checkBox_plot0.Visible = false;
            checkBox_plot1.Visible = false;
            checkBox_plot2.Visible = false;
            checkBox_plot3.Visible = false;
            checkBox_plot4.Visible = false;
            checkBox_plot5.Visible = false;
            checkBox_plot6.Visible = false;
            checkBox_plot7.Visible = false;
            checkBox_plot8.Visible = false;
            checkBox_plot9.Visible = false;
            checkBox_plot10.Visible = false;
            checkBox_plot11.Visible = false;
            checkBox_plot12.Visible = false;

            checkBox_plot13.Visible = false;
            checkBox_plot14.Visible = false;
            checkBox_plot15.Visible = false;
            checkBox_plot16.Visible = false;
            checkBox_plot17.Visible = false;
            checkBox_plot18.Visible = false;
            checkBox_plot19.Visible = false;
            checkBox_plot20.Visible = false;
            checkBox_plot21.Visible = false;
            checkBox_plot22.Visible = false;
            checkBox_plot23.Visible = false;
            checkBox_plot24.Visible = false;

            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                switch (i)
                {        //panel_CommandSetup.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
                    case 0: textBox_Colour_01.Visible = true; checkBox_plot0.Visible = true;
                  //      label_DataAl.Location(new System.Drawing.Point(16,32);
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot0.Visible = false;
                        break;
                    case 1: textBox_Colour_02.Visible = true; checkBox_plot1.Visible = true;
                  //      label_DataAl.Location(new System.Drawing.Point(16,32);
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot1.Visible = false;
                        break;
                    case 2: textBox_Colour_03.Visible = true; checkBox_plot2.Visible = true;
                 //       label_DataAl.Location(new System.Drawing.Point(16,32);
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot2.Visible = false;
                        break;
                    case 3: textBox_Colour_04.Visible = true; checkBox_plot3.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot3.Visible = false;
                        break;
                    case 4: textBox_Colour_05.Visible = true; checkBox_plot4.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot4.Visible = false;
                        break;
                    case 5: textBox_Colour_06.Visible = true; checkBox_plot5.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot5.Visible = false;
                        break;
                    case 6: textBox_Colour_07.Visible = true; checkBox_plot6.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot6.Visible = false;
                        break;
                    case 7: textBox_Colour_08.Visible = true; checkBox_plot7.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot7.Visible = false;
                        break;
                    case 8: textBox_Colour_09.Visible = true; checkBox_plot8.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot8.Visible = false;
                        break;
                    case 9: textBox_Colour_10.Visible = true; checkBox_plot9.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot9.Visible = false;

                        break;
                    case 10: textBox_Colour_11.Visible = true; checkBox_plot10.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot10.Visible = false;
                        break;
                    case 11: textBox_Colour_12.Visible = true; checkBox_plot11.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot11.Visible = false;
                        break;
                    case 12: textBox_Colour_13.Visible = true; checkBox_plot12.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot12.Visible = false;
                        break;
                    case 13: textBox_Colour_14.Visible = true; checkBox_plot13.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot13.Visible = false;
                        break;
                    case 14: textBox_Colour_15.Visible = true; checkBox_plot14.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot14.Visible = false;
                        break;
                    case 15: textBox_Colour_16.Visible = true; checkBox_plot15.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot15.Visible = false;
                        break;
                    case 16: textBox_Colour_17.Visible = true; checkBox_plot16.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot16.Visible = false;
                        break;
                    case 17: textBox_Colour_18.Visible = true; checkBox_plot17.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot17.Visible = false;
                        break;
                    case 18: textBox_Colour_19.Visible = true; checkBox_plot18.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot18.Visible = false;
                        break;
                    case 19: textBox_Colour_20.Visible = true; checkBox_plot19.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot19.Visible = false;
                        break;
                    case 20: textBox_Colour_21.Visible = true; checkBox_plot20.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot20.Visible = false;
                        break;
                    case 21: textBox_Colour_22.Visible = true; checkBox_plot21.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot21.Visible = false;
                        break;
                    case 22: textBox_Colour_23.Visible = true; checkBox_plot22.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot22.Visible = false;
                        break;
                    case 23: textBox_Colour_24.Visible = true; checkBox_plot23.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot23.Visible = false;
                        break;
                    case 24: textBox_Colour_25.Visible = true; checkBox_plot24.Visible = true;
                        if (Okyanus.Variables.Var_PlotOnOff[i] == 1) checkBox_plot24.Visible = false;
                        break;
                }
            }
    //        for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
    //        {

     //       this.Size = new System.Drawing.Size( 163,  (20+(Okyanus.Variables.RawIndex*24)));
   //         }

        }
        public void CHART_UpdateEnabledGraph(object sender, EventArgs e)
        {
            
            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                switch (i)
                {
                    case 0:
                        if (checkBox_plot0.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 1:
                        if (checkBox_plot1.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 2:
                        if (checkBox_plot2.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 3:
                        if (checkBox_plot3.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 4:
                        if (checkBox_plot4.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 5:
                        if (checkBox_plot5.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 6:
                        if (checkBox_plot6.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 7:
                        if (checkBox_plot7.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 8:
                        if (checkBox_plot8.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 9:
                        if (checkBox_plot9.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 10:
                        if (checkBox_plot10.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 11:
                        if (checkBox_plot11.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 12:
                        if (checkBox_plot12.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 13:
                        if (checkBox_plot13.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 14:
                        if (checkBox_plot14.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 15:
                        if (checkBox_plot15.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 16:
                        if (checkBox_plot16.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 17:
                        if (checkBox_plot17.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 18:
                        if (checkBox_plot18.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 19:
                        if (checkBox_plot19.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 20:
                        if (checkBox_plot20.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 21:
                        if (checkBox_plot21.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 22:
                        if (checkBox_plot22.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 23:
                        if (checkBox_plot23.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    case 24:
                        if (checkBox_plot24.Checked == true) Okyanus.Chart.GraphEnable[i] = true;
                        else Okyanus.Chart.GraphEnable[i] = false;
                        break;
                    default: break;
                }
            } 
            
        }
        private void CheckUpdate(object sender, EventArgs e)
        {
            //Okyanus.Chart.LogPropUpdate = true;
        }

   //    private void CHART_UpdateEnabledGraph()
  //      {

   //     }


        private void LOGData_Load(object sender, EventArgs e)
        {
            // ilk acilista
         //   label_DataAl.Text = "ilk aclils";
            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                switch (i)
                {
                    case 0:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot0.Checked = true;
                        else checkBox_plot0.Checked = false;
                        break;
                    case 1:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot1.Checked = true;
                        else checkBox_plot1.Checked = false;
                        break;
                    case 2:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot2.Checked = true;
                        else checkBox_plot2.Checked = false;
                        break;
                    case 3:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot3.Checked = true;
                        else checkBox_plot3.Checked = false;
                        break;
                    case 4:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot4.Checked = true;
                        else checkBox_plot4.Checked = false;
                        break;
                    case 5:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot5.Checked = true;
                        else checkBox_plot5.Checked = false;
                        break;
                    case 6:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot6.Checked = true;
                        else checkBox_plot6.Checked = false;
                        break;
                    case 7:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot7.Checked = true;
                        else checkBox_plot7.Checked = false;
                        break;
                    case 8:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot8.Checked = true;
                        else checkBox_plot8.Checked = false;
                        break;
                    case 9:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot9.Checked= true;
                        else checkBox_plot9.Checked = false;
                        break;
                    case 10:
                        if (Okyanus.Chart.GraphEnable[i] == true) checkBox_plot10.Checked = true;
                        else checkBox_plot10.Checked = false;
                        break;
                    case 11:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot11.Checked = true;
                        else checkBox_plot11.Checked = false;
                        break;
                    case 12:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot12.Checked = true;
                        else checkBox_plot12.Checked = false;
                        break;
                    case 13:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot13.Checked = true;
                        else checkBox_plot13.Checked = false;
                        break;
                    case 14:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot14.Checked = true;
                        else checkBox_plot14.Checked = false;
                        break;
                    case 15:
                        if ( Okyanus.Chart.GraphEnable[i] == true) checkBox_plot15.Checked = true;
                        else checkBox_plot15.Checked = false;
                        break;
                    case 16:
                        if ( Okyanus.Chart.GraphEnable[i] == true) checkBox_plot16.Checked = true;
                        else checkBox_plot16.Checked = false;
                        break;
                    case 17:
                        if ( Okyanus.Chart.GraphEnable[i] == true) checkBox_plot17.Checked = true;
                        else checkBox_plot17.Checked = false;
                        break;
                    case 18:
                        if ( Okyanus.Chart.GraphEnable[i] == true) checkBox_plot18.Checked = true;
                        else checkBox_plot18.Checked = false;
                        break;
                    case 19:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot19.Checked = true;
                        else checkBox_plot19.Checked = false;
                        break;
                    case 20:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot20.Checked = true;
                        else checkBox_plot20.Checked = false;
                        break;
                    case 21:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot21.Checked = true;
                        else checkBox_plot21.Checked = false;
                        break;
                    case 22:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot22.Checked = true;
                        else checkBox_plot22.Checked = false;
                        break;
                    case 23:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot23.Checked = true;
                        else checkBox_plot23.Checked = false;
                        break;
                    case 24:
                        if (Okyanus.Chart.GraphEnable[i]  == true) checkBox_plot24.Checked = true;
                        else checkBox_plot24.Checked = false;
                        break;
                    default: break;
                }
            }

        }
    }
}

