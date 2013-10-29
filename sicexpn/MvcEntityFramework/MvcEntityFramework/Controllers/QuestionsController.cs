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
    [MyAuth(Roles = "Admin")]
    [HandleError]
    public class QuestionsController : Controller
    {
        //
        // GET: /Questions/
        private AdminEntities db = new AdminEntities();
       
        public ActionResult List()
        {
            
            return View(db.questions.ToList());
        }

        //
        // GET: /Questions/Details/5

        public ActionResult Details(int id)
        {
            var question = (from m in db.questions
                           where m.Id == id
                           select m).First();
            return View(question);
        }

        //
        // GET: /Questions/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Questions/Create

        [HttpPost]
        public ActionResult Create(question  questionToCreate)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var question = (from m in db.questions
                            where m.Question == questionToCreate.Question
                            select m);
            if (question.Count() == 0)
            {
                db.AddToquestions(questionToCreate);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                TempData["question"] = "题目已经存在";
                return View();
            }
          
        }
        
        //
        // GET: /Questions/Edit/5
 
        public ActionResult Edit(int id)
        {
            var question = (from m in db.questions
                           where m.Id == id
                           select m).First();

            return View(question);
        }

        //
        // POST: /Questions/Edit/5

        [HttpPost]
        public ActionResult Edit(question questionToEdit)
        {
            var originalQuestion=(from m in db.questions
                                 where m.Id==questionToEdit.Id
                                 select m).First();
            if(!ModelState.IsValid)
            {
                return View(originalQuestion);
            }
            db.ApplyCurrentValues<question>(originalQuestion.EntityKey.EntitySetName, questionToEdit);
            db.SaveChanges();

            return RedirectToAction("List");
          
        }

        //
        // GET: /Questions/Delete/5
 
        public ActionResult Delete(int id)
        {
            var questionToDelete = (from m in db.questions
                                    where m.Id == id
                                    select m).First();
            db.questions.DeleteObject(questionToDelete);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        //
        // POST: /Questions/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
                return View();
          
        }
    }
}
