<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Currency Exchange Rate by MK</title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <link href="CSS/Styles.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">

        <div class="container-fluid">

            <%--  Row 1  --%>

            <div class="row">
                <div class="offset-md-3 col-md-6 overallBG border border-bottom-0">
                    <h2 class="text-center margin1 py-2">Select exchange currencies</h2>
                    <br />
                </div>
            </div>

            <%--  Row 2  --%>

            <div class="row">
                <div class="offset-md-3 col-md-6 border-left paddingLeft overallBG">
                    <div class="row">

                        <%--  DropDowns  --%>

                        <div class="col-md-7 DropDownRight">
                            <b>From:</b>
                            <asp:DropDownList ID="DropDown_From" runat="server" AutoPostBack="True"
                                Width="80px"
                                DataSourceID="ObjectDataSource1"
                                OnSelectedIndexChanged="SelectedChanged" ToolTip="Select Currency">
                            </asp:DropDownList>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetCurrencies" TypeName="ExchangeRateLibrary.JsonWorker"></asp:ObjectDataSource>

                            <b>&nbsp;To:</b>
                            <asp:DropDownList ID="DropDown_To" runat="server" AutoPostBack="True"
                                Width="80px"
                                DataSourceID="ObjectDataSource1"
                                OnSelectedIndexChanged="SelectedChanged" ToolTip="Select Currency" Selected="false">
                            </asp:DropDownList>
                        </div>

                        <%--  Card  --%>

                        <div class="col-md-5 border-right overallBG">
                            <div class="card backgroundCard MyCard mt-1">
                                <small class="smallFont">Real-time Exchange Rate</small>

                                <asp:Label runat="server" ID="lbl_ExchangeRate" CssClass="paddingText"><b>0.0000</b></asp:Label>

                                <div class="borderColor border-top">
                                    <asp:Label runat="server" ID="lbl_From">PLN</asp:Label>
                                    <asp:ImageButton ID="btn_swap" runat="server" ImageUrl="~/Swap Icon.png" Width="25px" Height="16px" OnClick="Btn_Swap_Click" />
                                    <asp:Label runat="server" ID="lbl_To">USD</asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">

                <%--  Row 3   --%>

                <%--  Chart  --%>

                <div class="col-10 offset-md-3 col-md-5 marginRight border-left overallBG">
                    <asp:Label ID="Lbl_CharError" runat="server" Width="412px" Visible="false"></asp:Label>
                    <asp:Chart ID="Chart" runat="server"  Visible="true"
                        Palette="Light" CssClass="chart margin1 marginRight" BackColor="WhiteSmoke">
                        <Series>
                            <asp:Series Name="Series1" ChartType="Area" ChartArea="ChartArea1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>

                </div>

                <%--  Chart Buttons  --%>

                <div class="col-2 col-md-1 marginLeft chartButtonPadding overallBG border-right">
                    <div class="btn-group-vertical">
                        <asp:Button ID="Button2" runat="server" Text="30D" Width="60" CssClass="btn btn-secondary btn-space" />
                        <asp:Button ID="Button3" runat="server" Text="60D" Width="60" CssClass="btn btn-secondary btn-space" />
                        <asp:Button ID="Button4" runat="server" Text="90D" Width="60" CssClass="btn btn-secondary btn-space" />
                        <asp:Button ID="Button5" runat="server" Text="180D" Width="60" CssClass="btn btn-secondary btn-space" />
                        <asp:Button ID="Button7" runat="server" Text="All" Width="60" CssClass="btn btn-secondary btn-space" />
                    </div>
                </div>
            </div>

            <%--  Row 4  --%>

            <div class="row">
                <asp:CheckBox runat="server" ID="Cbx_ShowTrend" Text="Show Trend Lines"  
                    AutoPostBack="True" OnCheckedChanged="Cbx_ShowTrend_CheckedChanged" Checked="true"
                    CssClass="offset-md-3 col-md-5 col-0 text-right overallBG border-bottom MyCheckBox" />
                <div class="col-md-1 border-bottom border-right overallBG d-none d-md-block">&nbsp</div>
            </div>
        </div>





        <%--        <div class="auto-style10">

                        <asp:Label ID="Lbl_CharError" runat="server" Width="412px" Visible="false"></asp:Label>
                        <asp:Chart ID="Chart" runat="server" Width="412px" Palette="Light" Visible="true">
                            <series>
                                <asp:Series Name="Series1" ChartType="Area" ChartArea="ChartArea1">

                            </asp:Series>
                            </series>
                             <chartareas>
                                 <asp:ChartArea Name="ChartArea1">
                             </asp:ChartArea>
                             </chartareas>
                        </asp:Chart>
                            </td>
                    </fieldset>


                </tr>
                <tr>
                    <td class="auto-style9">
                        <table class="auto-style6">
                            <tr>
                                <td><asp:RadioButton ID="rdb_30" runat="server" AutoPostBack="True" Text="30 days" GroupName="SelectedDays" OnCheckedChanged="Rdb_Changed"/>
                                </td>
                                <td><asp:RadioButton ID="rdb_60" runat="server" AutoPostBack="True" Text="60 days" GroupName="SelectedDays" OnCheckedChanged="Rdb_Changed"/>
                                </td>
                                <td><asp:RadioButton ID="rdb_90" runat="server" AutoPostBack="True" Text="90 days" GroupName="SelectedDays" OnCheckedChanged="Rdb_Changed"/>
                                </td>
                                <td><asp:RadioButton ID="rdb_180" runat="server" AutoPostBack="True" Text="180 days" GroupName="SelectedDays" OnCheckedChanged="Rdb_Changed"/>
                                </td>
                                <td><asp:RadioButton ID="rdb_all" runat="server" AutoPostBack="True" Text="All" GroupName="SelectedDays" OnCheckedChanged="Rdb_Changed" Checked="True"/>
                                </td>
                            </tr>

                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
</fieldset>
        </div>--%>
    </form>
</body>
</html>
