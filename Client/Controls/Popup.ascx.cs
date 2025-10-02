using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client.Controls
{
    public partial class Popup : System.Web.UI.UserControl
    {
        /// <summary>
        /// Show the popup with a message and color.
        /// durationMs = how long (ms) it remains visible.
        /// </summary>
        public void Show(string message, bool isSuccess, int durationMs)
        {
            lblMessage.Text = message;
            pnlPopup.CssClass = "popup " + (isSuccess ? "success" : "error");
            pnlPopup.Visible = true;

            // Auto-hide using client-side timer
            string script = $@"
            setTimeout(function() {{
                var el = document.getElementById('{pnlPopup.ClientID}');
                if(el) el.style.display = 'none';
            }}, {durationMs});
        ";

            Page.ClientScript.RegisterStartupScript(this.GetType(),
                Guid.NewGuid().ToString(), script, true);
        }
    }
}