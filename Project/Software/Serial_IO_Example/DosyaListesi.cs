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
    public partial class DosyaListesi : Form
    {
        public DosyaListesi()
        {
            InitializeComponent();

            System.IO.DriveInfo di = new System.IO.DriveInfo(@"C:\");
            //         Console.WriteLine(di.TotalFreeSpace);
            //         Console.WriteLine(di.VolumeLabel);

            System.IO.DirectoryInfo dirInfo = di.RootDirectory;
            //       Console.WriteLine(dirInfo.Attributes.ToString());



            String str = "Default Calisma Surucusu = C: " + System.Environment.NewLine;
                  str += "Gecerli Calisma Surucusu = " + Okyanus.Definitions.WorkDrive + System.Environment.NewLine;
                  str += System.Environment.NewLine + System.Environment.NewLine;
                  str += "Okyanus Emulator.exe Calisma Klasoru = " + System.IO.Directory.GetCurrentDirectory() + System.Environment.NewLine;

                  str += "Default Kayit Klasoru  = " + Okyanus.Definitions.Log_Directory + System.Environment.NewLine;
        //    str += "Default Log Dosyasi = " + Okyanus.Definitions.Original_Log_File + System.Environment.NewLine;
       //     str += "Default FlashDosya Klasoru = " + Okyanus.Definitions.Config_Directory + System.Environment.NewLine;

            str += System.Environment.NewLine;
            str += "Default Log Dosyasi      = " + Okyanus.Definitions.ConfigLog_File + System.Environment.NewLine;
            str += "Default Flash100 Dosyasi = " + Okyanus.Definitions.FlashCommand_File + System.Environment.NewLine;
            str += "Default Kahve Makinasi2 Flash Dosyasi = " + Okyanus.Definitions.FlashCommand500byte_Kahve2 + System.Environment.NewLine;
            str += "Default Tost Makinasi Flash Dosyasi   = " + Okyanus.Definitions.FlashCommand500byte_Tost + System.Environment.NewLine;

           


            //     String.Format("{0}", AssemblyTitle, "  Hakkinda:");

            str += System.Environment.NewLine + System.Environment.NewLine;
            str += "Gecerli Log Dosyasi      = " + Okyanus.Variables.LogFileName + System.Environment.NewLine;
            str += "Gecerli Flash100 Dosyasi = " + Okyanus.Variables.CommandFileName + System.Environment.NewLine;
            str += "Gecerli Kahve Makinasi2 Flash Dosyasi = " + Okyanus.Variables.FlashCommand500byte_Kahve2 + System.Environment.NewLine;
            str += "Gecerli Tost Makinasi  Flash Dosyasi  = " + Okyanus.Variables.FlashCommand500byte_Tost + System.Environment.NewLine;


            textBox1.Text = str;
            textBox2.Text = "";
            textBox2.Visible = false;
            textBox1.Enabled = false;
        }
    }
}
