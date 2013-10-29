<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcEntityFramework.Models.student>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="background-image:url(../../Content/pencil.png);background-repeat:no-repeat;padding-left:20px">Edit</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        
        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.stuId) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.stuId) %>
                <%: Html.ValidationMessageFor(model => model.stuId) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.userName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.userName) %>
                <%: Html.ValidationMessageFor(model => model.userName) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.passWord) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.passWord) %>
                <%: Html.ValidationMessageFor(model => model.passWord) %>
            </div>

             <div class="editor-label">
                <%: Html.LabelFor(model => model.baseGrade) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.baseGrade)%>
                <%: Html.ValidationMessageFor(model => model.baseGrade)%>
            </div>

             <div class="editor-label">
                <%: Html.LabelFor(model => model.Grade) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Grade)%>
                <%: Html.ValidationMessageFor(model => model.Grade)%>
            </div>
            
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div style="background-image:url(../../Content/back.png);background-repeat:no-repeat;padding-left:20px;">
        <%: Html.ActionLink("Back to List", "List") %>
    </div>

</asp:Content>

