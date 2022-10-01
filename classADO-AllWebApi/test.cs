using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classADO_AllWebApi
{
    public class test
    {
        private string conStr;
        private SqlConnection con;
        private DataTable Exceldt;

        public void connection()
        {
            conStr = @"Data Source = DESKTOP-TA8BLLL\SQLEXPRESS; Initial Catalog = CombineApi; Integrated Security = true;";
            con = new SqlConnection(conStr);
        }

        public DataSet selectData(string tableName)
        {
            con.Open();

            SqlDataAdapter adp = new SqlDataAdapter($"Select * from {tableName}", con);
            DataSet ds = new DataSet();
            adp.Fill(ds, tableName);
            return (ds);

            con.Close();

        }
//---------------------------------------------------------------------------------------------------------
        public DataSet spSelectData(string procedure)
        {
            connection();

            con.Open();

            SqlDataAdapter adp = new SqlDataAdapter(procedure, con);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return (ds);

            con.Close();

        }
//---------------------------------------------------------------------------------------------------------
        public DataSet selectDataRegistration()
        {
            con.Open();

            SqlDataAdapter adp = new SqlDataAdapter("select * from registration", con);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return (ds);

            con.Close();

        }
//--------------------------------------------------------------------------------------------------------
        public void insertDataIntoDatabase(string excelFilePath)
        {
            using (XLWorkbook workBook = new XLWorkbook(excelFilePath))
            {
                IXLWorksheet workSheet = workBook.Worksheet("Sheet1");

                Exceldt = new DataTable();

                bool firstRow = true;

                foreach (IXLRow row in workSheet.Rows())
                {
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            Exceldt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        Exceldt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            Exceldt.Rows[Exceldt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }
                }

                connection();

                con.Open();

                for (int i = 0; i < Exceldt.Rows.Count; i++)
                {
                    SqlDataAdapter adp = new SqlDataAdapter($"spEmployee '{Exceldt.Rows[i][1]}', '{Exceldt.Rows[i][2]}', '{Exceldt.Rows[i][3]}' ", con);

                    adp.SelectCommand.ExecuteNonQuery();
                }

                con.Close();
            }
        }
//--------------------------------------------------------------------------------------------------------
        public string getUniqueNumber()
        {
            string lastId, subLastId, date, uniqueId;
            int intSubLastId;

            connection();

            con.Open();

            SqlCommand cmd = new SqlCommand("spGetLastId", con);

            var temp = cmd.ExecuteScalar();

            con.Close();

            if (temp == null)
            {
                subLastId = "0001";
            }
            else
            {
                lastId = temp.ToString();

                subLastId = lastId.Substring((lastId.Length - 4), 4);

                intSubLastId = Convert.ToInt32(subLastId);

                intSubLastId++;

                subLastId = intSubLastId.ToString();

                subLastId = subLastId.PadLeft(4, '0');
            }

            date = DateTime.Now.ToString("yyMMdd");

            uniqueId = string.Concat(date, subLastId);

            return uniqueId;
        }
    }
}
