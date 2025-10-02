<%@ Page Title='<%# ResourceHelper.GetString("PageTitle") %>' Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Async="true" CodeBehind="User.aspx.cs" Inherits="Client.CreateUser" %>
<%@ Register Src="~/Controls/LinkValidationSummary.ascx" TagPrefix="uc" TagName="LinkValidationSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><asp:Literal runat="server" ID="litTitle" /></h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlCreateUser" runat="server">

                <uc:LinkValidationSummary 
                    ID="createUserSummary" 
                    runat="server"
                    ValidationGroup="CreateUserGroup"
                    HeaderTextKey="SummaryText"
                    ShowHeader="true"
                />

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
                    ValidationGroup="CreateUserGroup" />

                <!-- Email -->
                <asp:Label ID="lblEmail" runat="server" Text='<%# ResourceHelper.GetString("Email") %>' AssociatedControlID="txtEmail" />
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                <asp:RegularExpressionValidator
                    ID="revEmail"
                    runat="server"
                    ControlToValidate="txtEmail"
                    ValidationExpression="^\s*[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\s*$"
                    ErrorMessage='<%# ResourceHelper.GetString("InvalidEmail") %>'
                    Text='<%# ResourceHelper.GetString("InvalidEmail") %>'
                    Display="None"
                    ValidationGroup="CreateUserGroup" />

                <!-- User Type -->
                <asp:Label ID="lblUserType" runat="server" Text='<%# ResourceHelper.GetString("UserType") %>' AssociatedControlID="ddlUserType" />
                <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control">
                </asp:DropDownList>

                <!-- Password -->
                <asp:Label ID="lblPassword" visible="false" runat="server" Text='<%# ResourceHelper.GetString("Password") %>' AssociatedControlID="txtPassword" />
                <asp:TextBox ID="txtPassword" visible="false" runat="server" TextMode="Password" CssClass="form-control" />

                <!-- Confirm Password -->
                <asp:Label ID="lblConPassword" visible="false" runat="server" Text='<%# ResourceHelper.GetString("ConfirmPassword") %>' AssociatedControlID="txtConfPassword" />
                <asp:TextBox ID="txtConfPassword" visible="false" runat="server" CssClass="form-control" />

                <br />

                <!-- Create User Button -->
                <asp:Button 
                    ID="btnCreateUser" 
                    runat="server"
                    CssClass="btn btn-primary" 
                    OnClick="btnCreateUser_Click" />

                <br /><br />

                <!-- Error Label -->
                <asp:Label 
                    ID="createUserError" 
                    runat="server" 
                    CssClass="text-danger" 
                    Visible="false" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
