using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcEntityFramework.Models;
using System.Web.Script.Serialization;

namespace MvcEntityFramework.Functions
{
    public class Functions
    {
        private static AdminEntities db = new AdminEntities();
        private static int Qcounts = 5;
        private static int[] randomArray ;//= GetRandomUnrepeatArray(1,db.questions.Count(), Qcounts*2);//选择10个不重复随机数
        /// <summary>
        /// 产生不重复随机数
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public  static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
        {
            Random rnd = new Random();
            int length = maxValue - minValue + 1;
            byte[] keys = new byte[length];
            rnd.NextBytes(keys);
            int[] items = new int[length];
            for (int i = 0; i < length; i++)
            {
                items[i] = i + minValue;
            }
            Array.Sort(keys, items);
            int[] result = new int[count];
            Array.Copy(items, result, count);
            return result;
        }
        /// <summary>
        /// 随机选择题目
        /// </summary>
        /// <returns></returns>
        public  static IQueryable<question> QuestionSel()
        {
            //int counts = db.questions.Count();
            //int[] randomArray = GetRandomUnrepeatArray(1, counts  / 2, Qcounts);
            randomArray = GetRandomUnrepeatArray(1, db.questions.Count(), Qcounts * 2);
            int[] array = new int[Qcounts];
            for (int i = 0; i < Qcounts; i++)
            {
                array[i] = randomArray[i];
            }
            var question_sel = (from m in db.questions
                                where (from s in array
                                           select s).Contains(m.Id)
                                select m);
            return question_sel;     
                
        }
        public  static IQueryable<question> QuestionSel2()
        {
            //int counts=db.questions.Count();
            //int[] randomArray = GetRandomUnrepeatArray(counts / 2 + 1, counts, Qcounts);
            int[] array = new int[Qcounts];
            for (int i = 0; i < Qcounts; i++)
            {
                array[i] = randomArray[Qcounts + i];
            }
            var question_sel = (from m in db.questions
                                where (from s in array
                                       select s).Contains(m.Id)
                                select m);
            return question_sel;

        }
        
    }
}