<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransferableListBox.ascx.cs" Inherits="HawaiiDBEDT.Web.admin.controls.TransferableListBox" %>
<style type="text/css">
    .listBoxDiv
    {
        float: left;
    }
    .listBox
    {
        float: left;
        font-family: "Segoe UI",Arial,sans-serif;
        font-size: 12px;
        border: 1px solid #C1DBFC;
        background-color: white;
        margin: 0px;
        padding: 0px;
        overflow: auto;
    }    
    input[type='submit'] 
    {
        font-weight: bold;
        width: 29px;  
    }
    .nextButtons
    {
        margin: 2px 5px;      
    }
    .firstButton
    {
        margin: 15px 5px 2px 5px;
    }
    .listBoxDiv option:hover 
    {
        color: #1E395B;
    }    
    .listBoxDiv option
    {
        padding: 2px 5px;
    }          
</style>
<div>
    <div class="listBoxDiv">
        <asp:Label runat="server" ID="lblSourceList" CssClass="small"></asp:Label>
        <br />
        <div runat="server" id="divLeft" style="background-color: white;">
            <asp:CheckBoxList runat="server" OnSelectedIndexChanged="HandleSourceChange" AutoPostBack="True"
                 RepeatLayout="Flow" ID="cblSourceList" CssClass="listBox"/>
        </div>
    </div>
    <div class="listBoxDiv">
        <asp:Button runat="server" ID="btnMoveSelected" Text=">" Enabled="False" OnClick="btnMoveSelected_Click"
            CssClass="firstButton" CausesValidation="False" /><br />
        <asp:Button runat="server" ID="btnMoveBackSelected" Text="<" Enabled="False" OnClick="btnMoveBackSelected_Click"
            CssClass="nextButtons" CausesValidation="False"/><br />
    </div>
    <div class="listBoxDiv">
        <asp:Label runat="server" ID="lblDestinationList" CssClass="small"></asp:Label>
        <br />
        <div runat="server" id="divRight" style="background-color: white;">
            <asp:CheckBoxList runat="server" OnSelectedIndexChanged="HandleDestinationChange" AutoPostBack="True"
                 RepeatLayout="Flow" ID="cblDestinationList" CssClass="listBox"/>
        </div>
    </div>
    <asp:Panel ID="panReorderButtons" runat="server" CssClass="listBoxDiv" Visible="False">
        <asp:Button runat="server" ID="btnMoveUpSelected" Text="/\" Enabled="False" OnClick="btnMoveUpSelected_Click"
            CssClass="firstButton" CausesValidation="False"/><br />
        <asp:Button runat="server" ID="btnMoveDownSelected" Text="\/" Enabled="False" OnClick="btnMoveDownSelected_Click"
            CssClass="nextButtons" CausesValidation="False"/>
    </asp:Panel>
</div>