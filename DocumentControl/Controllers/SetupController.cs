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
        [HttpPost]
        public ActionResult Customer(FormCollection form)
        {
            bool addmode = false;
            if (form["cid"] == "")
            {
                addmode = true;
            }
            string sql = "";           
            if (addmode)
            {
                sql += @"INSERT INTO CounterCustomer (
companycode,
datagroup,
dataid,
contractid,
taxreference,
companynameth,
addressth,
companynameen,
companycontact
) VALUES (
'{0}',
'{1}',
'{2}',
'{3}',
'{4}',
'{5}',
'{6}',
'{7}',
'{8}'
                )";
                sql = string.Format(sql,
    form["companycode"],
    form["datagroup"],
    form["dataid"],
    form["contractid"],
    form["taxreference"],
    form["companynameth"],
    form["addressth"],
    form["companynameen"],
    form["companycontact"]
    );

            }
            else
            {
                sql = @"UPDATE CounterCustomer SET
companycode='{1}',
datagroup='{2}',
dataid='{3}',
contractid='{4}',
taxreference='{5}',
companynameth='{6}',
addressth='{7}',
companynameen='{8}',
companycontact='{9}'
WHERE cid={0}
";
                sql = string.Format(sql,
    form["cid"],
    form["companycode"],
    form["datagroup"],
    form["dataid"],
    form["contractid"],
    form["taxreference"],
    form["companynameth"],
    form["addressth"],
    form["companynameen"],
    form["companycontact"]
    );

            }
            
            var o=AppManager.ExecuteSQL(sql);
            TempData["Message"] = (o.Result.ToString()=="OK" ? 
                                    "Save Complete":
                                    o.Result.ToString());
            return Redirect("/Default?Action=Customers");
        }
    }
}