using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.App_Data
{
    public abstract class ProtectedPage : BasePage
    {
        /// <summary>
        /// Enter the resource name to check access to this page
        /// </summary>
        protected abstract string RESOURCE_NAME { get; set; }
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            bool isLogOn = Session["isLogOn"] as bool? ?? false;
            if (!isLogOn)
            {
                Response.Redirect("~/LoginPage.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            var cacheKey = $"PERMISSION_{RESOURCE_NAME}";
            bool? hasPermission = Session[cacheKey] as bool?;

            if (hasPermission == null)
            {
                var payload = new { resource = RESOURCE_NAME };

                var result = await ApiHelper.CallApiAsync<string>(
                    "role/permission/check", System.Net.Http.HttpMethod.Post, payload);

                if (result?.IsSuccess == true)
                {
                    hasPermission = true;
                    Session[cacheKey] = true;
                }
                else if (result?.IsSuccess == false)
                {
                    Session[cacheKey] = false;
                }
            }

            if (hasPermission != true)
            {
                Response.Redirect("~/UnauthorizedPage.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

        }
    }
}