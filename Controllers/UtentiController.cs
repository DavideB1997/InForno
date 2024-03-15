using InForno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InForno.Controllers
{
    public class UtentiController : Controller
    {
        DBContext db = new DBContext();


        //[Authorize(Roles = "0")]
        // GET: Utenti
        public ActionResult Index()
        {
            var utenti = db.Utentis.ToList();
            return View(utenti);
        }
    }
}