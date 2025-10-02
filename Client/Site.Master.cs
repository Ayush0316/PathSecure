using Client.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client
{
    public partial class SiteMaster : MasterPage
    {
        public Popup PopupControl => SitePopup;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var languages = new List<string> { "English", "Hindi", "Punjabi" };
                var languageDropDown = this.FindControl("language") as DropDownList;
                if (languageDropDown != null)
                {
                    PopulateDropdown(languageDropDown, languages);
                }
            }
            bool login = Session["isLogOn"] as bool? ?? false;
            if (login) logoutBtn.Visible = true;
            logoutBtn.Text = ResourceHelper.GetString("LoginPage", "Logout");
        }
        protected void PopulateDropdown(DropDownList ddl, IEnumerable<string> items)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("English", "en"));
            ddl.Items.Add(new ListItem("Hindi", "hi"));
            ddl.Items.Add(new ListItem("French", "fr"));

            string currentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            if (ddl.Items.FindByValue(currentCulture) != null)
                ddl.SelectedValue = currentCulture;
        }
        protected void Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            var languageDropDown = sender as DropDownList;
            if (languageDropDown != null)
            {
                string selectedLanguage = languageDropDown.SelectedValue;
                System.Diagnostics.Debug.WriteLine(selectedLanguage);

                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(selectedLanguage);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(selectedLanguage);

                // Store selection in session (optional) to persist across pages
                Session["Culture"] = selectedLanguage;

                // Reload the page to apply the new culture
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }

        protected async void logoutPressed(object sender, EventArgs e)
        {
            var res = await ApiHelper.CallApiAsync<string>("auth/logout", HttpMethod.Post);
            Session.Clear();
            Response.Redirect("~/LoginPage.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}