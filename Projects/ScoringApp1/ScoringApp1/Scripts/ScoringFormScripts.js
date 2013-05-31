
$(function () {

    $("#btnSubmit").button();
    $("#btnGather").button();
    $("#btnReset").button();
    //$("#progressbar").progressbar({ value: false });

    //$("#txtPrevDate").glDatePicker({
    //    showAlways: true,
    //    cssName: 'darkneon'
    //});
    //$("#cdrCurrentDate").glDatePicker();

    $("#version_id").selectbox();
    $("#mode_id").selectbox();
    $("#env_id").selectbox();
    $("#restriction_id").selectbox();
    $("#customer_id").selectbox();

    $("#model_id").selectbox({
        onChange: function (val, inst) {
            if (val != '') {
                //$(".disableControls").css("opacity", 1);
                $(".disableControls").animate({ "opacity": "1" }, 2000);
                $(".disableControls").css("pointer-events", "all");

            }
            else {
                // $(".disableControls").css("opacity", 0.5);
                $("#version_id").selectbox('change', 'Version', 'Version');
                $("#mode_id").selectbox('change', 'Mode', 'Mode');
                $("#env_id").selectbox('change', 'Environment', 'Environment');
                $("#restriction_id").selectbox('change', 'Restriction', 'Restriction');
                $("#customer_id").selectbox('change', 'Customer', 'Customer');
                $(".disableControls").animate({ "opacity": "0.5" }, 1000);
                $(".disableControls").css("pointer-events", "none");

            }

            if (val == 'RiskView') {
                //$("#version_id").selectbox('detach');
                //$("#version_id").val('4.0');
                //$("#version_id").selectbox('attach');
                $("#version_id").selectbox('change', '4.0', '4.0');
                $("#mode_id").selectbox('change', 'XML', 'XML');
                $("#env_id").selectbox('change', 'Cert', 'Cert');
                $("#restriction_id").selectbox('change', 'FCRA', 'FCRA');
                $("#customer_id").selectbox('change', 'Generic', 'Generic');
            }
            else if (val == 'LeadIntegrity') {

                $("#version_id").selectbox('change', '4.0', '4.0');
                $("#mode_id").selectbox('change', 'XML', 'XML');
                $("#env_id").selectbox('change', 'Cert', 'Cert');
                $("#restriction_id").selectbox('change', 'Non-FCRA', 'Non-FCRA');
                $("#customer_id").selectbox('change', 'Generic', 'Generic');
            }

            //$.ajax({
            //    type: "GET",
            //    data: { selModel: val },
            //    url: 

            //});
            //alert(val);
        }
    });
    //$("#imgLoading").css("display", "none");
    $("#divLoading").css("visibility", "hidden");
    $("#btnSubmit").click(function () {

        // $("div#content").html('<div id="overlay"><img src="img/loader-waiting.gif" class="loading_circle" alt="loading" /></div>');
        $("div#content").css("opacity", "0.2");
        $("#divLoading").css("visibility", "visible");
        //$("#imgLoading").css("display", "block");
        //$("#imgLoading").animate({ "display": "block" }, 2000);
        
        var modelVal = $("#model_id option:selected").val();
        var versionVal = $("#version_id option:selected").val();
        var modeVal = $("#mode_id option:selected").val();
        var envVal = $("#env_id option:selected").val();
        var restrictionVal = $("#restriction_id option:selected").val();
        var customerVal = $("#customer_id option:selected").val();
        
        $("#hifModel").val(modelVal);
        $("#hifVersion").val(modelVal);
        $("#hifMode").val(modelVal);
        $("#hifEnv").val(modelVal);
        $("#hifRestriction").val(modelVal);
        $("#hifCustomer").val(modelVal);

        //$("#<%=hifVersion.ClientID%>").val(versionVal);
        //$("#<%=hifMode.ClientID%>").val(modeVal);
        //$("#<%=hifEnv.ClientID%>").val(envVal);
        //$("#<%=hifRestriction.ClientID%>").val(restrictionVal);
        //$("#<%=hifCustomer.ClientID%>").val(customerVal);

        
        var itm = jQuery("#divProgress");
        //var par = jQuery(this);
        //par.attr("disabled", "disabled");
        //var bar = itm.progressbutton({
        //    percent: -1,
        //    fade: true
        //});
        //var clock = 0;
        //var interval = setInterval(function () {
        //    bar.progressbutton({
        //        percent: clock
        //    });
        //    clock = clock + 0.5;
        //    if (clock > 100) {
        //        par.removeAttr("disabled");
        //        clearInterval(interval);
        //        //bar.progressbutton("reset");
        //    }
        //}, 30);
        //Button click event
        //return false;
        //Disabling button
        //$(this).attr('disabled', 'disabled');
        ////Making sure that progress indicate 0
        ////$("#divProgress").progressbutton({ percent: -1 });
        //itm.progressbutton({ percent: -1 });
        ////Call PageMethod which triggers long running operation
        //PageMethods.Operation(function (result) {
        //    if (result) {
        //        //Updating progress
        //        itm.progressbutton({ percent: result.progress })
        //        //Setting the timer
        //        window.progressIntervalId = window.setInterval(function () {
        //            //Calling PageMethod for current progress
        //            PageMethods.OperationProgress(function (result) {
        //                //Updating progress
        //                itm.progressbutton({ percent: result.progress })
        //                //If operation is complete
        //                if (result.progress == 100) {
        //                    //Clear timer
        //                    window.clearInterval(window.progressIntervalId);
        //                    //Enable button
        //                    $(this).attr('disabled', '');
        //                }
        //            });
        //        }, 500);
        //    }
        //});
        
        //var intervalID = setInterval(updateProgress, 250);
        setInterval(function(){
            $.ajax({
                type: "POST",
                url: "ScoringForm.aspx/GetText",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (msg) {
                    $("#lblUpdate").text(msg.d);
                }

            })},250);

        
        //var intervalID = setInterval(updateProgress, 250);
        //$.ajax({
        //    type: "POST",
        //    url: "testPage.aspx/GetText",
        //    data: "{}",
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    async: true,
        //    success: function (msg) {
        //        $("#progressbar").progressbar("value", 100);
        //        $("#result").text(msg.d);
        //        clearInterval(intervalID);
        //    }
        
        //});



        //alert(radioSelected);
        //function updateProgress() {
        //    alert("1");
        //    var progress = '<%# Eval("progress") %>';
        //    alert(progress);
        //    $("lblUpdate").html(progress);
        //}
        //clearInterval(intervalID);
    });
   
    //function updateProgress() {
    //    var value = $("#progressbar").progressbar("option", "value");
    //    if (value < 100) {
    //        $("#progressbar").progressbar("value", value + 0.25);
    //    }
    //}

    $("#ddlPrevTime").change(function () {
        $("#ddlPrevTime").selectbox();
    });


});
