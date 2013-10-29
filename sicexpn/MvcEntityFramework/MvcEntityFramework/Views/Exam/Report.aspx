<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcEntityFramework.Models.report>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Report
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>成绩单</h2>
    <table >
        <tr>
            <th>姓名</th>
            <th>成绩</th>
            <th>及格分数线</th>
            <th>是否及格</th>
            <th>时间</th>
        </tr>
        <tr>
            <td><%:ViewData["name"] %></td>
            <td style="color:#990000"><%:ViewData["score"] %></td>
            <td><%:ViewData["baseGrade"] %></td>
            <td style="background-color:#333366"><%:ViewData["base"] %></td>
            <td><%:ViewData["date"] %></td>
        </tr>
    </table>
    <ul>
       <li> <%:Html.ActionLink("Detail Report","DetailReport") %></li>
       <li><%: Html.ActionLink("返回开始界面","Start") %></li>
    </ul>
</asp:Content>
