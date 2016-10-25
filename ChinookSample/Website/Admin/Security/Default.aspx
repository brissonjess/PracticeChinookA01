<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
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
                    <%-- Data Key Names contain the considered PKey field
                            of the class that is being used in insert, update, or delete 

                        RefreshAll() will call a generic method in my code behind
                            that will cause the ODS sets to rebind their data - refresh data in controls
                    --%>
                    <asp:ListView ID="RoleListView"
                        runat="server"
                        DataSourceID="RoleListViewODS"
                        ItemType="ChinookSystem.Security.RoleProfile"
                        InsertItemPosition="LastItem"
                        DataKeyNames="RoleId"
                        OnItemInserted="RefreshAll"
                        OnItemDeleted="RefreshAll">
                        <%-- min  #  of templates when doing  a DB read and update --%>
                        <EmptyDataTemplate>
                            <span>No security roles have been set up.</span>
                        </EmptyDataTemplate>
                        <%-- title area --%>
                        <LayoutTemplate>
                            <div class="row bginfo">
                                <div class="col-sm-3 h4">Action</div>
                                <div class="col-sm-3 h4">Role name</div>
                                <div class="col-sm-6 h4">Users</div>
                            </div>

                            <div class="row" id="ItemPlaceholder" runat="server">
                            </div>

                        </LayoutTemplate>
                        <%-- item area --%>
                        <ItemTemplate>
                            <div class="row">
                                <div class="col-sm-3">
                                    <%-- delete link button --%>
                                    <asp:LinkButton ID="RemoveRole" 
                                                    runat="server" 
                                                    CommandName="Delete">Remove</asp:LinkButton>
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
                            </div>
                        </ItemTemplate>
                        <%-- since we are also doing insert you will need insert template--%>
                        <InsertItemTemplate>
                            <div class="row">
                                <div class="col-sm-3">
                                    <%-- insert / cancel link buttons --%>
                                    <asp:LinkButton ID="InsertRole" 
                                                    runat="server" 
                                                    CommandName="Insert">Insert</asp:LinkButton>&nbsp&nbsp&nbsp
                                <asp:LinkButton ID="Cancel" runat="server">Cancel</asp:LinkButton>
                                </div>
                                <div class="col-sm-3">
                                    <%-- new role name --%>
                                    <asp:TextBox ID="RoleName" runat="server" Text='<%# BindItem.RoleName %>' placeholder="Role Name"></asp:TextBox>
                                </div>
                            </div>
                        </InsertItemTemplate>
                    </asp:ListView>
                    <asp:ObjectDataSource ID="RoleListViewODS" runat="server"
                        DataObjectTypeName="ChinookSystem.Security.RoleProfile"
                        DeleteMethod="RemoveRole"
                        InsertMethod="AddRole"
                        OldValuesParameterFormatString="original_{0}"
                        SelectMethod="ListAllRoles"
                        TypeName="ChinookSystem.Security.RoleManager"></asp:ObjectDataSource>
                </div>
                <%-- Unregistered tab --%>
                <div class="tab-pane fade" id="unregistered">
                    <asp:GridView ID="UnregisteredUsersGridView" runat="server" 
                        AutoGenerateColumns="False" 
                        DataSourceID="UnregisteredUsersODS"
                         DataKeyNames="UserId"
                         ItemType="ChinookSystem.Security.UnRegisteredUserProfile" OnSelectedIndexChanging="UnregisteredUsersGridView_SelectedIndexChanging">
                        <Columns>
                            <asp:CommandField SelectText="Register" ShowSelectButton="True"></asp:CommandField>
                            <asp:BoundField DataField="UserType" HeaderText="UserType" SortExpression="UserType"></asp:BoundField>
                            <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName"></asp:BoundField>
                            <asp:BoundField DataField="Lastname" HeaderText="Lastname" SortExpression="Lastname"></asp:BoundField>
                            <asp:TemplateField HeaderText="AssignedUserName" SortExpression="AssignedUserName">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("AssignedUserName") %>' 
                                        ID="AssignedUserName"></asp:TextBox>
                                </ItemTemplate>
                               
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AssignedEmail" SortExpression="AssignedEmail">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# Bind("AssignedEmail") %>' 
                                        ID="AssignedEmail"></asp:TextBox>
                                </ItemTemplate>
                               
                            </asp:TemplateField>
                            <EmptyDataTemplate>
                                No registered users to process.
                            </EmptyDataTemplate>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="UnregisteredUsersODS" runat="server" 
                        OldValuesParameterFormatString="original_{0}" 
                        SelectMethod="ListAllUnRegisteredUsers" 
                        TypeName="ChinookSystem.Security.UserManager">
                    </asp:ObjectDataSource>
                </div>
                <%-- \ unregistered pane --%>
            </div>
            <div class="tab-content"></div>
            <div class="tab-content"></div>
        </div>
    </div>
</asp:Content>

