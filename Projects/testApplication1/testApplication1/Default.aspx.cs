using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace testApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
       
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Console.WriteLine(lblHidden.Text);
        }

        protected DataSet dsHolidays;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cdrPrevRunDate.VisibleDate = DateTime.Today;
                FillHolidayDataset();
            }
        }

        protected void FillHolidayDataset()
        {
            DateTime firstDate = new DateTime(cdrPrevRunDate.VisibleDate.Year,
                cdrPrevRunDate.VisibleDate.Month, 1);
            DateTime lastDate = GetFirstDayOfNextMonth();
            dsHolidays = GetCurrentMonthData(firstDate, lastDate);
        }

        protected DateTime GetFirstDayOfNextMonth()
        {
            int monthNumber, yearNumber;
            if (cdrPrevRunDate.VisibleDate.Month == 12)
            {
                monthNumber = 1;
                yearNumber = cdrPrevRunDate.VisibleDate.Year + 1;
            }
            else
            {
                monthNumber = cdrPrevRunDate.VisibleDate.Month + 1;
                yearNumber = cdrPrevRunDate.VisibleDate.Year;
            }
            DateTime lastDate = new DateTime(yearNumber, monthNumber, 1);
            return lastDate;
        }

        protected DataSet GetCurrentMonthData(DateTime firstDate,
             DateTime lastDate)
{
    DataSet dsMonth = new DataSet();
    ConnectionStringSettings cs;
    cs = ConfigurationManager.ConnectionStrings["DQConnectionString"];
    String connString = cs.ConnectionString;
    SqlConnection dbConnection = new SqlConnection(connString);
    String query;
    query = "SELECT load_date FROM scr_master_runs " +
        " WHERE load_date >= @firstDate AND load_date < @lastDate";
    SqlCommand dbCommand = new SqlCommand(query, dbConnection);
    dbCommand.Parameters.Add(new SqlParameter("@firstDate", 
        firstDate));
    dbCommand.Parameters.Add(new SqlParameter("@lastDate", lastDate));

    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(dbCommand);
    try
    {
        sqlDataAdapter.Fill(dsMonth);
    }
    catch {}
    return dsMonth;
}

        protected void cdrPrevRunDate_Render(object sender, DayRenderEventArgs e)
        {
            DateTime nextDate;
            if (dsHolidays != null)
            {
                foreach (DataRow dr in dsHolidays.Tables[0].Rows)
                {
                    nextDate = (DateTime)dr["load_date"];
                    if (nextDate == e.Day.Date)
                    {
                        e.Cell.BackColor = System.Drawing.Color.Pink;
                    }
                }
            }
        }
        protected void Calendar1_VisibleMonthChanged(object sender,
            MonthChangedEventArgs e)
        {
            FillHolidayDataset();
        }
    }
}
