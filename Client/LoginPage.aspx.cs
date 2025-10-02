using Client.App_Data;
using Client.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class LoginPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            loginError.Visible = false;
            if (!IsPostBack)
            {
                bool isLogOn = Session["isLogOn"] as bool? ?? false;
                if (isLogOn)
                {
                    RedirectToDashboard();
                }
                var silentErr = Session["SILENT_LOGIN_ERROR"] as string;
                if(!String.IsNullOrEmpty(silentErr)){
                    loginError.Text = silentErr;
                    loginError.Visible = true;
                }
            }

        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("btn clicked");
            if (!loginSummary.ValidateAndShow())
                return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            var result = await ApiHelper.CallApiAsync<string>("auth/login", HttpMethod.Post,new { username, password });

            if (result?.IsSuccess == true)
            {
                SetRolesPermissions();
                RedirectToDashboard();
            }
            else
            {
                loginError.Text = result?.Message ?? ResourceHelper.GetString("LoginError");
                loginError.Visible = true;
            }
        }

        private void RedirectToDashboard()
        {
            Response.Redirect("~/Dashboard.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void SetRolesPermissions()
        {
            string accessToken = ApiHelper.GetAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                System.Diagnostics.Debug.WriteLine("token not found");
                return;
            }

            string[] tokenParts = accessToken.Split('.');
            if (tokenParts.Length != 3)
                return;

            string payload = tokenParts[1];
            
            int mod4 = payload.Length % 4;
            if (mod4 > 0)
                payload += new string('=', 4 - mod4);

            string jsonPayload = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));

            var claims = Newtonsoft.Json.Linq.JObject.Parse(jsonPayload);
            Session["UserId"] = claims.GetValue("UserId").ToString();
            Session["Roles"] = GetClaims(ClaimTypes.Role.ToString(), claims);
            Session["Permissions"] = GetClaims("UserPermission", claims);
        }

        private List<string> GetClaims(string name, JObject claims){
            var token = claims[name];
            List<string> values;
            if (token is JArray)
                values = token.ToObject<List<string>>();
            else if (token != null)
                values = new List<string> { token.ToString() };
            else
                values = new List<string>();
            return values;
        }
    }
}