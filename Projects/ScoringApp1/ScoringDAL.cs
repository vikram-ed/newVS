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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using ScoringApp1.helperClasses;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.Dxf;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.ConditionalFormatting;
using System.Security.Principal;


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
            sqlScoresCompare = "  select distinct version from scr_master_runs where product= @model and version is not null; select " +
                                " distinct mode from scr_master_runs where product=@model and mode is not null; select " +
                                " distinct environment from scr_master_runs where product=@model and environment is not null; " +
                                "select distinct fcra_nonfcra from scr_master_runs  where product=@model and fcra_nonfcra is not null; " +
                                "select distinct customer from scr_master_runs  where product=@model and customer is not null ";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);
            dbCommand.Parameters.AddWithValue("@model", model);
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
                dbConnection.Open();
                try
                {
                    daSelectedDates.Fill(dsDates);
                    alVersion.Add("Version");
                    alMode.Add("Mode");
                    alEnv.Add("Environment");
                    alRestriction.Add("Restriction");
                    alCustomer.Add("Customer");
                    foreach (DataRow row1 in dsDates.Tables[0].Rows)
                    {
                        alVersion.Add("v" + row1["version"].ToString());
                    }
                    foreach (DataRow row1 in dsDates.Tables[1].Rows)
                    {
                        alMode.Add(row1["mode"].ToString());
                    }
                    foreach (DataRow row1 in dsDates.Tables[2].Rows)
                    {
                        alEnv.Add(row1["environment"].ToString());
                    }
                    foreach (DataRow row1 in dsDates.Tables[3].Rows)
                    {
                        alRestriction.Add(row1["fcra_nonfcra"].ToString());
                    }
                    foreach (DataRow row1 in dsDates.Tables[4].Rows)
                    {
                        alCustomer.Add(row1["customer"].ToString());
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
            }
            return rvLists;
        }

        /// <summary>
        /// Gets dates for which the data is present in the database
        /// </summary>
        [Description("Gets dates for which the data is present in the database")]
        public DataSet GetDatesWithData(String selectedMonth, String selectedYear, String modelName,
                                                                    String version, String mode)
        {
            DataSet dsDates = new DataSet();
            selectedMonth = (Convert.ToInt32(selectedMonth) < 10) ? "0" + selectedMonth : selectedMonth;
            String yearMonth = selectedYear + selectedMonth;
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String sqlScoresCompare;
            sqlScoresCompare = "with base_cte as (select roxie_date, version, case when " +
                "scores_attributes IN ('Attributes','Scores') then 1 else 0 end as scores_attributes_test " +
                "from scr_master_runs where mode=@mode and version=@version and product=@product) " +
                "select distinct SUBSTRING(roxie_date,1,8) as dateWithData from " +
                "base_cte group by version,roxie_date having SUM(scores_attributes_test) = 2";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);

            dbCommand.Parameters.Add(new SqlParameter("@yearMonth", yearMonth));
            dbCommand.Parameters.Add(new SqlParameter("@product", modelName));

            dbCommand.Parameters.Add(new SqlParameter("@version", version));

            dbCommand.Parameters.Add(new SqlParameter("@mode", mode));
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
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
            }
            return dsDates;
        }

        /// <summary>
        /// Gets dates for which the data is present in the database
        /// </summary>
        [Description("Gets dates for which the data is present in the database")]
        public DataSet GetCurrentDatesWithData(String selectedMonth, String selectedYear, String previousDate, 
                                                                                   String modelName, String version, String mode)
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
                                        "CONVERT(datetime,substring(@previousDate,1,8),112) and " +
                                        "product = @product and version = @version and mode = @mode";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);


            dbCommand.Parameters.Add(new SqlParameter("@yearMonth", yearMonth));
            dbCommand.Parameters.Add(new SqlParameter("@previousDate", previousDate));
            dbCommand.Parameters.Add(new SqlParameter("@product", modelName));
            dbCommand.Parameters.Add(new SqlParameter("@version", version));
            dbCommand.Parameters.Add(new SqlParameter("@mode", mode));
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
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
            }
            return dsDates;
        }

        //database queries to get previous compare IDs and implmenting filters on that data
        public List<String> getPreviousCompareIDs(String userName)
        {
            List<String> compareIDsList = new List<String>();
            DataSet dsCompareIDs = new DataSet();

            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String getCompareIDsQuery;
            getCompareIDsQuery = " select distinct compare_id from Scoring_Project_score_reason_codes_test where [user_name] = @userName";

            SqlCommand dbCommand = new SqlCommand(getCompareIDsQuery, dbConnection);
            dbCommand.Parameters.AddWithValue("@userName", userName);
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
                dbConnection.Open();
                try
                {
                    daSelectedDates.Fill(dsCompareIDs);
                    foreach (DataRow row1 in dsCompareIDs.Tables[0].Rows)
                    {
                        compareIDsList.Add(row1["compare_id"].ToString());
                    }
                }
                catch (SqlException)
                {
                    Debug.WriteLine("SQL Exception occured in getPreviousCompareIDs");
                    throw;
                }
                finally
                {

                    dbConnection.Close();

                }
            }
            return compareIDsList;
        }

        public Dictionary<String, String> getAllExistingCompareIDs()
        {
            Dictionary<String, String> compareIDs = new Dictionary<string, String>();
            
            DataSet dsCompareIDs = new DataSet();

            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String getCompareIDsQuery;
            getCompareIDsQuery = " select distinct compare_id, user_name from Scoring_Project_score_reason_codes_test";

            SqlCommand dbCommand = new SqlCommand(getCompareIDsQuery, dbConnection);
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
                dbConnection.Open();
                try
                {
                    daSelectedDates.Fill(dsCompareIDs);
                    foreach (DataRow row1 in dsCompareIDs.Tables[0].Rows)
                    {
                        //compareIDsList.Add(row1["compare_id"].ToString());
                        compareIDs.Add( row1["compare_id"].ToString(),row1["user_name"].ToString());
                    }
                }
                catch (SqlException)
                {
                    Debug.WriteLine("SQL Exception occured in getAllExistingCompareIDs");
                    throw;
                }
                finally
                {

                    dbConnection.Close();

                }
            }
            return compareIDs;
        }

        public void  DeleteCompareID(String compareID, String userName)
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String getCompareIDsQuery;
            getCompareIDsQuery = " delete from Scoring_Project_score_reason_codes_test where compare_id=@compareID and user_name=@userName; " +
                "delete from Scoring_Project_scores_test where compare_id=@compareID and user_name=@userName;" +
                "delete from Scoring_Project_attributes_test where compare_id=@compareID and user_name=@userName";

            SqlCommand dbCommand = new SqlCommand(getCompareIDsQuery, dbConnection);
            dbCommand.Parameters.Add(new SqlParameter("@compareID", compareID));
            dbCommand.Parameters.Add(new SqlParameter("@userName",userName));
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    Debug.WriteLine("SQL Exception occured in DeleteCompareID");
                    throw;
                }
                finally
                {

                    dbConnection.Close();

                }
            }
        }
        public List<String> getFilteredExistingCompareIDs(String filter)
        {
            List<String> compareIDsList = new List<String>();
            DataSet dsCompareIDs = new DataSet();

            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String getCompareIDsQuery;
            getCompareIDsQuery = " select distinct compare_id from Scoring_Project_score_reason_codes_test" + filter;

            SqlCommand dbCommand = new SqlCommand(getCompareIDsQuery, dbConnection);
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
                dbConnection.Open();
                try
                {
                    daSelectedDates.Fill(dsCompareIDs);
                    foreach (DataRow row1 in dsCompareIDs.Tables[0].Rows)
                    {
                        compareIDsList.Add(row1["compare_id"].ToString());
                    }
                }
                catch (SqlException)
                {
                    Debug.WriteLine("SQL Exception occured in getAllExistingCompareIDs");
                    throw;
                }
                finally
                {

                    dbConnection.Close();

                }
            }
            return compareIDsList;
        }


        public List<String> getOtherUsersPreviousCompareIDs(String userName)
        {
            List<String> compareIDsList = new List<String>();
            DataSet dsCompareIDs = new DataSet();

            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String getCompareIDsQuery;
            getCompareIDsQuery = " select distinct compare_id from Scoring_Project_score_reason_codes_test where [user_name] not in (@userName, 'System')";

            SqlCommand dbCommand = new SqlCommand(getCompareIDsQuery, dbConnection);
            dbCommand.Parameters.AddWithValue("@userName", userName);
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
                dbConnection.Open();
                try
                {
                    daSelectedDates.Fill(dsCompareIDs);
                    foreach (DataRow row1 in dsCompareIDs.Tables[0].Rows)
                    {
                        compareIDsList.Add(row1["compare_id"].ToString());
                    }
                }
                catch (SqlException)
                {
                    Debug.WriteLine("SQL Exception occured in getPreviousCompareIDs");
                    throw;
                }
                finally
                {

                    dbConnection.Close();

                }
            }
            return compareIDsList;
        }

        public Boolean checkCompareIDExists(String compareID)
        {
            Boolean IDExists;
            
            DataSet dsCompareIDs = new DataSet();

            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String getCompareIDsQuery;
            getCompareIDsQuery = " select compare_id from Scoring_Project_score_reason_codes_test where compare_id = @compareID";

            SqlCommand dbCommand = new SqlCommand(getCompareIDsQuery, dbConnection);
            dbCommand.Parameters.AddWithValue("@compareID", compareID);
            SqlDataAdapter daSelectedDates = new SqlDataAdapter(dbCommand);
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
                dbConnection.Open();
                try
                {
                    daSelectedDates.Fill(dsCompareIDs);
                    if (dsCompareIDs.Tables[0].Rows.Count == 0)
                    {
                        IDExists = false;
                    }
                    else IDExists = true;
                }
                catch (SqlException)
                {
                    Debug.WriteLine("SQL Exception occured in checkCompareIDExists");
                    throw;
                }
                finally
                {

                    dbConnection.Close();

                }
            }
            return IDExists;

        }
      
      /// <summary>
      /// main method for insertion into the database
      /// </summary>
        public void RefreshData(String previousDate, String currentDate,
                                String model, String version, String mode,
                                String environment, String restriction, 
                                String customer, String loadTime, 
                                String userName, String compareID)
        {

            String excelFile = HttpContext.Current.Server.MapPath("Archive" + "\\" + compareID + ".xlsx");
            //if (!checkCompareIDExists(compareID))
            {
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
                dbCommand.Parameters.Add(new SqlParameter("@uName", userName));

                using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
                {
                    dbConnection.Open();
                    try
                    {
                        //await dbCommand.ExecuteNonQueryAsync();
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
            }
        }

        #region "Unused methods"
        public DataSet getReasonCodesSheet()
        {
            DataSet reasonCodes = new DataSet();
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            String sqlScoresCompare;
            sqlScoresCompare = "select * from SCORING_PROJECT_REASON_CODES_VIEW";

            SqlCommand dbCommand = new SqlCommand(sqlScoresCompare, dbConnection);
            dbCommand.CommandTimeout = 0;


            dbConnection.Open();
            try
            {
                reasonCodes = (DataSet)dbCommand.ExecuteScalar();
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
            return reasonCodes;
        }

        private void openExcel()
        {
            try
            {
                string XlsPath = HttpContext.Current.Server.MapPath("Compare_Reports_v03.xlsx");
                FileInfo fileDet = new System.IO.FileInfo(XlsPath);

                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Charset = "UTF-8";
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpContext.Current.Server.UrlEncode(fileDet.Name));
                HttpContext.Current.Response.AddHeader("Content-Length", fileDet.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //HttpContext.Current.Response.WriteFile(fileDet.FullName);
                HttpContext.Current.Response.TransmitFile(fileDet.FullName);
                HttpContext.Current.Response.End();

                //System.IO.FileStream fs = null;
                //fs = System.IO.File.Open(XlsPath, System.IO.FileMode.Open);

                //byte[] btFile = new byte[fs.Length];
                //fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                //fs.Close();
                //HttpContext.Current.Response.Buffer = true;
                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.ClearContent();
                //HttpContext.Current.Response.ClearHeaders();
                //HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + XlsPath);
                //HttpContext.Current.Response.Headers.Set("Cache-Control", "private, max-age=0");
                //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //HttpContext.Current.Response.BinaryWrite(btFile);
                //HttpContext.Current.Response.End();

                // tbl1 = (Table)reasonCodes.Tables[0];


            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public void RefreshExcel(object state)
        {
            Excel.Application excelApp = new Excel.Application();
            String workbookPath1 = HttpContext.Current.Server.MapPath("Compare_Reports_v03.xlsx");
            string workbookPath = "C:\\Users\\parevi01\\Documents\\LexisNexis\\Compare_Reports_v03.xlsx";

            string physicalPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            //workbookPath = physicalPath + "Compare_Reports_v03.xlsx";

            excelApp.Visible = true;
            Excel.Workbook sampleWorkBook = excelApp.Workbooks.Open(workbookPath, Missing.Value, false,
                                                                    Missing.Value, Missing.Value, Missing.Value,
                                                                    Missing.Value, Missing.Value, Missing.Value, true);

            foreach (Excel.Worksheet sheet1 in sampleWorkBook.Sheets)
            {
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

        //public static Task GenerateExcelAsync()
        //{

        //    return Task.Factory.StartNew(() => GenerateExcel());
        //}


        
        //public async Task GenerateExcel()
        //{

        //    //updating the excel document
        //    Excel.Application excelApp = new Excel.Application();
        //    excelApp.Visible = true;
        //    //string workbookPath = "C:\\Users\\parevi01\\Documents\\LexisNexis\\Compare_Reports_v03.xlsx";
        //    String workbookPath = HttpContext.Current.Server.MapPath("Compare_Reports_v03.xlsx");
        //    string physicalPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
        //    //workbookPath = physicalPath + "Compare_Reports_v03.xlsx";
        //    //DataSet reasonCodes = getReasonCodesSheet();
        //    try
        //    {
        //        //excelApp.Visible = true;
        //        //Excel.Workbook sampleWorkBook = excelApp.Workbooks.Open(workbookPath, Missing.Value, false,
        //        //                                                        Missing.Value, Missing.Value, Missing.Value,
        //        //                                                        Missing.Value, Missing.Value, Missing.Value, true);

        //        //sampleWorkBook.RefreshAll();
        //        //Thread.Sleep(70000);

        //        //Table tbl1 = new Table();
        //        //using (StringWriter sw = new StringWriter())
        //        //using (HtmlTextWriter htw = new HtmlTextWriter(sw))
        //        //{

        //        //   // tbl1 = (Table)reasonCodes.Tables[0];
        //        //}

        //        //openExcel();
        //        await createExcelPackage();
        //        //ProcessRequest(HttpContext.Current);

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    finally
        //    {

        //    }


        //}
#endregion


        #region "Excel Methods"
        public async Task getSummaryReport(String summaryPath)
        {
            try
            {
                //FileInfo template = new FileInfo(HttpContext.Current.Server.MapPath(@"template.xlsx"));
                
                if (!File.Exists(HttpContext.Current.Server.MapPath(summaryPath)))
                {
                    FileInfo newFile = new FileInfo(HttpContext.Current.Server.MapPath(summaryPath));
                    // Using the template to create the newFile...
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {
                      
                        List<AttributesSummary> attrSummaryList = GetAttributesSummary();
                        List<ScoresSummary> scoresSummaryList = GetScoresSummary();
                        List<RCSummary> rcSummaryList = GetReasonCodesSummary();
                        List<ScoresAttributesSummary> scoresAttrSummaryList = GetScoresAttributesSummary();
                        List<ScoresModelSummary> scoresModelSummaryList = GetScoresModelSummary();

                        // Getting the complete workbook...
                        ExcelWorkbook myWorkbook = excelPackage.Workbook;
                       
                        var attributesSummarySheet = excelPackage.Workbook.Worksheets.Add("AttributesSummary");
                        var scoresSummarySheet = excelPackage.Workbook.Worksheets.Add("ScoresSummary");
                        var rcSummarySheet = excelPackage.Workbook.Worksheets.Add("ReasonCodesSummary");
                        var scoresAttributesSummarySheet = excelPackage.Workbook.Worksheets.Add("ScoresAttributes");
                        var scoresModelSummarySheet = excelPackage.Workbook.Worksheets.Add("ScoresModel");
                  
                        await loadAttributesSummarySheet(attrSummaryList, attributesSummarySheet);
                        await loadScoresSummarySheet(scoresSummaryList, scoresSummarySheet);
                        await loadRCSummarySheet(rcSummaryList, rcSummarySheet);
                        await loadScoresAttributesSummarySheet(scoresAttrSummaryList, scoresAttributesSummarySheet);
                        await loadScoresModelSummarySheet(scoresModelSummaryList, scoresModelSummarySheet);
                  
                        // Saving the change...
                        excelPackage.Save();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task getDetailedReport(String detailedPath)
        {
            try
            {
                //FileInfo template = new FileInfo(HttpContext.Current.Server.MapPath(@"template.xlsx"));
                
                if (!File.Exists(HttpContext.Current.Server.MapPath(detailedPath)))
                {
                    FileInfo newFile = new FileInfo(HttpContext.Current.Server.MapPath(detailedPath));
                    using (ExcelPackage excelPackage = new ExcelPackage(newFile))
                    {
                        List<ScoresValues> scoresList = GetScoresData();
                        List<AttributesValues> attrList = GetAttributesData();
                        List<ReasonCodeValues> rcList = GetReasonCodesData();

                        // Getting the complete workbook...
                        ExcelWorkbook myWorkbook = excelPackage.Workbook;
                        // Getting the worksheet by its name... 
                        //ExcelWorksheet myWorksheet = myWorkbook.Worksheets["Scores"];
                        var myWorksheet = excelPackage.Workbook.Worksheets.Add("Scores");
                        var attributesSheet = excelPackage.Workbook.Worksheets.Add("Attributes");
                        var reasonCodesSheet = excelPackage.Workbook.Worksheets.Add("ReasonCodes");


                        await loadScoresSheet(scoresList, myWorksheet);
                        await loadAttributesSheet(attrList, attributesSheet);
                        await loadReasonCodesSheet(rcList, reasonCodesSheet);

                        // Saving the change...
                        excelPackage.Save();

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static async Task loadScoresSheet(List<ScoresValues> scoresList, ExcelWorksheet scoresSheet)
        {
            Task t = new Task
           (
               () =>
               {
                   scoresSheet.Cells["A1"].LoadFromCollection(
                        from s in scoresList
                        select s, true, OfficeOpenXml.Table.TableStyles.Medium2);
                   scoresSheet.Cells.AutoFitColumns();

               }
           );
            t.Start();
            await t;

        }
        private static async Task loadAttributesSheet(List<AttributesValues> attributesList, ExcelWorksheet attributesSheet)
        {
            Task t = new Task
           (
               () =>
               {
                   var dataRange = attributesSheet.Cells["A1"].LoadFromCollection(
                        from s in attributesList
                        select s, true, OfficeOpenXml.Table.TableStyles.Medium2);
                   attributesSheet.Cells.AutoFitColumns();

                   //var wsAttributesPivot = attributesSheet.Workbook.Worksheets.Add("AttributesPivot");
                   //var ptAttributes = wsAttributesPivot.PivotTables.Add(wsAttributesPivot.Cells["A3"], dataRange, "tblAttributes");
                   //ptAttributes.ShowHeaders = true;

                   //var modelFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["Model"]);
                   //var versionFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["Version"]);
                   //var modeFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["Mode"]);
                   //var envFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["Env"]);
                   //var restrictionFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["Restriction"]);
                   //var customerFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["Customer"]);
                   //var prevRunDateFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["PrevRunDate"]);
                   //var currentRunDateFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["CurrentRunDate"]);
                   //var indicatorFilter = ptAttributes.PageFields.Add(ptAttributes.Fields["Indicator"]);

                   
                   //ExcelPivotTableField rowlblAccountNbr = ptAttributes.RowFields.Add(ptAttributes.Fields["AccountNbr"]);
                   //rowlblAccountNbr.Outline = false;
                   //rowlblAccountNbr.SubTotalFunctions = eSubTotalFunctions.None;

                   //ExcelPivotTableField rowlblAttrName = ptAttributes.RowFields.Add(ptAttributes.Fields["AttributeName"]);
                   //rowlblAttrName.Outline = false;
                   //rowlblAttrName.SubTotalFunctions = eSubTotalFunctions.None;

                   //ExcelPivotTableField rowlblPrevValue = ptAttributes.RowFields.Add(ptAttributes.Fields["PreviousValue"]);
                   //rowlblPrevValue.Outline = false;
                   //rowlblPrevValue.SubTotalFunctions = eSubTotalFunctions.None;

                   //ExcelPivotTableField rowlblCurrentValue = ptAttributes.RowFields.Add(ptAttributes.Fields["CurrentValue"]);
                   //rowlblCurrentValue.Outline = false;
                   //rowlblCurrentValue.SubTotalFunctions = eSubTotalFunctions.None;


                   //ptAttributes.DataOnRows = false;

               }
           );
            t.Start();
            await t;

        }
        private static async Task loadReasonCodesSheet(List<ReasonCodeValues> reasonCodesList, ExcelWorksheet reasonCodesSheet)
        {
            Task t = new Task
           (
               () =>
               {
                   reasonCodesSheet.Cells["A1"].LoadFromCollection(
                        from s in reasonCodesList
                        select s, true, OfficeOpenXml.Table.TableStyles.Medium2);
                   reasonCodesSheet.Cells.AutoFitColumns();

               }
           );
            t.Start();
            await t;

        }
        private static async Task loadAttributesSummarySheet(List<AttributesSummary> attrSummaryList, ExcelWorksheet attrSummarySheet)
        {
            Task t = new Task
           (
               () =>
               {
                   var dataRange = attrSummarySheet.Cells["A1"].LoadFromCollection(
                        from s in attrSummaryList
                        orderby s.DiffCountPercent descending
                        select s, true, OfficeOpenXml.Table.TableStyles.Medium2);
                   attrSummarySheet.Cells.AutoFitColumns();

                   ExcelAddress percentChange = new ExcelAddress(2, 3, dataRange.End.Row, 3);
                   var percentChangeRule = attrSummarySheet.ConditionalFormatting.AddDatabar(percentChange, System.Drawing.Color.Green);

               }
           );
            t.Start();
            await t;

        }
        private static async Task loadScoresAttributesSummarySheet(List<ScoresAttributesSummary> scoresAttrList, ExcelWorksheet scoresAttrSheet)
        {
            Task t = new Task
           (
               () =>
               {
                   var dataRange = scoresAttrSheet.Cells["A1"].LoadFromCollection(
                        from s in scoresAttrList orderby s.AttrChangeCount descending
                        select s, true, OfficeOpenXml.Table.TableStyles.Medium2);
                   scoresAttrSheet.Cells.AutoFitColumns();

                   ExcelAddress percentChange = new ExcelAddress(2, 12,dataRange.End.Row,12);
                   var percentChangeRule = scoresAttrSheet.ConditionalFormatting.AddDatabar(percentChange, System.Drawing.Color.Green);
                   
               }
           );
            t.Start();
            await t;

        }
        private static async Task loadScoresSummarySheet(List<ScoresSummary> scoresSummaryList, ExcelWorksheet scoresSummarySheet)
        {
            Task t = new Task
           (
               () =>
               {
                   var dataRange = scoresSummarySheet.Cells["A1"].LoadFromCollection(
                        from s in scoresSummaryList orderby s.AbsoluteDiffCount descending
                        select s, true, OfficeOpenXml.Table.TableStyles.Medium2);
                   dataRange.AutoFitColumns();

                   ExcelAddress percentChange = new ExcelAddress(2, 6, dataRange.End.Row, 6);
                   var percentChangeRule = scoresSummarySheet.ConditionalFormatting.AddDatabar(percentChange, System.Drawing.Color.Green);

                   var wsScoresSummPivot = scoresSummarySheet.Workbook.Worksheets.Add("ScoresSummaryPivotTable");
                   var ptScoresSumm = wsScoresSummPivot.PivotTables.Add(wsScoresSummPivot.Cells["A3"], dataRange, "tblScoresSummary");
                   var reportFilter = ptScoresSumm.PageFields.Add(ptScoresSumm.Fields["ModelName"]);

                   var dfPrevCount = ptScoresSumm.DataFields.Add(ptScoresSumm.Fields["PrevCount"]);
                   var dfCurrentCount = ptScoresSumm.DataFields.Add(ptScoresSumm.Fields["CurrentCount"]);

                   dfPrevCount.Function = OfficeOpenXml.Table.PivotTable.DataFieldFunctions.Sum;
                   dfPrevCount.Name = "Sum of PrevCount";
                   dfCurrentCount.Function = OfficeOpenXml.Table.PivotTable.DataFieldFunctions.Sum;
                   dfCurrentCount.Name = "Sum of CurrentCount";

                   var fldScoreRange = ptScoresSumm.Fields["ScoreRange"];
                   fldScoreRange.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;
                   ptScoresSumm.RowFields.Add(fldScoreRange);
                   
                   ptScoresSumm.DataOnRows = false;

                   var chrtScoresSumm = wsScoresSummPivot.Drawings.AddChart("chrtScoresSummary", OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered, ptScoresSumm);
                   //chrtScoresSumm.PlotArea.Fill.Style = eFillStyle.GradientFill;

                   chrtScoresSumm.SetPosition(3, 0, 4, 0);
                   chrtScoresSumm.SetSize(1280, 760);

               }
           );
            t.Start();
            await t;

        }
        private static async Task loadScoresModelSummarySheet(List<ScoresModelSummary> scoresModelSummaryList, ExcelWorksheet scoresModelSummarySheet)
        {
            Task t = new Task
           (
               () =>
               {
                   var dataRange = scoresModelSummarySheet.Cells["A1"].LoadFromCollection(
                        from s in scoresModelSummaryList
                        orderby s.PercentChange descending
                        select s, true, OfficeOpenXml.Table.TableStyles.Medium2);
                   scoresModelSummarySheet.Cells.AutoFitColumns();
                   ExcelAddress percentChange = new ExcelAddress(2, 4,dataRange.End.Row,4);
                   var percentChangeRule = scoresModelSummarySheet.ConditionalFormatting.AddDatabar(percentChange, System.Drawing.Color.Green);


               }
           );
            t.Start();
            await t;

        }
        private static async Task loadRCSummarySheet(List<RCSummary> rcSummaryList, ExcelWorksheet rcSummarySheet)
        {
            Task t = new Task
           (
               () =>
               {
                   var dataRange = rcSummarySheet.Cells["A1"].LoadFromCollection(
                        from s in rcSummaryList orderby s.AbsoluteDiff descending
                        select s, true, OfficeOpenXml.Table.TableStyles.Medium2);
                   rcSummarySheet.Cells.AutoFitColumns();


                   ExcelAddress percentChange = new ExcelAddress(2, 6, dataRange.End.Row, 6);
                   var percentChangeRule = rcSummarySheet.ConditionalFormatting.AddDatabar(percentChange, System.Drawing.Color.Green);

               }
           );
            t.Start();
            await t;

        }

        private static List<ScoresValues> GetScoresData()
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            var ret = new List<ScoresValues>();
            // lets connect to the AdventureWorks sample database for some data
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT [user_name], [compare_id],[Account_no],[model_name],[previous_value]" +
                    ",[current_value],[difference_value],[difference_value_range], [absolute_difference],[diff_value_percent]" +
                    ",[difference_flag],[previous_range],[current_range],[range_difference_flag],[Model],[Version],[Mode]" +
                    ",[Environment],[Restriction],[prev_rundate],[curr_rundate],[customer],[load_date] FROM [DQ].[dbo].[SCORING_PROJECT_SCORES_VIEW]", sqlConn))
                {
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        //Get the data and fill rows 5 onwards
                        while (sqlReader.Read())
                        {

                            //ret.Add(sqlReader["compare_id"].ToString());
                            ret.Add(new ScoresValues
                            {
                                //UserName = sqlReader["user_name"] == DBNull.Value ? "" : sqlReader["user_name"].ToString(),
                                //CompareID = sqlReader["compare_id"] == DBNull.Value ? "" : sqlReader["compare_id"].ToString(),
                                AccountNbr = sqlReader["Account_no"] == DBNull.Value ? "" : sqlReader["Account_no"].ToString(),
                                ModelName = sqlReader["model_name"] == DBNull.Value ? "" : sqlReader["model_name"].ToString(),
                                PreviousValue = Convert.ToInt32((sqlReader["previous_value"] == DBNull.Value) ? "0" : sqlReader["previous_value"]),
                                CurrentValue = Convert.ToInt32((sqlReader["current_value"] == DBNull.Value) ? "0" : sqlReader["current_value"]),
                                DifferenceValue = Convert.ToInt32((sqlReader["difference_value"] == DBNull.Value) ? "0" : sqlReader["difference_value"]),
                                DifferenceValueRange = (sqlReader["difference_value_range"] == DBNull.Value) ? "" : sqlReader["difference_value_range"].ToString(),
                                AbsoluteDiff = Convert.ToInt32(sqlReader["absolute_difference"] == DBNull.Value ? "0" : sqlReader["absolute_difference"]),
                                DiffValuePercent = Convert.ToDecimal(sqlReader["diff_value_percent"] == DBNull.Value ? "0" : sqlReader["diff_value_percent"]),
                                DifferenceFlag = Convert.ToInt32(sqlReader["difference_flag"] == DBNull.Value ? "0" : sqlReader["difference_flag"]),
                                PrevRange = sqlReader["previous_range"] == DBNull.Value ? "" : sqlReader["previous_range"].ToString(),
                                CurrentRange = sqlReader["current_range"] == DBNull.Value ? "" : sqlReader["current_range"].ToString(),
                                RangeDiffFlag = Convert.ToInt32(sqlReader["range_difference_flag"] == DBNull.Value ? "0" : sqlReader["range_difference_flag"]),
                                Model = sqlReader["model"] == DBNull.Value ? "" : sqlReader["model"].ToString(),
                                Version = sqlReader["version"] == DBNull.Value ? "" : sqlReader["version"].ToString(),
                                Mode = sqlReader["Mode"] == DBNull.Value ? "" : sqlReader["Mode"].ToString(),
                                Env = sqlReader["Environment"] == DBNull.Value ? "" : sqlReader["Environment"].ToString(),
                                Restriction = sqlReader["Restriction"] == DBNull.Value ? "" : sqlReader["Restriction"].ToString(),
                                PrevRunDate = sqlReader["prev_rundate"] == DBNull.Value ? "" : sqlReader["prev_rundate"].ToString(),
                                CurrentRunDate = sqlReader["curr_rundate"] == DBNull.Value ? "" : sqlReader["curr_rundate"].ToString(),
                                Customer = sqlReader["customer"] == DBNull.Value ? "" : sqlReader["customer"].ToString(),
                                LoadDate = sqlReader["load_date"] == DBNull.Value ? "" : sqlReader["load_date"].ToString()

                            });
                        }
                    }
                }
            }
            return ret;
        }

        private static List<AttributesValues> GetAttributesData()
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            var ret = new List<AttributesValues>();
            // lets connect to the AdventureWorks sample database for some data
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT [user_name] ,[compare_id], " +
                    "[Account_no] ,[attribute_name],[previous_value],[current_value],[difference_value]," +
                    "[absolute_difference],[proportion],[difference_flag],[Indicator],[Model],[Version]," +
                    "[Mode],[Environment],[Restriction] ,[prev_rundate],[curr_rundate],[customer],[load_date] " +
                    "FROM [DQ].[dbo].[SCORING_PROJECT_ATTRIBUTES_VIEW]", sqlConn))
                {
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        //Get the data and fill rows 5 onwards
                        while (sqlReader.Read())
                        {

                            //ret.Add(sqlReader["compare_id"].ToString());
                            ret.Add(new AttributesValues
                            {
                                //UserName = sqlReader["user_name"] == DBNull.Value ? "" : sqlReader["user_name"].ToString(),
                               // CompareID = sqlReader["compare_id"] == DBNull.Value ? "" : sqlReader["compare_id"].ToString(),
                                AccountNbr = sqlReader["Account_no"] == DBNull.Value ? "" : sqlReader["Account_no"].ToString(),
                                AttributeName = sqlReader["attribute_name"] == DBNull.Value ? "" : sqlReader["attribute_name"].ToString(),
                                PreviousValue = (sqlReader["previous_value"] == DBNull.Value) ? "" : sqlReader["previous_value"].ToString(),
                                CurrentValue = (sqlReader["current_value"] == DBNull.Value) ? "" : sqlReader["current_value"].ToString(),
                                DiffValue = Convert.ToDecimal((sqlReader["difference_value"] == DBNull.Value) ? "0" : sqlReader["difference_value"]),
                                AbsoluteDiff = Convert.ToDecimal(sqlReader["absolute_difference"] == DBNull.Value ? "0" : sqlReader["absolute_difference"]),
                                Proportion = Convert.ToDecimal(sqlReader["proportion"] == DBNull.Value ? "0" : sqlReader["proportion"]),
                                DiffFlag = Convert.ToInt32(sqlReader["difference_flag"] == DBNull.Value ? "0" : sqlReader["difference_flag"]),
                                Indicator = sqlReader["Indicator"] == DBNull.Value ? "" : sqlReader["Indicator"].ToString(),
                                Model = sqlReader["model"] == DBNull.Value ? "" : sqlReader["model"].ToString(),
                                Version = sqlReader["version"] == DBNull.Value ? "" : sqlReader["version"].ToString(),
                                Mode = sqlReader["Mode"] == DBNull.Value ? "" : sqlReader["Mode"].ToString(),
                                Env = sqlReader["Environment"] == DBNull.Value ? "" : sqlReader["Environment"].ToString(),
                                Restriction = sqlReader["Restriction"] == DBNull.Value ? "" : sqlReader["Restriction"].ToString(),
                                PrevRunDate = sqlReader["prev_rundate"] == DBNull.Value ? "" : sqlReader["prev_rundate"].ToString(),
                                CurrentRunDate = sqlReader["curr_rundate"] == DBNull.Value ? "" : sqlReader["curr_rundate"].ToString(),
                                Customer = sqlReader["customer"] == DBNull.Value ? "" : sqlReader["customer"].ToString(),
                                LoadDate = sqlReader["load_date"] == DBNull.Value ? "" : sqlReader["load_date"].ToString()

                            });
                        }
                    }
                }
            }
            return ret;
        }

        private static List<ReasonCodeValues> GetReasonCodesData()
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            var ret = new List<ReasonCodeValues>();
            // lets connect to the AdventureWorks sample database for some data
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT [user_name] ,[field_name],[reason_code]," +
                    "[compare_id],[no_of_prev_accounts],[no_of_curr_accounts],[difference_value],[absolute_difference]" +
                    " ,[proportion] ,[difference_flag] ,[Model] ,[Version] ,[Mode] ,[Environment] ,[Restriction] ,[prev_rundate]" +
                    ",[curr_rundate] ,[customer] ,[load_date] FROM [DQ].[dbo].[SCORING_PROJECT_REASON_CODES_VIEW]", sqlConn))
                {
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        //Get the data and fill rows 5 onwards
                        while (sqlReader.Read())
                        {

                            //ret.Add(sqlReader["compare_id"].ToString());
                            ret.Add(new ReasonCodeValues
                            {
                                //UserName = sqlReader["user_name"] == DBNull.Value ? "" : sqlReader["user_name"].ToString(),
                                FieldName = sqlReader["field_name"] == DBNull.Value ? "" : sqlReader["field_name"].ToString(),
                                ReasonCode = sqlReader["reason_code"] == DBNull.Value ? "" : sqlReader["reason_code"].ToString(),
                                //CompareID = sqlReader["compare_id"] == DBNull.Value ? "" : sqlReader["compare_id"].ToString(),
                                NbrOfPrevAccts = Convert.ToInt32(sqlReader["no_of_prev_accounts"] == DBNull.Value ? "0" : sqlReader["no_of_prev_accounts"]),
                                NbrOfCurrentAccts = Convert.ToInt32(sqlReader["no_of_curr_accounts"] == DBNull.Value ? "0" : sqlReader["no_of_curr_accounts"]),
                                DiffValue = Convert.ToInt32((sqlReader["difference_value"] == DBNull.Value) ? "0" : sqlReader["difference_value"]),
                                AbsoluteDiff = Convert.ToInt32(sqlReader["absolute_difference"] == DBNull.Value ? "0" : sqlReader["absolute_difference"]),
                                Proportion = Convert.ToDecimal(sqlReader["difference_flag"] == DBNull.Value ? "0" : sqlReader["difference_flag"]),
                                DiffFlag = Convert.ToInt32(sqlReader["difference_flag"] == DBNull.Value ? "0" : sqlReader["difference_flag"]),
                                Model = sqlReader["model"] == DBNull.Value ? "" : sqlReader["model"].ToString(),
                                Version = sqlReader["version"] == DBNull.Value ? "" : sqlReader["version"].ToString(),
                                Mode = sqlReader["Mode"] == DBNull.Value ? "" : sqlReader["Mode"].ToString(),
                                Env = sqlReader["Environment"] == DBNull.Value ? "" : sqlReader["Environment"].ToString(),
                                Restriction = sqlReader["Restriction"] == DBNull.Value ? "" : sqlReader["Restriction"].ToString(),
                                PrevRunDate = sqlReader["prev_rundate"] == DBNull.Value ? "" : sqlReader["prev_rundate"].ToString(),
                                CurrentRunDate = sqlReader["curr_rundate"] == DBNull.Value ? "" : sqlReader["curr_rundate"].ToString(),
                                Customer = sqlReader["customer"] == DBNull.Value ? "" : sqlReader["customer"].ToString(),
                                LoadDate = sqlReader["load_date"] == DBNull.Value ? "" : sqlReader["load_date"].ToString()

                            });
                        }
                    }
                }
            }
            return ret;
        }

        private static List<AttributesSummary> GetAttributesSummary()
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            var ret = new List<AttributesSummary>();
            // lets connect to the AdventureWorks sample database for some data
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT [load_date] ,[attribute_name] " +
                    ",[difference_count],[diff_cnt_Pct(%)] as diff_Prct,[prev_rundate],[curr_rundate] " +
                    "FROM [DQ].[dbo].[ATTRIBUTES_CNT_VIEW]", sqlConn))
                {
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        //Get the data and fill rows 5 onwards
                        while (sqlReader.Read())
                        {

                            //ret.Add(sqlReader["compare_id"].ToString());
                            ret.Add(new AttributesSummary
                            {

                                //LoadDate = sqlReader["load_date"] == DBNull.Value ? "" : sqlReader["load_date"].ToString(),
                                AttributeName = sqlReader["attribute_name"] == DBNull.Value ? "" : sqlReader["attribute_name"].ToString(),
                                DiffCount = Convert.ToInt32(sqlReader["difference_count"] == DBNull.Value ? "0" : sqlReader["difference_count"]),
                                DiffCountPercent = Convert.ToDecimal(sqlReader["diff_Prct"] == DBNull.Value ? "0" : sqlReader["diff_Prct"]),
                                PrevRunDate = sqlReader["prev_rundate"] == DBNull.Value ? "" : sqlReader["prev_rundate"].ToString(),
                                CurrentRunDate = sqlReader["curr_rundate"] == DBNull.Value ? "" : sqlReader["curr_rundate"].ToString()
                            });
                        }
                    }
                }
            }
            return ret;
        }

        private static List<ScoresAttributesSummary> GetScoresAttributesSummary()
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            var ret = new List<ScoresAttributesSummary>();
            
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT [user_name], [compare_id],[Account_no],[model_name],[previous_value]" +
                    ",[current_value],[difference_value],[difference_value_range], [absolute_difference],[diff_value_percent]" +
                    ",[difference_flag],[previous_range],[current_range],[range_difference_flag],[Model],[Version],[Mode]" +
                    ",[Environment],[Restriction],[prev_rundate],[curr_rundate],[customer],[load_date]" +
                    ",[Attributes_Change_Count] FROM [DQ].[dbo].[SCORES_VIEW]", sqlConn))
                {
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        
                        while (sqlReader.Read())
                        {

                            //ret.Add(sqlReader["compare_id"].ToString());
                            ret.Add(new ScoresAttributesSummary
                            {
                                //UserName = sqlReader["user_name"] == DBNull.Value ? "" : sqlReader["user_name"].ToString(),
                                //CompareID = sqlReader["compare_id"] == DBNull.Value ? "" : sqlReader["compare_id"].ToString(),
                                AccountNbr = sqlReader["Account_no"] == DBNull.Value ? "" : sqlReader["Account_no"].ToString(),
                                ModelName = sqlReader["model_name"] == DBNull.Value ? "" : sqlReader["model_name"].ToString(),
                                PreviousValue = Convert.ToInt32((sqlReader["previous_value"] == DBNull.Value) ? "0" : sqlReader["previous_value"]),
                                CurrentValue = Convert.ToInt32((sqlReader["current_value"] == DBNull.Value) ? "0" : sqlReader["current_value"]),
                                DifferenceValue = Convert.ToInt32((sqlReader["difference_value"] == DBNull.Value) ? "0" : sqlReader["difference_value"]),
                                //DifferenceValueRange = (sqlReader["difference_value_range"] == DBNull.Value) ? "" : sqlReader["difference_value_range"].ToString(),
                                AbsoluteDiff = Convert.ToInt32(sqlReader["absolute_difference"] == DBNull.Value ? "0" : sqlReader["absolute_difference"]),
                                DiffValuePercent = Convert.ToDecimal(sqlReader["diff_value_percent"] == DBNull.Value ? "0" : sqlReader["diff_value_percent"]),
                                DifferenceFlag = Convert.ToInt32(sqlReader["difference_flag"] == DBNull.Value ? "0" : sqlReader["difference_flag"]),
                                PrevRange = sqlReader["previous_range"] == DBNull.Value ? "" : sqlReader["previous_range"].ToString(),
                                CurrentRange = sqlReader["current_range"] == DBNull.Value ? "" : sqlReader["current_range"].ToString(),
                                RangeDiffFlag = Convert.ToInt32(sqlReader["range_difference_flag"] == DBNull.Value ? "0" : sqlReader["range_difference_flag"]),
                                //Model = sqlReader["model"] == DBNull.Value ? "" : sqlReader["model"].ToString(),
                                //Version = sqlReader["version"] == DBNull.Value ? "" : sqlReader["version"].ToString(),
                                //Mode = sqlReader["Mode"] == DBNull.Value ? "" : sqlReader["Mode"].ToString(),
                                //Env = sqlReader["Environment"] == DBNull.Value ? "" : sqlReader["Environment"].ToString(),
                                //Restriction = sqlReader["Restriction"] == DBNull.Value ? "" : sqlReader["Restriction"].ToString(),
                                //PrevRunDate = sqlReader["prev_rundate"] == DBNull.Value ? "" : sqlReader["prev_rundate"].ToString(),
                                //CurrentRunDate = sqlReader["curr_rundate"] == DBNull.Value ? "" : sqlReader["curr_rundate"].ToString(),
                                //Customer = sqlReader["customer"] == DBNull.Value ? "" : sqlReader["customer"].ToString(),
                                //LoadDate = sqlReader["load_date"] == DBNull.Value ? "" : sqlReader["load_date"].ToString(),
                                AttrChangeCount = Convert.ToInt32(sqlReader["Attributes_Change_Count"] == DBNull.Value ? "0" : sqlReader["Attributes_Change_Count"])
                            });
                        }
                    }
                }
            }
            return ret;
        }

        private static List<ScoresSummary> GetScoresSummary()
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            var ret = new List<ScoresSummary>();
            // lets connect to the AdventureWorks sample database for some data
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT [model_name],[score_range],[prev_cnt],[curr_cnt],[diff_cnt] ,[absolute_diff_cnt] " +
                    "FROM [DQ].[dbo].[SCORES_VIEW_1]", sqlConn))
                {
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        //Get the data and fill rows 5 onwards
                        while (sqlReader.Read())
                        {

                            //ret.Add(sqlReader["compare_id"].ToString());
                            ret.Add(new ScoresSummary
                            {

                                ModelName = sqlReader["model_name"] == DBNull.Value ? "" : sqlReader["model_name"].ToString(),
                                ScoreRange = sqlReader["score_range"] == DBNull.Value ? "" : sqlReader["score_range"].ToString(),
                                PrevCount = Convert.ToInt32(sqlReader["prev_cnt"] == DBNull.Value ? "0" : sqlReader["prev_cnt"]),
                                CurrentCount = Convert.ToInt32(sqlReader["curr_cnt"] == DBNull.Value ? "0" : sqlReader["curr_cnt"]),
                                DiffCount = Convert.ToInt32(sqlReader["diff_cnt"] == DBNull.Value ? "0" : sqlReader["diff_cnt"]),
                                AbsoluteDiffCount = Convert.ToInt32(sqlReader["absolute_diff_cnt"] == DBNull.Value ? "0" : sqlReader["absolute_diff_cnt"])
                            });
                        }
                    }
                }
            }
            return ret;
        }

        private static List<ScoresModelSummary> GetScoresModelSummary()
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            var ret = new List<ScoresModelSummary>();
            // lets connect to the AdventureWorks sample database for some data
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT [model_name],[total]," +
                    "[no_of_changes],[perc_change],[model],[version],[mode],[Environment]," +
                    "[Restriction],[prev_rundate],[curr_rundate],[customer],[load_date],[compare_id]" +
                    "FROM [DQ].[dbo].[SCORES_VIEW_3]", sqlConn))
                {
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        //Get the data and fill rows 5 onwards
                        while (sqlReader.Read())
                        {

                            //ret.Add(sqlReader["compare_id"].ToString());
                            ret.Add(new ScoresModelSummary
                            {

                                ModelName = sqlReader["model_name"] == DBNull.Value ? "" : sqlReader["model_name"].ToString(),
                                Total = Convert.ToInt32(sqlReader["total"] == DBNull.Value ? "0" : sqlReader["total"]),
                                NbrOfChanges = Convert.ToInt32(sqlReader["no_of_changes"] == DBNull.Value ? "0" : sqlReader["no_of_changes"]),
                                PercentChange = Convert.ToDecimal(sqlReader["perc_change"] == DBNull.Value ? "0" : sqlReader["perc_change"]),
                                //Model = sqlReader["model"] == DBNull.Value ? "" : sqlReader["model"].ToString(),
                                //Version = sqlReader["version"] == DBNull.Value ? "" : sqlReader["version"].ToString(),
                                //Mode = sqlReader["Mode"] == DBNull.Value ? "" : sqlReader["Mode"].ToString(),
                                //Env = sqlReader["Environment"] == DBNull.Value ? "" : sqlReader["Environment"].ToString(),
                                //Restriction = sqlReader["Restriction"] == DBNull.Value ? "" : sqlReader["Restriction"].ToString(),
                                //PrevRunDate = sqlReader["prev_rundate"] == DBNull.Value ? "" : sqlReader["prev_rundate"].ToString(),
                                //CurrentRunDate = sqlReader["curr_rundate"] == DBNull.Value ? "" : sqlReader["curr_rundate"].ToString(),
                                //Customer = sqlReader["customer"] == DBNull.Value ? "" : sqlReader["customer"].ToString(),
                                //LoadDate = sqlReader["load_date"] == DBNull.Value ? "" : sqlReader["load_date"].ToString(),
                                //CompareID = sqlReader["compare_id"] == DBNull.Value ? "" : sqlReader["compare_id"].ToString()
                            });
                        }
                    }
                }
            }
            return ret;
        }

        private static List<RCSummary> GetReasonCodesSummary()
        {
            ConnectionStringSettings cs;
            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
            String connString = cs.ConnectionString;

            var ret = new List<RCSummary>();
            // lets connect to the AdventureWorks sample database for some data
            using (SqlConnection sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand("SELECT [rv_model],[reason_code]," +
                    "[no_of_prev_accounts],[no_of_curr_accounts],[difference_value] ," +
                    "[absolute_difference],[proportion],[difference_flag],[Model],[prev_rundate]," +
                    "[curr_rundate],[compare_id] FROM [DQ].[dbo].[REASON_CODES_VIEW]", sqlConn))
                {
                    using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    {
                        //Get the data and fill rows 5 onwards
                        while (sqlReader.Read())
                        {

                            //ret.Add(sqlReader["compare_id"].ToString());
                            ret.Add(new RCSummary
                            {
                                RVModel = sqlReader["rv_model"] == DBNull.Value ? "" : sqlReader["rv_model"].ToString(),
                                ReasonCode = sqlReader["reason_code"] == DBNull.Value ? "" : sqlReader["reason_code"].ToString(),
                               // CompareID = sqlReader["compare_id"] == DBNull.Value ? "" : sqlReader["compare_id"].ToString(),
                                NbrOfPrevAccts = Convert.ToInt32(sqlReader["no_of_prev_accounts"] == DBNull.Value ? "0" : sqlReader["no_of_prev_accounts"]),
                                NbrOfCurrentAccts = Convert.ToInt32(sqlReader["no_of_curr_accounts"] == DBNull.Value ? "0" : sqlReader["no_of_curr_accounts"]),
                                DiffValue = Convert.ToInt32((sqlReader["difference_value"] == DBNull.Value) ? "0" : sqlReader["difference_value"]),
                                AbsoluteDiff = Convert.ToInt32(sqlReader["absolute_difference"] == DBNull.Value ? "0" : sqlReader["absolute_difference"]),
                                Proportion = Convert.ToDecimal(sqlReader["proportion"] == DBNull.Value ? "0" : sqlReader["proportion"]),
                                DiffFlag = Convert.ToInt32(sqlReader["difference_flag"] == DBNull.Value ? "0" : sqlReader["difference_flag"]),
                                //Model = sqlReader["model"] == DBNull.Value ? "" : sqlReader["model"].ToString(),
                                //PrevRunDate = sqlReader["prev_rundate"] == DBNull.Value ? "" : sqlReader["prev_rundate"].ToString(),
                                //CurrentRunDate = sqlReader["curr_rundate"] == DBNull.Value ? "" : sqlReader["curr_rundate"].ToString()
                            });
                        }
                    }
                }
            }
            return ret;
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
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
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
            }
            return currentDates;
        }

        public DataSet GetReportGenerationTimes(String roxieDate, String model, String version, String mode)
        {
            String sqlGetTimeStamp = "";

            ConnectionStringSettings cs;

            sqlGetTimeStamp = "select distinct SUBSTRING(roxie_date,(LEN(roxie_date) - 5),6) as roxieTime" +
                " FROM [DQ].[dbo].[scr_master_runs] where SUBSTRING(roxie_date,1,8) = @roxieDate  and " +
                                        "product = @product and version = @version and mode = @mode";

            cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];

            String connString = cs.ConnectionString;
            SqlConnection dbConnection = new SqlConnection(connString);
            DataSet dsTimeStamps = new DataSet();
            SqlCommand dbCommand = new SqlCommand(sqlGetTimeStamp, dbConnection);
            SqlDataAdapter daTimeStamps = new SqlDataAdapter(dbCommand);
            dbCommand.CommandType = CommandType.Text;
            dbCommand.Parameters.Add(new SqlParameter("@roxieDate", roxieDate));

            dbCommand.Parameters.Add(new SqlParameter("@product", model));

            dbCommand.Parameters.Add(new SqlParameter("@version", version));

            dbCommand.Parameters.Add(new SqlParameter("@mode", mode));
            using (WindowsImpersonationContext impersonatedUser = (HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity).Impersonate())
            {
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
            }
            return dsTimeStamps;
        }
    }
}