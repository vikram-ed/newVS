using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ScoringApp1
{
    public partial class test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //List<String> compareIDList = new List<String>();
            //ScoringDAL dbAccess = new ScoringDAL();
            //compareIDList = dbAccess.getAllExistingCompareIDs();

            //String[] comparisonData;
            //List<String[]> allComparisonData = new List<String[]>();

            //String[] processedCompareList = new String[8];
            //CultureInfo provider = CultureInfo.InvariantCulture;
            //DateTime prevDateTime, currDateTime;
            //String compareID;

            ////tblCompareIDs.Columns.Add("Serial");
            ////tblCompareIDs.Columns.Add("Model");
            ////tblCompareIDs.Columns.Add("Dates");
            ////tblCompareIDs.Columns.Add("Version");
            ////tblCompareIDs.Columns.Add("Mode");
            ////tblCompareIDs.Columns.Add("Environment");
            ////tblCompareIDs.Columns.Add("Restriction");
            ////tblCompareIDs.Columns.Add("Customer");
            // HtmlTableCell serial, model, dates, version, mode, env, restriction,customer;
            //HtmlTableRow dr;
            //for (int i = 0; i < compareIDList.Count; i++)
            //{
            //    compareID = compareIDList[i];
            //    comparisonData = compareID.Split('_');
            //    if (comparisonData != null)
            //    {
            //         dr = new HtmlTableRow();
            //         serial = new HtmlTableCell("td");
            //        serial.InnerText = (i+1).ToString();
            //        dr.Cells.Add(serial);

            //         model = new HtmlTableCell("td");
            //        model.InnerText = comparisonData[0];
            //        dr.Cells.Add(model);

            //        prevDateTime = DateTime.ParseExact(comparisonData[1] + " " + comparisonData[2], "yyyyMMdd HHmmss", provider);
            //        currDateTime = DateTime.ParseExact(comparisonData[3] + " " + comparisonData[4], "yyyyMMdd HHmmss", provider);
            //         dates = new HtmlTableCell("td");
            //        dates.InnerText = prevDateTime.ToString() + " vs " + currDateTime.ToString();
            //        dr.Cells.Add(dates);

            //         version = new HtmlTableCell("td");
            //        version.InnerText = "v" + comparisonData[5];
            //        dr.Cells.Add(version);

            //         mode = new HtmlTableCell("td");
            //        mode.InnerText = comparisonData[6];
            //        dr.Cells.Add(mode);

            //         env = new HtmlTableCell("td");
            //        env.InnerText = comparisonData[7];
            //        dr.Cells.Add(env);

            //         restriction = new HtmlTableCell("td");
            //        restriction.InnerText = comparisonData[8];
            //        dr.Cells.Add(restriction);

            //         customer = new HtmlTableCell("td");
            //        customer.InnerText = comparisonData[9];
            //        dr.Cells.Add(customer);



            //        tblCompareIDs.Rows.Add(dr);
            //    }

            //    //allComparisonData.Add(comparisonData);
            //    //DataRow dr = tblCompareIDs.NewRow();
            //    //tblCompareIDs.Rows.Add(processedCompareList);


            //}
        }

      
    }
}