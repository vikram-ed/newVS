<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoringForm.aspx.cs" Inherits="ScoringApp1.ScoringForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery.selectbox-0.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.progressbar.js" type="text/javascript"></script>
    <script src="Scripts/glDatePicker.js" type="text/javascript"></script>
    <script src="Scripts/progressbutton.jquery.js"></script>


    <link href="CSS/jquery.ui.progressbar.css" rel="stylesheet" />
    <link href="CSS/jquery.selectbox.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="CSS/glDatePicker.darkneon.css" rel="stylesheet" type="text/css" />
    <link href="CSS/ScoringFormStyle.css" rel="stylesheet" />

    <script type="text/javascript">

        $(function () {

            $("#btnSubmit").button();
            //$("#progressbar").progressbar({ value: false });

            //$("#txtPrevDate").glDatePicker({
            //    showAlways: true,
            //    cssName: 'darkneon'
            //});
            //$("#cdrCurrentDate").glDatePicker();

            $("#model_id").selectbox({
                onChange: function (val, inst) {
                    //$.ajax({
                    //    type: "GET",
                    //    data: { selModel: val },
                    //    url: 

                    //});
                    alert(val);
                }
            });
            $("select").selectbox();
            $("#btnSubmit").click(function () {
                var modelVal = $("#model_id option:selected").val();
                var versionVal = $("#version_id option:selected").val();
                var modeVal = $("#mode_id option:selected").val();
                var envVal = $("#env_id option:selected").val();
                var restrictionVal = $("#restriction_id option:selected").val();
                var customerVal = $("#customer_id option:selected").val();

                $("#<%=hifModel.ClientID%>").val(modelVal);
                $("#<%=hifVersion.ClientID%>").val(versionVal);
                $("#<%=hifMode.ClientID%>").val(modeVal);
                $("#<%=hifEnv.ClientID%>").val(envVal);
                $("#<%=hifRestriction.ClientID%>").val(restrictionVal);
                $("#<%=hifCustomer.ClientID%>").val(customerVal);


                var itm = jQuery("#divProgress");
                var par = jQuery(this);
                par.attr("disabled", "disabled");
                var bar = itm.progressbutton({
                    percent: -1,
                    fade: true
                });
                var clock = 0;
                var interval = setInterval(function () {
                    bar.progressbutton({
                        percent: clock
                    });
                    clock = clock + 0.5;
                    if (clock > 100) {
                        par.removeAttr("disabled");
                        clearInterval(interval);
                        //bar.progressbutton("reset");
                    }
                }, 30);


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


                return false;
                //alert(radioSelected);
            });

            function updateProgress() {
                var value = $("#progressbar").progressbar("option", "value");
                if (value < 100) {
                    $("#progressbar").progressbar("value", value + 0.25);
                }
            }

            $("#ddlPrevTime").change(function () {
                $("#ddlPrevTime").selectbox();
            });

        });

    </script>


</head>
<body>
    <form id="form1" runat="server" style="margin: 10% auto; width: 70%;">
        <asp:ScriptManager runat="server" ID="smUpdateDates" EnablePartialRendering="true"></asp:ScriptManager>
        <div id="lnHeader" style="text-align: left; width: 100%; border-radius: 10px 0px 10px 0px">
            <img src="img/lexislogo.gif" alt="LexisNexis" style="text-align: left" />
            <span class="headingTag">Scoring Regression</span>
        </div>
        <div id="divProgress" data-opacity="0.8"></div>
        <div id="content" style="width: 100%; background-color: #e6e6e6;">
            <table>
                <tr>
                    <td style="width: 33%; padding: 10px 10px 10px 10px;">
                        <div class="disableControls1">
                            <asp:Label ID="lblPrevDate" runat="server" Text="Previous Date"></asp:Label>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Calendar BorderWidth="1px" DayNameFormat="FirstTwoLetters"
                                        Font-Names="Meiryo UI" Font-Size="16px" CssClass="calendar1" ID="cdrPrevDate" Visible="true"
                                        runat="server" CellSpacing="1" ForeColor="#172013" OnSelectionChanged="cdrPrevDate_SelectionChanged"
                                        Width="250px" OnDayRender="cdrPrevDate_DayRender" OnVisibleMonthChanged="cdrPrevDate_VisibleMonthChanged">
                                        <SelectedDayStyle BackColor="#FF6666" />
                                        <SelectorStyle BackColor="#FF6666" ForeColor="White" />
                                        <DayHeaderStyle BackColor="#d8c1c0" ForeColor="#CC0033" Font-Bold="false" Font-Size="Smaller" />
                                        <DayStyle BackColor="#ffffff" BorderColor="#FFFFCC" BorderWidth="1px" Font-Italic="False"
                                            Font-Names="Meiryo UI" Font-Size="Smaller" HorizontalAlign="Justify" Wrap="False" />
                                        <WeekendDayStyle BackColor="#cccccc" ForeColor="#999999" />
                                    </asp:Calendar>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cdrPrevDate" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>

                    </td>
                    <%--showing all the selection parameter dropdowns--%>
                    <td style="width: 34%; padding: 10px 10px 10px 10px;">
                        <div style="width: 100%; margin-top: 6%">
                            <select name="selModel" id="model_id" class="modelClass" tabindex="1">
                                <option value="">Model</option>
                                <option value="RiskView">Risk View</option>
                                <option value="FraudPoint">Fraud Point</option>
                                <option value="LeadIntegrity">Lead Integrity</option>
                            </select>
                            <select name="selVersion" id="version_id" tabindex="2">
                                <option value="">Version</option>
                                <option value="4.0">4.0</option>
                                <option value="3.0">3.0</option>
                            </select>
                            <select name="selMode" id="mode_id" tabindex="3">
                                <option value="">Mode</option>
                                <option value="XML">XML</option>
                                <option value="Batch">Batch</option>
                            </select>
                            <select name="selEnv" id="env_id" tabindex="4">
                                <option value="">Environment</option>
                                <option value="Cert">Cert</option>
                                <option value="Prod">Prod</option>
                            </select>
                            <select name="selRestriction" id="restriction_id" tabindex="5">
                                <option value="">Restriction</option>
                                <option value="FCRA">FCRA</option>
                                <option value="Non-FCRA">Non-FCRA</option>
                            </select>
                            <select name="selCustomer" id="customer_id" tabindex="6">
                                <option value="">Customer</option>
                                <option value="Generic">Generic</option>
                            </select>
                        </div>
                    </td>
                    <td style="width: 33%; padding: 10px 10px 10px 10px;">
                        <asp:Label ID="lblCurrentDate" runat="server" Text="Current Date"></asp:Label>
                        <asp:UpdatePanel ID="upCdrPrevDate" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Calendar BorderWidth="1px" DayNameFormat="FirstTwoLetters"
                                    Font-Names="Meiryo UI" Font-Size="16px" CssClass="calendar1" ID="cdrCurrentDate" Visible="true"
                                    runat="server" CellSpacing="1" ForeColor="#172013" OnSelectionChanged="cdrCurrentDate_SelectionChanged" Width="250px" OnDayRender="cdrCurrentDate_DayRender" OnVisibleMonthChanged="cdrCurrentDate_VisibleMonthChanged">
                                    <SelectedDayStyle BackColor="#FF6666" />
                                    <SelectorStyle BackColor="#FF6666" ForeColor="White" />
                                    <DayHeaderStyle BackColor="#d8c1c0" ForeColor="#CC0033" Font-Bold="false" Font-Size="Smaller" />
                                    <DayStyle BackColor="#ffffff" BorderColor="#FFFFCC" BorderWidth="1px" Font-Italic="False"
                                        Font-Names="Meiryo UI" Font-Size="Smaller" HorizontalAlign="Justify" Wrap="False" />
                                    <WeekendDayStyle BackColor="#cccccc" ForeColor="#999999" />
                                </asp:Calendar>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cdrCurrentDate" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%; padding: 10px 10px 10px 10px;">
                        <asp:UpdatePanel ID="getPrevTimes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPrevTime" runat="server"
                                    AppendDataBoundItems="true" DataTextField="PreviousTime"
                                    DataValueField="prevTime">
                                    <asp:ListItem Enabled="false" Selected="True" Text="Previous Time" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cdrPrevDate" EventName="SelectionChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <script type="text/javascript" lang="javascript">
                            var prm = Sys.WebForms.PageRequestManager.getInstance();
                            prm.add_pageLoaded(PageLoadedEventHandler);
                            function PageLoadedEventHandler() {
                                $("#ddlPrevTime").selectbox();
                            }
                        </script>
                    </td>
                    <td style="width: 34%; padding: 10px 10px 10px 10px;">
                        <div id="progressbar">
                        </div>
                    </td>
                    <td style="width: 33%; padding: 10px 10px 10px 10px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCurrentTime" runat="server"
                                    AppendDataBoundItems="true" DataTextField="CurrentTime"
                                    DataValueField="currentTime">
                                    <asp:ListItem Enabled="false" Selected="True" Text="Current Time" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cdrCurrentDate" EventName="SelectionChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <script type="text/javascript" lang="javascript">
                            var prm = Sys.WebForms.PageRequestManager.getInstance();
                            prm.add_pageLoaded(PageLoadedEventHandler);
                            function PageLoadedEventHandler() {
                                $("#ddlCurrentTime").selectbox();
                            }
                        </script>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSubmit" Text="Compare" runat="server" OnClick="btnSubmit_Click" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="result" style="width: 100%; margin-top: 5%;">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--all the hiddenfields to pass the jquery values to the code behind--%>
        <div>
            <asp:HiddenField ID="hifModel" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hifVersion" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hifMode" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hifEnv" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hifRestriction" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hifCustomer" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hifPrevDate" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hifCurrentDate" runat="server"></asp:HiddenField>
        </div>
    </form>
</body>
</html>
