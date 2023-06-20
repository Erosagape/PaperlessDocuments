using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentControl.Models;
namespace DocumentControl.Controllers
{
    public class SetupController : Controller
    {
        // GET: Setup
        public ActionResult Index()
        {
            var oConn = AppManager.TestConnect();
            ViewBag.ResultConnect = oConn.Result.ToString();
            ViewBag.MessageConnect = oConn.Message;
            return View();
        }
    }
}