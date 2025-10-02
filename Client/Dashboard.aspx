<%@ Page Title='<%# ResourceHelper.GetString("Dashboard_Title") %>' 
    Language="C#" MasterPageFile="~/Site.master" 
    AutoEventWireup="true" Async="true" 
    CodeBehind="Dashboard.aspx.cs" 
    Inherits="Client.Dashboard" %>

<asp:Content ID="DashboardContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <asp:Literal runat="server" ID="litTitle" Text='<%# ResourceHelper.GetString("Dashboard_Title") %>' />
    </h2>

    <!-- Buttons -->
    <asp:Button ID="btnCreateUser" runat="server" 
        Text='<%# ResourceHelper.GetString("Dashboard_CreateUser") %>' 
        CssClass="btn btn-primary" OnClick="btnCreateUser_Click" Visible="false" />

    <asp:Button ID="btnAssignRole" runat="server" 
        Text='<%# ResourceHelper.GetString("Dashboard_AssignRole") %>' 
        CssClass="btn btn-primary" OnClick="btnAssignRole_Click" Visible="false" />

    <asp:Button ID="UpdateProfile" runat="server" 
        Text='<%# ResourceHelper.GetString("Dashboard_UpdateProfile") %>' 
        CssClass="btn btn-primary" OnClick="btnUpdateProfile_Click" />

    <!-- User info -->
    <asp:Panel ID="pnlUserInfo" runat="server" CssClass="mt-3" Visible="false">
        <h3><%# ResourceHelper.GetString("Dashboard_UserInfo") %></h3>
        <table class="table table-sm table-bordered w-auto">
            <tbody>
                <tr>
                    <th scope="row"><%# ResourceHelper.GetString("Dashboard_UserName") %></th>
                    <td><asp:Label ID="lblUserName" runat="server" /></td>
                </tr>
                <tr>
                    <th scope="row"><%# ResourceHelper.GetString("Dashboard_Email") %></th>
                    <td><asp:Label ID="lblEmail" runat="server" /></td>
                </tr>
                <tr>
                    <th scope="row"><%# ResourceHelper.GetString("Dashboard_Type") %></th>
                    <td><asp:Label ID="lblType" runat="server" /></td>
                </tr>
                <tr>
                    <th scope="row"><%# ResourceHelper.GetString("Dashboard_Roles") %></th>
                    <td><asp:BulletedList ID="bltRoles" runat="server" DisplayMode="Text" CssClass="mb-0" /></td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>

    <!-- All users grid -->
    <asp:Panel ID="pnlAllUsers" runat="server" CssClass="mt-4" Visible="false">
        <h3><%# ResourceHelper.GetString("Dashboard_AllUsers") %></h3>
        <asp:GridView ID="gvUsers" runat="server"
            CssClass="table table-sm table-striped table-bordered"
            AutoGenerateColumns="False"
            DataKeyNames="Id"
            ShowHeader="true"
            OnRowDataBound="gvUsers_RowDataBound"
            OnRowCommand="gvUsers_RowCommand"
            OnRowDeleting="gvUsers_RowDeleting"
            AllowPaging="true" PageSize="20" OnPageIndexChanging="gvUsers_PageIndexChanging">

            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="User test"/>
                <asp:BoundField DataField="Email"/>
                <asp:BoundField DataField="Type"/>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkUpdate" runat="server"
                            CommandName="EditUser"
                            CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-sm btn-secondary me-1"
                            Text='<%# ResourceHelper.GetString("Dashboard_Update") %>' />

                        <asp:LinkButton ID="lnkDelete" runat="server"
                            CommandName="Delete"
                            CommandArgument='<%# Eval("Id") %>'
                            OnClientClick='<%# "return confirm(\"" + ResourceHelper.GetString("Dashboard_DeleteConfirm") + "\");" %>'
                            CssClass="btn btn-sm btn-danger"
                            Text='<%# ResourceHelper.GetString("Dashboard_Delete") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <EmptyDataTemplate>
                <div class="text-muted"><%# ResourceHelper.GetString("Dashboard_NoUsers") %></div>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
