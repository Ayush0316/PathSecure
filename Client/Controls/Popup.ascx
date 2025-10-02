<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Popup.ascx.cs" Inherits="Client.Controls.Popup" %>

<asp:Panel ID="pnlPopup" runat="server" CssClass="popup" Visible="false">
    <asp:Label ID="lblMessage" runat="server" />
</asp:Panel>

<style>
    .popup {
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 12px 18px;
        border-radius: 6px;
        z-index: 9999;
        color: white;
        font-family: Segoe UI, sans-serif;
        transition: opacity 0.5s ease;
    }
    .popup.success { background-color: #28a745; }
    .popup.error   { background-color: #dc3545; }
</style>