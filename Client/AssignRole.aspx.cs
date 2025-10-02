using Client.App_Data;
using Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class AssignRole : ProtectedPage
    {
        protected override string RESOURCE_NAME { get; set; } = "ASSIGN_ROLE";

        private const string ViewStateUsersKey = "AllUsersWithRoles";
        private const string ViewStateAllRolesKey = "AllRoles";

        protected async void Page_Load(object sender, EventArgs e)
        {
            bool isLogOn = Session["isLogOn"] as bool? ?? false;
            if (!isLogOn)
            {
                Response.Redirect("~/LoginPage.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                await LoadInitialDataAsync();
            }
            else
            {
                var roles = ViewState[ViewStateAllRolesKey] as List<RoleDto>;
                if (roles != null)
                {
                    BindRoles(roles); // rebuild identical structure
                }
            }
        }

        private async Task LoadInitialDataAsync()
        {
            lblResult.Visible = false;

            var usersResult = await ApiHelper.CallApiAsync<List<UserDto>>("user/special/get", HttpMethod.Get);
            var rolesResult = await ApiHelper.CallApiAsync<List<RoleDto>>("role/special/get", HttpMethod.Get);

            if (usersResult?.IsSuccess != true || rolesResult?.IsSuccess != true)
            {
                lblResult.Text = ResourceHelper.GetString("LoadFailed");
                lblResult.CssClass = "text-danger";
                lblResult.Visible = true;
                return;
            }

            ViewState[ViewStateUsersKey] = usersResult.Body;
            ViewState[ViewStateAllRolesKey] = rolesResult.Body;

            BindUsers(usersResult.Body);
            BindRoles(rolesResult.Body);
        }

        private void BindUsers(List<UserDto> users)
        {
            ddlUsers.Items.Clear();
            ddlUsers.Items.Add(new ListItem(ResourceHelper.GetString("SelectUserDefault"), ""));
            foreach (var u in users.OrderBy(x => x.Username))
            {
                ddlUsers.Items.Add(new ListItem(u.Username, u.Id));
            }
        }

        protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblResult.Visible = false;
            pnlRoles.Visible = false;

            if (string.IsNullOrEmpty(ddlUsers.SelectedValue))
                return;

            var users = ViewState[ViewStateUsersKey] as List<UserDto>;
            var roles = ViewState[ViewStateAllRolesKey] as List<RoleDto>;

            if (users == null || roles == null)
                return;

            string userId = ddlUsers.SelectedValue;

            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return;

            CheckUserRoles(user.Roles ?? new List<RoleDto>());
            pnlRoles.Visible = true;
        }

        private void BindRoles(IEnumerable<RoleDto> allRoles)
        {
            phRoles.Controls.Clear();

            foreach (var role in allRoles.OrderBy(r => r.Name))
            {
                var cb = new CheckBox
                {
                    ID = role.Id,
                    Checked = false,
                    CssClass = "form-check-input",
                };

                var wrapper = new Panel { CssClass = "form-check" };
                var label = new Label
                {
                    AssociatedControlID = cb.ID,
                    Text = role.Name,
                    CssClass = "form-check-label"
                };

                wrapper.Controls.Add(cb);
                wrapper.Controls.Add(label);
                phRoles.Controls.Add(wrapper);
            }
        }

        private void CheckUserRoles(IEnumerable<RoleDto> userRoles)
        {
            var userRoleSet = new HashSet<string>(userRoles.Select(ur => ur.Id) ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);

            foreach (Control c in phRoles.Controls)
            {
                if (c is Panel panel)
                {
                    foreach (Control inner in panel.Controls)
                    {
                        if (inner is CheckBox cb)
                        {
                            cb.Checked = userRoleSet.Contains(cb.ID);
                        }
                    }
                }
            }
        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            lblResult.Visible = false;

            if (string.IsNullOrEmpty(ddlUsers.SelectedValue))
            {
                lblResult.Text = ResourceHelper.GetString("UserSelectRequired");
                lblResult.CssClass = "text-danger";
                lblResult.Visible = true;
                return;
            }

            string userId = ddlUsers.SelectedValue;

            var selectedRoles = GetSelectedRoles();
            var payload = new
            {
                userId = userId,
                roleId = selectedRoles.Select(sr => sr.Id).ToList()
            };

            var assignResult = await ApiHelper.CallApiAsync<string>("user/assign-role", HttpMethod.Post, payload);

            if (assignResult?.IsSuccess == true)
            {
                UpdateCachedUserRoles(userId, selectedRoles);
                lblResult.Text = ResourceHelper.GetString("RolesUpdated");
                lblResult.CssClass = "text-success";
                lblResult.Visible = true;
            }
            else
            {
                lblResult.Text = assignResult?.Message ?? ResourceHelper.GetString("RolesUpdateFailed");
                lblResult.CssClass = "text-danger";
                lblResult.Visible = true;
            }
        }

        private List<RoleDto> GetSelectedRoles()
        {
            var roles = new List<RoleDto>();
            foreach (Control c in phRoles.Controls)
            {
                if (c is Panel panel)
                {
                    foreach (Control inner in panel.Controls)
                    {
                        if (inner is CheckBox cb && cb.Checked)
                        {
                            roles.Add(new RoleDto { Id = cb.ID, Name = cb.Text });
                        }
                    }
                }
            }
            return roles;
        }

        private void UpdateCachedUserRoles(string userId, List<RoleDto> roles)
        {
            var users = ViewState[ViewStateUsersKey] as List<UserDto>;
            if (users == null) return;
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Roles = roles;
            }
            ViewState[ViewStateUsersKey] = users;
        }
    }
}
