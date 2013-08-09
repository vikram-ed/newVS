<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test1.aspx.cs" Inherits="ScoringApp1.test1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <style type="text/css" title="currentStyle">
        @import "CSS/demo_page.css";
        @import "CSS/demo_table.css";
    </style>
    <script src="Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery.selectbox-0.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.progressbar.js" type="text/javascript"></script>
    <script src="Scripts/glDatePicker.js" type="text/javascript"></script>
    <script src="Scripts/progressbutton.jquery.js"></script>
    <script type="text/javascript" src="Scripts/jquery.dataTables.js"></script>



    <script type="text/javascript" charset="utf-8">



        (function ($) {
            /*
             * Function: fnGetColumnData
             * Purpose:  Return an array of table values from a particular column.
             * Returns:  array string: 1d data array 
             * Inputs:   object:oSettings - dataTable settings object. This is always the last argument past to the function
             *           int:iColumn - the id of the column to extract the data from
             *           bool:bUnique - optional - if set to false duplicated values are not filtered out
             *           bool:bFiltered - optional - if set to false all the table data is used (not only the filtered)
             *           bool:bIgnoreEmpty - optional - if set to false empty values are not filtered from the result array
             * Author:   Benedikt Forchhammer <b.forchhammer /AT\ mind2.de>
             */
            $.fn.dataTableExt.oApi.fnGetColumnData = function (oSettings, iColumn, bUnique, bFiltered, bIgnoreEmpty) {
                // check that we have a column id
                if (typeof iColumn == "undefined") return new Array();

                // by default we only want unique data
                if (typeof bUnique == "undefined") bUnique = true;

                // by default we do want to only look at filtered data
                if (typeof bFiltered == "undefined") bFiltered = true;

                // by default we do not want to include empty values
                if (typeof bIgnoreEmpty == "undefined") bIgnoreEmpty = true;

                // list of rows which we're going to loop through
                var aiRows;

                // use only filtered rows
                if (bFiltered == true) aiRows = oSettings.aiDisplay;
                    // use all rows
                else aiRows = oSettings.aiDisplayMaster; // all row numbers

                // set up data array	
                var asResultData = new Array();

                for (var i = 0, c = aiRows.length; i < c; i++) {
                    iRow = aiRows[i];
                    var aData = this.fnGetData(iRow);
                    var sValue = aData[iColumn];

                    // ignore empty values?
                    if (bIgnoreEmpty == true && sValue.length == 0) continue;

                        // ignore unique values?
                    else if (bUnique == true && jQuery.inArray(sValue, asResultData) > -1) continue;

                        // else push the value onto the result data array
                    else asResultData.push(sValue);
                }

                return asResultData;
            }
        }(jQuery));


        function fnCreateSelect(aData, colName) {
            var r = '<select id="prev'+colName+'"><option value="" selected style="width:25px">'+colName+'</option>', i, iLen = aData.length;
     
            for (i = 0 ; i < iLen ; i++) {
                r += '<option value="' + aData[i] + '" style="width:25px">' + aData[i] + '</option>';
            }
            return r + '</select>';
        }
        var tablStr, tableArr;
        function getTable(jsonStr) {
            jsonStr = $.parseJSON(jsonStr);
            //tablStr = "<thead><tr><th>Serial</th><th>Model</th><th>Dates</th><th>Version</th><th>Mode</th><th>Environment</th><th>Restriction</th><th>Customer</th></tr></thead><tbody>";
            //for (var count = 0; count < jsonStr.length; count++) {
            //    tablStr += "<tr><td>" + jsonStr[count].Serial + "</td><td>" + jsonStr[count].Model +
            //        "</td><td>" + jsonStr[count].Dates + "</td><td>" +
            //        jsonStr[count].Version + "</td><td>" + jsonStr[count].Mode + "</td><td>" +
            //        jsonStr[count].Environment + "</td><td>" + jsonStr[count].Restriction + "</td><td>" + jsonStr[count].Customer + "</td></tr>";
            //}
            tableArr = new Array();
            for (var count = 0; count < jsonStr.length; count++) {
                tableArr[count] = [jsonStr[count].Serial, jsonStr[count].Model, jsonStr[count].Dates, jsonStr[count].Version, jsonStr[count].Mode,
                    jsonStr[count].Environment, jsonStr[count].Restriction, jsonStr[count].Customer];

            }
            //tablStr += "</tbody><tfoot><tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr></tfoot>";
            //document.getElementById("example").innerHTML = tablStr;
            return tableArr;
        }
        var oTable, tableStr;

        function fnGetSelected(oTableLocal) {
            return oTableLocal.$('tr.row_selected');
        }
        function initClickEvents() {
            $("#example tbody").click(function (event) {

                $(oTable.fnSettings().aoData).each(function () {
                    $(this.nTr).removeClass('row_selected');
                });
                $(event.target.parentNode).addClass('row_selected');
                var gallery = $(event.target.parentNode);
                var contents = gallery.find("td").contents();
                alert(contents[0].data);
            });

            $("#example tbody tr").live('click', function (event) {
                var aPos = oTable.fnGetPosition(this);
                var aData = oTable.fnGetData(aPos);
                gIDNumber = aData[0];


            });
        }
        function filterAndSelect() {

            /* Add a select menu for each TH element in the table footer */
            $("#filtersTable thead tr th").each(function (i) {
                oTable = $("#example").dataTable();
                this.innerHTML = fnCreateSelect(oTable.fnGetColumnData(i));
                $('select', this).change(function () {
                    oTable.fnFilter($(this).val(), i);
                });
            });
        }


        function InitOverviewDataTable() {
        
            $.ajax({
                type: "POST",
                url: "ScoringService.asmx/getHTMLCompareTable",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    tableStr = msg.d;
                    if (typeof oTable == 'undefined') {
                        oTable = $("#example").dataTable({
                            "aaData": getTable(msg.d),
                            "bAutoWidth" : false,
                            "aoColumns": [
{ "sTitle": "", "sWidth": "10px", "sClass":"center" },
{ "sTitle": "Model", "sWidth": "30px" },
{ "sTitle": "Dates", "sWidth": "70px" },
{ "sTitle": "Version", "sWidth": "30px" },
{ "sTitle": "Mode", "sWidth": "30px" },
{ "sTitle": "Env.", "sClass": "center", "sWidth": "30px" },
{ "sTitle": "Restr.", "sClass": "center", "sWidth": "30px" },
{ "sTitle": "Cust.", "sClass": "center", "sWidth": "30px" }],
                            "bPaginate": true,
                            "sPaginationType": 'full_numbers',
                            "fnInitComplete": function (oSettings, json) {
                                /* Add a select menu for each TH element in the table footer */
                                $("#example thead tr th").each(function (i) {
                                    oTable = $("#example").dataTable();
                                    if (i != 2 &&  i!=0) {
                                        this.innerHTML = fnCreateSelect(oTable.fnGetColumnData(i), this.textContent);
                                        $('select', this).change(function () {
                                            oTable.fnFilter($(this).val(), i);
                                        });
                                    }
                                });
                            }
                        });
                    }
                    else {
                        oTable.fnClearTable(0);
                        oTable.fnDraw();
                    }
                }

            });
        }

        function RefreshTable(tableId, urlData) {
            $.getJSON(urlData, null, function (json) {
                table = $(tableId).dataTable();
                oSettings = table.fnSettings();

                table.fnClearTable(this);

                for (var i = 0; i < json.aaData.length; i++) {
                    table.oApi._fnAddData(oSettings, json.aaData[i]);
                }

                oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
                table.fnDraw();
            });
        }

        function AutoReload() {
            RefreshTable('#example', 'ScoringService.asmx/getHTMLCompareTable');

            setTimeout(function () { AutoReload(); }, 30000);
        }



        $(document).ready(function () {

            initClickEvents();
            InitOverviewDataTable();
            //setTimeout(function () { AutoReload(); }, 30000);   

        });

      
    </script>
</head>
<body id="dt_example">
    <div id="container">
        <div class="full_width big">
            DataTables individual column filtering example (using select menus)
        </div>

        <h1>Preamble</h1>
        <p>This example is almost identical to <a href="multi_filter.html">individual column example</a> and provides the same functionality, but using &lt;select&gt; menus rather than input elements. The API plug-in function fnGetColumnData from Benedikt Forchhammer provides much of the logic processing required, and integration with a table is almost trivial.</p>
        <p>One possible interaction chance would be to make use of fnGetColumnData's ability to get filtered data, so you could have the possible filtering values in the select menus to update to only those in the table, rather than all values.</p>
      
        <table style="border-width:1px; border-style:inset" class="display" id="example">
            <thead  style="background-color:#CC0033; color:white">
                <tr>
                    <th>Serial</th>
                    <th>Model</th>
                    <th>Dates</th>
                    <th>Version</th>
                    <th>Mode</th>
                    <th>Environment</th>
                    <th>Restriction</th>
                    <th>Customer</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </tbody>
            <tfoot>
                <tr>
                    <th></th>
                    <th></th>
                    <th></th>
                    <th></th>
                    <th></th>
                    <th></th>
                    <th></th>
                    <th></th>
                </tr>
            </tfoot>

        </table>
        <%--  <table class="display" id="tblCompareIDs" runat="server">
             <thead>
                <tr>
                    <th>Serial</th>
                    <th>Model</th>
                    <th>Dates</th>
                    <th>Version</th>
                    <th>Mode</th>
                    <th>Environment</th>
                    <th>Restriction</th>
                    <th>Customer</th>
                </tr>
            </thead>
        </table>--%>
        <h1>Live example</h1>
        <div id="demo">
            <input type="button" id="btnClick" value="click" />
            <%-- <table cellpadding="0" cellspacing="0" border="0" class="display" id="example1">
                <thead>
                    <tr>
                        <th>Rendering engine</th>
                        <th>Browser</th>
                        <th>Platform(s)</th>
                        <th>Engine version</th>
                        <th>CSS grade</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="gradeX">
                        <td>Trident</td>
                        <td>Internet
				 Explorer 4.0</td>
                        <td>Win 95+</td>
                        <td class="center">4</td>
                        <td class="center">X</td>
                    </tr>
                    <tr class="gradeC">
                        <td>Trident</td>
                        <td>Internet
				 Explorer 5.0</td>
                        <td>Win 95+</td>
                        <td class="center">5</td>
                        <td class="center">C</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Trident</td>
                        <td>Internet
				 Explorer 5.5</td>
                        <td>Win 95+</td>
                        <td class="center">5.5</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Trident</td>
                        <td>Internet
				 Explorer 6</td>
                        <td>Win 98+</td>
                        <td class="center">6</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Trident</td>
                        <td>Internet Explorer 7</td>
                        <td>Win XP SP2+</td>
                        <td class="center">7</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Trident</td>
                        <td>AOL browser (AOL desktop)</td>
                        <td>Win XP</td>
                        <td class="center">6</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Firefox 1.0</td>
                        <td>Win 98+ / OSX.2+</td>
                        <td class="center">1.7</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Firefox 1.5</td>
                        <td>Win 98+ / OSX.2+</td>
                        <td class="center">1.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Firefox 2.0</td>
                        <td>Win 98+ / OSX.2+</td>
                        <td class="center">1.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Firefox 3.0</td>
                        <td>Win 2k+ / OSX.3+</td>
                        <td class="center">1.9</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Camino 1.0</td>
                        <td>OSX.2+</td>
                        <td class="center">1.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Camino 1.5</td>
                        <td>OSX.3+</td>
                        <td class="center">1.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Netscape 7.2</td>
                        <td>Win 95+ / Mac OS 8.6-9.2</td>
                        <td class="center">1.7</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Netscape Browser 8</td>
                        <td>Win 98SE+</td>
                        <td class="center">1.7</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Netscape Navigator 9</td>
                        <td>Win 98+ / OSX.2+</td>
                        <td class="center">1.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.0</td>
                        <td>Win 95+ / OSX.1+</td>
                        <td class="center">1</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.1</td>
                        <td>Win 95+ / OSX.1+</td>
                        <td class="center">1.1</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.2</td>
                        <td>Win 95+ / OSX.1+</td>
                        <td class="center">1.2</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.3</td>
                        <td>Win 95+ / OSX.1+</td>
                        <td class="center">1.3</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.4</td>
                        <td>Win 95+ / OSX.1+</td>
                        <td class="center">1.4</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.5</td>
                        <td>Win 95+ / OSX.1+</td>
                        <td class="center">1.5</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.6</td>
                        <td>Win 95+ / OSX.1+</td>
                        <td class="center">1.6</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.7</td>
                        <td>Win 98+ / OSX.1+</td>
                        <td class="center">1.7</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Mozilla 1.8</td>
                        <td>Win 98+ / OSX.1+</td>
                        <td class="center">1.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Seamonkey 1.1</td>
                        <td>Win 98+ / OSX.2+</td>
                        <td class="center">1.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Gecko</td>
                        <td>Epiphany 2.20</td>
                        <td>Gnome</td>
                        <td class="center">1.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Webkit</td>
                        <td>Safari 1.2</td>
                        <td>OSX.3</td>
                        <td class="center">125.5</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Webkit</td>
                        <td>Safari 1.3</td>
                        <td>OSX.3</td>
                        <td class="center">312.8</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Webkit</td>
                        <td>Safari 2.0</td>
                        <td>OSX.4+</td>
                        <td class="center">419.3</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Webkit</td>
                        <td>Safari 3.0</td>
                        <td>OSX.4+</td>
                        <td class="center">522.1</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Webkit</td>
                        <td>OmniWeb 5.5</td>
                        <td>OSX.4+</td>
                        <td class="center">420</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Webkit</td>
                        <td>iPod Touch / iPhone</td>
                        <td>iPod</td>
                        <td class="center">420.1</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Webkit</td>
                        <td>S60</td>
                        <td>S60</td>
                        <td class="center">413</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Opera 7.0</td>
                        <td>Win 95+ / OSX.1+</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Opera 7.5</td>
                        <td>Win 95+ / OSX.2+</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Opera 8.0</td>
                        <td>Win 95+ / OSX.2+</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Opera 8.5</td>
                        <td>Win 95+ / OSX.2+</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Opera 9.0</td>
                        <td>Win 95+ / OSX.3+</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Opera 9.2</td>
                        <td>Win 88+ / OSX.3+</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Opera 9.5</td>
                        <td>Win 88+ / OSX.3+</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Opera for Wii</td>
                        <td>Wii</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Nokia N800</td>
                        <td>N800</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Presto</td>
                        <td>Nintendo DS browser</td>
                        <td>Nintendo DS</td>
                        <td class="center">8.5</td>
                        <td class="center">C/A<sup>1</sup></td>
                    </tr>
                    <tr class="gradeC">
                        <td>KHTML</td>
                        <td>Konqureror 3.1</td>
                        <td>KDE 3.1</td>
                        <td class="center">3.1</td>
                        <td class="center">C</td>
                    </tr>
                    <tr class="gradeA">
                        <td>KHTML</td>
                        <td>Konqureror 3.3</td>
                        <td>KDE 3.3</td>
                        <td class="center">3.3</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeA">
                        <td>KHTML</td>
                        <td>Konqureror 3.5</td>
                        <td>KDE 3.5</td>
                        <td class="center">3.5</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeX">
                        <td>Tasman</td>
                        <td>Internet Explorer 4.5</td>
                        <td>Mac OS 8-9</td>
                        <td class="center">-</td>
                        <td class="center">X</td>
                    </tr>
                    <tr class="gradeC">
                        <td>Tasman</td>
                        <td>Internet Explorer 5.1</td>
                        <td>Mac OS 7.6-9</td>
                        <td class="center">1</td>
                        <td class="center">C</td>
                    </tr>
                    <tr class="gradeC">
                        <td>Tasman</td>
                        <td>Internet Explorer 5.2</td>
                        <td>Mac OS 8-X</td>
                        <td class="center">1</td>
                        <td class="center">C</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Misc</td>
                        <td>NetFront 3.1</td>
                        <td>Embedded devices</td>
                        <td class="center">-</td>
                        <td class="center">C</td>
                    </tr>
                    <tr class="gradeA">
                        <td>Misc</td>
                        <td>NetFront 3.4</td>
                        <td>Embedded devices</td>
                        <td class="center">-</td>
                        <td class="center">A</td>
                    </tr>
                    <tr class="gradeX">
                        <td>Misc</td>
                        <td>Dillo 0.8</td>
                        <td>Embedded devices</td>
                        <td class="center">-</td>
                        <td class="center">X</td>
                    </tr>
                    <tr class="gradeX">
                        <td>Misc</td>
                        <td>Links</td>
                        <td>Text only</td>
                        <td class="center">-</td>
                        <td class="center">X</td>
                    </tr>
                    <tr class="gradeX">
                        <td>Misc</td>
                        <td>Lynx</td>
                        <td>Text only</td>
                        <td class="center">-</td>
                        <td class="center">X</td>
                    </tr>
                    <tr class="gradeC">
                        <td>Misc</td>
                        <td>IE Mobile</td>
                        <td>Windows Mobile 6</td>
                        <td class="center">-</td>
                        <td class="center">C</td>
                    </tr>
                    <tr class="gradeC">
                        <td>Misc</td>
                        <td>PSP browser</td>
                        <td>PSP</td>
                        <td class="center">-</td>
                        <td class="center">C</td>
                    </tr>
                    <tr class="gradeU">
                        <td>Other browsers</td>
                        <td>All others</td>
                        <td>-</td>
                        <td class="center">-</td>
                        <td class="center">U</td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                    </tr>
                </tfoot>
            </table> --%>
        </div>



    </div>
</body>
</html>
