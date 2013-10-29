using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcEntityFramework.Models
{
    public class MyAuthAttribute : AuthorizeAttribute
    {
        // 只需重载此方法，模拟自定义的角色授权机制  
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string currentRole = GetRole(httpContext.User.Identity.Name);
            if (Roles.Contains(currentRole))
                return true;
            return base.AuthorizeCore(httpContext);
        }

        // 返回用户对应的角色， 在实际中， 可以从SQL数据库中读取用户的角色信息  
        private string GetRole(string name)
        {
            AdminEntities db = new AdminEntities();
            var q = (from m in db.SuperUser
                     where m.Name == name
                     select m);
            if (q.Count() > 0)
            {
                return "Admin";
            }
            else
            {
                return "User";
            }
        }
    }  
}