using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;

namespace testApplication1
{
    public partial class testPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
 
            String typeValue;
            String modelValue;
            String previousDate;
            String currentDate;

            //Assigning default values for the variables
            previousDate = (hifPrevDate.Value == "") ? "20130210" : hifPrevDate.Value;
            currentDate = (hifCurrentDate.Value == "") ? "20130522" : hifCurrentDate.Value;
            
            modelValue = (hifModel.Value == "") ? "Risk View" : hifModel.Value;
            
            //Appending type and model values to match the stored procedure requirements
            String tableType="";
           
                tableType = "rvscores";
            

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

                //updating the excel document
                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = false;
                string workbookPath = "C:\\Users\\parevi01\\Documents\\LexisNexis\\Compare_Reports_v03.xlsx";

                string physicalPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                //workbookPath = physicalPath + "Compare_Reports_v03.xlsx";


                Excel.Workbook sampleWorkBook = excelApp.Workbooks.Open(workbookPath);
                
                sampleWorkBook.RefreshAll();

                System.Threading.Thread.Sleep(2000);
                sampleWorkBook.Save();
                System.Threading.Thread.Sleep(2000);
                excelApp.Workbooks.Close();
                excelApp.Quit();


                excelApp = new Excel.Application();
                excelApp.Visible = true;
                
                sampleWorkBook = excelApp.Workbooks.Open(workbookPath, 0, false, 5, "", "",
                                false, Excel.XlPlatform.xlWindows, "", true, true, 0, true, false, false);

            }
           
    
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
