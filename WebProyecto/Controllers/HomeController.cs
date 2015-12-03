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

        public ActionResult getUsuarios()
        {
            var usuarios = db.Usuarios.Include(u => u.Equipos);
            return Json(usuarios.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}