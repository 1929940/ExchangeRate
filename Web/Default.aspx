<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 99%;
            height: 95%;
            float: left;
            background-color: #FFFFFF;
        }
        .auto-style2 {
            width: 99%;
        }
        .auto-style4 {
            width: 140px;
            text-align: left;
        }
        .auto-style5 {
            height: 299px;
            width: 718px;
            text-align: left;
        }
        .auto-style6 {
            width: 100%;
        }
        .auto-style7 {
            height: 23px;
            text-align: right;
        }
        .auto-style9 {
            width: 718px;
            text-align: left;
        }
        .auto-style10 {
            width: 427px;
            text-align: left;
        }
        .auto-style11 {
            text-align: center;
            font-size: xx-large;
        }
        .auto-style12 {
            text-align: left;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="auto-style10">
            <fieldset>
            <table align="center" class="auto-style1" border="0">
                <tr>
                    <td class="auto-style9">
                        <h2 class="auto-style11">Currency Exchange Rate</h2>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style9" aria-orientation="horizontal">
                        <table class="auto-style2">
                            <tr>
                                <td class="auto-style4">From: <asp:DropDownList ID="DropDown_From" runat="server" Height="20px" Width="60px" AutoPostBack="True" DataSourceID="ObjectDataSource1" OnSelectedIndexChanged="SelectedChanged">
                                    </asp:DropDownList>
                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetCurrencies" TypeName="ExchangeRateLibrary.JsonWorker"></asp:ObjectDataSource>
                                    <br />
                                    <br />
                                </td>
                                <td class="auto-style4">To: <asp:DropDownList ID="DropDown_To" runat="server" Height="20px" Width="60px" AutoPostBack="True" DataSourceID="ObjectDataSource1" OnSelectedIndexChanged="SelectedChanged">
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                </td>
                                <td class="auto-style12">
                                    <fieldset>
                                        <font size="2">Real time exchange rate<br /></font>
                                            <b><asp:Label ID="lbl_ExchangeRate" runat="server" Text="0.00000"></asp:Label></b>
                                            <hr width="125px" align="left"/>
                                        <asp:Label ID="lbl_From" runat="server" Text="AED" Width="30px"></asp:Label>
                                        <asp:ImageButton ID="btn_swap" runat="server" ImageUrl="~/Swap Icon.png" OnClick="btn_Swap_Click" Width="25px" Height="16px" />
                                        <asp:Label ID="lbl_To" runat="server" Text="AED" Width="30px"></asp:Label>
                                    </fieldset>
                                    </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">
                        <fieldset>
                        <asp:Chart ID="Chart" runat="server" OnLoad="Chart_Load" Width="412px" Palette="Chocolate">
                            <series>
                                <asp:Series Name="Series1" ChartType="Area">
<%--                                    <Points>
                                        <asp:DataPoint AxisLabel="2019-05-18" YValues="4.3" />
                                        <asp:DataPoint AxisLabel="2019-05-19" YValues="4.4" />
                                        <asp:DataPoint AxisLabel="2019-05-20" YValues="4.6" />
                                        <asp:DataPoint AxisLabel="2019-05-21" YValues="4.0" />
                                        <asp:DataPoint AxisLabel="2019-05-22" YValues="1.9" />
                                        <asp:DataPoint AxisLabel="2019-05-23" YValues="2.4" />
                                        <asp:DataPoint AxisLabel="2019-05-24" YValues="3.2" />
                                        <asp:DataPoint AxisLabel="2019-05-25" YValues="4.0" />
                                        <asp:DataPoint AxisLabel="2019-05-26" YValues="4.1" />
                                        <asp:DataPoint AxisLabel="2019-05-27" YValues="1.9" />

                                    </Points>--%>
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
                                <td class="auto-style7">
                        <asp:CheckBox ID="Cbx_ShowTrend" runat="server" Text="Show Trend Lines" AutoPostBack="True" OnCheckedChanged="Cbx_ShowTrend_CheckedChanged"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
</fieldset>
        </div>
    </form>
</body>
</html>
