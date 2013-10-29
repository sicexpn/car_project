<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Start
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="background-image:url(../../Content/in.png);background-repeat:no-repeat;padding-left:20px">考试规则</h2>
    <p >
        
        用户选择及格分数线，点击"开始考试"链接，进入考试界面，答题时间规定为120秒，如果用户在规定时间内提交，那么答题结束直接显示成绩单；<br />
        如果计时超过120秒，用户提交，那么系统会判断当前用户考试成绩是否及格：
        
        <br />
        1、若及格，并要求用户第二次答题，及格分数线保持不变，用户点击"重新开始考试"，进入考试界面，用户答题完毕后，提交试卷，考试结束，显示成绩单。（注：此时总成绩为两次考试成绩的均值。）<br />
        2、若不及格，则考试结束，显示成绩单。
    </p>
        
    <form id="form1" runat="server">
    <% if( ViewData["flag"]=="1"){ %>
    
           <%using (Html.BeginForm())
             {%>
            <fieldset>
                <legend>选择及格分数线</legend>
                <select id="sel" name="sel">
                    <option value="100">100</option>
                    <option value="80">80</option>
                    <option value="60" selected="selected">60</option>
                    <option value="40">40</option>
                    <option value="20">20</option>
                </select>
                <br /><br />
                <input type="submit" value="开始考试" />
            </fieldset>
            <%} %>
    
    <%} if(ViewData["flag"]=="2"){%>
   <p style="background-image:url(../Content/bulb.png);background-repeat:no-repeat;padding-left:20px;color:#800020">您及格了，但是超时，请进行第二次考试</p>
    <%: Html.ActionLink("进行第二次考试","Middle")%>
    <%} %>
    </form>
</asp:Content>
