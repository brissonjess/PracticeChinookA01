<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManagePlayList.aspx.cs" Inherits="BusinessProcess_ManagePlayList" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="jumbotron">
        <h1>Manage Playlist</h1>
    </div>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

    <div class="row">
        <asp:DropDownList ID="ArtistList" runat="server"></asp:DropDownList>
        <asp:Label ID="Label1" runat="server" Text="Artist"></asp:Label>
        <asp:Button ID="ArtistFetch" runat="server" Text="List" CssClass="btn btn-primary" />
    </div>
    <asp:ObjectDataSource ID="ArtistListODS" runat="server"></asp:ObjectDataSource>
</asp:Content>

