<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcApplication界面设计.Models.Info>>"%>

<%--<%@ Import Namespace="MvcApplication界面设计.Models" %>--%>
<script runat="server">

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Remote
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    

    <h2>Remote-Control smart car</h2>
  <div id="cmd_car" style=" margin-left:20px">
  <h1 id="up" style=" margin-left:50px">
        <%:Html.ActionLinkWithImage(Url.Content("../../Content/up.png"), "Up", "Car")%>
  </h1>
  <h1 id="left" style="margin-top:0px">
        <%:Html.ActionLinkWithImage(Url.Content("../../Content/left.png"), "Left", "Car")%>    
  </h1>
  <h1 id="stop" style="margin-left:50px; margin-top:-60px">
        <%:Html.ActionLinkWithImage(Url.Content("../../Content/stop.png"), "Stop", "Car")%>
  </h1>
  <h1 id="right" style="margin-left:100px; margin-top:-60px">
        <%:Html.ActionLinkWithImage(Url.Content("../../Content/right.png"), "Right", "Car")%>
   </h1>
   <h1 id="down" style="margin-left:50px; margin-top:-5px">
        <%:Html.ActionLinkWithImage(Url.Content("../../Content/down.png"), "Down", "Car")%>
   </h1>
   </div> 
   <h1>
    <img alt="" src="../../Content/car.jpg" />
   </h1>
   
       
           
     
</asp:Content>