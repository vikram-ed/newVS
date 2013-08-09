<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoringForm.aspx.cs" Inherits="ScoringApp1.ScoringForm" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scoring Regression</title>


    <script src="Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.dataTables.js"></script>
    <script src="Scripts/jquery.selectbox-0.2.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.progressbar.js" type="text/javascript"></script>
    <script src="Scripts/glDatePicker.js" type="text/javascript"></script>
    <script src="Scripts/progressbutton.jquery.js"></script>
    <script type="text/javascript" src="Scripts/ScoringFormScripts.js"></script>


    <style type="text/css" title="currentStyle">
        @import "CSS/demo_page.css";
        @import "CSS/demo_table.css";
    </style>
    <link href="CSS/jquery.ui.progressbar.css" rel="stylesheet" />
    <link href="CSS/jquery.selectbox.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="CSS/glDatePicker.darkneon.css" rel="stylesheet" type="text/css" />
    <link href="CSS/ScoringFormStyle.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server" style="width: 100%;">
        <asp:ScriptManager runat="server" ID="smUpdateDates" EnablePartialRendering="true"></asp:ScriptManager>
        <asp:Label ID="lblError" Text="" runat="server"></asp:Label>
        <!--Header division markup -->
        <div id="lnHeader" style="text-align: left; width: 100%; border-radius: 10px 0px 10px 0px;">
            <img src="img/lexislogo.gif" alt="LexisNexis" style="text-align: left;" />
            <span class="headingTag">Scoring Regression</span>
            <span class="userNameTag">
                <asp:Label ID="lblUserName" runat="server" Style="text-align: right">
                </asp:Label>
            </span>
        </div>
        <!--Status division markup -->
        <div id="divLoading" data-opacity="1" class="divLoadingHidden">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div style="width: 100%">
                        <table style="width: 100%; background-color: transparent">
                            <tr>
                                <td>
                                    <div style="align-content: center">
                                        <img src="img/loader-waiting.gif" class="loading_circle" id="imgLoading" alt="loading" />
                                        <asp:Label ID="lblUpdate" runat="server" Text="Submitting values to database..." Style="display: block; color: #CC0033"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
                    <asp:AsyncPostBackTrigger ControlID="btnHistoryCompare" />
                    <asp:AsyncPostBackTrigger ControlID="btnGatherData" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!--The top screen displayed to the user - Home markup -->
        <div id="showScreen" style="width: 100%; background-color: #e6e6e6; margin-top: 5px;">
            <table>
                <tr>
                    <td colspan="3">
                        <p class="pHome hidden">
                            Scoring Regression tool can be used to get reports based on the daily comparison results, 
                        or the previously generated compare results. It also enables the user to generate data at the Thor level.
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style="height: 400px; width: 275px; margin-top: 5px">
                        <div id="eclDiv" class="partial">
                            <h2>Gather Data</h2>
                            <img src="img/ECLdiv.jpg" class="resize" />
                        </div>
                    </td>

                    <td style="height: 400px; width: 266px; margin-top: 5px">
                        <div id="compareDiv" class="partial">
                            <h2>Compare Now</h2>
                            <a href="#">
                                <img src="img/CompareNowDiv-small.png" class="resize" /></a>
                        </div>

                    </td>
                    <td style="height: 400px; width: 266px; margin-top: 5px">
                        <div id="historyDiv" class="partial">
                            <h2>Use Previous Comparisons</h2>
                            <img src="img/HistoryDiv1.png" class="resize" />
                        </div>
                    </td>

                </tr>
                <tr>
                    <td colspan="3">
                        <div id="description" class="full">
                            <p id="pHistory" class="hidden">"Previous Comparisons" enables you to generate excel report using the existing comparison results</p>
                            <p id="pCompareNow" class="hidden">"Compare Now" enables you to generate new comparison data based on available ECL results</p>
                            <p id="pECL" class="hidden">"Gather Data" enables you to generate new data at the Thor level</p>
                            <p id="pMsg" class="show">Hover over the images to enable the applications. Click on them to access them.</p>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <!--Gather data markup -->
        <div id="eclContent" style="width: 100%; background-color: #e6e6e6; margin-top: 5px;">
            <table>
                <tr style="height: 400px">
                    <td style="width: 34%; padding: 0px 50px 0px 50px;">
                        <div style="width: 100%; margin-top: 6%">
                            <select name="selModel" id="eclModelID" class="modelClass" tabindex="1">
                                <option value="" style="font-weight: bolder">Model</option>
                                <option value="Risk View">Risk View</option>
                                <option value="Fraud Point">Fraud Point</option>
                                <option value="Lead Integrity">Lead Integrity</option>
                            </select>
                            <div class="disableControls">
                                <select name="selVersion" id="eclVersionID" tabindex="2">
                                    <option value="">Version</option>
                                    <option value="4.0">4.0</option>
                                    <option value="3.0">3.0</option>
                                </select>
                                <select name="selMode" id="eclModeID" tabindex="3">
                                    <option value="">Mode</option>
                                    <option value="XML">XML</option>
                                    <option value="Batch">Batch</option>
                                </select>
                                  <select name="selEnv" id="eclEnvID" tabindex="4">
                                    <option value="">Environment</option>
                                    <option value="Cert">Cert</option>
                                    <option value="Prod">Prod</option>
                                </select>
                                <select name="selRestriction" id="eclRestricID" tabindex="5">
                                    <option value="">Restriction</option>
                                    <option value="FCRA">FCRA</option>
                                    <option value="Non-FCRA">Non-FCRA</option>
                                </select>
                                <select name="selCustomer" id="eclCustID" tabindex="6">
                                    <option value="">Customer</option>
                                    <option value="Flagship">Flagship</option>
                                </select>
                            </div>
                        </div>
                    </td>
                    <td>
                        <asp:Button ID="btnGatherData" runat="server" Text="Gather Data" CssClass="myButton" Style="width: 100px" OnClick="btnGatherData_Click" />
                        <asp:Button ID="btnECLHome" runat="server" Text="Home" class="homeButton" Style="width: 100px" />
                        <asp:Button ID="btnECLReset" runat="server" Text="Reset" class="resetButton" Style="width: 100px" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Compare Now markup -->
        <div id="content" style="width: 100%; background-color: #e6e6e6; margin-top: 5px;">
            <table>
                <tr>
                    <td style="width: 33%; padding: 10px 10px 10px 10px;">
                        <div class="disableControls">
                            <asp:Label ID="lblPrevDate" runat="server" Text="Previous Date" CssClass="myLabel"></asp:Label>
                            <asp:UpdatePanel ID="updPrevDate" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Calendar BorderWidth="1px" DayNameFormat="FirstTwoLetters"
                                        Font-Names="Meiryo UI" Font-Size="16px" CssClass="calendar1" ID="cdrPrevDate" Visible="true"
                                        runat="server" CellSpacing="1" ForeColor="#172013" OnSelectionChanged="cdrPrevDate_SelectionChanged"
                                        Width="250px" OnDayRender="cdrPrevDate_DayRender" OnVisibleMonthChanged="cdrPrevDate_VisibleMonthChanged">
                                        <SelectedDayStyle BackColor="#FF6666" />
                                        <SelectorStyle BackColor="#FF6666" ForeColor="White" />
                                        <DayHeaderStyle BackColor="#d8c1c0" ForeColor="Green" Font-Bold="false" Font-Size="Smaller" />
                                        <DayStyle BackColor="#ffffff" BorderColor="#FFFFCC" BorderWidth="1px" Font-Italic="False"
                                            Font-Names="Meiryo UI" Font-Size="Smaller" HorizontalAlign="Justify" Wrap="False" />
                                        <WeekendDayStyle BackColor="#cccccc" ForeColor="#999999" />
                                        <TitleStyle Font-Bold="true" ForeColor="Black" Font-Names="Arial, sans-serif" Font-Size="14px" />
                                    </asp:Calendar>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cdrPrevDate" />
                                    <asp:AsyncPostBackTrigger ControlID="mode_id" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>

                    </td>
                    <%--showing all the selection parameter dropdowns--%>
                    <td style="width: 34%; padding: 10px 10px 10px 10px;">
                        <div style="width: 100%; margin-top: 6%">
                            <%-- <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="model_id" runat="server" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="model_id_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="">Model</asp:ListItem>
                                        <asp:ListItem Value="Risk View">Risk View</asp:ListItem>
                                        <asp:ListItem Value="Fraud Point">Fraud Point</asp:ListItem>
                                        <asp:ListItem Value="Lead Integrity">Lead Integrity</asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="model_id" EventName="SelectedIndexChanged" />
                                    </Triggers>
                            </asp:UpdatePanel--%>
                            <select name="selModel" id="model_id" class="modelClass" tabindex="1">
                                <option value="" style="font-weight: bolder">Model</option>
                                <option value="Risk View">Risk View</option>
                                <option value="Fraud Point">Fraud Point</option>
                                <option value="Lead Integrity">Lead Integrity</option>
                            </select>
                            <div class="disableControls">
                                <select name="selVersion" id="version_id" tabindex="2">
                                    <option value="">Version</option>
                                    <option value="0">v0</option>
                                    <option value="3">v3</option>
                                    <option value="4">v4</option>
                                </select>
                                        <%--<asp:DropDownList ID="version_id" runat="server" TabIndex="2">
                                            <asp:ListItem Selected="True" Value="">Version</asp:ListItem>
                                           <asp:ListItem Value="0">v0</asp:ListItem>
                                            <asp:ListItem Value="3">v3</asp:ListItem>
                                            <asp:ListItem Value="4">v4</asp:ListItem>
                                        </asp:DropDownList>--%>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="mode_id" runat="server" TabIndex="3" OnSelectedIndexChanged="mode_id_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Selected="True" Value="">Mode</asp:ListItem>
                                            <asp:ListItem Value="BATCH">BATCH</asp:ListItem>
                                            <asp:ListItem Value="XML">XML</asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <script type="text/javascript" lang="javascript">
                                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                                    prm.add_pageLoaded(PageLoadedEventHandler);
                                    function PageLoadedEventHandler() {
                                        $("#version_id").selectbox();
                                        $("#mode_id").selectbox();
                                    }
                                </script>
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
                                    <option value="Flagship">Flagship</option>
                                </select>
                            </div>
                        </div>
                    </td>
                    <td style="width: 33%; padding: 10px 10px 10px 10px;">
                        <div id="divCurrentCal" class="disableControls">
                            <asp:Label ID="lblCurrentDate" runat="server" Text="Current Date" CssClass="myLabel"></asp:Label>
                            <asp:UpdatePanel ID="upCdrCurrentDate" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Calendar BorderWidth="1px" DayNameFormat="FirstTwoLetters"
                                        Font-Names="Meiryo UI" Font-Size="16px" CssClass="calendar1" ID="cdrCurrentDate" Visible="true"
                                        runat="server" CellSpacing="1" ForeColor="#172013" OnSelectionChanged="cdrCurrentDate_SelectionChanged"
                                        Width="250px" OnDayRender="cdrCurrentDate_DayRender" OnVisibleMonthChanged="cdrCurrentDate_VisibleMonthChanged">
                                        <SelectedDayStyle BackColor="#FF6666" />
                                        <SelectorStyle BackColor="#FF6666" ForeColor="White" />
                                        <DayHeaderStyle BackColor="#d8c1c0" ForeColor="Green" Font-Bold="false" Font-Size="Smaller" />
                                        <DayStyle BackColor="#ffffff" BorderColor="#FFFFCC" BorderWidth="1px" Font-Italic="False"
                                            Font-Names="Meiryo UI" Font-Size="Smaller" HorizontalAlign="Justify" Wrap="False" />
                                        <WeekendDayStyle BackColor="#cccccc" ForeColor="#999999" />
                                        <TitleStyle Font-Bold="true" ForeColor="Black" Font-Names="Arial, sans-serif" Font-Size="14px" />
                                    </asp:Calendar>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="cdrCurrentDate" />
                                    <asp:AsyncPostBackTrigger ControlID="cdrPrevDate" />
                                    <asp:AsyncPostBackTrigger ControlID="mode_id" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%; padding: 10px 10px 10px 10px;">
                        <div class="disableControls">
                            <asp:UpdatePanel ID="getPrevTimes" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divPrevTime">
                                        <asp:DropDownList ID="ddlPrevTime" runat="server"
                                            AppendDataBoundItems="true" DataTextField="PreviousTime"
                                            DataValueField="prevTime" OnSelectedIndexChanged="ddlPrevTime_SelectedIndexChanged">
                                            <asp:ListItem Text="Previous Time" Value="Previous Time1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
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
                        </div>
                    </td>
                    <td style="width: 34%; padding: 10px 10px 10px 10px;">
                        <asp:DropDownList runat="server" ID="ddlCompareIDs" Visible="false">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%; padding: 10px 10px 10px 10px;">
                        <div class="disableControls">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divCurrentTime">
                                        <asp:DropDownList ID="ddlCurrentTime" runat="server"
                                            AppendDataBoundItems="true" DataTextField="CurrentTime"
                                            DataValueField="currentTime" OnSelectedIndexChanged="ddlCurrentTime_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Text="Current Time" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
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
                                    //$("#ddlCurrentTime").selectbox('enable');
                                    //$("#divCurrentCal").click(function(){
                                    //    $("#divCurrentTime").slideDown(1000,'swing', function () {
                                    //        $(this).css({ display:"block" })
                                    //         .focus()
                                    //        //$("div").css("visibility", "hidden");
                                    //    })});
                                }
                            </script>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan='3' style="width: 100%">


                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <input type="button" id="btnHome" value="Home" class="homeButton" />

                                </td>
                                <td>
                                    <div class="disableControls" style="width: 100%">
                                        <asp:Button ID="btnSubmit" Text="Compare" runat="server" OnClick="btnSubmit_Click" CssClass="myButton" />
                                    </div>
                                </td>
                                <td>
                                    <div class="disableControls" style="width: 100%">
                                        <input type="button" id="btnReset" value="Reset" class="resetButton" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <div id="result" style="width: 100%; margin-top: 5%;">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <!--Getting previous comparisons markup -->
        <div id="historyContent" style="width: 100%; background-color: #e6e6e6; margin-top: 5px;">
            <table style="width: 100%">
                <tr>
                    <td>
                        <table style="border-width: 1px; border-style: inset" class="display" id="historyTable">
                            <thead style="background-color: #CC0033; color: white">
                                <tr>
                                    <th>Serial</th>
                                    <th>Model</th>
                                    <th>Dates</th>
                                    <th>Version</th>
                                    <th>Mode</th>
                                    <th>Environment</th>
                                    <th>Restriction</th>
                                    <th>Customer</th>
                                    <th>Delete</th>
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
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>

                </tr>
                <tr style="margin-bottom: 30px; width: 100%">
                    <td>
                        <table>
                            <tr>
                                <td style="width: 33.33%">
                                    <input type="button" id="btnHistoryHome" value="Home" class="homeButton" />
                                </td>
                                <td style="width: 33.33%">

                                    <asp:Button ID="btnHistoryCompare" Text="Get Compare Results" runat="server" OnClick="btnHistoryCompare_Click" CssClass="myButton" />

                                </td>
                                <td style="width: 33.33%">
                                    <input type="button" id="btnHistoryReset" value="Reset" class="resetButton" />

                                </td>
                            </tr>
                        </table>
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
            <asp:HiddenField ID="hifCompareIDSerial" runat="server" />
            <asp:HiddenField ID="hifECLModel" runat="server" />
            <asp:HiddenField ID="hifECLVersion" runat="server" />
            <asp:HiddenField ID="hifECLMode" runat="server" />
        </div>
    </form>
</body>
</html>
