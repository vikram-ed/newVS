//DownloadSpreadsheet.ashx:

//<%@ WebHandler Language="C#" Class="DownloadSpreadsheetHandler" %>

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.IO;

//namespace ScoringApp1.App_Code
//{
 


//public class ExcelHandler1: IHttpHandler {

//    public void ProcessRequest (HttpContext context) {
//        context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
//        string path = context.Server.MapPath("~/Book1.xlsx");
//        using (FileStream spreadsheet = File.OpenRead(path))
//        {
//            CopyStream(spreadsheet, context.Response.OutputStream);
//        }
//    }

//    public bool IsReusable {
//        get {
//            return false;
//        }
//    }

//    private static void CopyStream(Stream input, Stream output)
//    {
//        byte[] buffer = new byte[32768];
//        while (true)
//        {
//            int read = input.Read(buffer, 0, buffer.Length);
//            if (read <= 0)
//                return;
//            output.Write(buffer, 0, read);
//        }
//    }

//}
//}