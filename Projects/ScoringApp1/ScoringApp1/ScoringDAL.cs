using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;

namespace ScoringApp1
{
    public class ScoringDAL
    {
        /// <summary>
        /// Gets all the default values for the rvscores model
        /// </summary>
        /// <returns></returns>
        [Description("Gets all the default values for the model specified")]
        public Dictionary<String, ArrayList> GetDefaultLists(String model)
        {
            Dictionary<String, ArrayList> rvLists = new Dictionary<string, ArrayList>();
            DataSet dsDates = new DataSet();
            
            ArrayList alVersion = new ArrayList();
            ArrayList alMode = new ArrayList();
            ArrayList alEnv = new ArrayList();
            ArrayList alRestriction = new ArrayList();
            ArrayList alCustomer = new ArrayList();
          
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String sqlScoresCompare;
            sqlScoresCompare = "  select distinct version,mode,environment,fcra_nonfcra," +
                                "customer from scr_master_runs where product= @model ";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);
            dbCommand.Parameters.AddWithValue("@model", model);
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            dbConnection.Open();
            try
            {
                daSelectedDates.Fill(dsDates);
                foreach (DataRow row1 in dsDates.Tables[0].Rows)
                {
                    alVersion.Add(row1["version"].ToString());
                    alMode.Add(row1["mode"].ToString());
                    alMode.Add(row1["environment"].ToString());
                    alRestriction.Add(row1["fcra_nonfcra"].ToString());
                    alMode.Add(row1["customer"].ToString());  
                }
                rvLists.Add("Version", alVersion);
                rvLists.Add("Mode", alMode);
                rvLists.Add("Environment", alEnv);
                rvLists.Add("Restriction", alRestriction);
                rvLists.Add("Customer", alCustomer);
            }
            catch (SqlException)
            {
                Console.WriteLine("SQL Exception occured in submit button click event");
                throw;
            }
            finally
            {

                dbConnection.Close();
                
            }
            return rvLists;
        }

        /// <summary>
        /// Gets dates for which the data is present in the database
        /// </summary>
        [Description("Gets dates for which the data is present in the database")]
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
                throw;
            }
            finally
            {

                dbConnection.Close();
            }
            return dsDates;
        }

        public async Task RefreshDataAsync(String previousDate, String currentDate, String tableType)
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            String sqlScoresCompare;
            sqlScoresCompare = "SCORES_COMPARE_SCRIPT";
            using (SqlConnection dbConnection = new SqlConnection(connString))
            using (SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection))
            {
                try
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add(new SqlParameter("@previous", previousDate));
                    dbCommand.Parameters.Add(new SqlParameter("@current", currentDate));
                    dbCommand.Parameters.Add(new SqlParameter("@model", tableType));

                    await dbConnection.OpenAsync();

                    await dbCommand.ExecuteNonQueryAsync();
                    dbConnection.Close();
                }
                catch (Exception ex)
                {
                    // Handle exception here.
                }
            }
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
            dbCommand.CommandTimeout = 180;
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
                throw;
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
            excelApp.Visible = true;
            string workbookPath = "C:\\Users\\parevi01\\Documents\\LexisNexis\\Compare_Reports_v03.xlsx";

            string physicalPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            //workbookPath = physicalPath + "Compare_Reports_v03.xlsx";


            Excel.Workbook sampleWorkBook = excelApp.Workbooks.Open(workbookPath, Missing.Value, false, 
                                                                    Missing.Value, Missing.Value, Missing.Value, 
                                                                    Missing.Value, Missing.Value, Missing.Value, true);

            
            
            sampleWorkBook.RefreshAll();

            System.Threading.Thread.Sleep(20000);
            sampleWorkBook.Save();
            System.Threading.Thread.Sleep(20000);
            //excelApp.Workbooks.Close();
            //excelApp.Quit();


            //excelApp = new Excel.Application();
            //excelApp.Visible = true;

            //sampleWorkBook = excelApp.Workbooks.Open(workbookPath);
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