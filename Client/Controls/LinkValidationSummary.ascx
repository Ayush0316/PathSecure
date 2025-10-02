<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkValidationSummary.ascx.cs" Inherits="Client.Controls.LinkValidationSummary" %>
<link href="/Styles/LinkValidationSummary.css" rel="stylesheet" type="text/css" />

<asp:Panel ID="pnlContainer" runat="server" Visible="false" CssClass="validation-summary">
    <asp:Literal ID="litHeader" runat="server" EnableViewState="false" />
    <asp:Repeater ID="rptErrors" runat="server">
        <HeaderTemplate><ul></HeaderTemplate>
        <ItemTemplate>
            <li>
                <a href="javascript:void(0);" onclick="focusField('<%# Eval("ControlClientID") %>')">
                    <%# Eval("ErrorMessage") %>
                </a>
            </li>
        </ItemTemplate>
        <FooterTemplate></ul></FooterTemplate>
    </asp:Repeater>
</asp:Panel>

<script type="text/javascript">
    function focusField(controlId) {
        //console.log("Focus field fired with id " + controlId);
        var el = document.getElementById(controlId);
        if (el) {
            el.scrollIntoView({ behavior: 'smooth', block: 'center' });
            el.focus();
        }
    }
</script>
