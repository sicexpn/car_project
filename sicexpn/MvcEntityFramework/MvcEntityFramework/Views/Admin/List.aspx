<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcEntityFramework.Models.student>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="background-image:url(../Content/list.png);background-repeat:no-repeat;padding-left:20px">用户列表</h2>

    <table>
        <tr>
            <th></th>
            
            <th>
                userName
            </th>
            <th>
                passWord
            </th>
            <th>
                Grade
            </th>
            <th>
                baseGrade
            </th>
            <th>
                是否及格
            </th>
            <th>
                gradeReport
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>  
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id=item.stuId }) %> |
                <%: Html.ActionLink("Details", "Details", new { id=item.stuId })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id=item.stuId })%>
            </td>
            
            <td>
                <%: item.userName %>
            </td>
            <td>
                <%: item.passWord %>
            </td>
            <td style="color:#990000">
                <%: item.Grade %>
            </td>
            <td>
                <%: item.baseGrade %>
            </td>
            <td style="background-color:#333366">
                <%if (item.Grade >= item.baseGrade)
                  { %>
                  是
                <%}
                  else
                  {%>
                  否
                <%} %>
            </td>
            <td>
                <%: Html.ActionLink("Reports","Reports",new{id=item.stuId}) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    
        <ul style="color:Black">
    
        <li><%: Html.ActionLink("Create New", "Create") %></li>

        <li style="background-image:url(../Content/back.png);background-repeat:no-repeat;padding-left:20px;"><%: Html.ActionLink("Back","Index","Admin") %></li>
        </ul>
    

</asp:Content>

