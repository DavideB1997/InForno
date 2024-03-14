using InForno.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InForno.Controllers
{
    public class AuthController : Controller
    {
        DBContext db = new DBContext();

        bool keepLogged = false;


        // GET: Auth
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Utenti utente)
        {

            //metodo per loggare
            var loggedUtente = db.Utenti.Where(u => u.Utente == utente.Utente && u.Password == utente.Password).FirstOrDefault();

            if (loggedUtente == null)
            {
                TempData["ErrorLogin"] = true;
                return RedirectToAction("Login");
            }
            keepLogged = true;
            FormsAuthentication.SetAuthCookie(loggedUtente.Utente.ToString(), keepLogged);
            return RedirectToAction("Index", "Home");
        }
    }
}