<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Collect
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Collect-Data</h2>
    <div id="collect">
        <form id="form1" runat="server">
        <%using (Html.BeginForm())
          { %>
         <fieldset>
        <img  src="../../Content/man.png" />采集人：<input type="text" id ="collector" name="collector"/>
        <br /><br />
        <input type="submit" value="Collect" />
        </fieldset>
        <%} %>
        </form>
        <%--<input type="button" id="start" value="Start-Collect"  />
        <br /><br />
        <input type="button" id="end" value="End-Collect" />--%>
    </div>

</asp:Content>
