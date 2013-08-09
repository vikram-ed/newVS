using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections;
using System.Threading.Tasks;
using System.Web.SessionState;
using System.Diagnostics;
using System.Globalization;
using ScoringApp1.helperClasses;
using System.ComponentModel;
using Microsoft.SqlServer.Dts.Runtime;

namespace ScoringApp1
{

    [System.Web.Script.Services.ScriptService]
    public partial class ScoringForm : System.Web.UI.Page
    {
        DataSet dsPrevDateSelectedData, dsCurrentDateSelectedData;

        public static String progress, summaryPath, detailedPath;
        String compareID;
        String userName;


        /// <summary>
        /// Page load event handler
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                cdrPrevDate.VisibleDate = DateTime.Today;
                cdrCurrentDate.Enabled = false;
                //FillDatesUsingData(DateTime.Today);
                ddlPrevTime.Enabled = false;
                cdrPrevDate.Enabled = false;

                ScoringDAL dbAccess = new ScoringDAL();

                userName = HttpContext.Current.User.Identity.Name;
                userName = userName.Substring(userName.IndexOf('\\') + 1);


                if (Request.QueryString["user"] != null && Request.QueryString["id"] != null)
                {
                    if (Request.QueryString["user"] == userName)
                    {
                        Dictionary<String, String> compareIDs = new Dictionary<String, String>();
                        compareIDs = dbAccess.getAllExistingCompareIDs();
                        Int32 compareIDSerial = (Request.QueryString["id"] == null) ? 0 : Convert.ToInt32(Request.QueryString["id"]);
                        int i = 0;
                        foreach (KeyValuePair<String, String> userCompareIDs in compareIDs)
                        {
                            i = i + 1;
                            if (i == compareIDSerial)
                            {
                                compareID = userCompareIDs.Key;
                            }
                        }

                        dbAccess.DeleteCompareID(compareID, userName);
                    }

                }
                //DataTable tblCompareIDs = getPrevCompareTable();

                //List<String> compareIDList = new List<String>();

                //compareIDList = dbAccess.getAllExistingCompareIDs();

                //ClientScript.RegisterForEventValidation("mode_id");
            }
            else
            {
                dsPrevDateSelectedData = (DataSet)ViewState["selectedPrevDateDataForMonth"];
                // FillDatesUsingData(DateTime.Today);
                dsCurrentDateSelectedData = (DataSet)ViewState["selectedCurrentDateDataForMonth"];
                compareID = Session["compareID"] == null ? "" : Session["compareID"].ToString();
                ClientScript.RegisterHiddenField("isPostBack", "1");
            }
        }


        //public DataTable getPrevCompareTable()
        //{

        //    List<String> compareIDList = new List<String>();
        //    ScoringDAL dbAccess = new ScoringDAL();
        //    compareIDList = dbAccess.getAllExistingCompareIDs();

        //    String[] comparisonData;
        //    List<String[]> allComparisonData = new List<String[]>();

        //    DataTable tblCompareIDs = new DataTable();
        //    String[] processedCompareList = new String[8];
        //    CultureInfo provider = CultureInfo.InvariantCulture;
        //    DateTime prevDateTime, currDateTime;
        //    String compareID;

        //    tblCompareIDs.Columns.Add("Serial");
        //    tblCompareIDs.Columns.Add("Model");
        //    tblCompareIDs.Columns.Add("Dates");
        //    tblCompareIDs.Columns.Add("Version");
        //    tblCompareIDs.Columns.Add("Mode");
        //    tblCompareIDs.Columns.Add("Environment");
        //    tblCompareIDs.Columns.Add("Restriction");
        //    tblCompareIDs.Columns.Add("Customer");
        //    for (int i = 0; i < compareIDList.Count; i++)
        //    {
        //        compareID = compareIDList[i];
        //        comparisonData = compareID.Split('_');
        //        if (comparisonData != null)
        //        {
        //            DataRow dr = tblCompareIDs.NewRow();
        //            dr[0] = i + 1;
        //            dr[1] = comparisonData[0];
        //            prevDateTime = DateTime.ParseExact(comparisonData[1] + " " + comparisonData[2], "yyyyMMdd HHmmss", provider);
        //            currDateTime = DateTime.ParseExact(comparisonData[3] + " " + comparisonData[4], "yyyyMMdd HHmmss", provider);
        //            dr[2] = prevDateTime.ToString() + " vs " + currDateTime.ToString();
        //            dr[3] = "v" + comparisonData[5];
        //            dr[4] = comparisonData[6];
        //            dr[5] = comparisonData[7];
        //            dr[6] = comparisonData[8];
        //            dr[7] = comparisonData[9];

        //            tblCompareIDs.Rows.Add(dr);
        //        }

        //        //allComparisonData.Add(comparisonData);
        //        //DataRow dr = tblCompareIDs.NewRow();
        //        //tblCompareIDs.Rows.Add(processedCompareList);


        //    }

        //    return tblCompareIDs;
        //}



        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string getHTMLCompareTable()
        {

            Dictionary<String, String> compareIDs = new Dictionary<String, String>();
            ScoringDAL dbAccess = new ScoringDAL();
            compareIDs = dbAccess.getAllExistingCompareIDs();

            String[] comparisonData;
            List<String[]> allComparisonData = new List<String[]>();

            DataTable tblCompareIDs = new DataTable();
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime prevDateTime, currDateTime;
            String compareID, ownerName;
            int i = 0;

            tblCompareIDs.Columns.Add("Serial");
            tblCompareIDs.Columns.Add("Model");
            tblCompareIDs.Columns.Add("Dates");
            tblCompareIDs.Columns.Add("Version");
            tblCompareIDs.Columns.Add("Mode");
            tblCompareIDs.Columns.Add("Environment");
            tblCompareIDs.Columns.Add("Restriction");
            tblCompareIDs.Columns.Add("Customer");
            tblCompareIDs.Columns.Add("Owner");

            foreach (KeyValuePair<String, String> userCompareID in compareIDs)
            {
                ownerName = userCompareID.Value;
                compareID = userCompareID.Key;
                comparisonData = compareID.Split('_');
                if (comparisonData != null)
                {
                    DataRow dr = tblCompareIDs.NewRow();
                    dr[0] = ++i;
                    dr[1] = comparisonData[0];
                    prevDateTime = DateTime.ParseExact(comparisonData[1] + " " + comparisonData[2], "yyyyMMdd HHmmss", provider);
                    currDateTime = DateTime.ParseExact(comparisonData[3] + " " + comparisonData[4], "yyyyMMdd HHmmss", provider);
                    dr[2] = prevDateTime.ToString() + " vs " + currDateTime.ToString();
                    dr[3] = "v" + comparisonData[5];
                    dr[4] = comparisonData[6];
                    dr[5] = comparisonData[7];
                    dr[6] = comparisonData[8];
                    dr[7] = comparisonData[9];
                    dr[8] = ownerName;
                    tblCompareIDs.Rows.Add(dr);
                }

                //allComparisonData.Add(comparisonData);
                //DataRow dr = tblCompareIDs.NewRow();
                //tblCompareIDs.Rows.Add(processedCompareList);


            }

            // return "{\"aaData\" : " + GetJson(tblCompareIDs) + "}";
            return GetJson(tblCompareIDs);

        }

        public static string GetJson(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        //unused
        private void AddToCompareIDList(List<String> compareIDList)
        {
            ddlCompareIDs.Items.Add("Previous Compare IDs");
            String withoutUserName;
            foreach (String compareID in compareIDList)
            {
                withoutUserName = compareID.Substring(compareID.IndexOf('_') + 1);
                ddlCompareIDs.Items.Add(withoutUserName.Substring(withoutUserName.IndexOf('_') + 1));

            }
        }

        /// <summary>
        /// Get dates with data and fill the global dataset of dates with it
        /// </summary>
        private void FillDatesUsingData(DateTime visibleDate)
        {
            try
            {

                ScoringDAL dbAccess = new ScoringDAL();
                String selectedMonth = visibleDate.Month.ToString();
                String selectedYear = visibleDate.Year.ToString();

                //getting dates that have data - default value for model is taken as "Risk View"
                dsPrevDateSelectedData = dbAccess.GetDatesWithData(selectedMonth,
                                        selectedYear, (hifModel.Value == "") ?
                                        "Risk View" : hifModel.Value, (hifVersion.Value == "") ?
                                        "4" : hifVersion.Value, (mode_id.SelectedValue == "") ?
                                        "XML" : mode_id.SelectedValue);

                //this data is stored and sent as view state variables
                ViewState["selectedPrevDateDataForMonth"] = dsPrevDateSelectedData;

            }
            catch (Exception ex)
            {
                //StreamWriter swException = new StreamWriter(
                // @"C:\Users\parevi01\Documents\Visual Studio 2012\Projects " + 
                // @"\ScoringApp1\ScoringApp1\Log\ScoringFormExcpetions.txt");
                //swException.WriteLine(DateTime.Now.ToString() + "-" + ex.GetType() + "-" + ex.Message + "-" + ex.StackTrace);
                //swException.Close();
                // throw;
            }

        }

        #region "Calendar triggers"
        //protected void model_id_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ScoringDAL dbAccess = new ScoringDAL();
        //    Dictionary<String, ArrayList> defaultValues = new Dictionary<string, ArrayList>();

        //    //gets the default lists for the particular model that is selected by the user
        //    defaultValues = dbAccess.GetDefaultLists(model_id.SelectedValue);
        //    foreach (String ver in defaultValues["Version"])
        //    {
        //        version_id.Items.Add(ver);
        //    }
        //    foreach (String mode in defaultValues["Mode"])
        //    {
        //        mode_id.Items.Add(mode);
        //    }
        //}
        protected void version_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDatesUsingData(DateTime.Today);
        }
        protected void mode_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            cdrPrevDate.Enabled = true;
            cdrPrevDate.SelectedDates.Clear();
            cdrCurrentDate.SelectedDates.Clear();
            dsCurrentDateSelectedData = null;
            cdrCurrentDate.Enabled = false;
            ddlPrevTime.SelectedIndex = 0;
            ddlCurrentTime.SelectedIndex = 0;
            FillDatesUsingData(DateTime.Today);
        }
        #endregion

        #region "PrevDate calendar events"
        protected void cdrPrevDate_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime dateWithData;
            String strDateWithData;
            Boolean flagValue = false;

            if (dsPrevDateSelectedData != null && dsPrevDateSelectedData.Tables.Count != 0)
            {
                foreach (DataRow dr in dsPrevDateSelectedData.Tables[0].Rows)
                {
                    strDateWithData = dr["dateWithData"].ToString();

                    dateWithData = new DateTime(Convert.ToInt32(strDateWithData.Substring(0, 4)),
                                                Convert.ToInt32(strDateWithData.Substring(4, 2)),
                                                Convert.ToInt32(strDateWithData.Substring(6, 2)));
                    if (dateWithData == e.Day.Date)
                    {
                        e.Cell.BackColor = System.Drawing.Color.IndianRed;
                        flagValue = true;
                        break;
                    }

                }
                if (!flagValue)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
                    e.Cell.Enabled = false;
                    e.Cell.Controls.Clear();
                    e.Cell.Controls.Add(new LiteralControl(e.Day.Date.Day.ToString()));
                }

            }

        }


        protected void cdrPrevDate_SelectionChanged(object sender, EventArgs e)
        {
            DataSet dsTimeStamps;
            ScoringDAL dbAccess = new ScoringDAL();
            dsTimeStamps = dbAccess.GetReportGenerationTimes(cdrPrevDate.SelectedDate.ToString("yyyyMMdd"),
                                                                        (hifModel.Value == "") ? "Risk View" : hifModel.Value,
                                                                        (hifVersion.Value == "") ? "4" : hifVersion.Value,
                                                                        (mode_id.SelectedValue == "") ? "XML" : mode_id.SelectedValue);

            cdrCurrentDate.Enabled = true;
            cdrCurrentDate.VisibleDate = cdrPrevDate.SelectedDate;
            ddlPrevTime.Items.Clear();
            if (dsTimeStamps != null)
            {
                ddlPrevTime.Items.Add("Previous Time");
                foreach (DataRow drTimeStamp in dsTimeStamps.Tables[0].Rows)
                {
                    String strPrevTime = drTimeStamp["roxieTime"].ToString();
                    TimeSpan ts = new TimeSpan(Convert.ToInt32(strPrevTime.Substring(0, 2)),
                                               Convert.ToInt32(strPrevTime.Substring(2, 2)),
                                               Convert.ToInt32(strPrevTime.Substring(4, 2)));
                    DateTime prevTime = cdrPrevDate.SelectedDate + ts;
                    //DateTime prevTime = Convert.ToDateTime(strPrevTime.Substring(0, 2) + ":" + strPrevTime.Substring(2, 2) + ":" + strPrevTime.Substring(4, 2));

                    ddlPrevTime.Items.Add(prevTime.ToString("MM/dd/yyyy hh:mm:ss tt"));

                    if (dsTimeStamps.Tables[0].Rows.Count == 1)
                    {
                        ddlPrevTime.SelectedIndex = 1;
                        TimeSpan duration = new TimeSpan(1, 0, 0, 0);
                        dsCurrentDateSelectedData = dbAccess.GetCurrentDatesWithData(cdrPrevDate.SelectedDate.Month.ToString(),
                                                                       cdrPrevDate.SelectedDate.Year.ToString(),
                                                                       cdrPrevDate.SelectedDate.Add(duration).ToString("yyyyMMdd"),
                                                                       (hifModel.Value == "") ? "Risk View" : hifModel.Value,
                                                                       (hifVersion.Value == "") ? "4" : hifVersion.Value,
                                                                       (mode_id.SelectedValue == "") ? "XML" : mode_id.SelectedValue);
                        ViewState["selectedCurrentDateDataForMonth"] = dsCurrentDateSelectedData;
                    }
                    else
                    {
                        ddlPrevTime.SelectedIndex = 0;
                        dsCurrentDateSelectedData = dbAccess.GetCurrentDatesWithData(cdrPrevDate.SelectedDate.Month.ToString(),
                                                                       cdrPrevDate.SelectedDate.Year.ToString(),
                                                                       cdrPrevDate.SelectedDate.ToString("yyyyMMdd"),
                                                                       (hifModel.Value == "") ? "Risk View" : hifModel.Value,
                                                                       (hifVersion.Value == "") ? "4" : hifVersion.Value,
                                                                       (mode_id.SelectedValue == "") ? "XML" : mode_id.SelectedValue);
                        ViewState["selectedCurrentDateDataForMonth"] = dsCurrentDateSelectedData;
                    }

                    ddlPrevTime.Enabled = true;
                }
            }
            // RenderControl(cdrPrevDate);
            //FillDatesUsingData(cdrPrevDate.SelectedDate);

        }


        protected void cdrPrevDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            cdrPrevDate.VisibleDate = e.NewDate;
            FillDatesUsingData(e.NewDate);
        }

        protected void ddlPrevTime_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion
        #region "CurrentDate calendar events"
        protected void cdrCurrentDate_SelectionChanged(object sender, EventArgs e)
        {

            DataSet dsTimeStamps;
            ArrayList alTimeStamps = new ArrayList();
            ScoringDAL dbAccess = new ScoringDAL();
            dsTimeStamps = dbAccess.GetReportGenerationTimes(cdrCurrentDate.SelectedDate.ToString("yyyyMMdd"),
                                                                        (hifModel.Value == "") ? "Risk View" : hifModel.Value,
                                                                        (hifVersion.Value == "") ? "4" : hifVersion.Value,
                                                                        (mode_id.SelectedValue == "") ? "XML" : mode_id.SelectedValue);
            ddlCurrentTime.Items.Clear();
            if (dsTimeStamps != null)
            {
                ddlCurrentTime.Items.Add("Current Time");
                foreach (DataRow drTimeStamp in dsTimeStamps.Tables[0].Rows)
                {
                    String strCurrentTime = drTimeStamp["roxieTime"].ToString();
                    TimeSpan ts = new TimeSpan(Convert.ToInt32(strCurrentTime.Substring(0, 2)),
                                               Convert.ToInt32(strCurrentTime.Substring(2, 2)),
                                               Convert.ToInt32(strCurrentTime.Substring(4, 2)));
                    DateTime prevTime = cdrCurrentDate.SelectedDate + ts;
                    //DateTime prevTime = Convert.ToDateTime(strPrevTime.Substring(0, 2) + ":" + strPrevTime.Substring(2, 2) + ":" + strPrevTime.Substring(4, 2));
                    //alTimeStamps.Add("Current Time");
                    //alTimeStamps.Add(prevTime.ToString("MM/dd/yyyy hh:mm:ss tt"));

                    ddlCurrentTime.Items.Add(prevTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    if (dsTimeStamps.Tables[0].Rows.Count == 1)
                    {
                        ddlCurrentTime.SelectedIndex = 1;
                    }
                    else
                    {
                        ddlCurrentTime.SelectedIndex = 0;
                    }
                    ddlCurrentTime.Enabled = true;
                }
                //ddlCurrentTime.DataSource = alTimeStamps;
                //ddlCurrentTime.DataBind();
            }
            btnSubmit.Enabled = true;

        }

        protected void cdrCurrentDate_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime dateWithData;
            String strDateWithData;
            Boolean flagValue = false;
            if (dsCurrentDateSelectedData != null)
            {
                foreach (DataRow dr in dsCurrentDateSelectedData.Tables[0].Rows)
                {
                    strDateWithData = dr["dateWithData"].ToString();

                    dateWithData = new DateTime(Convert.ToInt32(strDateWithData.Substring(0, 4)),
                                                Convert.ToInt32(strDateWithData.Substring(4, 2)),
                                                Convert.ToInt32(strDateWithData.Substring(6, 2)));
                    if (dateWithData == e.Day.Date)
                    {
                        e.Cell.BackColor = System.Drawing.Color.IndianRed;
                        flagValue = true;
                        break;
                    }

                }
                if (!flagValue)
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGray;
                    e.Cell.Enabled = false;
                    e.Cell.Controls.Clear();
                    e.Cell.Controls.Add(new LiteralControl(e.Day.Date.Day.ToString()));
                }

            }
        }

        protected void cdrCurrentDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            cdrCurrentDate.VisibleDate = e.NewDate;
            FillDatesUsingData(e.NewDate);
        }



        protected void ddlCurrentTime_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        #endregion

        protected async void btnGatherData_Click(object sender, EventArgs e)
        {
            //string command = string.Format("sampleBat.bat");
            //ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            ////procStartInfo.WorkingDirectory = "C:\MyApp\";
            //procStartInfo.RedirectStandardOutput = true;
            //procStartInfo.UseShellExecute = false;
            //procStartInfo.CreateNoWindow = false;

            //Process proc = new Process();
            //proc.StartInfo = new ProcessStartInfo("sampleBat.bat");
            //proc.Start();
            //proc.WaitForExit();
            System.Threading.Tasks.Task generateFilesTask = GenerateFiles();
            await System.Threading.Tasks.Task.WhenAny(generateFilesTask);
        

        }

        private async System.Threading.Tasks.Task GenerateFiles()
        {
           // System.Threading.Tasks.Task t = new System.Threading.Tasks.Task
           //(
           //    () =>
           //    {
            Package pkg = null;
            try
            {

            
                   Microsoft.SqlServer.Dts.Runtime.Application app;
                  
                   DTSExecResult pkgResults;
                   Variables vars;
                   String pkgLocation = @"G:\Package1.dtsx";
                   //String pkgLocation = @"C:\Users\parevi01\documents\visual studio 2012\Projects\ScoringApp1\ScoringApp1\Refs\Package1.dtsx";
                   //String pkgLocation = @"C:\Users\parevi01\Documents\Visual Studio 2008\Projects\emailtest\emailtest\Package.dtsx";
                   Console.WriteLine("test1");
                   lblUpdate.Text = "test1";
                   //String pkgLocation = @"C:\Users\burjuksx\Desktop\Scoring_Project_Scores\Scoring_Project_Scores\Package1.dtsx";
                   Console.WriteLine("test2");
                   lblUpdate.Text = "test2";
                   //String pkgLocation = @"C:\Users\parevi01\Desktop\emailtest\emailtest\Package.dtsx";
                  

                   app = new Microsoft.SqlServer.Dts.Runtime.Application();
                   pkg = app.LoadPackage(pkgLocation, null);
                   vars = pkg.Variables;

                   //vars["User::Sub"].Value = "RISKVIEW";
                   vars["User::model"].Value = "RISKVIEW";
                   vars["User::mode"].Value = "XML";
                   vars["User::version"].Value = "3";

                   pkgResults = pkg.Execute();
                if (pkgResults == Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure)
                {
                    foreach (Microsoft.SqlServer.Dts.Runtime.DtsError local_DtsError in pkg.Errors)
                    {
                        //MessageBox.Show(local_DtsError.Description);
                        Console.WriteLine(local_DtsError.Description);
                        lblUpdate.Text = local_DtsError.Description;
                        throw new System.InvalidOperationException("Package execute with errors !!" + local_DtsError.Description);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("test3");
                lblUpdate.Text = "test3";
                lblUpdate.Text = ex.Message;
                throw ex;
            }
            finally
            {
                pkg.Dispose();
                pkg = null;
            }


           //    }
           //);
           // t.Start();

         //   await t;
            
        }

        /// <summary>
        /// btnHistoryCompare corresponds to the submit button on the history compare screen
        /// </summary>
        protected void btnHistoryCompare_Click(object sender, EventArgs e)
        {
            //updating the display status of the application
            progress = "Comparison results are being populated into your view...";

            //calling the Run method that in turn calls the appropriate DAL method
            Run("History Compare");

            //updating the display status of the application
            progress = "Results populated in the database. Please refresh the specific excel to see the results.";
        }


        /// <summary>
        /// main execution code for running the comparisons
        /// </summary>
        private void Run(String typeOfTask)
        {
            //Current application user name
            userName = HttpContext.Current.User.Identity.Name;
            userName = userName.Substring(userName.IndexOf('\\') + 1);

            ScoringDAL dbAccess = new ScoringDAL();
            //changing the progress that is to be displayed on the loading division
            progress = "Retrieving values from UI...";

            try
            {
                //if the type of task is to compare based on the previous compareIDs, 
                //then the stored proc is called with just the username and the compareID
                if (typeOfTask == "History Compare")
                {


                    //getting all the compareIDs as a list. This list was sent to the
                    //client during page load, as an ajax call from jquery. 
                    //When the datatable in prev comparison screen is clicked, 
                    //jquery returns the serial number on the grid, through which we fetch the compareID
                    Dictionary<String, String> compareIDs = new Dictionary<String, String>();
                    compareIDs = dbAccess.getAllExistingCompareIDs();
                    Int32 compareIDSerial = (hifCompareIDSerial.Value == null) ? 0 : Convert.ToInt32(hifCompareIDSerial.Value);
                    int i = 0;
                    foreach (KeyValuePair<String, String> userCompareIDs in compareIDs)
                    {
                        i = i + 1;
                        if (i == compareIDSerial + 1)
                        {
                            compareID = userCompareIDs.Key;
                        }
                    }


                    //calling DAL method that calls the stored procedure
                    dbAccess.RefreshData("", "", "", "", "", "", "", "", "", userName, compareID);
                    progress = "Results populated in the database. Please refresh the specific excel to see the results.";

                }
                else
                {
                    //this is the case where the user requested that the comparison results be generated at real time

                    //server validation for dates
                    if (!(cdrPrevDate.SelectedDate == Convert.ToDateTime("1/1/0001 12:00:00 AM") ||
                        cdrCurrentDate.SelectedDate == Convert.ToDateTime("1/1/0001 12:00:00 AM")))
                    {

                        String model, version, mode, env, restriction, customer;
                        String previousDate, currentDate;
                        String loadTime = DateTime.Now.ToString();

                        //getting all the required values from the user interface, 
                        //that will be passed to the stored proc at the backend
                        String prevTime = Convert.ToDateTime(ddlPrevTime.SelectedValue.ToString()).ToString("HHmmss");
                        String currentTime = Convert.ToDateTime(ddlCurrentTime.SelectedValue.ToString()).ToString("HHmmss");

                        previousDate = cdrPrevDate.SelectedDate.ToString("yyyyMMdd") + "_" + prevTime;
                        currentDate = cdrCurrentDate.SelectedDate.ToString("yyyyMMdd") + "_" + currentTime;

                        model = (hifModel.Value == "") ? "Risk View" : hifModel.Value;
                        version = hifVersion.Value.Substring(1);
                        mode = hifMode.Value;
                        env = hifEnv.Value;
                        restriction = hifRestriction.Value;
                        customer = hifCustomer.Value;
                        userName = HttpContext.Current.User.Identity.Name;
                        userName = userName.Substring(userName.IndexOf('\\') + 1);

                        //preparing a compareID that is passed to the stored proc
                        compareID = model + "_" + previousDate + "_"
                            + currentDate + "_" + version + "_" +
                            mode + "_" + env + "_" + restriction + "_" + customer;

                        //storing the compareID as a session variable
                        Session["compareID"] = compareID;

                        //changing the display status of the application
                        progress = "Running queries to generate comparison results...";

                        //calling the DAL method using the parameters passed
                        dbAccess.RefreshData(previousDate, currentDate, model, version, mode, env, restriction, customer, loadTime, userName, compareID);

                        //updating the display status
                        progress = "Results populated in the database. Please refresh the specific excel to see the results.";
                    }
                    else
                    {
                        //error while validating the calendar dates
                        progress = "Please enter the values for all the previous and current comparison dates";
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Submit button corresponds to the server button in the compare now screen
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //updating the display status of the application
            progress = "Comparison results are being generated...";

            //calling the Run method that in turn calls the appropriate DAL method
            Run("Compare Now");

            //updating the display status of the application
            progress = "Results populated in the database. Please refresh the specific excel to see the results.";

        }

        /// <summary>
        /// Reset button corresponds to the reset button in the compare now screen
        /// </summary>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            //resetting the calendars and the time dropdown boxes - 
            //these are the only server side controls in the compare now screen
            cdrPrevDate.SelectedDates.Clear();
            cdrCurrentDate.SelectedDates.Clear();
            ddlPrevTime.ClearSelection();
            ddlCurrentTime.ClearSelection();
        }

        /// <summary>
        /// method that is called by the client jquery scripts using ajax in order to update the status of the application
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static String GetText()
        {
            //returns the global variable "progress" that changes at each execution part in the program
            return progress;
        }

        /// <summary>
        /// method is called as a web service method from the jquery ajax script - 
        /// it gets the default values as json objects, these default values are assigned to the dropdown boxes
        /// </summary>
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static Dictionary<String, ArrayList> GetDefaultValues(String model)
        {
            ScoringDAL dbAccess = new ScoringDAL();
            Dictionary<String, ArrayList> defaultValues = new Dictionary<string, ArrayList>();

            //gets the default lists for the particular model that is selected by the user
            defaultValues = dbAccess.GetDefaultLists(model);
            return defaultValues;
        }


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static String GetCurrentUser()
        {
            String userName = HttpContext.Current.User.Identity.Name;
            userName = userName.Substring(userName.IndexOf('\\') + 1);
            return userName;
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static void DeleteCompareID(String serialID)
        {
            ScoringDAL dbAccess = new ScoringDAL();
            String compareID = "";
            Dictionary<String, String> compareIDs = new Dictionary<String, String>();
            compareIDs = dbAccess.getAllExistingCompareIDs();
            Int32 compareIDSerial = (serialID == null || serialID == "") ? 0 : Convert.ToInt32(serialID);
            int i = 0;
            foreach (KeyValuePair<String, String> userCompareIDs in compareIDs)
            {
                i = i + 1;
                if (i == compareIDSerial + 1)
                {
                    compareID = userCompareIDs.Key;
                }
            }

            // dbAccess.DeleteCompareID(compareID);
        }

    }
}