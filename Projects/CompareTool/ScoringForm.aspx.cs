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

namespace ScoringApp1
{

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
                //userName = System.Environment.UserName +
                //    System.Security.Principal.WindowsIdentity.GetCurrent().Name +
                //    HttpContext.Current.User.Identity.Name;
                //lblUserName.Text = userName;
                //userName = System.Environment.UserName;
                //userName = userName.Substring(userName.IndexOf('\\') + 1);


                //cdrPrevDate.VisibleDate = DateTime.Today;
                //cdrCurrentDate.Enabled = false;
                //FillDatesUsingData(DateTime.Today);
                //ddlPrevTime.Enabled = false;

                ////populating default lists into viewstate for access using client script
                //Dictionary<String, ArrayList> defaultLists = new Dictionary<string, ArrayList>();
                //ScoringDAL dbAccess = new ScoringDAL();
                //lbDailyCompareIDs.DataBind();
                //defaultLists = dbAccess.GetDefaultLists("Risk View");

                //Page.ClientScript.RegisterHiddenField("vCode", ViewState["listsDictionary"].ToString());








                cdrPrevDate.VisibleDate = DateTime.Today;
                cdrCurrentDate.Enabled = false;
                FillDatesUsingData(DateTime.Today);
                ddlPrevTime.Enabled = false;

                //populating default lists into viewstate for access using client script
                //Dictionary<String, ArrayList> defaultLists = new Dictionary<string, ArrayList>();
                ScoringDAL dbAccess = new ScoringDAL();
                //lbDailyCompareIDs.DataBind();
                //defaultLists = dbAccess.GetDefaultLists("Risk View");
                //ViewState["listsDictionary"] = defaultLists;
                //Page.ClientScript.RegisterHiddenField("vCode", ViewState["listsDictionary"].ToString());



                //Code to generate existing compareIDs
                //userName = System.Environment.UserName +
                //    System.Security.Principal.WindowsIdentity.GetCurrent().Name +
                //    HttpContext.Current.User.Identity.Name;
                //lblUserName.Text = userName;
                userName = HttpContext.Current.User.Identity.Name;
                userName = userName.Substring(userName.IndexOf('\\') + 1);

              

               
                //foreach (String compareID in compareIDList)
                //{
                //    comparisonData = compareID.Split('_');
                //    allComparisonData.Add(comparisonData);
                //}
                DataTable tblCompareIDs = getPrevCompareTable();
            
                grdPrevCompareIDs.DataSource = tblCompareIDs;
                grdPrevCompareIDs.DataBind();


             
            }
            else
            {
                dsPrevDateSelectedData = (DataSet)ViewState["selectedPrevDateDataForMonth"];
                dsCurrentDateSelectedData = (DataSet)ViewState["selectedCurrentDateDataForMonth"];
                compareID = Session["compareID"] == null ? "" : Session["compareID"].ToString();
                ClientScript.RegisterHiddenField("isPostBack", "1");
            }
        }

        private DataTable getPrevCompareTable()
        {

            List<String> compareIDList = new List<String>();
            ScoringDAL dbAccess = new ScoringDAL();
            compareIDList = dbAccess.getAllExistingCompareIDs();

            String[] comparisonData;
            List<String[]> allComparisonData = new List<String[]>();

            DataTable tblCompareIDs = grdPrevCompareIDs.DataSource as DataTable;
            String[] processedCompareList = new String[8];
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime prevDateTime, currDateTime;
            String compareID;
            if (tblCompareIDs != null)
            {
                for (int i = 0; i < compareIDList.Count; i++)
                {
                    compareID = compareIDList[i];
                    comparisonData = compareID.Split('_');
                    //allComparisonData.Add(comparisonData);
                    DataRow dr = tblCompareIDs.NewRow();
                    tblCompareIDs.Rows.Add(comparisonData);
                }

            }
            else
            {
                tblCompareIDs = new DataTable();
                tblCompareIDs.Columns.Add("Serial");
                tblCompareIDs.Columns.Add("Model");
                tblCompareIDs.Columns.Add("Dates");
                tblCompareIDs.Columns.Add("Version");
                tblCompareIDs.Columns.Add("Mode");
                tblCompareIDs.Columns.Add("Environment");
                tblCompareIDs.Columns.Add("Restriction");
                tblCompareIDs.Columns.Add("Customer");
                for (int i = 0; i < compareIDList.Count; i++)
                {
                    compareID = compareIDList[i];
                    comparisonData = compareID.Split('_');
                    if (comparisonData != null)
                    {
                        DataRow dr = tblCompareIDs.NewRow();
                        dr[0] = i + 1;
                        dr[1] = comparisonData[0];
                        prevDateTime = DateTime.ParseExact(comparisonData[1] + " " + comparisonData[2], "yyyyMMdd HHmmss", provider);
                        currDateTime = DateTime.ParseExact(comparisonData[3] + " " + comparisonData[4], "yyyyMMdd HHmmss", provider);
                        dr[2] = prevDateTime.ToString() + " vs " + currDateTime.ToString();
                        dr[3] = comparisonData[5];
                        dr[4] = comparisonData[6];
                        dr[5] = comparisonData[7];
                        dr[6] = comparisonData[8];
                        dr[7] = comparisonData[9];


                        tblCompareIDs.Rows.Add(dr);
                    }

                    //allComparisonData.Add(comparisonData);
                    //DataRow dr = tblCompareIDs.NewRow();
                    //tblCompareIDs.Rows.Add(processedCompareList);

                }
            }

            return tblCompareIDs;
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
                dsPrevDateSelectedData = dbAccess.GetDatesWithData(selectedMonth,
                                        selectedYear, (hifModel.Value == "") ?
                                        "Risk View" : hifModel.Value);
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
            dsTimeStamps = dbAccess.GetReportGenerationTimes(cdrPrevDate.SelectedDate.ToString("yyyyMMdd"));

            cdrCurrentDate.Enabled = true;
            cdrCurrentDate.VisibleDate = cdrPrevDate.SelectedDate;
            dsCurrentDateSelectedData = dbAccess.GetCurrentDatesWithData(cdrPrevDate.SelectedDate.Month.ToString(),
                                                                        cdrPrevDate.SelectedDate.Year.ToString(),
                                                                        cdrPrevDate.SelectedDate.ToString("yyyyMMdd"),
                                                                        (hifModel.Value == "") ? "Risk View" : hifModel.Value);
            ViewState["selectedCurrentDateDataForMonth"] = dsCurrentDateSelectedData;
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
                    }
                    else
                    {
                        ddlPrevTime.SelectedIndex = 0;
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
            dsTimeStamps = dbAccess.GetReportGenerationTimes(cdrCurrentDate.SelectedDate.ToString("yyyyMMdd"));
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




        protected async void btnHistoryCompare_Click(object sender, EventArgs e)
        {
           

            await Run("History Compare");
        }

        private async Task Run(String typeOfTask)
        {

            userName = HttpContext.Current.User.Identity.Name;
            userName = userName.Substring(userName.IndexOf('\\') + 1);

            ScoringDAL dbAccess = new ScoringDAL();
            progress = "Retrieving values from UI...";

            try
            {
                if (typeOfTask == "History Compare")
                {

                    DataTable tblCompareIDs = getPrevCompareTable();
                    String[] processedCompareList = new String[8];
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (tblCompareIDs != null)
                    {
                        foreach (DataRow compareIDRow in tblCompareIDs.Rows)
                        {
                            if (compareIDRow.ItemArray[0].ToString() == Request.Form["rdoCompareID"].ToString())
                            {
                                String[] datesArray = compareIDRow.ItemArray[2].ToString().Split(new String[] { "vs" }, StringSplitOptions.None);
                                String prevDateTime2 = Convert.ToDateTime(datesArray[0]).ToString("yyyyMMdd_HHmmss");
                                String currentDateTime2 = Convert.ToDateTime(datesArray[1]).ToString("yyyyMMdd_HHmmss");
                                compareID = compareIDRow.ItemArray[1] + "_" + prevDateTime2 + "_" + currentDateTime2 + "_" + compareIDRow.ItemArray[3] + "_" +
                                    compareIDRow.ItemArray[4] + "_" + compareIDRow.ItemArray[5] + "_" + compareIDRow.ItemArray[6] + "_" + compareIDRow.ItemArray[7]; 
                                dbAccess.RefreshData("", "", "", "", "", "", "", "", "", userName, compareID);
                            }
                        }

                    }

                   // compareID = lbCurrentUserGenCIDs.SelectedValue;

                    //Response.Redirect(HttpUtility.HtmlEncode("http://" + Server.MachineName + "/ScoringApp1/ScoringForm.aspx?id=" + compareID), false);
                }
                else
                {
                    String model, version, mode, env, restriction, customer;
                    String previousDate, currentDate;
                    String loadTime = DateTime.Now.ToString();
                    String prevTime = Convert.ToDateTime(ddlPrevTime.SelectedValue.ToString()).ToString("HHmmss");
                    String currentTime = Convert.ToDateTime(ddlCurrentTime.SelectedValue.ToString()).ToString("HHmmss");

                    //Assigning default values for the variables
                    previousDate = (hifPrevDate.Value == "") ? "20130210" : hifPrevDate.Value;
                    currentDate = (hifCurrentDate.Value == "") ? "20130522" : hifCurrentDate.Value;
                    previousDate = cdrPrevDate.SelectedDate.ToString("yyyyMMdd") + "_" + prevTime;
                    currentDate = cdrCurrentDate.SelectedDate.ToString("yyyyMMdd") + "_" + currentTime;

                    model = (hifModel.Value == "") ? "Risk View" : hifModel.Value;
                    version = hifVersion.Value;
                    mode = hifMode.Value;
                    env = hifEnv.Value;
                    restriction = hifRestriction.Value;
                    customer = hifCustomer.Value;
                    userName = HttpContext.Current.User.Identity.Name;
                    userName = userName.Substring(userName.IndexOf('\\') + 1);
                    compareID = model + "_" + previousDate + "_"
                        + currentDate + "_" + version + "_" +
                        mode + "_" + env + "_" + restriction + "_" + customer;
                    Session["compareID"] = compareID;

                    progress = "Running queries to generate data...";
                    //Task refreshDataTask = dbAccess.RefreshData(previousDate, currentDate, model, version, mode, env, restriction, customer, loadTime, userName, compareID);
                    //await Task.WhenAny(refreshDataTask);
                    dbAccess.RefreshData(previousDate, currentDate, model, version, mode, env, restriction, customer, loadTime, userName, compareID);


                    ////RefreshDataAsync(previousDate, currentDate, tableType);
                    //progress = "Generating Excel...";
                    ////dbAccess.GenerateExcel();

                    //summaryPath = (compareID == "") ? "Archive/noCompareID.xlsx" : "Archive/" + compareID + "-Summary.xlsx";
                    //Task generateSummaryExcelTask = dbAccess.getSummaryReport(summaryPath);
                    //summaryPath = HttpUtility.HtmlEncode("http://" + Server.MachineName + "/ScoringApp1/" + summaryPath);
                    //await Task.WhenAny(generateSummaryExcelTask);
                    ////btnGetSummaryReport.Enabled = true;


                    //progress = "Summary Report Generated!";
                    //Thread.Sleep(2000);
                    //progress = "Generating Detailed Report...";

                    //detailedPath = (compareID == "") ? "Archive/noCompareID.xlsx" : "Archive/" + compareID + "-Detailed.xlsx";
                    //Task generateDetailedExcelTask = dbAccess.getDetailedReport(detailedPath);
                    //detailedPath = HttpUtility.HtmlEncode("http://" + Server.MachineName + "/ScoringApp1/" + detailedPath);
                    //await Task.WhenAny(generateDetailedExcelTask);

                    //progress = "Detailed Report Generated!";
                    ////Thread.Sleep(5000);
                    //Response.Redirect(HttpUtility.HtmlEncode("http://" + Server.MachineName + "/ScoringApp1/ScoringForm.aspx?id=" + compareID), false);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            //progress = "Excel Generated";
        }



        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(5000);
            Task runTask = Run("Compare Now");
            await Task.WhenAny(runTask);
            progress = "Excel Generated";

        }

        protected async void btnGetSummaryReport_Click(object sender, EventArgs e)
        {
            String summaryPath;
            summaryPath = (compareID == "") ? "Archive\\noCompareID.xlsx" : compareID + "-Summary.xlsx";
            await sendExcelResponse(summaryPath);
        }

        protected async void btnGetDetailedReport_Click(object sender, EventArgs e)
        {
            String detailedPath;
            detailedPath = (compareID == "") ? "Archive\\noCompareID.xlsx" : compareID + "-Detailed.xlsx";
            await sendExcelResponse(detailedPath);
        }

        private async Task sendExcelResponse(String reportName)
        {
            Task t = new Task
          (
              () =>
              {
                  String xlsPath = Server.MapPath("Archive\\" + reportName);
                  FileInfo fileDet = new System.IO.FileInfo(xlsPath);

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

              }
          );
            t.Start();
            await t;

        }

        private async void RefreshDataAsync(String previousDate, String currentDate, String tableType)
        {
            ScoringDAL dbAccess = new ScoringDAL();
            //dbAccess.RefreshData(previousDate, currentDate, tableType);
            await dbAccess.RefreshDataAsync(previousDate, currentDate, tableType);
        }

        [System.Web.Services.WebMethod]
        public static string GetText()
        {
            //for (int loopIndex = 0; loopIndex < 10; loopIndex++)
            //{
            //    Thread.Sleep(1000);
            //}
            return progress;
        }

        [System.Web.Services.WebMethod]
        public static string GetSummaryPath()
        {
            //for (int loopIndex = 0; loopIndex < 10; loopIndex++)
            //{
            //    Thread.Sleep(1000);
            //}
            return summaryPath;
        }

        [System.Web.Services.WebMethod]
        public static string GetDetailedPath()
        {
            //for (int loopIndex = 0; loopIndex < 10; loopIndex++)
            //{
            //    Thread.Sleep(1000);
            //}
            return detailedPath;
        }


        [System.Web.Services.WebMethod]
        public static Dictionary<String, ArrayList> GetDefaultValues(String model)
        {
            ScoringDAL dbAccess = new ScoringDAL();
            Dictionary<String, ArrayList> defaultValues = new Dictionary<string, ArrayList>();
            defaultValues = dbAccess.GetDefaultLists(model);
            //for (int loopIndex = 0; loopIndex < 10; loopIndex++)
            //{
            //    Thread.Sleep(1000);
            //}
            return defaultValues;
        }

        //protected void lbDailyCompareIDs_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    lbCurrentUserGenCIDs.Enabled = false;
        //    lbCurrentUserGenCIDs.ClearSelection();

        //    lbOtherUserGenCIDs.Enabled = false;
        //    lbOtherUserGenCIDs.ClearSelection();
        //}



    }
}