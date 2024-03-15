using InForno.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace InForno.Controllers
{
    public class AuthController : Controller
    {
        DBContext db = new DBContext();

        public static string connectionString = ConfigurationManager.ConnectionStrings["DBContext"].ToString();

        //bool keepLogged = false;


        // GET: Auth
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult TestLogin(Login login)
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            if (login.Utente != null && login.Password != null)
            {
                try
                {
                    var loggedUtente = db.Utentis.FirstOrDefault(u => u.Utente == login.Utente && u.Password == login.Password);

                    if (loggedUtente == null)
                    {
                        TempData["ErrorLogin"] = true;
                        return RedirectToAction("Login");
                    }

                    DateTime expirationDate = DateTime.Now.AddDays(15);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        loggedUtente.Utente.ToString(), // Assicurati che questo sia l'ID dell'utente o qualche altra informazione
                        DateTime.Now,
                        expirationDate,
                        true,
                        loggedUtente.Utente.ToString(),
                        FormsAuthentication.FormsCookiePath);

                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    authCookie.Expires = expirationDate;
                    Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Si è verificato un errore durante il tentativo di login.";
                    return RedirectToAction("Error", "Home");
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                HttpCookie authCookie = Response.Cookies[FormsAuthentication.FormsCookieName];
                authCookie.Expires = DateTime.Now.AddDays(-16); // Imposta la data di scadenza nel passato
                Response.Cookies.Add(authCookie);
                ViewBag.CookiePresente = null;
            }


            return View("~/Views/Home/Index.cshtml");

        }




        public ActionResult Reg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reg(string utente, string email, string password)
        {
            string ruolo = "User";


            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("INSERT INTO Utenti (Utente, Email, Password, Ruolo) VALUES (@utente, @email, @password, @ruolo)", conn);
                command.Parameters.Add("@utente", SqlDbType.NVarChar).Value = utente;
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                command.Parameters.Add("@ruolo", SqlDbType.NVarChar).Value = ruolo;

                command.ExecuteNonQuery();

                conn.Close();
            }

            return View("~/Views/Auth/TestLogin.cshtml");
        }



    }




}