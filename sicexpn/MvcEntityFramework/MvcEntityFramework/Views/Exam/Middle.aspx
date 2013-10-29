<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcEntityFramework.Models.question>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	考试中...
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="background-image:url(../../Content/menu.png);background-repeat:no-repeat;padding-left:20px">多项选择题</h2>
    <form id="form1" runat="server">
      
    
    
    <% using (Html.BeginForm())
       {%>
        <%: Html.ValidationSummary(true)%>
        
        <fieldset>
            <legend>题目和选项</legend>
            <ul>
             <%foreach (var item in Model)
               {%>
               
               <div class="display-label" style="background-color:White">
               <li style="font-size:larger">
               <%:item.Question%>
               </li>
                
                </div>
                
                <div class="display-field">
                   
                   <%:Html.CheckBox(item.Question + item.A, false)%>
                   A:<%:item.A%><br />
                   
                   <%:Html.CheckBox(item.Question + item.B, false)%>
                   B:<%:item.B%><br />

                   <%:Html.CheckBox(item.Question + item.C, false)%>
                   C:<%:item.C%><br />

                   <%:Html.CheckBox(item.Question + item.D, false)%>
                   D:<%:item.D%>
              </div>
                   <br />
              <% }%>

           </ol>
            <p>
            
               计时： <%:Html.TextBox("Timer")%>
               
                
                <input type="submit" value="Submit" style="background-color:#7094DB" />
            </p>
           
        </fieldset>

    <% } %>

    </form>
        <script type="text/javascript" language="javascript">
            time = 0; // Timer model
            function startTime() {
                 
                if (time >= 0) {

                    document.getElementById('Timer').value = time;
                    time = time + 1;


                    setTimeout('startTime()', 1000);
                    return;
                }
                
//                else if (time < 0) {

//                    form1.submit();

//                }


            }
            window.onload = startTime;
    </script>
</asp:Content>
