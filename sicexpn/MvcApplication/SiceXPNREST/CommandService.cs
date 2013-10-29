using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace SiceXPNREST
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class CommandService
    {
        static List<Instruction> ItemList = new List<Instruction>();
        static bool flag = false;
        static Thread clearItem;
        // TODO: Implement the collection resource that will contain the SampleItem instances
        public CommandService()
        {
            if (flag == false)
            {
                clearItem = new Thread(new ThreadStart(ClearItemFun));
                clearItem.Start();
                flag = true;
            }
        }

        [WebGet(UriTemplate = "")]
        //[WebGet(UriTemplate = "Add/{x}/{y}")]
        public List<SampleItem> GetCollection()
        {
            // TODO: Replace the current implementation to return a collection of SampleItem instances
            return new List<SampleItem>() { new SampleItem() { Id = 1, StringValue = "Hello" } };
        }
        /// <summary>
        /// 向中间平台提交请求
        /// </summary>
        /// <param name="MWID"></param>
        /// <param name="item"></param>
        [WebInvoke(UriTemplate = "Instruction", Method = "POST", ResponseFormat = WebMessageFormat.Xml)]
        public void Instruction( Instruction item)
        {
            if (item != null)
            {
                item.OPTime = DateTime.Now;
                lock (ItemList)
                {
                    ItemList.Add(item);
                }
            }
        }
        [WebInvoke(UriTemplate = "GetDEV/{MWID}?devid={DEVID}", Method = "GET", ResponseFormat = WebMessageFormat.Xml)]
        public List<Instruction> GetDEV(string MWID, string DEVID)
        {
             //List<Instruction> q1 = (from c in ItemList where (c.MWID == MWID && c.DeviceList  select c).ToList();
            //List<Instruction> q = (from a in ItemList where (from c in a.DeviceList where c.DID == DEVID select c).ToList() select a).ToList();

            var q = from a in ItemList where a.MWID == (from b in a.DeviceList where b.DID == DEVID && b.MWID == MWID select b).Single().MWID select a;

            lock (ItemList)
            {
                 foreach (Instruction item in q)
                {
                    ItemList.Remove(item);
                }
            }
            return q.ToList() ;
 
         }
          
     
        /// <param name="item"></param>
        [WebInvoke(UriTemplate = "GetMW/{MWID}", Method = "GET", ResponseFormat = WebMessageFormat.Xml)]
        public List<Instruction> GetMW(string MWID)
        {
            //List<Instruction> q=(from a in ItemList where( from c in a.DeviceList where  c.
            List<Instruction> q=(from a in ItemList where a.MWID==MWID select a).ToList();
            lock (ItemList)
            {
                //foreach (Instruction item in q)
                //{
                //    ItemList.Remove(item);
                //}
                for (int i = q.Count() - 1; i >= 0; i--)
                {
                    ItemList.Remove(q[i]);
                }
            }
            return q;       
        }
        
        [WebInvoke(UriTemplate = "", Method = "POST")]
        public SampleItem Create(SampleItem instance)
        {
            // TODO: Add the new instance of SampleItem to the collection
            throw new NotImplementedException();
        }

        [WebGet(UriTemplate = "{id}")]
        public SampleItem Get(string id)
        {
            // TODO: Return the instance of SampleItem with the given id
            throw new NotImplementedException();
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        public SampleItem Update(string id, SampleItem instance)
        {
            // TODO: Update the given instance of SampleItem in the collection
            throw new NotImplementedException();
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public void Delete(string id)
        {
            // TODO: Remove the instance of SampleItem with the given id from the collection
            throw new NotImplementedException();
        }
        public void ClearItemFun()
        {
            while (1 == 1)
            {
                Thread.Sleep(300000);
                lock (ItemList)
                {
                    var templist = ItemList.Where(c => c.OPTime < DateTime.Now.AddSeconds(-300)).ToList();
                    for (int i = templist.Count() - 1; i >= 0; i--)
                    {
                        ItemList.Remove(templist[i]);
                    }
                }

            }
        }
    }
}
