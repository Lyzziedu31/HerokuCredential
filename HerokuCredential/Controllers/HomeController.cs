using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;

namespace HerokuCredential.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string email, string password)
        {
            // Specify connection options and open an connection
            NpgsqlConnection conn = new NpgsqlConnection("Server=ec2-184-73-210-189.compute-1.amazonaws.com;User Id=zmyjxkdpvxmpbc;" +
                                    "Password=e16e5a91a76d6f782e1a388d22bd9c2fb7f22d8ebe21da5b0431f945e070d234;Database=d7l8o1gt48gnmj;sslmode=Require;Trust Server Certificate=true;");
            conn.Open();

            // Define a query
            NpgsqlCommand cmd = new NpgsqlCommand("select credential.email from credential where credential.email like '" + email +"' and credential.password like '"+ password +"';", conn);
           
            // Execute a query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // Read all rows and output the first column in each row
            if ((dr.Read()))
            {
                // Close connection
                conn.Close();
                ViewBag.message = "Bienvenue " + email;
                return PartialView("_PartialLoginResult", ViewBag.message);
            }
            // Close connection
            conn.Close();
            ViewBag.message = "user inconnu, veillez créer un compte";
            return PartialView("_PartialLoginResult", ViewBag.message);
         
        }

        public ActionResult Create(string email, string password)
        {
            
                // Specify connection options and open an connection
                NpgsqlConnection conn = new NpgsqlConnection("Server=ec2-184-73-210-189.compute-1.amazonaws.com;User Id=zmyjxkdpvxmpbc;" +
                                        "Password=e16e5a91a76d6f782e1a388d22bd9c2fb7f22d8ebe21da5b0431f945e070d234;Database=d7l8o1gt48gnmj;sslmode=Require;Trust Server Certificate=true;");
                conn.Open();

                // Define a query
                NpgsqlCommand cmd = new NpgsqlCommand("select credential.email from credential where credential.email like '" + email + "';", conn);

            // Execute a query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // Read all rows and output the first column in each row
            if ((dr.Read()))
            {
                // Close connection
                conn.Close();
                ViewBag.message = "Le login " + email + "existe déjà";
                return PartialView("_PartialLoginResult", ViewBag.message);
            }
            // Define a query
            NpgsqlCommand cmdwrite = new NpgsqlCommand("insert into credential values '" + email + "','" + password + "';", conn);

            // Execute a query
            NpgsqlDataReader drwrite = cmdwrite.ExecuteReader();

            // Close connection
            conn.Close();
            ViewBag.message = "utilisateur créé";
            return PartialView("_PartialLoginResult", ViewBag.message);
        }
    }
}
