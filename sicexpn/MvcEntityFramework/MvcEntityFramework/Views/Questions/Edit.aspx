﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcEntityFramework.Models.question>" %>

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
                <%: Html.LabelFor(model => model.Question) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Question) %>
                <%--<input type="text" id="Question" class="Question" readonly="readonly" value="<%: Model.Question %>" />--%>
                <%: Html.ValidationMessageFor(model => model.Question) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.A) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.A) %>
                <%: Html.ValidationMessageFor(model => model.A) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.B) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.B) %>
                <%: Html.ValidationMessageFor(model => model.B) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.C) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.C) %>
                <%: Html.ValidationMessageFor(model => model.C) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.D) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.D) %>
                <%: Html.ValidationMessageFor(model => model.D) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Answers) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Answers) %>
                <%: Html.ValidationMessageFor(model => model.Answers) %>
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

