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

namespace ScoringApp1
{

    public partial class ScoringForm : System.Web.UI.Page
    {
        DataSet dsPrevDateSelectedData, dsCurrentDateSelectedData;

        public static String progress;
        /// <summary>
        /// Page load event handler
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cdrPrevDate.VisibleDate = DateTime.Today;
                //cdrCurrentDate.VisibleDate = DateTime.Today;
                cdrCurrentDate.Enabled = false;
                FillDatesUsingData(DateTime.Today);
                ddlPrevTime.Enabled = false;
                Dictionary<String, ArrayList> myCollection = new Dictionary<string, ArrayList>();
                ScoringDAL dbAccess = new ScoringDAL();
                myCollection = dbAccess.GetDefaultLists("Risk View");
                ViewState["listsDictionary"] = myCollection;
                Page.ClientScript.RegisterHiddenField("vCode", ViewState["listsDictionary"].ToString());

            }
            else
            {
                dsPrevDateSelectedData = (DataSet)ViewState["selectedPrevDateDataForMonth"];
                dsCurrentDateSelectedData = (DataSet)ViewState["selectedCurrentDateDataForMonth"];
            }
        }


        /// <summary>
        /// Get dates with data and fill the global dataset with it
        /// </summary>
        private void FillDatesUsingData(DateTime visibleDate)
        {
            ScoringDAL dbAccess = new ScoringDAL();
            String selectedMonth = visibleDate.Month.ToString();
            String selectedYear = visibleDate.Year.ToString();
            dsPrevDateSelectedData = dbAccess.GetDatesWithData(selectedMonth, selectedYear);
            ViewState["selectedPrevDateDataForMonth"] = dsPrevDateSelectedData;
            
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
                                                                        cdrPrevDate.SelectedDate.ToString("yyyyMMdd"));
            ViewState["selectedCurrentDateDataForMonth"] = dsCurrentDateSelectedData;
            ddlPrevTime.Items.Clear();
            if (dsTimeStamps != null)
            {
                foreach (DataRow drTimeStamp in dsTimeStamps.Tables[0].Rows)
                {
                    String strPrevTime = drTimeStamp["roxieTime"].ToString();
                    TimeSpan ts = new TimeSpan(Convert.ToInt32(strPrevTime.Substring(0, 2)),
                                               Convert.ToInt32(strPrevTime.Substring(2, 2)),
                                               Convert.ToInt32(strPrevTime.Substring(4, 2)));
                    DateTime prevTime = cdrPrevDate.SelectedDate + ts;
                    //DateTime prevTime = Convert.ToDateTime(strPrevTime.Substring(0, 2) + ":" + strPrevTime.Substring(2, 2) + ":" + strPrevTime.Substring(4, 2));
                    ddlPrevTime.Items.Add("Previous Time");
                    ddlPrevTime.Items.Add(prevTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    ddlPrevTime.Enabled = true;
                }
            }
            // RenderControl(cdrPrevDate);
            //FillDatesUsingData(cdrPrevDate.SelectedDate);

        }

        //public String RenderControl(Control ctrl)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    StringWriter sw = new StringWriter(sb);
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //    ctrl.RenderControl(hw);
        //    return sb.ToString();
        //}

        protected void cdrPrevDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            cdrPrevDate.VisibleDate = e.NewDate;
            FillDatesUsingData(e.NewDate);
        }

        #endregion
        #region "CurrentDate calendar events"
        protected void cdrCurrentDate_SelectionChanged(object sender, EventArgs e)
        {

            DataSet dsTimeStamps;
            ArrayList alTimeStamps= new ArrayList();
            ScoringDAL dbAccess = new ScoringDAL();
            dsTimeStamps = dbAccess.GetReportGenerationTimes(cdrCurrentDate.SelectedDate.ToString("yyyyMMdd"));
            ddlCurrentTime.Items.Clear();
            if (dsTimeStamps != null)
            {
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
                    ddlCurrentTime.Items.Add("Current Time");
                    ddlCurrentTime.Items.Add(prevTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    ddlCurrentTime.Enabled = true;
                }
                //ddlCurrentTime.DataSource = alTimeStamps;
                //ddlCurrentTime.DataBind();
            }

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
        #endregion

        /// <summary>
        /// PageMethod for triggering long running operation
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object Operation()
        {
            HttpSessionState session = HttpContext.Current.Session;

            //Separate thread for long running operation
            ThreadPool.QueueUserWorkItem(delegate
            {

                int operationProgress;

                for (operationProgress = 0; operationProgress <= 100; operationProgress = operationProgress + 2)
                {
                    session["OPERATION_PROGRESS"] = operationProgress;
                    Thread.Sleep(1000);
                }
            });

            return new { progress = 0 };
        }

        /// <summary>
        /// PageMethod for reporting progress
        /// </summary>
        /// <returns></returns>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object OperationProgress()
        {
            int operationProgress = 0;

            if (HttpContext.Current.Session["OPERATION_PROGRESS"] != null)
                operationProgress = (int)HttpContext.Current.Session["OPERATION_PROGRESS"];

            return new { progress = operationProgress };
        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {

            String modelValue, version, mode, env, restriction, customer;
            String previousDate;
            String currentDate;

            progress = "Retrieving values...";

            btnSubmit.Enabled = false;

            Debug.WriteLine(hifEnv.Value);
            //Assigning default values for the variables
            previousDate = (hifPrevDate.Value == "") ? "20130210" : hifPrevDate.Value;
            currentDate = (hifCurrentDate.Value == "") ? "20130522" : hifCurrentDate.Value;
            previousDate = cdrPrevDate.SelectedDate.ToString("yyyyMMdd");

            currentDate = cdrCurrentDate.SelectedDate.ToString("yyyyMMdd");


            modelValue = (hifModel.Value == "") ? "Risk View" : hifModel.Value;
            version = hifVersion.Value;
            mode = hifMode.Value;
            env = hifEnv.Value;
            restriction = hifRestriction.Value;
            customer = hifCustomer.Value;

            ScoringDAL dbAccess = new ScoringDAL();
            progress = "Running queries to generate data...";
            dbAccess.RefreshData(previousDate, currentDate, modelValue, version, mode, env, restriction, customer, DateTime.Now.ToString());
            //RefreshDataAsync(previousDate, currentDate, tableType);
            progress = "Generating Excel...";
            Task generateExcelTask = ScoringDAL.GenerateExcel();
            await Task.WhenAny(generateExcelTask);
            //dbAccess.RefreshExcel();

            //Task genExcelTask = ScoringDAL.GenerateExcelAsync();

            //genExcelTask.Wait();
            Thread.Sleep(1000);
            progress = "Excel Generated";


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



    }
}