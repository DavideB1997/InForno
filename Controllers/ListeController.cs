using InForno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace InForno.Controllers
{
    public class ListeController : Controller
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DBContext"].ToString();


        // GET: Liste
        public ActionResult Articolo()
        {
            List<Articolo> articoli = new List<Articolo>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT a.*, i.*, i.Nome AS NomeIngrediente, i.Descrizione AS DescrizioneIngrediente " +
                    "FROM Articolo a " +
                    "JOIN Ingrediente_Articolo ia ON a.IdArticolo = ia.IdArticolo " +
                    "JOIN Ingredienti i ON ia.IdIngrediente = i.IdIngrediente", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idArticolo = (int)reader["IdArticolo"];

                        var articolo = articoli.FirstOrDefault(a => a.IdArticolo == idArticolo);

                        if (articolo == null)
                        {
                            articolo = new Articolo()
                            {
                                IdArticolo = idArticolo,
                                Nome = reader["Nome"].ToString(),
                                Descrizione = reader["Descrizione"].ToString(),
                                Prezzo = (int)reader["Prezzo"],
                                TempoConsegna = (int)reader["TempoConsegna"],
                                Ingredientis = new List<Ingredienti>()
                            };

                            articoli.Add(articolo);
                        }

                        var ingrediente = new Ingredienti()
                        {
                            Nome = reader["NomeIngrediente"].ToString(),
                            Descrizione = reader["DescrizioneIngrediente"].ToString(),
                        };

                        articolo.Ingredientis.Add(ingrediente);
                    }
                }
            }


            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            bool cookiePresente = (authCookie != null);

            ViewBag.CookiePresente = cookiePresente;

            return View("~/Views/Liste/Articolo.cshtml", articoli);
        }

        public ActionResult Carrello()
        {
            //select tutti gli oggetti nel carrello per poi buttarli in un ordine con tutti i dati dell'order
            ViewBag.Totale = 0;

            List<ArticoloDaCarrello> articoloDaCarrellos = new List<ArticoloDaCarrello>();
            int totale = 0;

            string idUtente = "";
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string userData = ticket.UserData;

                idUtente = userData;
            }
            else
            {
                return RedirectToAction("TestLogin", "Auth");
            }


            using (var conn = new SqlConnection(connectionString))
            {
                int idCarrello = 0;
                conn.Open();

                var cmdCarrello = new SqlCommand("SELECT IdCarrello FROM Carrello WHERE IdUtente = @idUtente", conn);

                cmdCarrello.Parameters.AddWithValue("@idUtente", idUtente);

                using (var reader = cmdCarrello.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idCarrello = (int)reader["IdCarrello"];
                    }
                }



                if (idCarrello == 0)
                {
                    var cmdAddCarrello = new SqlCommand("INSERT INTO Carrello (IdUtente) VALUES (@idUtente); SELECT SCOPE_IDENTITY();", conn);
                    cmdAddCarrello.Parameters.AddWithValue("@idUtente", idUtente);


                    idCarrello = Convert.ToInt32(cmdAddCarrello.ExecuteScalar());
                }


                conn.Close();



                var commandoCarrello = new SqlCommand("SELECT AC.*, A.Nome, A.Prezzo, A.Descrizione, A.TempoConsegna, C.Evaso, C.Totale FROM ArticoloCarrello AC INNER JOIN Articolo A ON AC.IdArticolo = A.IdArticolo INNER JOIN Carrello C ON AC.IdCarrello = C.IdCarrello WHERE AC.IdCarrello = @idCarrello AND C.Evaso = 0", conn);
                commandoCarrello.Parameters.AddWithValue("@idCarrello", idCarrello);

                conn.Open();


                using (var reader = commandoCarrello.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var articolo = new ArticoloDaCarrello
                        {
                            IdArticolo = (int)reader["IdArticolo"],
                            IdCarrello = (int)reader["IdCarrello"],
                            Quantità = (int)reader["Quantità"],
                            Prezzo = (int)reader["Prezzo"],
                            Nome = reader["Nome"].ToString(),
                            Descrizione = reader["Descrizione"].ToString(),
                            TempoConsegna = (int)reader["TempoConsegna"],
                            Evaso = (int)reader["Evaso"],
                            Totale = (int)reader["Prezzo"] * (int)reader["Quantità"]

                        };

                        totale += articolo.Totale;

                        articoloDaCarrellos.Add(articolo);
                    }
                }

                var cmdAddTotale = new SqlCommand("INSERT INTO Carrello (Totale) VALUES (@totale) where IdCarrello = @IdCarrello", conn);
                cmdAddTotale.Parameters.AddWithValue("@IdCarrello", idCarrello);
                cmdAddTotale.Parameters.AddWithValue("@totale", totale);


                idCarrello = Convert.ToInt32(cmdAddTotale.ExecuteScalar());





                ViewBag.ArticoliDaCarrello = articoloDaCarrellos;
                ViewBag.Totale = totale;

                conn.Close();


                return View("~/Views/Liste/Carrello.cshtml");
            }

        }


        public ActionResult OrdiniLista()
        {
            List<ArticoloOrdine> articoliOrdines = new List<ArticoloOrdine>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var commandOrdine = new SqlCommand("SELECT O.Indirizzo, O.Note, C.Evaso, C.Totale, U.Utente FROM Ordine O INNER JOIN Carrello C ON O.IdCarrello = C.IdCarrello INNER JOIN Utenti U ON C.IdUtente = U.Utente WHERE C.Evaso = 0", conn);

                using (var reader = commandOrdine.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var articolo = new ArticoloOrdine
                        {
                            Indirizzo = reader["Indirizzo"].ToString(),
                            Evaso = Convert.IsDBNull(reader["Evaso"]) ? 0 : (int)reader["Evaso"], 
                            Note = reader["Note"].ToString(),
                            Totale = Convert.IsDBNull(reader["Totale"]) ? 0 : Convert.ToInt32(reader["Totale"]), 
                            Utente = reader["Utente"].ToString(),
                        };
                        articoliOrdines.Add(articolo);
                    }
                }
            }

            ViewBag.ListaOrdini = articoliOrdines;

            return View("~/Views/Liste/OrdiniLista.cshtml");
        }

    }
}
