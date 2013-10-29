<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcEntityFramework.Models.report>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DetailReport
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%:Model.First().stuName %>的详细成绩单</h2>

    <table>
        <tr>
            
            
            <th>
                Question
            </th>
            <th>
                A
            </th>
            <th>
                B
            </th>
            <th>
                C
            </th>
            <th>
                D
            </th>
            <th>
                CorrectAnswer
            </th>
            <th>
                UserAnswer
            </th>
            <th>
                Grade
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            
            <td>
                <%: item.Question %>
            </td>
            <td>
                <%: item.A %>
            </td>
            <td>
                <%: item.B %>
            </td>
            <td>
                <%: item.C %>
            </td>
            <td>
                <%: item.D %>
            </td>
            <td style="color:#990000">
                <%: item.CorrectAnswer %>
            </td>
            <td>
                <%: item.UserAnswer %>
            </td>
            <td>
                <%: String.Format("{0:F}", item.Grade) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p style="background-image:url(../../Content/back.png);background-repeat:no-repeat;padding-left:20px;">
        <%: Html.ActionLink("back", "Report") %>
    </p>

</asp:Content>

