<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jqGridTest.aspx.cs" Inherits="ScoringApp1.jqGridTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script src="Scripts/jquery-1.7.2.js" type="text/javascript"></script>
     <script src="Scripts/jquery.jqGrid.min.js" type="text/javascript"></script>
     <link rel="stylesheet" href="CSS/ui.jqgrid.css" />
    <link rel="stylesheet" href="CSS/jquery-ui.css" />
    <script type="text/javascript">

        function resAjaxRequestGetPeopleList() {
            if (req.readyState == 4) {
                var res = JSON.parse(req.responseText);
                var thegrid = $("#example")[0];
                thegrid.addJSONData(JSON.parse(res.d));
            }
        }

        function AjaxRequestGetPeopleList() {
            req.open("POST", "/ScoringService.asmx/getHTMLCompareTable1", true);
            req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            req.onreadystatechange = resAjaxRequestGetPeopleList;
            req.send(null);
        }

        $(document).ready(function () {
            $("#example").jqGrid({
                datatype: function (pdata) {
                    AjaxRequestGetPeopleList();
                },
                colName: ['Serial', 'Model', 'Dates','Version','Mode','Environment','Restriction','Customer'],
                colModel: [
                        { name: 'Serial' },
                        { name: 'Model' },
                        { name: 'Dates' },
                        { name: 'Version' },
                        { name: 'Mode' },
                        { name: 'Environment' },
                        { name: 'Restriction' },
                        { name: 'Customer' },
                ],
                rowNum: 10,
                rowList: [10, 20, 30],
                pager: '#pagerBar',
                sortname: 'Serial',
                viewrecords: true,
                sortorder: "desc",
                caption: "Previous Compare IDs"
            });
            $("#example").jqGrid('navGrid', '#pagerBar', { edit: false, add: false, del: false });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table id="example"></table>
        <div id="pagerBar"></div>
    </form>
</body>
</html>
