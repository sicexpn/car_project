using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEntityFramework.Models;
using MvcEntityFramework.Functions;
using System.Web.Script.Serialization;

  
namespace MvcEntityFramework.Controllers
{
    [HandleError]
    [Authorize]
    public class ExamController : Controller
    {
        //
        // GET: /Exam/
        #region 变量声明
        
        private static float baseGrade = 60.0F;
        private static int flag = 1;//判断是否为第二次考试的标记.1表示第一次，2表示第二次考试。

        private AdminEntities db = new AdminEntities();//数据库访问
        private static IQueryable<question> questionSel ;//= Functions.Functions.QuestionSel();

        private static IQueryable<question> questionSel2 ;//= Functions.Functions.QuestionSel2();

        //private static IQueryable<question> questionSel = Functions.Functions.QuestionSel();
                                                          
        //private static IQueryable<question> questionSel2 = Functions.Functions.QuestionSel2();

        private static int counts;//=questionSel.Count();
        #endregion
        /// <summary>
        /// 开始界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Start()
        {
            //counts = questionSel.Count();
            
            if (flag == 1)
            {
                questionSel = Functions.Functions.QuestionSel();
                questionSel2 = Functions.Functions.QuestionSel2();
                counts = questionSel.Count();//重新随机选择题目
                ViewData["flag"] = "1";
            }
            else if (flag == 2)
            {
                ViewData["flag"] = "2";
            }
            return View();
        }
        /// <summary>
        /// 获取用户选择的及格分数线
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Start(FormCollection collection)
        {
            baseGrade = float.Parse(collection["sel"]);
            //var student = (from m in db.Students
            //               where m.stuId == StudentInfo.getInfo.stuId
            //               select m).First();
            //student.baseGrade = baseGrade;
            return RedirectToAction("Middle");
        }
        /// <summary>
        /// 考试界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Middle()
        {
            
            if (flag == 1)
            {
                //var oldRecord = (from m in db.Reports
                //                 where m.StuId == StudentInfo.getInfo.stuId
                //                 select m);
                
                //if (oldRecord.Count() > 0)
                //{
                //    foreach (var record in oldRecord)
                //    {
                //        db.DeleteObject(record);//删除以前的记录
                        
                //    }
                //}
                //db.SaveChanges();

                ViewData["flag"] = "1";
                return View(questionSel.ToList());
                
            }
            else 
            {
                ViewData["flag"] = "2";
                return View(questionSel2.ToList());
            }
        }
        /// <summary>
        /// 试卷提交
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Middle(FormCollection collection)//考试表单提交
        {
            
            int counts = collection.Count - 2;
            int groups = counts / 4;
            IQueryable<string> RightAnswers;
            if (flag == 1)
            {
                var oldRecord = (from m in db.Reports
                                 where m.StuId == StudentInfo.getInfo.stuId
                                 select m);

                if (oldRecord.Count() > 0)
                {
                    foreach (var record in oldRecord)
                    {
                        db.DeleteObject(record);//删除以前的记录

                    }
                }
                db.SaveChanges();

                RightAnswers = from m in questionSel
                                                  select m.Answers;
            }
            else
            {
                RightAnswers = from m in questionSel2
                                                  select m.Answers;
            }
            string[] rightAnswers = new string[counts];
            rightAnswers = RightAnswers.ToArray();


            string[] userAnswers = new string[counts];
            for (int i = 0; i < rightAnswers.Count(); i++)
            {
                rightAnswers[i] = rightAnswers[i].Replace(" ", "");//为了匹配，去掉空格，否则不能匹配
            }

            //float grade = 0;//成绩
            for (int i = 0; i < groups; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    if (collection[i * 4 + j].Contains("true"))
                    {
                        switch (j)
                        {
                            case 1:
                                userAnswers[i] += "A";
                                break;
                            case 2:
                                userAnswers[i] += "B";
                                break;
                            case 3:
                                userAnswers[i] += "C";
                                break;
                            case 4:
                                userAnswers[i] += "D";
                                break;
                        }
                    }
                }

            }



            report[] r = new report[counts];

            question[] qSel = new question[counts];
            if (flag == 1)
            {
                qSel = questionSel.ToArray();
            }
            else
            {
                qSel = questionSel2.ToArray();
            }
            
            var score1 = 0.0;//计算第一次成绩
            for (int i = 0; i < groups; i++)//将考生考试记录写入数据库reports表中，便于以后查询成绩单
            {
                r[i] = new report();

                r[i].StuId = StudentInfo.getInfo.stuId;
                //r[i].StuId = 1;//获取用户ID
                r[i].stuName = StudentInfo.getInfo.userName;
                r[i].QuestionId = qSel[i].Id;
                r[i].Question = qSel[i].Question;
                r[i].A = qSel[i].A;
                r[i].B = qSel[i].B;
                r[i].C = qSel[i].C;
                r[i].D = qSel[i].D;
                r[i].CorrectAnswer = qSel[i].Answers;
                r[i].UserAnswer = userAnswers[i];


                
                if (userAnswers[i] == rightAnswers[i])//判断答题是否正确
                {
                    r[i].Grade = (100 / groups);
                    score1 += r[i].Grade.Value;
                }
                else
                {
                    r[i].Grade = 0;
                }
                db.Reports.AddObject(r[i]);
                db.SaveChanges();
            }

            int t = int.Parse( collection[21].ToString());
            if (flag == 1)
            {
                if (t <= 120)//120
                {
                    return RedirectToAction("Report");//一次考试
                }
                else
                {
                    //第二次考试
                    if (score1 >= baseGrade)
                    {
                        flag = 2;
                        return RedirectToAction("Start");
                    }
                    else
                    {
                        return RedirectToAction("Report");
                    }
                }
            }
            else
            {
                flag = 1;
                return RedirectToAction("Report");
            }
            //return Content(grade.ToString());
            
        }
        /// <summary>
        /// 只有一次考试成绩单
        /// </summary>
        /// <returns></returns>
        //public ActionResult Report1()
        //{
        //    var tmp=(from r in db.Reports  //从成绩单中选出用户答题记录
        //                 join q in db.questions on r.QuestionId equals q.Id
        //                 join s in db.Students on r.StuId equals s.stuId
        //                 select r).ToList();
        //    int num = tmp.Count();
        //    report[] reports = new report[counts];
        //    var score = 0.0;
        //    for (int i = 1; i <= counts; i++)//选择倒数5个
        //    {
        //        reports[counts - i] = tmp[num - i];
        //        score += reports[counts - i].Grade.Value;
        //    }
        //    if (score >= baseGrade)
        //    {
        //        ViewData["base"] = "是";
        //    }
        //    else
        //    {
        //        ViewData["base"] = "否";
        //    }
        //    ViewData["name"] = StudentInfo.getInfo.userName;
        //    ViewData["score"] = score;//传递考试总成绩
        //    ViewData["baseGrade"] = baseGrade;
        //    ViewData["date"] = DateTime.Now.ToString();

        //    var stu = (from m in db.Students
        //               where m.stuId == StudentInfo.getInfo.stuId
        //               select m).First();
        //    student stuToEdit = new student();
        //    stuToEdit = stu;
        //    stuToEdit.baseGrade = baseGrade;
        //    stuToEdit.Grade = score;
        //    db.ApplyCurrentValues<student>(stu.EntityKey.EntitySetName, stuToEdit);//添加基准分和总成绩到数据库表student
        //    db.SaveChanges();
        //    return View();
        //}
        /// <summary>
        /// 两次考试的成绩单
        /// </summary>
        /// <returns></returns>
        //public ActionResult Report2()
        //{
        //    var tmp = (from r in db.Reports  //从成绩单中选出用户答题记录
        //               join q in db.questions on r.QuestionId equals q.Id
        //               join s in db.Students on r.StuId equals s.stuId
        //               select r).ToList();
        //    int num = tmp.Count();
        //    report[] reports = new report[10];
        //    var score1 = 0.0;
        //    var score2 = 0.0;
        //    var score = 0.0;
        //    for (int i = 1; i <= counts*2; i++)//选择倒数5个
        //    {
        //        reports[10 - i] = tmp[num - i];
        //        if (i <= counts)
        //        {
        //            score2 += reports[counts*2 - i].Grade.Value;
        //        }
        //        else
        //        {
        //            score1 += reports[counts*2 - i].Grade.Value;
        //        }
        //    }
        //    score = (score1 + score2) / 2;
        //    ViewData["score1"] = score1;
        //    ViewData["score2"] = score2;

        //    if (score >= baseGrade)
        //    {
        //        ViewData["base"] = "考试通过";
        //    }
        //    else
        //    {
        //        ViewData["base"] = "考试未通过";
        //    }
        //    ViewData["name"] = StudentInfo.getInfo.userName;
        //    ViewData["date"] = DateTime.Now.ToString();
        //    ViewData["baseGrade"] = baseGrade;
        //    ViewData["score"] = score;//传递考试总成绩

        //    var stu = (from m in db.Students
        //               where m.stuId == StudentInfo.getInfo.stuId
        //               select m).First();
        //    student stuToEdit = new student();
        //    stuToEdit = stu;
        //    stuToEdit.baseGrade = baseGrade;
        //    stuToEdit.Grade1 = score1;
        //    stuToEdit.Grade2 = score2;
        //    stuToEdit.Grade = score;
        //    db.ApplyCurrentValues<student>(stu.EntityKey.EntitySetName, stuToEdit);//添加相关成绩到数据库表student
        //    db.SaveChanges();
        //    return View();
        //}
        public ActionResult Report()
        {
            var reports = (from m in db.Reports
                           where m.StuId == StudentInfo.getInfo.stuId
                           select m);
            var score1 = 0.0;
            var score2 = 0.0;
            var score = 0.0;
            if (reports.Count() == counts)//考一次的情况
            {
                foreach (var item in reports)
                {
                    score += item.Grade.Value;
                }
            }
            else//考2次的情况
            {
                report[] reportsArray = new report[counts*2];
                reportsArray=reports.ToArray();
                for (int i = 0; i < counts; i++)
                {
                    score1 += reportsArray[i].Grade.Value;
                }
                for (int j = counts; j < counts * 2; j++)
                {
                    score2 += reportsArray[j].Grade.Value;
                }
                score = (score1 + score2) / 2;
            }

            if (score >= baseGrade)
            {
                ViewData["base"] = "是";
            }
            else
            {
                ViewData["base"] = "否";
            }
            ViewData["name"] = StudentInfo.getInfo.userName;
            ViewData["date"] = DateTime.Now.ToString();
            ViewData["baseGrade"] = baseGrade;
            ViewData["score"] = score;//传递考试总成绩

            var stu = (from m in db.Students
                       where m.stuId == StudentInfo.getInfo.stuId
                       select m).First();
            student stuToEdit = new student();
            stuToEdit = stu;
            stuToEdit.baseGrade = baseGrade;
            stuToEdit.Grade1 = score1;
            stuToEdit.Grade2 = score2;
            stuToEdit.Grade = score;
            db.ApplyCurrentValues<student>(stu.EntityKey.EntitySetName, stuToEdit);//添加相关成绩到数据库表student
            db.SaveChanges();
            return View();
        }
        public ActionResult DetailReport()
        {
            var reports = (from m in db.Reports
                           where m.StuId == StudentInfo.getInfo.stuId
                           select m).ToList();
            return View(reports);
        }
        //public ActionResult List()
        //{
        //    TempData["sel"] = questionSel;
        //    return View(questionSel.ToList());
        //}
        //[HttpPost]
 //       public ActionResult List(FormCollection collection)
 //       {
 //           //var v = ViewData["1"];
 //           //return Content(v.ToString());
            
 //           int counts = collection.Count - 1;
 //           int groups = counts / 4;
 //           //IQueryable<question> sel = TempData["sel"]
 //           IQueryable<string> RightAnswers = from m in questionSel
 //                                             select m.Answers;
 //           string[] rightAnswers = new string[5];
 //           rightAnswers = RightAnswers.ToArray();

 //           //List<string> userAnswers = new List<string>() ;
 //           string[] userAnswers = new string[5];
 //           for (int i = 0; i < rightAnswers.Count(); i++)
 //           {
 //               rightAnswers[i] = rightAnswers[i].Replace(" ", "");//为了匹配，去掉空格，否则不能匹配
 //           }

 //           float grade = 0;
 //           for (int i = 0; i < groups; i++)
 //           {
 //               for (int j = 1; j <= 4; j++)
 //               {
 //                   if (collection[i * 4 + j].Contains("true"))
 //                   {
 //                       switch (j)
 //                       {
 //                           case 1:
 //                               userAnswers[i] += "A";
 //                               break;
 //                           case 2:
 //                               userAnswers[i] += "B";
 //                               break;
 //                           case 3:
 //                               userAnswers[i] += "C";
 //                               break;
 //                           case 4:
 //                               userAnswers[i] += "D";
 //                               break;
 //                       }
 //                   }
 //               }

 //           }
 //           for (int i = 0; i < groups; i++)
 //           {
 //               if (userAnswers[i] == rightAnswers[i])
 //               {
 //                   grade += (100 / groups);
 //               }
 //           }
 ////           string checkbox1 = counts.ToString();
 //           return Content(grade.ToString());
 //       }
 //       public string HelloAjax(string query)
 //       {
 //           return "You entered:" + query;
 //       }
 //       public ActionResult test()
 //       {

 //           TempData["json"] = "JSON";
 //           return View();
 //       }
 //       public ActionResult getTest(string time)
 //       {
 //           //if (TempData["time"].ToString()=="1")
 //           //    return Content("1");
 //           //else
 //           //int t = Int16.Parse(time);
 //               return Content("0");
 //       }
        
 //       public ActionResult GetTime()
 //       {
 //           return Content(DateTime.Now.ToShortDateString());
 //       }
        
    }
}
