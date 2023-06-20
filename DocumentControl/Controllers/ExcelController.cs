using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Dapper;
using ExcelDataReader;
using Newtonsoft.Json;
using System.Configuration;
namespace DocumentControl.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Index()
        {
            ViewBag.Message = "Ready";
            ViewBag.DataSource = null;
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];

            if (TempData["DataExcel"] != null)
                ViewBag.DataSource = TempData["DataExcel"];

            if (Request.QueryString["Action"] != null)
                return View(Request.QueryString["Action"]);
            return View();
        }
        [HttpPost]
        public ActionResult ImportData()
        {
            try
            {
                ViewBag.DataSource = null;
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    var filePath = Server.MapPath("//") + file.FileName;
                    file.SaveAs(filePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        using (var stream = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            var xls = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                            var tbl = xls.AsDataSet().Tables[Convert.ToInt32(Request.Form["sheetno"])];
                            ViewBag.DataSource = tbl;
                            string conn = Properties.Settings.Default.MSSQLConnection;
                            using (SqlConnection cn = new SqlConnection(conn))
                            {
                                cn.Open();
                                if (cn.State.Equals(System.Data.ConnectionState.Open))
                                {
                                    int rowstart = Convert.ToInt32(Request.Form["rowstart"]);
                                    int rowprocess = 0;
                                    for(int i = rowstart; i < tbl.Rows.Count; i++)
                                    {
                                        string sql = "INSERT INTO " + Request.Form["tablename"];
                                        string sqlh = "";
                                        int j = 0;
                                        while(Request.Form["fld" + j] != null)
                                        {
                                            if (sqlh != "")
                                                sqlh += ",";
                                            sqlh += Request.Form["fld" + j].ToString();
                                            j++;
                                        }
                                        sql += "(" + sqlh + ") VALUES (";
                                        string sqld = "";
                                        j = 0;
                                        while (Request.Form["col" + j] != null)
                                        {
                                            if (sqld != "")
                                                sqld += ",";
                                            var idx = Request.Form["col" + j];
                                            if (!Request.Form["typ" + j].Equals("FIX"))
                                            {
                                                if(Request.Form["typ"+ j].Equals("POS"))
                                                {
                                                    var pos = idx.Split(',');
                                                    sqld += string.Format("'{0}'", tbl.Rows[Convert.ToInt32(pos[0])][Convert.ToInt32(pos[1])].ToString());
                                                } else
                                                {
                                                    sqld += string.Format("'{0}'", tbl.Rows[i][Convert.ToInt32(idx)].ToString());
                                                }                           
                                            }
                                            else
                                            {
                                                sqld += string.Format("'{0}'", idx);
                                            }
                                            j++;
                                        }
                                        sql += sqld +")";
                                        cn.Execute(sql);
                                        rowprocess += 1;
                                    }
                                    ViewBag.Message = rowprocess + " rows processed";
                                }
                            }
                        }
                    }
                }
                return View(Request.Form["returnto"]);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View(Request.Form["returnto"]);
            }
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult PostIndex()
        {
            TempData["DataExcel"] = null;
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var filePath = Server.MapPath("//") + file.FileName;
                file.SaveAs(filePath);
                if (System.IO.File.Exists(filePath))
                {
                    using (var stream = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        var xls = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                        var tbl = xls.AsDataSet().Tables[0];
                        TempData["DataExcel"] = tbl;
                    }
                }
            }            
            TempData["Message"] = "1 File Uploaded";
            return RedirectToAction("Index");
        }
    }
}