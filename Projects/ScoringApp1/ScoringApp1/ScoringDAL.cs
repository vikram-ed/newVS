using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;

namespace ScoringApp1
{
    public class ScoringDAL
    {

        public DataSet GetDatesWithData(String selectedMonth, String selectedYear)
        {
            DataSet dsDates = new DataSet();
            selectedMonth = (Convert.ToInt32(selectedMonth) < 10) ? "0" + selectedMonth : selectedMonth;
            String yearMonth = selectedYear + selectedMonth;
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String sqlScoresCompare;
            sqlScoresCompare = "select distinct SUBSTRING(roxie_date,1,8) as dateWithData FROM " +
                        "[DQ].[dbo].[scr_master_runs] where SUBSTRING(roxie_date,1,6) = @yearMonth";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);


            dbCommand.Parameters.Add(new SqlParameter("@yearMonth", yearMonth));
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            dbConnection.Open();
            try
            {
                daSelectedDates.Fill(dsDates);
            }
            catch (SqlException)
            {
                Console.WriteLine("SQL Exception occured in submit button click event");
            }
            finally
            {

                dbConnection.Close();
            }
            return dsDates;
        }

        public void RefreshData(String previousDate, String currentDate, String tableType)
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String sqlScoresCompare;
            sqlScoresCompare = "SCORES_COMPARE_SCRIPT";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);
            dbCommand.CommandType = CommandType.StoredProcedure;

            dbCommand.Parameters.Add(new SqlParameter("@previous", previousDate));
            dbCommand.Parameters.Add(new SqlParameter("@current", currentDate));
            dbCommand.Parameters.Add(new SqlParameter("@model", tableType));

            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                Console.WriteLine("SQL Exception occured in submit button click event");
            }
            finally
            {

                dbConnection.Close();
            }
        }

        public void GenerateExcel()
        {

            //updating the excel document
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false;
            string workbookPath = "C:\\Users\\parevi01\\Documents\\LexisNexis\\Compare_Reports_v03.xlsx";

            string physicalPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            //workbookPath = physicalPath + "Compare_Reports_v03.xlsx";


            Excel.Workbook sampleWorkBook = excelApp.Workbooks.Open(workbookPath, Missing.Value, false, 
                                                                    Missing.Value, Missing.Value, Missing.Value, 
                                                                    Missing.Value, Missing.Value, Missing.Value, true);

            sampleWorkBook.RefreshAll();

            System.Threading.Thread.Sleep(10000);
            sampleWorkBook.Save();
            System.Threading.Thread.Sleep(2000);
            excelApp.Workbooks.Close();
            excelApp.Quit();


            excelApp = new Excel.Application();
            excelApp.Visible = true;

            sampleWorkBook = excelApp.Workbooks.Open(workbookPath);
        }


        public DataSet GetReportGenerationTimes(String roxieDate)
        {
            String sqlGetTimeStamp = "";

            ConnectionStringSettings cs;

            sqlGetTimeStamp = "select distinct SUBSTRING(roxie_date,(LEN(roxie_date) - 5),6) as roxieTime" +
                " FROM [DQ].[dbo].[scr_master_runs] where SUBSTRING(roxie_date,1,8) = @roxieDate";

            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];

            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            DataSet dsTimeStamps = new DataSet();
            SqlCommand dbCommand = new SqlCommand(sqlGetTimeStamp, dbConnection);
            SqlDataAdapter daTimeStamps = new SqlDataAdapter(dbCommand);
            dbCommand.CommandType = CommandType.Text;
            dbCommand.Parameters.Add(new SqlParameter("@roxieDate", roxieDate));

            dbConnection.Open();
            try
            {
                daTimeStamps.Fill(dsTimeStamps);
            }
            catch (SqlException)
            {
                Console.WriteLine("SQL Exception occured in submit button click event");
            }
            finally
            {
                dbConnection.Close();
            }
            return dsTimeStamps;
        }
    }
}