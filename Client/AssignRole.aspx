<%@ Page Title='<%# ResourceHelper.GetString("PageTitle") %>' Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" Async="true" CodeBehind="AssignRole.aspx.cs" Inherits="Client.AssignRole" %>
<%@ Register Src="~/Controls/LinkValidationSummary.ascx" TagPrefix="uc" TagName="LinkValidationSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><asp:Literal runat="server" ID="litTitle" Text='<%# ResourceHelper.GetString("PageTitle") %>' /></h2>

    <asp:UpdatePanel ID="updRoles" runat="server">
        <ContentTemplate>

            <uc:LinkValidationSummary 
                ID="assignRoleSummary" 
                runat="server"
                ValidationGroup="AssignRoleGroup"
                HeaderTextKey="SummaryText"
                ShowHeader="true" />

            <!-- User Selection -->
            <div class="form-group">
                <asp:Label ID="lblUser" runat="server" Text='<%# ResourceHelper.GetString("SelectUser") %>' AssociatedControlID="ddlUsers" />
                <asp:DropDownList 
                    ID="ddlUsers" 
                    runat="server" 
                    CssClass="form-control"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator 
                    ID="rfvUser" 
                    runat="server" 
                    ControlToValidate="ddlUsers" 
                    InitialValue=""
                    ErrorMessage='<%# ResourceHelper.GetString("UserRequired") %>'
                    Text='<%# ResourceHelper.GetString("UserRequired") %>'
                    Display="None"
                    ValidationGroup="AssignRoleGroup" />
            </div>

            <!-- Roles -->
            <asp:Panel ID="pnlRoles" runat="server" Visible="false" CssClass="mt-3">
                <asp:Label ID="lblRoles" runat="server" Text='<%# ResourceHelper.GetString("AssignRoles") %>' />
                <div>
                    <asp:PlaceHolder ID="phRoles" runat="server" />
                </div>
            </asp:Panel>

            <br />

            <!-- Submit -->
            <asp:Button 
                ID="btnSubmit" 
                runat="server" 
                Text='<%# ResourceHelper.GetString("BtnSaveRoles") %>' 
                CssClass="btn btn-primary"
                OnClick="btnSubmit_Click"
                ValidationGroup="AssignRoleGroup" />

            <br /><br />

            <!-- Feedback -->
            <asp:Label ID="lblResult" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
