using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Clona_p.Pages.Clona
{
    public class IndexModel : PageModel
    {
        public Postare infoPostare = new Postare();
        public string errorMessage = "";
        public string successMessage = "";

        public List<Postare> ListaPostari { get; set; }

        public void OnGet()
        {
            ListaPostari = new List<Postare>();

            try
            {
                string connectionString = "Data Source=DESKTOP-K9JQ7OB\\SQLEXPRESS;Initial Catalog=Pastebin!;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM postare";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        SqlDataReader dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            Postare postare = new Postare
                            {
                                id = dataReader["id"].ToString(),
                                titlu = dataReader["titlu"].ToString(),
                                autor = dataReader["autor"].ToString(),
                                textul = dataReader["textul"].ToString()
                            };

                            ListaPostari.Add(postare);
                        }

                        dataReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public IActionResult OnPost()
        {
            infoPostare.titlu = Request.Form["titlu"];
            infoPostare.autor = Request.Form["autor"];
            infoPostare.textul = Request.Form["textul"];

            if (string.IsNullOrEmpty(infoPostare.titlu) || string.IsNullOrEmpty(infoPostare.autor) || string.IsNullOrEmpty(infoPostare.textul))
            {
                errorMessage = "All fields are required";
                return Page();
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-K9JQ7OB\\SQLEXPRESS;Initial Catalog=Pastebin!;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO postare " +
                                 "(titlu, autor, textul) VALUES " +
                                 "(@titlu, @autor, @textul);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@titlu", infoPostare.titlu);
                        command.Parameters.AddWithValue("@autor", infoPostare.autor);
                        command.Parameters.AddWithValue("@textul", infoPostare.textul);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            infoPostare.titlu = "";
            infoPostare.autor = "";
            infoPostare.textul = "";
            successMessage = "New Post Added Correctly";

            return RedirectToPage("/Clona!/Index");
        }
    }

    public class Postare
    {
        public string id { get; set; }
        public string titlu { get; set; }
        public string autor { get; set; }
        public string textul { get; set; }
    }
}
