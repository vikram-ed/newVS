using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using ScoringApp1.helperClasses;


namespace ScoringApp1
{
    [DataObject]
    public class CompareIDDAL
    {

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CompareIDFields> getPrevCompareIDs()
        {
            ScoringDAL dbAccess = new ScoringDAL();
            List<String> compareIDList = new List<String>();
            compareIDList = dbAccess.getAllExistingCompareIDs();

            CultureInfo provider = CultureInfo.InvariantCulture;
            List<CompareIDFields> compareIDObjs = new List<CompareIDFields>();
            String[] comparisonData;
            DateTime prevDateTime, currDateTime;
            String compareID;
            CompareIDFields compareIDObj;
            for (int i = 0; i < compareIDList.Count; i++)
            {
                compareID = compareIDList[i];
                comparisonData = compareID.Split('_');
                if (comparisonData != null)
                {
                    compareIDObj = new CompareIDFields();
                    compareIDObj.Serial = i + 1;
                    compareIDObj.Model = comparisonData[0];
                    prevDateTime = DateTime.ParseExact(comparisonData[1] + " " + comparisonData[2], "yyyyMMdd HHmmss", provider);
                    currDateTime = DateTime.ParseExact(comparisonData[3] + " " + comparisonData[4], "yyyyMMdd HHmmss", provider);
                    compareIDObj.Dates = prevDateTime.ToString() + " vs " + currDateTime.ToString();
                    compareIDObj.Version = comparisonData[5];
                    compareIDObj.Mode = comparisonData[6];
                    compareIDObj.Environment = comparisonData[7];
                    compareIDObj.Restriction = comparisonData[8];
                    compareIDObj.Customer = comparisonData[9];
                    compareIDObjs.Add(compareIDObj);
                }

                //allComparisonData.Add(comparisonData);
                //DataRow dr = tblCompareIDs.NewRow();
                //tblCompareIDs.Rows.Add(processedCompareList);

            }
            return compareIDObjs;
        }
    }
}