using Client.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Client.App_Data
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            var culture = Session["Culture"] as string ?? "en";
            System.Threading.Thread.CurrentThread.CurrentCulture =
                new System.Globalization.CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture =
                new System.Globalization.CultureInfo(culture);
            base.InitializeCulture();
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.DataBind(); // ensures all <%# %> bindings run
            base.OnPreRender(e);
        }
        public Popup GlobalPopup
        {
            get
            {
                var master = this.Master as SiteMaster;
                return master?.PopupControl;
            }
        }
    }
}