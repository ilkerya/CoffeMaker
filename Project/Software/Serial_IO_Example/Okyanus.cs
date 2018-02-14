using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serial_IO_Example;
using System.Drawing;
namespace Okyanus
{
    class DebugVar
    {
        public static int Write500_1;
        public static int Write500_2;
        public static int Write500_3;
        public static int Write500_4;
        public static int Write500_5;
        public static int Write500_6;
        public static int Write500_7;
        public static int Write500_8;
        public static int Write500_9;
        public static int Write500_10;
        public static int Write500_11;

        public static int Write500_a;
        public static int Write500_b;
        public static int Write500_c;
        public static int Write500_d;
        public static int Write500_e;

        public static int Timeout_Val;
    }

    class Variables
    {
        public static String Preamble_str = "";
        public static String FlashBuffer_str = "";
        public static String RamBuffer_str = "";
        public static String AckBuffer_str = "";

        public static byte[] FlashBuffer = new byte[256];
        public static byte[] RamBuffer = new byte[256];
        public static byte[] AckBuffer = new byte[256];


        public static string[] NameArr = new string[256];
        public static int[] AdressArr = new int[20];
        public static byte[] ByteArr = new byte[4]; //  bit okuma // 1 byte // 2 byte // 4 byte
        public static byte[] SignArr = new byte[2]; // 
        public static byte[] BitArr = new byte[8];
    //    public static int[] IntervalArr = new int[7]; // bunu  kaldir
        public static int[] PlotOnOffArr = new int[2];  // combo  box
        public static int[] PlotMultArr = new int[7];  // combo  box  1 10 20 30 40 50 100
        public static int[] PlotMaxArr = new int[300];  // numeric box
        public static int[] PlotMinArr = new int[300];  // numeric box
        public static int[] PlotColorArr = new int[1000];  // combo  box  color palette



        public static int[] CommandTypeArr = new int[50]; // command Sayisi
        public static int[] CommandArr = new int[50]; // command Sayisi
 

                
  //      public static int[] Var_CommandName = new int[30]; // command sayisi 
        public static int[] Var_CommandType = new int[50]; // command tipi 
        public static int[] Var_Command = new int[50]; // command tipi 
        public static int[] Var_Parameter1 = new int[30]; // parameter  sayisi
        public static int[] Var_Parameter2 = new int[30]; // parameter sayisi
        public static int[] Var_Parameter3 = new int[30]; // parameter sayisi
        public static int[] Var_Parameter4 = new int[30]; // parameter sayisi
        public static byte CommandRawIndex = 0;  // her bir raw i temsil eden index

        // for 500 byte sending buffers
 /*       public static int[] SendBuffer0 = new int[30]; // command tipi 
        public static int[] SendBuffer1 = new int[30]; // parameter  sayisi
        public static int[] SendBuffer2 = new int[30]; // parameter sayisi
        public static int[] SendBuffer3 = new int[30]; // parameter sayisi
        public static int[] SendBuffer4 = new int[30]; // parameter sayisi
        public static int[] SendBuffer5 = new int[30]; // parameter  sayisi
        public static int[] SendBuffer6 = new int[30]; // parameter sayisi
        public static int[] SendBuffer7 = new int[30]; // parameter sayisi
        public static int[] SendBuffer8 = new int[30]; // parameter sayisi
        public static int[] SendBuffer9 = new int[30]; // parameter say
*/
        public static int[] SendBuffer = new int[300]; // command tipi 
        public static int[] ReadBuffer = new int[300]; // command tipi 
 //       public static byte CommandRawIndex500byte = 0;  // her bir raw i temsil eden index


        
        public static string[] CommandVar_Name = new string[256];



        public static string[] CommandParameter1_value = new string[256];


        public static int CommandVar_NameIndex = 0;

        // her  bir raw icin tutulan conf sayisi
        // tabloya eklenen degisken sayisi
        public static int[] Var_Adress = new int[256];
        public static byte[] Var_Sign = new byte[256];
        public static byte[] Var_Length = new byte[256];
        public static byte[] Var_Bit = new byte[256];

        public static int[] Var_PlotOnOff = new int[256];  // max raw sayisi kadar olmali hepsi
        public static int[] Var_PlotMult = new int[256];
        public static int[] Var_PlotMin = new int[256];  // 
        public static int[] Var_PlotMax = new int[256];
        public static int[] Var_PlotColour = new int[256];

        public static byte RawIndex = 0;  // her bir raw i temsil eden index
        public static byte MaxRawIndex = 0;

        //     static int[] Var_Interval = new int[256];
    //    public static List<String> Variables_Name = new List<String>(256);  // degiskenlerin ismi
   //     public static List<String> Variables_Data = new List<String>(256); // degiskenlerin dinusturulmus icerikleri


        public static string[] Variables_Name = new string[256];
        public static int Variables_NameIndex =0;
        public static string[] Variables_NameBuffer = new string[256];
        public static string[] Variables_Data = new string[256];

        public static bool FlashRead100byte = true;
        public static int FlashRead500Phase = 0;
        public static bool UpdateGrid500Table = false;
        public static int FlashRead500Timeout = 0;
        public static int FlashRead500EndMessageTimer = 5;
        public static bool FlashRead500EndMessageEnable = false;
        
      //  public static int[] IntervalArr = new int[6];


   //     public static int[] fff = new int[2];

        // okunan 20 bytelik datayi buraya indexle yerlestir log ve chart bundan okuyarak yazacak 20 lik adresler halinde
        public static uint[] SP_ReceivedData= new uint[20];


        public static List<String> Name_OfInput = new List<String>();


        public static int X_Point = 0;  //4
        public static int Y_Point  = 24;// menu strip yuksekligi
        public static int Width_Panel = 1425;// 1400
        public static int Height_Panel = 820; // 795 form1 den 55 eksik

        public static int Width_Form = 1445;// 1550
        public static int Height_Form = 885; //860 form1 den 55 eksik


    //    public static int X_Point = ;
  //      public static int Y_Point = 24;
    //    public static int Width = 1400;
   //     public static int Height = 730;

/*
		struct {
				bool Status;
				bool Error;
				unsigned int Counter;
				unsigned int  SampleCounter;
 * 
				unsigned int InitTime;
				unsigned int  FileCount;
				unsigned int Original_Log_File_post_add;
 * 
				bool LogChartDeleteEnable;
				int LogChartDeleteTimer;
		 		int LogStopCounter ;
		 		int Log_SampleTime ;
		 		int ChartSoftOffTimer;
		} Log;
*/

        public static int LOg_ViewTimer=0;
        public static bool Log_Status = false;
        public static bool Log_Error = false;
        public static int Log_Counter = 0;
        public static int Log_SampleCounter = 0;

        public static int Log_InitTime = 0;
        public static int Log_FileCount = 0;
        public static int Log_Original_Log_File_post_add = 0;

        public static bool Log_LogChartDeleteEnable = false;
        public static int Log_LogChartDeleteTimer = 0;
        public static int Log_LogStopCounter = 0;
        public static int Log_LogSampleTime = 0;
        public static int Log_ChartSoftOffTimer = 0;

        public static int SendCommandQue = 0;
        public static int SendCommandTimer = 0;


        public static bool WriteFlashEnable = false;
        public static bool LogPlay = false;

        public static bool SendReadCommandEnable=false;

        public static int TimeoutErrror_Count = 0;
        public static int Preamble_Count = 0;
        public static int Preamble_errorcount = 0;
        public static int Length_errorcount = 0;
        public static int RealReceivedDataCount = 0;


        public static String PreambleCountr_str="";
        public static String PreambleError_str = "";
        public static String LengthError_str = "";

        public static bool OpenLogFileatInit=false;
        public static bool OpenCommandFileatInit=false;
        public static bool OpenCommandFlashFileatInit = false;
        
        public static bool OpenCommandFile500atInit = false;
        public static bool OpenCommandFile500atInit2 = false;

        public static String LogFileName = "";
        public static String CommandFileName = "";
        public static String FlashCommand500byte_Kahve2 = "";
        public static String FlashCommand500byte_Tost = "";
       
 

         public static String[] Drives = Environment.GetLogicalDrives();

         public static bool UpdateVariables=false;

         public static byte Version = 0;

    }
    class Definitions
    {
        public const byte FlashRead = 12;
        public const byte RamRead = 14;
        public const byte FlashWrite = 10;
        public const byte ACKRead = 20;

        public static readonly string WriteComamnd = "Write To Card";
        public static readonly string ReadComamnd = "Read From Card";
        public static string nl = System.Environment.NewLine;

        public static string WorkDrive="";
        public static bool BJU_emulator_yazma_kisitli_V=false;


        public static readonly string Kahve2Dosyaismi = "Kahve Mak. Yuklu Dosya : ";
        public static readonly string   TostDosyaismi = "Tost Mak. Yuklu Dosya : ";



        public const byte  BJUEmulatorKisitli = 4;
        public const byte  BJUEmulator = 8;
        public const byte  KahveMakinasiEmulator2 = 16;
        public const byte  TostMakinasiEmulatoru2 = 32;
        public const byte  KahveMakinasiEmulator1 = 64;
/*
        public static readonly string Log_Directory = "C:\\DataLog";
        public static string Original_Log_File = "C:\\DataLog\\data.csv";
        public static string Original_Log_File_Base;

        public static readonly string Config_Directory = "C:\\DataLog";
        public static readonly string ConfigLog_File = "C:\\DataLog\\LogConfig";

        public static readonly string Command_Directory = "C:\\DataLog";
        public static readonly string Command_File = "C:\\DataLog\\ComConfig";
*/
        public static readonly string Log_Directory = "DataLog";
        public static string Original_Log_File = "DataLog\\data.csv";
        public static string Original_Log_File_Base;

        public static readonly string Config_Directory = "DataLog";
        public static readonly string ConfigLog_File = "DataLog\\BJU_LogConfig";

        public static readonly string Command_Directory = "DataLog";
        public static readonly string Command_File = "DataLog\\ComConfig";
        public static readonly string FlashCommand_File = "DataLog\\BJU_ComConfig";
        public static readonly string FlashCommand500byte_Kahve2 = "DataLog\\TurkK2_500byte_ComConfig";
        public static readonly string FlashCommand500byte_Tost = "DataLog\\TostM_500byte_ComConfig";        
     //   public static readonly string ConfigLog_FileAlternative = "D:\\DataLog\\LogConfig.txt";

        
        public static readonly string LogOffWarning= "Log Devam Ediyor " + nl +  "Once Log islemini Durdurun";
        public static readonly string ChartOffWarning = "Grafik isleme Devam Ediyor " + nl + "Once Grafik islemini Durdurun";

        public static readonly string ErrorWarning = "ErrorError12345ter";
        public static readonly string FileForgive = "FileForgive";

        public const byte MaxConfigLine = 25;

        public const byte MaxCommandLine = 10;

        public const string Komut00 = "_bos";
        public const string Komut01 = "_aProg";
        public const string Komut02 = "_isit_birak_SS2";
        public const string Komut03 = "_isit_birak_SS"; //3
        public const string Komut04 = "_sicak_tut_SS";
        public const string Komut05 = "_sicak_tut_SS2";
        public const string Komut06 = "_isit_oransal";//6
        public const string Komut07 = "_soguma_bekle";
        public const string Komut08 = "_buyukse_atla"; //8
        public const string Komut09 = "_kucukesitse_atla";
        public const string Komut10 = "_zamanlayici_kur";
        public const string Komut11 = "_bekle"; //11
        public const string Komut12 = "_adima_git";
        public const string Komut13 = "_bitir";
        public const string Komut14 = "_alarm";//14
        public const string Komut15 = "_komponent_kapa";
        public const string Komut16 = "_hProg";
        public const string Komut17 = "_isit_birak_PI";//17
        public const string Komut18 = "_isit_PI";
        public const string Komut19 = "_I_Max_Ata"; // 19


        public const string Komut20 = "_Prog20"; // 


        public const string Komut21 = "_M1";//
        public const string Komut22 = "_M2";//
        public const string Komut23 = "_M3";//
        public const string Komut24 = "_M4";//
        public const string Komut25 = "_M5";//
        public const string Komut26 = "_M6";//

        public const string Komut27 = "_CIKIS3";//
        public const string Komut28 = "_CIKIS4";//
        public const string Komut29 = "_CIKIS5";//
        public const string Komut30 = "_CIKIS6";//



        public const string Komut31 = "_G1_Aksiyon";//
        public const string Komut32 = "_G2_Aksiyon";//
        public const string Komut33 = "_G3_Aksiyon";//
        public const string Komut34 = "_G4_Aksiyon";//
        public const string Komut35 = "_G5_Aksiyon";//
        public const string Komut36 = "_G6_Aksiyon";//
        public const string Komut37 = "_G7_Aksiyon";//
        public const string Komut38 = "_G8_Aksiyon";//
        public const string Komut39 = "_G9_Aksiyon";//
        public const string Komut40 = "_G10_Aksiyon";//
        public const string Komut41 = "_G11_Aksiyon";//
        public const string Komut42 = "_G12_Aksiyon";//

        public const string Komut43 = "_Prog43";//
        public const string Komut44 = "_Prog44";//

        public const string Komut45 = "_SS_Calis";//
        public const string Komut46 = "_SS1_Aksiyon";//
        public const string Komut47 = "_SS2_Aksiyon";//
        public const string Komut48 = "_SS3_Aksiyon";//
        public const string Komut49 = "_Prog49";//



        /*
                public const string Komut00 = "bos";
                public const string Komut01 = "aKomut";
                public const string Komut02 = "bKomut";
                public const string Komut03 = "isit_birak_SS"; //3
                public const string Komut04 = "sicak_tut_SS";
                public const string Komut05 = "cKomut";
                public const string Komut06 = "isit_oransal";//6
                public const string Komut07 = "soguma_bekle";
                public const string Komut08 = "dKomut"; //8
                public const string Komut09 = "eKomut";
                public const string Komut10 = "fKomut";
                public const string Komut11 = "bekle"; //11
                public const string Komut12 = "adima_git";
                public const string Komut13 = "bitir";
                public const string Komut14 = "alarm";//14
                public const string Komut15 = "gKomut";
                public const string Komut16 = "hKomut";
                public const string Komut17 = "isit_birak_PI";//17
                public const string Komut18 = "isit_PI";
                public const string Komut19 = "I_Max_Ata"; // 19
        */
        /*
        public const int PlotMul00 = 1; // 0.001
        public const int PlotMul01 = 10;//0.01
        public const int PlotMul02 = 100; // 0.1
        public const int PlotMul03 = 1000;//1
        public const int PlotMul04 = 10000; // 10
        public const int PlotMul05 = 100000;//100
        public const int PlotMul06 = 1000000;//1000
*/
   
        public const int PlotMul0001 = 0; // 0.001
        public const int PlotMul001 = 1;//0.01
        public const int PlotMul01 = 2; // 0.1
        public const int PlotMul1 = 3;//1
        public const int PlotMul10 = 4; // 10
        public const int PlotMul100 = 5;//100
        public const int PlotMul1000 = 6;//1000

        public const string Mul_0001 = "x0.001";//17
        public const string Mul_001 =  "x0.01";
        public const string Mul_01 =   "x0.1"; // 19
        public const string Mul_1 =    "x1"; // 19
        public const string Mul_10 =   "x10"; // 19
        public const string Mul_100 =  "x100"; // 19
        public const string Mul_1000 = "x1000"; // 19

        public const int FLASH500BYTE_TIMEOUT = 150;
        public const int DISPLAY_TEXT_WRITING = 77;
        public const int DISPLAY_TEXT_WRITTEN = 6;
        public const int DISPLAY_TEXT_END = 1;

        public const int FLASH_500_TIME_INTERVAL = 11; //11

        public const int TIMER_SHARE_BASE = 9; // IN ORDER TO UPDATE DISPLAY AND OTHERS
/*
        public const string PlotMul02 = "10";
        public const string PlotMul03 = "25";
        public const string PlotMul04 = "50";//17
        public const string PlotMul05 = "75";
        public const string PlotMul06 = "100"; // 19
        public const string PlotMul07 = "150"; // 19
*/
        /*
            Okyanus.Variables.PlotMultArr[0] = 1;
            Okyanus.Variables.PlotMultArr[1] = 5;
            Okyanus.Variables.PlotMultArr[2] = 10;
            Okyanus.Variables.PlotMultArr[3] = 25;
            Okyanus.Variables.PlotMultArr[4] = 50;
            Okyanus.Variables.PlotMultArr[5] = 75;
            Okyanus.Variables.PlotMultArr[6] = 100;
            Okyanus.Variables.PlotMultArr[7] = 150;
*/
    }
    class Chart
    {
        public static string[] DataArray0 = new string[12000];
        public static string[] DataArray1 = new string[12000];
        public static string[] DataArray2 = new string[12000];
        public static string[] DataArray3 = new string[12000];
        public static string[] DataArray4 = new string[12000];
        public static string[] DataArray5 = new string[12000];
        public static string[] DataArray6 = new string[12000];
        public static string[] DataArray7 = new string[12000];
        public static string[] DataArray8 = new string[12000];
        public static string[] DataArray9 = new string[12000];
        public static string[] DataArray10 = new string[12000];
        public static string[] DataArray11 = new string[12000];
        public static string[] DataArray12 = new string[12000];
        public static string[] DataArray13 = new string[12000];
        public static string[] DataArray14 = new string[12000];
        public static string[] DataArray15 = new string[12000];
        public static string[] DataArray16 = new string[12000];
        public static string[] DataArray17 = new string[12000];
        public static string[] DataArray18 = new string[12000];
        public static string[] DataArray19 = new string[12000];
        public static string[] DataArray20 = new string[12000];
        public static string[] DataArray21 = new string[12000];
        public static string[] DataArray22 = new string[12000];
        public static string[] DataArray23 = new string[12000];
        public static string[] DataArray24 = new string[12000];

        public static string[] TES1307_01_DataArray = new string[12000];
        public static string[] TES1307_02_DataArray = new string[12000];


        public static System.Collections.ArrayList TimeArray = new System.Collections.ArrayList();
    //    public static System.Collections.ArrayList XTimes = new System.Collections.ArrayList();

        public static System.Collections.ArrayList XTimes = new System.Collections.ArrayList();
  //      ArrayList XTimes = new ArrayList();
     //   ArrayList XTimes = new ArrayList();

        public static readonly int MAXTIME = 3600; // 60 san x 60 = 3600 ornek/saat 3600 * 3= 10800 3 saat
        public const int MAXLOGCOUNT = 3600; // 60 san x 60 = 3600 ornek/saat 3600 * 3= 10800 3 saat

        public static int Time_Maxindexer=0;
        public static int Time_Minindexer=0;

        public static bool Play=false;
        public static int PlayDelay=0;

        public static bool ClearChart=false;

        public static System.Drawing.Color Renk0 = new System.Drawing.Color();
        public static System.Drawing.Color Renk1 = new System.Drawing.Color();
        public static System.Drawing.Color Renk2 = new System.Drawing.Color();
        public static System.Drawing.Color Renk3 = new System.Drawing.Color();
        public static System.Drawing.Color Renk4 = new System.Drawing.Color();

        public static System.Drawing.Color Renk5 = new System.Drawing.Color();
        public static System.Drawing.Color Renk6 = new System.Drawing.Color();
        public static System.Drawing.Color Renk7 = new System.Drawing.Color();
        public static System.Drawing.Color Renk8 = new System.Drawing.Color();
        public static System.Drawing.Color Renk9 = new System.Drawing.Color();

        public static System.Drawing.Color Renk10 = new System.Drawing.Color();
        public static System.Drawing.Color Renk11 = new System.Drawing.Color();
        public static System.Drawing.Color Renk12 = new System.Drawing.Color();
        public static System.Drawing.Color Renk13 = new System.Drawing.Color();
        public static System.Drawing.Color Renk14 = new System.Drawing.Color();

        public static System.Drawing.Color Renk15 = new System.Drawing.Color();
        public static System.Drawing.Color Renk16 = new System.Drawing.Color();
        public static System.Drawing.Color Renk17 = new System.Drawing.Color();
        public static System.Drawing.Color Renk18 = new System.Drawing.Color();
        public static System.Drawing.Color Renk19 = new System.Drawing.Color();

        public static System.Drawing.Color Renk20 = new System.Drawing.Color();
        public static System.Drawing.Color Renk21 = new System.Drawing.Color();
        public static System.Drawing.Color Renk22 = new System.Drawing.Color();
        public static System.Drawing.Color Renk23 = new System.Drawing.Color();
        public static System.Drawing.Color Renk24 = new System.Drawing.Color();

    //     if (checkBox_plot7.Checked == true)

        public static bool[] GraphEnable= new bool[25];

        public static bool LogPropUpdate=false;

        public static bool TES1307_Enabled=false;

        public static int TES1307_01_Raw=0;
        public static int TES1307_02_Raw=0;

        public static string VeriFormBasligi = "Anlık Veri";

   //     Renk1 = System.Drawing.Color.
       //     Color wetr = new Color();
        //    wetr = Color.Blue;

    }

}
/*
static DataTable GetTable2()
{
    //
    // Here we create a DataTable with four columns.
    //
//     /*
    DataTable table = new DataTable();
    table.Columns.Add("Dosage", typeof(int));
    table.Columns.Add("Drug", typeof(string));
    table.Columns.Add("Patient", typeof(string));
    table.Columns.Add("Date", typeof(DateTime));

    //
    // Here we add five DataRows.
    //
    table.Rows.Add(25, "Indocin", "David", DateTime.Now);
    table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
    table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
    table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
    table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
    return table;
       
}
*/