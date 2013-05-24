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

namespace ScoringApp1
{

    public partial class ScoringForm : System.Web.UI.Page
    {
        DataSet dsSelectedData;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cdrPrevDate.VisibleDate = DateTime.Today;
                cdrCurrentDate.VisibleDate = DateTime.Today;
                FillDatesUsingData(DateTime.Today);

                Dictionary<String, ArrayList> myCollection = new Dictionary<string, ArrayList>();

            }
            else
            {
                dsSelectedData = (DataSet)ViewState["selectedDataForMonth"];
            }
        }
        
        private void FillDatesUsingData(DateTime visibleDate)
        {
            ScoringDAL dbAccess = new ScoringDAL();
            String selectedMonth = visibleDate.Month.ToString();
            String selectedYear = visibleDate.Year.ToString();
            dsSelectedData = dbAccess.GetDatesWithData(selectedMonth, selectedYear);
            ViewState["selectedDataForMonth"] = dsSelectedData;
        }



        protected void cdrPrevDate_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime dateWithData;
            String strDateWithData;
            Boolean flagValue = false;
            if (dsSelectedData != null)
            {
                foreach (DataRow dr in dsSelectedData.Tables[0].Rows)
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
            ddlPrevTime.Items.Clear();
            if (dsTimeStamps != null)
            {
                foreach (DataRow drTimeStamp in dsTimeStamps.Tables[0].Rows)
                {
                    String strPrevTime = drTimeStamp["roxieTime"].ToString();
                    DateTime prevTime = Convert.ToDateTime(strPrevTime.Substring(0, 2) + ":" + strPrevTime.Substring(2, 2) + ":" + strPrevTime.Substring(4, 2));
                    ddlPrevTime.Items.Add(prevTime.ToString("hh:mm:ss tt"));
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



        protected void cdrCurrentDate_SelectionChanged(object sender, EventArgs e)
        {

            DataSet dsTimeStamps;
            ScoringDAL dbAccess = new ScoringDAL();
            dsTimeStamps = dbAccess.GetReportGenerationTimes(cdrCurrentDate.SelectedDate.ToString("yyyyMMdd"));
            ddlCurrentTime.Items.Clear();
            if (dsTimeStamps != null)
            {
                foreach (DataRow drTimeStamp in dsTimeStamps.Tables[0].Rows)
                {
                    String strPrevTime = drTimeStamp["roxieTime"].ToString();
                    DateTime prevTime = Convert.ToDateTime(strPrevTime.Substring(0, 2) + ":" + strPrevTime.Substring(2, 2) + ":" + strPrevTime.Substring(4, 2));
                    ddlCurrentTime.Items.Add(prevTime.ToString("hh:mm:ss tt"));
                }
            }

        }

        protected void cdrCurrentDate_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime dateWithData;
            String strDateWithData;
            Boolean flagValue = false;
            if (dsSelectedData != null)
            {
                foreach (DataRow dr in dsSelectedData.Tables[0].Rows)
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


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            String modelValue;
            String previousDate;
            String currentDate;

            //Assigning default values for the variables
            previousDate = (hifPrevDate.Value == "") ? "20130210" : hifPrevDate.Value;
            currentDate = (hifCurrentDate.Value == "") ? "20130522" : hifCurrentDate.Value;
            previousDate = cdrPrevDate.SelectedDate.ToString("yyyyMMdd");

            currentDate = cdrCurrentDate.SelectedDate.ToString("yyyyMMdd");


            modelValue = (hifModel.Value == "") ? "Risk View" : hifModel.Value;

            //Appending type and model values to match the stored procedure requirements
            String tableType = "";

            tableType = "rvscores";

            ScoringDAL dbAccess = new ScoringDAL();
            dbAccess.RefreshData(previousDate, currentDate, tableType);
            dbAccess.GenerateExcel();

        }

        [System.Web.Services.WebMethod]
        public static string GetText()
        {
            for (int loopIndex = 0; loopIndex < 10; loopIndex++)
            {
                Thread.Sleep(1000);
            }
            return "Loading finished";
        }



    }
}