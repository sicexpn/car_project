<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcEntityFramework.Models.student>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        
        
      <ul>
       <li> <div class="display-label">userName</div></li>
        <div class="display-field"><%: Model.userName %></div>
        
        <li><div class="display-label">passWord</div></li>
        <div class="display-field"><%: Model.passWord %></div>
        
       <li> <div class="display-label">baseGrade</div></li>
        <div class="display-field"><%: String.Format("{0:F}", Model.baseGrade) %></div>
        
        <li><div class="display-label">Grade</div></li>
        <div class="display-field"><%: String.Format("{0:F}", Model.Grade) %></div>
        
       <li> <div class="display-label">是否及格</div></li>
        <div class="display-field">
        <%if (Model.baseGrade <= Model.Grade)
          { %>
                 是             
        <%}
          else
          {%>
          否
        <%} %>
        </div>
       </ul>  
    </fieldset>
    <ul>

      <li>  <%: Html.ActionLink("Edit", "Edit", new { id=Model.stuId }) %> </li>
      <li style="background-image:url(../../Content/back.png);background-repeat:no-repeat;padding-left:20px;"> <%: Html.ActionLink("Back to List", "List") %></li>
    </ul>

</asp:Content>

