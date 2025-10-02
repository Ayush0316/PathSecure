<%@ Page Title='<%# ResourceHelper.GetString("PageTitle") %>' Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="PasswordSet.aspx.cs" Async="true" Inherits="Client.PasswordSet" %>
<%@ Register Src="~/Controls/LinkValidationSummary.ascx" TagPrefix="uc" TagName="LinkValidationSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><asp:Literal runat="server" ID="litTitle" Text='<%# ResourceHelper.GetString("PageTitle") %>' /></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlSetPassword" runat="server">

                <uc:LinkValidationSummary 
                    ID="setPasswordSummary" 
                    runat="server"
                    ValidationGroup="SetPasswordGroup"
                    HeaderTextKey="SummaryText"
                    ShowHeader="true"
                />

                <br />

                <!-- Password -->
                <asp:Label ID="lblPassword" runat="server" Text='<%# ResourceHelper.GetString("Password") %>' AssociatedControlID="txtPassword" />
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator 
                    ID="rfvPassword" 
                    runat="server" 
                    ControlToValidate="txtPassword" 
                    ErrorMessage='<%# ResourceHelper.GetString("PasswordRequired") %>'
                    Text='<%# ResourceHelper.GetString("PasswordRequired") %>'
                    Display="None" 
                    ValidationGroup="SetPasswordGroup" />

                <!-- Confirm Password -->
                <asp:Label ID="lblConPassword" runat="server" Text='<%# ResourceHelper.GetString("ConfirmPassword") %>' AssociatedControlID="txtConfPassword" />
                <asp:TextBox ID="txtConfPassword" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator 
                    ID="rfvConfPassword" 
                    runat="server" 
                    ControlToValidate="txtConfPassword" 
                    ErrorMessage='<%# ResourceHelper.GetString("ConfirmPasswordRequired") %>'
                    Text='<%# ResourceHelper.GetString("ConfirmPasswordRequired") %>'
                    Display="None" 
                    ValidationGroup="SetPasswordGroup" />

                <br />

                <!-- Set Password Button -->
                <asp:Button 
                    ID="btnSetPassword" 
                    runat="server" 
                    Text='<%# ResourceHelper.GetString("BtnText") %>' 
                    CssClass="btn btn-primary" 
                    OnClick="btnSetPassword_Click" />

                <br /><br />

                <!-- Error Label -->
                <asp:Label 
                    ID="setPasswordError" 
                    runat="server" 
                    CssClass="text-danger" 
                    Visible="false" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
