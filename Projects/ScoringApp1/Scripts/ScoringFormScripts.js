
$(function () {


    var current_h = null;
    var current_w = null;
    //function to toggle the css class for a given element
    function toggle(element, clsToBeAdded, clsToBeRemoved) {
        $(element).removeClass(clsToBeRemoved);
        $(element).addClass(clsToBeAdded);
    }

    //$("div#content").addClass("hide");
    //$("div#historyContent").addClass("hide");
    //$("div#eclDataContent").addClass("hide");
    $('.resize').hover(
        function () {
            current_h = $(this, 'div')[0].height;
            current_w = $(this, 'div')[0].width;
            $(this).stop(true, false).animate({ width: (current_w * 1.3), height: (current_h * 1.3) }, 300);
        },
        function () {
            $(this).stop(true, false).animate({ width: current_w + 'px', height: current_h + 'px' }, 300);
        }
    );

    $("#compareDiv").hover(
        function () {
            $(this).animate({ "opacity": "1" }, 350);
            //$("#description").html("<p></p>").animate({ "opacity": "1" }, 350);
            // $("div#description").animate({ "opacity": "1" }, 350);
            toggle("p#pCompareNow", "show", "hidden");
            toggle("p#pMsg", "hidden", "show");
        },
        function () {
            $(this).animate({ "opacity": "0.3" }, 350);
            // $("div#description").animate({ "opacity": "0" }, 350);
            // $("p#pCompareNow").css({ "visibility": "hidden" });
            toggle("p#pCompareNow", "hidden", "show");
            toggle("p#pMsg", "show", "hidden");
        });
    $("#historyDiv").hover(
    function () {
        $(this).animate({ "opacity": "1" }, 350);
        //$("div#description").animate({ "opacity": "1" }, 350);
        toggle("p#pHistory", "show", "hidden");
        toggle("p#pMsg", "hidden", "show");
    },
    function () {
        $(this).animate({ "opacity": "0.3" }, 350);
        // $("div#description").animate({ "opacity": "0" }, 350);
        toggle("p#pHistory", "hidden", "show");
        toggle("p#pMsg", "show", "hidden");
    });
    $("#eclDiv").hover(
    function () {
        $(this).animate({ "opacity": "1" }, 350);
        // $("div#description").animate({ "opacity": "1" }, 350);
        toggle("p#pECL", "show", "hidden");
        toggle("p#pMsg", "hidden", "show");
    },
    function () {
        $(this).animate({ "opacity": "0.3" }, 350);
        //  $("div#description").animate({ "opacity": "0" }, 350);
        toggle("p#pECL", "hidden", "show");
        toggle("p#pMsg", "show", "hidden");
    });
    $("#content").slideUp(0);
    $("#historyContent").slideUp(0);
    $("#eclContent").slideUp(0);
    $("#showScreen").slideDown();
    $("#compareDiv").click(
        function () {
            //$("#showScreen").css("visibility:hidden", "display:none");
            //$("#content").css("opacity","1")
            //$("#content").css("display","block");
            //toggle("div#showScreen", "hide", "show");

            //$("div#showScreen").animate({ "opacity": "0", "display": "none" }, 250);
            //$("div#content").animate({ "opacity": "1", "display": "inline" }, 250);
            $("#showScreen").slideUp(1000);

            toggle("div#content", "show", "hide");
            $("#content").slideDown(1000);

        });
    $("#historyDiv").click(
       function () {
           //$("#showScreen").css("visibility:hidden", "display:none");
           //$("#content").css("opacity","1")
           //$("#content").css("display","block");
           //toggle("div#showScreen", "hide", "show");

           //$("div#showScreen").animate({ "opacity": "0", "display": "none" }, 250);
           //$("div#content").animate({ "opacity": "1", "display": "inline" }, 250);
           $("#showScreen").slideUp(1000);

           toggle("div#historyContent", "show", "hide");
           $("#historyContent").slideDown(1000);


       });
    $("#eclDiv").click(
      function () {
          $("#showScreen").slideUp(1000);

          toggle("div#eclContent", "show", "hide");
          $("#eclContent").slideDown(1000);


      });

    //ECL javascripts
    $("#eclVersionID").selectbox();
    $("#eclModeID").selectbox();
    $("#eclModelID").selectbox({
        onChange: function (val, inst) {
            if (val == '') {

                // $(".disableControls").css("opacity", 0.5);
                $("#eclVersionID").selectbox('change', 'Version', 'Version');
                $("#eclModeID").selectbox('change', 'Mode', 'Mode');

            }

            else {
                //$(".disableControls").css("opacity", 1);

                var allLists, versionList, modeList, envList, restrictionList, customerList;
                $.ajax({
                    type: "POST",
                    url: "ScoringForm.aspx/GetDefaultValues",
                    data: "{'model':'Risk View'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (msg) {
                        allLists = eval(msg.d);
                    }

                });
                var timeToWait = 500;
                setTimeout(function () {
                    //alert(allLists);
                    versionList = allLists['Version'];

                    $("#eclVersionID").selectbox("detach");
                    $("#eclVersionID")
                        .find('option')
                        .remove()
                        .end()
                    $("#eclVersionID").selectbox("attach");
                    //alert(key + ":" + value);


                    $.each(versionList, function (key, value) {
                        $("#eclVersionID").selectbox("detach");
                        if (value == "v4") {
                            $("#eclVersionID")
                            .append($("<option selected=" + '"selected"' + "></option>")
                            .attr("value", key)
                            .text(value));
                            $("#eclVersionID").selectbox("attach");
                        }
                        else {
                            $("#eclVersionID")
                                .append($("<option></option>")
                                .attr("value", key)
                                .text(value));
                            $("#eclVersionID").selectbox("attach");
                        }
                    });

                    modeList = allLists['Mode'];

                    $("#eclModeID").selectbox("detach");
                    $("#eclModeID")
                        .find('option')
                        .remove()
                        .end()
                    $("#eclModeID").selectbox("attach");
                    //alert(key + ":" + value);

                    $.each(modeList, function (key, value) {
                        if (value == "XML") {
                            $("#eclModeID").selectbox("detach");
                            $("#eclModeID")
                                .append($("<option selected=" + '"selected"' + "></option>")
                                .attr("value", key)
                                .text(value));
                            $("#eclModeID").selectbox("attach");
                        }
                        else {
                            $("#eclModeID").selectbox("detach");
                            $("#eclModeID")
                                .append($("<option></option>")
                                .attr("value", key)
                                .text(value));
                            $("#eclModeID").selectbox("attach");
                        }
                    });

           
                    if (val == 'Lead Integrity') {

                        $("#eclVersionID").selectbox('change', 'v4', 'v4');
                        $("#eclModeID").selectbox('change', 'XML', 'XML');
                       
                    }

                    $(".disableControls").animate({ "opacity": "1" }, 2000);
                    $(".disableControls").css("pointer-events", "all");

                }, timeToWait);

            }
        }
    });
    $("#btnGatherData").button();
    $("#btnECLHome").button();
    $("#btnECLReset").button();
    //History Compare javascripts
    //$("#modelFilter").selectbox();
    //$("#versionFilter").selectbox();
    //$("#modeFilter").selectbox();
    //$("#envFilter").selectbox();
    //$("#restrictionFilter").selectbox();
    //$("#customerFilter").selectbox();
    //$("#ddlModel").selectbox();
    $("#btnHistoryCompare").button();
    $("#btnHistoryHome").button();
    $("#btnHistoryReset").button();

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
        var r = '<select id="prev' + colName + '"><option value="" selected style="width:25px">' + colName + '</option>', i, iLen = aData.length;

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
                jsonStr[count].Environment, jsonStr[count].Restriction, jsonStr[count].Customer, jsonStr[count].Owner];

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
        $("#historyTable tbody").click(function (event) {

            $(oTable.fnSettings().aoData).each(function () {
                $(this.nTr).removeClass('row_selected');
            });
            $(event.target.parentNode).addClass('row_selected');
            var gallery = $(event.target.parentNode);
            var contents = gallery.find("td").contents();
            if (!(typeof contents[0] === 'undefined'))
                $("#hifCompareIDSerial").val(contents[0].data);
        });

        $("#historyTable tbody tr").live('click', function (event) {
            var aPos = oTable.fnGetPosition(this);
            var aData = oTable.fnGetData(aPos);
            gIDNumber = aData[0];


        });
    }

    var currentUser;
    function getCurrentUser() {
        $.ajax({
            type: "POST",
            url: "ScoringForm.aspx/GetCurrentUser",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                currentUser = msg.d;
            }
        });
    }

    function deleteCompareID(tableID) {
        $.ajax({
            type: "POST",
            url: "ScoringForm.aspx/DeleteCompareID",
            data: "{'serialID':'"+ $("#hifCompareIDSerial").val()+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                
            }
        });
    }

    function InitOverviewDataTable() {
        currentUser = getCurrentUser();
        $.ajax({
            type: "POST",
            url: "ScoringForm.aspx/getHTMLCompareTable",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                tableStr = msg.d;
                if (typeof oTable == 'undefined') {
                    oTable = $("#historyTable").dataTable({
                        "aaData": getTable(msg.d),
                        "bAutoWidth": false,
                        "bfilter":false,
                        "aoColumns": [
{ "sTitle": "", "sWidth": "10px", "sClass": "center" },
{ "sTitle": "Model", "sClass": "center" },
{ "sTitle": "Dates", "sClass": "center" },
{ "sTitle": "Version", "sClass": "center" },
{ "sTitle": "Mode", "sClass": "center" },
{ "sTitle": "Env.", "sClass": "center" },
{ "sTitle": "Restr.", "sClass": "center" },
{ "sTitle": "Cust.", "sClass": "center" },
                        {"sTitle": "Delete", "sName": "Serial", "bSearchable": false, "sClass": "center",
                            "bSortable": false,
                            "fnRender": function (oObj) {
                                if (oObj.aData[8] == currentUser) {
                                    //return "<a class='table-action-deletelink' href='DeleteData.php?test=test&id=" + oObj.aData[0] + "'>Delete</a>";
                                    return "<a class='table-action-deletelink' href='ScoringForm.aspx?user="+currentUser+"&id=" + oObj.aData[0] + "'>Delete</a>";
                                    //return "<input type='button' value='Delete' onclick='deleteCompareID("+oObj.aData[0]+");'>";
                                }
                                else {
                                    return "N/A";
                                }
                            }                        }
                        ],
                        "bPaginate": true,
                        "sPaginationType": 'full_numbers',
                        "fnInitComplete": function (oSettings, json) {
                            /* Add a select menu for each TH element in the table footer */
                            $("#historyTable thead tr th").each(function (i) {
                                oTable = $("#historyTable").dataTable();
                                if (i != 2 && i != 0 && i!=8) {
                                    this.innerHTML = fnCreateSelect(oTable.fnGetColumnData(i), this.textContent);
                                    $('select', this).change(function () {
                                        oTable.fnFilter($(this).val(), i);
                                    });

                                }
                            });
                        }
                    })
                }
                else {
                    oTable.fnClearTable(0);
                    oTable.fnDraw();
                }
            }

        });
    }


    $("#btnHistoryCompare").click(
        function () {
            $("div#historyContent").animate({ "opacity": "0.6" }, 2000);
            $("div#historyContent").css("disabled", "disabled");
            $("#divLoading").css("visibility", "visible");
            $("#divLoading").css("display", "block");

            if ($("#divLoading:first").is(":hidden")) {
                $("#divLoading").slideDown("slow");
                $("#imgLoading").slideDown(1000);
            }

            setInterval(function () {
                $.ajax({
                    type: "POST",
                    url: "ScoringForm.aspx/GetText",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (msg) {
                        $("#lblUpdate").text(msg.d);
                        if (msg.d == "Results populated in the database. Please refresh the specific excel to see the results.") {
                            $("#imgLoading").slideUp(1000);
                        }
                    }

                })
            }, 50);
        });

    //Compare Now javascripts

    $("#btnSubmit").button();
    $("#btnHome").button();
    $("#btnReset").button();
    $("#ddlCompareIDs").selectbox();
    var compareIDsb = $("#ddlCompareIDs").attr('sb');
    $("#sbSelector_" + compareIDsb).removeClass("sbSelector")
    $("#sbSelector_" + compareIDsb).addClass("sbSelector1")

    $("#version_id").selectbox();
    $("#mode_id").selectbox();
    //    onChange: function (val, inst) {
    //        if (val == '') {
    //            $(".disableCalendar").animate({ "opacity": "0.5" }, 1000);
    //            $(".disableCalendar").css("pointer-events", "none");
    //        }
    //        $(".disableCalendar").animate({ "opacity": "1" }, 2000);
    //        $(".disableCalendar").css("pointer-events", "all");
    //        return true;
    //    }
    //});
    $("#env_id").selectbox();
    $("#restriction_id").selectbox();
    $("#customer_id").selectbox();

    $("#model_id").selectbox({
        onChange: function (val, inst) {
            if (val == '') {

                // $(".disableControls").css("opacity", 0.5);
                //$("#version_id").selectbox('change', 'Version', 'Version');
                //$("#mode_id").selectbox('change', 'Mode', 'Mode');
                $("#env_id").selectbox('change', 'Environment', 'Environment');
                $("#restriction_id").selectbox('change', 'Restriction', 'Restriction');
                $("#customer_id").selectbox('change', 'Customer', 'Customer');
                $(".disableControls").animate({ "opacity": "0.5" }, 1000);
                $(".disableControls").css("pointer-events", "none");

            }

            else {
                //$(".disableControls").css("opacity", 1);
                
                var allLists, versionList, modeList, envList, restrictionList, customerList;
                $.ajax({
                    type: "POST",
                    url: "ScoringForm.aspx/GetDefaultValues",
                    data: "{'model':'Risk View'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (msg) {
                        allLists = eval(msg.d);
                    }

                });
                var timeToWait = 500;
                setTimeout(function () {
                    //alert(allLists);
                    versionList = allLists['Version'];

                    $("#version_id").selectbox("detach");
                    $("#version_id")
                        .find('option')
                        .remove()
                        .end()
                    $("#version_id").selectbox("attach");
                    //alert(key + ":" + value);


                    $.each(versionList, function (key, value) {
                        $("#version_id").selectbox("detach");
                        if (value == "v4") {
                            $("#version_id")
                            .append($("<option selected=" + '"selected"' + "></option>")
                            .attr("value", key)
                            .text(value));
                            $("#version_id").selectbox("attach");
                        }
                        else {
                            $("#version_id")
                                .append($("<option></option>")
                                .attr("value", key)
                                .text(value));
                            $("#version_id").selectbox("attach");
                        }
                    });

                    //modeList = allLists['Mode'];

                    //$("#mode_id").selectbox("detach");
                    //$("#mode_id")
                    //    .find('option')
                    //    .remove()
                    //    .end()
                    //$("#mode_id").selectbox("attach");
                    ////alert(key + ":" + value);

                    //$.each(modeList, function (key, value) {
                    //    //if (value == "XML") {
                    //    //    $("#mode_id").selectbox("detach");
                    //    //    $("#mode_id")
                    //    //        .append($("<option selected=" + '"selected"' + "></option>")
                    //    //        .attr("value", key)
                    //    //        .text(value));
                    //    //    $("#mode_id").selectbox("attach");
                    //    //}
                    //    //else {
                    //        $("#mode_id").selectbox("detach");
                    //        $("#mode_id")
                    //            .append($("<option></option>")
                    //            .attr("value", key)
                    //            .text(value));
                    //        $("#mode_id").selectbox("attach");
                    //   // }
                    //});

                    envList = allLists['Environment'];

                    $("#env_id").selectbox("detach");
                    $("#env_id")
                        .find('option')
                        .remove()
                        .end()
                    $("#env_id").selectbox("attach");
                    //alert(key + ":" + value);


                    $.each(envList, function (key, value) {
                        if (value == "Cert") {
                            $("#env_id").selectbox("detach");
                            $("#env_id")
                                .append($("<option selected=" + '"selected"' + "></option>")
                                .attr("value", key)
                                .text(value));
                            $("#env_id").selectbox("attach");
                        }
                        else {
                            $("#env_id").selectbox("detach");
                            $("#env_id")
                                .append($("<option></option>")
                                .attr("value", key)
                                .text(value));
                            $("#env_id").selectbox("attach");
                        }
                    });

                    customerList = allLists['Customer'];

                    $("#customer_id").selectbox("detach");
                    $("#customer_id")
                        .find('option')
                        .remove()
                        .end()
                    $("#customer_id").selectbox("attach");
                    //alert(key + ":" + value);


                    $.each(customerList, function (key, value) {
                        if (value == "Flagship") {
                            $("#customer_id").selectbox("detach");
                            $("#customer_id")
                                .append($("<option selected=" + '"selected"' + "></option>")
                                .attr("value", key)
                                .text(value));
                            $("#customer_id").selectbox("attach");
                        }
                        else {
                            $("#customer_id").selectbox("detach");
                            $("#customer_id")
                                .append($("<option></option>")
                                .attr("value", key)
                                .text(value));
                            $("#customer_id").selectbox("attach");
                        }
                    });

                    restrictionList = allLists['Restriction'];

                    $("#restriction_id").selectbox("detach");
                    $("#restriction_id")
                        .find('option')
                        .remove()
                        .end()
                    $("#restriction_id").selectbox("attach");
                    //alert(key + ":" + value);


                    $.each(restrictionList, function (key, value) {
                        if (value == "FCRA") {
                            $("#restriction_id").selectbox("detach");
                            $("#restriction_id")
                                .append($("<option selected=" + '"selected"' + "></option>")
                                .attr("value", key)
                                .text(value));
                            $("#restriction_id").selectbox("attach");
                        }
                        else {
                            $("#restriction_id").selectbox("detach");
                            $("#restriction_id")
                                .append($("<option></option>")
                                .attr("value", key)
                                .text(value));
                            $("#restriction_id").selectbox("attach");
                        }
                    });

                    //if (val == 'Risk View') {
                    //    //$("#version_id").selectbox('detach');
                    //    //$("#version_id").val('4.0');
                    //    //$("#version_id").selectbox('attach');
                    //    //$("#version_id").selectbox('change', '4', '4');
                    //   // $("#version_id").find("option[value='4']").attr("selected", TRUE);
                    //    $("#mode_id").selectbox('change', 'XML', 'XML');
                    //    $("#env_id").selectbox('change', 'Cert', 'Cert');
                    //    $("#restriction_id").selectbox('change', 'FCRA', 'FCRA');
                    //    $("#customer_id").selectbox('change', 'Generic', 'Generic');
                    //}
                    //else
                    if (val == 'Lead Integrity') {

                        $("#version_id").selectbox('change', '4', '4');
                        $("#mode_id").selectbox('change', 'XML', 'XML');
                        $("#env_id").selectbox('change', 'Cert', 'Cert');
                        $("#restriction_id").selectbox('change', 'Non-FCRA', 'Non-FCRA');
                        $("#customer_id").selectbox('change', 'Generic', 'Generic');
                    }

                    $(".disableControls").animate({ "opacity": "1" }, 2000);
                    $(".disableControls").css("pointer-events", "all");

                    $("#mode_id").focus();

                }, timeToWait);
              
            }
        }
    });

 

    $("#btnSubmit").click(function () {

        var modelVal = $("#model_id option:selected").val();
        var versionVal = $("#version_id option:selected").html();
        var modeVal = $("#mode_id option:selected").html();
        var envVal = $("#env_id option:selected").html();
        var restrictionVal = $("#restriction_id option:selected").html();
        var customerVal = $("#customer_id option:selected").html();

        $("div#content").animate({ "opacity": "0.6" }, 2000);
        $("div#content").css("disabled", "disabled");
        $("#divLoading").css("visibility", "visible");
         
        $("#btnGetSummaryReport").prop("disabled", true);
        $("#hifModel").val(modelVal);
        $("#hifVersion").val(versionVal);
        $("#hifMode").val(modeVal);
        $("#hifEnv").val(envVal);
        $("#hifRestriction").val(restrictionVal);
        $("#hifCustomer").val(customerVal);

        if ($("#divLoading:first").is(":hidden")) {
            $("#divLoading").slideDown("slow");
            $("#imgLoading").slideDown(1000);
        }
      
        var itm = jQuery("#divProgress");
        

        var summFlag = 0;
        var detFlag = 0;
        var intervalID = setInterval(function () {
            $.ajax({
                type: "POST",
                url: "ScoringForm.aspx/GetText",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (msg) {
                    $("#lblUpdate").text(msg.d);
                    if (msg.d == "Results populated in the database. Please refresh the specific excel to see the results.") {
                        $("#imgLoading").slideUp(1000);
                        $("#content").slideUp(1000);
                        $("#historyContent").slideUp(1000);
                        $("#eclContent").slideUp(1000);
                        $("#showScreen").slideDown(1000);
                        //clearInterval(intervalID);
                    }
                }

            })
        }, 50);

    });

    $(".homeButton").click(function () {
        //toggle("div#showScreen", "show", "hide");
        //toggle("div#content", "hide", "show");
        $("#content").slideUp(1000);
        $("#historyContent").slideUp(1000);
        $("#eclContent").slideUp(1000);
        $("#divLoading").slideUp(1000);
        $("#showScreen").slideDown(1000);
        setInterval(function(){
            location.reload(true)},1000);
    });

    $("#btnReset").click(function () {
           

                // $(".disableControls").css("opacity", 0.5);
        //$("#model_id").selectbox('change', 'Model', 'Model');
       
                $("#version_id").selectbox('change', 'Version', 'Version');
                $("#mode_id").selectbox('change', 'Mode', 'Mode');
                $("#env_id").selectbox('change', 'Environment', 'Environment');
                $("#restriction_id").selectbox('change', 'Restriction', 'Restriction');
                $("#customer_id").selectbox('change', 'Customer', 'Customer');
                $(".disableControls").animate({ "opacity": "0.5" }, 1000);
                $(".disableControls").css("pointer-events", "none");

                $("#model_id").selectbox("detach");
                $("#model_id").selectbox("attach");
                $("#model_id").selectbox({
                    onChange: function (val, inst) {
                        if (val == '') {

                            // $(".disableControls").css("opacity", 0.5);
                            $("#version_id").selectbox('change', 'Version', 'Version');
                            $("#mode_id").selectbox('change', 'Mode', 'Mode');
                            $("#env_id").selectbox('change', 'Environment', 'Environment');
                            $("#restriction_id").selectbox('change', 'Restriction', 'Restriction');
                            $("#customer_id").selectbox('change', 'Customer', 'Customer');
                            $(".disableControls").animate({ "opacity": "0.5" }, 1000);
                            $(".disableControls").css("pointer-events", "none");

                        }

                        else {
                            //$(".disableControls").css("opacity", 1);

                            var allLists, versionList, modeList, envList, restrictionList, customerList;
                            $.ajax({
                                type: "POST",
                                url: "ScoringForm.aspx/GetDefaultValues",
                                data: "{'model':'Risk View'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                success: function (msg) {
                                    allLists = eval(msg.d);
                                }

                            });
                            var timeToWait = 500;
                            setTimeout(function () {
                                //alert(allLists);
                                versionList = allLists['Version'];

                                $("#version_id").selectbox("detach");
                                $("#version_id")
                                    .find('option')
                                    .remove()
                                    .end()
                                $("#version_id").selectbox("attach");
                                //alert(key + ":" + value);


                                $.each(versionList, function (key, value) {
                                    $("#version_id").selectbox("detach");
                                    if (value == "v4") {
                                        $("#version_id")
                                        .append($("<option selected=" + '"selected"' + "></option>")
                                        .attr("value", key)
                                        .text(value));
                                        $("#version_id").selectbox("attach");
                                    }
                                    else {
                                        $("#version_id")
                                            .append($("<option></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#version_id").selectbox("attach");
                                    }
                                });

                                modeList = allLists['Mode'];

                                $("#mode_id").selectbox("detach");
                                $("#mode_id")
                                    .find('option')
                                    .remove()
                                    .end()
                                $("#mode_id").selectbox("attach");
                                //alert(key + ":" + value);

                                $.each(modeList, function (key, value) {
                                    if (value == "XML") {
                                        $("#mode_id").selectbox("detach");
                                        $("#mode_id")
                                            .append($("<option selected=" + '"selected"' + "></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#mode_id").selectbox("attach");
                                    }
                                    else {
                                        $("#mode_id").selectbox("detach");
                                        $("#mode_id")
                                            .append($("<option></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#mode_id").selectbox("attach");
                                    }
                                });

                                envList = allLists['Environment'];

                                $("#env_id").selectbox("detach");
                                $("#env_id")
                                    .find('option')
                                    .remove()
                                    .end()
                                $("#env_id").selectbox("attach");
                                //alert(key + ":" + value);


                                $.each(envList, function (key, value) {
                                    if (value == "Cert") {
                                        $("#env_id").selectbox("detach");
                                        $("#env_id")
                                            .append($("<option selected=" + '"selected"' + "></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#env_id").selectbox("attach");
                                    }
                                    else {
                                        $("#env_id").selectbox("detach");
                                        $("#env_id")
                                            .append($("<option></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#env_id").selectbox("attach");
                                    }
                                });

                                customerList = allLists['Customer'];

                                $("#customer_id").selectbox("detach");
                                $("#customer_id")
                                    .find('option')
                                    .remove()
                                    .end()
                                $("#customer_id").selectbox("attach");
                                //alert(key + ":" + value);


                                $.each(customerList, function (key, value) {
                                    if (value == "Flagship") {
                                        $("#customer_id").selectbox("detach");
                                        $("#customer_id")
                                            .append($("<option selected=" + '"selected"' + "></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#customer_id").selectbox("attach");
                                    }
                                    else {
                                        $("#customer_id").selectbox("detach");
                                        $("#customer_id")
                                            .append($("<option></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#customer_id").selectbox("attach");
                                    }
                                });

                                restrictionList = allLists['Restriction'];

                                $("#restriction_id").selectbox("detach");
                                $("#restriction_id")
                                    .find('option')
                                    .remove()
                                    .end()
                                $("#restriction_id").selectbox("attach");
                                //alert(key + ":" + value);


                                $.each(restrictionList, function (key, value) {
                                    if (value == "FCRA") {
                                        $("#restriction_id").selectbox("detach");
                                        $("#restriction_id")
                                            .append($("<option selected=" + '"selected"' + "></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#restriction_id").selectbox("attach");
                                    }
                                    else {
                                        $("#restriction_id").selectbox("detach");
                                        $("#restriction_id")
                                            .append($("<option></option>")
                                            .attr("value", key)
                                            .text(value));
                                        $("#restriction_id").selectbox("attach");
                                    }
                                });

                                //if (val == 'Risk View') {
                                //    //$("#version_id").selectbox('detach');
                                //    //$("#version_id").val('4.0');
                                //    //$("#version_id").selectbox('attach');
                                //    //$("#version_id").selectbox('change', '4', '4');
                                //   // $("#version_id").find("option[value='4']").attr("selected", TRUE);
                                //    $("#mode_id").selectbox('change', 'XML', 'XML');
                                //    $("#env_id").selectbox('change', 'Cert', 'Cert');
                                //    $("#restriction_id").selectbox('change', 'FCRA', 'FCRA');
                                //    $("#customer_id").selectbox('change', 'Generic', 'Generic');
                                //}
                                //else
                                if (val == 'Lead Integrity') {

                                    $("#version_id").selectbox('change', '4', '4');
                                    $("#mode_id").selectbox('change', 'XML', 'XML');
                                    $("#env_id").selectbox('change', 'Cert', 'Cert');
                                    $("#restriction_id").selectbox('change', 'Non-FCRA', 'Non-FCRA');
                                    $("#customer_id").selectbox('change', 'Generic', 'Generic');
                                }

                                $(".disableControls").animate({ "opacity": "1" }, 2000);
                                $(".disableControls").css("pointer-events", "all");

                            }, timeToWait);

                        }
                    }
                });


    });

    $("#ddlPrevTime").change(function () {
        $("#ddlPrevTime").selectbox();
    });

    
    function getParameterByName(name) {
        // ***this goes on the global scope
        // get querystring as an array split on "&"
        
        var querystring = location.search.replace('?', '').split('&');
        
        // declare object
        var queryObj = {};
        // loop through each name-value pair and populate object
        for (var i = 0; i < querystring.length; i++) {
            // get name and value
            var name1 = querystring[i].split('=')[0];
            var value1 = querystring[i].split('=')[1];
            // populate object
            queryObj[name1] = value1;
        }
        return queryObj[name];
    }

    var isPostBackObject = document.getElementById('isPostBack');
    if (isPostBackObject != null) {
        if ($("#divLoading:first").is(":hidden")) {
            $("#divLoading").slideDown("slow");
           // $("#divLoading").toggle("divLoadingShow", "divLoadingHidden");
            $("#divLoading").removeClass("divLoadingHidden");
            $("#divLoading").addClass("divLoadingShow");
        }
    }
    else {
        var compareID = getParameterByName("id");
        
        if (compareID != null)
        {
            if ($("#divLoading:first").is(":hidden")) {
               
                $("#divLoading").removeClass("divLoadingHidden");
                $("#divLoading").addClass("divLoadingShow");
                $("#imgLoading").slideUp(1000);
                $("#lblUpdate").text("Results populated in the database. Please refresh the specific excel to see the results.");
                //$("#divLoading").slideDown("slow");
                
            }
        }
    }

    
        initClickEvents();
        InitOverviewDataTable();
        //setTimeout(function () { AutoReload(); }, 30000);   

    


});
