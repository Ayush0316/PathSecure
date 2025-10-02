using Client.App_Data;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class Dashboard : BasePage
    {
        private const string SessionUsersKey = "AllUsersWithRoles";
        private HashSet<string> _permissionSet; // Cached permissions for this request

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterAsyncTask(new PageAsyncTask(LoadDashboardAsync));
            }
        }

        public void btnCreateUser_Click(object sender, EventArgs e)
        {
            RedirectToUrl("~/User.aspx");
        }

        public void btnAssignRole_Click(object sender, EventArgs e)
        {
            RedirectToUrl("~/AssignRole.aspx");
        }

        private async Task LoadDashboardAsync()
        {
            bool isLogOn = Session["isLogOn"] as bool? ?? false;
            if (!isLogOn)
            {
                RedirectToUrl("~/LoginPage.aspx");
            }

            // Cache permissions once
            _permissionSet = new HashSet<string>(Session["Permissions"] as List<string> ?? new List<string>(), StringComparer.Ordinal);

            UserDto userDto = Session["MY_DATA"] as UserDto;
            if (userDto == null)
            {
                var result = await ApiHelper.CallApiAsync<UserDto>("user/get", HttpMethod.Get);
                if (result?.IsSuccess == true)
                {
                    userDto = result.Body;
                    Session["MY_DATA"] = userDto;
                }
            }

            BindUserData(userDto);
            SetBtnVisiblity();
            await BindAllUsers();
        }

        private async Task BindAllUsers()
        {
            // Ensure permission cache exists (in case of postback paths)
            if (_permissionSet == null)
                _permissionSet = new HashSet<string>(Session["Permissions"] as List<string> ?? new List<string>(), StringComparer.Ordinal);

            bool havePermission = _permissionSet.Contains("ViewUser")
                               || _permissionSet.Contains("EditUser")
                               || _permissionSet.Contains("DeleteUser");

            if (!havePermission) return;

            var users = await LoadAllUsers();
            pnlAllUsers.Visible = true;
            gvUsers.DataSource = users;
            gvUsers.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (gvUsers.Columns.Count >= 4)
            {
                gvUsers.Columns[0].HeaderText = ResourceHelper.GetString("Dashboard_UserName");
                gvUsers.Columns[1].HeaderText = ResourceHelper.GetString("Dashboard_Email");
                gvUsers.Columns[2].HeaderText = ResourceHelper.GetString("Dashboard_Type");
                gvUsers.Columns[3].HeaderText = ResourceHelper.GetString("Dashboard_Actions");
            }
        }

        private async Task<IList<UserDto>> LoadAllUsers()
        {
            var users = Session[SessionUsersKey] as List<UserDto>;
            if (users != null)
            {
                return users;
            }
            var result = await ApiHelper.CallApiAsync<List<UserDto>>("user/special/get", HttpMethod.Get);
            if (result?.IsSuccess == true)
            {
                users = result.Body;
                Session[SessionUsersKey] = users;
                return users;
            }
            return new List<UserDto>();
        }

        // Paging
        protected async void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            await BindAllUsers();
        }

        // Row command for Update
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                var myData = Session["MY_DATA"] as UserDto;
                var userId = (string)e.CommandArgument;
                var isSelf = string.Equals(userId, myData.Id);
                RedirectToUrl($"User.aspx?id={HttpUtility.UrlEncode(userId)}&self={isSelf}");
            }
        }

        protected async void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var id = (string)gvUsers.DataKeys[e.RowIndex].Value;
            await DeleteUser(id);
            await BindAllUsers();
        }

        private async Task DeleteUser(string id)
        {
            var result = await ApiHelper.CallApiAsync<object>($"user/special/delete/{HttpUtility.UrlEncode(id)}", HttpMethod.Delete);

            if (result?.IsSuccess == true)
            {
                var users = await LoadAllUsers();
                users = users.Where(u => u.Id != id).ToList();
                Session[SessionUsersKey] = users;
            }
            return;
        }

        private void BindUserData(UserDto data)
        {
            if (data == null)
            {
                pnlUserInfo.Visible = false;
                return;
            }

            lblUserName.Text = HttpUtility.HtmlEncode(data.Username);
            lblEmail.Text = HttpUtility.HtmlEncode(data.Email);
            lblType.Text = HttpUtility.HtmlEncode(data.Type);

            bltRoles.DataSource = data.Roles.Select(r => r.Name) ?? Array.Empty<string>();
            bltRoles.DataBind();

            pnlUserInfo.Visible = true;
        }

        private void RedirectToUrl(string url)
        {
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void SetBtnVisiblity()
        {
            var permissions = new HashSet<string>(Session["Permissions"] as List<string> ?? new List<string>());
            btnCreateUser.Visible = permissions.Contains("CreateUser");
            btnAssignRole.Visible = permissions.Contains("AssignRole");
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            var myData = Session["MY_DATA"] as UserDto;
            RedirectToUrl($"User.aspx?id={HttpUtility.UrlEncode(myData.Id)}&self={true}");
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType != DataControlRowType.DataRow) return;

            if (_permissionSet == null)
                _permissionSet = new HashSet<string>(Session["Permissions"] as List<string> ?? new List<string>(), StringComparer.Ordinal);

            var btnUpdate = e.Row.FindControl("lnkUpdate") as LinkButton;
            if (btnUpdate != null)
            {
                btnUpdate.Visible = _permissionSet.Contains("UpdateUser");
            }

            var btnDelete = e.Row.FindControl("lnkDelete") as LinkButton;
            if (btnDelete != null)
            {
                btnDelete.Visible = _permissionSet.Contains("DeleteUser");
            }

            if (btnUpdate != null && btnDelete != null && !btnUpdate.Visible && !btnDelete.Visible)
            {
                btnUpdate.Parent.Visible = false;
            }
        }
    }
}