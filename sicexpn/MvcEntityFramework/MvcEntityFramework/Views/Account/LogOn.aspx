<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcEntityFramework.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    登录
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
    <h2>Tester用户登录</h2>
    <p>
        请输入用户名和密码。 没有账号请<%:Html.ActionLink("注册", "Register", "Account")%>
    </p>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "登录不成功。请更正错误并重试。") %>
        <div>
            <fieldset>
                <legend>帐户信息</legend>
                
                <%--<div class="editor-label">
                    <%: Html.LabelFor(m => m.UserName) %>
                </div>--%>
                <div class="editor-field" style="background-image:url(Content/user.png);background-repeat:no-repeat;padding-left:30px">
                    <%: Html.TextBoxFor(m => m.UserName) %>
                    <%: Html.ValidationMessageFor(m => m.UserName) %>
                </div>
                
                <%--<div class="editor-label">
                    <%: Html.LabelFor(m => m.Password) %>
                </div>--%>
                <div class="editor-field" style="background-image:url(Content/key.png);background-repeat:no-repeat;padding-left:30px">
                    <%: Html.PasswordFor(m => m.Password) %>
                    <%: Html.ValidationMessageFor(m => m.Password) %>
                </div>
                
                <%--<div class="editor-label">
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>
                </div>--%>
                
                <p style="text-indent:2em">
                    <input type="submit" value="登录" style="background-color:#7094DB" />
                </p>
            </fieldset>
        </div>
    <% } %>
    
    <%:Html.ActionLink("Administrator登陆", "SuperUser")%>
    
    </form>
    
</asp:Content>
