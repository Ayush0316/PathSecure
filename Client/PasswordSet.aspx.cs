using Client.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace Client
{
    public partial class PasswordSet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            setPasswordError.Visible = false;
        }

        protected async void btnSetPassword_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("btn clicked");
            if (!setPasswordSummary.ValidateAndShow())
                return;

            string password = txtPassword.Text;
            string confPassword = txtConfPassword.Text;

            if(!String.Equals(password, confPassword))
            {
                setPasswordError.Text = ResourceHelper.GetString("RequireSame");
                setPasswordError.Visible = true;
            }

            (string userId, string rawToken) = GetQueryParams();
            var result = await ApiHelper.CallApiAsync<string>("auth/setPassword", HttpMethod.Post, new { password, userId, rawToken });

            if (result?.IsSuccess == true)
            {
                RedirectToLogin();
            }
            else
            {
                var err = result?.Message ?? ResourceHelper.GetString("UnableToSet");
                setPasswordError.Text = err;
                setPasswordError.Visible = true;
            }
        }

        private (string userId, string rawToken) GetQueryParams()
        {
            var userId = Request.QueryString["UserId"];
            var rawToken = Request.QueryString["Token"];
            return (userId, rawToken);
        }

        private void RedirectToLogin()
        {
            Response.Redirect("~/LoginPage.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}