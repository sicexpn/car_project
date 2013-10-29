<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcEntityFramework.Models.report>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Reports
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2> <%:Model.First().stuName %>的成绩单</h2>
    

    
     <table>
        <tr>
            
            <th>
                题目
            </th>
            <th>
                A
            </th>
            <th>
                B
            </th>
            <th>
                C
            </th>
            <th>
                D
            </th>
            <th>
                正确答案
            </th>
            <th>
                您的答案
            </th>
            <th>
                成绩
            </th>
        </tr>
    <% var item = Model.ToArray(); %>
    
    <% for (int i = 0; i < 5;i++ )
       { %>
    
        <tr>
            
            
            <td>
                <%: item[i].Question%>
            </td>
            <td>
                <%: item[i].A%>
            </td>
            <td>
                <%: item[i].B%>
            </td>
            <td>
                <%: item[i].C%>
            </td>
            <td>
                <%: item[i].D%>
            </td>
            <td style="color:Red">
                <%: item[i].CorrectAnswer%>
            </td>
            <td style="color:black">
                <%: item[i].UserAnswer%>
            </td>
            <td>
                <%: String.Format("{0:F}", item[i].Grade)%>
            </td>
        </tr>
    
    <% } %>

    <%if(item.Count()>5){ %>
        <tr>
            
            <th></th>
            <th></th>
            <th></th>
            <th></th>
            
            <th colspan="4" style="color:Red">第一次考试成绩：<%: String.Format("{0:F}", ViewData["score1"]) %></th>
        </tr>
    
     <% for (int i = 5; i < 10;i++ )
       { %>
    
        <tr>
            
          
            <td>
                <%: item[i].Question%>
            </td>
            <td>
                <%: item[i].A%>
            </td>
            <td>
                <%: item[i].B%>
            </td>
            <td>
                <%: item[i].C%>
            </td>
            <td>
                <%: item[i].D%>
            </td>
            <td style="color:Red">
                <%: item[i].CorrectAnswer%>
            </td>
            <td style="color:Black">
                <%: item[i].UserAnswer%>
            </td>
            <td>
                <%: String.Format("{0:F}", item[i].Grade)%>
            </td>
        </tr>
    
    <% } %>
   
        <tr>
            
            <th></th>
            <th></th>
            <th></th>
            <th></th>
            
            <th colspan="4" style="color:Red">第二次考试成绩：<%: String.Format("{0:F}", ViewData["score2"]) %></th>
        </tr>
     <%} %>
        <tr>
            
            <th></th>
            <th></th>
            <th colspan="2" style="color:Red">及格分数：<%:ViewData["baseGrade"] %></th>
            <th colspan="2" style="color:Red">结果：<%:ViewData["base"] %></th>
            <th colspan="2" style="color:Red">总成绩：<%: String.Format("{0:F}", ViewData["score"]) %></th>
        </tr>
    </table>

    <p style="background-image:url(../Content/back.png);background-repeat:no-repeat;padding-left:20px;">
        <%:Html.ActionLink("back","List") %>
    </p>

</asp:Content>

