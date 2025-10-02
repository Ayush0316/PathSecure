using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Client.Controls
{
    public partial class LinkValidationSummary : System.Web.UI.UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.DesignMode)
                this.DataBind();
        }
        public class ValidationItem
        {
            public string ErrorMessage { get; set; }
            public string ControlClientID { get; set; }
        }

        /// <summary> Validation group to filter errors. Blank = default group. </summary>
        public string ValidationGroup { get; set; }

        /// <summary> Optional header text above the error list. </summary>
        public string HeaderTextKey { get; set; }

        public string HeaderText { get; set; }

        /// <summary> Show or hide the header text. </summary>
        public bool ShowHeader { get; set; } = true;

        /// <summary> If true, also shows a JavaScript alert with errors. </summary>
        public bool ShowMessageBox { get; set; } = false;


        /// <summary>
        /// Optional CSS class applied to the outer container of the validation summary.
        /// </summary>
        public string SummaryCssClass
        {
            get => pnlContainer.CssClass;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    pnlContainer.CssClass = value;
                }
            }
        }

        /// <summary>
        /// Displays a summary of validation errors for a specified validation group,
        /// with options to show a header, display errors in a message box, and link errors to controls.
        /// </summary>
        public bool ValidateAndShow()
        {
            if (Page == null) return false;

            Page.Validate(this.ValidationGroup);
            bool isValid = Page.IsValid;

            if (!isValid)
            {
                ShowErrors();  // reuse the existing method
            }
            else
            {
                pnlContainer.Visible = false;
            }

            return isValid;
        }


        /// <summary>
        /// Collects failed validators for the specified group and displays them.
        /// Call after Page.Validate().
        /// </summary>
        public void ShowErrors()
        {
            if (Page == null) return;

            var failed = Page.Validators
                .OfType<BaseValidator>()
                .Where(v => !v.IsValid &&
                            string.Equals(v.ValidationGroup ?? string.Empty,
                                          this.ValidationGroup ?? string.Empty,
                                          StringComparison.OrdinalIgnoreCase))
                .Select(v => new ValidationItem
                {
                    ErrorMessage = v.ErrorMessage,
                    ControlClientID = !string.IsNullOrEmpty(v.ControlToValidate)
                        ? FindControlClientId(v.ControlToValidate, v.NamingContainer)
                        : Page.ClientID
                })
                .ToList();

            pnlContainer.Visible = failed.Count > 0;

            if (ShowHeader && failed.Count > 0)
                litHeader.Text = $"<p>{ResourceHelper.GetString(HeaderTextKey) ?? HeaderText}</p>";
            else
                litHeader.Text = string.Empty;

            rptErrors.DataSource = failed;
            rptErrors.DataBind();

            if (ShowMessageBox && failed.Count > 0)
            {
                var jsErrors = string.Join("\\n", failed.Select(f => f.ErrorMessage));
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "ValidationAlert" + this.ClientID,
                    $"alert('{jsErrors}');", true);
            }
        }

        private string FindControlClientId(string controlId, Control v)
        {
            var ctrl = v.FindControl(controlId);
            return ctrl != null ? ctrl.ClientID : string.Empty;
        }
    }
}

