<%@ Page Title='<%# ResourceHelper.GetString("PageTitle") %>' Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Async="true" CodeBehind="LoginPage.aspx.cs" Inherits="Client.LoginPage" %>
<%@ Register Src="~/Controls/LinkValidationSummary.ascx" TagPrefix="uc" TagName="LinkValidationSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><asp:Literal runat="server" ID="litTitle" Text='<%# ResourceHelper.GetString("PageTitle") %>' /></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlLogin" runat="server">
                <uc:LinkValidationSummary 
                    ID="loginSummary" 
                    runat="server"
                    ValidationGroup="LoginGroup"
                    HeaderTextKey="SummaryText"
                    ShowHeader=true />

                <br />

                <!-- Username -->
                <asp:Label ID="lblUsername" runat="server" Text='<%# ResourceHelper.GetString("Username") %>' AssociatedControlID="txtUsername" />
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator 
                    ID="rfvUsername" 
                    runat="server" 
                    ControlToValidate="txtUsername" 
                    ErrorMessage='<%# ResourceHelper.GetString("UsernameRequired") %>'
                    Text='<%# ResourceHelper.GetString("UsernameRequired") %>'
                    Display="None" 
                    ValidationGroup="LoginGroup" />

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
                    ValidationGroup="LoginGroup" />

                <br />

                <!-- Login Button -->
                <asp:Button 
                    ID="btnLogin" 
                    runat="server" 
                    Text='<%# ResourceHelper.GetString("BtnText") %>' 
                    CssClass="btn btn-primary" 
                    OnClick="btnLogin_Click" />

                <br /><br />

                <asp:Label 
                    ID="loginError" 
                    runat="server" 
                    CssClass="text-danger" 
                    Visible="false" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>