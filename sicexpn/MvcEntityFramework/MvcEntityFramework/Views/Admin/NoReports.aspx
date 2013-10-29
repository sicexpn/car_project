<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	NoReports
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>没有此用户的成绩单</h2>
    <p>
        此用户没有进行考试
    </p>
    <p style="background-image:url(../Content/back.png);background-repeat:no-repeat;padding-left:20px;">
        <%:Html.ActionLink("back","List") %>
    </p>
</asp:Content>
