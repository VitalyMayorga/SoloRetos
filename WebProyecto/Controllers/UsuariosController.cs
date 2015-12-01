using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProyecto.Models;

namespace WebProyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private ProyectoWeb db = new ProyectoWeb();
        
        // GET: Usuarios
        public ActionResult Index()
        {
            var usuarios = db.Usuarios.Include(u => u.Equipos);
            return View(usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewBag.equipo_id = new SelectList(db.Equipos, "id", "nombre");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,apellido,telefono,correo,contraseña,equipo_id")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.equipo_id = new SelectList(db.Equipos, "id", "nombre", usuario.equipo_id);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.equipo_id = new SelectList(db.Equipos, "id", "nombre", usuario.equipo_id);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,apellido,telefono,correo,contraseña,equipo_id")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.equipo_id = new SelectList(db.Equipos, "id", "nombre", usuario.equipo_id);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Usuarios/Registrarse
        public ActionResult Registrarse()
        {
            ViewBag.equipo_id = new SelectList(db.Equipos, "id", "nombre");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrarse([Bind(Include = "id,nombre,apellido,telefono,correo,contraseña,equipo_id")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                //return RedirectToAction("Index");
                TempData["registro"] = "Usuario creado con éxito";
                return RedirectToAction("Nuevo_Usuario", "Home");
            }

            ViewBag.equipo_id = new SelectList(db.Equipos, "id", "nombre", usuario.equipo_id);
            
            return View(usuario);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Iniciar_Sesion(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Iniciar_Sesion(string returnUrl, string correo, string contraseña)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                /*var rol = (from q in db.Usuarios
                            where q.correo == correo && q.contraseña == contraseña
                            select q).Select(model => model.Rol).Single();
                Session["rol"] = rol.ToString();*/
                var usuario = (from q in db.Usuarios
                           where q.correo == correo && q.contraseña == contraseña
                           select q).Single();
                string nombre = usuario.nombre + " " + usuario.apellido;
                Session["rol"] = usuario.Rol.ToString();
                Session["usuario"] = nombre;
                return RedirectToAction("Index", "Home");
            }
            catch(Exception e) {
                ModelState.AddModelError("", "Usuario o contraseña inválidos");
                Session["rol"] = null;
                Session["usuario"] = null;
                return View();
            }
            
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["rol"] = null;
            Session["usuario"] = null;
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult getNombreEquipo (string codAcceso)
        {
            var result = from r in db.Equipos where r.codAcceso == codAcceso select new { r.nombre };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
