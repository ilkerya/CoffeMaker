
#define DEBUGw
//#define SP1_BUFFER 0x34


using System;
using System.Collections.Generic;
using System.ComponentModel;

using NPlot.Bitmap;
using  System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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

namespace Serial_IO_Example
{

    /*
    struct SeriPodrt1
    {

        uint Length;
        int One; //
        int Two;//
        int Three;//
        int Four;//
        int CRC;
        uint CRC_Calc;
        uint CRC_Error;
    };
*/

    /*
    class MySleepThread
    {
        private int sleepTime;
        private static Random random = new Random();

        public MySleepThread()
        {
            sleepTime = random.Next(10000);

        }
        public void MySleepmethod()
        {
            Thread current = Thread.CurrentThread;
            System.Diagnostics.Debug.WriteLine(current.Name + "sleeping for" + sleepTime);

            //
            Thread.Sleep(sleepTime);
            System.Diagnostics.Debug.WriteLine(current.Name + "awake");
        }

    }
*/
   // private PrintDocument printDocument1 = new PrintDocument();
    
    public partial class Serial_test : Form
    {
    //    int LOg_ViewTimer;
        private PrintDocument printDocument1 = new PrintDocument();
        // global variables here
        int BaseCounter;
        int Timer_1sec;
        int TimerShareBase;
     //   bool TimerShareEnable;
        bool Enable_DATA_CommandFlashArray_ReadFill_Grid;
                          String TES1307_01 ;
                          String TES1307_02;

        //    int Update_Chart;

        //     static int[] SerialDataF = new int[88];
        //      static String[] DataS = new String[44];
                          byte Preamble_Test = 0;
                          byte CRC_Test = 0;

                          int GidenPaket = 0;

        //   uint SP1_Preamble;
        //     uint SP1_Length = 88;
        uint SP1_Data1 = 0;
        uint SP1_Data2 = 0;
        uint SP1_Data3 = 0;
        uint SP1_Data4 = 0;
        uint SP1_Data5 = 0;
        uint SP1_CRC = 0;

        uint SP1_RecCommand = 0;
        uint SP1_RecAdress = 0;
        uint SP1_RecAdrLength = 0;

        static byte[] SP1_ReceiveBuf = new byte[256]; // preamble+length+crc + 4*4 byte data  4 byte + 4 byte + 4 byte + 16 byte
        static byte[] SP1_SendBuf = new byte[256]; // preamble+length+crc + 4*4 byte data  4 byte + 4 byte + 4 byte + 16 byte

        static byte[] SP1_Buffer = new byte[256];

        static byte[] SP1_FirstBuffer = new byte[256];


        UInt32 SP1_Send_TotLength;
        UInt32 SP1_SendCommand;
        uint SP1_SendAdress;
        uint SP1_Send_AdrLength;
        uint SP1_SendData1 = 234;
        uint SP1_SendData2 = 567;
        uint SP1_SendData3 = 34567;
        uint SP1_SendData4 = 11134;
        uint SP1_SendData5 = 11134;
        uint SP1_SendCRC;
        int SP2_ComTimeout;
        int SP2_ErrorCount;
        uint SP2_ReadSequence;

        uint SP2_Preamble;
        uint SP2_Length = 88;
        uint SP2_Data1;
        uint SP2_Data2;
        uint SP2_Data3;
        uint SP2_Data4;
        uint SP2_Data5;
        uint SP2_CRC;

        uint SP2_RecCommand;
        uint SP2_RecAdress;
        uint SP2_RecAdrLength;

        static byte[] SP2_ReceiveBuf = new byte[256]; // preamble+length+crc + 4*4 byte data  4 byte + 4 byte + 4 byte + 16 byte
        static byte[] SP2_SendBuf = new byte[256]; // preamble+length+crc + 4*4 byte data  4 byte + 4 byte + 4 byte + 16 byte
        static byte[] SP2_Buffer = new byte[256];


        uint SP2_SendCommand;
        uint SP2_SendAdress;
        uint SP2_Send_AdrLength;
        uint SP2_SendData1 = 378;
        uint SP2_SendData2 = 4567;
        uint SP2_SendData3 = 834;
        uint SP2_SendData4 = 12;
        uint SP2_SendData5 = 56134;
        uint SP2_SendCRC;

        uint SP2_CRC_Calc;
        uint SP2_CRC_Error;

        static readonly UInt16 SP1_DEFAULT_LENGTH = 32; // LENGTH 28


        UInt16 SP2_DEFAULT_LENGTH = 32; // LENGTH 28
        //       UInt16 PREAMBLE_BYTES + DATA_BYTES
        // constant variables
        UInt16 PREAMBLE_BYTES = 4; // preamble+length 4 byte + 2 byte
        UInt16 DATALENGTH_BYTES = 2;
   //     UInt16 PREAMBLE_LENGTH = 8; // preamble+length 4 byte + 4 byte
        UInt32 DEFAULT_PREAMBLE = 0XAAAAAAAA; // preamble+length 4 byte + 4 byte
        //     UInt32 DEFAULT_LENGTH =       0X0000001C; // LENGTH 28
        UInt32 DEFAULT_CRC_INIT = 0X55555555; // preamble+length 4 byte + 4 byte

        UInt16 SHIFT24 = 24;
        UInt16 SHIFT16 = 16;
        UInt16 SHIFT8 = 8;

        static String SP1_PortConnectName;
        static String SP2_PortConnectName;
        //  static String PortConnectSituation;
        static String nl = Environment.NewLine;

        //public static string[] ComponentArray4 = new string[1000];  //            ComponentArray4[0] = "g";
        // end of global variables decleration
        public void GetWorkingDrive()
        {
            String Drive1 = "C:\\";
            String Drive2 = "D:\\";


            for (int i = 0; i < Okyanus.Variables.Drives.Length; i++)
            {
                if (Drive1 == Okyanus.Variables.Drives[i])
                {
                    Okyanus.Definitions.WorkDrive = Drive1;
                    return;
                }
            }  // oncelik yukardakinde
            for (int i = 0; i < Okyanus.Variables.Drives.Length; i++)
            {          
                if (Drive2 == Okyanus.Variables.Drives[i])
                {
                    Okyanus.Definitions.WorkDrive = Drive2;
                    return;
                }
            }
            MessageBox.Show("Bilgisayara bagli" + Drive1 + " ve " +Drive2+  "suruculeri bulunamadi !!!");
        }
        public void Byte500_StartupConfigDisabled()
        {
            buyukFlashOkumaToolStripMenuItem.Visible = false;
            buyukFlashOkumaToolStripMenuItem.Enabled = false;
            Yazma500Basarili2.Enabled = false;
            Yazma500Basarili2.Visible = false;
            button500byteFlash1.Enabled = false;
            button500byteFlash1.Visible = false;
            button500byteOku2.Enabled = false;
            button500byteOku2.Visible = false;

            button_Flash500_dosyaoku.Enabled = false;
            button_Flash500_dosyaoku.Visible = false;

            button_Flash500_dosyaya.Enabled = false;
            button_Flash500_dosyaya.Visible = false;

            if (panel_500CommandSetup.Visible == true)
            {
                panel_500CommandSetup.Visible = false;
                panel_Communication.Visible = true;
            }
        }
        public void Byte500_StartupConfigEnabled()
        {
            buyukFlashOkumaToolStripMenuItem.Visible = true;
            buyukFlashOkumaToolStripMenuItem.Enabled = true;
            Yazma500Basarili2.Enabled = true;
            Yazma500Basarili2.Visible = true;
            button500byteFlash1.Enabled = true;
            button500byteFlash1.Visible = true;
            button500byteOku2.Enabled = true;
            button500byteOku2.Visible = true;
            button_Flash500_dosyaoku.Enabled = true;
            button_Flash500_dosyaoku.Visible = true;
            button_Flash500_dosyaya.Enabled = true;
            button_Flash500_dosyaya.Visible = true;
        }
        public void FlashWrite_StartupConfigEnabled()
        {
            button_CommandStart.Enabled = true;
            button_CommandRead.Enabled = true;
            button_CommandStart.Visible = true;
            button_CommandRead.Visible = true;
            commandSetupToolStripMenuItem.Visible = true;
            YazmaBasarili2.Visible = true;
        }
        public void FlashWrite_StartupConfigDisabled()
        {
            button_CommandStart.Enabled = false;
            button_CommandRead.Enabled = false;
            button_CommandStart.Visible = false;
            button_CommandRead.Visible = false;
            commandSetupToolStripMenuItem.Visible = false;
            YazmaBasarili2.Visible = false;



      //      panel_Communication.Visible = true;
      //      panel_Log.Visible = false; panel_Colour.Visible = false;
      //      panel_Configuration.Visible = false;
      //      panel_CommandSetup.Visible = false;
      //      panel_Simulasyon.Visible = false;


            if (panel_CommandSetup.Visible == true)
            {
                panel_CommandSetup.Visible = false;
                panel_Flash_Combobox.Visible = false;
                panel_Flash_Numeric.Visible = false;
                panel_Communication.Visible = true;

            }
        }
        private Color CustomColor()
        {
            Color myColor = new Color();
        //    myColor = Color.FromRgb(0, 255, 0);
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    myColor = Color.BlanchedAlmond;

                    
                   break;
                case Okyanus.Definitions.BJUEmulator:
                   myColor = Color.PaleTurquoise;

                   
                   break;
                case Okyanus.Definitions.KahveMakinasiEmulator2:
                   myColor = Color.BurlyWood;

                    
                   break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2:      
                   myColor = Color.Khaki;

                   
                   break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:
                   myColor = Color.LightBlue;


                   break;



                default:

                   break;
            }
            bJUKisitliToolStripMenuItem.BackColor = Color.BlanchedAlmond;
            bJUEmulatorToolStripMenuItem.BackColor = Color.PaleTurquoise;
            kahveMakinasiToolStripMenuItem.BackColor = Color.BurlyWood;
            tostMakinasiToolStripMenuItem.BackColor = Color.Khaki;
            kahveMakinasi_1ToolStripMenuItem.BackColor = Color.LightBlue;
            

            return myColor;
        }








        public void Versiyon_RenkSecim()
        {
            this.ShowIcon = true;
            this.ShowInTaskbar = true;
            this.BackColor = CustomColor();
            panel_500CommandSetup.BackColor = CustomColor();
            panel_Colour.BackColor = CustomColor();
            panel_CommandSetup.BackColor = CustomColor();
            panel_Flash_Combobox.BackColor = CustomColor();
            panel_Flash_Numeric.BackColor = CustomColor();
					
			
            panel_Communication.BackColor = CustomColor();
            panel_Configuration.BackColor = CustomColor();
            panel_Log.BackColor = CustomColor();
            panel_Simulasyon.BackColor = CustomColor();

            SP1_DataRcvtextBox.BackColor = CustomColor();
            SP1_DatatextBox.BackColor = CustomColor(); ;
            SP2_richTextBox.BackColor = CustomColor();
            SP2_SendtextBox.BackColor = CustomColor();
            SP2_DatatextBox.BackColor = CustomColor();
            textBox_SP3_Receive.BackColor = CustomColor();
            SP2_textBox_PortName.BackColor = CustomColor();
            textBox__ConnectionSP3.BackColor = CustomColor();
            SP1_textBox_PortName.BackColor = CustomColor();
            textBox_CommandFileName.BackColor = CustomColor();
            //        dt_500Command.BackColor = CustomColor();
            dt_500Command.BackgroundColor = CustomColor();
            //         dt_Command.BackColor = CustomColor();
            dt_Command.BackgroundColor = CustomColor();
            //         dt.BackColor = CustomColor();
            dt.BackgroundColor = CustomColor();
            textBox_Log_FileName.BackColor = CustomColor();
            //          numericUpDown_ChartMax.BackColor = CustomColor(); 
            //          numericUpDown_ChartMin.BackColor = CustomColor();
            plotSurface2D1.BackColor = CustomColor();
            textBox_500byte1.BackColor = CustomColor();
            textBox_500byte2.BackColor = CustomColor();
            menuStrip1.BackColor = CustomColor();

            textBoxTost1.BackColor = CustomColor();
            textBoxTost2.BackColor = CustomColor();
            textBoxTost3.BackColor = CustomColor();
            textBoxTost4.BackColor = CustomColor();
            textBoxTost5.BackColor = CustomColor();
            textBoxTost6.BackColor = CustomColor();
            textBoxTost7.BackColor = CustomColor();
            textBoxTost8.BackColor = CustomColor();
            textBoxTost9.BackColor = CustomColor();
            textBoxTost10.BackColor = CustomColor();
            textBoxTost11.BackColor = CustomColor();

            fileToolStripMenuItem.BackColor = CustomColor();
            dataLoggingToolStripMenuItem.BackColor = CustomColor();
            configurationToolStripMenuItem.BackColor = CustomColor();
            commandSetupToolStripMenuItem.BackColor = CustomColor();
            buyukFlashOkumaToolStripMenuItem.BackColor = CustomColor();
            helpToolStripMenuItem.BackColor = CustomColor();
            takeScreenShotToolStripMenuItem.BackColor = CustomColor();
            exitToolStripMenuItem.BackColor = CustomColor();
            emulatorTipiToolStripMenuItem.BackColor = CustomColor();
            fileSaveAsToolStripMenuItem.BackColor = CustomColor();
            stopToolStripMenuItem1.BackColor = CustomColor();
            samplingTimeToolStripMenuItem.BackColor = CustomColor();
            viewToolStripMenuItem1.BackColor = CustomColor();
            systemTimeToolStripMenuItem1.BackColor = CustomColor();
            verileriAcToolStripMenuItem.BackColor = CustomColor();
            grafigiDurdurToolStripMenuItem.BackColor = CustomColor();
            clearToolStripMenuItem.BackColor = CustomColor();
            stopTimeToolStripMenuItem1.BackColor = CustomColor();
            startTimeToolStripMenuItem1.BackColor = CustomColor();
            dataLogOnOffToolStripMenuItem1.BackColor = CustomColor();
            sToolStripMenuItem.BackColor = CustomColor();
            saniyeToolStripMenuItem.BackColor = CustomColor();
            saniyeToolStripMenuItem1.BackColor = CustomColor();
            saniyeToolStripMenuItem2.BackColor = CustomColor();
            saniyeToolStripMenuItem3.BackColor = CustomColor();
            saveAsConfigToolStripMenuItem.BackColor = CustomColor();
            openConfigToolStripMenuItem.BackColor = CustomColor();
            SaveAsCommandtoolStripMenuItem2.BackColor = CustomColor();
            OpenCommandtoolStripMenuItem3.BackColor = CustomColor();
            dosyadanFlashTablosuAcmaToolStripMenuItem.BackColor = CustomColor();
            aboutToolStripMenuItem.BackColor = CustomColor();
            //compiileTimeVersionToolStripMenuItem.BackColor = CustomColor();

            YazmaBasarili.BackColor = CustomColor();
            YazmaBasarili2.BackColor = CustomColor();
            Yazma500Basarili2.BackColor = CustomColor();

            flashTablosundanDosyayaKaydetToolStripMenuItem.BackColor = CustomColor();
            emulatorEkraniToolStripMenuItem.BackColor = CustomColor();
            tES1307EToolStripMenuItem.BackColor = CustomColor();

            dt.GridColor = CustomColor();
            dt_500Command.GridColor = CustomColor();
            dt_500Command.BackgroundColor = CustomColor();
            dt_500Command2.GridColor = CustomColor();
            dt_500Command2.BackgroundColor = CustomColor();
            dt_Command.GridColor = CustomColor();


            komutListesiToolStripMenuItem.BackColor = CustomColor();
            dosyaListesiToolStripMenuItem.BackColor = CustomColor();
      //      dt_500Command
           

        }

        public void Versiyon_Karsilastirma()
        {
            Versiyon_RenkSecim();

            // jpg ler farkli yuklenecek
            // jpg deki degiskenler farkli yuklenecek
            // menude 500 byte yazma olmayacak
            // yazma kisitlamalari olacak
            //
     //       Okyanus.Variables.Version =  Okyanus.Definitions.BJUEmulatorKisitli;
   //         Okyanus.Variables.Version = Okyanus.Definitions.KahveMakinasiEmulator2;
    //        Okyanus.Variables.Version = Okyanus.Definitions.BJUEmulator;
    //        Okyanus.Variables.Version = Okyanus.Definitions.TostMakinasiEmulatoru2

            
     //       System.Drawing.Icon ico = new System.Drawing.Icon("coffee-maker-32.ico");
    //        System.Drawing.Icon ico = new System.Drawing.Icon("coffee-maker-32.ico");

            string DIR = Directory.GetCurrentDirectory() + "\\Pictures\\";

            string UTU = DIR ;
            string KAHVE = DIR;
            string TOST = DIR;

            string ICON_FINAL;
      //      string pic = UTU;
      //      pic = "Simulasyon_Ekran_BJU.jpg";
        
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    this.Text = "BJU Emulator Data Logger";
                    FlashWrite_StartupConfigDisabled();
                    Byte500_StartupConfigDisabled();

                     UTU += "Iron1.ico";
                     ICON_FINAL = UTU;


                    break;
                case Okyanus.Definitions.BJUEmulator:
                    this.Text = "BJU Emulator V";
                    FlashWrite_StartupConfigEnabled();
                    Byte500_StartupConfigDisabled();

                     UTU += "Iron2.ico";
                     ICON_FINAL = UTU;

                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2:
                    this.Text = "Kahve Makinesi Emulator 2";
               //     FlashWrite_StartupConfigEnabled();
                    FlashWrite_StartupConfigDisabled();
                    Byte500_StartupConfigEnabled();

                    KAHVE += "Kahve.ico";
                     ICON_FINAL = KAHVE;


                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:
                    this.Text = "Kahve Makinesi Emulator";
                    FlashWrite_StartupConfigEnabled(); // BU DEGISECEK
                    Byte500_StartupConfigDisabled();

                    KAHVE += "Kahve.ico";
                    ICON_FINAL = KAHVE;


                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2 :
                    this.Text = "Tost Makinesi Emulator 2";
            //        FlashWrite_StartupConfigEnabled();
                    FlashWrite_StartupConfigDisabled();
                    Byte500_StartupConfigEnabled();
                    TOST += "Toaster.ico";
                    ICON_FINAL = TOST;

                    break;




                default:
                    this.Text = "Model secilmedi !!!";
                    FlashWrite_StartupConfigDisabled();
                    Byte500_StartupConfigDisabled();
                    UTU += "Iron1.ico";
                    ICON_FINAL = UTU;
                    break;

            }

            

            if ((File.Exists(ICON_FINAL)))
            {
                System.Drawing.Icon ico = new System.Drawing.Icon(ICON_FINAL);
                this.Icon = ico;
            }
            else MessageBox.Show("Icon Resmi yok");


        }
        public void BJU_emulator_yazma_kisitli_mi()
        {
            Okyanus.Definitions.BJU_emulator_yazma_kisitli_V = false; // evet
      //      Okyanus.Definitions.BJU_emulator_yazma_kisitli_V = false; // hayir
            
            if (Okyanus.Definitions.BJU_emulator_yazma_kisitli_V == false)
            {
                this.Text = "BJU Emulator V";



            
            }

            if (Okyanus.Definitions.BJU_emulator_yazma_kisitli_V == true)
            {
                commandSetupToolStripMenuItem.Visible = false;
                this.Text = "BJU Emulator Data Logger";
                button_CommandStart.Enabled = false;
                button_CommandRead.Enabled = false;
                button_CommandStart.Visible = false;
                button_CommandRead.Visible = false;
                YazmaBasarili2.Visible = false;
            }

        }

  //      GridHelper gridHelper500 = new GridHelper(dt_500Command);
  //      DataTable dataElements = gridHelper500.GetDataTable4_500ByteCommand();

        public Serial_test()
        {

            GetWorkingDrive();

            Start form = new Start();
            form.ShowDialog();


         
            Okyanus.Variables.Log_LogSampleTime = 1;
            

            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            SP1_IO_Serial_UpdateCOMPortList();
            SP2_IO_Serial_UpdateCOMPortList();
            communicationToolStripMenuItem_Click(this, null);
            label_DataAl.Visible = false;
            Okyanus.Variables.Log_LogSampleTime = 22;// 1sn
            samplingTimeToolStripMenuItem.Text = "Ornekleme Suresi ...." + "1 Saniye Aktif";
            //     Serial_IO_Example.Serial_test.ActiveForm.Width = 1500;
            //      Serial_IO_Example.Serial_test.ActiveForm.Height = 800;

            //    Serial_test.ActiveForm.Size.Width = 1500;
            //    Serial_test.ActiveForm.Size.Height = System.Int32.
            Okyanus.Chart.Renk0 = Color.Blue;
            Okyanus.Chart.Renk1 = Color.Red;
            Okyanus.Chart.Renk2 = Color.Black;
            Okyanus.Chart.Renk3 = Color.Green;
            Okyanus.Chart.Renk4 = Color.Goldenrod;

            Okyanus.Chart.Renk5 = Color.Orange;
            Okyanus.Chart.Renk6 = Color.Olive;
            Okyanus.Chart.Renk7 = Color.Purple;
            Okyanus.Chart.Renk8 = Color.MediumSeaGreen;
            Okyanus.Chart.Renk9 = Color.Thistle;

            Okyanus.Chart.Renk10 = Color.MediumTurquoise;
            Okyanus.Chart.Renk11 = Color.IndianRed;
            Okyanus.Chart.Renk12 = Color.GreenYellow;
            Okyanus.Chart.Renk13 = Color.PaleVioletRed;
            Okyanus.Chart.Renk14 = Color.DarkBlue;

            Okyanus.Chart.Renk15 = Color.DarkCyan;
            Okyanus.Chart.Renk16 = Color.DarkGray;
            Okyanus.Chart.Renk17 = Color.Tan;
            Okyanus.Chart.Renk18 = Color.SandyBrown;
            Okyanus.Chart.Renk19 = Color.Gray;

            Okyanus.Chart.Renk20 = Color.SlateBlue;
            Okyanus.Chart.Renk21 = Color.Lime;
            Okyanus.Chart.Renk22 = Color.Salmon;
            Okyanus.Chart.Renk23 = Color.HotPink;
            Okyanus.Chart.Renk24 = Color.Yellow;

            this.Width = Okyanus.Variables.Width_Form;
            this.Height = Okyanus.Variables.Height_Form;

     //       BJU_emulator_yazma_kisitli_mi();
            Okyanus.Variables.Version = Okyanus.Definitions.BJUEmulatorKisitli; // default     
            panel_Additional_Communication.Visible = false;
            Versiyon_Karsilastirma();
            INITILAZE_Arrays_ComboBoxes_NumericBoxes();

            ChartPlayPause();     
            Okyanus.Variables.OpenLogFileatInit = true ;
            openConfigToolStripMenuItem_Click(this,null);



            OpenCommandtoolStripMenuItem3_Click(this, null);
            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                Okyanus.Chart.GraphEnable[i] = true;
            }
    //        String[] drives = Environment.GetLogicalDrives();
       //     Okyanus.Variables.Drives

//            LOG_texBoxesColoursSelect();
            checkBox_TES1307_CheckedChanged(this, null);

            DATA_CommandArray_Table_Delete();

            Okyanus.Variables.OpenCommandFlashFileatInit = true;
            File_Open_FlashCommandFile();

            Okyanus.Variables.OpenCommandFileatInit = false;
            //      File_Open_CommandFile();
            Okyanus.Variables.OpenCommandFile500atInit2 = true;
            File_Open_500_FlashCommandFile_Tost2();
            Okyanus.Variables.OpenCommandFile500atInit = true;
            File_Open_500_FlashCommandFile_Kahve2();

        }
        void INITILAZE_Arrays_ComboBoxes_NumericBoxes()
        {
            Flash500ByteWrite = false;
            Flash500ByteWriteEnable = false;
            Okyanus.Variables.PlotOnOffArr[0] = 0;
            Okyanus.Variables.PlotOnOffArr[1] = 1;
            comboBox_Chart_On.Items.Add("Var");
            comboBox_Chart_On.Items.Add("Yok");

            Okyanus.Variables.PlotMultArr[0] = Okyanus.Definitions.PlotMul0001; // x0.001
            comboBox_Chart_Multiply.Items.Add(Okyanus.Definitions.Mul_0001);
            Okyanus.Variables.PlotMultArr[1] = Okyanus.Definitions.PlotMul001; // x0.01
            comboBox_Chart_Multiply.Items.Add(Okyanus.Definitions.Mul_001);
            Okyanus.Variables.PlotMultArr[2] = Okyanus.Definitions.PlotMul01; //x0.1
            comboBox_Chart_Multiply.Items.Add(Okyanus.Definitions.Mul_01);
            Okyanus.Variables.PlotMultArr[3] = Okyanus.Definitions.PlotMul1; // x1
            comboBox_Chart_Multiply.Items.Add(Okyanus.Definitions.Mul_1);
            Okyanus.Variables.PlotMultArr[4] = Okyanus.Definitions.PlotMul10;// x10
            comboBox_Chart_Multiply.Items.Add(Okyanus.Definitions.Mul_10);
            Okyanus.Variables.PlotMultArr[5] = Okyanus.Definitions.PlotMul100;// x100
            comboBox_Chart_Multiply.Items.Add(Okyanus.Definitions.Mul_100);
            Okyanus.Variables.PlotMultArr[6] = Okyanus.Definitions.PlotMul1000;// x1000
            comboBox_Chart_Multiply.Items.Add(Okyanus.Definitions.Mul_1000);

         //       comboBox_Chart_Multiply.SelectionStart = 3;

     //       for (int i = 0; i < Okyanus.Variables.PlotMultArr.GetLength(0); i++)
      //          comboBox_Chart_Multiply.Items.Add("x" + (Okyanus.Variables.PlotMultArr[i]));

            for (int i = 0; i < 256; i++)
            {
                Okyanus.Variables.NameArr[i] = "Degisken" + i.ToString();
            }

            for (int i = 0; i < 256; i++)
            {
                Okyanus.Variables.PlotColorArr[i] = i;
                comboBox_ChartColour.Items.Add(Okyanus.Variables.PlotColorArr[i]);
            }

            Okyanus.Variables.ByteArr[0] = 0;
            Okyanus.Variables.ByteArr[1] = 1;
            Okyanus.Variables.ByteArr[2] = 2;
            Okyanus.Variables.ByteArr[3] = 4;

            comboBox_Length.Items.Add("Bit Read");
            for (int i = 1; i < Okyanus.Variables.ByteArr.GetLength(0); i++)
                comboBox_Length.Items.Add(Okyanus.Variables.ByteArr[i] + " Bytes");

            Okyanus.Variables.SignArr[0] = 0;
            Okyanus.Variables.SignArr[1] = 1;

            comboBox_Sign.Items.Add("isaretli");
            comboBox_Sign.Items.Add("isaretsiz");

            //   if (comboBox_Length.SelectedIndex == 0)
            //   {

            for (byte i = 0; i < Okyanus.Variables.BitArr.GetLength(0); i++) // 
            {
                Okyanus.Variables.BitArr[i] = i;
                comboBox_BitSet.Items.Add(Okyanus.Variables.BitArr[i] + ".Bit");

            }


            for (int i = 0; i < Okyanus.Variables.AdressArr.GetLength(0); i++)
            {
                Okyanus.Variables.AdressArr[i] = i;
                comboBox_Adress.Items.Add(Okyanus.Variables.AdressArr[i]);

            }
            // command box setup
            /*
                        Okyanus.Variables.CommandArr[0] = 10;
                        Okyanus.Variables.CommandArr[1] = 12;
                        comboBox_Command.Items.Add(Okyanus.Definitions.WriteComamnd);
                        comboBox_Command.Items.Add(Okyanus.Definitions.ReadComamnd);
                        comboBox_Command.SelectedIndex = 0;
            */


            for (int i = 0; i < 50; i++) // 20 yidi
            {
                Okyanus.Variables.CommandArr[i] = i;

            }
            //     Okyanus.Definitions.Komut00
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut00);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut01);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut02);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut03); //3
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut04);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut05);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut06);//6
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut07);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut08); //8
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut09);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut10);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut11); //11
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut12);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut13);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut14);//14
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut15);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut16);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut17);//17
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut18);
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut19); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut20); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut21); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut22); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut23); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut24); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut25); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut26); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut27); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut28); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut29); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut30); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut31); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut32); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut33); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut34); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut35); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut36); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut37); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut38); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut39); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut40); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut41); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut42); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut43); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut44); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut45); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut46); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut47); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut48); // 19
            comboBox_Command.Items.Add(Okyanus.Definitions.Komut49); // 19



            comboBox_Command.SelectedIndex = 0;

    //        LOG_PopulatetexboxandCheckBoxes();
            Okyanus.Chart.LogPropUpdate = true; // update Log textboxes and check boxes
            comboBox_Adress.SelectedIndex = 0;
            comboBox_Sign.SelectedIndex = 1;  // isarestsiz
            comboBox_Length.SelectedIndex = 1; // 1 byte

            comboBox_BitSet.SelectedIndex = 0;
            comboBox_Chart_On.SelectedIndex = 0;
            comboBox_Chart_Multiply.SelectedIndex = 3;
            comboBox_ChartColour.SelectedIndex = 0;

            numericUpDown_ChartMin.Value = 0;
            numericUpDown_ChartMax.Value = 10000;

            Okyanus.Variables.Variables_Name[0] = "Degisken0";
            Okyanus.Variables.CommandVar_Name[0] = "1";


            Enable_DATA_CommandFlashArray_ReadFill_Grid = false; 
        }

        void SP1_CalculateSendData()
        {
            int i;

            SP1_SendCommand = 12;
            SP1_SendAdress = 23456;
            SP1_Send_AdrLength = 169;

            SP1_SendData1++;
            SP1_SendData2 += 125;
            SP1_SendData3--;
            SP1_SendData4 += 46;
            SP1_SendData5 = 54711;
            SP1_Send_TotLength = 34;

            SP1_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP1_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP1_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP1_Buffer[3] = (byte)DEFAULT_PREAMBLE;

            SP1_Buffer[4] = (byte)(SP1_Send_TotLength >> SHIFT8);
            SP1_Buffer[5] = (byte)SP1_Send_TotLength;

            SP1_Buffer[6] = (byte)SP1_SendCommand;
            SP1_Buffer[7] = (byte)(SP1_SendAdress >> SHIFT8);
            SP1_Buffer[8] = (byte)SP1_SendAdress;
            SP1_Buffer[9] = (byte)SP1_Send_AdrLength;

            // data 
            SP1_Buffer[10] = (byte)(SP1_SendData1 >> SHIFT24);
            SP1_Buffer[11] = (byte)(SP1_SendData1 >> SHIFT16);
            SP1_Buffer[12] = (byte)(SP1_SendData1 >> SHIFT8);
            SP1_Buffer[13] = (byte)SP1_SendData1;

            SP1_Buffer[14] = (byte)(SP1_SendData2 >> SHIFT24);
            SP1_Buffer[15] = (byte)(SP1_SendData2 >> SHIFT16);
            SP1_Buffer[16] = (byte)(SP1_SendData2 >> SHIFT8);
            SP1_Buffer[17] = (byte)SP1_SendData2;
            SP1_Buffer[18] = (byte)(SP1_SendData3 >> SHIFT24);
            SP1_Buffer[19] = (byte)(SP1_SendData3 >> SHIFT16);
            SP1_Buffer[20] = (byte)(SP1_SendData3 >> SHIFT8);
            SP1_Buffer[21] = (byte)SP1_SendData3;
            SP1_Buffer[22] = (byte)(SP1_SendData4 >> SHIFT24);
            SP1_Buffer[23] = (byte)(SP1_SendData4 >> SHIFT16);
            SP1_Buffer[24] = (byte)(SP1_SendData4 >> SHIFT8);
            SP1_Buffer[25] = (byte)SP1_SendData4;
            SP1_Buffer[26] = (byte)(SP1_SendData5 >> SHIFT24);
            SP1_Buffer[27] = (byte)(SP1_SendData5 >> SHIFT16);
            SP1_Buffer[28] = (byte)(SP1_SendData5 >> SHIFT8);
            SP1_Buffer[29] = (byte)SP1_SendData5;

            SP1_SendCRC = DEFAULT_CRC_INIT;

            for (i = 4; i < (SP1_Send_TotLength - 4); i++)
            {
                SP1_SendCRC ^= SP1_Buffer[i];
            }

            SP1_Buffer[30] = (byte)(SP1_SendCRC >> SHIFT24);
            SP1_Buffer[31] = (byte)(SP1_SendCRC >> SHIFT16);
            SP1_Buffer[32] = (byte)(SP1_SendCRC >> SHIFT8);
            SP1_Buffer[33] = (byte)SP1_SendCRC;

            for (i = 0; i < SP1_Send_TotLength; i++)
            {
                SP1_SendBuf[i] = SP1_Buffer[i];
            }
        }
        String SP1_SendTextData() // soldaki data alma ekrani
        {
            uint Length = (uint)(SP1_SendBuf[4] << SHIFT8) + SP1_SendBuf[5];
            uint Preamble = (uint)((SP1_SendBuf[0] << SHIFT24) + (SP1_SendBuf[1] << SHIFT16) + (SP1_SendBuf[2] << SHIFT8) + SP1_SendBuf[3]);
            uint Command = (uint)SP1_SendBuf[6];
            uint Adr = (uint)(SP1_SendBuf[7] << SHIFT8) + SP1_SendBuf[8];
            uint Adr_length = (uint)SP1_SendBuf[9];


          String Textdata = "";

            Textdata += "Preamble :" + " 0 X" + Preamble.ToString("X") + "   ";
            Textdata += "Length : " + Length.ToString() + " / 0 X" + Length.ToString("X") + "  ";
            Textdata += "Command : " + Command.ToString() + " / 0 X" + Command.ToString("X") + nl;

            Textdata += "Adress : " + Adr.ToString() + " / 0 X" + Adr.ToString("X") + "   " ;
            Textdata += "Adr Length : " + Adr_length.ToString() + " / 0 X" + Adr_length.ToString("X") + "   Toplam Gonderilen Paket:" + GidenPaket.ToString() + nl;

            String str = "";

            int j = 0;
            for (int i = 0; i < Length; i++)
            {
                str += i.ToString() + ".." + SP1_Buffer[i].ToString() + "    ";
                j++;
                if (j > 11)// 8 di 11 yaptim
                {
                //    str += System.Environment.NewLine;
                    j = 0;
                }
            }
            return Textdata + str;

            
        }
        void SP1_CalculateRcvData()
        {
            SP1_RecCommand = (uint)SP1_ReceiveBuf[0];
            SP1_RecAdress = (uint)((SP1_ReceiveBuf[1] << SHIFT8) + SP1_ReceiveBuf[2]);
            SP1_RecAdrLength = (uint)SP1_ReceiveBuf[3];
            SP1_Data1 = (uint)((SP1_ReceiveBuf[4] << SHIFT24) + (SP1_ReceiveBuf[5] << SHIFT16) + (SP1_ReceiveBuf[6] << SHIFT8) + SP1_ReceiveBuf[7]);
            SP1_Data2 = (uint)((SP1_ReceiveBuf[8] << SHIFT24) + (SP1_ReceiveBuf[9] << SHIFT16) + (SP1_ReceiveBuf[10] << SHIFT8) + SP1_ReceiveBuf[11]);
            SP1_Data3 = (uint)((SP1_ReceiveBuf[12] << SHIFT24) + (SP1_ReceiveBuf[13] << SHIFT16) + (SP1_ReceiveBuf[14] << SHIFT8) + SP1_ReceiveBuf[15]);
            SP1_Data4 = (uint)((SP1_ReceiveBuf[16] << SHIFT24) + (SP1_ReceiveBuf[17] << SHIFT16) + (SP1_ReceiveBuf[18] << SHIFT8) + SP1_ReceiveBuf[19]);
            SP1_Data5 = (uint)((SP1_ReceiveBuf[20] << SHIFT24) + (SP1_ReceiveBuf[21] << SHIFT16) + (SP1_ReceiveBuf[22] << SHIFT8) + SP1_ReceiveBuf[23]);

            //      for (uint i = 0; i < 20; i++)
            //       {
            //    Okyanus.Variables.SP_ReceivedData[i] = i;// test
            //   Okyanus.Variables.SP_ReceivedData[i] = (uint)SP1_ReceiveBuf[i];
            //      }



            //           SP1_CRC = (uint)((SP1_ReceiveBuf[24] << SHIFT24) + (SP1_ReceiveBuf[25] << SHIFT16) + (SP1_ReceiveBuf[26] << SHIFT8) + SP1_ReceiveBuf[27]);

            byte CRC_Count = (byte)((Okyanus.SP1.Length - 1) - 6);


            //       SP1_CRC = (uint)((SP1_ReceiveBuf[(CRC_Count-3)] << SHIFT24) + (SP1_ReceiveBuf[(CRC_Count-2)] << SHIFT16) + (SP1_ReceiveBuf[(CRC_Count - 1)] << SHIFT8) + SP1_ReceiveBuf[CRC_Count]);



            /*
                        Okyanus.SP1.CRC_Calc = DEFAULT_CRC_INIT;
                        Okyanus.SP1.CRC_Calc ^= (uint)(Okyanus.SP1.Length >> SHIFT8);
                        Okyanus.SP1.CRC_Calc ^= (uint)Okyanus.SP1.Length;
                */
            int Count = (int)(Okyanus.SP1.Length - 10);
            if (Count < 0) Count = 0;

            for (int i = 0; i < Count; i++)  //  data crc
            {
                Okyanus.SP1.CRC_Calc ^= SP1_ReceiveBuf[i];
            }
            if (Okyanus.SP1.CRC_Calc != SP1_CRC) Okyanus.SP1.CRC_Error++;
        }



        String SP1_GetTextdata2()
        {

            String Textdata = "";
            for (int i = 0; i < 6; i++)
            {
                //  Textdata += i.ToString() + " : " + SP1_Buffer[i].ToString() + " / 0 X" + SP1_Buffer[i].ToString("X") + System.Environment.NewLine;
                Textdata += i.ToString() + "." + SP1_Buffer[i].ToString() + "  ";
            }
            Textdata += System.Environment.NewLine;
            int k = 0; int j = 0;
            for (int i = 6; i < Okyanus.SP1.Length; i++)
            {

                // Textdata += i.ToString() + " : " + SP1_ReceiveBuf[k].ToString() +  " / 0 X"+SP1_ReceiveBuf[k].ToString("X") + System.Environment.NewLine;
                //            Textdata += i.ToString() + "." + SP1_ReceiveBuf[k].ToString() + "  ";
                k++;
                j++;
                if (j > 5)
                {
                    Textdata += System.Environment.NewLine;
                    j = 0;
                }
            }
            return Textdata;
        }
        String SP1_GetAllReceivedData()
        {
            String Textdata = "";
            Textdata = "Received Data " + nl;
            Textdata += "Preamble :" + " 0 X" + Okyanus.SP1.Preamble.ToString("X") + "   ";
            Textdata += "Length : " + Okyanus.SP1.Length.ToString() + " / 0 X" + Okyanus.SP1.Length.ToString("X") + "  ";
            Textdata += "Command : " + SP1_RecCommand.ToString() + " / 0 X" + SP1_RecCommand.ToString("X") + nl;


            Textdata += "Adress : " + SP1_RecAdress.ToString() + " / 0 X" + SP1_RecAdress.ToString("X") +  "   ";
            Textdata += "Adr Length : " + SP1_RecAdrLength.ToString() + " / 0 X" + SP1_RecAdrLength.ToString("X") + nl;

            int j = 0;
            Textdata += "All Data (except preamble + paket length)" + nl;

            try
            {
                for (uint i = 0; i < (Okyanus.SP1.Length - 6); i++)
                {
                    string space =       "       ";
                    if (i < 1) space =   "        ";
                    if (i >= 91) space = "    ";
                    Textdata += (i + 9).ToString() + ".. " + SP1_ReceiveBuf[i].ToString() + space; //6
                    j++;
                    if (j > 9)// 5 
                    {
                        j = 0;
                        Textdata += nl;
                    }
                }
            }
            catch
            {

                Textdata = "Gelen data toplami paket uzunluk bilgisinden kisa !!";
            }




            /*

                        Textdata += "Data 1 : " + SP1_Data1.ToString() + " / 0 X" + SP1_Data1.ToString("X") + System.Environment.NewLine;
                        Textdata += "Data 2 : " + SP1_Data2.ToString() + " /  0 X" + SP1_Data2.ToString("X") + System.Environment.NewLine;
                        Textdata += "Data 3 : " + SP1_Data3.ToString() + " /  0 X " + SP1_Data3.ToString("X") + System.Environment.NewLine;
                        Textdata += "Data 4 : " + SP1_Data4.ToString() + " /  0 X" + SP1_Data4.ToString("X") + System.Environment.NewLine;
                        Textdata += "Data 5 : " + SP1_Data5.ToString() + " /  0 X" + SP1_Data5.ToString("X") + System.Environment.NewLine;
                        Textdata += "CRC     : " + SP1_CRC.ToString() + " /  0 X" + SP1_CRC.ToString("X") + System.Environment.NewLine;
                        Textdata += System.Environment.NewLine + "Calculated:  " + System.Environment.NewLine;
                        Textdata += "CRC     : " + Okyanus.SP1.CRC_Calc.ToString() + " /  0X" + Okyanus.SP1.CRC_Calc.ToString("X") + System.Environment.NewLine;
                        Textdata += "Error Count : " + Okyanus.SP1.CRC_Error.ToString() + System.Environment.NewLine;
                        Textdata += "Temperature 2 : " + ((short)(SP1_Data2) / 10).ToString() + "." + (SP1_Data2 % 10).ToString() + "°C" + System.Environment.NewLine;
                        Textdata += "Temperature 3 : " + ((short)(SP1_Data3) / 10).ToString() + "." + (SP1_Data3 % 10).ToString() + "°C" + System.Environment.NewLine;
                        Textdata += "Temperature 4 : " + ((short)(SP1_Data4) / 10).ToString() + "." + (SP1_Data4 % 10).ToString() + "°C" + System.Environment.NewLine;

            */
            return Textdata;



        }
        String SP1_GetTextdata()
        {
            String Textdata = "";
            Textdata = "Received Data " + System.Environment.NewLine;
            Textdata += "Preamble :" + " 0 X" + Okyanus.SP1.Preamble.ToString("X") + System.Environment.NewLine;
            Textdata += "Length : " + Okyanus.SP1.Length.ToString() + " / 0 X" + Okyanus.SP1.Length.ToString("X") + System.Environment.NewLine;



            Textdata += "Command : " + SP1_RecCommand.ToString() + " / 0 X" + SP1_RecCommand.ToString("X") + System.Environment.NewLine;
            Textdata += "Adress : " + SP1_RecAdress.ToString() + " / 0 X" + SP1_RecAdress.ToString("X") + System.Environment.NewLine;
            Textdata += "Adr Length : " + SP1_RecAdrLength.ToString() + " / 0 X" + SP1_RecAdrLength.ToString("X") + System.Environment.NewLine;

            Textdata += "Data 1 : " + SP1_Data1.ToString() + " / 0 X" + SP1_Data1.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 2 : " + SP1_Data2.ToString() + " /  0 X" + SP1_Data2.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 3 : " + SP1_Data3.ToString() + " /  0 X " + SP1_Data3.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 4 : " + SP1_Data4.ToString() + " /  0 X" + SP1_Data4.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 5 : " + SP1_Data5.ToString() + " /  0 X" + SP1_Data5.ToString("X") + System.Environment.NewLine;
            Textdata += "CRC     : " + SP1_CRC.ToString() + " /  0 X" + SP1_CRC.ToString("X") + System.Environment.NewLine;
            Textdata += System.Environment.NewLine + "Calculated:  " + System.Environment.NewLine;
            Textdata += "CRC     : " + Okyanus.SP1.CRC_Calc.ToString() + " /  0X" + Okyanus.SP1.CRC_Calc.ToString("X") + System.Environment.NewLine;
            Textdata += "Error Count : " + Okyanus.SP1.CRC_Error.ToString() + System.Environment.NewLine;
            Textdata += "Temperature 2 : " + ((short)(SP1_Data2) / 10).ToString() + "." + (SP1_Data2 % 10).ToString() + "°C" + System.Environment.NewLine;
            Textdata += "Temperature 3 : " + ((short)(SP1_Data3) / 10).ToString() + "." + (SP1_Data3 % 10).ToString() + "°C" + System.Environment.NewLine;
            Textdata += "Temperature 4 : " + ((short)(SP1_Data4) / 10).ToString() + "." + (SP1_Data4 % 10).ToString() + "°C" + System.Environment.NewLine;
            return Textdata;
        }
        void SP2_CalculateRcvData()
        {
            SP2_RecCommand = (uint)SP2_ReceiveBuf[0];
            SP2_RecAdress = (uint)((SP2_ReceiveBuf[1] << SHIFT8) + SP2_ReceiveBuf[2]);
            SP2_RecAdrLength = (uint)SP2_ReceiveBuf[3];
            SP2_Data1 = (uint)((SP2_ReceiveBuf[4] << SHIFT24) + (SP2_ReceiveBuf[5] << SHIFT16) + (SP2_ReceiveBuf[6] << SHIFT8) + SP2_ReceiveBuf[7]);
            SP2_Data2 = (uint)((SP2_ReceiveBuf[8] << SHIFT24) + (SP2_ReceiveBuf[9] << SHIFT16) + (SP2_ReceiveBuf[10] << SHIFT8) + SP2_ReceiveBuf[11]);
            SP2_Data3 = (uint)((SP2_ReceiveBuf[12] << SHIFT24) + (SP2_ReceiveBuf[13] << SHIFT16) + (SP2_ReceiveBuf[14] << SHIFT8) + SP2_ReceiveBuf[15]);
            SP2_Data4 = (uint)((SP2_ReceiveBuf[16] << SHIFT24) + (SP2_ReceiveBuf[17] << SHIFT16) + (SP2_ReceiveBuf[18] << SHIFT8) + SP2_ReceiveBuf[19]);
            SP2_Data5 = (uint)((SP2_ReceiveBuf[20] << SHIFT24) + (SP2_ReceiveBuf[21] << SHIFT16) + (SP2_ReceiveBuf[22] << SHIFT8) + SP2_ReceiveBuf[23]);
            SP2_CRC = (uint)((SP2_ReceiveBuf[24] << SHIFT24) + (SP2_ReceiveBuf[25] << SHIFT16) + (SP2_ReceiveBuf[26] << SHIFT8) + SP2_ReceiveBuf[27]);
            SP2_CRC_Calc = DEFAULT_CRC_INIT;
            SP2_CRC_Calc ^= (uint)(SP2_Length >> SHIFT8);
            SP2_CRC_Calc ^= (uint)SP2_Length;

            for (uint i = 0; i < 24; i++)  //  data crc
            {
                SP2_CRC_Calc ^= SP2_ReceiveBuf[i];
            }
            if (SP2_CRC_Calc != SP2_CRC) SP2_CRC_Error++;
        }

        String SP2_GetTextdata()
        {
            String Textdata = "";
            Textdata = "Received Data " + System.Environment.NewLine;
            Textdata += "Preamble :" + " 0 X" + SP2_Preamble.ToString("X") + System.Environment.NewLine;
            Textdata += "Length : " + SP2_Length.ToString() + " / 0 X" + SP2_Length.ToString("X") + System.Environment.NewLine;

            Textdata += "Command : " + SP2_RecCommand.ToString() + " / 0 X" + SP2_RecCommand.ToString("X") + System.Environment.NewLine;
            Textdata += "Adress : " + SP2_RecAdress.ToString() + " / 0 X" + SP2_RecAdress.ToString("X") + System.Environment.NewLine;
            Textdata += "Adr Length : " + SP2_RecAdrLength.ToString() + " / 0 X" + SP2_RecAdrLength.ToString("X") + System.Environment.NewLine;

            Textdata += "Data 1 : " + SP2_Data1.ToString() + " / 0 X" + SP2_Data1.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 2 : " + SP2_Data2.ToString() + " /  0 X" + SP2_Data2.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 3 : " + SP2_Data3.ToString() + " /  0 X " + SP2_Data3.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 4 : " + SP2_Data4.ToString() + " /  0 X" + SP2_Data4.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 5 : " + SP2_Data5.ToString() + " /  0 X" + SP2_Data5.ToString("X") + System.Environment.NewLine;
            Textdata += "CRC     : " + SP2_CRC.ToString() + " /  0 X" + SP2_CRC.ToString("X") + System.Environment.NewLine;
            Textdata += System.Environment.NewLine + "Calculated:  " + System.Environment.NewLine;
            //       CRC_Test



            Textdata += "CRC     : " + SP2_CRC_Calc.ToString() + " /  0X" + SP2_CRC_Calc.ToString("X") + System.Environment.NewLine;



            Textdata += "Error Count : " + SP2_CRC_Error.ToString() + System.Environment.NewLine;
            Textdata += "Temperature 2 : " + ((short)(SP2_Data2) / 10).ToString() + "." + (SP2_Data2 % 10).ToString() + "°C" + System.Environment.NewLine;
            Textdata += "Temperature 3 : " + ((short)(SP2_Data3) / 10).ToString() + "." + (SP2_Data3 % 10).ToString() + "°C" + System.Environment.NewLine;
            Textdata += "Temperature 4 : " + ((short)(SP2_Data4) / 10).ToString() + "." + (SP2_Data4 % 10).ToString() + "°C" + System.Environment.NewLine;
            return Textdata;
        }
        String SP2_GetTextdata2()
        {
            String Textdata = "";
            for (int i = 0; i < 6; i++)
            {
                Textdata += i.ToString() + " : " + SP2_Buffer[i].ToString() + " / 0 X" + SP2_Buffer[i].ToString("X") + System.Environment.NewLine;
            }
            int k = 0;
            for (int i = 6; i < 34; i++)
            {
                Textdata += i.ToString() + " : " + SP2_ReceiveBuf[k].ToString() + " / 0 X" + SP2_ReceiveBuf[k].ToString("X") + System.Environment.NewLine;
                k++;
            }
            return Textdata;
        }
        void SP2_CalculateSendData()
        {
            int i;

            SP2_SendCommand = 12;
            SP2_SendAdress = 23456;
            SP2_Send_AdrLength = 169;

            SP2_SendData1++;
            SP2_SendData2 += 125;
            SP2_SendData3--;
            SP2_SendData4 += 46;
            SP2_SendData5 = 54711;



            SP2_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP2_Buffer[1] = (byte)(Preamble_Test);
            //           SP2_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP2_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP2_Buffer[3] = (byte)DEFAULT_PREAMBLE;

            SP2_Buffer[4] = (byte)(SP2_DEFAULT_LENGTH >> SHIFT8);
            SP2_Buffer[5] = (byte)SP2_DEFAULT_LENGTH;

            SP2_Buffer[6] = (byte)SP2_SendCommand;
            SP2_Buffer[7] = (byte)(SP2_SendAdress >> SHIFT8);
            SP2_Buffer[8] = (byte)SP2_SendAdress;
            SP2_Buffer[9] = (byte)SP2_Send_AdrLength;

            // data 
            SP2_Buffer[10] = (byte)(SP2_SendData1 >> SHIFT24);
            SP2_Buffer[11] = (byte)(SP2_SendData1 >> SHIFT16);
            SP2_Buffer[12] = (byte)(SP2_SendData1 >> SHIFT8);
            SP2_Buffer[13] = (byte)SP2_SendData1;

            SP2_Buffer[14] = (byte)(SP2_SendData2 >> SHIFT24);
            SP2_Buffer[15] = (byte)(SP2_SendData2 >> SHIFT16);
            SP2_Buffer[16] = (byte)(SP2_SendData2 >> SHIFT8);
            SP2_Buffer[17] = (byte)SP2_SendData2;
            SP2_Buffer[18] = (byte)(SP2_SendData3 >> SHIFT24);
            SP2_Buffer[19] = (byte)(SP2_SendData3 >> SHIFT16);
            SP2_Buffer[20] = (byte)(SP2_SendData3 >> SHIFT8);
            SP2_Buffer[21] = (byte)SP2_SendData3;
            SP2_Buffer[22] = (byte)(SP2_SendData4 >> SHIFT24);
            SP2_Buffer[23] = (byte)(SP2_SendData4 >> SHIFT16);
            SP2_Buffer[24] = (byte)(SP2_SendData4 >> SHIFT8);
            SP2_Buffer[25] = (byte)SP2_SendData4;
            SP2_Buffer[26] = (byte)(SP2_SendData5 >> SHIFT24);
            SP2_Buffer[27] = (byte)(SP2_SendData5 >> SHIFT16);
            SP2_Buffer[28] = (byte)(SP2_SendData5 >> SHIFT8);
            SP2_Buffer[29] = (byte)SP2_SendData5;

            SP2_SendCRC = DEFAULT_CRC_INIT;

            for (i = 4; i < 30; i++)
            {
                SP2_SendCRC ^= SP2_Buffer[i];
            }
            if (CRC_Test == 32)
            {
                SP2_SendCRC += 23;
                CRC_Test = 0;
            }
            SP2_Buffer[30] = (byte)(SP2_SendCRC >> SHIFT24);
            SP2_Buffer[31] = (byte)(SP2_SendCRC >> SHIFT16);
            SP2_Buffer[32] = (byte)(SP2_SendCRC >> SHIFT8);
            SP2_Buffer[33] = (byte)SP2_SendCRC;

            for (i = 0; i < 34; i++)
            {
                SP2_SendBuf[i] = SP2_Buffer[i];
            }
        }
        String SP2_SendTextData()
        {

            String Textdata = "";
            //    Textdata = "Received Data " + System.Environment.NewLine;
            Textdata += "Preamble :" + " 0 X" +

              ((byte)(DEFAULT_PREAMBLE >> SHIFT24)).ToString("X") +
                //            ((byte)(DEFAULT_PREAMBLE >> SHIFT16)).ToString("X") +
              Preamble_Test.ToString("X") +

              ((byte)(DEFAULT_PREAMBLE >> SHIFT8)).ToString("X") +
              ((byte)(DEFAULT_PREAMBLE)).ToString("X") + System.Environment.NewLine;



            Textdata += "Length : " + SP2_DEFAULT_LENGTH.ToString() + " / 0 X" + SP2_DEFAULT_LENGTH.ToString("X") + System.Environment.NewLine;
            Textdata += "Command : " + SP2_SendCommand.ToString() + " / 0 X" + SP2_SendCommand.ToString("X") + System.Environment.NewLine;
            Textdata += "Adress : " + SP2_SendAdress.ToString() + " / 0 X" + SP2_SendAdress.ToString("X") + System.Environment.NewLine;
            Textdata += "Adr Length : " + SP2_Send_AdrLength.ToString() + " / 0 X" + SP2_Send_AdrLength.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 1 : " + SP2_SendData1.ToString() + " / 0 X" + SP2_SendData1.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 2 : " + SP2_SendData2.ToString() + " / 0 X" + SP2_SendData2.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 3 : " + SP2_SendData3.ToString() + " / 0 X" + SP2_SendData3.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 4 : " + SP2_SendData4.ToString() + " / 0 X" + SP2_SendData4.ToString("X") + System.Environment.NewLine;
            Textdata += "Data 5 : " + SP2_SendData5.ToString() + " / 0 X" + SP2_SendData5.ToString("X") + System.Environment.NewLine;
            Textdata += "CRC     : " + SP2_SendCRC.ToString() + " /  0 X" + SP2_SendCRC.ToString("X") + System.Environment.NewLine;

            //       ((byte) (SP2_SendCRC >> SHIFT24)).ToString("X") + 
            //            ((byte)(SP2_SendCRC >> SHIFT16)).ToString("X") +
            //         CRC_Test.ToString("X") +

            //         ((byte)(SP2_SendCRC >> SHIFT8)).ToString("X")+     
            //       ((byte)(SP2_SendCRC )).ToString("X") + System.Environment.NewLine;




            return Textdata;

        }

        public void ReqRamReadCommand()
        {
            int i; UInt16 CRC = 0;
            int Total_Length = 12;
            UInt16 Adr = 0;
            byte Adr_Length = 20;
            SP1_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP1_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP1_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP1_Buffer[3] = (byte)DEFAULT_PREAMBLE;
            SP1_Buffer[4] = (byte)(Total_Length >> SHIFT8);
            SP1_Buffer[5] = (byte)Total_Length;
            SP1_Buffer[6] = 14; // read command
            SP1_Buffer[7] = (byte)(Adr >> SHIFT8); // 10.000
            SP1_Buffer[8] = (byte)Adr;
            SP1_Buffer[9] = Adr_Length;//  (byte)SP1_Send_AdrLength;  

            for (i = 4; i < (Total_Length - 2); i++) // crc uzunlugu 2 byte oldu
            {
                CRC += SP1_Buffer[i];
            }

            SP1_Buffer[10] = (byte)(CRC >> SHIFT8);
            SP1_Buffer[11] = (byte)CRC;

            for (i = 0; i < Total_Length; i++)
            {
                SP1_SendBuf[i] = SP1_Buffer[i];
            }

            try
            {
                SP1_serialPort.Write(SP1_SendBuf, 0, Total_Length);
            }
            catch
            {

                SP1_DisConnect_Procedure();
                StopCommandsendProcedure();
                MessageBox.Show(" Send Error " + nl + "Port Maybe Disconnected");
                return;
            }
            GidenPaket++;
        }
        public void ReadFlash500RequestProcedure(UInt16 Adr)
        {
            Okyanus.Variables.FlashRead100byte = false;// 500 luk okuma
            int i = 0;
            int Total_Length = 12;
            byte Adr_Length = 100;
            UInt16 CRC = 0;
     //       UInt16 k = 0;
            switch (Adr)
            {
                default:
                     Okyanus.Variables.FlashRead500Phase = 0;
                    break;
                case 10000: Okyanus.Variables.FlashRead500Phase = 1;
                    break;
                case 10100:  Okyanus.Variables.FlashRead500Phase = 2;
                    break;
                case 10200:  Okyanus.Variables.FlashRead500Phase = 3;
                    break;
                case 10300:  Okyanus.Variables.FlashRead500Phase = 4;
                    break;
                case 10400:  Okyanus.Variables.FlashRead500Phase = 5;
                    break;
            }


            SP1_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP1_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP1_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP1_Buffer[3] = (byte)DEFAULT_PREAMBLE;
            SP1_Buffer[4] = (byte)(Total_Length >> SHIFT8);
            SP1_Buffer[5] = (byte)Total_Length;
            SP1_Buffer[6] = 12; // read from uc command 12
            SP1_Buffer[7] = (byte)(Adr >> SHIFT8);
            SP1_Buffer[8] = (byte)Adr;
            SP1_Buffer[9] = Adr_Length;//  (byte)SP1_Send_AdrLength;  


            for (i = 4; i < (Total_Length - 2); i++)
            {
                CRC += SP1_Buffer[i];
            }
            SP1_Buffer[10] = (byte)(CRC >> SHIFT8);
            SP1_Buffer[11] = (byte)CRC;


            for (i = 0; i < Total_Length; i++)
            {
                SP1_SendBuf[i] = SP1_Buffer[i];
            }

            try
            {
                SP1_serialPort.Write(SP1_SendBuf, 0, Total_Length);
            }
            catch
            {
                SP1_DisConnect_Procedure();
                StopCommandsendProcedure();
                MessageBox.Show(" Send Error " + nl + "Port Maybe Disconnected");
                return;
            }
            GidenPaket++;
        }
        public void ReadFlashRequestProcedure()
        {
            Okyanus.Variables.FlashRead100byte = true;
            int i = 0;
            int Total_Length = 12;
            UInt16 Adr = 10000;
            byte Adr_Length = 100;
            UInt16 CRC = 0;

            SP1_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP1_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP1_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP1_Buffer[3] = (byte)DEFAULT_PREAMBLE;
            SP1_Buffer[4] = (byte)(Total_Length >> SHIFT8);
            SP1_Buffer[5] = (byte)Total_Length;
            SP1_Buffer[6] = 12; // read from uc command 12
            SP1_Buffer[7] = (byte)(Adr >> SHIFT8);
            SP1_Buffer[8] = (byte)Adr;
            SP1_Buffer[9] = Adr_Length;//  (byte)SP1_Send_AdrLength;  


            for (i = 4; i < (Total_Length - 2); i++)
            {
                CRC += SP1_Buffer[i];
            }
            SP1_Buffer[10] = (byte)(CRC >> SHIFT8);
            SP1_Buffer[11] = (byte)CRC;


            for (i = 0; i < Total_Length; i++)
            {
                SP1_SendBuf[i] = SP1_Buffer[i];
            }

            try
            {
                SP1_serialPort.Write(SP1_SendBuf, 0, Total_Length);
            }
            catch
            {
                SP1_DisConnect_Procedure();
                StopCommandsendProcedure();
                MessageBox.Show(" Send Error " + nl + "Port Maybe Disconnected");
                return;
            }
            GidenPaket++;
        }
        public void WriteToFlash500Procedure(UInt16 Adr) // komut 10
        {
            UInt16 i = 0;
            UInt16 Total_Length = 112;
            byte Adr_Length = 100;
            UInt16 CRC = 0;
            UInt16 k = 0;
            switch (Adr)
            {
                default:
                case 10000: k = 0;
                    break;
                case 10100: k = 50;
                    break;
                case 10200: k = 100;
                    break;
                case 10300: k = 150;
                    break;
                case 10400: k = 200;
                    break;
            }

            SP1_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP1_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP1_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP1_Buffer[3] = (byte)DEFAULT_PREAMBLE;
            SP1_Buffer[4] = (byte)(Total_Length >> SHIFT8);
            SP1_Buffer[5] = (byte)Total_Length;
            SP1_Buffer[6] = Okyanus.Definitions.FlashWrite;//  (byte)Okyanus.Variables.CommandArr[0]; // write command 10 
            SP1_Buffer[7] = (byte)(Adr >> SHIFT8); // 10.000
            SP1_Buffer[8] = (byte)Adr;
            SP1_Buffer[9] = Adr_Length;//  (byte)SP1_Send_AdrLength;

            // ALTTAN ITIBAREN 100 BYTE LIK 5 GRUP ICIN AYRIM YAPMAK LAZIM!!!
         
            UInt16 j = 10;
            for (i = 0; i < 5; i++) 
            {   // 500 luk ilk versiyon paket gonderimi
                // data 
                // data 
                SP1_Buffer[j] =     (byte)(Okyanus.Variables.SendBuffer[k + i*10] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 1] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10]);
                SP1_Buffer[j + 2] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 1] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 3] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 1]);
                SP1_Buffer[j + 4] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 2] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 5] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 2]);
                SP1_Buffer[j + 6] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 3] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 7] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 3]);
                SP1_Buffer[j + 8] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 4] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 9] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 4]);

                SP1_Buffer[j + 10] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 5] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 11] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 5]);
                SP1_Buffer[j + 12] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 6] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 13] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 6]);
                SP1_Buffer[j + 14] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 7] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 15] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 7]);
                SP1_Buffer[j + 16] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 8] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 17] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 8]);
                SP1_Buffer[j + 18] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 9] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 19] = (byte)(Okyanus.Variables.SendBuffer[k + i * 10 + 9]);
                j += 20;
              
            }

            for (i = 4; i < (Total_Length - 2); i++) // preamble ve crc haric
            {
                CRC += SP1_Buffer[i];
            }
            SP1_Buffer[Total_Length - 2] = (byte)(CRC >> SHIFT8);
            SP1_Buffer[Total_Length - 1] = (byte)CRC;

            for (i = 0; i < Total_Length; i++)
            {
                SP1_SendBuf[i] = SP1_Buffer[i];
            }
            try
            {
                SP1_serialPort.Write(SP1_SendBuf, 0, Total_Length);
            }
            catch
            {
                SP1_DisConnect_Procedure();
                StopCommandsendProcedure();
                MessageBox.Show(" Send Error " + nl + "Port Maybe Disconnected");
                return;
            }

            GidenPaket++;
            Okyanus.DebugVar.Write500_11++;
        }


        public void WriteToFlashProcedure(UInt16 Adr) // komut 10
        {
            UInt16 i = 0;
            UInt16 Total_Length = 112;
            byte Adr_Length = 100;
            UInt16 CRC = 0;
            SP1_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP1_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP1_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP1_Buffer[3] = (byte)DEFAULT_PREAMBLE;
            SP1_Buffer[4] = (byte)(Total_Length >> SHIFT8);
            SP1_Buffer[5] = (byte)Total_Length;
            SP1_Buffer[6] = Okyanus.Definitions.FlashWrite;//  (byte)Okyanus.Variables.CommandArr[0]; // write command 10 
            SP1_Buffer[7] = (byte)(Adr >> SHIFT8); // 10.000
            SP1_Buffer[8] = (byte)Adr;
            SP1_Buffer[9] = Adr_Length;//  (byte)SP1_Send_AdrLength;

            // ALTTAN ITIBAREN 100 BYTE LIK 5 GRUP ICIN AYRIM YAPMAK LAZIM!!!

            UInt16 j = 10;
            for (i = 0; i < 10; i++)
            {
                // data 
                SP1_Buffer[j] = (byte)(Okyanus.Variables.Var_Command[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 1] = (byte)(Okyanus.Variables.Var_Command[i]);
                SP1_Buffer[j + 2] = (byte)(Okyanus.Variables.Var_Parameter1[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 3] = (byte)(Okyanus.Variables.Var_Parameter1[i]);
                SP1_Buffer[j + 4] = (byte)(Okyanus.Variables.Var_Parameter2[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 5] = (byte)(Okyanus.Variables.Var_Parameter2[i]);
                SP1_Buffer[j + 6] = (byte)(Okyanus.Variables.Var_Parameter3[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 7] = (byte)(Okyanus.Variables.Var_Parameter3[i]);
                SP1_Buffer[j + 8] = (byte)(Okyanus.Variables.Var_Parameter4[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 9] = (byte)(Okyanus.Variables.Var_Parameter4[i]);
                j += 10;

            }
            /*
                SP1_Buffer[20] = (byte)(Okyanus.Variables.Var_Command[List2] >> SHIFT8); // firmware komut
                SP1_Buffer[21] = (byte)(Okyanus.Variables.Var_Command[List2]);
                SP1_Buffer[22] = (byte)(Okyanus.Variables.Var_Parameter1[List2] >> SHIFT8); // firmware komut
                SP1_Buffer[23] = (byte)(Okyanus.Variables.Var_Parameter1[List2]);
                SP1_Buffer[24] = (byte)(Okyanus.Variables.Var_Parameter2[List2] >> SHIFT8); // firmware komut
                SP1_Buffer[25] = (byte)(Okyanus.Variables.Var_Parameter2[List2]);
                SP1_Buffer[26] = (byte)(Okyanus.Variables.Var_Parameter3[List2] >> SHIFT8); // firmware komut
                SP1_Buffer[27] = (byte)(Okyanus.Variables.Var_Parameter3[List2]);
                SP1_Buffer[28] = (byte)(Okyanus.Variables.Var_Parameter4[List2] >> SHIFT8); // firmware komut
                SP1_Buffer[29] = (byte)(Okyanus.Variables.Var_Parameter4[List2]);
            }
*/


            for (i = 4; i < (Total_Length - 2); i++) // preamble ve crc haric
            {
                CRC += SP1_Buffer[i];
            }
            SP1_Buffer[Total_Length - 2] = (byte)(CRC >> SHIFT8);
            SP1_Buffer[Total_Length - 1] = (byte)CRC;

            for (i = 0; i < Total_Length; i++)
            {
                SP1_SendBuf[i] = SP1_Buffer[i];
            }
            try
            {
                SP1_serialPort.Write(SP1_SendBuf, 0, Total_Length);
            }
            catch
            {
                SP1_DisConnect_Procedure();
                StopCommandsendProcedure();
                MessageBox.Show(" Send Error " + nl + "Port Maybe Disconnected");
                return;
            }

            GidenPaket++;
        }
        // Send_Data_sequence(0, 1, 10000)
        public void WriteTableSequence(uint List1, uint List2, uint Adr)
        {
            int i;
            if (Okyanus.Variables.WriteFlashEnable == false) return;


            int Total_Length = 32;


            SP1_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP1_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP1_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP1_Buffer[3] = (byte)DEFAULT_PREAMBLE;

            SP1_Buffer[4] = (byte)(Total_Length >> SHIFT8);
            SP1_Buffer[5] = (byte)Total_Length;

            SP1_Buffer[6] = (byte)Okyanus.Variables.CommandArr[0]; // write command
            SP1_Buffer[7] = (byte)(Adr >> SHIFT8); // 10.000
            SP1_Buffer[8] = (byte)Adr;
            SP1_Buffer[9] = 20;//  (byte)SP1_Send_AdrLength;



            // data 
            SP1_Buffer[10] = (byte)(Okyanus.Variables.Var_Command[List1] >> SHIFT8); // firmware komut
            SP1_Buffer[11] = (byte)(Okyanus.Variables.Var_Command[List1]);
            SP1_Buffer[12] = (byte)(Okyanus.Variables.Var_Parameter1[List1] >> SHIFT8); // firmware komut
            SP1_Buffer[13] = (byte)(Okyanus.Variables.Var_Parameter1[List1]);
            SP1_Buffer[14] = (byte)(Okyanus.Variables.Var_Parameter2[List1] >> SHIFT8); // firmware komut
            SP1_Buffer[15] = (byte)(Okyanus.Variables.Var_Parameter2[List1]);
            SP1_Buffer[16] = (byte)(Okyanus.Variables.Var_Parameter3[List1] >> SHIFT8); // firmware komut
            SP1_Buffer[17] = (byte)(Okyanus.Variables.Var_Parameter3[List1]);
            SP1_Buffer[18] = (byte)(Okyanus.Variables.Var_Parameter4[List1] >> SHIFT8); // firmware komut
            SP1_Buffer[19] = (byte)(Okyanus.Variables.Var_Parameter4[List1]);

            SP1_Buffer[20] = (byte)(Okyanus.Variables.Var_Command[List2] >> SHIFT8); // firmware komut
            SP1_Buffer[21] = (byte)(Okyanus.Variables.Var_Command[List2]);
            SP1_Buffer[22] = (byte)(Okyanus.Variables.Var_Parameter1[List2] >> SHIFT8); // firmware komut
            SP1_Buffer[23] = (byte)(Okyanus.Variables.Var_Parameter1[List2]);
            SP1_Buffer[24] = (byte)(Okyanus.Variables.Var_Parameter2[List2] >> SHIFT8); // firmware komut
            SP1_Buffer[25] = (byte)(Okyanus.Variables.Var_Parameter2[List2]);
            SP1_Buffer[26] = (byte)(Okyanus.Variables.Var_Parameter3[List2] >> SHIFT8); // firmware komut
            SP1_Buffer[27] = (byte)(Okyanus.Variables.Var_Parameter3[List2]);
            SP1_Buffer[28] = (byte)(Okyanus.Variables.Var_Parameter4[List2] >> SHIFT8); // firmware komut
            SP1_Buffer[29] = (byte)(Okyanus.Variables.Var_Parameter4[List2]);


            SP1_SendCRC = 0;

            for (i = 4; i < (Total_Length - 2); i++)
            {
                SP1_SendCRC += SP1_Buffer[i];
            }

            SP1_Buffer[30] = (byte)(SP1_SendCRC >> SHIFT8);
            SP1_Buffer[31] = (byte)SP1_SendCRC;

            for (i = 0; i < Total_Length; i++)
            {
                SP1_SendBuf[i] = SP1_Buffer[i];
            }

            try
            {
                SP1_serialPort.Write(SP1_SendBuf, 0, Total_Length);
            }
            catch
            {
                SP1_DisConnect_Procedure();
                StopCommandsendProcedure();
                MessageBox.Show(" Send Error " + nl + "Port Maybe Disconnected");
                return;
            }
        }

        private void Base_Timer1mSec_Tick(object sender, EventArgs e)
        {
            Timer_1sec++;

            if (Okyanus.SP1.ReadTimeout != 0)
            {
                Okyanus.SP1.ReadTimeout--;

            }
            else
            {
                //timeout
        //        DebugVar.Timeout_Val = 34;

    

            }
            if (Okyanus.SP1.ReadSequence == 1)
            {
                if (Okyanus.SP1.ReadTimeout == 0)
                {
                    Okyanus.SP1.ReadSequence = 0;
                    Okyanus.Variables.TimeoutErrror_Count++;
                    //RealReceivedDataCount SP1_serialPort.BytesToRead
                    Okyanus.Variables.RealReceivedDataCount = SP1_serialPort.BytesToRead;
                }
            }

            if (Timer_1sec > Okyanus.Variables.Log_LogSampleTime)  // 32*20 = 640  hizli  32*32 yavas 24 yakin ama cok hafi f yavas  22 1sn icin
            {
                Timer_1sec = 0;

                if (Okyanus.Variables.Log_Status == true)
                {
                    if (SP1_serialPort.IsOpen == false)
                    {
                        Okyanus.Variables.Log_Status = false;
                        MessageBox.Show("Com Port Error!" + nl + "Loggging Disabled");
                        Okyanus.Variables.Log_Counter = 0;
                    }
                    else
                    {
                        ReqRamReadCommand();
                        DATA_Prep_TableSelectDatas_4_Log(); // burada secilen degiskelnelre gore icerik her adimda guncellenir 
                        CHART_Plot_Main();
                        LOG_DataLogProcess(Okyanus.Definitions.Original_Log_File);
                        LOG_ShowVariablesAtLogScreen();
                        if (Okyanus.Chart.PlayDelay != 0) Okyanus.Chart.PlayDelay--;
                    }
                }

            }
            Handle500Byte();

                TimerShareBase++;
                if (TimerShareBase > Okyanus.Definitions.TIMER_SHARE_BASE) TimerShareBase = 0;//11

                switch (TimerShareBase)
                {
                    case 0:
                   //     String Flash500;
                //        if(Flash500ByteWrite == true)Flash500 =" ON";
                   //     else Flash500 = "---";
                        String str = SP1_GetAllReceivedData() + nl;
                        //          str += (nl);
                        str += nl + "Hatali Preamble" + Okyanus.Variables.PreambleError_str;
                        str += nl + "preamble arkasi Uzunluk" + Okyanus.Variables.LengthError_str;
                        str += nl + "preamble+length giren data" + Okyanus.Variables.Preamble_Count.ToString();
                        str += nl + "Timeout Count" + Okyanus.Variables.TimeoutErrror_Count.ToString();
                        str += nl + "Gelen gercek byte sayisi" + Okyanus.Variables.RealReceivedDataCount.ToString();

                        str += nl +  "Flash500ByteWriteText: " + Flash500ByteWriteText.ToString();

                        str += nl + "Flash500ByteWriteEnable: " + Flash500ByteWriteEnable.ToString();
                        str += nl + "Flash500ByteWrite:       " + Flash500ByteWrite.ToString();

                        str += nl + "Flash500ByteWriteTimeoutTimer: " + Flash500ByteWriteTimeoutTimer.ToString();
                        str += nl + "Flash500ByteWriteTimer:        " + Flash500ByteWriteTimer.ToString();

                        str += nl + "Flash500ByteCount:     " + Flash500ByteCount.ToString();
                        str += nl + "Flash500ByteCountPrev: " + Flash500ByteCountPrev.ToString();

                        str += nl + "writeVerify:    " + writeVerify.ToString();



                        str += nl + "FlashRead100byte: " + Okyanus.Variables.FlashRead100byte.ToString();
                        str += nl + "FlashRead500Phase: " + Okyanus.Variables.FlashRead500Phase.ToString();
                        str += nl + "UpdateGrid500Table: " + Okyanus.Variables.UpdateGrid500Table.ToString();
                        str += nl + "FlashRead500Timeout: " + Okyanus.Variables.FlashRead500Timeout.ToString();



                        SP1_DatatextBox.Text = str;
                        //           Okyanus.Variables.PreambleCountr_str = Okyanus.Variables.Preamble_Count.ToString();                                                                             
                        break;
                    case 1:
                        if (Flash500ByteWriteText == false)

                            SP1_DataRcvtextBox.Text = SP1_SendTextData();
                        else
                        {
                     //       if (Flash500ByteCount == 5) SP1_DataRcvtextBox.Text += SP1_SendTextData();

                            if (Flash500ByteCount != Flash500ByteCountPrev)
                            {
                                Flash500ByteCountPrev = Flash500ByteCount;   
                                switch (Flash500ByteCount)
                                {
                                   // case 0:
                                        
                                  //      break;
                                    case 0:
                                        
                                     //   SP1_DataRcvtextBox.Text = SP1_SendTextData();
                                        break;
                                    case 1:
                                        SP1_DataRcvtextBox.Clear();
                                        SP1_DataRcvtextBox.Text = SP1_SendTextData();
                                        break;
                                    case 2:
                                        SP1_DataRcvtextBox.Text += nl +SP1_SendTextData();
                                        break;
                                    case 3:
                                        SP1_DataRcvtextBox.Text += nl + SP1_SendTextData();
                                        break;
                                    case 4:
                                        SP1_DataRcvtextBox.Text += nl + SP1_SendTextData();
                                   //     Flash500ByteCountPrev = 0;
                                        break;
                                    case 5:
                                        SP1_DataRcvtextBox.Text += nl +  SP1_SendTextData();
                                        Flash500ByteCount = 0;
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        
                        break;
                    case 2: //SP1_richTextBox.Text = SP1_GetTextdata(); 

                                      
                            if (Enable_DATA_CommandFlashArray_ReadFill_Grid == true)
                            {
                                DATA_CommandFlashArray_ReadFill_Grid(); // 100 byte lik flash okumadan sonra gridi doldurur..
                                Enable_DATA_CommandFlashArray_ReadFill_Grid = false;
                            }



                        break;
                    case 3:
                        TES1307_01 = (Okyanus.Chart.TES1307_01_Raw / 10).ToString() + "." + (Okyanus.Chart.TES1307_01_Raw % 10).ToString();
                        TES1307_02 = (Okyanus.Chart.TES1307_02_Raw / 10).ToString() + "." + (Okyanus.Chart.TES1307_02_Raw % 10).ToString();
                        if (Okyanus.Chart.TES1307_01_Raw == 0X3FFF) TES1307_01 = "---";
                        if (Okyanus.Chart.TES1307_02_Raw == 0X3FFF) TES1307_02 = "---";
                        textBox_SP3_Receive.Text = " TES1307 01: " + TES1307_01 + "  02: " + TES1307_02; // +Okyanus.SeriPort3.Receive_Buffer[6].ToString("x") + "  ";


                        break;
                    /*
              
                                    textBox_FlashRead.Text = "uC Flash tan okunan" + nl;
                                    for (int i = 0; i < (4); i++) // 30 kontrol bytlerini iceren kismi
                                        textBox_FlashRead.Text += i.ToString() + "->" + Okyanus.Variables.FlashBuffer[i].ToString() + "         ";

                                    textBox_FlashRead.Text += nl;
                                    for (int i = 4; i < (100 + 6); i++) // 30 kontrol bytlerini iceren kismi
                                        textBox_FlashRead.Text += i.ToString() + "->" + Okyanus.Variables.FlashBuffer[i].ToString() + "         ";



                                    textBox_FlashRead.Text += "Length" + Okyanus.SP1.Length.ToString();
                                    textBox_FlashRead.Text += nl + "Preamble:" + Okyanus.Variables.Preamble_str;

                                    textBox_RamRead.Text = "uC Ram den  okunan" + nl;
                                    for (int i = 0; i < (20 + 6); i++) // 6 kontrol bytlerini iceren kismi
                                        textBox_RamRead.Text += i.ToString() + ".." + Okyanus.Variables.RamBuffer[i].ToString() + "  ";

                                    textBox_AckRead.Text = "uC Gelen Ack" + nl;
                                    for (int i = 0; i < 3; i++)
                                        textBox_AckRead.Text += i.ToString() + ".." + Okyanus.Variables.AckBuffer[i].ToString() + " ";
                    */


                    /*
                                     for (uint i = 0; i < 20; i++)
                                     {
                                         //   Okyanus.Variables.SP_ReceivedData[i] = i+100;// test
                                                Okyanus.Variables.SP_ReceivedData[i] = (uint)SP1_ReceiveBuf[i+4]; 
                                        str +=  Okyanus.Variables.SP_ReceivedData[i].ToString();
                                     }
                    */
                    case 5:
                        if (writeVerifytimer == Okyanus.Definitions.DISPLAY_TEXT_WRITING)
                        {
                            if (Flash500ByteWrite == false)
                            {
                                Botton100_DISPLAY_TEXT_WRITING();    
                            }
                            else
                            {
                                Botton500_DISPLAY_TEXT_WRITING();                  
                            }
                        }
                        if (writeVerifytimer == Okyanus.Definitions.DISPLAY_TEXT_WRITTEN)
                            {
                                if (Flash500ByteWrite == false)
                                {
                                    Botton100_DISPLAY_TEXT_WRITTEN();
                                }
                                else {
                                    Botton500_DISPLAY_TEXT_WRITTEN();
                                }
                          }
                            if (writeVerifytimer == Okyanus.Definitions.DISPLAY_TEXT_END)
                            {
                                Botton100_DISPLAY_TEXT_END();
                                Botton500_DISPLAY_TEXT_END();
                                Flash500ByteWrite = false;// yazma uyari islemi bittikten sonra o yapsin
                            }

                        if (writeVerifytimer != 0)
                        {
                            writeVerifytimer--;
                        }
                        break;
                    case 6:
                        if (panel_Simulasyon.Visible == false) break;

                        textBox_Sim_thu.Text = Decimal.ToInt32(numericUpDown_Par1.Value).ToString() + " °C";
                        textBox_Sim_thk.Text = Decimal.ToInt32(numericUpDown_Par0.Value).ToString() + " °C";

                        textBox_Sim_tk.Text = Okyanus.Variables.Variables_Data[0] + " °C";

                        textBox_Sim_tu.Text = Okyanus.Variables.Variables_Data[1] + " °C";

                        textBox_Sim_mvt.Text = Okyanus.Variables.Variables_Data[4] + " Gram";

                        textBox_Sim_tva.Text = Okyanus.Variables.Variables_Data[2] + " °C";
                        textBox_Sim_tvki.Text = Okyanus.Variables.Variables_Data[3] + " °C";
                        textBox_Sim_tv1.Text = Okyanus.Variables.Variables_Data[5] + " Sn";


                        break;

                    case 7:
                        systemTimeToolStripMenuItem1.Text = " System Time & Date: " + COMMON_GetDateTime();
                        if (Okyanus.Variables.UpdateGrid500Table == true) // bitti mesaji
                        {
                        switch (Okyanus.Variables.Version)
                        {
                            case Okyanus.Definitions.BJUEmulatorKisitli:
                                break;
                            case Okyanus.Definitions.BJUEmulator:
                                break;
                            case Okyanus.Definitions.KahveMakinasiEmulator2:Byte500GridWriteV1();   // kahve2
                                break;
                            case Okyanus.Definitions.TostMakinasiEmulatoru2: Byte500GridWriteV2();  // tost
                                break;
                            case Okyanus.Definitions.KahveMakinasiEmulator1:
                                break;
                            default:
                                break;
                        }

                            button500byteOku.Text = Flash500ButtonReadSuccessMessage;
                            button500byteOku.ForeColor = Color.Green;
                            button500byteOku2.Text = Flash500ButtonReadSuccessMessage;
                            button500byteOku2.ForeColor = Color.Green;

                            Okyanus.Variables.UpdateGrid500Table = false;
                            Okyanus.Variables.FlashRead500Phase = 0;

                            Okyanus.Variables.FlashRead500EndMessageEnable = true;
                            Okyanus.Variables.FlashRead500EndMessageTimer = 0;
                        }
                         if (Okyanus.Variables.FlashRead500EndMessageEnable == true){
                             Okyanus.Variables.FlashRead500EndMessageTimer++;
                            if (Okyanus.Variables.FlashRead500EndMessageTimer > 5)
                            {
                                Okyanus.Variables.FlashRead500EndMessageEnable = false;
                                Okyanus.Variables.FlashRead500EndMessageTimer = 0;

                                button500byteOku.Text = Flash500ButtonDefaultMessage;
                                button500byteOku.ForeColor = Color.Black;
                                button500byteOku2.Text = Flash500ButtonDefaultMessage;
                                button500byteOku2.ForeColor = Color.Black;


                            }
                         }
                       
                        if (Okyanus.Variables.FlashRead500Phase == 0) Okyanus.Variables.FlashRead500Timeout = 0;
                        else Okyanus.Variables.FlashRead500Timeout++;


                        if (Okyanus.Variables.FlashRead500Timeout > 8)
                        {
                            button500byteOku.Text = "Hata !!!";
                            button500byteOku2.Text = "Hata !!!";                         
                        }

                        if (Okyanus.Variables.FlashRead500Timeout > 15)
                        {
                            Okyanus.Variables.FlashRead500Timeout = 0;
                            Okyanus.Variables.FlashRead500Phase = 0;
                            Okyanus.Variables.UpdateGrid500Table = false;

                            button500byteOku.Text = Flash500ButtonDefaultMessage;
                            button500byteOku.ForeColor = Color.Black;
                            button500byteOku2.Text = Flash500ButtonDefaultMessage;
                            button500byteOku2.ForeColor = Color.Black;
                        }
                        break;
                    case 8:
                    switch (Okyanus.Variables.Version)
                    {
                        case Okyanus.Definitions.BJUEmulatorKisitli:


    
                            break;
                        case Okyanus.Definitions.BJUEmulator: //this.Text = "BJU Emulator V";


                            break;
                        case Okyanus.Definitions.KahveMakinasiEmulator1:

                            break;
                        case Okyanus.Definitions.KahveMakinasiEmulator2:

                            //   button_KM2_01.Enabled = false;

                            //    button_KM2_01.BackColor = Color.GreenYellow;

                            //     if (Okyanus.Variables.Variables_Data[0] & 0x01)

                            //       byte Buff;

                      //      Okyanus.Variables.RamBuffer[0] = 0x08;
                      

                            if ((KahveSim & 0x80) == 0x80) button_KM2_08.BackColor = Color.GreenYellow;
                            else button_KM2_08.BackColor = Color.Transparent;
                            if ((KahveSim & 0x40) == 0x40) button_KM2_07.BackColor = Color.GreenYellow;
                            else button_KM2_07.BackColor = Color.Transparent;
                            if ((KahveSim & 0x20) == 0x20) button_KM2_06.BackColor = Color.GreenYellow;
                            else button_KM2_06.BackColor = Color.Transparent;
                            if ((KahveSim & 0x10) == 0x10) button_KM2_05.BackColor = Color.GreenYellow;
                            else button_KM2_05.BackColor = Color.Transparent;
                            if ((KahveSim & 0x08) == 0x08) button_KM2_04.BackColor = Color.GreenYellow;
                            else button_KM2_04.BackColor = Color.Transparent;
                            if ((KahveSim & 0x04) == 0x04) button_KM2_03.BackColor = Color.GreenYellow;
                            else button_KM2_03.BackColor = Color.Transparent;
                            if ((KahveSim & 0x02) == 0x02) button_KM2_02.BackColor = Color.GreenYellow;
                            else button_KM2_02.BackColor = Color.Transparent;
                            if ((KahveSim & 0x01) == 0x01) button_KM2_01.BackColor = Color.GreenYellow;
                            else button_KM2_01.BackColor = Color.Transparent;


                            textBoxTost1.Text = "0 : " + Okyanus.Variables.Variables_Data[0];
                            textBoxTost2.Text = "1 : " + Okyanus.Variables.Variables_Data[1];
                            textBoxTost3.Text = "2 : " + Okyanus.Variables.Variables_Data[2];
                            textBoxTost4.Text = "3 : " + Okyanus.Variables.Variables_Data[3];
                            textBoxTost5.Text = "4 : " + Okyanus.Variables.Variables_Data[4] ; // alt sicaklik
                            textBoxTost6.Text = "5 : " + Okyanus.Variables.Variables_Data[5] ;// ust  sicaklik
                            textBoxTost7.Text = "6 : " + Okyanus.Variables.Variables_Data[6];
                            textBoxTost8.Text = "7 : " + Okyanus.Variables.Variables_Data[7];
                            textBoxTost9.Text = "8 : " + Okyanus.Variables.Variables_Data[8];
                            textBoxTost10.Text = "9 : " + Okyanus.Variables.Variables_Data[9];
                            textBoxTost11.Text = "10 : " + Okyanus.Variables.Variables_Data[10];
                            break;

                        case Okyanus.Definitions.TostMakinasiEmulatoru2:
                            textBoxTost1.Text = "DURUM : " + Okyanus.Variables.Variables_Data[2];
                            textBoxTost2.Text = "P_ADIM : " + Okyanus.Variables.Variables_Data[3];
                            textBoxTost3.Text = "P_KOMUT : " + Okyanus.Variables.Variables_Data[4];
                            textBoxTost4.Text = "ADIM_TIMER : " + Okyanus.Variables.Variables_Data[5];
                            textBoxTost5.Text = "T_ALT_SIC : " + Okyanus.Variables.Variables_Data[6] + "°C"; // alt sicaklik
                            textBoxTost6.Text = "T_UST_SIC : " + Okyanus.Variables.Variables_Data[7] + "°C";// ust  sicaklik
                            textBoxTost7.Text = "G_ZAM_0 : " + Okyanus.Variables.Variables_Data[8];
                            textBoxTost8.Text = "G_ZAM_1 : " + Okyanus.Variables.Variables_Data[9];
                            textBoxTost9.Text = "G_ZAM_2 : " + Okyanus.Variables.Variables_Data[10];
                            textBoxTost10.Text = "G_ZAM_3 : " + Okyanus.Variables.Variables_Data[11];
                            textBoxTost11.Text = "Variable13 : " + Okyanus.Variables.Variables_Data[12];
                            break;
                        default:
                           // this.Text = "Model secilmedi !!!";
                            break;

                    }




                        String strtext= "1.." + Okyanus.DebugVar.Write500_1.ToString() + nl;
                        strtext += "2.." +  Okyanus.DebugVar.Write500_2.ToString() + nl;
                        strtext += "3.." +  Okyanus.DebugVar.Write500_3.ToString() + nl;
                        strtext += "4.." +  Okyanus.DebugVar.Write500_4.ToString() + nl;
                        strtext += "5.." +  Okyanus.DebugVar.Write500_5.ToString() + nl;
                        strtext += "6.." +  Okyanus.DebugVar.Write500_6.ToString() + nl;
                        strtext += "7.." +  Okyanus.DebugVar.Write500_7.ToString() + nl;
                        strtext += "8.." +  Okyanus.DebugVar.Write500_8.ToString() + nl;
                        strtext += "9.." +  Okyanus.DebugVar.Write500_9.ToString() + nl;
                        strtext += "10." +  Okyanus.DebugVar.Write500_10.ToString() + nl;
                        strtext += "11." +  Okyanus.DebugVar.Write500_11.ToString() + nl;

                        strtext += "a." + Okyanus.DebugVar.Write500_a.ToString() + nl;
                        strtext += "b.." + Okyanus.DebugVar.Write500_b.ToString() + nl;
                        strtext += "c.." + Okyanus.DebugVar.Write500_c.ToString() + nl;
                        strtext += "d.." + Okyanus.DebugVar.Write500_d.ToString() + nl;
                        strtext += "e.." + Okyanus.DebugVar.Write500_e.ToString() + nl;
 


                  //      strtext += "7.." +  Okyanus.DebugVar.Write500_.ToString() + nl;                                           
         


                        
                        textBoxDebugforwrite500.Text = "Debug Screeen" + nl + strtext;
                            break;
                    default: break;
                }
            
            
            try
            {
                if (SP2_serialPort.BytesToRead != 0)
                {
                    SP2_ComTimeout++;
                }
                else SP2_ComTimeout = 0;

                if (SP2_ComTimeout > 2)
                {
                    // clear buffer
                    SP2_serialPort.ReadExisting();
                    SP2_ErrorCount++;
                }
            }
            catch { }


            BaseCounter++;
            BaseCounter = 0;
       
            if (Okyanus.SP1.ReadSequence == 2)
            {
 
            }


            try
            {
        //        SP1_CalculateRcvData();
                SP1_IO_Serial_UpdateCOMPortList();
                //      SP2_IO_Serial_UpdateCOMPortList();
            }
            catch { }
            try
            {
                
                SP3_IO_Serial_UpdateCOMPortList();
                //      SP2_IO_Serial_UpdateCOMPortList();
            }
            catch { }



            if (SP2_ReadSequence == 2)
            {
                SP2_ReadSequence = 0;

                SP2_CalculateRcvData();
                SP2_richTextBox.Text = SP2_GetTextdata();
                SP2_DatatextBox.Text = SP2_GetTextdata2();
            }
            try
            {
                SP2_IO_Serial_UpdateCOMPortList();
            }
            catch { }
        }

        ///  serial port 1 connect

        private void Botton100_DISPLAY_TEXT_WRITING()
        {
            YazmaBasarili.Text = "Yaziliyor Bekle !";
     //       YazmaBasarili.BackColor = Color.GhostWhite;
            YazmaBasarili.ForeColor = Color.Red;
            YazmaBasarili2.Text = "Yaziliyor Bekle !";  //Yaziliyor Bekle !
    //        YazmaBasarili2.BackColor = Color.GhostWhite;
            YazmaBasarili2.ForeColor = Color.Red;
        }
        private void Botton100_DISPLAY_TEXT_WRITTEN()
        {
            YazmaBasarili.Text = "Yazma Onaylandi";
     //       YazmaBasarili.BackColor = Color.GhostWhite;
            YazmaBasarili.ForeColor = Color.Green;
            YazmaBasarili2.Text = "Yazma Onaylandi";  //Yaziliyor Bekle !
    //        YazmaBasarili2.BackColor = Color.GhostWhite;
            YazmaBasarili2.ForeColor = Color.Green;
        }
        private void Botton100_DISPLAY_TEXT_END()
        {
            //    if (Flash500ByteWrite == false){
            YazmaBasarili.Text = "Yazma Onay Bilgisi";
  //          YazmaBasarili.BackColor = Color.AntiqueWhite;
            YazmaBasarili.ForeColor = Color.Black;
            YazmaBasarili2.Text = "Yazma Onay Bilgisi";
 //           YazmaBasarili2.BackColor = Color.AntiqueWhite;
            YazmaBasarili2.ForeColor = Color.Black;

        }
        private void Botton500_DISPLAY_TEXT_WRITING()
        {
            Yazma500Basarili2.Text = "Yaziliyor Bekle !";
      //      Yazma500Basarili2.BackColor = Color.GhostWhite;
            Yazma500Basarili2.ForeColor = Color.Red;
            //     button1.Text = "500 Byte ";
            //        button1.BackColor = Color.GhostWhite;
            button500byteFlash1.ForeColor = Color.Red;
            button500byteFlash1.Text = "Flash a 500" + nl + " Yaziliyor !";
            button500byte_2.ForeColor = Color.Red;
            button500byte_2.Text = "Flash a 500" + nl + " Yaziliyor !";

            
            //          button1.Enabled = false;
        }
        private void Botton500_DISPLAY_TEXT_WRITTEN()
        {
            Yazma500Basarili2.Text = "Yazma Onaylandi";
       //     Yazma500Basarili2.BackColor = Color.GhostWhite;
            Yazma500Basarili2.ForeColor = Color.Green;
            button500byteFlash1.ForeColor = Color.Green;
            button500byteFlash1.Text = "Flash a 500" + nl + "  Yazildi !";
            button500byte_2.ForeColor = Color.Green;
            button500byte_2.Text = "Flash a 500" + nl + "  Yazildi !";


        }
        private void Botton500_DISPLAY_TEXT_END()
        {
      Yazma500Basarili2.Text = "Yazma Onay Bilgisi";
  //    Yazma500Basarili2.BackColor = Color.AntiqueWhite;
      Yazma500Basarili2.ForeColor = Color.Black;
  //    button1.BackColor = Color.PaleGoldenrod;
      button500byteFlash1.ForeColor = Color.Black;
      button500byteFlash1.Text = "Flash a 500" + nl + "   Yaz";
   //   button1.Enabled = true;
      button500byte_2.ForeColor = Color.Black;
      button500byte_2.Text = "Flash a 500" + nl + "   Yaz";
            


  }

        bool Flash500ByteWriteText;

        private void button2_Click(object sender, EventArgs e)
        {
            //   textBox1.Clear();
            //Okyanus.SP_oky.deneme = 16;
        }



        /*              ******************  serial port1 ***********************************************************/
        private void SP1_IO_Serial_UpdateCOMPortList()
        {
            int i; i = 0; bool foundDifference;
            foundDifference = false;
            //   textBox_PortName.Text = PortConnectName;
            //If the number of COM ports is different than the last time we
            //  checked, then we know that the COM ports have changed and we
            //  don't need to verify each entry.
            // if (IO_Serial_lstCOMPorts.Items.Count == SerialPort.GetPortNames().Length)
            if (SP1_IO_Serial_lstCOMPorts.Items.Count == SerialPort.GetPortNames().Length)
            {
                try
                {
                    foreach (string s in SerialPort.GetPortNames())
                    {
                        if (SP1_IO_Serial_lstCOMPorts.Items[i++].Equals(s) == false)
                        {
                            foundDifference = true;

                        }
                    }
                }
                catch { }
            }
            else foundDifference = true;
            if (foundDifference == false) return;
            //If something has changed, then clear the list
            SP1_IO_Serial_lstCOMPorts.Items.Clear();
            String PortPosition = "Bulunamadı";
            //Add all of the current COM ports to the list
            foreach (string s in SerialPort.GetPortNames())
            {
                SP1_IO_Serial_lstCOMPorts.Items.Add(s);
                if (s == SP1_PortConnectName) PortPosition = SP1_PortConnectName;
                SP1_IO_Serial_lstCOMPorts.SelectedIndex = 0; // 24.04.2014
            }
            if (PortPosition == "Bulunamadi")
            {

                SP1_DisConnect_Procedure();
            }
            SP1_textBox_PortName.Text = PortPosition;
            //Set the listbox to point to the first entry in the list
            //  SP1_IO_Serial_lstCOMPorts.SelectedIndex = 0; // 24.04.2014
        }
        void SP1_Connect_Procedure()
        {
            try
            {
                //Get the port name from the application list box.
                //  the PortName attribute is a string name of the COM
                //  port (e.g. - "COM1").
                SP1_serialPort.PortName = SP1_IO_Serial_lstCOMPorts.Items[SP1_IO_Serial_lstCOMPorts.SelectedIndex].ToString();
                SP1_PortConnectName = SP1_serialPort.PortName;
                SP1_textBox_PortName.Text = SP1_PortConnectName;
                //Open the COM port.
                SP1_serialPort.Open();

                //Change the state of the application objects
                //   btnConnect.Enabled = false;
                SP1_ConnectButton.Enabled = false;
                SP1_IO_Serial_lstCOMPorts.Enabled = false;
                //   btnClose.Enabled = true;
                SP1_DisConnectButton.Enabled = true;
                //            textBox1.Clear(); 
                //            textBox1.AppendText("Connected.\r\n");

            }
            catch
            {
                //If there was an exception, then close the handle to 
                //  the device and assume that the device was removed
                //  button_Close_Click(this, null);

                SP1_DisConnect_Procedure();
                SP1_PortConnectName = "BağlantıYok!";
            }

        }
        void SP1_DisConnect_Procedure()
        {
            //Reset the state of the application objects
            SP1_PortConnectName = "BağlantıYok!";
            SP1_textBox_PortName.Text = SP1_PortConnectName;
            SP1_DisConnectButton.Enabled = false;
            SP1_ConnectButton.Enabled = true;
            SP1_IO_Serial_lstCOMPorts.Enabled = true;

            //This section of code will try to close the COM port.
            //  Please note that it is important to use a try/catch
            //  statement when closing the COM port.  If a USB virtual
            //  COM port is removed and the PC software tries to close
            //  the COM port before it detects its removal then
            //  an exeception is thrown.  If the execption is not in a
            //  try/catch statement this could result in the application
            //  crashing.
            try
            {
                //Dispose the In and Out buffers;
                SP1_serialPort.DiscardInBuffer();
                SP1_serialPort.DiscardOutBuffer();

                //Close the COM port
                SP1_serialPort.Close();
            }
            //If there was an exeception then there isn't much we can
            //  do.  The port is no longer available.
            catch { }
        }
        int writeVerify;
        int writeVerifytimer;
        int Flash500ByteCount;
        int Flash500ByteCountPrev;
        void SP1_DisposeReceivedToBuffers()
        {
            //     UInt16 CRC_Received = (UInt16)Okyanus.SP1.Length; // 

            //        for (int i = 0; i < Okyanus.SP1.Length - (PREAMBLE_BYTES + DATALENGTH_BYTES); i++)
            //           CRC_Received += SP1_ReceiveBuf[i];

            //      UInt16 CRC_Calculated = (UInt16)((SP1_ReceiveBuf[Okyanus.SP1.Length - 1] << SHIFT8) + SP1_ReceiveBuf[Okyanus.SP1.Length - 2]);

            //    if (CRC_Calculated != CRC_Received) return;
            int k = 0;
     //       int Offset = 4;
            switch (SP1_ReceiveBuf[0])  // komut bufferina bak ona gore dagit
            {
                    
                case 12:  // flash read  
                    //     case 40:  // flash read  
                    for (int i = 0; i < Okyanus.SP1.Length - (PREAMBLE_BYTES + DATALENGTH_BYTES); i++) // 26 byte kaldi
                    {
                        Okyanus.Variables.FlashBuffer[i] = SP1_ReceiveBuf[i];
                        //          Okyanus.Variables.RamBuffer_str += Okyanus.Variables.RamBuffer[i].ToString();
                    }
                    writeVerify = 0;
                    //        Okyanus.Variables.FlashBuffer[Okyanus.SP1.Length - (PREAMBLE_BYTES + DATALENGTH_BYTES)] = Okyanus.SP1.Length;
                    // okuma basarili ise grisleri guncelle
                    if (Okyanus.Variables.FlashRead100byte == true)
                   //     DATA_CommandFlashArray_ReadFill_Grid(); // 100 byte lik flash okumadan sonra gridi doldurur..
                        Enable_DATA_CommandFlashArray_ReadFill_Grid = true;
                    else
                    {
                        switch (Okyanus.Variables.FlashRead500Phase)
                        {
                            case 0: k = 0;
                                break;
                            case 1: k = 0;
                                break;
                            case 2: k = 50;
                                break;
                            case 3: k = 100;
                                break;
                            case 4: k = 150;
                                break;
                            case 5: k = 200;
                                break;

                            default: k = 0;
                                break;

                        }
                //        byte BigOffset = 0;
                        for (int i = 0; i < 50; i++)
                        {
       
                           Okyanus.Variables.ReadBuffer[i + k] = SP1_ReceiveBuf[(i * 2) + 4] * 256 + SP1_ReceiveBuf[(i * 2) + 4 + 1];  // dogru
                        }                    
                    }

                    switch (Okyanus.Variables.FlashRead500Phase)
                    {
                        case 0 : // default bos
                            break;
                        case 1: ReadFlash500RequestProcedure(10100); // 10000 geldi
                            break;
                        case 2: ReadFlash500RequestProcedure(10200);
                            break;
                        case 3: ReadFlash500RequestProcedure(10300);
                            break;
                        case 4: ReadFlash500RequestProcedure(10400);// 10300 geldi
  
                            break;
                        case 5:  // 10400 geldi
                                Okyanus.Variables.UpdateGrid500Table = true;
                                Okyanus.Variables.FlashRead100byte = false;
                    //        Okyanus.Variables.FlashRead500Phase = 0;
                            
                            break;

                        default: Okyanus.Variables.FlashRead500Phase = 0;
                            break;

                    }


                    break;
                case 14: // ram  read
                    for (int i = 0; i < Okyanus.SP1.Length - (PREAMBLE_BYTES + DATALENGTH_BYTES); i++) // 26 byte kaldi
                    {
                        Okyanus.Variables.RamBuffer[i] = SP1_ReceiveBuf[i];
                        //          Okyanus.Variables.RamBuffer_str += Okyanus.Variables.RamBuffer[i].ToString();
                    }
          //          LOg_ViewTimer = 9;
                    Okyanus.Variables.LOg_ViewTimer =9;
                    writeVerify = 0;
                    break;
                case 20:  // ack read
                    Okyanus.DebugVar.Write500_7++;

                    for (int i = 0; i < Okyanus.SP1.Length - (PREAMBLE_BYTES + DATALENGTH_BYTES); i++)
                    {
                        Okyanus.Variables.AckBuffer[i] = SP1_ReceiveBuf[i];
                        //          Okyanus.Variables.AckBuffer_str += Okyanus.Variables.AckBuffer[i].ToString();
                        Okyanus.DebugVar.Write500_8++;
                    }
                    if (writeVerify == 12) // yuz baytlik flash onayi
                    {
                       writeVerifytimer = Okyanus.Definitions.DISPLAY_TEXT_WRITING;

                        if (Flash500ByteWrite == false)
                        { // TEK FRAME FLASH YAZMA 
                            writeVerifytimer = Okyanus.Definitions.DISPLAY_TEXT_WRITTEN;
                            writeVerify = 0;
                        }
                        else
                        {   // 5 FRAME FLASH YAZMA

                            Okyanus.DebugVar.Write500_9++;

                            Flash500ByteCount++;
                            Flash500ByteWriteTimer = 0;// timeout i sifirla
                            Flash500ByteWriteTimeoutTimer = 0;
                            if (Flash500ByteCount >= 5)
                            {
                                writeVerifytimer = Okyanus.Definitions.DISPLAY_TEXT_WRITTEN;
                                writeVerify = 0;

                                Okyanus.DebugVar.Write500_10++;
                            }
                        }
                    }

                    break;
                default:

                    break;
            }
        }
        string GetCommands_FromNumbers(int Data)
        {
            string str = Data.ToString();
            switch (str)
            {
                case "0": str = Okyanus.Definitions.Komut00; break;
                case "1": str = Okyanus.Definitions.Komut01; break;
                case "2": str = Okyanus.Definitions.Komut02; break;
                case "3": str = Okyanus.Definitions.Komut03; break;
                case "4": str = Okyanus.Definitions.Komut04; break;
                case "5": str = Okyanus.Definitions.Komut05; break;
                case "6": str = Okyanus.Definitions.Komut06; break;
                case "7": str = Okyanus.Definitions.Komut07; break;
                case "8": str = Okyanus.Definitions.Komut08; break;
                case "9": str = Okyanus.Definitions.Komut09; break;
                case "10": str = Okyanus.Definitions.Komut10; break;
                case "11": str = Okyanus.Definitions.Komut11; break;
                case "12": str = Okyanus.Definitions.Komut12; break;
                case "13": str = Okyanus.Definitions.Komut13; break;
                case "14": str = Okyanus.Definitions.Komut14; break;
                case "15": str = Okyanus.Definitions.Komut15; break;
                case "16": str = Okyanus.Definitions.Komut16; break;
                case "17": str = Okyanus.Definitions.Komut17; break;
                case "18": str = Okyanus.Definitions.Komut18; break;
                case "19": str = Okyanus.Definitions.Komut19; break;
                case "20": str = Okyanus.Definitions.Komut20; break;
                case "21": str = Okyanus.Definitions.Komut21; break;
                case "22": str = Okyanus.Definitions.Komut22; break;
                case "23": str = Okyanus.Definitions.Komut23; break;
                case "24": str = Okyanus.Definitions.Komut24; break;
                case "25": str = Okyanus.Definitions.Komut25; break;
                case "26": str = Okyanus.Definitions.Komut26; break;
                case "27": str = Okyanus.Definitions.Komut27; break;
                case "28": str = Okyanus.Definitions.Komut28; break;
                case "29": str = Okyanus.Definitions.Komut29; break;
                case "30": str = Okyanus.Definitions.Komut30; break;
                case "31": str = Okyanus.Definitions.Komut31; break;
                case "32": str = Okyanus.Definitions.Komut32; break;
                case "33": str = Okyanus.Definitions.Komut33; break;
                case "34": str = Okyanus.Definitions.Komut34; break;
                case "35": str = Okyanus.Definitions.Komut35; break;
                case "36": str = Okyanus.Definitions.Komut36; break;
                case "37": str = Okyanus.Definitions.Komut37; break;
                case "38": str = Okyanus.Definitions.Komut38; break;
                case "39": str = Okyanus.Definitions.Komut39; break;
                case "40": str = Okyanus.Definitions.Komut40; break;
                case "41": str = Okyanus.Definitions.Komut41; break;
                case "42": str = Okyanus.Definitions.Komut42; break;
                case "43": str = Okyanus.Definitions.Komut43; break;
                case "44": str = Okyanus.Definitions.Komut44; break;
                case "45": str = Okyanus.Definitions.Komut45; break;
                case "46": str = Okyanus.Definitions.Komut46; break;
                case "47": str = Okyanus.Definitions.Komut47; break;
                case "48": str = Okyanus.Definitions.Komut48; break;
                case "49": str = Okyanus.Definitions.Komut49; break;

                default:
                    break;

            }
            return str;
        }
        void Byte500GridWriteV1()
        {  // ilk versiyon 10 sutun 25 satir
            // kahve2 
            try
            {
                GridHelper gridHelper500 = new GridHelper(dt_500Command);
                DataTable dataElements = gridHelper500.GetDataTable4_500Byte50Line6Column();

                int j = 0;
                for (int i = 0; i < 50; i++)
                {
                    DataRow dr = dataElements.NewRow();
                    j = i * 5;//5->10
                              //   dr[0] = (i+1).ToString();
                    dr[0] = (i).ToString();
                    dr[1] = GetCommands_FromNumbers(Okyanus.Variables.ReadBuffer[j]);
                    dr[2] = Okyanus.Variables.ReadBuffer[j + 1];
                    dr[3] = Okyanus.Variables.ReadBuffer[j + 2];
                    dr[4] = Okyanus.Variables.ReadBuffer[j + 3];
                    dr[5] = Okyanus.Variables.ReadBuffer[j + 4];
                    //    dr[5] = Okyanus.Variables.ReadBuffer[j + 5];// artti bes tane row
                    dataElements.Rows.Add(dr);  // row eklemek icin kullan
                }
                gridHelper500.PrintScreen();
            }
            catch
            {
                MessageBox.Show("View Error!!");
            }
        }


        void Byte500GridWriteV2()  // tost
        {  // ilk versiyon 10 sutun 25 satir
            try
            {
                GridHelper gridHelper500 = new GridHelper(dt_500Command2);
                DataTable dataElements = gridHelper500.GetDataTable4_500Byte50Line6Column();
               
                int j = 0;
                for (int i = 0; i < 50; i++)
                {
                    DataRow dr = dataElements.NewRow();
                    j = i * 5;//5->10
                 //   dr[0] = (i+1).ToString();
                    dr[0] = (i).ToString();
                    dr[1] = GetCommands_FromNumbers(Okyanus.Variables.ReadBuffer[j]);
                    dr[2] = Okyanus.Variables.ReadBuffer[j + 1];
                    dr[3] = Okyanus.Variables.ReadBuffer[j + 2];
                    dr[4] = Okyanus.Variables.ReadBuffer[j + 3];
                    dr[5] = Okyanus.Variables.ReadBuffer[j + 4];
                //    dr[5] = Okyanus.Variables.ReadBuffer[j + 5];// artti bes tane row
                    dataElements.Rows.Add(dr);  // row eklemek icin kullan
                }
                gridHelper500.PrintScreen();
            }
            catch
            {
                MessageBox.Show("View Error!!");
            }
        }


void Handle500Byte(){
    if (Flash500ByteWrite == false) return;// timeout check
        Flash500ByteWriteTimeoutTimer++;
        if (Flash500ByteWriteTimeoutTimer > Okyanus.Definitions.FLASH500BYTE_TIMEOUT)
        {
            Flash500ByteWriteTimeoutTimer = 0; // timeout           
            Flash500ByteCount = 0;
            Flash500ByteWriteTimer = 0;
             Flash500ByteWrite = false; // bunu kaldridim bir ustteki satir dki kod yapacak bunu
             Botton500_DISPLAY_TEXT_END();
            return;
        }
        Flash500ByteWriteTimer++;
        if (Flash500ByteWriteTimer < Okyanus.Definitions.FLASH_500_TIME_INTERVAL) return;
        Flash500ByteWriteTimer = 0;
            // CALISMAYAN DEGUSIKLIK ICIN EKLENEN ESKI KOD 23Nisan2016
            switch (Flash500ByteCount)
            {
                case 0: // ilk 100 byte gitti ama henuz ack gelmedi
                    break;
                case 1:
                    WriteToFlash500Procedure(10100); //WriteToFlashProcedure(10100);// ilk ack geldi ilk 100 byte gitti
                    break;
                case 2:
                    WriteToFlash500Procedure(10200); //WriteToFlashProcedure(10200);
                    break;
                case 3:
                    WriteToFlash500Procedure(10300); //WriteToFlashProcedure(10300);
                    break;
                case 4:
                    WriteToFlash500Procedure(10400); //WriteToFlashProcedure(10400);
                    break;
                case 5:  // son  ack geldi son 100 byte gitti

                    break;
                default:
                    break;
            }
            // CALISMAYAN DEGUSIKLIK 23Nisan2016
            /*
            switch (Flash500ByteCount){
			case 0 : // ilk 100 byte gitti ama henuz ack gelmedi
				break;
            case 1:

                if (Okyanus.DebugVar.Write500_b == 0)
                {
                    WriteToFlash500Procedure(10100); //WriteToFlashProcedure(10100);// ilk ack geldi ilk 100 byte gitti
                    Okyanus.DebugVar.Write500_b++;
                }
				break;
            case 2:
                if (Okyanus.DebugVar.Write500_c == 0)
                {
                    WriteToFlash500Procedure(10200); //WriteToFlashProcedure(10200);
                    Okyanus.DebugVar.Write500_c++;
                }
				break;
            case 3:
                if (Okyanus.DebugVar.Write500_d == 0)
                {
                    WriteToFlash500Procedure(10300); //WriteToFlashProcedure(10300);
                    Okyanus.DebugVar.Write500_d++;
                }
				break;
            case 4:
                if (Okyanus.DebugVar.Write500_e == 0)
                {
                    WriteToFlash500Procedure(10400); //WriteToFlashProcedure(10400);
                    Okyanus.DebugVar.Write500_e++;
                }
				break;
			case 5 :  // son  ack geldi son 100 byte gitti

				break;
			default:
				break;
		}
        */

            Flash500ByteWriteTimer = 0;
}

        bool Flash500ByteWrite;
        bool Flash500ByteWriteEnable;
        int Flash500ByteWriteTimer;
        int Flash500ByteWriteTimeoutTimer;

     
        void SP1_ReceiveData_Procedure()
        {
            Okyanus.DebugVar.Write500_2++;
            try
            {
                if (Okyanus.SP1.ReadSequence == 0)
                {
                    Okyanus.DebugVar.Write500_3++;
                    if (SP1_serialPort.BytesToRead >= (PREAMBLE_BYTES + DATALENGTH_BYTES)) // READ PREAMBLE AND LENGTH BYTES
                    {
                        SP1_serialPort.Read(SP1_FirstBuffer, 0, (PREAMBLE_BYTES + DATALENGTH_BYTES));

                        Okyanus.SP1.Preamble = (uint)((SP1_FirstBuffer[0] << SHIFT24) + (SP1_FirstBuffer[1] << SHIFT16) + (SP1_FirstBuffer[2] << SHIFT8) + SP1_FirstBuffer[3]);
                        Okyanus.SP1.Length = (uint)((SP1_FirstBuffer[4] << SHIFT8) + SP1_FirstBuffer[5]);
                        Okyanus.Variables.LengthError_str = Okyanus.SP1.Length.ToString();

                        Okyanus.Variables.Preamble_Count++;

                        //              Okyanus.Variables.Preamble_str = Okyanus.SP1.Preamble.ToString("X");
                        if (Okyanus.SP1.Preamble == DEFAULT_PREAMBLE) // CHECK IF PREAMLE IS EQUAL TO THE DEFAULT ONE
                        {
                            Okyanus.SP1.ReadSequence = 1; // PREAMBLE MATCHES WITH THE DEFAULT ONE
                            Okyanus.SP1.ReadTimeout = 50; // 32 * 10 ms timout

                            Okyanus.DebugVar.Write500_4++;

                        }
                        else
                        {
                            Okyanus.SP1.ReadSequence = 0;   // PREAMBLE MATCH FAIL
                            Okyanus.SP1.Length = 6; // default
                            SP1_serialPort.ReadExisting();
                            Okyanus.Variables.Preamble_errorcount++;
                            //        Okyanus.Variables.Length_errorcount++;

                            //   Okyanus.Variables.Preamble_str = Okyanus.SP1.Preamble.ToString("X");

                            Okyanus.Variables.PreambleError_str = Okyanus.Variables.Preamble_errorcount.ToString();
                            //   Okyanus.Variables.LengthError_str = Okyanus.Variables.Length_errorcount.ToString();

                            Okyanus.DebugVar.Write500_5++;
                        }
                    }
                }
             
                if (Okyanus.SP1.ReadSequence == 1) // IF MATCH SUCCESFULL GO ON READING THE REMAINING
                {
                    //                while(SP1_serialPort.BytesToRead >= Okyanus.SP1.Length - (PREAMBLE_BYTES + DATALENGTH_BYTES))



                    if (SP1_serialPort.BytesToRead >= (Okyanus.SP1.Length - (PREAMBLE_BYTES + DATALENGTH_BYTES))) // READ REMAINING CRC AND DATA BYTES IF ALL AT BUFFER
                    {
                        SP1_serialPort.Read(SP1_ReceiveBuf, 0, (int)(Okyanus.SP1.Length - (PREAMBLE_BYTES + DATALENGTH_BYTES)));  // READ REMAINING CRC AND DATA BYTES
                        //     Okyanus.SP1.ReadSequence = 2;
                        SP1_DisposeReceivedToBuffers();
                        Okyanus.SP1.ReadSequence = 0;
                        Okyanus.DebugVar.Write500_6++;
                    }

                }

            }
            catch
            {
                SP1_DisConnect_Procedure();
                //       TxtData = "hata";
                //;  TrigDisp = 1;
                Okyanus.SP1.ReadSequence = 0;
            }

        }
        void SP1_SendData_Procedure()
        {
            try
            {
           //     SP1_CalculateSendData();
                SP1_serialPort.Write(SP1_SendBuf, 0, (int)SP1_DEFAULT_LENGTH);
            }
            catch
            {
                SP1_DisConnect_Procedure();
            }
        }
        private void SP1_buttonSpecial_Click(object sender, EventArgs e)
        {
            if (Okyanus.Variables.WriteFlashEnable == true)
            {
                MessageBox.Show("Command Send Active" + nl + "First Disable Commands");
                return;
            }

            byte k = 0;


            //       	System::Int16(this->numericUpDown_InMidPip_01->Value* 10);
            //     	this->numericUpDown_InMidPip_01->Value = System::Decimal(Temperature.InMidPipe)/10;break;

            //    SP1_SendCommand = System::Int16(this->numericUpDown_InMidPip_01->Value* 10);;
            SP1_SendCommand = Convert.ToUInt32(numericUpDown1.Value);
            SP1_SendAdress = Convert.ToUInt32(numericUpDown2.Value);
            SP1_Send_AdrLength = Convert.ToUInt32(numericUpDown3.Value); ;

            SP1_Send_TotLength = (UInt16)(SP1_Send_AdrLength + 14); // kontrol byte + CRC byte
            //        SP1_Send_TotLength = 214;
            SP1_Buffer[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            SP1_Buffer[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            SP1_Buffer[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            SP1_Buffer[3] = (byte)DEFAULT_PREAMBLE;
            SP1_Buffer[4] = (byte)(SP1_Send_TotLength >> SHIFT8);
            SP1_Buffer[5] = (byte)SP1_Send_TotLength;

            SP1_Buffer[6] = (byte)SP1_SendCommand;
            SP1_Buffer[7] = (byte)(SP1_SendAdress >> SHIFT8);
            SP1_Buffer[8] = (byte)SP1_SendAdress;
            SP1_Buffer[9] = (byte)SP1_Send_AdrLength;

            for (int i = 10; i < (SP1_Send_TotLength - 4); i++)
            {
                SP1_Buffer[i] = k;
                k++;
            }
            // data 


            SP1_SendCRC = DEFAULT_CRC_INIT;

            for (int i = 4; i < (SP1_Send_TotLength - 4); i++)
            {
                SP1_SendCRC ^= SP1_Buffer[i];
            }

            SP1_Buffer[SP1_Send_TotLength - 4] = (byte)(SP1_SendCRC >> SHIFT24);
            SP1_Buffer[SP1_Send_TotLength - 3] = (byte)(SP1_SendCRC >> SHIFT16);
            SP1_Buffer[SP1_Send_TotLength - 2] = (byte)(SP1_SendCRC >> SHIFT8);
            SP1_Buffer[SP1_Send_TotLength - 1] = (byte)SP1_SendCRC;

            for (int i = 0; i < SP1_Send_TotLength; i++)
            {
                SP1_SendBuf[i] = SP1_Buffer[i];
            }


            try
            {
                SP1_serialPort.Write(SP1_SendBuf, 0, (int)SP1_Send_TotLength);

            }
            catch
            {
                SP1_DisConnect_Procedure();
            }
        }
        /*              ******************  serial port2 ***********************************************************/
        private void SP2_IO_Serial_UpdateCOMPortList()
        {
            int i; i = 0; bool foundDifference;
            foundDifference = false;
            if (SP2_IO_Serial_lstCOMPorts.Items.Count == SerialPort.GetPortNames().Length)
            {
                try
                {
                    foreach (string s in SerialPort.GetPortNames())
                    {
                        if (SP2_IO_Serial_lstCOMPorts.Items[i++].Equals(s) == false)
                        {
                            foundDifference = true;

                        }
                    }
                }
                catch { }
            }
            else foundDifference = true;
            if (foundDifference == false) return;
            //If something has changed, then clear the list
            SP2_IO_Serial_lstCOMPorts.Items.Clear();
            String PortPosition = "Bulunamadi";
            //Add all of the current COM ports to the list
            foreach (string s in SerialPort.GetPortNames())
            {
                SP2_IO_Serial_lstCOMPorts.Items.Add(s);
                if (s == SP2_PortConnectName) PortPosition = SP2_PortConnectName;
                SP2_IO_Serial_lstCOMPorts.SelectedIndex = 0;// 24.04.2014
            }
            if (PortPosition == "Bulunamadi")
            {
                //   this.SP2_DisConnectButton_Click(this, null);
                SP2_DisConnect_Procedure();
            }
            SP2_textBox_PortName.Text = PortPosition;
            //Set the listbox to point to the first entry in the list
            //    SP2_IO_Serial_lstCOMPorts.SelectedIndex = 0;// 24.04.2014
        }
        void SP2_Connect_Procedure()
        {
            try
            {
                //Get the port name from the application list box.
                //  the PortName attribute is a string name of the COM
                //  port (e.g. - "COM1").
                SP2_serialPort.PortName = SP2_IO_Serial_lstCOMPorts.Items[SP2_IO_Serial_lstCOMPorts.SelectedIndex].ToString();
                SP2_PortConnectName = SP2_serialPort.PortName;
                SP2_textBox_PortName.Text = SP2_PortConnectName;
                //Open the COM port.
                SP2_serialPort.Open();

                //Change the state of the application objects
                //   btnConnect.Enabled = false;
                SP2_ConnectButton.Enabled = false;
                SP2_IO_Serial_lstCOMPorts.Enabled = false;
                //   btnClose.Enabled = true;
                SP2_DisConnectButton.Enabled = true;

                //Clear the textbox and print that we are connected.
                // txtDataReceived.Clear();
                //          textBox1.Clear();
                //  txtDataReceived.AppendText("Connected.\r\n");
                //         textBox1.AppendText("Connected.\r\n");
                //        PortConnectSituation = Connected
            }
            catch
            {
                SP2_DisConnect_Procedure();
                SP2_PortConnectName = "BaglantiYok!";
            }

        }
        void SP2_DisConnect_Procedure()
        {
            SP2_PortConnectName = "BaglantiYok!";
            SP2_textBox_PortName.Text = SP2_PortConnectName;
            SP2_DisConnectButton.Enabled = false;
            SP2_ConnectButton.Enabled = true;
            SP2_IO_Serial_lstCOMPorts.Enabled = true;
            try
            {
                SP2_serialPort.DiscardInBuffer();
                SP2_serialPort.DiscardOutBuffer();
                SP2_serialPort.Close();
            }
            catch { }
        }
        void SP2_ReceiveData_Procedure()
        {
            try
            {
                if (SP2_ReadSequence == 0)
                {
                    if (SP2_serialPort.BytesToRead >= (PREAMBLE_BYTES + DATALENGTH_BYTES)) // READ PREAMBLE AND LENGTH BYTES
                    {
                        //           byte[] SP2_Buffer = new byte[256];
                        SP2_serialPort.Read(SP2_Buffer, 0, (PREAMBLE_BYTES + DATALENGTH_BYTES));

                        SP2_Preamble = (uint)((SP2_Buffer[0] << SHIFT24) + (SP2_Buffer[1] << SHIFT16) + (SP2_Buffer[2] << SHIFT8) + SP2_Buffer[3]);
                        SP2_Length = (uint)((SP2_Buffer[4] << SHIFT8) + SP2_Buffer[5]);
                        if (SP2_Preamble == DEFAULT_PREAMBLE) // CHECK IF PREAMLE IS EQUAL TO THE DEFAULT ONE
                        {
                            SP2_ReadSequence = 1; // PREAMBLE MATCHES WITH THE DEFAULT ONE
                        }
                        else SP2_ReadSequence = 0;   // PREAMBLE MATCH FAIL
                    }
                }
                if (SP2_ReadSequence == 1) // IF MATCH SUCCESFULL GO ON READING THE REMAINING
                {
                    if (SP2_serialPort.BytesToRead >= SP2_Length - (PREAMBLE_BYTES + DATALENGTH_BYTES)) // READ REMAINING CRC AND DATA BYTES IF ALL AT BUFFER
                    {
                        SP2_serialPort.Read(SP2_ReceiveBuf, 0, (int)SP2_Length - (PREAMBLE_BYTES + DATALENGTH_BYTES));  // READ REMAINING CRC AND DATA BYTES
                        SP2_ReadSequence = 2;
                    }
                }
            }
            catch
            {
                SP2_DisConnect_Procedure();
                //       TxtData = "hata";
                //;  TrigDisp = 1;
                SP2_ReadSequence = 0;
            }

        }
        void SP2_SendData_Procedure()
        {
            try
            {
                SP2_CalculateSendData();
                SP2_serialPort.Write(SP2_SendBuf, 0, (int)SP1_DEFAULT_LENGTH);
                SP2_SendtextBox.Text = SP2_SendTextData();
            }
            catch
            {
                SP2_DisConnect_Procedure();
            }
        }
        /*              ******************  serial port3 ***********************************************************/
        private void SP3_serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
             if (SP3_serialPort.BytesToRead >= 10)
             {
                 SP3_serialPort.Read(Okyanus.SeriPort3.Receive_Buffer, 0, 10);
                 if (Okyanus.SeriPort3.Receive_Buffer[0] == 2)
                 {
                     if (Okyanus.SeriPort3.Receive_Buffer[9] == 3)
                     {              
                         Okyanus.Chart.TES1307_01_Raw = ((Okyanus.SeriPort3.Receive_Buffer[5] & 0X3F) << 8) + Okyanus.SeriPort3.Receive_Buffer[6];
                         Okyanus.Chart.TES1307_02_Raw = ((Okyanus.SeriPort3.Receive_Buffer[7] & 0X3F) << 8) + Okyanus.SeriPort3.Receive_Buffer[8];
                     }                                     
                 }
                 else SP3_serialPort.DiscardInBuffer();    
             }                               
        }
        private void SP3_IO_Serial_UpdateCOMPortList()
        {
            int i; i = 0; bool foundDifference;
            foundDifference = false;
            //   textBox_PortName.Text = PortConnectName;
            //If the number of COM ports is different than the last time we
            //  checked, then we know that the COM ports have changed and we
            //  don't need to verify each entry.
            // if (IO_Serial_lstCOMPorts.Items.Count == SerialPort.GetPortNames().Length)
            if (comboBox_PortsSP3.Items.Count == SerialPort.GetPortNames().Length)
            {
                try
                {
                    foreach (string s in SerialPort.GetPortNames())
                    {
                        if (comboBox_PortsSP3.Items[i++].Equals(s) == false)
                        {
                            foundDifference = true;

                        }
                    }
                }
                catch { }
            }
            else foundDifference = true;
            if (foundDifference == false) return;
            //If something has changed, then clear the list
            comboBox_PortsSP3.Items.Clear();
            String PortPosition = "Bulunamadi";
            //Add all of the current COM ports to the list
            foreach (string s in SerialPort.GetPortNames())
            {
                comboBox_PortsSP3.Items.Add(s);
                if (s == Okyanus.SeriPort3.PortConnectName) PortPosition = Okyanus.SeriPort3.PortConnectName;
                comboBox_PortsSP3.SelectedIndex = 0; // 24.04.2014
            }
            if (PortPosition == "Bulunamadi")
            {

                SP3_DisConnect_Procedure();
            }
            textBox__ConnectionSP3.Text = PortPosition;
            //Set the listbox to point to the first entry in the list
            //  SP1_IO_Serial_lstCOMPorts.SelectedIndex = 0; // 24.04.2014
        }
        void SP3_Connect_Procedure()
        {
            try
            {
                //Get the port name from the application list box.
                //  the PortName attribute is a string name of the COM
                //  port (e.g. - "COM1").
                SP3_serialPort.PortName = comboBox_PortsSP3.Items[comboBox_PortsSP3.SelectedIndex].ToString();
                Okyanus.SeriPort3.PortConnectName = SP3_serialPort.PortName;
                textBox__ConnectionSP3.Text = Okyanus.SeriPort3.PortConnectName;
                //Open the COM port.
                SP3_serialPort.Open();
                
                //Change the state of the application objects
                //   btnConnect.Enabled = false;
                button_ConnectSP3.Enabled = false;
                comboBox_PortsSP3.Enabled = false;
                //   btnClose.Enabled = true;
                button_DisconnectSP3.Enabled = true;
                //            textBox1.Clear(); 
                //            textBox1.AppendText("Connected.\r\n");

            }
            catch
            {
                //If there was an exception, then close the handle to 
                //  the device and assume that the device was removed
                //  button_Close_Click(this, null);

                SP3_DisConnect_Procedure();
                Okyanus.SeriPort3.PortConnectName = "BaglantiYok!";
            }

        }
        void SP3_DisConnect_Procedure()
        {
            //Reset the state of the application objects
            Okyanus.SeriPort3.PortConnectName = "BaglantiYok!";
            textBox__ConnectionSP3.Text = Okyanus.SeriPort3.PortConnectName;
            button_DisconnectSP3.Enabled = false;
            button_ConnectSP3.Enabled = true;
            comboBox_PortsSP3.Enabled = true;

            //This section of code will try to close the COM port.
            //  Please note that it is important to use a try/catch
            //  statement when closing the COM port.  If a USB virtual
            //  COM port is removed and the PC software tries to close
            //  the COM port before it detects its removal then
            //  an exeception is thrown.  If the execption is not in a
            //  try/catch statement this could result in the application
            //  crashing.
            try
            {
                //Dispose the In and Out buffers;
                SP3_serialPort.DiscardInBuffer();
                SP3_serialPort.DiscardOutBuffer();

                //Close the COM port
                SP3_serialPort.Close();
            }
            //If there was an exeception then there isn't much we can
            //  do.  The port is no longer available.
            catch { }
        }

        private void button_ConnectSP3_Click(object sender, EventArgs e)
        {
            SP3_Connect_Procedure();
        }

        private void button_DisconnectSP3_Click(object sender, EventArgs e)
        {
            SP3_DisConnect_Procedure();
        }

        private void SP1_ConnectButton_Click(object sender, EventArgs e)
        {
            SP1_Connect_Procedure();
        }
        private void SP1_DisconnectButton_Click(object sender, EventArgs e)
        {
            SP1_DisConnect_Procedure();
        }
/*
        private void SP1_SendButton_Click(object sender, EventArgs e)
        {
            if (Okyanus.Variables.WriteFlashEnable == true)
            {
                MessageBox.Show("Command Send Active" + nl + "First Disable Commands");
                return;
            }
            SP1_SendData_Procedure();
        }
*/
        private void SP1_serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SP1_ReceiveData_Procedure();
        }
        private void SP2_ConnectButton_Click(object sender, EventArgs e)
        {
            SP2_Connect_Procedure();
        }
        private void SP2_DisConnectButton_Click(object sender, EventArgs e)
        {
            SP2_DisConnect_Procedure();
        }
        private void SP2_SendButton_Click(object sender, EventArgs e)
        {
            Preamble_Test = 0XAA;
            SP2_SendData_Procedure();
        }
        private void SP2_serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SP2_ReceiveData_Procedure();
        }
/*
        private void button1_Click(object sender, EventArgs e)
        {
      //      SP1_richTextBox.Clear();
            SP1_DatatextBox.Clear();
        }
*/
        private void SP2_buttondataboz_Click(object sender, EventArgs e)
        {
            Preamble_Test = 0X44;
            SP2_SendData_Procedure();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            CRC_Test = 32;
            Preamble_Test = 0XAA;
            SP2_SendData_Procedure();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SP2_richTextBox.Clear();
            SP2_DatatextBox.Clear();
        }
        public void CHART_UpdateEnabledGraph()
        { 
            /*
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
            */
        }
        public void CHART_Plot_Main()
        {
            //    Grid grid = new Grid();
            //    grid.
            //     if (Okyanus.Variables.RawIndex == 0) return;

            //  return;
            if (Okyanus.Chart.ClearChart == true)
            {
               
                plotSurface2D1.Clear();
                Okyanus.Chart.Time_Minindexer = 0;
                Okyanus.Chart.Time_Maxindexer = 0;
                Okyanus.Chart.XTimes.Clear();
                Okyanus.Chart.ClearChart = false;
                return;
            }
       //     CHART_UpdateEnabledGraph();
            if (Okyanus.Variables.Log_Status == false) return;
            if (Okyanus.Chart.PlayDelay != 0) return;
            if (Okyanus.Chart.Play == false) return;

            NPlot.Grid grid = new NPlot.Grid();
            plotSurface2D1.Clear();
            grid.VerticalGridType = NPlot.Grid.GridType.Coarse;
            grid.HorizontalGridType = NPlot.Grid.GridType.Coarse;
            grid.MajorGridPen = new Pen(Color.LightGray, 1.0f);
            grid.MinorGridPen = new Pen(Color.LightGray, 1.0f);
            plotSurface2D1.Add(grid);


            NPlot.LinePlot Graph0 = new NPlot.LinePlot();
            Graph0.Pen = new Pen(Okyanus.Chart.Renk0, 1);
            List<String> Data0 = new List<String>();

            NPlot.LinePlot Graph1 = new NPlot.LinePlot();
            Graph1.Pen = new Pen(Okyanus.Chart.Renk1, 1);
            List<String> Data1 = new List<String>();

            NPlot.LinePlot Graph2 = new NPlot.LinePlot();
            Graph2.Pen = new Pen(Okyanus.Chart.Renk2, 1);
            List<String> Data2 = new List<String>();

            NPlot.LinePlot Graph3 = new NPlot.LinePlot();
            Graph3.Pen = new Pen(Okyanus.Chart.Renk3, 1);
            List<String> Data3 = new List<String>();

            NPlot.LinePlot Graph4 = new NPlot.LinePlot();
            Graph4.Pen = new Pen(Okyanus.Chart.Renk4, 1);
            List<String> Data4 = new List<String>();

            NPlot.LinePlot Graph5 = new NPlot.LinePlot();
            Graph5.Pen = new Pen(Okyanus.Chart.Renk5, 1);
            List<String> Data5 = new List<String>();

            NPlot.LinePlot Graph6 = new NPlot.LinePlot();
            Graph6.Pen = new Pen(Okyanus.Chart.Renk6, 1);
            List<String> Data6 = new List<String>();

            NPlot.LinePlot Graph7 = new NPlot.LinePlot();
            Graph7.Pen = new Pen(Okyanus.Chart.Renk7, 1);
            List<String> Data7 = new List<String>();

            NPlot.LinePlot Graph8 = new NPlot.LinePlot();
            Graph8.Pen = new Pen(Okyanus.Chart.Renk8, 1);
            List<String> Data8 = new List<String>();

            NPlot.LinePlot Graph9 = new NPlot.LinePlot();
            Graph9.Pen = new Pen(Okyanus.Chart.Renk9, 1);
            List<String> Data9 = new List<String>();

            NPlot.LinePlot Graph10 = new NPlot.LinePlot();
            Graph10.Pen = new Pen(Okyanus.Chart.Renk10, 1);
            List<String> Data10 = new List<String>();

            NPlot.LinePlot Graph11 = new NPlot.LinePlot();
            Graph11.Pen = new Pen(Okyanus.Chart.Renk11, 1);
            List<String> Data11 = new List<String>();

            NPlot.LinePlot Graph12 = new NPlot.LinePlot();
            Graph12.Pen = new Pen(Okyanus.Chart.Renk12, 1);
            List<String> Data12 = new List<String>();

            NPlot.LinePlot Graph13 = new NPlot.LinePlot();
            Graph13.Pen = new Pen(Okyanus.Chart.Renk13, 1);
            List<String> Data13 = new List<String>();

            NPlot.LinePlot Graph14 = new NPlot.LinePlot();
            Graph14.Pen = new Pen(Okyanus.Chart.Renk14, 1);
            List<String> Data14 = new List<String>();

            NPlot.LinePlot Graph15 = new NPlot.LinePlot();
            Graph15.Pen = new Pen(Okyanus.Chart.Renk15, 1);
            List<String> Data15 = new List<String>();

            NPlot.LinePlot Graph16 = new NPlot.LinePlot();
            Graph16.Pen = new Pen(Okyanus.Chart.Renk16, 1);
            List<String> Data16 = new List<String>();

            NPlot.LinePlot Graph17 = new NPlot.LinePlot();
            Graph17.Pen = new Pen(Okyanus.Chart.Renk17, 1);
            List<String> Data17 = new List<String>();

            NPlot.LinePlot Graph18 = new NPlot.LinePlot();
            Graph18.Pen = new Pen(Okyanus.Chart.Renk18, 1);
            List<String> Data18 = new List<String>();

            NPlot.LinePlot Graph19 = new NPlot.LinePlot();
            Graph19.Pen = new Pen(Okyanus.Chart.Renk19, 1);
            List<String> Data19 = new List<String>();

            NPlot.LinePlot Graph20 = new NPlot.LinePlot();
            Graph20.Pen = new Pen(Okyanus.Chart.Renk20, 1);
            List<String> Data20 = new List<String>();

            NPlot.LinePlot Graph21 = new NPlot.LinePlot();
            Graph21.Pen = new Pen(Okyanus.Chart.Renk21, 1);
            List<String> Data21 = new List<String>();

            NPlot.LinePlot Graph22 = new NPlot.LinePlot();
            Graph22.Pen = new Pen(Okyanus.Chart.Renk22, 1);
            List<String> Data22 = new List<String>();

            NPlot.LinePlot Graph23 = new NPlot.LinePlot();
            Graph23.Pen = new Pen(Okyanus.Chart.Renk23, 1);
            List<String> Data23 = new List<String>();

            NPlot.LinePlot Graph24 = new NPlot.LinePlot();
            Graph24.Pen = new Pen(Okyanus.Chart.Renk24, 2);
            List<String> Data24 = new List<String>();

            NPlot.LinePlot GraphScale = new NPlot.LinePlot();
            GraphScale.Pen = new Pen(Color.White, 1);
            List<String> DataScale = new List<String>();
 
            Okyanus.Chart.Time_Maxindexer++;
            if (Okyanus.Chart.Time_Maxindexer > Okyanus.Chart.MAXTIME)  Okyanus.Chart.Time_Minindexer++;
               
            int index = Okyanus.Chart.Time_Maxindexer - 1;

            plotSurface2D1.Clear();
            int parseData;

            DateTime d1 = DateTime.Now;
           
            Okyanus.Chart.XTimes.Add(d1.AddDays(0).AddSeconds(0)).ToString();

            if (Okyanus.Chart.XTimes.Count > Okyanus.Chart.MAXTIME) Okyanus.Chart.XTimes.RemoveAt(0);       

            for (int j = 0; j < Okyanus.Variables.RawIndex; j++)
            {
                switch (j)
                {
                    case 0: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        if (Okyanus.Chart.TES1307_Enabled == true) Okyanus.Chart.TES1307_01_DataArray[index] = (Okyanus.Chart.TES1307_01_Raw/10).ToString();
                        else                                                 Okyanus.Chart.DataArray0[index] = Okyanus.Variables.Variables_Data[j];                       
                        break;
                    case 1: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        if (Okyanus.Chart.TES1307_Enabled == true) Okyanus.Chart.TES1307_02_DataArray[index] = (Okyanus.Chart.TES1307_02_Raw / 10).ToString();
                        else                                                 Okyanus.Chart.DataArray1[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 2: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray2[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 3: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray3[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 4: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray4[index] = Okyanus.Variables.Variables_Data[j];     
                        break;
                    case 5: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray5[index] = Okyanus.Variables.Variables_Data[j];    
                        break;
                    case 6: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray6[index] = Okyanus.Variables.Variables_Data[j];                    
                        break;
                    case 7: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray7[index] = Okyanus.Variables.Variables_Data[j];                       
                        break;
                    case 8: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray8[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 9: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray9[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 10: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray10[index] = Okyanus.Variables.Variables_Data[j]; 
                        break;
                    case 11: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray11[index] = Okyanus.Variables.Variables_Data[j];               
                        break;
                    case 12: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray12[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 13: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray13[index] = Okyanus.Variables.Variables_Data[j];                    
                        break;
                    case 14: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray14[index] = Okyanus.Variables.Variables_Data[j];                   
                        break;
                    case 15: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray15[index] = Okyanus.Variables.Variables_Data[j];              
                        break;
                    case 16: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray16[index] = Okyanus.Variables.Variables_Data[j];                      
                        break;
                    case 17: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray17[index] = Okyanus.Variables.Variables_Data[j];                    
                        break;
                    case 18: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray18[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 19: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray19[index] = Okyanus.Variables.Variables_Data[j];              
                        break;
                    case 20: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray20[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 21: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray21[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 22: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray22[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 23: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray23[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    case 24: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Okyanus.Chart.DataArray24[index] = Okyanus.Variables.Variables_Data[j];
                        break;
                    default: break;
                }
            }

            for (int i = Okyanus.Chart.Time_Minindexer; i < Okyanus.Chart.Time_Maxindexer; i++)
            {
                for (int j = 0; j < Okyanus.Variables.RawIndex; j++)
                {                    
                    switch (j)
                    {
                        case 0: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;

                            if (Okyanus.Chart.TES1307_Enabled == true) Int32.TryParse(Okyanus.Chart.TES1307_01_DataArray[i], out parseData);
                            else                                        Int32.TryParse(Okyanus.Chart.DataArray0[i], out parseData);
                            Data0.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 1: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            if (Okyanus.Chart.TES1307_Enabled == true) Int32.TryParse(Okyanus.Chart.TES1307_02_DataArray[i], out parseData);
                            else                                        Int32.TryParse(Okyanus.Chart.DataArray1[i], out parseData);
                            Data1.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString()); 
                            break;
                        case 2: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray2[i], out parseData);
                            Data2.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 3: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray3[i], out parseData);
                            Data3.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());          
                            break;
                        case 4: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray4[i], out parseData);
                            Data4.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 5: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray5[i], out parseData);
                            Data5.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());          
                            break;
                        case 6: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray6[i], out parseData);
                            Data6.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());             
                            break;
                        case 7: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray7[i], out parseData);
                            Data7.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());        
                            break;
                        case 8: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray8[i], out parseData);
                            Data8.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 9: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray9[i], out parseData);
                            Data9.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());     
                            break;
                        case 10: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray10[i], out parseData);
                            Data10.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 11: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray11[i], out parseData);
                            Data11.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 12: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray12[i], out parseData);
                            Data12.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 13: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray13[i], out parseData);
                            Data13.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 14: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray4[i], out parseData);
                            Data14.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 15: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray15[i], out parseData);
                            Data15.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 16: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray16[i], out parseData);
                            Data16.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 17: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray17[i], out parseData);
                            Data17.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 18: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray18[i], out parseData);
                            Data18.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 19: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray19[i], out parseData);
                            Data19.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 20: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray20[i], out parseData);
                            Data20.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 21: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray21[i], out parseData);
                            Data21.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 22: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray22[i], out parseData);
                            Data22.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 23: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray23[i], out parseData);
                            Data23.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        case 24: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                            Int32.TryParse(Okyanus.Chart.DataArray24[i], out parseData);
                            Data24.Add(CHART_Calculate_Multiply_Value(parseData, j).ToString());
                            break;
                        default: break;
                    }
                }
            }
            for (int j = (Okyanus.Variables.RawIndex-1); j > -1 ; j--)
            {
                switch (j)
                {
                    case 0: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph0.AbscissaData = Okyanus.Chart.XTimes;
                        Graph0.DataSource = Data0;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph0);
                        break;
                    case 1: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph1.AbscissaData = Okyanus.Chart.XTimes;
                        Graph1.DataSource = Data1;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph1);
                        break;
                    case 2: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph2.AbscissaData = Okyanus.Chart.XTimes;
                        Graph2.DataSource = Data2;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph2);
                        break;
                    case 3: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph3.AbscissaData = Okyanus.Chart.XTimes;
                        Graph3.DataSource = Data3;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph3);
                        break;
                    case 4: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph4.AbscissaData = Okyanus.Chart.XTimes;
                        Graph4.DataSource = Data4;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph4);
                        break;
                    case 5: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph5.AbscissaData = Okyanus.Chart.XTimes;
                        Graph5.DataSource = Data5;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph5);
                        break;
                    case 6: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph6.AbscissaData = Okyanus.Chart.XTimes;
                        Graph6.DataSource = Data6;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph6);
                        break;
                    case 7: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph7.AbscissaData = Okyanus.Chart.XTimes;
                        Graph7.DataSource = Data7;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph7);
                        break;
                    case 8: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph8.AbscissaData = Okyanus.Chart.XTimes;
                        Graph8.DataSource = Data8;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph8);
                        break;
                    case 9: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph9.AbscissaData = Okyanus.Chart.XTimes;
                        Graph9.DataSource = Data9;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph9);
                        break;
                    case 10: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph10.AbscissaData = Okyanus.Chart.XTimes;
                        Graph10.DataSource = Data10;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph10);
                        break;
                    case 11: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph11.AbscissaData = Okyanus.Chart.XTimes;
                        Graph11.DataSource = Data11;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph11);
                        break;
                    case 12: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph12.AbscissaData = Okyanus.Chart.XTimes;
                        Graph12.DataSource = Data12;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph12);
                        break;
                    case 13: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph13.AbscissaData = Okyanus.Chart.XTimes;
                        Graph13.DataSource = Data13;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph13);
                        break;
                    case 14: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph14.AbscissaData = Okyanus.Chart.XTimes;
                        Graph14.DataSource = Data14;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph14);                        
                        break;
                    case 15: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph15.AbscissaData = Okyanus.Chart.XTimes;
                        Graph15.DataSource = Data15;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph15);
                        break;
                    case 16: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph16.AbscissaData = Okyanus.Chart.XTimes;
                        Graph16.DataSource = Data16;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph16);
                        break;
                    case 17: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph17.AbscissaData = Okyanus.Chart.XTimes;
                        Graph17.DataSource = Data17;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph17);
                        break;
                    case 18: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph18.AbscissaData = Okyanus.Chart.XTimes;
                        Graph18.DataSource = Data18;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph18);
                        break;
                    case 19: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph19.AbscissaData = Okyanus.Chart.XTimes;
                        Graph19.DataSource = Data19;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph19);
                        break;
                    case 20: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph20.AbscissaData = Okyanus.Chart.XTimes;
                        Graph20.DataSource = Data20;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph20);
                        break;
                    case 21: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph21.AbscissaData = Okyanus.Chart.XTimes;
                        Graph21.DataSource = Data21;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph21);
                        break;
                    case 22: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph22.AbscissaData = Okyanus.Chart.XTimes;
                        Graph22.DataSource = Data22;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph22);
                        break;
                    case 23: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph23.AbscissaData = Okyanus.Chart.XTimes;
                        Graph23.DataSource = Data23;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph23);              
                        break;
                    case 24: if (Okyanus.Variables.Var_PlotOnOff[j] == 1) break;
                        Graph24.AbscissaData = Okyanus.Chart.XTimes;
                        Graph24.DataSource = Data24;
                        if (Okyanus.Chart.GraphEnable[j] == true) plotSurface2D1.Add(Graph24);

                        break;
                    default: break;
                }
            }

            try
            {
                plotSurface2D1.ShowCoordinates = true;
                plotSurface2D1.YAxis1.Label = " ";
                plotSurface2D1.YAxis1.LabelOffsetAbsolute = true;
                plotSurface2D1.YAxis1.LabelOffset = 0;
                plotSurface2D1.YAxis1.HideTickText = false;
                plotSurface2D1.YAxis1.NumberFormat = "{0:#0}";

                 //       plotSurface2D1.XAxis1.NumberFormat = "yyyy-MM-dd HH:mm:ss"; // bu ok
                        plotSurface2D1.XAxis1.NumberFormat = "HH:mm:ss"; // bu ok
                 //       plotSurface2D1.XAxis1.NumberFormat = "yyyy-MM-dd HH:mm";
                plotSurface2D1.XAxis1.TicksLabelAngle = 90;
           //     plotSurface2D1.XAxis1.TickTextNextToAxis = true;
                plotSurface2D1.XAxis1.FlipTicksLabel = true;

        //        plotSurface2D1.XAxis1.AutoScaleTicks = true;
        //        plotSurface2D1.XAxis1.AutoScaleText = true;
                plotSurface2D1.XAxis1.HideTickText = false;
                plotSurface2D1.Padding = 20;  //5 ti
       //         plotSurface2D1.AutoScaleTitle = true;
                //  plotSurface2D1.BackColor = 
                plotSurface2D1.Refresh();
             //   plotSurface2D1.Bitmap
             //   this.plotSurface2D1
           // Response.Buffer = true;
       //         plotSurface2D1.Print(1);

/*
                PictureBox pbox1 = new PictureBox();
                pbox1.Width = 1300;
                pbox1.Height = 600;

               NPlot.Bitmap.PlotSurface2D  bmpPlotSurface2D = new NPlot.Bitmap.PlotSurface2D(pbox1.Width, pbox1.Height);
         //       NPlot.Bitmap.PlotSurface2D bmpPlotSurface2D = new NPlot.Bitmap.PlotSurface2D(1300, 600);
            //    bmpPlotSurface2D.c
                NPlot.PointPlot pointPlot1 = new NPlot.PointPlot();

                bmpPlotSurface2D.BackColor = Color.White;
                bmpPlotSurface2D.Add(pointPlot1);
                bmpPlotSurface2D.Refresh();
                pbox1.Image = bmpPlotSurface2D.Bitmap;
                bmpPlotSurface2D.Bitmap.Save("D:\\test.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

*/
                
            }
            catch { };
        }
        public Int32 CHART_Calculate_Multiply_Value(Int32 Parsed_Value, int index)
        {
            switch (Okyanus.Variables.Var_PlotMult[index])
            {
                case Okyanus.Definitions.PlotMul0001: Parsed_Value /= 1000; break;
                case Okyanus.Definitions.PlotMul001: Parsed_Value /= 100; break;
                case Okyanus.Definitions.PlotMul01: Parsed_Value /= 10; break;
                default:
                case Okyanus.Definitions.PlotMul1: Parsed_Value *= 1; break;
                case Okyanus.Definitions.PlotMul10: Parsed_Value *= 10; break;
                case Okyanus.Definitions.PlotMul100: Parsed_Value *= 100; break;
                case Okyanus.Definitions.PlotMul1000: Parsed_Value *= 1000; break;
            }
            if (Parsed_Value > Okyanus.Variables.Var_PlotMax[index]) Parsed_Value = Okyanus.Variables.Var_PlotMax[index];
            if (Parsed_Value < Okyanus.Variables.Var_PlotMin[index]) Parsed_Value = Okyanus.Variables.Var_PlotMin[index];
            return Parsed_Value;
        }

        public void PANEL_TES1307()
        {
            panel_TES1307.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
            panel_TES1307.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel, Okyanus.Variables.Height_Panel);

            panel_TES1307.Visible = true;
            panel_Communication.Visible = false;
            panel_Log.Visible = false; panel_Colour.Visible = false;
            panel_Configuration.Visible = false;
            panel_CommandSetup.Visible = false;
            panel_Simulasyon.Visible = false;
            panel_500CommandSetup.Visible = false;

            panel_Flash_Combobox.Visible = false;
            panel_Flash_Numeric.Visible = false;
            panel_KM2_BITS.Visible = false;
        }
        public void PANEL_Communication()
        {
            panel_Communication.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
            panel_Communication.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel, Okyanus.Variables.Height_Panel);

            panel_Communication.Visible = true;
            panel_Log.Visible = false; panel_Colour.Visible = false;
            panel_Configuration.Visible = false;
            panel_CommandSetup.Visible = false;
            panel_Simulasyon.Visible = false;
            panel_500CommandSetup.Visible = false;
            panel_TES1307.Visible = false;
            panel_Flash_Combobox.Visible = false;
            panel_Flash_Numeric.Visible = false;
            panel_KM2_BITS.Visible = false;
        }
        public void SimulatorMonitorBJU()
        {
            SimulatorClearTextBox_Labels();
            label_thk.Visible = true;
            textBox_Sim_thk.Visible = true;
            label_tk.Visible = true;
            textBox_Sim_tk.Visible = true;
            label_tv1.Visible = true;
            textBox_Sim_tv1.Visible = true;
            label_tvki.Visible = true;
            textBox_Sim_tvki.Visible = true;
            label_tva.Visible = true;
            textBox_Sim_tva.Visible = true;
            label_mvt.Visible = true;
            textBox_Sim_mvt.Visible = true;
            label_thu.Visible = true;
            textBox_Sim_thu.Visible = true;
            label_tu.Visible = true;
            textBox_Sim_tu.Visible = true;
            label_thk.Location = new System.Drawing.Point(580, 200);
            textBox_Sim_thk.Location = new System.Drawing.Point(620, 200);
            label_tk.Location = new System.Drawing.Point(590, 250);
            textBox_Sim_tk.Location = new System.Drawing.Point(620, 250);
            label_tv1.Location = new System.Drawing.Point(830, 220);
            textBox_Sim_tv1.Location = new System.Drawing.Point(870, 220);
            label_tvki.Location = new System.Drawing.Point(830, 270);
            textBox_Sim_tvki.Location = new System.Drawing.Point(870, 270);
            label_tva.Location = new System.Drawing.Point(830, 320);
            textBox_Sim_tva.Location = new System.Drawing.Point(870, 320);
            label_mvt.Location = new System.Drawing.Point(1010, 280);
            textBox_Sim_mvt.Location = new System.Drawing.Point(1050, 280);
            label_thu.Location = new System.Drawing.Point(1240, 150);
            textBox_Sim_thu.Location = new System.Drawing.Point(1280, 150);
            label_tu.Location = new System.Drawing.Point(1250, 200);
            textBox_Sim_tu.Location = new System.Drawing.Point(1280, 200);
        }



        public void SimulatorMonitorTost()
        {
            SimulatorClearTextBox_Labels();

            

            textBoxTost1.Visible = true;
            textBoxTost2.Visible = true;
            textBoxTost3.Visible = true;
            textBoxTost4.Visible = true;
            textBoxTost5.Visible = true;
            textBoxTost5.Location = new System.Drawing.Point(600, 570); // 582 122 alt
            textBoxTost6.Visible = true;
            textBoxTost6.Location = new System.Drawing.Point(600, 180); // 582 122 ust
            textBoxTost7.Visible = true;
            textBoxTost8.Visible = true;
            textBoxTost9.Visible = true;
            textBoxTost10.Visible = true;
            textBoxTost11.Visible = true;

            int NoktaX = 60;
            int NoktaY = 200;
            textBoxTost1.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40;// 582 122 ust 
            textBoxTost2.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost3.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40;// 582 122 ust
            textBoxTost4.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost7.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost8.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40;// 582 122 ust
            textBoxTost9.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost10.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost11.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40;// 582 122 ust

            int SizeX = 200;
            int SizeY = 20;


            textBoxTost1.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost2.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost3.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost4.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost5.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost6.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost7.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost8.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost9.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost10.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost11.Size = new System.Drawing.Size(SizeX, SizeY);

        }



        public void SimulatorMonitorKahvemakinasi1()
        {
            SimulatorClearTextBox_Labels();

        }
        public void SimulatorMonitorKahvemakinasi2()
        {
            SimulatorClearTextBox_Labels();

            panel_KM2_BITS.Location  = new System.Drawing.Point(410, 324); // 582 122 alt
            panel_KM2_BITS.Visible = true;

      //      button_KM2_01.Visible = true;
      //      button_KM2_02.Visible = true;

            textBoxTost1.Visible = true;
            textBoxTost2.Visible = true;
            textBoxTost3.Visible = true;
            textBoxTost4.Visible = true;
            textBoxTost5.Visible = true;
     //       textBoxTost5.Location = new System.Drawing.Point(600, 570); // 582 122 alt
            textBoxTost6.Visible = true;
     //       textBoxTost6.Location = new System.Drawing.Point(600, 180); // 582 122 ust
            textBoxTost7.Visible = true;
            textBoxTost8.Visible = true;
            textBoxTost9.Visible = true;
            textBoxTost10.Visible = true;
            textBoxTost11.Visible = true;

            int NoktaX = 60;
            int NoktaY = 200;
            textBoxTost1.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40;// 582 122 ust 
            textBoxTost2.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost3.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40;// 582 122 ust
            textBoxTost4.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost5.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost6.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost7.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost8.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40;// 582 122 ust
            textBoxTost9.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost10.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40; // 582 122 ust
            textBoxTost11.Location = new System.Drawing.Point(NoktaX, NoktaY); NoktaY += 40;// 582 122 ust

            int SizeX = 200;
            int SizeY = 20;


            textBoxTost1.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost2.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost3.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost4.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost5.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost6.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost7.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost8.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost9.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost10.Size = new System.Drawing.Size(SizeX, SizeY);
            textBoxTost11.Size = new System.Drawing.Size(SizeX, SizeY);

        }
        public void SimulatorClearTextBox_Labels()
        {

            panel_KM2_BITS.Visible = false;
            
            //      button_KM2_01.Visible = false;
            //     button_KM2_02.Visible = false;



            label_thk.Visible = false;
            textBox_Sim_thk.Visible = false;
            label_tk.Visible = false;
            textBox_Sim_tk.Visible = false;
            label_tv1.Visible = false;
            textBox_Sim_tv1.Visible = false;
            label_tvki.Visible = false;
            textBox_Sim_tvki.Visible = false;
            label_tva.Visible = false;
            textBox_Sim_tva.Visible = false;
            label_mvt.Visible = false;
            textBox_Sim_mvt.Visible = false;
            label_thu.Visible = false;
            textBox_Sim_thu.Visible = false;
            label_tu.Visible = false;
            textBox_Sim_tu.Visible = false;



            textBoxTost1.Visible = false;
            textBoxTost2.Visible = false;
            textBoxTost3.Visible = false;
            textBoxTost4.Visible = false;
            textBoxTost5.Visible = false;
            textBoxTost6.Visible = false;
            textBoxTost7.Visible = false;
            textBoxTost8.Visible = false;
            textBoxTost9.Visible = false;
            textBoxTost10.Visible = false;
            textBoxTost11.Visible = false;

        }
        public void PANEL_Simulasyon_Ekrani()
        {

      //      static	String ^ Core_Directory = Directory::GetCurrentDirectory() + "\\Core_Source\\";
            string pic = Directory.GetCurrentDirectory() + "\\Pictures\\";
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    //this.Text = "BJU Emulator Data Logger";

                    SimulatorMonitorBJU();
                    pic += "Simulasyon_Ekran_BJU.jpg";
                    break;
                case Okyanus.Definitions.BJUEmulator: //this.Text = "BJU Emulator V";

                    SimulatorMonitorBJU();
                     pic += "Simulasyon_Ekran_BJU.jpg";
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:
                    // this.Text = "Kahve Makinesi Emulator 2";
                    SimulatorMonitorKahvemakinasi1();
                    pic += "Simulasyon_Ekran_Kahve.jpg";
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2:
                   // this.Text = "Kahve Makinesi Emulator 2";
                    SimulatorMonitorKahvemakinasi2();
                     pic += "Simulasyon_Ekran_Kahve.jpg";
                    break;

                case Okyanus.Definitions.TostMakinasiEmulatoru2:
                  //  this.Text = "Tost Makinesi Emulator 2";
                    SimulatorMonitorTost();
                     pic += "Simulasyon_Ekran_Tost.jpg";
                    break;
                default:
                    this.Text = "Model secilmedi !!!";
                    break;

            }
            if ((File.Exists(pic))) pictureBox1.Image = System.Drawing.Image.FromFile(pic);
            else MessageBox.Show("Simulasyon Resmi yok");
            panel_Simulasyon.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
            panel_Simulasyon.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel, Okyanus.Variables.Height_Panel);

            pictureBox1.Location = new System.Drawing.Point(Okyanus.Variables.X_Point+10, Okyanus.Variables.Y_Point);
            pictureBox1.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel-10, Okyanus.Variables.Height_Panel-30);


     //       textBox_Sim_tu.Location = new System.Drawing.Point(750, 100);
      //      string pic = "Simulasyon_Ekran.jpg";
    //        if ((File.Exists(pic))) pictureBox1.Image = System.Drawing.Image.FromFile(pic);
   //         else MessageBox.Show("Simulasyon Resmi yok");

            panel_Simulasyon.Visible = true;
            panel_Colour.Visible = false;
            panel_Log.Visible = false;
            panel_Configuration.Visible = false;
            panel_Communication.Visible = false;
            panel_TES1307.Visible = false;
            panel_CommandSetup.Visible = false;
            panel_500CommandSetup.Visible = false;
            panel_Flash_Combobox.Visible = false;
            panel_Flash_Numeric.Visible = false;  
                
        }

        public void PANEL_Chart()
        {
            panel_Log.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
            panel_Log.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel, Okyanus.Variables.Height_Panel);

            //       plotSurface2D1.Location = new System.Drawing.Point(Okyanus.Variables.X_Point+10, Okyanus.Variables.Y_Point+10);
            //       plotSurface2D1.Size = new System.Drawing.Size(Okyanus.Variables.Width-20, Okyanus.Variables.Height-20);
            plotSurface2D1.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
     //       plotSurface2D1.Size = new System.Drawing.Size(Okyanus.Variables.Width - 50, Okyanus.Variables.Height - 40); // 70 40
            plotSurface2D1.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel - 20, Okyanus.Variables.Height_Panel - 25); // 70 40
            panel_Log.Visible = true;

/*
            panel_Colour.Location = new System.Drawing.Point(Okyanus.Variables.Width - 45, Okyanus.Variables.Y_Point); // 45
            panel_Colour.Size = new System.Drawing.Size(150, Okyanus.Variables.Height);
            panel_Colour.Visible = true;
            panel_Colour.BringToFront();
*/
            panel_Colour.Visible = false;

            panel_Configuration.Visible = false;
            panel_Communication.Visible = false;
            panel_TES1307.Visible = false;
            panel_CommandSetup.Visible = false;
            panel_Simulasyon.Visible = false;
            panel_500CommandSetup.Visible = false;
            panel_Flash_Combobox.Visible = false;
            panel_Flash_Numeric.Visible = false;
            panel_KM2_BITS.Visible = false;
        }
        public void PANEL_RamConfig()
        {
            panel_Configuration.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
            panel_Configuration.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel, Okyanus.Variables.Height_Panel);

            panel_Configuration.Visible = true;
            panel_Communication.Visible = false;
            panel_TES1307.Visible = false;
            panel_Log.Visible = false; panel_Colour.Visible = false;
            panel_CommandSetup.Visible = false;
            panel_Simulasyon.Visible = false;
            panel_500CommandSetup.Visible = false;
            panel_Flash_Combobox.Visible = false;
            panel_Flash_Numeric.Visible = false;
            panel_KM2_BITS.Visible = false;
        }
        public void PANEL_FlashCommandConfig()
        {
            panel_CommandSetup.Location = new System.Drawing.Point(100, 370);
            panel_CommandSetup.Size = new System.Drawing.Size(1150, 350);

            panel_Flash_Combobox.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
            panel_Flash_Combobox.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel, Okyanus.Variables.Height_Panel);

            panel_Flash_Numeric.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
            panel_Flash_Numeric.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel, Okyanus.Variables.Height_Panel);


            panel_CommandSetup.Visible = true;  

            panel_Configuration.Visible = false;
            panel_Communication.Visible = false;
            panel_TES1307.Visible = false;
            panel_Log.Visible = false; panel_Colour.Visible = false;
            panel_Simulasyon.Visible = false;
            panel_500CommandSetup.Visible = false;
            panel_KM2_BITS.Visible = false;

            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    panel_Flash_Numeric.Visible = false;
                    panel_Flash_Combobox.Visible = false;
                    break;
                case Okyanus.Definitions.BJUEmulator: //this.Text = "BJU Emulator V";
                    panel_Flash_Numeric.Visible = true;
                    panel_Flash_Combobox.Visible = false;
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:
                    panel_Flash_Numeric.Visible = false;
                    panel_Flash_Combobox.Visible = true;
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2:
                    panel_Flash_Numeric.Visible = false;
                    panel_Flash_Combobox.Visible = true;
                    break;

                case Okyanus.Definitions.TostMakinasiEmulatoru2:
                    panel_Flash_Numeric.Visible = false;
                    panel_Flash_Combobox.Visible = true;
                    break;
                default:
                    
                    break;

            }
  //          panel_Flash_Combobox.Visible = false;
            panel_CommandSetup.Visible = true;      


        }
        public void KONFIGURASYON_TABLO_KAHVE2()
        {

                dt_500Command2.Visible = false;
                dt_500Command.Location = new System.Drawing.Point(650, 0);
                 dt_500Command.Size = new System.Drawing.Size(640, 827);
                 dt_500Command.Visible = true;

            button500byteOku.Location = new System.Drawing.Point(100, 454);
            button500byte_2.Location = new System.Drawing.Point(100, 551);
            button_Flash500_dosyaoku.Location = new System.Drawing.Point(100, 154);
            button_Flash500_dosyaya.Location = new System.Drawing.Point(100, 251);

            textBox_500byte1.Location = new System.Drawing.Point(100,380);
            textBox_500byte1.Size = new System.Drawing.Size(500, 66);
            label_FlashTablosu.Location = new System.Drawing.Point(1320, 250); //50
            label_FlashTablosu.Text = "F" + nl + "l" + nl + "a" + nl + "s" + nl + "h" + nl + " " + nl + "T" + nl + "a" + nl + "b" + nl + "l" + nl + "o" + nl + "s" + nl + "u";
                textBox_500byte1.Visible = true;
                textBox_500byte2.Visible = false;
            //        label_FlashTablosu.Text = "Flash Tablosu";

            textBoxDebugforwrite500.Location = new System.Drawing.Point(300, 54);
            textBoxDebugforwrite500.Size = new System.Drawing.Size(200, 300); // 640, 827


            /*
            button_Flash500_dosyaoku.Enabled = false;
                button_Flash500_dosyaoku.Visible = false;
                button_Flash500_dosyaya.Enabled = false;
                button_Flash500_dosyaya.Visible = false;
            */
           
            dt_500Command.Columns[GridHelper.Variable_Oky100].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command.Columns[GridHelper.Variable_Oky101].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command.Columns[GridHelper.Variable_Oky102].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command.Columns[GridHelper.Variable_Oky103].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command.Columns[GridHelper.Variable_Oky104].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command.Columns[GridHelper.Variable_Oky105].SortMode = DataGridViewColumnSortMode.NotSortable;


        }
        public void KONFIGURASYON_TABLO_TOST2()
        {
            /*                     dt_500Command2.Location = new System.Drawing.Point(103, 10);
                       dt_500Command2.Size = new System.Drawing.Size(640, 780); // 622, 1150
                       dt_500Command2.Visible = true;
                      
                       button_Flash500_dosyaoku.Location = new System.Drawing.Point(884, 154);
                       button_Flash500_dosyaya.Location = new System.Drawing.Point(884, 251);
                       button500byteOku.Location = new System.Drawing.Point(884, 454);
                       button500byte_2.Location = new System.Drawing.Point(884, 551);
                       textBox_500byte2.Location = new System.Drawing.Point(884, 354);

            */
            dt_500Command.Visible = false;
            dt_500Command2.Location = new System.Drawing.Point(650, 0); // 650
            dt_500Command2.Size = new System.Drawing.Size(640, 827); // 640, 827
            dt_500Command2.Visible = true;

            button_Flash500_dosyaoku.Location = new System.Drawing.Point(100, 154);
            button_Flash500_dosyaya.Location = new System.Drawing.Point(100, 251);

            button500byteOku.Location = new System.Drawing.Point(100, 454);
            button500byte_2.Location = new System.Drawing.Point(100, 551);

            textBox_500byte2.Location = new System.Drawing.Point(100, 380);
            textBox_500byte2.Size = new System.Drawing.Size(500, 66);
            textBox_500byte2.Visible = true;

            textBox_500byte1.Visible = false;
           
            textBoxDebugforwrite500.Location = new System.Drawing.Point(300, 54);
            textBoxDebugforwrite500.Size = new System.Drawing.Size(200, 300); // 640, 827

        
            label_FlashTablosu.Location = new System.Drawing.Point(1320, 250); //50
            label_FlashTablosu.Text = "F" + nl + "l" + nl + "a" + nl + "s" + nl + "h" + nl + " " + nl + "T" + nl + "a" + nl + "b" + nl + "l" + nl + "o" + nl + "s" + nl + "u";



            dt_500Command2.Columns[GridHelper.Variable_Oky100].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command2.Columns[GridHelper.Variable_Oky101].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command2.Columns[GridHelper.Variable_Oky102].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command2.Columns[GridHelper.Variable_Oky103].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command2.Columns[GridHelper.Variable_Oky104].SortMode = DataGridViewColumnSortMode.NotSortable;
            dt_500Command2.Columns[GridHelper.Variable_Oky105].SortMode = DataGridViewColumnSortMode.NotSortable;         
        }

        public void PANEL_500FlashCommandConfig()
        {
            panel_500CommandSetup.Location = new System.Drawing.Point(Okyanus.Variables.X_Point, Okyanus.Variables.Y_Point);
            panel_500CommandSetup.Size = new System.Drawing.Size(Okyanus.Variables.Width_Panel, Okyanus.Variables.Height_Panel);

            panel_500CommandSetup.Visible = true;

            panel_CommandSetup.Visible = false;
            panel_Configuration.Visible = false;
            panel_Communication.Visible = false;
            panel_TES1307.Visible = false;
            panel_Log.Visible = false; panel_Colour.Visible = false;
            panel_Simulasyon.Visible = false;
            panel_Flash_Combobox.Visible = false;
            panel_Flash_Numeric.Visible = false;
            panel_KM2_BITS.Visible = false;
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator: //this.Text = "BJU Emulator V";
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:

                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: KONFIGURASYON_TABLO_KAHVE2();//  KONFIGURASYON_TABLO_KAHVE2();
                    break;                 
                case Okyanus.Definitions.TostMakinasiEmulatoru2:KONFIGURASYON_TABLO_TOST2();
                    break;
                default:
                   
                    break;

            }      
        
        
        }

        byte KahveSim;
        public void DATA_Prep_TableSelectDatas_4_Log()/**/
        {
            DATA_LogConfigTable_Read();
            KahveSim = Okyanus.Variables.RamBuffer[0];//Kahve2 Simulasyon ekrani icin data

        //    KahveSim = 0x08;
            //       DataTable_procedure();

            //     textBox1.Text += Okyanus.Variables.Variables_Data[i];
            //  textBox1.Text += Okyanus.Variables.Variables_Name[i];
            //        if (Okyanus.Variables.RawIndex < 0) return;
            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                int AdressIndex = Okyanus.Variables.Var_Adress[i] + 4;

                //    textBox1.Text = Okyanus.Variables.RawIndex.ToString();
                //         Okyanus.Variables.Var_Adress[i] = Okyanus.Variables.AdressArr[i];  // combobox ta tutulan byte bilgisini dizisine at
                // combobox ta tutulan byte bilgisini dizisine at

                if (Okyanus.Variables.Var_Length[i] == 0)
                {// bit okuma

                    String str = "";
                    switch (Okyanus.Variables.Var_Bit[i])
                    {
                        case 0: str = (Okyanus.Variables.RamBuffer[AdressIndex] & 0X01).ToString(); break;
                        case 1: str = (Okyanus.Variables.RamBuffer[AdressIndex] & 0X02).ToString(); break;
                        case 2: str = (Okyanus.Variables.RamBuffer[AdressIndex] & 0X04).ToString(); break;
                        case 3: str = (Okyanus.Variables.RamBuffer[AdressIndex] & 0X08).ToString(); break;
                        case 4: str = (Okyanus.Variables.RamBuffer[AdressIndex] & 0X10).ToString(); break;
                        case 5: str = (Okyanus.Variables.RamBuffer[AdressIndex] & 0X20).ToString(); break;
                        case 6: str = (Okyanus.Variables.RamBuffer[AdressIndex] & 0X40).ToString(); break;
                        case 7: str = (Okyanus.Variables.RamBuffer[AdressIndex] & 0X80).ToString(); break;
                    }
                    Okyanus.Variables.Variables_Data[i] = str;
                    // bit secim               
                }
                if (Okyanus.Variables.Var_Length[i] == 1)
                {//  1 byte
                    if (i == 0)
                    {
                        if (Okyanus.Chart.TES1307_Enabled == true) Okyanus.Variables.Variables_Data[i] = TES1307_01;
                    }
                    else
                    {
                        if (i == 1)
                        {
                            if (Okyanus.Chart.TES1307_Enabled == true) Okyanus.Variables.Variables_Data[i] = TES1307_02;

                        }
                        else Okyanus.Variables.Variables_Data[i] = Okyanus.Variables.RamBuffer[AdressIndex].ToString();
                    }                        
                    
                }
                if (Okyanus.Variables.Var_Length[i] == 2)
                {//  2 byte
                    Okyanus.Variables.Variables_Data[i] = ((Okyanus.Variables.RamBuffer[AdressIndex] << SHIFT8) + Okyanus.Variables.RamBuffer[AdressIndex + 1]).ToString();

                }
                if (Okyanus.Variables.Var_Length[i] == 4)
                {// 4 byte
                    Okyanus.Variables.Variables_Data[i] = ((Okyanus.Variables.RamBuffer[AdressIndex] << SHIFT24) + (Okyanus.Variables.RamBuffer[AdressIndex + 1] << SHIFT16) +
                                                          (Okyanus.Variables.RamBuffer[AdressIndex + 2] << SHIFT8) + Okyanus.Variables.RamBuffer[AdressIndex + 3]).ToString();
                }

            }
            Okyanus.Variables.UpdateVariables = true;
        } // DATA_CommandTable_Read
        public void DATA_LogConfigTable_Read()
        {
            GridHelper gridHelper = new GridHelper(dt);
            DataTable dataElements = gridHelper.GetDataTable();
            List<String> gridValues = gridHelper.GetGridValues();
            //     dt.EditingControl
            //
            //         if (gridValues.Count > 0)
            //        {
            //     Okyanus.Variables.Name_OfInput.Add(gridValues[0]);
            //    Okyanus.Variables.Variables_Name[i] = gridValues[0].ToString();
            for (int i = 0; i < gridValues.Count; i++)
            {
                //      Okyanus.Variables.Variables_NameBuffer[i] = gridValues[i]; // ilk colon icin colon sayisini atlattir
                Okyanus.Variables.Variables_Name[i] = gridValues[i]; // ilk colon icin colon sayisini atlattir
                Okyanus.Variables.Variables_NameIndex = i;
                //  Okyanus.Variables.Variables_Name[i] = Okyanus.Variables.Variables_NameBuffer[i];
            }
        }
        public void DATA_Fill_Grid_From_LogConfigArrays()
        {
            GridHelper gridHelper = new GridHelper(dt);
            DataTable dataElements = gridHelper.GetDataTable();
  /*          List<String> gridValues = gridHelper.GetGridValues(); // 30.05.2014

            for (int i = 0; i < gridValues.Count; i++)
            {
                Okyanus.Variables.Variables_Name[i] = gridValues[i]; // ilk colon icin colon sayisini atlattir
                Okyanus.Variables.Variables_NameIndex = i;
            }      
  */
            DATA_LogConfigTable_Read();

            String str;
            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                DataRow dr = dataElements.NewRow();
                if (i > Okyanus.Variables.Variables_NameIndex) dr[0] = "Degisken" + i.ToString();
                else dr[0] = Okyanus.Variables.Variables_Name[i];

                dr[1] = Okyanus.Variables.Var_Adress[i];
                if (Okyanus.Variables.Var_Length[i] != 0)
                {
                    if (Okyanus.Variables.Var_Sign[i] == 1) str = "+";
                    else str = "+/-";
                }
                else str = " ";
                dr[2] = str;   //   checkbox li versiyon

                if (Okyanus.Variables.Var_Length[i] == 0) dr[3] = "Bit Read";
                else dr[3] = Okyanus.Variables.Var_Length[i];

                if (Okyanus.Variables.Var_Length[i] == 0) dr[4] = Okyanus.Variables.Var_Bit[i];
                else dr[4] = "   ";

                if (Okyanus.Variables.Var_PlotOnOff[i] == 1)
                {
                    dr[5] = "Yok";    //   checkbox li versiyon
                    dr[6] = " ";
                    dr[7] = " ";
                    dr[8] = " ";
                    dr[9] = " ";
                }
                else
                {
                    dr[5] = "Var";  //   checkbox li versiyon
                    switch(Okyanus.Variables.Var_PlotMult[i]){
                            case 0:dr[6] = Okyanus.Definitions.Mul_0001;break;
                            case 1:dr[6] = Okyanus.Definitions.Mul_001;break;
                            case 2:dr[6] = Okyanus.Definitions.Mul_01;break;
                            case 3:dr[6] = Okyanus.Definitions.Mul_1;break;
                            case 4:dr[6] = Okyanus.Definitions.Mul_10;break;
                            case 5:dr[6] = Okyanus.Definitions.Mul_100;break;
                            case 6: dr[6] = Okyanus.Definitions.Mul_1000; break;         
                    }                    
                    dr[7] = Okyanus.Variables.Var_PlotMin[i];
                    dr[8] = Okyanus.Variables.Var_PlotMax[i];
                    dr[9] = Okyanus.Variables.Var_PlotColour[i];
                }
                dataElements.Rows.Add(dr);  // row eklemek icin kullan
            }
            gridHelper.PrintScreen();
            //         Okyanus.Variables.NameArr[0] = dataElements.Rows[0][0].ToString();

            //     textBox1.Text = dataElements.Rows[0][0].ToString();  // raw/ column

            //
            //          dataElements.Rows[0][1] = 22;

            DATA_LogConfigTable_Read();
   
        }
        public void DATA_CommandTable_Read()/**/
        {
            GridHelper gridHelper = new GridHelper(dt_Command);
            DataTable dataElements = gridHelper.GetDataTable();
            List<String> gridValues = gridHelper.GetGridValues();
            //     dt.EditingControl
            //
            //         if (gridValues.Count > 0)
            //        {
            //     Okyanus.Variables.Name_OfInput.Add(gridValues[0]);
            //    Okyanus.Variables.Variables_Name[i] = gridValues[0].ToString();
            textBoxss_debug.Text = "";
            for (int i = 0; i < gridValues.Count; i++)
            {
                //      Okyanus.Variables.Variables_NameBuffer[i] = gridValues[i]; // ilk colon icin colon sayisini atlattir
                Okyanus.Variables.CommandVar_Name[i] = gridValues[i]; // bu raw0  u oku  ve at        
                Okyanus.Variables.CommandVar_NameIndex = i;

            }
        }
      
                public void DATA_Fill_Grid_From_CommandArrays()
                {
                    GridHelper gridHelper = new GridHelper(dt_Command);
                    DataTable dataElements = gridHelper.GetDataTable4Command_oldType();
                    DATA_CommandTable_Read();
                    String str = "";
                    for (int i = 0; i < Okyanus.Variables.CommandRawIndex; i++)
                    {
                        DataRow dr = dataElements.NewRow();               
                        if (i < (Okyanus.Variables.CommandRawIndex - 1)) dr[0] = Okyanus.Variables.CommandVar_Name[i];
                        else dr[0] = i + 1;

                        switch (Okyanus.Variables.Var_Command[i])
                        {
                            default:
                            case 0: str = Okyanus.Definitions.Komut00; break;
                            case 1: str = Okyanus.Definitions.Komut01; break;
                            case 2: str = Okyanus.Definitions.Komut02; break;
                            case 3: str = Okyanus.Definitions.Komut03; break;
                            case 4: str = Okyanus.Definitions.Komut04; break;
                            case 5: str = Okyanus.Definitions.Komut05; break;
                            case 6: str = Okyanus.Definitions.Komut06; break;
                            case 7: str = Okyanus.Definitions.Komut07; break;
                            case 8: str = Okyanus.Definitions.Komut08; break;
                            case 9: str = Okyanus.Definitions.Komut09; break;
                            case 10: str = Okyanus.Definitions.Komut10; break;
                            case 11: str = Okyanus.Definitions.Komut11; break;
                            case 12: str = Okyanus.Definitions.Komut12; break;
                            case 13: str = Okyanus.Definitions.Komut13; break;
                            case 14: str = Okyanus.Definitions.Komut14; break;
                            case 15: str = Okyanus.Definitions.Komut15; break;
                            case 16: str = Okyanus.Definitions.Komut16; break;
                            case 17: str = Okyanus.Definitions.Komut17; break;
                            case 18: str = Okyanus.Definitions.Komut18; break;
                            case 19: str = Okyanus.Definitions.Komut19; break;
                            case 20: str = Okyanus.Definitions.Komut20; break;
                            case 21: str = Okyanus.Definitions.Komut21; break;
                            case 22: str = Okyanus.Definitions.Komut22; break;
                            case 23: str = Okyanus.Definitions.Komut23; break;
                            case 24: str = Okyanus.Definitions.Komut24; break;
                            case 25: str = Okyanus.Definitions.Komut25; break;
                            case 26: str = Okyanus.Definitions.Komut26; break;
                            case 27: str = Okyanus.Definitions.Komut27; break;
                            case 28: str = Okyanus.Definitions.Komut28; break;
                            case 29: str = Okyanus.Definitions.Komut29; break;
                            case 30: str = Okyanus.Definitions.Komut30; break;
                            case 31: str = Okyanus.Definitions.Komut31; break;
                            case 32: str = Okyanus.Definitions.Komut32; break;
                            case 33: str = Okyanus.Definitions.Komut33; break;
                            case 34: str = Okyanus.Definitions.Komut34; break;
                            case 35: str = Okyanus.Definitions.Komut35; break;
                            case 36: str = Okyanus.Definitions.Komut36; break;
                            case 37: str = Okyanus.Definitions.Komut37; break;
                            case 38: str = Okyanus.Definitions.Komut38; break;
                            case 39: str = Okyanus.Definitions.Komut39; break;
                            case 40: str = Okyanus.Definitions.Komut40; break;
                            case 41: str = Okyanus.Definitions.Komut41; break;
                            case 42: str = Okyanus.Definitions.Komut42; break;
                            case 43: str = Okyanus.Definitions.Komut43; break;
                            case 44: str = Okyanus.Definitions.Komut44; break;
                            case 45: str = Okyanus.Definitions.Komut45; break;
                            case 46: str = Okyanus.Definitions.Komut46; break;
                            case 47: str = Okyanus.Definitions.Komut47; break;
                            case 48: str = Okyanus.Definitions.Komut48; break;
                            case 49: str = Okyanus.Definitions.Komut49; break;
                        }
                        dr[1] = str;
       
                        //       if (i > Okyanus.Variables.CommandVar_NameIndex) dr[2] = Okyanus.Variables.Var_Parameter1[i];
                        //      else dr[2] = Okyanus.Variables.CommandParameter1_value[i];
                        dr[2] = Okyanus.Variables.Var_Parameter1[i];
                        dr[3] = Okyanus.Variables.Var_Parameter2[i];
                        dr[4] = Okyanus.Variables.Var_Parameter3[i];
                        dr[5] = Okyanus.Variables.Var_Parameter4[i];
                        dataElements.Rows.Add(dr);  // row eklemek icin kullan
                    }
                    gridHelper.PrintScreen();
                    DATA_CommandTable_Read();
                    //       gridHelper.Export("D:\\DataLog\\ComConfig.csv", FileTypes.CSV);
                }
    

        public void DATA_Fill_Grid_From_FlashCommandArrays()
        {      
            GridHelper gridHelper = new GridHelper(dt_Command);
            DataTable dataElements = gridHelper.GetDataTable4Command();
            DATA_CommandTable_Read();
      //      String str = "";
            for (int i = 0; i < Okyanus.Variables.CommandRawIndex; i++)
            {
                DataRow dr = dataElements.NewRow();

                dr[0] = Okyanus.Variables.Var_Command[i];
                dr[1] = Okyanus.Variables.Var_Parameter1[i];
                dr[2] = Okyanus.Variables.Var_Parameter2[i];
                dr[3] = Okyanus.Variables.Var_Parameter3[i];
                dr[4] = Okyanus.Variables.Var_Parameter4[i];
                dataElements.Rows.Add(dr);  // row eklemek icin kullan
            }
            gridHelper.PrintScreen();
            DATA_CommandTable_Read();

        }
        public void DATA_populate_Config_Arrays_FromComboboxes()
        {
            Okyanus.Variables.Var_Adress[Okyanus.Variables.RawIndex] = Okyanus.Variables.AdressArr[comboBox_Adress.SelectedIndex];
            Okyanus.Variables.Var_Sign[Okyanus.Variables.RawIndex] = Okyanus.Variables.SignArr[comboBox_Sign.SelectedIndex];
            Okyanus.Variables.Var_Length[Okyanus.Variables.RawIndex] = Okyanus.Variables.ByteArr[(byte)comboBox_Length.SelectedIndex];
            Okyanus.Variables.Var_Bit[Okyanus.Variables.RawIndex] = Okyanus.Variables.BitArr[(int)comboBox_BitSet.SelectedIndex];
            Okyanus.Variables.Var_PlotMult[Okyanus.Variables.RawIndex] = Okyanus.Variables.PlotMultArr[(int)comboBox_Chart_Multiply.SelectedIndex];
            Okyanus.Variables.Var_PlotOnOff[Okyanus.Variables.RawIndex] = Okyanus.Variables.PlotOnOffArr[(int)comboBox_Chart_On.SelectedIndex];

            Okyanus.Variables.Var_PlotMin[Okyanus.Variables.RawIndex] = (int)numericUpDown_ChartMin.Value;
            Okyanus.Variables.Var_PlotMax[Okyanus.Variables.RawIndex] = (int)numericUpDown_ChartMax.Value;
            Okyanus.Variables.Var_PlotColour[Okyanus.Variables.RawIndex] = Okyanus.Variables.PlotColorArr[(int)comboBox_ChartColour.SelectedIndex];

        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine00(uint i) 
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par0.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par1.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par2.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par3.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par4.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine01(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par5.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par6.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par7.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par8.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par9.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine02(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par10.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par11.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par12.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par13.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par14.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine03(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par15.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par16.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par17.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par18.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par19.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine04(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par20.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par21.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par22.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par23.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par24.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine05(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par25.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par26.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par27.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par28.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par29.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine06(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par30.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par31.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par32.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par33.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par34.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine07(uint i)
        {
                  Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par35.Value;
                    Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par36.Value;
                    Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par37.Value;
                    Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par38.Value;
                    Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par39.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine08(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par40.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par41.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par42.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par43.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par44.Value;
        }
        public void DATA_populate_CommandArrays_From_NumericBoxesLine09(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par45.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par46.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par47.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par48.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par49.Value;
        }
        public void DATA_populate_CommandArrays_From_Comboboxes_NumericBoxesV2()
        {
            DATA_populate_CommandArrays_From_NumericBoxesLine00(0);
            DATA_populate_CommandArrays_From_NumericBoxesLine01(1);
            DATA_populate_CommandArrays_From_NumericBoxesLine02(2);
            DATA_populate_CommandArrays_From_NumericBoxesLine03(3);
            DATA_populate_CommandArrays_From_NumericBoxesLine04(4);
            DATA_populate_CommandArrays_From_NumericBoxesLine05(5);
            DATA_populate_CommandArrays_From_NumericBoxesLine06(6);
            DATA_populate_CommandArrays_From_NumericBoxesLine07(7);
            DATA_populate_CommandArrays_From_NumericBoxesLine08(8);
            DATA_populate_CommandArrays_From_NumericBoxesLine09(9);     
        }
        public void DATA_populate_CommandArrays_From_Comboboxes_NumericBoxesV1()
        {
            if (numericUpDownV1_Par1.Value > 65535) numericUpDownV1_Par1.Value = 65535;
            if (numericUpDownV1_Par2.Value > 65535) numericUpDownV1_Par2.Value = 65535;
            if (numericUpDownV1_Par3.Value > 65535) numericUpDownV1_Par3.Value = 65535;
            if (numericUpDownV1_Par4.Value > 65535) numericUpDownV1_Par4.Value = 65535;
            if (numericUpDownV1_Par1.Value < 0) numericUpDownV1_Par1.Value = 0;
            if (numericUpDownV1_Par2.Value < 0) numericUpDownV1_Par2.Value = 0;
            if (numericUpDownV1_Par3.Value < 0) numericUpDownV1_Par3.Value = 0;
            if (numericUpDownV1_Par4.Value < 0) numericUpDownV1_Par4.Value = 0;
      //      if (numericUpDown_Par0.Value > 65535) numericUpDown_Par0.Value = 65535;
      //      if (numericUpDown_Par0.Value < 0) numericUpDown_Par0.Value = 0;

            //          1. versiyon
                  Okyanus.Variables.Var_Command[Okyanus.Variables.CommandRawIndex] = Okyanus.Variables.CommandArr[comboBox_Command.SelectedIndex]; // numara ile
                  Okyanus.Variables.Var_Parameter1[Okyanus.Variables.CommandRawIndex] = (UInt16)numericUpDownV1_Par1.Value;
                   Okyanus.Variables.Var_Parameter2[Okyanus.Variables.CommandRawIndex] = (UInt16)numericUpDownV1_Par2.Value;
                   Okyanus.Variables.Var_Parameter3[Okyanus.Variables.CommandRawIndex] = (UInt16)numericUpDownV1_Par3.Value;
                    Okyanus.Variables.Var_Parameter4[Okyanus.Variables.CommandRawIndex] = (UInt16)numericUpDownV1_Par4.Value;
            //  2. versiyon
        }

        private void DATA_CommandFlashArray_ReadFill_Grid()
        {   // 100 bytelik flash okuma bu
            // bu flash yazma komutu islenip flash yazildiktan sonra
            // flash bufferlarindakini gridlere yazip guncelleyecek
            // bunu okuma yapildiktan sonra cagirmak gerekiyor
            byte i;


           for (i = 0; i < 11; i++)
            {
                Okyanus.Variables.CommandRawIndex = i;
                switch (Okyanus.Variables.Version)
                {
                    case Okyanus.Definitions.BJUEmulatorKisitli:
                        break;
                    case Okyanus.Definitions.BJUEmulator:
                      //      for (i = 0; i < (Okyanus.Definitions.MaxCommandLine + 1); i++)
                     //     {
                       //     Okyanus.Variables.CommandRawIndex = i;
                            DATA_Fill_Grid_From_FlashCommandArrays();  // 2. versiyon
                     //   }
                        break;
                    case Okyanus.Definitions.KahveMakinasiEmulator2: //DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma

                        DATA_Fill_Grid_From_CommandArrays();

                        break;
                    case Okyanus.Definitions.TostMakinasiEmulatoru2:
                        DATA_Fill_Grid_From_CommandArrays();
                        break;
                    case Okyanus.Definitions.KahveMakinasiEmulator1: //DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                        DATA_Fill_Grid_From_CommandArrays();
                        break;
                    default:
                        break;
                }
            }

        }

        private void DATA_CommandArray_Table_UpdateAll()
        {  // flash tablusunu guncelle yani tampondan tabloya atiyor  numeric boxlardan aldikkarini gride atiyor butun fonk
            DATA_populate_CommandArrays_From_Comboboxes_NumericBoxesV1();
            // ustteki numeric boxlardan aldiklarini aradaki buffer a atiyor yani flash okuma yazma yapilan buffer a
            StopCommandsendProcedure();
            /*
            if (Okyanus.Variables.CommandRawIndex < Okyanus.Definitions.MaxCommandLine)
            {
                Okyanus.Variables.CommandRawIndex++;
            }
            else
            {
                MessageBox.Show("Max Komut Sayisi Asildi");
                return;
            }
            */
            // allttaki toplam satir sayisi kadar flash bufferlarindakileri gride yaziyor
            byte i;
            for (i = 0; i < (Okyanus.Definitions.MaxCommandLine + 1); i++)
            {
                Okyanus.Variables.CommandRawIndex = i;
                DATA_Fill_Grid_From_CommandArrays();

            }
            textBox_CommandFileName.Text = "";
            //    DATA_Fill_Grid_From_CommandArrays();
        }

        private void DATA_CommandFlashArray_Table_UpdateAll()
        {  // flash tablusunu guncelle yani tampondan tabloya atiyor  numeric boxlardan aldikkarini gride atiyor butun fonk
            DATA_populate_CommandArrays_From_Comboboxes_NumericBoxesV2();
            // ustteki numeric boxlardan aldiklarini aradaki buffer a atiyor yani flash okuma yazma yapilan buffer a
            StopCommandsendProcedure();
            /*
            if (Okyanus.Variables.CommandRawIndex < Okyanus.Definitions.MaxCommandLine)
            {
                Okyanus.Variables.CommandRawIndex++;
            }
            else
            {
                MessageBox.Show("Max Komut Sayisi Asildi");
                return;
            }
            */
            // allttaki toplam satir sayisi kadar flash bufferlarindakileri gride yaziyor
            byte i;
            for (i = 0; i < (Okyanus.Definitions.MaxCommandLine+1); i++)
            {
                         Okyanus.Variables.CommandRawIndex = i;
                       DATA_Fill_Grid_From_FlashCommandArrays();  // 2. versiyon

            }
            textBox_CommandFileName.Text = "";
            //    DATA_Fill_Grid_From_CommandArrays();
        }
        private void DATA_CommandFlashArray_Table_Add()
        {
            DATA_populate_CommandArrays_From_Comboboxes_NumericBoxesV2();

            StopCommandsendProcedure();
            if (Okyanus.Variables.CommandRawIndex < Okyanus.Definitions.MaxCommandLine)
            {
                Okyanus.Variables.CommandRawIndex++;
            }
            else
            {
                MessageBox.Show("Max Komut Sayisi Asildi");
                return;
            }
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator: DATA_Fill_Grid_From_FlashCommandArrays();  // 2. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                default:
                    break;
            } 

            textBox_CommandFileName.Text = "";
        }
        private void DATA_CommandArray_Table_Add()
        {
            DATA_populate_CommandArrays_From_Comboboxes_NumericBoxesV1();

            StopCommandsendProcedure();
            if (Okyanus.Variables.CommandRawIndex < Okyanus.Definitions.MaxCommandLine)
            {
                Okyanus.Variables.CommandRawIndex++;
            }
            else
            {
                MessageBox.Show("Max Komut Sayisi Asildi");
                return;
            }
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator: DATA_Fill_Grid_From_FlashCommandArrays();  // 2. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                default:
                    break;
            } 

            textBox_CommandFileName.Text = "";
        }
        private void DATA_CommandArray_Table_Delete()
        {
            DATA_populate_CommandArrays_From_Comboboxes_NumericBoxesV1();
            if (Okyanus.Variables.CommandRawIndex != 0) Okyanus.Variables.CommandRawIndex--; // min Rawindexe kadar
            //       if(Okyanus.Variables.CommandRawIndex == 0) Okyanus.Variables.SendCommandEnable = false;

            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator: DATA_Fill_Grid_From_FlashCommandArrays();  // 2. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                default:
                    break;
            }           
            textBox_CommandFileName.Text = "";
        }
        private void DATA_ConfigArray_Table_Add()
        {
            if (LOG_OnOffCheck() == false) return;
            DATA_populate_Config_Arrays_FromComboboxes();

            if (Okyanus.Variables.Var_Length[Okyanus.Variables.RawIndex] == 0) { if (comboBox_Adress.SelectedIndex < (Okyanus.Variables.AdressArr.GetLength(0))) comboBox_Adress.SelectedIndex += 0; }//
            if (Okyanus.Variables.Var_Length[Okyanus.Variables.RawIndex] == 1) { if (comboBox_Adress.SelectedIndex < (Okyanus.Variables.AdressArr.GetLength(0) - 1)) comboBox_Adress.SelectedIndex += 1; }//
            if (Okyanus.Variables.Var_Length[Okyanus.Variables.RawIndex] == 2) { if (comboBox_Adress.SelectedIndex < (Okyanus.Variables.AdressArr.GetLength(0) - 2)) comboBox_Adress.SelectedIndex += 2; }//
            if (Okyanus.Variables.Var_Length[Okyanus.Variables.RawIndex] == 4) { if (comboBox_Adress.SelectedIndex < (Okyanus.Variables.AdressArr.GetLength(0) - 4)) comboBox_Adress.SelectedIndex += 4; }//

            Okyanus.Variables.RawIndex++; // max Rawindexe kadar

            Okyanus.Variables.MaxRawIndex = 0;
            int j = 0;
            byte Cumulative = 1;

            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                if (Okyanus.Variables.Var_Length[i] == 0) Cumulative = 1;
                if (Okyanus.Variables.Var_Length[i] == 1) Cumulative = 1;
                if (Okyanus.Variables.Var_Length[i] == 2) Cumulative = 2;
                if (Okyanus.Variables.Var_Length[i] == 4) Cumulative = 4;

                Okyanus.Variables.MaxRawIndex += Cumulative;
                j = i;
            }
            if (Okyanus.Variables.MaxRawIndex > Okyanus.Definitions.MaxConfigLine) // max 25 adet
            {
                if (Okyanus.Variables.Var_Length[j] == 0) Cumulative = 1;
                if (Okyanus.Variables.Var_Length[j] == 1) Cumulative = 1;
                if (Okyanus.Variables.Var_Length[j] == 2) Cumulative = 2;
                if (Okyanus.Variables.Var_Length[j] == 4) Cumulative = 4;
                Okyanus.Variables.MaxRawIndex -= Cumulative;
                //      textBox1.Text = Okyanus.Variables.MaxRawIndex.ToString();
                Okyanus.Variables.RawIndex--;
                MessageBox.Show("Max Adress Asildi");
                return;
            }
            DATA_Fill_Grid_From_LogConfigArrays();  // arrayden doldur
      //      LOG_PopulatetexboxandCheckBoxes();
            Okyanus.Chart.LogPropUpdate = true; // update Log textboxes and check boxes
            textBox_Log_FileName.Text = "";
        }
        private void DATA_ConfigArray_Table_Delete()
        {
            if (LOG_OnOffCheck() == false) return;
            DATA_populate_Config_Arrays_FromComboboxes();
            if (Okyanus.Variables.RawIndex != 0) Okyanus.Variables.RawIndex--; // min Rawindexe kadar
            //  else Okyanus.Variables.Variables_Name[0] = " Variable0";
            DATA_Fill_Grid_From_LogConfigArrays();
    //        LOG_PopulatetexboxandCheckBoxes();
            Okyanus.Chart.LogPropUpdate = true; // update Log textboxes and check boxes
            textBox_Log_FileName.Text = "";
        }
        /*
        private void dt_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            GridHelper gridHelper = new GridHelper(dt);
            DataTable dataElements = gridHelper.GetDataTable();
    //        textBox1.Text = "  ";
            //    dataElements.Rows[0][0].ToString();
       //          textBox1.Text = dataElements.Rows.Count.ToString();

        }
*/
    //    Bitmap memoryImage;
        private void takeScreenShotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileToolStripMenuItem.Visible = false;
            takeScreenShotToolStripMenuItem.Visible = false;
    //        takeScreenShotToolStripMenuItem.m
            SNAPSHOT_LogScreen(); // ilk ana menu
            takeScreenShotToolStripMenuItem.Visible = true;
            fileToolStripMenuItem.Visible = true;

            //   ScreenCapture sc = new ScreenCapture();
            /*
            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                //     bitmap.Save("C://test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                try
                {
                    //bitmap.Save(Okyanus.Definitions.Log_Directory + "\\" + LOG_GetTime() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitmap.
                }
                catch (Exception)
                {
                    MessageBox.Show("Save error");
                }
            }
            */
  /*          Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
*/
            //printscreen.
       //       System.Drawing.Imaging.  
      //      printDocument1.Print();
    //        System.Drawing.

        }
        private void printDocument1_PrintPage(System.Object sender,System.Drawing.Printing.PrintPageEventArgs e)
        {
         //   e.Graphics.DrawImage(memoryImage, 0, 0);
        }
        private bool LOG_OnOffCheck()
        {
            if (Okyanus.Variables.Log_Status == true) { MessageBox.Show(Okyanus.Definitions.LogOffWarning); return false; }
            return true;
        }
        public void LOG_SaveLogFile()
        {
            if (Okyanus.Variables.RawIndex == 0)
            {
                MessageBox.Show("En az bir tane degisken secmelisiniz!"); return;
            }

            if (LOG_File_SaveAs() == false) return;
            Okyanus.Chart.PlayDelay = 2;
            Okyanus.Chart.Play = true;
   //         ChartPlayPause();
            //          Okyanus.Chart.Play = true;
             ChartPlayPause();

             List<string> aciklar = new List<string>();
             foreach (Form item in Application.OpenForms)
             {
                 aciklar.Add(item.Text);
             }
             string durum = "";
             durum = aciklar.Find(a => a == Okyanus.Chart.VeriFormBasligi);
             if (durum == null)
             {
                 verileriAcToolStripMenuItem_Click(this, null);
             }
        }
        public void LOG_StopLog()
        {
            Okyanus.Chart.Play = false;
            ChartPlayPause();
            LOG_File_Stop();
        }
        public void LOG_ViewLogFile()
        {
            if (Okyanus.Variables.Log_Status == true)
            {
                MessageBox.Show("Logging is  Continuing...  Firts Stop Logging !");
                return;
            }
            try
            {
                //		System::Diagnostics.Process::Start(Original_Log_File);  		
                //		System::Diagnostics.Process::Start(Original_Log_File_Base);  
                //		System::Diagnostics.Process::StartInfo::set(
                Process.Start("Excel.exe", Okyanus.Definitions.Original_Log_File_Base);
                //		Process::Start( "Excel.exe", Okyanus.Definitions.WorkDrive + Original_Log_File );  // dosya sonunda 001 oldugu icin tipini anlamadan stabdart txt dostasi gibi aciyor
            }
            catch (IOException)
            {
                MessageBox.Show("Log File Error or No File Selected ! ");
            }
        }
        public String LOG_GetTimeWithSec()
        {
            DateTime d1 = DateTime.Now;
            int Day = d1.Day; int Month = d1.Month; int Year = d1.Year; int Hour = d1.Hour; int Min = d1.Minute;
            int Sec =  d1.Second;
            String Mystring = "";
            if (Day < 10) Mystring += "0"; else Mystring += "";
            Mystring += Day.ToString() + ".";
            if (Month < 10) Mystring += "0"; else Mystring += "";
            Mystring += Month.ToString() + ".";
            Mystring += Year.ToString() + ".";
            if (Hour < 10) Mystring += "0"; else Mystring += "";
            Mystring += Hour.ToString() + ".";
            if (Min < 10) Mystring += "0"; else Mystring += "";
            Mystring += Min.ToString()+ ".";
            if (Sec < 10) Mystring += "0"; else Mystring += "";
            Mystring += Sec.ToString();
            return Mystring;
        }
        public String LOG_GetTime()
        {
            DateTime d1 = DateTime.Now;
            int Day = d1.Day; int Month = d1.Month; int Year = d1.Year; int Hour = d1.Hour; int Min = d1.Minute;

            String Mystring = "";
            if (Day < 10) Mystring += "0"; else Mystring += "";
            Mystring += Day.ToString() + ".";
            if (Month < 10) Mystring += "0"; else Mystring += "";
            Mystring += Month.ToString() + ".";
            Mystring += Year.ToString() + "__";
            if (Hour < 10) Mystring += "0"; else Mystring += "";
            Mystring += Hour.ToString() + ".";
            if (Min < 10) Mystring += "0"; else Mystring += "";
            Mystring += Min.ToString();
            return Mystring;
        }
        public bool LOG_File_SaveAs()
        {
            if (SP1_serialPort.IsOpen == false) { MessageBox.Show("Com Port Not Open"); return false; }
            if (Okyanus.Variables.Log_Status == true) { MessageBox.Show("First Stop Logging "); return false; }

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Log_Directory;
            saveFileDialog1.Title = "Select  a File to Log Data";
            if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Log_Directory)))
            { // directory yoksa my documents dan basliyor
                try { Directory.CreateDirectory(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Log_Directory); }
                catch (IOException)
                {
                    Okyanus.Variables.Log_Error = true;
                    MessageBox.Show("Log Save Error !!");
                    //    return;
                }// e->GetType()->Name ;
            }
            String str = LOG_GetTime() + ".csv";
            saveFileDialog1.FileName = str;
            saveFileDialog1.Filter = "Txt files (*.txt)|*.txt | Csv files (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {     // hata dosya aciksa kitleniyor burada
                   //     Okyanus.Definitions.WorkDrive + 
                            Okyanus.Definitions.Original_Log_File = saveFileDialog1.FileName; //secilen isimle dosya ismini tut
                        /* 
                               Original_Log_File_Base = Original_Log_File;
                               GUI.Log.Original_Log_File_post_add = 0;
                               GUI.Log.Status = true;
                               GUI.Log.Error = false ;
                           */
                        Okyanus.Definitions.Original_Log_File_Base = Okyanus.Definitions.Original_Log_File; // Okyanus.Definitions.WorkDrive +
                        Okyanus.Variables.Log_Status = true;
                        Okyanus.Variables.Log_Error = false;
                        myStream.Close();
                    }
                }
                catch (IOException)
                {
                    Okyanus.Variables.Log_Error = true;
                    Okyanus.Variables.Log_Status = false;
                    MessageBox.Show("File Error !!" + nl + "Close File If Open");
                    return false;
                }

                LOG_LogStatusUpdate();
            }
            Okyanus.Variables.Log_SampleCounter = 0;
            return true;
        }
        public void LOG_File_Stop()
        {

            if (Okyanus.Variables.Log_Status == false) { MessageBox.Show("No Log File to Stop "); return; }
            Okyanus.Variables.Log_Status = false;

            Okyanus.Variables.Log_Counter = 0;
            LOG_LogStatusUpdate();
            Okyanus.Variables.Log_SampleCounter = 0;

        }
        public void LOG_LogStatusUpdate()
        {
            String Str;
            if (Okyanus.Variables.Log_Status == true)
            {
                if (Okyanus.Variables.Log_Error == true)
                {
                    Str = "Stopped @: " + COMMON_GetDateTime() + "Did You Open the File?";
                    startTimeToolStripMenuItem1.ForeColor = System.Drawing.Color.Black;
                }
                else
                {
                    Str = "Started @ " + COMMON_GetDateTime() + " to " +  Okyanus.Definitions.Original_Log_File;
                    startTimeToolStripMenuItem1.ForeColor = System.Drawing.Color.Green;
                }
                startTimeToolStripMenuItem1.Text = Str;
                dataLogOnOffToolStripMenuItem1.Text = "Logging to : " +  Okyanus.Definitions.Original_Log_File;
                dataLogOnOffToolStripMenuItem1.ForeColor = System.Drawing.Color.Green;
                stopTimeToolStripMenuItem1.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                if (Okyanus.Variables.Log_Error == true) { Str = "Stopped @: " + COMMON_GetDateTime() + "  Is  File Already Open?"; }
                else { Str = "Stopped @: " + COMMON_GetDateTime() + " to " +   Okyanus.Definitions.Original_Log_File; }
                stopTimeToolStripMenuItem1.Text = Str;
                dataLogOnOffToolStripMenuItem1.ForeColor = System.Drawing.Color.Black;
                dataLogOnOffToolStripMenuItem1.Text = "Logging Off   ";
                stopTimeToolStripMenuItem1.ForeColor = System.Drawing.Color.Red;
                startTimeToolStripMenuItem1.ForeColor = System.Drawing.Color.Black;
            }
        }
        public String COMMON_GetDateTime()
        {
            //	DateTime oldDate = DateTime.Parse("11/9/03 12:00");

            DateTime d1 = DateTime.Now;
            int Hour = d1.Hour; int Min = d1.Minute; int Sec = d1.Second; int Day = d1.Day; int Month = d1.Month; int Year = d1.Year;
            String HourZero = ""; String MinZero = ""; String SecZero = "";
            if (Hour < 10) HourZero = "0"; else HourZero = "";
            if (Min < 10) MinZero = "0"; else MinZero = "";
            if (Sec < 10) SecZero = "0"; else SecZero = "";
            //		   String ^ Mystring = Day.ToString() + "." +  Month.ToString() + "."+ Year.ToString() + ",";
            //			Mystring += HourZero +  Hour.ToString()+":"+MinZero+Min.ToString()+":"+SecZero+Sec.ToString() ;

            String Mystring = HourZero + Hour.ToString() + ":" + MinZero + Min.ToString() + ":" + SecZero + Sec.ToString() + "    ";
            Mystring += Day.ToString() + "." + Month.ToString() + "." + Year.ToString();
            return Mystring;

        }
        public String WriteStream(StreamWriter Sw, String path, String data)
        {
            try
            {
                if (Sw != null)
                {
                    Sw.WriteLine(data);
                }
            }
            catch (IOException) { return "false"; }
            return "true";
        }
        public void LOG_ShowVariablesAtLogScreen()  //komple child forma tasi
        {

        }
        public void LOG_DataLogProcess(String FileName)
        {
            if (!Okyanus.Variables.Log_Status)
            {
                Okyanus.Variables.Log_Error = false;
                return;
            }
     //       if (LOG_SampleCount() != 0) return;

            String path = FileName;
            //				path= Environment::SpecialFolder::MyDocuments;
            if (!(File.Exists(path)))
            {
                try
                {
                    FileStream fs = File.Create(path);
                    //	delete fs;
                }
                catch (IOException) { return; }
            }
            // create a strem reader
            StreamReader sr;
            try { sr = new StreamReader(path); }
            catch (IOException) { Okyanus.Variables.Log_Error = true; return; }

            String DataFile;
            DataFile = LOG_ReadStream(sr, path); // read data from file
            if (DataFile == "false") return;
            if (LOG_DeleteStreamReader(sr) == "false") return;	 //
            // create a stream writer
            StreamWriter sw;
            try { sw = new StreamWriter(path); }
            catch (IOException) { Okyanus.Variables.Log_Error = true; return; }
            // wite 2 stream
            if (LOG_WriteStream(sw, path, DataFile) == "false") return;	 // write  data to file
            if (LOG_DeleteStreamWriter(sw) == "false") return;	 // write  data to file
            String fr;
            if (Okyanus.Variables.Log_Counter >= Okyanus.Chart.MAXLOGCOUNT)
            {			 // DEFAULT_LOGMAXHOUR

                Okyanus.Variables.Log_FileCount += Okyanus.Variables.Log_Counter;
                Okyanus.Variables.Log_Counter = 0;
                Okyanus.Variables.Log_Original_Log_File_post_add++;

                if (Okyanus.Variables.Log_Original_Log_File_post_add < 10) fr = "00";
                else if (Okyanus.Variables.Log_Original_Log_File_post_add < 100) fr = "0";
                else fr = "";
          //      Okyanus.Definitions.WorkDrive + 
                    Okyanus.Definitions.Original_Log_File = Okyanus.Definitions.Original_Log_File_Base + fr + Okyanus.Variables.Log_Original_Log_File_post_add.ToString();	 // add 

            }
            else
            {

                Okyanus.Variables.Log_Counter++;
            }
        }
        public String LOG_ReadStream(StreamReader Sr, String path)
        {
            try
            {
                if (Sr != null)
                {     // hata dosya aciksa kitleniyor burada
                    return Sr.ReadToEnd();
                }
            }
            catch (IOException) { Okyanus.Variables.Log_Error = true; return "false"; }
            return "false";
        }
        public String LOG_WriteStream(StreamWriter Sw, String path, String data)
        {
            try
            {
                if (Sw != null)
                    if (Okyanus.Variables.Log_Counter != 0) Sw.WriteLine(data + (Okyanus.Variables.Log_Counter + Okyanus.Variables.Log_FileCount).ToString() + LOG_PrepareData_2WriteFile_01());
                    // ilk sira     
                    else
                    {

                        String Mystring = " Log No,   Tarih  , Saat , ";
                        for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
                        {
                            Mystring += Okyanus.Variables.Variables_Name[i] + ",";
                        }
                        Sw.WriteLine(data + Mystring);
                    }
            }
            catch (IOException) { Okyanus.Variables.Log_Error = true; return "false"; };
            return "true";
        }
        public String LOG_PrepareData_2WriteFile_01()
        {

            DateTime d1 = DateTime.Now;

            int Hour = d1.Hour; int Min = d1.Minute; int Sec = d1.Second; int Day = d1.Day; int Month = d1.Month; int Year = d1.Year;
            String HourZero = "";
            String MinZero = "";
            String SecZero = "";
            if (Hour < 10) HourZero = "0"; else HourZero = "";
            if (Min < 10) MinZero = "0"; else MinZero = "";
            if (Sec < 10) SecZero = "0"; else SecZero = "";

            String Mystring = ",";
            Mystring += Day.ToString() + "." + Month.ToString() + "." + Year.ToString() + ",";
            Mystring += HourZero + Hour.ToString() + ":" + MinZero + Min.ToString() + ":" + SecZero + Sec.ToString();
            Mystring += ",";
      
            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                Mystring += Okyanus.Variables.Variables_Data[i] + ",";
            }
            return Mystring;

        }
        public String LOG_DeleteStreamWriter(StreamWriter Sw)
        {
            try
            {
                if (Sw != null)
                {

                    Sw.Close();

                }
            }
            catch (IOException) { Okyanus.Variables.Log_Error = true; return "false"; };
            return "true";
        }
        public String LOG_DeleteStreamReader(StreamReader Sr)
        {
            try
            {
                if (Sr != null)	//delete Sr;
                    Sr.Close();
            }
            catch (IOException) { Okyanus.Variables.Log_Error = true; return "false"; }
            return "true";
        }
        public void LOG_PopulatetexboxandCheckBoxes()  // komple child form a tasi\\
        {
            LOG_texBoxesColoursSelect();
        }
        public void LOG_texBoxesColoursSelect() // komple child form a tasi
        {
/*
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
*/
        }
        ///<Summary>
        /// Gets the answer
        ///</Summary>
        public String LOG_GetDateTime()
        {
            DateTime d1 = DateTime.Now;

            int Hour = d1.Hour; int Min = d1.Minute; int Sec = d1.Second; int Day = d1.Day; int Month = d1.Month; int Year = d1.Year;
            String HourZero = "";
            String MinZero = "";
            String SecZero = "";
            if (Hour < 10) HourZero = "0"; else HourZero = "";
            if (Min < 10) MinZero = "0"; else MinZero = "";
            if (Sec < 10) SecZero = "0"; else SecZero = "";

            String Mystring;
            Mystring = Day.ToString() + "." + Month.ToString() + "." + Year.ToString() + "  ";
            Mystring += HourZero + Hour.ToString() + ":" + MinZero + Min.ToString() + ":" + SecZero + Sec.ToString();

            return Mystring;
        }
        ///<Summary>
        /// Gets the answer
        ///</Summary>
        public void File_Save_CommandFile()
        {
            GridHelper gridHelper = new GridHelper(dt_Command);
          //  File_SaveAsCommon("D:\\DataLog\\ComConfig", gridHelper, "Command");
            File_SaveAsCommon(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Command_File, gridHelper, "Command");
        }
        public void File_Save_FlashCommandFile()
        {
            GridHelper gridHelper = new GridHelper(dt_Command);
            //  File_SaveAsCommon("D:\\DataLog\\ComConfig", gridHelper, "Command");
            File_SaveAsCommon(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.FlashCommand_File, gridHelper, "Command");
        }       
        public void File_Save_ConfigFile()
        {
            GridHelper gridHelper = new GridHelper(dt);
            File_SaveAsCommon(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.ConfigLog_File, gridHelper, "LogConfig");
        }
        public void File_Open_ConfigFile()
        {   /*
            //Ram Okuma Dosyasinin yuklenmesi open dan
            */
            if (LOG_OnOffCheck() == false) return; // zaten uyari var


            String str = "";
            if (Okyanus.Variables.OpenLogFileatInit == true) // ilk acilis okumasi
            {
                Okyanus.Variables.OpenLogFileatInit = false;
                if (!(Directory.Exists(Okyanus.Definitions.WorkDrive +  Okyanus.Definitions.Config_Directory)))
                { // directory yoksa mydocumentstan basliyor
                    try { }
                    catch (IOException)
                    {// MessageBox.Show("Directory Error !!");
                        return;
                    }// e->GetType()->Name ;
                }
                str = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.ConfigLog_File + ".csv";
            }
            else str = File_OpenCommonFileDialog();

            Okyanus.Variables.LogFileName = "";
            if (str == Okyanus.Definitions.FileForgive) return;
            if (str == Okyanus.Definitions.ErrorWarning) { MessageBox.Show("File Read Error"); return; }
            Okyanus.Variables.LogFileName = str;
            textBox_Log_FileName.Text = "Yuklu Dosya : " + Okyanus.Variables.LogFileName;
            String DataFile = "false";
            try
            {
                StreamReader sr = new StreamReader(str);  // openFileDialog1.FileName dan gelen
                if (sr != null)
                {     // hata dosya aciksa kitleniyor burada
                    DataFile = sr.ReadToEnd();
                }
            }
            catch (IOException)
            {
                /* MessageBox.Show("okuma hatasi"); */
                textBox_Log_FileName.Text = "";
                return;
            }
            if (DataFile == "false") { MessageBox.Show("data yok"); return; }
            string[] csvseparator = { ",", nl, 
                                    GridWork.GridHelper.Variable_Oky1,
                                    GridWork.GridHelper.Variable_Oky2, 
                                    GridWork.GridHelper.Variable_Oky3, 
                                    GridWork.GridHelper.Variable_Oky4, 
                                    GridWork.GridHelper.Variable_Oky5, 
                                    GridWork.GridHelper.Variable_Oky7, 
                                    GridWork.GridHelper.Variable_Oky8, 
                                    GridWork.GridHelper.Variable_Oky9, 
                                    GridWork.GridHelper.Variable_Oky10, 
                                    GridWork.GridHelper.Variable_Oky11, 
                                };
            string[] words = DataFile.Split(csvseparator, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 10) // en az bir kolon olmali
            { // tam 6x10 luk olmali tablo 
                MessageBox.Show("Hatali yada eksik tablo girisi !!"); return;
            }
            Okyanus.Variables.RawIndex = (byte)(words.Length / 10); //
            if (Okyanus.Variables.RawIndex > Okyanus.Definitions.MaxConfigLine) Okyanus.Variables.RawIndex = Okyanus.Definitions.MaxConfigLine; // max satir sayisi
            textBoxss_debug.Text = "";

            textBoxss_debug.Text = Okyanus.Variables.RawIndex.ToString() + " / " + words.Length.ToString();

            GridHelper gridHelper = new GridHelper(dt);
            DataTable dataElements = gridHelper.GetDataTable();
            int j = 0;
            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                DataRow dr = dataElements.NewRow();
                j = i * 10;
                dr[0] = words[j];
                dr[1] = words[j + 1];
                dr[2] = words[j + 2];
                dr[3] = words[j + 3];
                dr[4] = words[j + 4];
                dr[5] = words[j + 5];
                dr[6] = words[j + 6];
                dr[7] = words[j + 7];
                dr[8] = words[j + 8];
                dr[9] = words[j + 9];
                dataElements.Rows.Add(dr);  // row eklemek icin kullan
            }
            gridHelper.PrintScreen();

            Int16 parseData;
            for (int i = 0; i < Okyanus.Variables.RawIndex; i++)
            {
                j = i * 10;
                //    Okyanus.Variables.CommandVar_Name[i]=words[j];
                Int16.TryParse(words[j + 1], out parseData);
                Okyanus.Variables.Var_Adress[i] = parseData;

                switch (words[j + 2])
                {
                    default:
                    case "+": Okyanus.Variables.Var_Sign[i] = 1; break;
                    case "+/-": Okyanus.Variables.Var_Sign[i] = 0; break;
                }
                switch (words[j + 3])
                {
                    default:
                    case "1": Okyanus.Variables.Var_Length[i] = 1; break;
                    case "2": Okyanus.Variables.Var_Length[i] = 2; break;
                    case "4": Okyanus.Variables.Var_Length[i] = 4; break;
                    case "Bit Read": Okyanus.Variables.Var_Length[i] = 0; break;
                }
                switch (words[j + 4])
                {
                    default:
                    case "1": Okyanus.Variables.Var_Bit[i] = 1; break;
                    case "2": Okyanus.Variables.Var_Bit[i] = 2; break;
                    case "3": Okyanus.Variables.Var_Bit[i] = 3; break;
                    case "4": Okyanus.Variables.Var_Bit[i] = 4; break;
                    case "5": Okyanus.Variables.Var_Bit[i] = 5; break;
                    case "6": Okyanus.Variables.Var_Bit[i] = 6; break;
                    case "7": Okyanus.Variables.Var_Bit[i] = 7; break;
                    case "8": Okyanus.Variables.Var_Bit[i] = 8; break;
                }
                switch (words[j + 5])
                {
                    default:
                    case "Var": Okyanus.Variables.Var_PlotOnOff[i] = 0; break;
                    case "Yok": Okyanus.Variables.Var_PlotOnOff[i] = 1; break;
                }

                switch (words[j + 6])
                {
                    case Okyanus.Definitions.Mul_0001: Okyanus.Variables.Var_PlotMult[i] = 0; break;
                    case Okyanus.Definitions.Mul_001: Okyanus.Variables.Var_PlotMult[i] = 1; break;
                    case Okyanus.Definitions.Mul_01: Okyanus.Variables.Var_PlotMult[i] = 2; break;
                    default:
                    case Okyanus.Definitions.Mul_1: Okyanus.Variables.Var_PlotMult[i] = 3; break;
                    case Okyanus.Definitions.Mul_10: Okyanus.Variables.Var_PlotMult[i] = 4; break;
                    case Okyanus.Definitions.Mul_100: Okyanus.Variables.Var_PlotMult[i] = 5; break;
                    case Okyanus.Definitions.Mul_1000: Okyanus.Variables.Var_PlotMult[i] = 6; break;
                }

                Int32 Parse32Data;
                Int32.TryParse(words[j + 7], out Parse32Data);
                if (Parse32Data < numericUpDown_ChartMin.Minimum) Parse32Data = (Int16)numericUpDown_ChartMin.Minimum;
                if (Parse32Data > numericUpDown_ChartMin.Maximum) Parse32Data = (Int16)numericUpDown_ChartMin.Maximum;
                Okyanus.Variables.Var_PlotMin[i] = Parse32Data;

                Int32.TryParse(words[j + 8], out Parse32Data);
                if (Parse32Data < numericUpDown_ChartMax.Minimum) Parse32Data = (Int16)numericUpDown_ChartMax.Minimum;
                if (Parse32Data > numericUpDown_ChartMax.Maximum) Parse32Data = (Int16)numericUpDown_ChartMax.Maximum;
                Okyanus.Variables.Var_PlotMax[i] = Parse32Data;

                Int32.TryParse(words[j + 9], out Parse32Data);
                Okyanus.Variables.Var_PlotColour[i] = Parse32Data;
         //       LOG_PopulatetexboxandCheckBoxes();
                Okyanus.Chart.LogPropUpdate = true;
            }
            DATA_Fill_Grid_From_LogConfigArrays();  // 10.05.2014  tum degerleri tabloya okuyup arraye aldiktan sonra ararydakileri tekrar tabloya dok

            //      gridHelper.ImportCSV(str, gridHelper.GetDataTable());


        }
        public void File_Open_CommandFile()
        {   /*
            //Flash Yazma Dosyasinin yuklenmesi open dan
            */
            String str = "";
            if (Okyanus.Variables.OpenCommandFileatInit == true) // ilk acilis okumasi
            {
                Okyanus.Variables.OpenCommandFileatInit = false;
                if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Command_Directory)))
                { // directory yoksa mydocumentstan basliyor
                    try { }
                    catch (IOException)
                    {// MessageBox.Show("Directory Error !!");
                        return;
                    }// e->GetType()->Name ;
                }
                str = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Command_File + ".csv";
            }
            else str = File_OpenCommonFileDialog();
            Okyanus.Variables.CommandFileName = "";

            if (str == Okyanus.Definitions.FileForgive) return;
            if (str == Okyanus.Definitions.ErrorWarning) { MessageBox.Show("File Read Error"); return; }
            Okyanus.Variables.CommandFileName = str;
            textBox_CommandFileName.Text = "Yuklu Dosya : " + Okyanus.Variables.CommandFileName;
            String DataFile = "false";

            try
            {
                StreamReader sr = new StreamReader(str);  // openFileDialog1.FileName dan gelen
                if (sr != null)
                {     // hata dosya aciksa kitleniyor burada
                    DataFile = sr.ReadToEnd();
                }
            }
            catch (IOException)
            {
                /* MessageBox.Show("okuma hatasi"); */
                textBox_CommandFileName.Text = "";
                return;
            }

            if (DataFile == "false") { MessageBox.Show("data yok"); return; }
            string[] csvseparator = { ",", nl, GridWork.GridHelper.Variable_Oky100,GridWork.GridHelper.Variable_Oky101, GridWork.GridHelper.Variable_Oky102, 
                            GridWork.GridHelper.Variable_Oky103, GridWork.GridHelper.Variable_Oky104, GridWork.GridHelper.Variable_Oky105 };

            string[] words = DataFile.Split(csvseparator, StringSplitOptions.RemoveEmptyEntries);
            //    string[] words = DataFile.Split(csvseparator, StringSplitOptions.None);

            if (words.Length != 60)
            { // tam 6x10 luk olmali tablo 
                MessageBox.Show("Hatali yada eksik tablo girisi !!"); return;
            }

            GridHelper gridHelper = new GridHelper(dt_Command);
            DataTable dataElements = gridHelper.GetDataTable4Command_oldType();

            Okyanus.Variables.CommandRawIndex = 10;

            int j = 0;
            for (int i = 0; i < 10; i++)
            {

                DataRow dr = dataElements.NewRow();
                j = i * 6;
                dr[0] = words[j];
                dr[1] = words[j + 1];
                dr[2] = words[j + 2];
                dr[3] = words[j + 3];
                dr[4] = words[j + 4];
                dr[5] = words[j + 5];

                dataElements.Rows.Add(dr);  // row eklemek icin kullan
            }
            gridHelper.PrintScreen();

            Int16 parseData;
            for (int i = 0; i < 10; i++)
            {
                j = i * 6;
                Okyanus.Variables.CommandVar_Name[i] = words[j];
                switch (words[j + 1])
                {
                    default:
                    case Okyanus.Definitions.Komut00: Okyanus.Variables.Var_Command[i] = 0; break;
                    case Okyanus.Definitions.Komut01: Okyanus.Variables.Var_Command[i] = 1; break;
                    case Okyanus.Definitions.Komut02: Okyanus.Variables.Var_Command[i] = 2; break;
                    case Okyanus.Definitions.Komut03: Okyanus.Variables.Var_Command[i] = 3; break;
                    case Okyanus.Definitions.Komut04: Okyanus.Variables.Var_Command[i] = 4; break;
                    case Okyanus.Definitions.Komut05: Okyanus.Variables.Var_Command[i] = 5; break;
                    case Okyanus.Definitions.Komut06: Okyanus.Variables.Var_Command[i] = 6; break;
                    case Okyanus.Definitions.Komut07: Okyanus.Variables.Var_Command[i] = 7; break;
                    case Okyanus.Definitions.Komut08: Okyanus.Variables.Var_Command[i] = 8; break;
                    case Okyanus.Definitions.Komut09: Okyanus.Variables.Var_Command[i] = 9; break;
                    case Okyanus.Definitions.Komut10: Okyanus.Variables.Var_Command[i] = 10; break;
                    case Okyanus.Definitions.Komut11: Okyanus.Variables.Var_Command[i] = 11; break;
                    case Okyanus.Definitions.Komut12: Okyanus.Variables.Var_Command[i] = 12; break;
                    case Okyanus.Definitions.Komut13: Okyanus.Variables.Var_Command[i] = 13; break;
                    case Okyanus.Definitions.Komut14: Okyanus.Variables.Var_Command[i] = 14; break;
                    case Okyanus.Definitions.Komut15: Okyanus.Variables.Var_Command[i] = 15; break;
                    case Okyanus.Definitions.Komut16: Okyanus.Variables.Var_Command[i] = 16; break;
                    case Okyanus.Definitions.Komut17: Okyanus.Variables.Var_Command[i] = 17; break;
                    case Okyanus.Definitions.Komut18: Okyanus.Variables.Var_Command[i] = 18; break;
                    case Okyanus.Definitions.Komut19: Okyanus.Variables.Var_Command[i] = 19; break;
                    case Okyanus.Definitions.Komut20: Okyanus.Variables.Var_Command[i] =20; break;
                    case Okyanus.Definitions.Komut21: Okyanus.Variables.Var_Command[i] =21; break;
                    case Okyanus.Definitions.Komut22: Okyanus.Variables.Var_Command[i] =22; break;
                    case Okyanus.Definitions.Komut23: Okyanus.Variables.Var_Command[i] =23; break;
                    case Okyanus.Definitions.Komut24: Okyanus.Variables.Var_Command[i] =24; break;
                    case Okyanus.Definitions.Komut25: Okyanus.Variables.Var_Command[i] =25; break;
                    case Okyanus.Definitions.Komut26: Okyanus.Variables.Var_Command[i] =26; break;
                    case Okyanus.Definitions.Komut27: Okyanus.Variables.Var_Command[i] =27; break;
                    case Okyanus.Definitions.Komut28: Okyanus.Variables.Var_Command[i] =28; break;
                    case Okyanus.Definitions.Komut29: Okyanus.Variables.Var_Command[i] =29; break;
                    case Okyanus.Definitions.Komut30: Okyanus.Variables.Var_Command[i] =30; break;
                    case Okyanus.Definitions.Komut31: Okyanus.Variables.Var_Command[i] =31; break;
                    case Okyanus.Definitions.Komut32: Okyanus.Variables.Var_Command[i] =32; break;
                    case Okyanus.Definitions.Komut33: Okyanus.Variables.Var_Command[i] =33; break;
                    case Okyanus.Definitions.Komut34: Okyanus.Variables.Var_Command[i] =34; break;
                    case Okyanus.Definitions.Komut35: Okyanus.Variables.Var_Command[i] =35; break;
                    case Okyanus.Definitions.Komut36: Okyanus.Variables.Var_Command[i] =36; break;
                    case Okyanus.Definitions.Komut37: Okyanus.Variables.Var_Command[i] =37; break;
                    case Okyanus.Definitions.Komut38: Okyanus.Variables.Var_Command[i] =38; break;
                    case Okyanus.Definitions.Komut39: Okyanus.Variables.Var_Command[i] =39; break;
                    case Okyanus.Definitions.Komut40: Okyanus.Variables.Var_Command[i] =40; break;
                    case Okyanus.Definitions.Komut41: Okyanus.Variables.Var_Command[i] =41; break;
                    case Okyanus.Definitions.Komut42: Okyanus.Variables.Var_Command[i] =42; break;
                    case Okyanus.Definitions.Komut43: Okyanus.Variables.Var_Command[i] =43; break;
                    case Okyanus.Definitions.Komut44: Okyanus.Variables.Var_Command[i] =44; break;
                    case Okyanus.Definitions.Komut45: Okyanus.Variables.Var_Command[i] =45; break;
                    case Okyanus.Definitions.Komut46: Okyanus.Variables.Var_Command[i] =46; break;
                    case Okyanus.Definitions.Komut47: Okyanus.Variables.Var_Command[i] =47; break;
                    case Okyanus.Definitions.Komut48: Okyanus.Variables.Var_Command[i] =48; break;
                    case Okyanus.Definitions.Komut49: Okyanus.Variables.Var_Command[i] =49; break;

                }

                Int16.TryParse(words[j + 2], out parseData);
                Okyanus.Variables.Var_Parameter1[i] = parseData;
                Int16.TryParse(words[j + 3], out parseData);
                Okyanus.Variables.Var_Parameter2[i] = parseData;
                Int16.TryParse(words[j + 4], out parseData);
                Okyanus.Variables.Var_Parameter3[i] = parseData;
                Int16.TryParse(words[j + 5], out parseData);
                Okyanus.Variables.Var_Parameter4[i] = parseData;
            }
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator: DATA_Fill_Grid_From_FlashCommandArrays();  // 2. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                default:
                    break;
            } 
            //      gridHelper.ImportCSV(str, gridHelper.GetDataTable4Command()); // sadece gride atiyor datay arraye almadi
        }

        public void File_Open_500_FlashCommandSelectFile()
        {
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator:
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2:File_Open_500_FlashCommandFile_Kahve2();
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: File_Open_500_FlashCommandFile_Tost2();
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:
                    break;
                default:
                    break;
            }
        }
        public void File_Save_500_FlashCommandSelectFile()
        {
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator:
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: File_Save_500_FlashCommandFile_Kahve2();
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: File_Save_500_FlashCommandFile_Tost2(); 
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:
                    break;
                default:
                    break;
            } 
        }
        
        public void File_Save_500_FlashCommandFile_Kahve2()
        {
            GridHelper gridHelper500_1 = new GridHelper(dt_500Command);
            File_SaveAsCommon(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.FlashCommand500byte_Kahve2, gridHelper500_1, "Flash500Kahve2");
        }
        public void File_Save_500_FlashCommandFile_Tost2()
        {
            GridHelper gridHelper500_2 = new GridHelper(dt_500Command2);
            File_SaveAsCommon(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.FlashCommand500byte_Tost, gridHelper500_2, "Flash500Tost");
        }

        public void File_Open_500_FlashCommandFile_Kahve2()
        {
            //     return;
            //2. versiyonda Flash Yazma Dosyasinin yuklenmesi open dan

            String str = "";
            if (Okyanus.Variables.OpenCommandFile500atInit == true) // ilk acilis okumasi
            {
                Okyanus.Variables.OpenCommandFile500atInit = false;
                if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Command_Directory)))
                { // directory yoksa mydocumentstan basliyor
                    try { }
                    catch (IOException)
                    {// MessageBox.Show("Directory Error !!");
                        return;
                    }// e->GetType()->Name ;
                }
                str = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.FlashCommand500byte_Kahve2 + ".csv";
            }
            else str = File_OpenCommonFileDialog();
            Okyanus.Variables.FlashCommand500byte_Kahve2 = "";

            if (str == Okyanus.Definitions.FileForgive) return;
            if (str == Okyanus.Definitions.ErrorWarning) { MessageBox.Show("File Read Error"); return; }
            Okyanus.Variables.FlashCommand500byte_Kahve2 = str;
            textBox_500byte1.Text = Okyanus.Definitions.Kahve2Dosyaismi + Okyanus.Variables.FlashCommand500byte_Kahve2;
            String DataFile = "false";

            


            try
            {
                StreamReader sr = new StreamReader(str);  // openFileDialog1.FileName dan gelen
                if (sr != null)
                {     // hata dosya aciksa kitleniyor burada
                    DataFile = sr.ReadToEnd();
                }
            }
            catch (IOException)
            {
                /* MessageBox.Show("okuma hatasi"); */
                textBox_500byte1.Text = "Dosya Okuma Hatasi!";
                return;
            }
            if (DataFile == "false") { MessageBox.Show("data yok"); return; }
            string[] csvseparator = { ",", nl,
                            GridWork.GridHelper.Variable_Oky105,
                            GridWork.GridHelper.Variable_Oky100,
                            GridWork.GridHelper.Variable_Oky101,
                            GridWork.GridHelper.Variable_Oky102,
                            GridWork.GridHelper.Variable_Oky103,
                            GridWork.GridHelper.Variable_Oky104};

            string[] words = DataFile.Split(csvseparator, StringSplitOptions.RemoveEmptyEntries);
            //    string[] words = DataFile.Split(csvseparator, StringSplitOptions.None);
            //     textBox_500byte.Text = words.Length.ToString();
            if (words.Length != 300) // 10x25 luk oldu 50->250
            { // tam 5x10 luk olmali tablo 
              //     MessageBox.Show("Flash yazma için Hatali yada eksik tablo girisi !!");
                MessageBox.Show("Kahve icin hatali dosya okuma : " + words.Length.ToString());

                // MessageBox.Show("Flash yazma için Hatali yada eksik tablo girisi 50 satir 5 sutun olmali !!");

                return;
            }

            GridHelper gridHelper500 = new GridHelper(dt_500Command);
            DataTable dataElements = gridHelper500.GetDataTable4_500Byte50Line6Column();

            //          Okyanus.Variables.CommandRawIndex500byte = 50; // 10->25
            int j = 0;
            for (int i = 0; i < 50; i++)
            {
                DataRow dr = dataElements.NewRow();
                j = i * 6;//5->10->5
                dr[0] = words[j];
                switch (words[j + 1])
                {
                    case "0": words[j + 1] = Okyanus.Definitions.Komut00; break;
                    case "1": words[j + 1] = Okyanus.Definitions.Komut01; break;
                    case "2": words[j + 1] = Okyanus.Definitions.Komut02; break;
                    case "3": words[j + 1] = Okyanus.Definitions.Komut03; break;
                    case "4": words[j + 1] = Okyanus.Definitions.Komut04; break;
                    case "5": words[j + 1] = Okyanus.Definitions.Komut05; break;
                    case "6": words[j + 1] = Okyanus.Definitions.Komut06; break;
                    case "7": words[j + 1] = Okyanus.Definitions.Komut07; break;
                    case "8": words[j + 1] = Okyanus.Definitions.Komut08; break;
                    case "9": words[j + 1] = Okyanus.Definitions.Komut09; break;
                    case "10": words[j + 1] = Okyanus.Definitions.Komut10; break;
                    case "11": words[j + 1] = Okyanus.Definitions.Komut11; break;
                    case "12": words[j + 1] = Okyanus.Definitions.Komut12; break;
                    case "13": words[j + 1] = Okyanus.Definitions.Komut13; break;
                    case "14": words[j + 1] = Okyanus.Definitions.Komut14; break;
                    case "15": words[j + 1] = Okyanus.Definitions.Komut15; break;
                    case "16": words[j + 1] = Okyanus.Definitions.Komut16; break;
                    case "17": words[j + 1] = Okyanus.Definitions.Komut17; break;
                    case "18": words[j + 1] = Okyanus.Definitions.Komut18; break;
                    case "19": words[j + 1] = Okyanus.Definitions.Komut19; break;
                    case "20": words[j + 1] = Okyanus.Definitions.Komut20; break;
                    case "21": words[j + 1] = Okyanus.Definitions.Komut21; break;
                    case "22": words[j + 1] = Okyanus.Definitions.Komut22; break;
                    case "23": words[j + 1] = Okyanus.Definitions.Komut23; break;
                    case "24": words[j + 1] = Okyanus.Definitions.Komut24; break;
                    case "25": words[j + 1] = Okyanus.Definitions.Komut25; break;
                    case "26": words[j + 1] = Okyanus.Definitions.Komut26; break;
                    case "27": words[j + 1] = Okyanus.Definitions.Komut27; break;
                    case "28": words[j + 1] = Okyanus.Definitions.Komut28; break;
                    case "29": words[j + 1] = Okyanus.Definitions.Komut29; break;
                    case "30": words[j + 1] = Okyanus.Definitions.Komut30; break;
                    case "31": words[j + 1] = Okyanus.Definitions.Komut31; break;
                    case "32": words[j + 1] = Okyanus.Definitions.Komut32; break;
                    case "33": words[j + 1] = Okyanus.Definitions.Komut33; break;
                    case "34": words[j + 1] = Okyanus.Definitions.Komut34; break;
                    case "35": words[j + 1] = Okyanus.Definitions.Komut35; break;
                    case "36": words[j + 1] = Okyanus.Definitions.Komut36; break;
                    case "37": words[j + 1] = Okyanus.Definitions.Komut37; break;
                    case "38": words[j + 1] = Okyanus.Definitions.Komut38; break;
                    case "39": words[j + 1] = Okyanus.Definitions.Komut39; break;
                    case "40": words[j + 1] = Okyanus.Definitions.Komut40; break;
                    case "41": words[j + 1] = Okyanus.Definitions.Komut41; break;
                    case "42": words[j + 1] = Okyanus.Definitions.Komut42; break;
                    case "43": words[j + 1] = Okyanus.Definitions.Komut43; break;
                    case "44": words[j + 1] = Okyanus.Definitions.Komut44; break;
                    case "45": words[j + 1] = Okyanus.Definitions.Komut45; break;
                    case "46": words[j + 1] = Okyanus.Definitions.Komut46; break;
                    case "47": words[j + 1] = Okyanus.Definitions.Komut47; break;
                    case "48": words[j + 1] = Okyanus.Definitions.Komut48; break;
                    case "49": words[j + 1] = Okyanus.Definitions.Komut49; break;
                    default: break;
                }
                dr[1] = words[j + 1];
                dr[2] = words[j + 2];
                dr[3] = words[j + 3];
                dr[4] = words[j + 4];
                dr[5] = words[j + 5];// artti bes tane row
                dataElements.Rows.Add(dr);  // row eklemek icin kullan
            }

            UInt16 parseData;
            j = 0;
            for (int i = 0; i < (250); i++)
            {

                UInt16.TryParse(words[i], out parseData);
                Okyanus.Variables.SendBuffer[i] = parseData;

            }
            gridHelper500.PrintScreen();
        }





        public void File_Open_500_FlashCommandFile_Tost2()
        {
            //     return;
            //2. versiyonda Flash Yazma Dosyasinin yuklenmesi open dan
            
            String str = "";
            if (Okyanus.Variables.OpenCommandFile500atInit2 == true) // ilk acilis okumasi
            {
                Okyanus.Variables.OpenCommandFile500atInit2 = false;
                if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Command_Directory)))
                { // directory yoksa mydocumentstan basliyor
                    try { }
                    catch (IOException)
                    {// MessageBox.Show("Directory Error !!");
                        return;
                    }// e->GetType()->Name ;
                }
                str = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.FlashCommand500byte_Tost + ".csv";
            }
            else str = File_OpenCommonFileDialog();
            Okyanus.Variables.FlashCommand500byte_Tost = "";

            if (str == Okyanus.Definitions.FileForgive) return;
            if (str == Okyanus.Definitions.ErrorWarning) { MessageBox.Show("File Read Error"); return; }
            Okyanus.Variables.FlashCommand500byte_Tost = str;
            textBox_500byte2.Text = Okyanus.Definitions.TostDosyaismi + Okyanus.Variables.FlashCommand500byte_Tost;
            String DataFile = "false";

            try
            {
                StreamReader sr = new StreamReader(str);  // openFileDialog1.FileName dan gelen
                if (sr != null)
                {     // hata dosya aciksa kitleniyor burada
                    DataFile = sr.ReadToEnd();
                }
            }
            catch (IOException)
            {
                /* MessageBox.Show("okuma hatasi"); */
                textBox_500byte2.Text = "Dosya Okuma Hatasi!";
                return;
            }
            if (DataFile == "false") { MessageBox.Show("data yok"); return; }
            string[] csvseparator = { ",", nl, 
                            GridWork.GridHelper.Variable_Oky105,
                            GridWork.GridHelper.Variable_Oky100, 
                            GridWork.GridHelper.Variable_Oky101,
                            GridWork.GridHelper.Variable_Oky102,
                            GridWork.GridHelper.Variable_Oky103,
                            GridWork.GridHelper.Variable_Oky104};

            string[] words = DataFile.Split(csvseparator, StringSplitOptions.RemoveEmptyEntries);
            //    string[] words = DataFile.Split(csvseparator, StringSplitOptions.None);
            //     textBox_500byte.Text = words.Length.ToString();
            if (words.Length != 300) // 10x25 luk oldu 50->250
            { // tam 5x10 luk olmali tablo 
           //     MessageBox.Show("Flash yazma için Hatali yada eksik tablo girisi !!");
                MessageBox.Show("Tost icin hatali dosya okuma : " + words.Length.ToString());

               // MessageBox.Show("Flash yazma için Hatali yada eksik tablo girisi 50 satir 5 sutun olmali !!");

                return;
            }

            GridHelper gridHelper500 = new GridHelper(dt_500Command2);
            DataTable dataElements = gridHelper500.GetDataTable4_500Byte50Line6Column();

  //          Okyanus.Variables.CommandRawIndex500byte = 50; // 10->25
            int j = 0;
            for (int i = 0; i < 50; i++)
            {
                DataRow dr = dataElements.NewRow();
                j = i * 6;//5->10->5
                dr[0] = words[j];              
                switch (words[j + 1])
                {
                    case "0": words[j + 1] = Okyanus.Definitions.Komut00;break;
                    case "1": words[j + 1] = Okyanus.Definitions.Komut01;break;
                    case "2": words[j + 1] = Okyanus.Definitions.Komut02;break;
                    case "3": words[j + 1] = Okyanus.Definitions.Komut03;break;
                    case "4": words[j + 1] = Okyanus.Definitions.Komut04;break;
                    case "5": words[j + 1] = Okyanus.Definitions.Komut05;break;
                    case "6": words[j + 1] = Okyanus.Definitions.Komut06;break;
                    case "7": words[j + 1] = Okyanus.Definitions.Komut07;break;
                    case "8": words[j + 1] = Okyanus.Definitions.Komut08;break;
                    case "9": words[j + 1] = Okyanus.Definitions.Komut09;break;
                    case "10": words[j + 1] = Okyanus.Definitions.Komut10;break;
                    case "11": words[j + 1] = Okyanus.Definitions.Komut11;break;
                    case "12": words[j + 1] = Okyanus.Definitions.Komut12;break;
                    case "13": words[j + 1] = Okyanus.Definitions.Komut13;break;
                    case "14": words[j + 1] = Okyanus.Definitions.Komut14;break;
                    case "15": words[j + 1] = Okyanus.Definitions.Komut15;break;
                    case "16": words[j + 1] = Okyanus.Definitions.Komut16;break;
                    case "17": words[j + 1] = Okyanus.Definitions.Komut17;break;
                    case "18": words[j + 1] = Okyanus.Definitions.Komut18;break;
                    case "19": words[j + 1] = Okyanus.Definitions.Komut19;break;
                    case "20": words[j + 1] = Okyanus.Definitions.Komut20; break;
                    case "21": words[j + 1] = Okyanus.Definitions.Komut21; break;
                    case "22": words[j + 1] = Okyanus.Definitions.Komut22; break;
                    case "23": words[j + 1] = Okyanus.Definitions.Komut23; break;
                    case "24": words[j + 1] = Okyanus.Definitions.Komut24; break;
                    case "25": words[j + 1] = Okyanus.Definitions.Komut25; break;
                    case "26": words[j + 1] = Okyanus.Definitions.Komut26; break;
                    case "27": words[j + 1] = Okyanus.Definitions.Komut27; break;
                    case "28": words[j + 1] = Okyanus.Definitions.Komut28; break;
                    case "29": words[j + 1] = Okyanus.Definitions.Komut29; break;
                    case "30": words[j + 1] = Okyanus.Definitions.Komut30; break;
                    case "31": words[j + 1] = Okyanus.Definitions.Komut31; break;
                    case "32": words[j + 1] = Okyanus.Definitions.Komut32; break;
                    case "33": words[j + 1] = Okyanus.Definitions.Komut33; break;
                    case "34": words[j + 1] = Okyanus.Definitions.Komut34; break;
                    case "35": words[j + 1] = Okyanus.Definitions.Komut35; break;
                    case "36": words[j + 1] = Okyanus.Definitions.Komut36; break;
                    case "37": words[j + 1] = Okyanus.Definitions.Komut37; break;
                    case "38": words[j + 1] = Okyanus.Definitions.Komut38; break;
                    case "39": words[j + 1] = Okyanus.Definitions.Komut39; break;
                    case "40": words[j + 1] = Okyanus.Definitions.Komut40; break;
                    case "41": words[j + 1] = Okyanus.Definitions.Komut41; break;
                    case "42": words[j + 1] = Okyanus.Definitions.Komut42; break;
                    case "43": words[j + 1] = Okyanus.Definitions.Komut43; break;
                    case "44": words[j + 1] = Okyanus.Definitions.Komut44; break;
                    case "45": words[j + 1] = Okyanus.Definitions.Komut45; break;
                    case "46": words[j + 1] = Okyanus.Definitions.Komut46; break;
                    case "47": words[j + 1] = Okyanus.Definitions.Komut47; break;
                    case "48": words[j + 1] = Okyanus.Definitions.Komut48; break;
                    case "49": words[j + 1] = Okyanus.Definitions.Komut49; break;
                    default:break;
                }
                dr[1] = words[j + 1];
                dr[2] = words[j + 2];
                dr[3] = words[j + 3];
                dr[4] = words[j + 4];
                dr[5] = words[j + 5];// artti bes tane row
                dataElements.Rows.Add(dr);  // row eklemek icin kullan
            }

            UInt16 parseData;
            j = 0;
            for (int i = 0; i < (250); i++)
            {

                UInt16.TryParse(words[i], out parseData);
                Okyanus.Variables.SendBuffer[i] = parseData;

            }
            gridHelper500.PrintScreen();
        }
        public void File_Open_FlashCommandFile()
        {   /*
            //2. versiyonda Flash Yazma Dosyasinin yuklenmesi open dan
            */
            String str = "";
            if (Okyanus.Variables.OpenCommandFlashFileatInit == true) // ilk acilis okumasi
            {
                Okyanus.Variables.OpenCommandFlashFileatInit = false;
                if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Command_Directory)))
                { // directory yoksa mydocumentstan basliyor
                    try { }
                    catch (IOException)
                    {// MessageBox.Show("Directory Error !!");
                        return;
                    }// e->GetType()->Name ;
                }
                str = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.FlashCommand_File + ".csv";
            }
               
            else str = File_OpenCommonFileDialog();
            Okyanus.Variables.CommandFileName = "";

            if (str == Okyanus.Definitions.FileForgive) return;
            if (str == Okyanus.Definitions.ErrorWarning) { MessageBox.Show("File Read Error"); return; }
            Okyanus.Variables.CommandFileName = str;
            textBox_CommandFileName.Text = "Yuklu Dosya : " + Okyanus.Variables.CommandFileName;
            String DataFile = "false";

            try
            {
                StreamReader sr = new StreamReader(str);  // openFileDialog1.FileName dan gelen
                if (sr != null)
                {     // hata dosya aciksa kitleniyor burada
                    DataFile = sr.ReadToEnd();
                }
            }
            catch (IOException)
            {
                /* MessageBox.Show("okuma hatasi"); */
                textBox_CommandFileName.Text = "Okuma Hatasi!!";
                return;
            }

            if (DataFile == "false") { MessageBox.Show("data yok"); return; }
            string[] csvseparator = { ",", nl, GridWork.GridHelper.Variable_FC_Oky100,GridWork.GridHelper.Variable_FC_Oky101, GridWork.GridHelper.Variable_FC_Oky102, 
                            GridWork.GridHelper.Variable_FC_Oky103, GridWork.GridHelper.Variable_FC_Oky104};
            string[] words = DataFile.Split(csvseparator, StringSplitOptions.RemoveEmptyEntries);
            //    string[] words = DataFile.Split(csvseparator, StringSplitOptions.None);

            if (words.Length != 50)
            { // tam 5x10 luk olmali tablo 
                MessageBox.Show("Flash yazma için Hatali yada eksik tablo girisi 5 satir 10 sutun olmali !!"); return;
            }

            GridHelper gridHelper = new GridHelper(dt_Command);
            DataTable dataElements = gridHelper.GetDataTable4Command();

            Okyanus.Variables.CommandRawIndex = 10;

            int j = 0;
            for (int i = 0; i < 10; i++)
            {

                DataRow dr = dataElements.NewRow();
                j = i * 5;
                dr[0] = words[j];
                dr[1] = words[j + 1];
                dr[2] = words[j + 2];
                dr[3] = words[j + 3];
                dr[4] = words[j + 4];       

                dataElements.Rows.Add(dr);  // row eklemek icin kullan
            }
            gridHelper.PrintScreen();

            UInt16 parseData;
            for (int i = 0; i < 10; i++)
            {
                j = i * 5;
                UInt16.TryParse(words[j + 0], out parseData);
                Okyanus.Variables.Var_Command[i] = parseData;
                UInt16.TryParse(words[j + 1], out parseData);
                Okyanus.Variables.Var_Parameter1[i] = parseData;
                UInt16.TryParse(words[j + 2], out parseData);
                Okyanus.Variables.Var_Parameter2[i] = parseData;
                UInt16.TryParse(words[j + 3], out parseData);
                Okyanus.Variables.Var_Parameter3[i] = parseData;
                UInt16.TryParse(words[j + 4], out parseData);
                Okyanus.Variables.Var_Parameter4[i] = parseData;


                switch(i){
                    default:
                        break;
                    case 0:
                        numericUpDown_Par0.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par1.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par2.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par3.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par4.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 1:
                        numericUpDown_Par5.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par6.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par7.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par8.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par9.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 2:
                        numericUpDown_Par10.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par11.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par12.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par13.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par14.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 3:
                        numericUpDown_Par15.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par16.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par17.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par18.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par19.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 4:
                        numericUpDown_Par20.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par21.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par22.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par23.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par24.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 5:
                        numericUpDown_Par25.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par26.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par27.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par28.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par29.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 6:
                        numericUpDown_Par30.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par31.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par32.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par33.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par34.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 7:
                        numericUpDown_Par35.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par36.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par37.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par38.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par39.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 8:
                        numericUpDown_Par40.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par41.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par42.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par43.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par44.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                    case 9:
                        numericUpDown_Par45.Value = Okyanus.Variables.Var_Command[i];
                        numericUpDown_Par46.Value = Okyanus.Variables.Var_Parameter1[i];
                        numericUpDown_Par47.Value = Okyanus.Variables.Var_Parameter2[i];
                        numericUpDown_Par48.Value = Okyanus.Variables.Var_Parameter3[i];
                        numericUpDown_Par49.Value = Okyanus.Variables.Var_Parameter4[i];
                        break;
                }

            }
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator: DATA_Fill_Grid_From_FlashCommandArrays();  // 2. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1: DATA_Fill_Grid_From_CommandArrays();   // combodan eski tip okuma
                    break;
                default:
                    break;
            } 
            //      gridHelper.ImportCSV(str, gridHelper.GetDataTable4Command()); // sadece gride atiyor datay arraye almadi
        }
        public void File_SaveAsCommon(String FileName, GridHelper gridHelper, String Type)
        {

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory)))
            { // directory yoksa mydocumentstan basliyor
                saveFileDialog1.InitialDirectory = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory;
            }
  
        //    saveFileDialog1.Title = "Select a File to Save Config Data";
            saveFileDialog1.Title = "Bir Dosya Ismi Secerek Kaydedin ";
            if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory)))
            { // directory yoksa mydocumentstan basliyor
                try { Directory.CreateDirectory(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory); }
           //     catch (IOException) { MessageBox.Show("Directory Error !!"); return; }// e->GetType()->Name ;
                catch (IOException) { MessageBox.Show("Directory Error !!");}// e->GetType()->Name ;
            }
            saveFileDialog1.FileName = FileName + ".csv";
            saveFileDialog1.Filter = "Txt files (*.txt)|*.txt | Csv files (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {     // hata dosya aciksa kitleniyor burada

                        myStream.Close();
                    }
                    gridHelper.ExportCSV(saveFileDialog1.FileName);
                    
               //     gridHelper.GetGridValues
                }
                catch (IOException)
                {
                    MessageBox.Show("File Error !!" + nl + "Close File If Open");
                    return;
                }
                if (Type == "LogConfig") textBox_Log_FileName.Text = "Yuklu Dosya : " + saveFileDialog1.FileName;
                if (Type == "Command") textBox_CommandFileName.Text = "Yuklu Dosya : " + saveFileDialog1.FileName;
                if (Type == "Flash500Tost") textBox_500byte2.Text = Okyanus.Definitions.TostDosyaismi + saveFileDialog1.FileName;
                if (Type == "Flash500Kahve2") textBox_500byte1.Text = Okyanus.Definitions.Kahve2Dosyaismi + saveFileDialog1.FileName;
            }

            
        }
        public String File_OpenCommonFileDialog()
        {
            //    StreamReader myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            openFileDialog1.InitialDirectory = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory;
            openFileDialog1.Title = "Select a File to Open Config Data";
            if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory)))
            { // directory yoksa mydocumentstan basliyor
                try { }
                catch (IOException) { MessageBox.Show("Directory Error !!"); return Okyanus.Definitions.ErrorWarning; }// e->GetType()->Name ;
            }
            //   String str = LOG_GetTime() + ".csv";
            //    String str = "LogConfig" + ".csv";
            //     openFileDialog1.FileName = FileName + ".csv";

            openFileDialog1.Filter = "Txt files (*.txt)|*.txt | Csv files (*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    return openFileDialog1.FileName;
                }
                catch
                {

                    return Okyanus.Definitions.ErrorWarning;
                }
            }
            else
            {
                return Okyanus.Definitions.FileForgive;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

      //       Form tempChild = (Form)this.MdiChildren[x];
            EndApp form = new EndApp();
        //    form.MdiParent = this;
      //     form.ShowDialog();
            form.BackColor = CustomColor();
            form.Show();
        //    form.
        //    form.Show();
        }


        private void saveAsConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File_Save_ConfigFile();
        }
        private void SaveAsCommandtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveFlashFileCommon();
        }
        public void SaveFlashFileCommon()
        {
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator: File_Save_FlashCommandFile();  //  2. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: File_Save_CommandFile();  //  1. versiyon
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: File_Save_CommandFile();  //  1. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1: File_Save_CommandFile();  //  1. versiyon
                    break;
                default:
                    break;
            }  


        }


        public void OpenFlashFileCommon(){
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator: File_Open_FlashCommandFile(); // 2. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: File_Open_CommandFile();    // 1. versiyon
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: File_Open_CommandFile();    // 1. versiyon
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1: File_Open_CommandFile();    // 1. versiyon
                    break;
                default:
                    break;
            } 


        }
        private void OpenCommandtoolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenFlashFileCommon();
        }


        private void dosyadanFlashTablosuAcmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File_Open_500_FlashCommandSelectFile();

        }
        private void flashTablosundanDosyayaKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File_Save_500_FlashCommandSelectFile();
        }

        private void openConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File_Open_ConfigFile();
        }
        private void stopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LOG_StopLog();
        }
        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LOG_ViewLogFile();
        }
        private void fileSaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LOG_SaveLogFile();
        }
        private void button_CommandAdd_Click(object sender, EventArgs e)
        {
      //      DATA_CommandArray_Table_Add(); // KAHVE MAKINASI 1 SATIR EKLEME
   //         DATA_CommandFlashArray_Table_Add();
            DATA_CommandFlashArray_Table_UpdateAll();
        }
        private void button_CommadDelete_Click(object sender, EventArgs e)
        {
            //    DATA_CommandArray_Table_Delete(); // KAHVE MAKINASI 1 SATIR SILME
        }
        private void button_CommandStart_Click(object sender, EventArgs e)
        {
            // flash a yaz butonu
            if (Okyanus.Variables.CommandRawIndex != Okyanus.Definitions.MaxCommandLine)
            {
                MessageBox.Show("Fill All Data To Start Send");
                return;
            }
            WriteToFlashProcedure(10000);
            writeVerify = 12;           // 28.05.2015
            Flash500ByteWriteText = false;
        }


        String Flash500ButtonReadMessage = "Flash tan 500" + nl + " Okunuyor !";
        String Flash500ButtonDefaultMessage = "Flash tan 500" + nl + "    Oku!";
        String Flash500ButtonReadSuccessMessage = "Flash tan 500" + nl + " Okundu!";



        private void Flas500ReadButtonProcedure()
        {
            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator:
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2:
                    Okyanus.Variables.FlashCommand500byte_Kahve2 = " ";
                    textBox_500byte1.Text = Okyanus.Definitions.Kahve2Dosyaismi + Okyanus.Variables.FlashCommand500byte_Kahve2;
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2:
                    Okyanus.Variables.FlashCommand500byte_Tost = " ";
                    textBox_500byte2.Text = Okyanus.Definitions.TostDosyaismi + Okyanus.Variables.FlashCommand500byte_Tost;

                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:
                    break;
                default:
                    break;
            }


            button500byteOku.Text = Flash500ButtonReadMessage;
            button500byteOku.ForeColor = Color.Red;
            button500byteOku2.Text = Flash500ButtonReadMessage;
            button500byteOku2.ForeColor = Color.Red;    


            Flash500ByteWriteText = false;
            ReadFlash500RequestProcedure(10000);
            writeVerify = 0;      // yok      // 28.05.2015


        }
        public bool Update500byteSendBuffersV1(String FileName) // KAhve2 icin
        {
            GridHelper gridHelper500 = new GridHelper(dt_500Command);

            if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory)))
            { // directory yoksa mydocumentstan basliyor
                try { Directory.CreateDirectory(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory); }
                //     catch (IOException) { MessageBox.Show("Directory Error !!"); return; }// e->GetType()->Name ;
                catch (IOException) { MessageBox.Show("Directory Error !!"); return false; }// e->GetType()->Name ;
            }
            FileName = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory + "\\" + FileName;
            String DataFile = "false";
            try
            {
                gridHelper500.ExportCSV(FileName);// dosyayi kaydet

                StreamReader sr = new StreamReader(FileName);  // // dosyayi okuma hazirligi
                if (sr != null)
                {     // oku
                    DataFile = sr.ReadToEnd();
                }
                sr.Close();
                DataTable dataElements = gridHelper500.GetDataTable4_500Byte50Line6Column();
                string[] csvseparator = { ",", nl,
                            GridWork.GridHelper.Variable_Oky100,
                            GridWork.GridHelper.Variable_Oky101,
                            GridWork.GridHelper.Variable_Oky102,
                            GridWork.GridHelper.Variable_Oky103,
                            GridWork.GridHelper.Variable_Oky104,
                            GridWork.GridHelper.Variable_Oky105,
               };

                string[] words = DataFile.Split(csvseparator, StringSplitOptions.RemoveEmptyEntries);
                //     string[] words = DataFile.Split(); 
                if (words.Count() != 300)
                { // toplam 250 eleman olmasi gerekiyor!!!
                    MessageBox.Show("Temp. File  Error !!" + nl + " Grid Updated?" + nl + FileName + nl + words.Count());

                    return false;
                }
                int j = 0;
                for (int i = 0; i < 50; i++)
                {
                    DataRow dr = dataElements.NewRow();
                    j = i * 6;//5->10                   
                    dr[0] = words[j];
                    words[j + 1] = GetBack_NumberfromCommands(words[j + 1]);
                    dr[1] = words[j + 1];
                    dr[2] = words[j + 2];
                    dr[3] = words[j + 3];
                    dr[4] = words[j + 4];
                    dr[5] = words[j + 5];// artti bes tane row
                    dataElements.Rows.Add(dr);  // row eklemek icin kullan
                }
                UInt16 parseData;
                j = 0;
                for (int i = 0; i < (25 * 10); i++)
                {
                    UInt16.TryParse(words[i + (i / 5) + 1], out parseData);
                    Okyanus.Variables.SendBuffer[i] = parseData;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File Error !!" + nl + "Close File If Open");
                return false;
            }
            if ((File.Exists(FileName)))
            {
                File.Delete(FileName);
            }
            return true;
        }
        public bool Update500byteSendBuffersV2(String FileName) // Tost2 icin
        {
            GridHelper gridHelper500 = new GridHelper(dt_500Command2);

            if (!(Directory.Exists(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory)))
            { // directory yoksa mydocumentstan basliyor
                try { Directory.CreateDirectory(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory); }
                //     catch (IOException) { MessageBox.Show("Directory Error !!"); return; }// e->GetType()->Name ;
                catch (IOException) { MessageBox.Show("Directory Error !!"); return false; }// e->GetType()->Name ;
            }
            FileName = Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Config_Directory + "\\" + FileName;
            String DataFile = "false";
            try
            {
                gridHelper500.ExportCSV(FileName);// dosyayi kaydet

                StreamReader sr = new StreamReader(FileName);  // // dosyayi okuma hazirligi
                if (sr != null)
                {     // oku
                    DataFile = sr.ReadToEnd();
                }
                sr.Close();
                DataTable dataElements = gridHelper500.GetDataTable4_500Byte50Line6Column();
                string[] csvseparator = { ",", nl, 
                            GridWork.GridHelper.Variable_Oky100, 
                            GridWork.GridHelper.Variable_Oky101,
                            GridWork.GridHelper.Variable_Oky102,
                            GridWork.GridHelper.Variable_Oky103,
                            GridWork.GridHelper.Variable_Oky104,
                            GridWork.GridHelper.Variable_Oky105,                                                
               };

                string[] words = DataFile.Split(csvseparator, StringSplitOptions.RemoveEmptyEntries);
                //     string[] words = DataFile.Split(); 
                if (words.Count() != 300)
                { // toplam 250 eleman olmasi gerekiyor!!!
                    MessageBox.Show("Temp. File  Error !!" + nl + " Grid Updated?" + nl + FileName +nl+ words.Count());

                    return false;
                }
                int j = 0;
                for (int i = 0; i < 50; i++)
                {
                    DataRow dr = dataElements.NewRow();
                    j = i * 6;//5->10                   
                    dr[0] = words[j];                   
                    words[j + 1] = GetBack_NumberfromCommands(words[j + 1]);
                    dr[1] = words[j + 1];
                    dr[2] = words[j + 2];
                    dr[3] = words[j + 3];
                    dr[4] = words[j + 4];
                    dr[5] = words[j + 5];// artti bes tane row
                    dataElements.Rows.Add(dr);  // row eklemek icin kullan
                }
                UInt16 parseData;
                j = 0;
                for (int i = 0; i < (25 * 10); i++)
                {
                    UInt16.TryParse(words[i + (i/5) +1], out parseData);
                    Okyanus.Variables.SendBuffer[i] = parseData;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File Error !!" + nl + "Close File If Open");
                return false;
            }
            if ((File.Exists(FileName)))
            {
                File.Delete(FileName);
            }
            return true;
        }
        public string GetBack_NumberfromCommands(string str)
        {
            switch (str)
            {
                case Okyanus.Definitions.Komut00: str = "0";break;
                case Okyanus.Definitions.Komut01: str = "1";break;
                case Okyanus.Definitions.Komut02: str = "2";break;
                case Okyanus.Definitions.Komut03: str = "3";break;
                case Okyanus.Definitions.Komut04: str = "4";break;
                case Okyanus.Definitions.Komut05: str = "5";break;
                case Okyanus.Definitions.Komut06: str = "6";break;
                case Okyanus.Definitions.Komut07: str = "7";break;
                case Okyanus.Definitions.Komut08: str = "8";break;
                case Okyanus.Definitions.Komut09: str = "9";break;
                case Okyanus.Definitions.Komut10: str = "10";break;
                case Okyanus.Definitions.Komut11: str = "11";break;
                case Okyanus.Definitions.Komut12: str = "12";break;
                case Okyanus.Definitions.Komut13: str = "13";break;
                case Okyanus.Definitions.Komut14: str = "14";break;
                case Okyanus.Definitions.Komut15: str = "15";break;
                case Okyanus.Definitions.Komut16: str = "16";break;
                case Okyanus.Definitions.Komut17: str = "17";break;
                case Okyanus.Definitions.Komut18: str = "18";break;
                case Okyanus.Definitions.Komut19: str = "19";break;
                case Okyanus.Definitions.Komut20: str = "20"; break;
                case Okyanus.Definitions.Komut21: str = "21"; break;
                case Okyanus.Definitions.Komut22: str = "22"; break;
                case Okyanus.Definitions.Komut23: str = "23"; break;
                case Okyanus.Definitions.Komut24: str = "24"; break;
                case Okyanus.Definitions.Komut25: str = "25"; break;
                case Okyanus.Definitions.Komut26: str = "26"; break;
                case Okyanus.Definitions.Komut27: str = "27"; break;
                case Okyanus.Definitions.Komut28: str = "28"; break;
                case Okyanus.Definitions.Komut29: str = "29"; break;
                case Okyanus.Definitions.Komut30: str = "30"; break;
                case Okyanus.Definitions.Komut31: str = "31"; break;
                case Okyanus.Definitions.Komut32: str = "32"; break;
                case Okyanus.Definitions.Komut33: str = "33"; break;
                case Okyanus.Definitions.Komut34: str = "34"; break;
                case Okyanus.Definitions.Komut35: str = "35"; break;
                case Okyanus.Definitions.Komut36: str = "36"; break;
                case Okyanus.Definitions.Komut37: str = "37"; break;
                case Okyanus.Definitions.Komut38: str = "38"; break;
                case Okyanus.Definitions.Komut39: str = "39"; break;
                case Okyanus.Definitions.Komut40: str = "40"; break;
                case Okyanus.Definitions.Komut41: str = "41"; break;
                case Okyanus.Definitions.Komut42: str = "42"; break;
                case Okyanus.Definitions.Komut43: str = "43"; break;
                case Okyanus.Definitions.Komut44: str = "44"; break;
                case Okyanus.Definitions.Komut45: str = "45"; break;
                case Okyanus.Definitions.Komut46: str = "46"; break;
                case Okyanus.Definitions.Komut47: str = "47"; break;
                case Okyanus.Definitions.Komut48: str = "48"; break;
                case Okyanus.Definitions.Komut49: str = "49"; break;
                default:break;
            }
            return str;
        }



        private void Flas500WriteButtonProcedure()
        {
            // flash 500 yaz butonu
  /*          if (Okyanus.Variables.CommandRawIndex != Okyanus.Definitions.MaxCommandLine)
            {
                MessageBox.Show("Fill All Data To Start Send");
                return;
            }
*/

            Okyanus.DebugVar.Write500_1 = 0;
            Okyanus.DebugVar.Write500_2 = 0;
            Okyanus.DebugVar.Write500_3 = 0;
            Okyanus.DebugVar.Write500_4 = 0;
            Okyanus.DebugVar.Write500_5 = 0;
            Okyanus.DebugVar.Write500_6 = 0;
            Okyanus.DebugVar.Write500_7 = 0;
            Okyanus.DebugVar.Write500_8 = 0;
            Okyanus.DebugVar.Write500_9 = 0;
            Okyanus.DebugVar.Write500_10 = 0;
            Okyanus.DebugVar.Write500_a = 0;
            Okyanus.DebugVar.Write500_b = 0;
            Okyanus.DebugVar.Write500_c = 0;
            Okyanus.DebugVar.Write500_d = 0;
            Okyanus.DebugVar.Write500_e = 0;

            if (Flash500ByteWrite == true) return;
            Flash500ByteWriteTimeoutTimer = 0;

            switch (Okyanus.Variables.Version)
            {
                case Okyanus.Definitions.BJUEmulatorKisitli:
                    break;
                case Okyanus.Definitions.BJUEmulator:
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator2: if (Update500byteSendBuffersV1("temp2345temp4567.csv") == false) return;
                    break;
                case Okyanus.Definitions.TostMakinasiEmulatoru2: if (Update500byteSendBuffersV2("temp1234temp1234.csv") == false) return;
                    break;
                case Okyanus.Definitions.KahveMakinasiEmulator1:
                    break;
                default: return;
                 //   break;
            }   
            writeVerify = 12;           // 28.05.2015
            Flash500ByteCount = 0;     // her 500 byte basinda set edilmeli
            Flash500ByteWrite = true;
            Flash500ByteWriteText = true;

            // CALISMAYAN DEGUSIKLIK ICIN EKLENEN ESKI KOD 23Nisan2016
            WriteToFlash500Procedure(10000);

            // CALISMAYAN DEGUSIKLIK 23Nisan2016
            /*
            if (Okyanus.DebugVar.Write500_a == 0)
            {
                WriteToFlash500Procedure(10000);
                Okyanus.DebugVar.Write500_a++;
            }
            */
            Okyanus.DebugVar.Write500_1++;
    
        }
        private void button500byteOku_Click(object sender, EventArgs e)
        {

            Flas500ReadButtonProcedure(); // flashtan (islemciden) 500 byte okuma

        }
       
        private void button500byteOku2_Click(object sender, EventArgs e)
        {          
            // Kodda yok
            Flas500ReadButtonProcedure(); // flashtan (islemciden) 500 byte okuma


        }
        private void button500byte_2_Click(object sender, EventArgs e)
        {
            Flas500WriteButtonProcedure(); // flasha 500 byte yazma
            // have ve tost icin ortak
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Flas500WriteButtonProcedure();     
        }

        public void StopCommandsendProcedure()
        {
            Okyanus.Variables.WriteFlashEnable = false;
            //     button_CommandStart.Text = "Start Write Table";
            //     button_CommandStart.ForeColor = System.Drawing.Color.Yellow;	
        }
        private void btnFill_Click(object sender, EventArgs e)
        {
            DATA_ConfigArray_Table_Add();
        }
        private void button_delete_Click(object sender, EventArgs e)
        {
            DATA_ConfigArray_Table_Delete();
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PANEL_RamConfig();
        }
        private void commandSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            PANEL_FlashCommandConfig();
        }
        private void buyukFlashOkumaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PANEL_500FlashCommandConfig();
        }
        bool MainComm = true;
        public void communicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainComm == true) PANEL_Communication();
            else PANEL_TES1307();
        }
        private void emulatorEkraniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PANEL_Communication(); MainComm = true;
        }

        private void tES1307EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PANEL_TES1307(); MainComm = false;
        }
        private void dataLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PANEL_Chart();
        }
        private void SimulasyonEkranıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PANEL_Simulasyon_Ekrani();
            
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }
        private void Update_Click(object sender, EventArgs e)
        {

        }
        private void buttonRead_Click(object sender, EventArgs e)
        {

        }
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*      if (Okyanus.Chart.EnableLog == true)
                  {
                      Okyanus.Chart.EnableLog = false;
                      startToolStripMenuItem.Text = "Start";
                      startToolStripMenuItem.ForeColor = Color.Green;
                  }
                  else
                  {
                      Okyanus.Chart.EnableLog = true;
                      startToolStripMenuItem.Text = "Stop";
                      startToolStripMenuItem.ForeColor = Color.Red;

                  }
          */
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            if (Okyanus.Variables.Log_Status == true) startToolStripMenuItem_Click(this, null);
            plotSurface2D1.Clear();
            Okyanus.Chart.Time_Minindexer = 0;
            Okyanus.Chart.Time_Maxindexer = 0;
         */
        }

        private void button_CommandRead_Click(object sender, EventArgs e)
        {
            Flash500ByteWriteText = false;
           
            ReadFlashRequestProcedure();
            writeVerify = 0;      // yok      // 28.05.2015
        }

        private void button_ReqRamRead_Click(object sender, EventArgs e)
        {
            // RAM OKU BUTONU
            Flash500ByteWriteText = false;
            ReqRamReadCommand();
        }

        private void button_FlashreadReqtest_Click(object sender, EventArgs e)
        {
            // flash okumaya karsi verilen uc cevabi
            byte[] Buf = new byte[256];
            byte Command = 12;
            UInt16 Adress = 10000;
            byte AdrLength = 100;
            UInt16 Packetlength = 112;
            UInt16 CRC = 0;

            Buf[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            Buf[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            Buf[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            Buf[3] = (byte)DEFAULT_PREAMBLE;

            Buf[4] = (byte)(Packetlength >> SHIFT8);
            Buf[5] = (byte)Packetlength;

            Buf[6] = Command;
            Buf[7] = (byte)(Adress >> SHIFT8);
            Buf[8] = (byte)Adress;
            Buf[9] = (byte)AdrLength;

            for (byte i = 10; i < (Packetlength - 2); i++)
            {
                Buf[i] = i;
            }

            for (int i = 4; i < (Packetlength - 2); i++)
            {
                CRC += Buf[i];
            }

            Buf[Packetlength - 2] = (byte)(CRC >> SHIFT8);
            Buf[Packetlength - 1] = (byte)CRC;

            try
            {

                SP2_serialPort.Write(Buf, 0, (int)Packetlength);

            }
            catch
            {
                SP2_DisConnect_Procedure();
                MessageBox.Show("Send Error !" + nl + "Port Disconnected?");
            }
        }

        private void button_UcRamreadSimulasyonu_Click(object sender, EventArgs e)
        {
            // ram okumaya karsi verilen uc cevabi
            byte[] Buf = new byte[256];
            byte Command = 14;
            UInt16 Adress = 10000;
            byte AdrLength = 100;
            UInt16 Packetlength = 32;
            UInt16 CRC = 0;

            Buf[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            Buf[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            Buf[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            Buf[3] = (byte)DEFAULT_PREAMBLE;

            Buf[4] = (byte)(Packetlength >> SHIFT8);
            Buf[5] = (byte)Packetlength;

            Buf[6] = Command;
            Buf[7] = (byte)(Adress >> SHIFT8);
            Buf[8] = (byte)Adress;
            Buf[9] = (byte)AdrLength;

            for (byte i = 10; i < (Packetlength - 2); i++)
            {
                Buf[i] = i;
            }

            for (int i = 4; i < (Packetlength - 2); i++)
            {
                CRC += Buf[i];
            }

            Buf[Packetlength - 2] = (byte)(CRC >> SHIFT8);
            Buf[Packetlength - 1] = (byte)CRC;

            try
            {

                SP2_serialPort.Write(Buf, 0, (int)Packetlength);

            }
            catch
            {
                SP2_DisConnect_Procedure();
                MessageBox.Show("Send Error !" + nl + "Port Disconnected?");
            }
        }

        private void button_Ack_uc_simulasyonu_Click(object sender, EventArgs e)
        {
            
            // write flas   okumaya karsi verilen uc ack cevabi
            byte[] Buf = new byte[256];
            byte Command = 20;
            UInt16 Packetlength = 9;
            UInt16 CRC = 0;

            Buf[0] = (byte)(DEFAULT_PREAMBLE >> SHIFT24);
            Buf[1] = (byte)(DEFAULT_PREAMBLE >> SHIFT16);
            Buf[2] = (byte)(DEFAULT_PREAMBLE >> SHIFT8);
            Buf[3] = (byte)DEFAULT_PREAMBLE;

            Buf[4] = (byte)(Packetlength >> SHIFT8);
            Buf[5] = (byte)Packetlength;

            Buf[6] = Command;


            for (int i = 4; i < (Packetlength - 2); i++)
            {
                CRC += Buf[i];
            }

            Buf[Packetlength - 2] = (byte)(CRC >> SHIFT8);
            Buf[Packetlength - 1] = (byte)CRC;

            try
            {

                SP2_serialPort.Write(Buf, 0, (int)Packetlength);

            }
            catch
            {
                SP2_DisConnect_Procedure();
                MessageBox.Show("Send Error !" + nl + "Port Disconnected?");
            }
        }

        private void button_Cleartexxtboxes_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < (100 + 6); i++) // 30 kontrol bytlerini iceren kismi
                Okyanus.Variables.FlashBuffer[i] = 0;

            for (int i = 0; i < (20 + 6); i++) // 6 kontrol bytlerini iceren kismi
                Okyanus.Variables.RamBuffer[i] = 0;

            for (int i = 0; i < 3; i++)
                Okyanus.Variables.AckBuffer[i] = 0;
        }

        private void compiileTimeVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //   String str = "Acit  Compiled  Time & Date : " + __TIME__() + "..." + __DATE__;
            //    String str = __D
            //    MessageBox.Show(str);
        }

        private void button_ReqRamRead_Click_1(object sender, EventArgs e)
        {
            ReqRamReadCommand();
            
        }

        private void clearToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            ClearChart form = new ClearChart();
            form.BackColor = CustomColor();
            form.ShowDialog();
        //    Okyanus.Chart.ClearChart = true;
           /*
            plotSurface2D1.Clear();
            Okyanus.Chart.Time_Minindexer = 0;
            Okyanus.Chart.Time_Maxindexer = 0;
            Okyanus.Chart.XTimes.Clear();
         */
        }

        private void button_FlashWrite_Click(object sender, EventArgs e)
        {
            
            if (Okyanus.Variables.CommandRawIndex != Okyanus.Definitions.MaxCommandLine)
            {
                MessageBox.Show("Fill All Data To Start Send");
                return;
            }
            
            Flash500ByteWriteText = false;
            WriteToFlashProcedure(10000);

            writeVerify = 12;            // 28.05.2015
        }

        private void button_FlashRead_Click(object sender, EventArgs e)
        {
            Flash500ByteWriteText = false;
          
            ReadFlashRequestProcedure();
            writeVerify = 0;      // yok      // 28.05.2015
        }

        private void button_RamRead_Click(object sender, EventArgs e)
        {
            Flash500ByteWriteText = false;
            ReqRamReadCommand();
        }

        private void logScreenShotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //    dataLoggingToolStripMenuItem_Click(sender, e);
            /*
            Rectangle bounds = this.panel_Log.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                //     bitmap.Save("C://test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                try
                {
                    bitmap.Save(Okyanus.Definitions.Log_Directory + "\\" + LOG_GetTime() + "_Log.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception)
                {
                    MessageBox.Show("Save error");
                }
            }
            */
        }

        public void  SNAPSHOT_LogScreen(){
            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);         

                }
                //     bitmap.Save("C://test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                try
                {
                    bitmap.Save(Okyanus.Definitions.WorkDrive + Okyanus.Definitions.Log_Directory + "\\" + LOG_GetTimeWithSec() + ".Log.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception)
                {
                    MessageBox.Show("Save error");
                }
            }

        }

        private void button_LogScreenshot_Click(object sender, EventArgs e)
        {
          //  SNAPSHOT_LogScreen();
        }

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 500 ms secme
            Okyanus.Variables.Log_LogSampleTime = 11;
            samplingTimeToolStripMenuItem.Text = "Ornekleme Suresi ...." + "500 mSaniye Aktif";
        }

        private void saniyeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1 saniye secme
            Okyanus.Variables.Log_LogSampleTime = 22;
            samplingTimeToolStripMenuItem.Text = "Ornekleme Suresi ...." + "1 Saniye Aktif";
        }

        private void saniyeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 2 saniye secme
            Okyanus.Variables.Log_LogSampleTime = 44;
            samplingTimeToolStripMenuItem.Text = "Ornekleme Suresi ...." + "2 Saniye Aktif";
        }

        private void saniyeToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // 5 saniye secme
            Okyanus.Variables.Log_LogSampleTime = 110;
            samplingTimeToolStripMenuItem.Text = "Ornekleme Suresi ...." + "5 Saniye Aktif";
        }

        private void saniyeToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            // 10 saniye secme
            Okyanus.Variables.Log_LogSampleTime = 220;
            samplingTimeToolStripMenuItem.Text = "Ornekleme Suresi ...." + "10 Saniye Aktif";
        }


        private void ChartPlayPause(){
            if (Okyanus.Chart.Play == false)
            {
         //       button4.Text = "Play";
         //       button4.BackColor = System.Drawing.Color.Red;
                grafigiDurdurToolStripMenuItem.Text = "Garfigi Oynat";
                grafigiDurdurToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
     //           panel_Log.BackColor = System.Drawing.Color.AntiqueWhite;
        //        panel_Log.BackColor = CustomColor();
                //       menuStrip1.BackColor = System.Drawing.Color.AntiqueWhite; 
          //      panel_Colour.BackColor = System.Drawing.Color.AntiqueWhite;
         //       plotSurface2D1.BackColor = System.Drawing.Color.AntiqueWhite;
        //        panel_Colour.BackColor = CustomColor();
         //       plotSurface2D1.BackColor = CustomColor();
            }
            else
            {
         //       button4.Text = "Pause";
        //        button4.BackColor = System.Drawing.Color.LimeGreen;
                grafigiDurdurToolStripMenuItem.Text = "Garfigi Beklet";
                grafigiDurdurToolStripMenuItem.ForeColor = System.Drawing.Color.LimeGreen;
      //          panel_Log.BackColor = System.Drawing.Color.OldLace;    // AliceBlue Azure

       //         panel_Log.BackColor = CustomColor();
                //     menuStrip1.BackColor = System.Drawing.Color.OldLace;   
       //         panel_Colour.BackColor = System.Drawing.Color.OldLace;
       //         plotSurface2D1.BackColor = System.Drawing.Color.OldLace;
     //           panel_Colour.BackColor = CustomColor();
     //           plotSurface2D1.BackColor = CustomColor();

            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
 

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
       //     AboutBox1.ActiveForm.ShowDialog(

            //       Form tempChild = (Form)this.MdiChildren[x];
            AboutBox1 form = new AboutBox1();
            //    form.MdiParent = this;
            //     form.ShowDialog();
       //     form.BackColor = CustomColor();
            form.Show();
            //    form.
            //    form.Show();

        }

        private void grafigiDurdurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // play // pause
            if (Okyanus.Variables.Log_Status == false) { 
                MessageBox.Show("Once Veri Toplamayi Baslatmalisiniz "); 
                return; 
            
            }

            if (Okyanus.Chart.Play == true)
            {
                  Okyanus.Chart.Play = false;
            }
            else
            {
                 Okyanus.Chart.Play = true;
            }
              ChartPlayPause();
        }

        private void verileriAcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Okyanus.Chart.LogPropUpdate = true; 
            LOGData form = new LOGData();
            form.BackColor = CustomColor();
            form.Show();
        }

        private void checkBox_TES1307_CheckedChanged(object sender, EventArgs e)
        {
           if(checkBox_TES1307.Checked == true) 
               Okyanus.Chart.TES1307_Enabled = true;
           else Okyanus.Chart.TES1307_Enabled = false;
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Serial_test_Load(object sender, EventArgs e)
        {

        }

        public void ModelDegisim(){
            Versiyon_Karsilastirma();
            if (panel_Simulasyon.Visible == true) PANEL_Simulasyon_Ekrani();
            if (panel_CommandSetup.Visible == true) PANEL_FlashCommandConfig();
            if(panel_500CommandSetup.Visible == true) PANEL_500FlashCommandConfig();
        }


        private void bJUKisitliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Okyanus.Variables.Version = Okyanus.Definitions.BJUEmulatorKisitli;
            ModelDegisim();
        }

        private void bJUEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Okyanus.Variables.Version = Okyanus.Definitions.BJUEmulator;
            ModelDegisim();
        }

        private void kahveMakinasiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Okyanus.Variables.Version = Okyanus.Definitions.KahveMakinasiEmulator2;
            ModelDegisim();       
        }

        private void tostMakinasiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Okyanus.Variables.Version = Okyanus.Definitions.TostMakinasiEmulatoru2;
            ModelDegisim();
        }

        private void kahveMakinasiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Okyanus.Variables.Version = Okyanus.Definitions.KahveMakinasiEmulator1;
             ModelDegisim();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DATA_CommandArray_Table_Add();
        }

        private void button_FlashComboSatirSil_Click(object sender, EventArgs e)
        {
            DATA_CommandArray_Table_Delete();
        }

        private void button_FlashRead_Click_1(object sender, EventArgs e)
        {
            Flash500ByteWriteText = false;

            ReadFlashRequestProcedure();
            writeVerify = 0;      // yok      // 28.05.2015
        }

        private void button_FlashWrite_Click_1(object sender, EventArgs e)
        {
            // flash a yaz butonu
            if (Okyanus.Variables.CommandRawIndex != Okyanus.Definitions.MaxCommandLine)
            {
                MessageBox.Show("Fill All Data To Start Send");
                return;
            }
            WriteToFlashProcedure(10000);
            writeVerify = 12;           // 28.05.2015
            Flash500ByteWriteText = false;
        }

        private void komutListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //       Form tempChild = (Form)this.MdiChildren[x];
            CommandListApp form = new Okyanus.CommandListApp();
            //    form.MdiParent = this;
            //     form.ShowDialog();
            form.BackColor = CustomColor();
            form.Show();

        }

        private void button_Flash500_dosyaoku_Click(object sender, EventArgs e)
        {
            File_Open_500_FlashCommandSelectFile();
        }

        private void button_Flash500_dosyaya_Click(object sender, EventArgs e)
        {
            File_Save_500_FlashCommandSelectFile();
        }

        private void dosyaListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DosyaListesi form = new Okyanus.DosyaListesi();
            form.BackColor = CustomColor();
            form.Show();
        }

        private void button_DosyaKaydet_Click(object sender, EventArgs e)
        {
            SaveFlashFileCommon();
        }

        private void button_DosyaAc_Click(object sender, EventArgs e)
        {
            OpenFlashFileCommon();
        }

        private void dt_500Command_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Kahve2 cell values changed
            Okyanus.Variables.FlashCommand500byte_Kahve2 = " ";
            textBox_500byte1.Text = Okyanus.Definitions.Kahve2Dosyaismi + Okyanus.Variables.FlashCommand500byte_Kahve2;
        }

        private void dt_500Command2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Okyanus.Variables.FlashCommand500byte_Tost = " ";
            textBox_500byte2.Text = Okyanus.Definitions.TostDosyaismi + Okyanus.Variables.FlashCommand500byte_Tost;

            
        
        }
        //button_KM2_01.BackColor = Color.Aqua;
        // 

    }
}


