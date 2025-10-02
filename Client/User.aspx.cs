using Client.App_Data;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using WebGrease.Activities;

namespace Client
{
    public partial class CreateUser : ProtectedPage
    {
        protected override string RESOURCE_NAME { get; set; } = "CREATE_USER";
        private bool isEditMode = false;
        private bool isSelf = false;
        private string editUserId = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            string userId = Request.QueryString["id"];
            if (userId == null)
            {
                isEditMode = false;
                btnCreateUser.Text = ResourceHelper.GetString("CreateUser");
                litTitle.Text = ResourceHelper.GetString("CreateUser");
            }
            else
            {
                RESOURCE_NAME = "SPECIAL_EDIT_USER";
                litTitle.Text = ResourceHelper.GetString("EditUser");
                btnCreateUser.Text = ResourceHelper.GetString("EditUser");
                ddlUserType.Enabled = false;
                isEditMode = true;
                editUserId = userId;
                bool.TryParse(Request.QueryString["self"], out isSelf);
                if (isSelf)
                {
                    RESOURCE_NAME = "EDIT_USER";
                    lblConPassword.Visible = true;
                    lblPassword.Visible = true;
                    txtConfPassword.Visible = true;
                    txtPassword.Visible = true;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            createUserError.Visible = false;
            if (!IsPostBack)
            {
                ddlUserType.Items.Clear();
                ddlUserType.Items.Add(new ListItem(ResourceHelper.GetString("UserTypeRegular"), "REGULAR"));
                ddlUserType.Items.Add(new ListItem(ResourceHelper.GetString("UserTypeOAuth2"), "OAUTH2"));
                ddlUserType.Items.Add(new ListItem(ResourceHelper.GetString("UserTypeSilent"), "SILENT"));
                if(isEditMode)
                {
                    LoadUserData(editUserId);
                }
            }
        }
        protected async void btnCreateUser_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("btn clicked");
            if (!createUserSummary.ValidateAndShow())
                return;

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text;
            string type = ddlUserType.Text;
            string password = null;

            if (isEditMode && isSelf)
            {
                string ps = txtPassword.Text;
                string cps = txtConfPassword.Text;
                if (!String.IsNullOrEmpty(ps) && !String.Equals(ps, cps))
                {
                    createUserError.Text = ResourceHelper.GetString("RequireSame");
                    createUserError.Visible = true;
                } else if (!String.IsNullOrEmpty(ps))
                {
                    password = ps;
                }
            }

            ResponseDto<string> result;
            if (isEditMode && isSelf)
            {
                result = await ApiHelper.CallApiAsync<string>("user/update", HttpMethod.Put, new { userId = editUserId, username, email, password});
            } else if (isEditMode)
            {
                result = await ApiHelper.CallApiAsync<string>("user/special/update", HttpMethod.Put, new { userId = editUserId, username, email });
            }
            else
                result = await ApiHelper.CallApiAsync<string>("user/special/create", HttpMethod.Post, new { username, email, type });

            if (result?.IsSuccess == true)
            {
                UpdateSessionState(editUserId, username, email);
                Redirect();
            }
            else
            {
                createUserError.Text = result?.Message ?? ResourceHelper.GetString("CreateErr");
                createUserError.Visible = true;
            }
        }

        private void Redirect()
        {
            Response.Redirect("~/Dashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private async void LoadUserData(string userId)
        {
            var result = await ApiHelper.CallApiAsync<UserDto>($"user/get/{HttpUtility.UrlEncode(userId)}", HttpMethod.Get);
            if(result?.IsSuccess == true)
            {
                var data = result.Body;
                txtUsername.Text = data.Username;
                txtEmail.Text = data.Email;
                ddlUserType.SelectedValue = data.Type;
            }
            else
            {
                createUserError.Text = result?.Message ?? ResourceHelper.GetString("LoadUserErr");
                createUserError.Visible = true;
                btnCreateUser.Enabled = false;
            }
        }

        private void UpdateSessionState(string userId, string username, string email)
        {
            if (isEditMode)
            {
                var users = Session["AllUsersWithRoles"] as List<UserDto>;
                if (users == null) return;

                var user = users.Find(u => u.Id == userId);
                if(user == null)
                {
                    Session.Remove("AllUsersWithRoles");
                    return;
                }
                users.Remove(user);
                user.Username = username;
                user.Email = email;
                users.Add(user);

                if (isSelf)
                    Session["MY_DATA"] = user;
                
                return;
            }else
            {
                Session.Remove("AllUsersWithRoles");
                return;
            }
        }
    }
}