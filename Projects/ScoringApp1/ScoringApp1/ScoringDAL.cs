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
using System.Threading;
using System.Diagnostics;

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
                Debug.WriteLine("SQL Exception occured in submit button click event");
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
                Debug.WriteLine("SQL Exception occured in submit button click event");
                throw;
            }
            finally
            {

                dbConnection.Close();
            }
            return dsDates;
        }

        /// <summary>
        /// Gets dates for which the data is present in the database
        /// </summary>
        [Description("Gets dates for which the data is present in the database")]
        public DataSet GetCurrentDatesWithData(String selectedMonth, String selectedYear, String previousDate)
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
                        "[DQ].[dbo].[scr_master_runs] where SUBSTRING(roxie_date,1,6) = @yearMonth and " +
                                        "CONVERT(datetime,substring(roxie_date,1,8),112)>= " +
                                        "CONVERT(datetime,substring(@previousDate,1,8),112)";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);


            dbCommand.Parameters.Add(new SqlParameter("@yearMonth", yearMonth));
            dbCommand.Parameters.Add(new SqlParameter("@previousDate", previousDate));
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            dbConnection.Open();
            try
            {
                daSelectedDates.Fill(dsDates);
            }
            catch (SqlException)
            {
                Debug.WriteLine("SQL Exception occured in submit button click event");
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

        private String createCompareID(String previousDate, String currentDate,
                                       String model, String version, String mode,
                                       String env, String restriction,
                                       String customer, String loadTime)
        {
            String compareID = "";
            String systemName = System.Environment.UserName;
            compareID = systemName + "_" + loadTime + "_" + previousDate + "_" + currentDate + "_" + model + "_" +
                        version + "_" + mode + "_" + env + "_" + restriction + "_" + customer;
            return compareID;
        }

        public void RefreshData(String previousDate, String currentDate,
                                String model, String version, String mode, String environment,
                                String restriction, String customer, String loadTime)
        {
            String compareID = createCompareID(previousDate, currentDate, model, version, mode, environment, restriction, customer, loadTime);
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String sqlScoresCompare;
            sqlScoresCompare = "SCORES_COMPARE_SCRIPT";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandTimeout = 0;
            dbCommand.Parameters.Add(new SqlParameter("@previous", previousDate));
            dbCommand.Parameters.Add(new SqlParameter("@current", currentDate));
            dbCommand.Parameters.Add(new SqlParameter("@model", model));
            dbCommand.Parameters.Add(new SqlParameter("@version", version));
            dbCommand.Parameters.Add(new SqlParameter("@mode", mode));
            dbCommand.Parameters.Add(new SqlParameter("@environment", environment));
            dbCommand.Parameters.Add(new SqlParameter("@restriction", restriction));
            dbCommand.Parameters.Add(new SqlParameter("@customer", customer));
            dbCommand.Parameters.Add(new SqlParameter("@compare_id", compareID));
            dbCommand.Parameters.Add(new SqlParameter("@load_date", loadTime));


            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                Debug.WriteLine("SQL Exception occured in submit button click event");
                throw;
            }
            finally
            {

                dbConnection.Close();
            }
        }

        #region "Excel Methods"
        public static async Task GenerateExcel()
        {

            //updating the excel document
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;
            String workbookName = "Compare_Reports_v03.xlsx";
            String folderPath = "C:\\Users\\parevi01\\Documents\\LexisNexis\\";
            String workbookPath = folderPath + workbookName;

            string physicalPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            //workbookPath = physicalPath + "Compare_Reports_v03.xlsx";
            try
            {

                Excel.Workbook sampleWorkBook = excelApp.Workbooks.Open(workbookPath, Missing.Value, false,
                                                                        Missing.Value, Missing.Value, Missing.Value,
                                                                        Missing.Value, Missing.Value, Missing.Value, true);
             //   sampleWorkBook.Connections.Add("myserver2", "", "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=True;" +
             //"Initial Catalog=DQ;Data Source=alawqpnc004, 2010;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;" +
             //"Workstation ID=ALAPAREVI01-W7D;Use Encryption for Data=False;Tag with column collation when possible=False",
             //"Select * from dbo.Scoring_Project_Attributes_test where compare_id = '6/6/2013 12:46:21 PM_20130506_20130508_Risk View_4.0_XML_Cert_FCRA_Generic'", "SQL");
                //foreach (Excel.Worksheet sheet1 in sampleWorkBook.Worksheets)
                //{
                //    Debug.WriteLine(sheet1.Name);

                //}
                Task refreshExcelTask = refreshExcelAsync(sampleWorkBook);
                //Task t = await Task.WhenAny(refreshExcelTask);
                refreshExcelTask.Wait(60000);
                #region "task execution trials"
                //try
                //{
                //    //Process(await t);

                //}
                //catch (Exception)
                //{

                //    throw;
                //}


                //while (!t.IsCompleted)
                //{
                //    Thread.Sleep(1000);
                //    //Debug.WriteLine(excelApp.StatusBar);
                //    Debug.WriteLine("Waiting on refresh task");
                //}
                #endregion
                //if (t.IsCompleted)
                //{
                //    Task saveExcelTask = saveExcelAsync(sampleWorkBook);
                //    Task t1 = await Task.WhenAny(saveExcelTask);
                //}
                Task saveExcelTask = saveExcelAsync(sampleWorkBook);
                saveExcelTask.Wait(40000);
                //sampleWorkBook.SaveAs(folderPath + "new.xlsx", Missing.Value, Missing.Value, Missing.Value, 
                //                       Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange);
                #region "task execution trials2"
                //ThreadPool.QueueUserWorkItem(new WaitCallback(refreshExcel), handle1[0]);
                //WaitHandle.WaitAll(handle1);
                //sampleWorkBook.RefreshAll();

                //System.Threading.Thread.Sleep(20000);
                //sampleWorkBook.Save();

                //saveExcelAsync(sampleWorkBook);

                //saveExcelTask.Wait();
                //while (!saveExcelTask.IsCompleted)
                //{
                //    Thread.Sleep(1000);
                //    Debug.WriteLine(excelApp.StatusBar);
                //    Debug.WriteLine("Waiting on save task");
                //}
                //System.Threading.Thread.Sleep(20000);
                #endregion
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                excelApp.Workbooks.Close();
                excelApp.Quit();


                excelApp = new Excel.Application();
                excelApp.Visible = true;
                excelApp.Workbooks.Open(workbookPath);
            }


        }

        public void RefreshExcel()
        {
            Excel.Application excelApp = new Excel.Application();
            string workbookPath = "C:\\Users\\parevi01\\Documents\\LexisNexis\\Compare_Reports_v03.xlsx";

            string physicalPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            //workbookPath = physicalPath + "Compare_Reports_v03.xlsx";
             ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            excelApp.Visible = true;
            Excel.Workbook sampleWorkBook = excelApp.Workbooks.Open(workbookPath, Missing.Value, false,
                                                                    Missing.Value, Missing.Value, Missing.Value,
                                                                    Missing.Value, Missing.Value, Missing.Value, true);
            sampleWorkBook.Connections.Add("myserver2", "", "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=True;" +
                "Initial Catalog=DQ;Data Source=alawqpnc004, 2010;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;" +
                "Workstation ID=ALAPAREVI01-W7D;Use Encryption for Data=False;Tag with column collation when possible=False",
                "Select * from dbo.Scoring_Project_Attributes_test where compare_id = '6/6/2013 12:46:21 PM_20130506_20130508_Risk View_4.0_XML_Cert_FCRA_Generic'", "SQL");

            foreach (Excel.Worksheet sheet1 in sampleWorkBook.Sheets)
            {
               // sheet1.QueryTables.Add(connString,
                
                if (sheet1.QueryTables.Count > 0)
                {
                    for (int i = 1; i <= sheet1.QueryTables.Count; i++)
                    {
                        if (!sheet1.QueryTables[i].Refresh(false))
                        {
                            Debug.WriteLine("Refresh of query table " +
                                sheet1.QueryTables[i].Name + " failed.");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("This worksheet contains no query tables.");
                }
            }
        }

        private static async Task refreshExcelAsync(Excel.Workbook sampleWorkBook)
        {
            //await Task.Factory.StartNew(() => sampleWorkBook.RefreshAll());
            Task t = new Task
            (
                () =>
                {
                    sampleWorkBook.RefreshAll();

                }
            );
            t.Start();

            await t;

            //while (!t.IsCompleted)
            //{
            //    Thread.Sleep(1000);
            //    //Debug.WriteLine(excelApp.StatusBar);
            //    Debug.WriteLine("Waiting on refresh task");
            //}
            //await t;
            Debug.WriteLine("after await");
        }

        private static async Task saveExcelAsync(Excel.Workbook sampleWorkBook)
        {
            Task t = new Task
           (
               () =>
               {
                   sampleWorkBook.Save();

               }
           );
            t.Start();
            await t;

        }

       
        #endregion

        /// <summary>
        /// gets the current date values based on previous date values
        /// </summary>
        /// <param name="previousDate"></param>
        /// <returns></returns>
        public DataSet GetCurrentDates(String previousDate)
        {
            DataSet currentDates = new DataSet();
            String sqlGetCurrentDates = "select distinct SUBSTRING(roxie_date,1,8) as currentDate " +
                                        " FROM [DQ].[dbo].[scr_master_runs] where " +
                                        "CONVERT(datetime,substring(roxie_date,1,8),112)>= " +
                                        "CONVERT(datetime,substring(@previousDate,1,8),112)";
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];

            String connString = cs.ConnectionString;

            SqlConnection dbConn = new SqlConnection(connString);
            SqlCommand dbCommand = new SqlCommand(sqlGetCurrentDates, dbConn);

            SqlDataAdapter daCurrentDates = new SqlDataAdapter(dbCommand);
            dbCommand.CommandType = CommandType.Text;
            dbCommand.Parameters.AddWithValue("@previousDate", previousDate);

            dbConn.Open();
            try
            {
                daCurrentDates.Fill(currentDates);
            }
            catch (SqlException)
            {
                Debug.WriteLine("SQL Exception occured in submit button click event");
            }
            finally
            {
                dbConn.Close();
            }
            return currentDates;
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
                Debug.WriteLine("SQL Exception occured in submit button click event");
            }
            finally
            {
                dbConnection.Close();
            }
            return dsTimeStamps;
        }
    }
}