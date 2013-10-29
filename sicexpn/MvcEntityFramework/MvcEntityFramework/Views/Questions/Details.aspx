<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcEntityFramework.Models.question>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <fieldset>
        <legend>Fields</legend>
        
        
     <ul>
     <li><div class="display-label">Question</div></li> 
        <div class="display-field"><%: Model.Question %></div>
        
     <li><div class="display-label">A</div></li> 
        <div class="display-field"><%: Model.A %></div>
        
        <li><div class="display-label">B</div></li> 
        <div class="display-field"><%: Model.B %></div>
        
        <li><div class="display-label">C</div></li> 
        <div class="display-field"><%: Model.C %></div>
        
        <li><div class="display-label">D</div></li> 
        <div class="display-field"><%: Model.D %></div>
        
        <li><div class="display-label">Answers</div></li> 
        <div class="display-field"><%: Model.Answers %></div>
       </ul> 
    </fieldset>
    <ul>

      <li>  <%: Html.ActionLink("Edit", "Edit", new { id=Model.Id }) %> </li>
      <li style="background-image:url(../../Content/back.png);background-repeat:no-repeat;padding-left:20px;"> <%: Html.ActionLink("Back to List", "List") %></li>
    </ul>

</asp:Content>

