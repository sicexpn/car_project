using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace MvcApplication界面设计.Controllers
{
    public class CarController : Controller
    {
        //下行控制
        // GET: /Car/
        
        //DID:01-----car
        /*DID:02-----采集
           OpID:01---up
         * OpID:02---down
         * OpID:03---left
         * OpID:04---right
         * OpID:05---stop
         * Value:与OpID值一样-----有操作
         */
        //String cmd_sendUrl = "http://localhost:8080/CommandService/Instruction";//"http://192.168.0.141:8080/CommandService/Instruction";
        String cmd_sendUrl = "http://www.cloudsensing.cn:9071/WIFPa/extension";
        public ActionResult Index()
        {
            
            return RedirectToAction("Remote", "Home");
        }
        
        #region 大平台
        public void sendCmd(string dId,string opId,string value)
        {
            StringBuilder sb_cmd = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            String mw_id = "0001000023cbd64a";
            //String op_time = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"); // 日期固定格式

            using (XmlWriter xw = XmlWriter.Create(sb_cmd))
            {
                xw.WriteStartElement("Device");
                xw.WriteStartElement("DID");
                xw.WriteString(dId);
                xw.WriteEndElement();

                xw.WriteStartElement("OpID");
                xw.WriteString(opId);
                xw.WriteEndElement();

                xw.WriteStartElement("Value");
                xw.WriteString(value);
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
            String cmd_str = sb.ToString();//已经调试
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "text/xml";
            webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_sendCommandCompleted);
            webClient.UploadStringAsync(new Uri(cmd_sendUrl), cmd_str);
        }
        public ActionResult Up()//OpId:01 
        {
            sendCmd("01", "01", "01");
            return RedirectToAction("Remote", "Home");
        }
        public ActionResult Down()//OpId:02 
        {
            sendCmd("01", "02", "02");
            return RedirectToAction("Remote", "Home");
        }
        public ActionResult Left()//OpId:03 
        {
            sendCmd("01", "03", "03");
            return RedirectToAction("Remote", "Home");
        }
        public ActionResult Right()//OpId:04 
        {
            sendCmd("01", "04", "04");
            return RedirectToAction("Remote", "Home");
        }
        public ActionResult Stop()//OpId:05 
        {
            sendCmd("01", "05", "05");
            return RedirectToAction("Remote", "Home");
        }
        void webClient_sendCommandCompleted(object sender,UploadStringCompletedEventArgs e)
        {
            //string str = e.Result;
            //int i;
        }
        #endregion
    }
}
