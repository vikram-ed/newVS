﻿
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
    $("#eclDataContent").slideUp(0);
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
   

    //History Compare javascripts
    $("#modelFilter").selectbox();
    $("#versionFilter").selectbox();
    $("#modeFilter").selectbox();
    $("#envFilter").selectbox();
    $("#restrictionFilter").selectbox();
    $("#customerFilter").selectbox();
    $("#ddlModel").selectbox();
    $("#btnHistoryCompare").button();
    $("#btnHistoryHome").button();
    $("#btnHistoryReset").button();

    $("#btnHistoryCompare").click(
        function () {
            $("div#content").animate({ "opacity": "0.6" }, 2000);
            $("#divLoading").css("visibility", "visible");

            if ($("#divLoading:first").is(":hidden")) {
                $("#divLoading").slideDown("slow");
            }

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
                        if (msg.d == "Excel Generated") {
                            $("#imgLoading").animate({ "opacity": "0" }, 2000);
                        }
                        else if (msg.d == "Summary Report Generated!") {
                            $.ajax({
                                type: "POST",
                                url: "ScoringForm.aspx/GetSummaryPath",
                                data: "{}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                success: function (msg) {
                                    $("#aSummaryLink").attr("href", msg.d);
                                }
                            });
                        } else if (msg.d == "Detailed Report Generated!") {
                            $.ajax({
                                type: "POST",
                                url: "ScoringForm.aspx/GetDetailedPath",
                                data: "{}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                success: function (msg) {    
                                    $("#aDetailedLink").attr("href", msg.d);
                                }
                            });
                            
                        }
                       
                    }

                })
            }, 5000);
        });

    //Compare Now javascripts

    $("#btnGetSummaryReport").button();
    $("#btnGetDetailedReport").button();
    $("#btnSubmit").button();
    $("#btnHome").button();
    $("#btnReset").button();
    $("#ddlCompareIDs").selectbox();
    var compareIDsb = $("#ddlCompareIDs").attr('sb');
    $("#sbSelector_" + compareIDsb).removeClass("sbSelector")
    $("#sbSelector_" + compareIDsb).addClass("sbSelector1")

    $("#version_id").selectbox();
    $("#mode_id").selectbox();
    $("#env_id").selectbox();
    $("#restriction_id").selectbox();
    $("#customer_id").selectbox();

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
                $(".disableControls").animate({ "opacity": "1" }, 2000);
                $(".disableControls").css("pointer-events", "all");
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
                var timeToWait = 200;
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
                        if (value == "4") {
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

                }, timeToWait);

                ////alert(versionList);
                //for (var i = 0; i < versionList.length; i++) {
                //    alert(versionList[i]);
                //}
                // $("#version_id").html(versionList);
                //modeList = eval(msg.d["Mode"]);
                //envList = eval(msg.d["Environment"]);
                //restrictionList = eval(msg.d["Restriction"]);
                //customerList = eval(msg.d["Customer"]);



                //getDictionaryValue: function(array, key) {
                //    var keyValue = key;
                //    var result;  
                //    jQuery.each(array, function() {  
                //        if (this.Key == keyValue) {  
                //            result = this.Value;  
                //            return false;  
                //        }  
                //    });  
                //    return result;

                //}

                //$.ajax({
                //    type: "GET",
                //    data: { selModel: val },
                //    url: 

                //});
                //alert(val);
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
                    if (msg.d == "Excel Generated") {
                        $("#imgLoading").slideUp(1000);
                        //clearInterval(intervalID);
                    }
                    else if (msg.d == "Summary Report Generated!" && summFlag == 0) {
                        $.ajax({
                            type: "POST",
                            url: "ScoringForm.aspx/GetSummaryPath",
                            data: "{}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            success: function (msg) {
                                //$("#aSummaryLink").toggle("resultButtonsShow", "resultButtonsHide");
                                $("#aSummaryLink").attr("href", msg.d);
                                $("#summ1").attr("src", "img/summaryExcel1.png");
                                summFlag = 1;
                            }
                        });
                       // $("#btnGetSummaryReport").prop("disabled", false);
                    } else if (msg.d == "Detailed Report Generated!" && detFlag == 0) {
                        $.ajax({
                            type: "POST",
                            url: "ScoringForm.aspx/GetDetailedPath",
                            data: "{}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            success: function (msg) {
                                //$("#aDetailedLink").toggle("resultButtonsShow", "resultButtonsHide");
                                $("#aDetailedLink").attr("href", msg.d);
                                $("#det1").attr("src", "img/detailedExcel1.png");
                                detFlag = 1;
                            }
                        });
                        // $("#btnGetSummaryReport").prop("disabled", false);
                    }
                
                }

            })
        }, 500);

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
            $("#divLoading").toggle("divLoadingShow", "divLoadingHidden");

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
                $("#lblUpdate").text("Excel Reports Generated");
                $("#summ1").attr("src", "img/summaryExcel1.png");
                $("#det1").attr("src", "img/detailedExcel1.png");
                $("#aSummaryLink").attr("href", "http://" + document.location.hostname + "/ScoringApp1/Archive/" + compareID + "-Summary.xlsx");
                $("#aDetailedLink").attr("href", "http://" + document.location.hostname + "/ScoringApp1/Archive/" + compareID + "-Detailed.xlsx");
                //$("#divLoading").slideDown("slow");
                
            }
        }
    }


});
