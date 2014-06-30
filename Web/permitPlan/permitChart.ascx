<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="permitChart.ascx.cs" Inherits="HawaiiDBEDT.Web.permitPlan.permitChart" %>
    <asp:Chart ID="Chart1" runat="server" Width="850px">
        <Legends>
            <asp:Legend LegendStyle="Row" Docking="Top" Alignment="Center" Title="Permit Schedule" TitleFont="Arial, 12pt, style=Bold">
            </asp:Legend>
        </Legends>
        <Series>
            <asp:Series Name="Federal" ChartType="RangeBar" Color="#F9DAB2" Font="Arial, 8pt, style=Bold" BackHatchStyle="Cross">
            </asp:Series>
            <asp:Series Name="State" ChartType="RangeBar" Color="#B0D7BE" Font="Arial, 8pt, style=Bold" BackHatchStyle="Vertical">
            </asp:Series>
            <asp:Series Name="County" ChartType="RangeBar" Color="#A4D5E6" Font="Arial, 8pt, style=Bold" >
            </asp:Series>
        </Series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1">
                <axisy LineColor="64, 64, 64, 64" IsStartedFromZero="True" Interval="1" Minimum="0" Title="Months">
					<MajorGrid LineColor="64, 64, 64, 64" />
				</axisy>
				<axisx LineColor="64, 64, 64, 64" Enabled="False" Interval="1">
					<MajorGrid LineColor="64, 64, 64, 64"/>
				</axisx>
            </asp:ChartArea>
        </chartareas>
    </asp:Chart>