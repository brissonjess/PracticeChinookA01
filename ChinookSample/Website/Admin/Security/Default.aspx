<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row jumbotron">
        <h1>User and Role Administration</h1>
    </div>
    <div class="row">
        <div class="col-md-12">
            <%-- Nav Tabs --%>
            <ul class="nav nav-tabs">
                <li class="active"><a href="#users" data-toggle="tab">Users</a></li>
                <li><a href="#roles" data-toggle="tab">Roles</a></li>
                <li><a href="#unregistered" data-toggle="tab">UnRegistered Users</a></li>
            </ul>
            <%-- tab content area --%>
            <div class="tab-content">
                <%-- User tab --%>
                <div class="tab-pane fade-in active" id="users">
                </div>
                <%-- Roles tab --%>
                <div class="tab-pane fade" id="roles">
                    <asp:ListView ID="RoleListView" runat="server" DataSourceID="RoleListViewODS" ItemType="RoleProfile" InsertItemPosition="LastItem">
                        <%-- min  #  of templates when doing  a DB read and update --%>
                        <EmptyDataTemplate>
                            <span>No security roles have been set up.</span>
                        </EmptyDataTemplate>
                        <%-- title area --%>
                        <LayoutTemplate>
                            <div class="col-sm-3 h4">Action</div>
                            <div class="col-sm-3 h4">Role name</div>
                            <div class="col-sm-6 h4">Users</div>
                        </LayoutTemplate>
                        <%-- item area --%>
                        <ItemTemplate>
                            <div class="col-sm-3">
                                <%-- delete link button --%>
                                <asp:LinkButton ID="RemoveRole" runat="server">Remove</asp:LinkButton>
                            </div>
                            <div class="col-sm-3">
                                <%-- get the role name --%>
                                <%# Item.RoleName %>
                            </div>
                            <div class="col-sm-6">
                                <%-- use a repeater to get a list of users --%>
                                <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# Item.UserNames %>' ItemType="System.String">
                                    <ItemTemplate>
                                         <%# Item %>
                                    </ItemTemplate>
                                    <%-- defines a character to seperate your items. can put any character you want --%>
                                    <SeparatorTemplate>, </SeparatorTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                        <%-- since we are also doing insert you will need insert template--%>
                        <InsertItemTemplate>
                            <div class="col-sm-3">
                                <%-- insert / cancel link buttons --%>
                                <asp:LinkButton ID="InsertRole" runat="server">Insert</asp:LinkButton>&nbsp&nbsp&nbsp
                                <asp:LinkButton ID="Cancel" runat="server">Cancel</asp:LinkButton>
                            </div>
                            <div class="col-sm-3">
                                <%-- new role name --%>
                                <asp:TextBox ID="RoleName" runat="server" Text='<%# BindItem.RoleName %>' placeholder="Role Name"></asp:TextBox>
                            </div>
                        </InsertItemTemplate>
                    </asp:ListView>
                    <asp:ObjectDataSource ID="RoleListViewODS" runat="server">

                    </asp:ObjectDataSource>
                </div>
                <%-- Unregistered tab --%>
                <div class="tab-pane fade" id="unregistered">
                </div><%-- \ unregistered pane --%>
            </div>
            <div class="tab-content"></div>
            <div class="tab-content"></div>
        </div>
    </div>
</asp:Content>

