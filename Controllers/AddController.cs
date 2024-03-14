using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InForno.Models;
using System.Configuration;
using System.Transactions;

namespace InForno.Controllers
{
    public class AddController : Controller
    {

        public static string connectionString = ConfigurationManager.ConnectionStrings["DBContext"].ToString();

        // GET: Add
        public ActionResult Index()
        {
            List<Ingredienti> ingredienti = new List<Ingredienti>();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var command = new SqlCommand("SELECT IdIngrediente,Nome, Descrizione FROM Ingredienti", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ingrediente = new Ingredienti
                        {
                            IdIngrediente = (int)reader["IdIngrediente"],
                            Nome = reader["Nome"].ToString(),
                            Descrizione = reader["Descrizione"].ToString()
                        };
                        ingredienti.Add(ingrediente);
                    }
                }
            }

            ViewBag.Ingredienti = ingredienti;

            return View("~/Views/Add/AddArticolo.cshtml");
        }


        public ActionResult AddArticolo(Articolo articolo, List<int> idIngredienti)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string insertArticoloQuery = "INSERT INTO Articolo (Nome, Descrizione, Prezzo,TempoConsegna) VALUES (@nome, @descrizione, @prezzo, @tempoConsegna); SELECT SCOPE_IDENTITY()";
                        int articoloId;
                        using (var command = new SqlCommand(insertArticoloQuery, conn, transaction))
                        {
                            command.Parameters.AddWithValue("@nome", articolo.Nome);
                            command.Parameters.AddWithValue("@descrizione", articolo.Descrizione);
                            command.Parameters.AddWithValue("@prezzo", articolo.Prezzo);
                            command.Parameters.AddWithValue("@tempoConsegna", articolo.TempoConsegna);




                            articoloId = Convert.ToInt32(command.ExecuteScalar());
                        }


                        string insertIngredienteArticoloQuery = "INSERT INTO Ingrediente_Articolo (IdArticolo, IdIngrediente) VALUES (@idArticolo, @idIngrediente)";
                        foreach (int idIngrediente in idIngredienti)
                        {
                            using (var command = new SqlCommand(insertIngredienteArticoloQuery, conn, transaction))
                            {
                                command.Parameters.AddWithValue("@idArticolo", articoloId);
                                command.Parameters.AddWithValue("@idIngrediente", idIngrediente);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return View("~/Views/Home/Index.cshtml");
        }










        public ActionResult Ingrediente()
        {
            return View("~/Views/Add/AddIngrediente.cshtml");
        }



        public ActionResult AddIngrediente(Ingredienti ingrediente)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();


                var command = new SqlCommand("INSERT INTO Ingredienti (Nome, Descrizione) VALUES (@nome, @descrizione)", conn);
                command.Parameters.AddWithValue("@nome", ingrediente.Nome);
                command.Parameters.AddWithValue("@descrizione", ingrediente.Descrizione);
                command.ExecuteNonQuery();

            }

            return RedirectToAction("Index", "Home");
        }


        public ActionResult AddCarrello(List<int> IdArticolo, int quantita)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string idUtente = "asd";
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
                    cmdAddCarrello.ExecuteNonQuery();

                }






                conn.Close();


                conn.Open();

                foreach (int idArticolo in IdArticolo)
                {
                    //fixxare il login e aggiungerlo qui
                    var command = new SqlCommand("INSERT INTO ArticoloCarrello (IdArticolo, IdCarrello, Quantità) VALUES (@idArticolo, @idCarrello, @quantità)", conn);

                    {
                        command.Parameters.AddWithValue("@idArticolo", idArticolo);
                        command.Parameters.AddWithValue("@quantità", quantita);

                        command.Parameters.AddWithValue("@idCarrello", idCarrello);
                        command.ExecuteNonQuery();
                    }
                }
            }
            return View("~/Views/Home/Index.cshtml");
        }



    }
}
