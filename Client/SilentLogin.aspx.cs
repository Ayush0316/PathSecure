using Client.App_Data;
using Client.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Security;

namespace Client
{
    public partial class SilentLogin : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            string password = Request.QueryString["password"];
            string userId = Request.QueryString["userId"];
            string token = Request.QueryString["token"];

            bool namePass = !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
            bool idToken = !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(token);

            if (!namePass && !idToken)
            {
                Session["SILENT_LOGIN_ERROR"] = "Missing parameters in login request";
                RedirectToUrl("~/LoginPage.aspx");
                return;
            }

            var result = await ApiHelper.CallApiAsync<string>("auth/silentLogin", HttpMethod.Post, new {username,password,userId,token});
            
            if (result?.IsSuccess == true)
            {
                string returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    RedirectToUrl(returnUrl);
                }
                else
                {
                    RedirectToUrl("~/Dashboard.aspx");
                }
            }
            else
            {
                var err = result?.Message ?? "Some error occured!";
                Session["SILENT_LOGIN_ERROR"] = err;
                RedirectToUrl("~/LoginPage.aspx");
            }
        }

        private void RedirectToUrl(string url)
        {
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
