using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Okyanus
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format("{0}", AssemblyTitle, "  Hakkinda:") + "   " + String.Format("{0}", AssemblyVersion);
        //    this.Text = "Exe Calisma Klasoru = " + System.IO.Directory.GetCurrentDirectory() + System.Environment.NewLine;
            this.labelProductName.Text = AssemblyProduct;

    //        this.labelCompanyName.Text = AssemblyCompany;
            
            this.labelVersion.Text = String.Format("Derleme UTC (Saat:Dakika.Gun.Ay.Yil)    {0}", AssemblyVersion);

            this.labelCopyright.Text = System.IO.Directory.GetCurrentDirectory() + "\\" + AssemblyTitle + ".exe" + System.Environment.NewLine + AssemblyCopyright;
        //    str += "Exe Calisma Klasoru = " + System.IO.Directory.GetCurrentDirectory() + System.Environment.NewLine;
            
          //  String str = AssemblyDescription;
            String str = "Aciklama" + Environment.NewLine;
             str += AssemblyDescription;
       //     this.textBoxDescription.Text = " " +  + AssemblyDescription;

             this.textBoxDescription.Text = str;
             this.textBoxDescription.BackColor = Color.AliceBlue;


             this.labelVersion.BackColor = Color.AliceBlue;
            this.labelCopyright.BackColor = Color.AliceBlue;
            this.labelProductName.BackColor = Color.AliceBlue;
            this.BackColor = Color.AliceBlue;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void labelProductName_Click(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            this.linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.okyanuselektronik.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            this.linkLabel2.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.timeanddate.com/time/aboututc.html");
        
        }
    }
}
