using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEntityFramework.Models;

namespace MvcEntityFramework.Controllers
{
    /// <summary>
    /// 过滤器，只允许超级用户访问
    /// </summary>
    [MyAuth(Roles="Admin")]
    [HandleError]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private AdminEntities db = new AdminEntities();
        /// <summary>
        /// 超级用户管理界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View(db.Students.ToList());
        }

        //
        // GET: /Admin/Details/5
        /// <summary>
        /// 查看用户详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var stuToDetail = (from m in db.Students
                               where m.stuId == id
                               select m).First();
            return View(stuToDetail);
        }
        public ActionResult Reports(int id)
        {
            var stuToReports = (from m in db.Reports
                                where m.StuId == id
                                select m);
            if (stuToReports.Count() > 0)
            {
                var stu = (from m in db.Students
                           where m.stuId == id
                           select m).First();
                var score = stu.Grade;
                var baseGrade = stu.baseGrade;
                var score1 = stu.Grade1;
                var score2 = stu.Grade2;
                if (score >= baseGrade)
                {
                    ViewData["base"] = "考试通过";
                }
                else
                {
                    ViewData["base"] = "考试未通过";
                }
                ViewData["score1"] = score1;
                ViewData["score2"] = score2;
                ViewData["baseGrade"] = baseGrade;
                ViewData["score"] = score;//传递考试总成绩
                return View(stuToReports);
            }
            else
            {
                return RedirectToAction("NoReports");
            }
        }
        public ActionResult NoReports()
        {
            return View();
        }

        //
        // GET: /Admin/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/Create
        /// <summary>
        /// 新建用户，用户名不允许重复
        /// </summary>
        /// <param name="stuToCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create([Bind(Exclude="stuId")] student stuToCreate)
        {
            if (!ModelState.IsValid)
                return View();
            var student = (from m in db.Students
                           where m.userName == stuToCreate.userName
                           select m);
            if (student.Count() == 0)
            {
                db.AddToStudents(stuToCreate);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                TempData["name"] = "用户名已经存在";
                return View();
            }
        }
        
        //
        // GET: /Admin/Edit/5
 
        public ActionResult Edit(int id)
        {
            var stuToEdit = (from m in db.Students
                             where m.stuId == id
                             select m).First();
            return View(stuToEdit);
        }

        //
        // POST: /Admin/Edit/5
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="stuToEdit"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(student stuToEdit)
        {
            var orignalStudent = (from m in db.Students
                             where m.stuId == stuToEdit.stuId
                             select m).First();
            if (!ModelState.IsValid)
                return View(orignalStudent);
            db.ApplyCurrentValues<student>(orignalStudent.EntityKey.EntitySetName, stuToEdit);
            db.SaveChanges();

            return RedirectToAction("List");

        }

        //
        // GET: /Admin/Delete/5
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
 
        public ActionResult Delete(int id)
        {
            
            var stuToDelete = (from m in db.Students
                               where m.stuId == id
                               select m).First();
            db.DeleteObject(stuToDelete);
            db.SaveChanges();

            return RedirectToAction("List");
            //return View(stuToDelete);
        }

        //
        // POST: /Admin/Delete/5

        [HttpPost]
        public ActionResult Delete(student stuToDelete)
        {
            //var model = (from m in db.Students
            //             where m.stuId == stuToDelete.stuId
            //             select m).First();
            //db.DeleteObject(model);
            //db.SaveChanges();
            return RedirectToAction("List");

        }
    }
}
