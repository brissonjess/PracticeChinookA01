<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MessageUserControl.ascx.cs" Inherits="UserControls_MessageUserControl" %>
<%-- a panel is a way to group a bunch of controls together --%>
<asp:Panel ID="MessagePanel" runat="server">
    <div class="panel-heading">
        <asp:Label ID="MessageTitleIcon" runat="server"> </asp:Label>
        <asp:Label ID="MessageTitle" runat="server" />
    </div>
    <div class="panel-body">
        <asp:Label ID="MessageLabel" runat="server" />
        <asp:Repeater ID="MessageDetailsRepeater" runat="server" EnableViewState="false">
            <%-- note these can be put in any order --%>
            <%-- create header with opening list tag --%>
            <headertemplate>
                <ul>
            </headertemplate>
            <%-- create list items for error messages --%>
            <itemtemplate>
                <li><%# Eval("Error") %></li>
            </itemtemplate>
            <%-- create footer with closing list tag --%>
            <footertemplate>
                </ul>
            </footertemplate>
            
        </asp:Repeater>
    </div>
</asp:Panel>