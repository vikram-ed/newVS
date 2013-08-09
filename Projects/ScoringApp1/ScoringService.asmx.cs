using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ScoringApp1
{
    /// <summary>
    /// Summary description for ScoringService
    /// </summary>
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ScoringService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getHTMLCompareTable1()
        {

            Dictionary<String, String> compareIDs = new Dictionary<String, String>();
            ScoringDAL dbAccess = new ScoringDAL();
            compareIDs = dbAccess.getAllExistingCompareIDs();

            String compareID;
            String[] comparisonData;
            DateTime prevDateTime, currDateTime;
            CultureInfo provider = CultureInfo.InvariantCulture;
            List<String[]> allCompareData = new List<String[]>();
            int i = 0;
            foreach (KeyValuePair<String,String> userCompareID in compareIDs)
            {

                compareID = userCompareID.Key;
                comparisonData = compareID.Split('_');
                if (comparisonData != null)
                {
                    
                    String[] tableData = new String[8];

                    //dr[0] = i + 1 + "," + comparisonData[0] + "," + prevDateTime.ToString() + " vs " +
                    //    currDateTime.ToString() + "," + "v" + comparisonData[5] + "," +
                    //    comparisonData[6] + "," + comparisonData[7] + "," + comparisonData[8] + "," + comparisonData[9];
                    tableData[0] = (i + 1).ToString();
                    tableData[1] = comparisonData[0];
                    prevDateTime = DateTime.ParseExact(comparisonData[1] + " " + comparisonData[2], "yyyyMMdd HHmmss", provider);
                    currDateTime = DateTime.ParseExact(comparisonData[3] + " " + comparisonData[4], "yyyyMMdd HHmmss", provider);
                    tableData[2] = prevDateTime.ToString() + " vs " + currDateTime.ToString();
                    tableData[3] = "v" + comparisonData[5];
                    tableData[4] = comparisonData[6];
                    tableData[5] = comparisonData[7];
                    tableData[6] = comparisonData[8];
                    tableData[7] = comparisonData[9];
                    allCompareData.Add(tableData);
                }
            }
            //for (int i = 0; i < compareIDList.Count; i++)
            //{
            //    compareID = compareIDList[i];
            //    comparisonData = compareID.Split('_');
            //    if (comparisonData != null)
            //    {
                    
            //        String[] tableData = new String[8];

            //        //dr[0] = i + 1 + "," + comparisonData[0] + "," + prevDateTime.ToString() + " vs " +
            //        //    currDateTime.ToString() + "," + "v" + comparisonData[5] + "," +
            //        //    comparisonData[6] + "," + comparisonData[7] + "," + comparisonData[8] + "," + comparisonData[9];
            //        tableData[0] = (i + 1).ToString();
            //        tableData[1] = comparisonData[0];
            //        prevDateTime = DateTime.ParseExact(comparisonData[1] + " " + comparisonData[2], "yyyyMMdd HHmmss", provider);
            //        currDateTime = DateTime.ParseExact(comparisonData[3] + " " + comparisonData[4], "yyyyMMdd HHmmss", provider);
            //        tableData[2] = prevDateTime.ToString() + " vs " + currDateTime.ToString();
            //        tableData[3] = "v" + comparisonData[5];
            //        tableData[4] = comparisonData[6];
            //        tableData[5] = comparisonData[7];
            //        tableData[6] = comparisonData[8];
            //        tableData[7] = comparisonData[9];
            //        allCompareData.Add(tableData);
            //    }

            //    //allComparisonData.Add(comparisonData);
            //    //DataRow dr = tblCompareIDs.NewRow();
            //    //tblCompareIDs.Rows.Add(processedCompareList);


            //}
           // return "{\"aaData\" : " + GetJson(tblCompareIDs) + "}";
            return GetJson1(allCompareData);

        }

        public static string GetJson1(List<String[]> allComparisonData)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Object[] rowsData = new Object[allComparisonData.Count];
            Dictionary<string, object> row = null;

            for (int count = 0; count < allComparisonData.Count; count++)
            {
                row = new Dictionary<string, object>();

                row.Add("id", (count + 1).ToString());
                row.Add("cell", allComparisonData[count]);

                rowsData[count] = row;
            }


            row = new Dictionary<string, object>();
            row.Add("total", "1");
            row.Add("page", "1");
            row.Add("records", allComparisonData.Count);
            row.Add("rows", rowsData);
            rows.Add(row);
            

            return serializer.Serialize(rows);
        }


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        //public string getHTMLCompareTable()
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

        //   // return "{\"aaData\" : " + GetJson(tblCompareIDs) + "}";
        //    return GetJson(tblCompareIDs);

        //}

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
    }
}
