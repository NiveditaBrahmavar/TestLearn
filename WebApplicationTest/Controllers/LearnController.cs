using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationTest.Models;

namespace WebApplicationTest.Controllers
{
    public class LearnController : Controller
    {
        // GET: Learn
        public ActionResult Index()
        {
             

            return View();
        }

        [HttpGet]
        public JsonResult GetSuggestion(string text)
        {
            List<MyShop> shpList = new List<MyShop>();
            shpList.Add(new MyShop { ItemID = 1, ItemName = "Name1", IsAvailable = true });
            shpList.Add(new MyShop { ItemID = 2, ItemName = "Name2", IsAvailable = true });
            shpList.Add(new MyShop { ItemID = 3, ItemName = "Name3", IsAvailable = true });

            List<string> newlist = new List<string>();

            newlist = shpList.Where(x => x.ItemName.ToLower().Contains(text.ToLower())).Select(y=> y.ItemName).ToList();

            return Json(newlist, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ValidationCheck()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ValidationCheck(Customer cust)
        {
            if(ModelState.IsValid)
            {
                return View("Index");
            }
            return View(cust);
        }
    }
}