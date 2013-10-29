<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SuperUser
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>SuperUser</h2>
    
    <ul>
    <li><%:Html.ActionLink("用户管理","List","Admin") %></li>
    
    <li><%:Html.ActionLink("题库管理","List","Questions") %></li>
    </ul>
    
</asp:Content>
