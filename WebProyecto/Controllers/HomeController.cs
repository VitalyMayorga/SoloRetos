using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProyecto.Models;
using System.Data;
using System.Data.Entity;


namespace WebProyecto.Controllers
{
    public class HomeController : Controller
    {
        private ProyectoWeb db = new ProyectoWeb();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Nuevo_Usuario()
        { 
            return View();
        }

        public ActionResult Administrar()
        {
            return View();
        }

        public ActionResult getCanchas()
        {
            var canchas = from c in db.Canchas
                          select c;
            return PartialView("~/Views/Canchas/Index.cshtml", canchas.ToList());
        }

        public ActionResult getEquipos()
        {
            var equipos= from e in db.Equipos
                          select e;
            return PartialView("~/Views/Equipos/Index.cshtml", equipos.ToList());
        }

        public ActionResult getRetos()
        {
            var retos = from r in db.Retos
                          select r;
            return PartialView("~/Views/Retos/Index.cshtml", retos.ToList());
        }

        public ActionResult getUsuarios()
        {
            var usuarios = from u in db.Usuarios
                           select u;
            return PartialView("~/Views/Usuarios/Index.cshtml", usuarios.ToList());   
        }     
    }
}