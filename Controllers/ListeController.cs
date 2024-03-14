using InForno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;

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
                                Ingredienti = new List<Ingredienti>()
                            };

                            articoli.Add(articolo);
                        }

                        var ingrediente = new Ingredienti()
                        {
                            Nome = reader["NomeIngrediente"].ToString(),
                            Descrizione = reader["DescrizioneIngrediente"].ToString(),
                        };

                        articolo.Ingredienti.Add(ingrediente);
                    }
                }
            }

            return View("~/Views/Liste/Articolo.cshtml", articoli);
        }

        public ActionResult Carrello()
        {
            List<Articolo> articoli = new List<Articolo>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand(
                   "SELECT * FROM Carrello where IdUtente = @idUtente", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var carrello = new Carrello()
                        {
                            Totale = (int)reader["Totale"],
                            IdUtente = reader["IdUtente"].ToString(),
                        };
                    }
                }









                return View();
            }

        }
    }
}