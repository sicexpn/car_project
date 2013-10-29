using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;
using MvcApplication界面设计.Models;
using System.Diagnostics;

namespace MvcApplication界面设计.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        String Gw_id="0001000023cbd64a";
        String getGpsUrl="http://www.cloudsensing.cn:9071/WIFPa/GatewayAbstract.xml/0001000023cbd64a?lang=zh";
        String getDataUrl = "http://www.cloudsensing.cn:9071/WIFPa/MWHistoryData/";

        InfoDataContext db = new InfoDataContext();//数据库
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult Remote()
        {
            ViewData["Message"] = "欢迎使用 ASP.NET MVC!";
            return View();
        }

        public ActionResult Collect()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Collect(FormCollection collection)
        {
            String name = collection["collector"];//关键是 name="collector"
            var name_find = from m in db.Name
                            where m.name == name
                            select m;
            int name_num;
            if (name_find.Count() != 0)//保证num和name一一对应，在wot平台中，num就代表对应的采集人
            {
                name_num = name_find.First().num;
            }
            else//没有匹配的就写入数据库
            {
                Name na = new Name();
                na.name = name;

                db.Name.InsertOnSubmit(na);
                db.SubmitChanges();

                var num_find = from m in db.Name
                            where m.name == name
                            select m;
                name_num = num_find.First().num;
            } 
            
            #region 采集指令下发 DID:02;OpId:01;Value:保存num
            //String cmd_sendUrl = "http://localhost:8080/CommandService/Instruction";
            String cmd_sendUrl = "http://www.cloudsensing.cn:9071/WIFPa/extension";
            #region old 平台
            //StringBuilder sb = new StringBuilder();
            //String mw_id = "0001000023cbd64a";
            //String op_time = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"); // 日期固定格式

            //#region 创建xml文件流
            //using (XmlWriter writer = XmlWriter.Create(sb))
            //{
            //    writer.WriteStartElement("Instruction");
            //    writer.WriteStartElement("DeviceList");
            //    writer.WriteStartElement("Device");

            //    writer.WriteStartElement("DID");
            //    writer.WriteString("02");
            //    writer.WriteEndElement();

            //    writer.WriteStartElement("Name");
            //    writer.WriteString("Collect");
            //    writer.WriteEndElement();
            //    #region 操作列表
            //    writer.WriteStartElement("OperationList");
            //    writer.WriteStartElement("Operation");

            //    writer.WriteStartElement("Name");
            //    writer.WriteString("collect");
            //    writer.WriteEndElement();

            //    writer.WriteStartElement("OpId");
            //    writer.WriteString("01");
            //    writer.WriteEndElement();

            //    writer.WriteStartElement("Value");
            //    writer.WriteString(name_num.ToString());
            //    writer.WriteEndElement();

            //    writer.WriteEndElement();
            //    writer.WriteEndElement();
            //    #endregion
            //    writer.WriteEndElement();
            //    writer.WriteEndElement();

            //    writer.WriteStartElement("MWID");
            //    writer.WriteString(mw_id);
            //    writer.WriteEndElement();

            //    writer.WriteStartElement("OPTime");
            //    writer.WriteString(op_time);
            //    writer.WriteEndElement();

            //    writer.WriteEndElement();
            //    writer.Flush();
            //    writer.Close();
            //}
            //#endregion
            #endregion
            
            StringBuilder sb_cmd = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            String mw_id = "0001000023cbd64a";
            //String op_time = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"); // 日期固定格式

            using (XmlWriter xw = XmlWriter.Create(sb_cmd))
            {
                xw.WriteStartElement("Device");
                xw.WriteStartElement("DID");
                xw.WriteString("02");
                xw.WriteEndElement();

                xw.WriteStartElement("OpID");
                xw.WriteString("01");
                xw.WriteEndElement();

                xw.WriteStartElement("Value");
                xw.WriteString(name_num.ToString());
                xw.WriteEndElement();

                xw.WriteEndElement();
                xw.Flush();
                xw.Close();
            }
            sb_cmd.Remove(0, 39);
            sb_cmd.Replace(@"<", @"^").Replace(@">", @"$");
            #region 创建xml文件流
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                writer.WriteStartElement("Extension");

                writer.WriteStartElement("Target");
                writer.WriteString(mw_id);
                writer.WriteEndElement();

                writer.WriteStartElement("Type");
                writer.WriteString("command");
                writer.WriteEndElement();

                writer.WriteStartElement("Content");
                writer.WriteString(sb_cmd.ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
            }
            #endregion
            sb.Remove(0, 39);
            //sb.Insert(12, " xmlns='http://schemas.datacontract.org/2004/07/SiceXPNREST'");
            String cmd_str = sb.ToString();//已经调试
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "text/xml";
            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_sendCommandCompleted);
            webClient.UploadStringAsync(new Uri(cmd_sendUrl), cmd_str);
            //return RedirectToAction("Remote", "Home");
            #endregion 
            return RedirectToAction("Collect");
        }
        void webClient_sendCommandCompleted(object sender, UploadStringCompletedEventArgs e)
        { 
            
        }
        public ActionResult Read()
        {
            List<Info> infos = new List<Info>();
            return View(infos);
        }
        [HttpPost]
        public ActionResult Read(FormCollection collection)
        {
            
            #region 获取网关Gps
            WebClient webClient = new WebClient();
            Stream data = webClient.OpenRead(getGpsUrl);
            StreamReader reader = new StreamReader(data);
            String str = reader.ReadToEnd();

            str = str.Remove(0, 38);//去掉前面的说明，否则会出错
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            XmlNode root = doc.FirstChild;
            XmlNode deployInfo = root["DeployInfo"];
            XmlNode gps = deployInfo["GPS"];
            //获取gps信息
            String latitude = gps["Latitude"].InnerText;
            String longtitude = gps["Longtitude"].InnerText;
            String Altitude = gps["Altitude"].InnerText;

            TempData["latitude"] = latitude;
            TempData["longtitude"] = longtitude;

            data.Close();
            reader.Close();
            #endregion
            #region xml解析获取采集数据，入库
            //解析xml文件
            getDataUrl += Gw_id;//获取DataPoint数据的URL
            WebClient webClient2 = new WebClient();
            Stream sm2 = webClient2.OpenRead(getDataUrl);
            StreamReader sr2 = new StreamReader(sm2);
            String str2 = sr2.ReadToEnd();
            str2 = str2.Remove(0, 38);


            XmlDocument doc2 = new XmlDocument();
            doc2.LoadXml(str2);
            XmlNode root2 = doc2.FirstChild;
            XmlNodeList serviceData = root2.ChildNodes;
            int sdata_len = serviceData.Count;
            List<Info> info_list = new List<Info>();//向表中插入采集信息

            //向列表中存数信息
            for (int i = 0; i < sdata_len; i++)
            {
                XmlNode sdata = serviceData[i];
                string mwid = sdata.FirstChild.InnerText;//网关ID
                string dt = sdata["datatime"].InnerText;//时间

                XmlNodeList dpList = sdata.ChildNodes;
                int dp_count = dpList.Count;
                for (int j = 2; j < dp_count; j++)
                {
                    Info info_insert = new Info();
                    string value = dpList[j]["value"].InnerText;//采集数据值 
                    string num = dpList[j]["num"].InnerText;//采集人编号
                   

                    info_insert.gw_id = mwid;
                    info_insert.data_time = DateTime.Parse(dt);
                    int no = Int16.Parse(num);
                    //no = no - 1;
                    if (no >= 10)
                    {
                        info_insert.num = no;

                        var name_num = from m in db.Name//选择对应编号的采集人姓名存入数据库
                                       where m.num == no
                                       select m.name;
                        if (name_num.Count() != 0)//查询判断,异常处理
                        {
                            info_insert.name = name_num.First();
                        }
                        info_insert.value = float.Parse(value);

                        info_list.Add(info_insert);
                    }

                }
            }
            db.Info.InsertAllOnSubmit(info_list);//将解析的数据添加入数据库
            db.SubmitChanges();

            #endregion
            #region 根据用户输入的信息，从数据库中查询对应的数据

            string name = collection["collector"];//要查询的采集人
            string date = collection[2];//要查询的日期
            var n1 = from m in db.Name
                     where m.name == name
                     select m.num;//根据采集人查找对应的编号
            DateTime dt_read = new DateTime();
            if (date != "")
            {
                dt_read = DateTime.Parse(date).Date;
            }
            
            List<Info> infos = new List<Info>();
            //3中情况讨论：1、仅输入采集人名字 2、仅输入日期 3、采集人名字和日期都输入
            if (n1.Count() != 0 && date=="")//1、仅采集人
            {
                //int n2 = n1.First();
                var infos_find = from m in db.Info
                                 where name == m.name
                                 select m;
                infos = infos_find.ToList();
            }
            else if(n1.Count()==0 && date!="")//2、仅输入日期
            {
                var infos_find=from m in db.Info
                               where m.data_time.Value.Date==dt_read
                               select m;
                infos = infos_find.ToList();
                               
            }
            else if (n1.Count() != 0 && date !="")//3、输入采集人和日期
            {
                var infos_find = from m in db.Info
                                 //where DateTime.Compare(DateTime.Parse(m.data_time.ToString()).Date, dt_read) == 0
                                 where m.data_time.Value.Date ==dt_read && name == m.name
                                 select m;
                infos = infos_find.ToList();
            }
            else//无输入处理
            {
                
            }
            #endregion
            #region 清空数据库
            var infos_delete = from m in db.Info
                               select m;
            if (infos_delete.Count() > 0)
            {
                db.Info.DeleteAllOnSubmit(infos_delete);
                db.SubmitChanges();
            }

            #endregion
            return View(infos);
        }
       
    }
}
