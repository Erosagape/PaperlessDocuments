using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentControl.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            ViewBag.Message = "Ready";
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];
            if(Request.QueryString["Action"]!=null)
                return View(Request.QueryString["Action"]);
            return View();
        }
    }
}