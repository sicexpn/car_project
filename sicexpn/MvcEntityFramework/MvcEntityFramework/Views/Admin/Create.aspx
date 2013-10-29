<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcEntityFramework.Models.student>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="background-image:url(../Content/add.png);background-repeat:no-repeat;padding-left:20px">Create</h2>
    <div style="color:Red">
        <%:TempData["name"] %>
    </div>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Fields</legend>
            
            
            
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
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div style="background-image:url(../Content/back.png);background-repeat:no-repeat;padding-left:20px;">
        <%: Html.ActionLink("Back to List", "List") %>
    </div>

</asp:Content>

