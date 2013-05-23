<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="testApplication1._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>

    <link href="CSS/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-ui.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#btnSubmit").click(function() {
                alert("Hello World");
            });
            $(function() {
                $("#tabs").tabs({
                    activate: function(event, ui) {
                        //getting the active tab index
                        var active = $("#tabs").tabs("option", "active");
                        //getting the html inside the a tag for the active tab
                        var tabName = $("#tabs ul>li a").eq(active).html();
                        //setting the value of the asp label to the active tab's name
                        $("#<%=lblHidden.ClientID%>").html(tabName);

                    }
                });
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="tabs">
        <ul>
            <li><a href="#tabScores">Scores</a></li>
            <li><a href="#tabAttributes">Attributes</a></li>
            <li><a href="#tabReasonCodes">Reason codes</a></li>
        </ul>
        <asp:Label runat="server" ID="lblValue"></asp:Label>
        <div id="tabScores">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblModel" runat="server" Text="Model:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlModel" runat="server">
                            <asp:ListItem Enabled="true" Selected="True" Text="Select" Value=""></asp:ListItem>
                            <asp:ListItem Text="RiskView" Value="RiskView"></asp:ListItem>
                            <asp:ListItem Text="FraudPoint" Value="FraudPoint"></asp:ListItem>
                            <asp:ListItem Text="Lead Integrity" Value="LeadIntegrity"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblVersion" runat="server" Text="Version:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVersion" runat="server">
                            <asp:ListItem Enabled="true" Selected="True" Text="Select" Value=""></asp:ListItem>
                            <asp:ListItem Text="4.0" Value="4"></asp:ListItem>
                            <asp:ListItem Text="3.0" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMode" runat="server" Text="Mode:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMode" runat="server">
                            <asp:ListItem Enabled="true" Selected="True" Text="Select" Value=""></asp:ListItem>
                            <asp:ListItem Text="XML" Value="XML"></asp:ListItem>
                            <asp:ListItem Text="Batch" Value="Batch"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEnvironment" runat="server" Text="Environment:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEnvironment" runat="server">
                            <asp:ListItem Enabled="true" Selected="True" Text="Select" Value=""></asp:ListItem>
                            <asp:ListItem Text="Cert" Value="Cert"></asp:ListItem>
                            <asp:ListItem Text="Prod" Value="Prod"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRestriction" runat="server" Text="Restriction:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRestriction" runat="server">
                            <asp:ListItem Enabled="true" Selected="True" Text="Select" Value=""></asp:ListItem>
                            <asp:ListItem Text="FCRA" Value="FCRA"></asp:ListItem>
                            <asp:ListItem Text="Non-FCRA" Value="NonFCRA"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPrevRunDate" runat="server" Text="Previous Rundate:"></asp:Label>
                    </td>
                    <td>
                       <asp:Calendar id="cdrPrevRunDate" OnDayRender="cdrPrevRunDate_Render" runat="server" ></asp:Calendar>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCurrentRunDate" runat="server" Text="Current Rundate:"></asp:Label>
                    </td>
                    <%--<td>
                       <asp:Calendar id="cdrCurrentRunDate" OnDayRender="cdrCurrentRunDate_Render" runat="server" ></asp:Calendar>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCustomer" runat="server" Text="Customer:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCustomer" runat="server">
                            <asp:ListItem Enabled="true" Selected="True" Text="Select" Value=""></asp:ListItem>
                            <asp:ListItem Text="Generic" Value="Generic"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabAttributes">
            Attributes tab
        </div>
        <div id="tabReasonCodes">
            Reason codes tab
        </div>
        <asp:Button ID="btnSubmit" runat="server" Text="Refresh" UseSubmitBehavior="true"
            OnClick="btnSubmit_Click" />
        <asp:Label ID="lblHidden" runat="server"></asp:Label>
    </div>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="ALAWQPNC004">
        <Columns>
            <asp:BoundField DataField="request_in" HeaderText="request_in" 
                SortExpression="request_in" />
            <asp:BoundField DataField="accountnumber" HeaderText="accountnumber" 
                SortExpression="accountnumber" />
            <asp:BoundField DataField="column_name" HeaderText="column_name" 
                SortExpression="column_name" />
            <asp:BoundField DataField="file1_value" HeaderText="file1_value" 
                SortExpression="file1_value" />
            <asp:BoundField DataField="file2_value" HeaderText="file2_value" 
                SortExpression="file2_value" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="ALAWQPNC004" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DQConnectionString %>" 
        SelectCommand="SELECT [request_in], [accountnumber], [column_name], [file1_value], [file2_value] FROM [scr_compare_results]">
    </asp:SqlDataSource>
    </form>
</body>
</html>
