<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testPage.aspx.cs" Inherits="testApplication1.testPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title></title>

    <script src="Scripts/jquery-1.7.2.js" type="text/javascript"></script>

    <link href="CSS/jquery.selectbox.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery.selectbox-0.2.js" type="text/javascript"></script>

    <script src="Scripts/jquery.selectbox-0.2.min.js" type="text/javascript"></script>

    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>

    <link href="CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jquery.ui.progressbar.min.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery.ui.progressbar.js" type="text/javascript"></script>

    <script src="Scripts/glDatePicker.js" type="text/javascript"></script>

    <link href="CSS/glDatePicker.darkneon.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        $(function() {
            $("select").selectbox();
            $("#btnSubmit").button();
            $("#progressbar").progressbar({ value: 0 });

            $("#txtPrevDate").glDatePicker({
                showAlways: true,
                cssName: 'darkneon'
            });
            $("#cdrCurrentDate").glDatePicker();

            $("#btnSubmit").click(function() {
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

                var intervalID = setInterval(updateProgress, 250);
                $.ajax({
                    type: "POST",
                    url: "testPage.aspx/GetText",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function(msg) {
                        $("#progressbar").progressbar("value", 100);
                        $("#result").text(msg.d);
                        clearInterval(intervalID);
                        
                    }

                });

                //alert(radioSelected);
            });

            function updateProgress() {
                var value = $("#progressbar").progressbar("option", "value");
                if (value < 100) {
                    $("#progressbar").progressbar("value", value + 0.25);
                }
            }




        });
            
       
        
    </script>

    <style type="text/css">
        label
        {
            width: 200px;
        }
        table
        {
            background-color: #2d2d2d;
        }
        #btnSubmit
        {
            background-color: #2d2d2d;
            color: #EBB52D;
            border: solid 1px #515151;
            font-family: Arial, sans-serif;
            font-size: 12px;
            font-weight: normal;
            height: 30px;
            position: relative;
            width: 200px;
        }
        label
        {
            background-color: #2d2d2d;
            color: #EBB52D;
        }
        #lnHeader
        {
            background-color: #c8c8c8;
        }
        body
        {
            background-color: #F0F0F0;
        }
    </style>
</head>
<body style="margin-left: auto; margin-right: auto; margin-top: 5%; width: 100%">
    <form id="form1" runat="server">
    <div id="lnHeader">
        <img src="img/lexislogo.gif" alt="LexisNexis" /></div>
    <table width="100%">
        <tr>
            <td align="center" style="width:33%">
                <asp:TextBox ID="txtPrevDate" runat="server" Width="50%"></asp:TextBox>
            </td>
            <%--showing all the selection parameter dropdowns--%>
            <td align="center" style="width:33%">
                <div align="center" style="width: 65%; margin-top: 5%">
                    <select name="selModel" id="model_id" tabindex="1">
                        <option value="">Model </option>
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
            <td align="center" style="width:33%">
                <div id="cdrCurrentDate" style="width: 30%">
                </div>
            </td>
        </tr>
        <tr style="margin-top: 15%">
            <td colspan="3" align="center">
                <asp:Button ID="btnSubmit" Text="Submit" runat="server" Width="10%" OnClick="btnSubmit_Click" />
            </td>
        </tr>
        <tr style="margin-top: 15%">
            <td colspan="3" align="center">
                <div id="progressbar" style="width: 33%; margin-top: 5%;" align="center">
                </div>
            </td>
        </tr>
        <tr style="margin-top: 15%">
            <td colspan="3">
                <div id="result" style="width: 100%; margin-top: 5%;" align="center">
                </div>
            </td>
        </tr>
    </table>
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
