using InForno.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InForno.Controllers
{
    public class APIController : Controller
    {
        DBContext db = new DBContext();


        // GET: API
        public JsonResult TotaleOrdini()
        {
            var result = db.Carrelloes.Where(o => o.Evaso == 1).Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Totale(DateTime d)
        {
            var results = db.Ordines.Where(o => o.DataOrdine == d).Count();
            return Json(results, JsonRequestBehavior.AllowGet);

        }
    }
}