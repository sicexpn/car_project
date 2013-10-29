
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcApplication界面设计.Models.Info>>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Read
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Read-Data</h2>
   <div id="read">
   <form id="form1" runat="server">
        <%using (Html.BeginForm())
          { %>
         <fieldset>
        <img  src="../../Content/man.png" />采集人：<input id="collector" name="collector" type="text" value="xingpanning" /><br />
        <img  style="height:30px;width:35px;margin-left:2px;margin-right:18px;margin-bottom:-2px" src="../../Content/date.png" />
        日期：
        
        <input type="text" name="d_s" size="20" value="" id="begin_date_b"><input type="reset" value="..."
        onclick="return showCalendar('begin_date_b', 'y-m-d');">
<br />
        <br />
        
        <input id="read-data" type="submit" value="Read-Data" /><br />
        <br />
        
        </fieldset>
        <%} %>
        </form>
        <table>
        <%--<caption>Read-list</caption>--%>
        <tr>
            <th>
                采集人
            </th>
            <th>
                日期
            </th>
            <th>
                核辐射剂量 (uSv/h)
            </th>
            <th>
                GPS信息
            </th>
         </tr>  
       <%if(Model.Count()!=0){ %>
        <% foreach (var item in Model)
           {%>
         <tr>
            <td>
                <%:item.name %>
            </td>
            <td>
                <%:item.data_time %>
            </td>
            <td>
                <%:item.value %>
            </td>
            <td>
                经度：<%:TempData["latitude"].ToString().Substring(0,7)%>
                纬度：<%:TempData["longtitude"].ToString().Substring(0,7)%>
            </td>
         </tr>  
         <%} }%>
         
        </table>

        

   </div>
</asp:Content>
