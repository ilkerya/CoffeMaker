using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using LumenWorks.Framework.IO.Csv;

namespace GridWork
{
    public class GridHelper
    {
        //     private static readonly string Variable_Oky1 = "Variable Name";
        public static readonly string Variable_Oky1 = "Variable Name";
        public static readonly string Variable_Oky2 = "Adress";
        public static readonly string Variable_Oky3 = "Sign";
        public static readonly string Variable_Oky4 = "Length";
        public static readonly string Variable_Oky5 = "Bit";
        //  public static readonly string Variable_Oky6 = "Interval";
        public static readonly string Variable_Oky7 = "Charting On";
        public static readonly string Variable_Oky8 = "Chart Multiply";
        public static readonly string Variable_Oky9 = "Chart Min";
        public static readonly string Variable_Oky10 = "Chart Max";
        public static readonly string Variable_Oky11 = "Chart Colour";

        // dt.Columns.Add(Variable_OkyCheck, typeof(bool));

        // command menu taBLES
        public static readonly string Variable_Oky100 = "Sira";
        public static readonly string Variable_Oky101 = "Komut";
        public static readonly string Variable_Oky102 = "Parametre1";
        public static readonly string Variable_Oky103 = "Parametre2";
        public static readonly string Variable_Oky104 = "Parametre3";
        public static readonly string Variable_Oky105 = "Adim Zamani";

        // command menu taBLES  2. versiyon program
        public static readonly string Variable_FC_Oky100 = "P1";
        public static readonly string Variable_FC_Oky101 = "P2";
        public static readonly string Variable_FC_Oky102 = "P3";
        public static readonly string Variable_FC_Oky103 = "P4";
        public static readonly string Variable_FC_Oky104 = "P5";

        public static readonly string Variable_FC_Oky105 = "P6";
        public static readonly string Variable_FC_Oky106 = "P7";
        public static readonly string Variable_FC_Oky107 = "P8";
        public static readonly string Variable_FC_Oky108 = "P9";
        public static readonly string Variable_FC_Oky109 = "P10";




     //   public static readonly string Variable_Oky105 = "Adim Zamani";

        private static readonly string GridColumnDelimeter = ",";

        DataGridView m_gridView;
        DataTable m_Data;

        public GridHelper(DataGridView dataGrid)
        {
            m_gridView = dataGrid;
        }
        public List<String> GetGridValues()
        {
            List<String> gridValues = new List<string>();
            for (int rows = 0; rows < m_gridView.Rows.Count; rows++)
            {
                StringBuilder rowInfo = new StringBuilder();
                int col = 0;
                for (col = 0; col < m_gridView.Rows[rows].Cells.Count; col++)
                {
                    if (col == 0)
                    {
                        if (m_gridView.Rows[rows].Cells[0].Value == null)
                        {
                            break;
                        }

                        if (string.IsNullOrEmpty(m_gridView.Rows[rows].Cells[0].Value.ToString()))
                        {
                            rowInfo.Append("F");
                        }
                        else
                        {
                            //   rowInfo.Append("T");
                            rowInfo.Append(m_gridView.Rows[rows].Cells[0].Value.ToString()); // 
                            //     rowInfo.Append(GridColumnDelimeter); 
                            //     rowInfo.Append(m_gridView.Rows[rows].Cells[col].Value.ToString());
                        }
                    }
                    else
                    {
                        // rowInfo.Append(m_gridView.Rows[rows].Cells[col].Value.ToString());
                    }

                    if (col + 1 != m_gridView.Rows[rows].Cells.Count)
                    {
                        // rowInfo.Append(GridColumnDelimeter);  // colon sonu 
                    }
                }

                if (col == m_gridView.Columns.Count)
                {
                    gridValues.Add(rowInfo.ToString());
                  //  colValues.Add(col.ToString());
         //           colValue = col.ToString();
                }
            }

            return gridValues;
        }
        public DataTable GetDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(Variable_Oky1);
            dt.Columns.Add(Variable_Oky2);
            dt.Columns.Add(Variable_Oky3);
            //      dt.Columns.Add(Variable_Oky3, typeof(bool));
            dt.Columns.Add(Variable_Oky4);
            dt.Columns.Add(Variable_Oky5);
            //       dt.Columns.Add(Variable_Oky6);
            dt.Columns.Add(Variable_Oky7);
            //     dt.Columns.Add(Variable_Oky7, typeof(bool));
            dt.Columns.Add(Variable_Oky8);
            dt.Columns.Add(Variable_Oky9);
            dt.Columns.Add(Variable_Oky10);
            dt.Columns.Add(Variable_Oky11);
            //   dt.Columns.
            m_Data = dt;
            return dt;
        }
     public DataTable GetDataTable4Command_oldType()
        {   // 1. versiyon için
            DataTable dt = new DataTable();
            dt.Columns.Add(Variable_Oky100);
            dt.Columns.Add(Variable_Oky101);
            dt.Columns.Add(Variable_Oky102);
            dt.Columns.Add(Variable_Oky103);
            dt.Columns.Add(Variable_Oky104);
            dt.Columns.Add(Variable_Oky105);
            m_Data = dt;
            return dt;
        }

        public DataTable GetDataTable4Command()
        {    // 2. versiyon için
            DataTable dt = new DataTable();
            dt.Columns.Add(Variable_FC_Oky100);
            dt.Columns.Add(Variable_FC_Oky101);
            dt.Columns.Add(Variable_FC_Oky102);
            dt.Columns.Add(Variable_FC_Oky103);
            dt.Columns.Add(Variable_FC_Oky104);
            m_Data = dt;
            return dt;
        }

        public DataTable GetDataTable4_500Byte50Line6Column()
        {    // 2. versiyon için
            DataTable dt = new DataTable();
            dt.Columns.Add(Variable_Oky100);
            dt.Columns.Add(Variable_Oky101);
            dt.Columns.Add(Variable_Oky102);
            dt.Columns.Add(Variable_Oky103);
            dt.Columns.Add(Variable_Oky104);
            dt.Columns.Add(Variable_Oky105);
            m_Data = dt;
            return dt;
        }

        /*
        public DataTable GetDataTable4_500ByteCommand2()
        {    // 2. versiyon için
            DataTable dt = new DataTable();
            dt.Columns.Add(Variable_Oky100);
            dt.Columns.Add(Variable_Oky101);
            dt.Columns.Add(Variable_Oky102);
            dt.Columns.Add(Variable_Oky103);
            dt.Columns.Add(Variable_Oky104);
            dt.Columns.Add(Variable_Oky105);    
            m_Data = dt;
        
            return dt;
        }
        */

        public void PrintScreen()
        {
            m_gridView.DataSource = m_Data;
        }

        public void Export(string fileName, FileTypes fileType)
        {
            switch (fileType)
            {
                case FileTypes.Excel:
                    ExportExcel(fileName);
                    break;
                case FileTypes.CSV:
                default:
                    ExportCSV(fileName);
                    break;
            }
        }
        /*
                public void Import(string fileName, FileTypes fileType)
                {
                    switch (fileType)
                    {
                        case FileTypes.Excel:
                            ImportExcel(fileName);
                            break;
                        case FileTypes.CSV:
                        default:
                            ImportCSV(fileName);
                            break;
                    }        
                }
        */
        private void ExportExcel(string fileName)
        {
            //string delimeter = ",";
            string delimeter = "\t";

            string stOutput = "";
            // Export titles:
            string sHeaders = "";

            for (int j = 0; j < m_gridView.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(m_gridView.Columns[j].HeaderText) + delimeter;
            stOutput += sHeaders + "\r\n";
            // Export data.
            for (int i = 0; i < m_gridView.RowCount - 1; i++)
            {
                string stLine = "";
                for (int j = 0; j < m_gridView.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(m_gridView.Rows[i].Cells[j].Value) + delimeter;
                stOutput += stLine + "\r\n";
            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
        }

        private void ImportExcel(string fileName)
        {
            if (fileName.Contains(".xlsx"))
            {
                String name = "Items";
                String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                fileName +
                                ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

                OleDbConnection con = new OleDbConnection(constr);
                OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
                con.Open();

                OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                DataTable data = new DataTable();
                sda.Fill(data);
                m_gridView.DataSource = data;
            }
            else
            {
                String name = "Items";
                String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                fileName +
                                ";Extended Properties='Excel 8.0;HDR=YES;';";

                OleDbConnection con = new OleDbConnection(constr);
                OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
                con.Open();

                OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                DataTable data = new DataTable();
                sda.Fill(data);
                m_gridView.DataSource = data;
            }
        }
        /*
        private void ExportCSV(string fileName)
        {
            try
            {
                System.IO.StreamWriter csvFileWriter = new StreamWriter(fileName, false);

                string columnHeaderText = "";

                int countColumn = m_gridView.ColumnCount - 1;

                if (countColumn >= 0)
                {
                    columnHeaderText = m_gridView.Columns[0].HeaderText;
                }

                for (int i = 1; i <= countColumn; i++)
                {
                    columnHeaderText = columnHeaderText + ',' + m_gridView.Columns[i].HeaderText;
                }

                csvFileWriter.WriteLine(columnHeaderText);
                
                foreach (DataGridViewRow dataRowObject in m_gridView.Rows)
                {
                    if (!dataRowObject.IsNewRow)
                    {
                        string dataFromGrid = "";

                        dataFromGrid = dataRowObject.Cells[0].Value.ToString();
                    //    dataFromGrid = dataFromGrid + ',' + dataRowObject.Cells[1].Value.ToString();
                    //    csvFileWriter.WriteLine(dataFromGrid);
                      
                        for (int i = 1; i <= countColumn; i++)
                        {
                            dataFromGrid = dataFromGrid + ',' + dataRowObject.Cells[i].Value.ToString();
                           // dataFromGrid =  ',' + dataRowObject.Cells[i].Value.ToString();
                            csvFileWriter.WriteLine(dataFromGrid);
                        }                       
                    }
                }         
                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());
            }
        }

        private void ImportCSV(string fileName)
        {
            using (CachedCsvReader csv = new CachedCsvReader(new StreamReader(fileName), true))
            {
                m_gridView.DataSource = csv;
            }
        
            // open the file "data.csv" which is a CSV file with headers
            using (CsvReader csv =
                   new CsvReader(new StreamReader(fileName), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    for (int i = 0; i < fieldCount; i++)

                        Console.Write(string.Format("{0} = {1};",
                                      headers[i], csv[i]));
                    Console.WriteLine();

                }
            }

        }

        */
        public void ExportCSV(string fileName)
        {
            try
            {
                System.IO.StreamWriter csvFileWriter = new StreamWriter(fileName, false);

                string columnHeaderText = "";

                int countColumn = m_gridView.ColumnCount - 1;

                if (countColumn >= 0)
                {
                    columnHeaderText = m_gridView.Columns[0].HeaderText;
                }

                for (int i = 1; i <= countColumn; i++)
                {
                    columnHeaderText = columnHeaderText + ',' + m_gridView.Columns[i].HeaderText;
                }


                csvFileWriter.WriteLine(columnHeaderText);

                foreach (DataGridViewRow dataRowObject in m_gridView.Rows)
                {
                    if (!dataRowObject.IsNewRow)
                    {
                        string dataFromGrid = "";

                        dataFromGrid = dataRowObject.Cells[0].Value.ToString();
                        /*
                        bool result = false;
                        if (Boolean.TryParse(dataFromGrid, out result) && result)
                            dataFromGrid = "True";
                        else
                            dataFromGrid = "False";
                        */
                        for (int i = 1; i <= countColumn; i++)
                        {
                            dataFromGrid = dataFromGrid + ',' + dataRowObject.Cells[i].Value.ToString();
                        }

                        csvFileWriter.WriteLine(dataFromGrid);
                    }
                }


                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
            catch (Exception exceptionObject)
            {
                MessageBox.Show(exceptionObject.ToString());
            }
        }

        public void ImportCSV(string fileName, DataTable Table)
        {/*
            switch (Case)
            {
                case "Log" :
                    GetDataTable();
                    break;
                case "Command" :
                    GetDataTable4Command();
                    break;
            }
            */
            //  try
            //     {

            string file = File.ReadAllText(fileName); // dosya aciksa kitleniyor

            string[] fileInputLines = file.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            bool header = true;
            foreach (string fileInput in fileInputLines)
            {
                if (header)
                {
                    header = false;
                    continue;
                }

                DataRow dr = m_Data.NewRow();
                //       m_Data.Rows.
                string[] cells = fileInput.Split(new string[] { GridColumnDelimeter }, StringSplitOptions.None);
                for (int i = 0; i < cells.Length; i++)
                {
                    //    if( m_Data.Rows.Count >= i) // bu row sayisi
                    //   if(dr.

                    dr[i] = cells[i];  // Cannot find column 6.
                    //   dr.
                }
                m_Data.Rows.Add(dr);
            }

            m_gridView.DataSource = m_Data;
        }

    }

    public enum FileTypes
    { 
        Excel = 0,
        CSV = 1
    }
}
