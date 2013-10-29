<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcEntityFramework.Models.question>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="background-image:url(../Content/list.png);background-repeat:no-repeat;padding-left:20px">题库列表</h2>

    <table>
        <tr>
            <th></th>
            
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
                Answers
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td style="width:150px">
                <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
                <%: Html.ActionLink("Details", "Details", new { id=item.Id })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.Id })%>
            </td>
            
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
            <td>
                <%: item.Answers %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <ul style="color:Black">
    
        <li><%: Html.ActionLink("Create New", "Create") %></li>

        <li style="background-image:url(../Content/back.png);background-repeat:no-repeat;padding-left:20px;"><%: Html.ActionLink("Back", "Index", "Admin")%></li>
        
     </ul>
     
</asp:Content>

