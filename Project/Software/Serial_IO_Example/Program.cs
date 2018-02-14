
//#define TOT_DATA  edr  //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
namespace Serial_IO_Example
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            String  Pic1_path = Environment.SystemDirectory;
	        String  File1_path =   "NPlot.dll";
            String File2_path = "LumenWorks.Framework.IO.dll";

            if (File.Exists(Pic1_path) || File.Exists(File1_path))
            {
                if (File.Exists(Pic1_path) || File.Exists(File2_path))
                {
                    Application.Run(new Serial_test());
                }
                else
                {
                    MessageBox.Show(File2_path + " missing at Windows System or Root Directory");
                }
            }
            else
            {
                MessageBox.Show(File1_path + " missing at Windows System or Root Directory" + System.Environment.NewLine + "You can obtain from :  http://netcontrols.org");
            }
        }
    }
}
