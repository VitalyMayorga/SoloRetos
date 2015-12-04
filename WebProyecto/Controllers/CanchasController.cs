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
    public class CanchasController : Controller
    {
        private ProyectoWeb db = new ProyectoWeb();

        // GET: Canchas
        public ActionResult Index()
        {
            var canchas = db.Canchas.Include(c => c.Usuario);
            return View(canchas.ToList());
        }

        // GET: Canchas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cancha cancha = db.Canchas.Find(id);
            if (cancha == null)
            {
                return HttpNotFound();
            }
            return View(cancha);
        }

        // GET: Canchas/Create
        public ActionResult Create()
        {
            ViewBag.admCancha = new SelectList(db.Usuarios, "id", "nombre");
            return View();
        }

        // POST: Canchas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,direccion,cantCanchas,telefono,admCancha,provincia,canton")] Cancha cancha)
        {
            if (ModelState.IsValid)
            {
                db.Canchas.Add(cancha);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.admCancha = new SelectList(db.Usuarios, "id", "nombre", cancha.admCancha);
            return View(cancha);
        }

        // GET: Canchas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cancha cancha = db.Canchas.Find(id);
            if (cancha == null)
            {
                return HttpNotFound();
            }
            ViewBag.admCancha = new SelectList(db.Usuarios, "id", "nombre", cancha.admCancha);
            return View(cancha);
        }

        // POST: Canchas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,direccion,cantCanchas,telefono,admCancha,provincia,canton")] Cancha cancha)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cancha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.admCancha = new SelectList(db.Usuarios, "id", "nombre", cancha.admCancha);
            return View(cancha);
        }

        // GET: Canchas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cancha cancha = db.Canchas.Find(id);
            if (cancha == null)
            {
                return HttpNotFound();
            }
            return View(cancha);
        }

        // POST: Canchas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cancha cancha = db.Canchas.Find(id);
            db.Canchas.Remove(cancha);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Canchas/Lista
        public ActionResult Lista()
        {
            Cancha cancha = new Cancha();
            var prov = (from c in db.Canchas
                        select c).Select(model => model.provincia).Distinct();
            ViewBag.provincia = new SelectList(prov.ToList(),"provincia");
            var canton = (from c in db.Canchas
                          select c).Select(model => model.canton).Distinct();
            ViewBag.canton = new SelectList(canton.ToList(), "canton");
            var nombre = (from c in db.Canchas
                          select c).Select(model => model.nombre);
            ViewBag.nombre = new SelectList(nombre.ToList(), "nombre");
            return View(cancha);
        }
        //Llenar la lista de canchas, dependiendo de los primeros filtros seleccionados
        [HttpPost]
        public ActionResult llenarLista(string provincia,string canton)
        {

            if (provincia.Equals("--Elija una provincia--", StringComparison.Ordinal)) {
                if (canton.Equals("--Elija un cantón--", StringComparison.Ordinal))
                {
                    var nombre = (from c in db.Canchas
                                  select c).Select(model => model.nombre);
                    SelectList canchas = new SelectList(nombre.ToList(), "nombre");
                    return Json(canchas);
                }
                else {
                    var nombre = (from c in db.Canchas
                                  where c.canton == canton
                                  select c).Select(model => model.nombre);
                    SelectList canchas = new SelectList(nombre.ToList(), "nombre");
                    return Json(canchas);

                }
                
            }
            else {
                if (canton.Equals("--Elija un cantón--", StringComparison.Ordinal))
                {
                    var nombre = (from c in db.Canchas
                                  where c.provincia == provincia
                                  select c).Select(model => model.nombre);
                    SelectList canchas = new SelectList(nombre.ToList(), "nombre");
                    return Json(canchas);
                }
                else
                {
                    var nombre = (from c in db.Canchas
                                  where c.provincia == provincia && c.canton == canton
                                  select c).Select(model => model.nombre);
                    SelectList canchas = new SelectList(nombre.ToList(), "nombre");
                    return Json(canchas);

                }
               
            }
            
        }
        //muestra los datos de la cancha seleccionada
        [HttpPost]
        public ActionResult mostrarCancha(string nombre)
        {
            var datos = (from c in db.Canchas
                      where c.nombre == nombre
                      select c).Single();
            var horarios = from h in db.Horarios
                           where h.id_cancha == datos.id
                           select  h;
            
            
           
            return PartialView("~/Views/Horarios/DatosCancha.cshtml", horarios.ToList());

        }
        //Obtiene la dirección de la cancha seleccionada
        [HttpPost]
        public ActionResult getDireccion(string nombre)
        {
            var direccion = (from c in db.Canchas
                         where c.nombre == nombre
                         select c).Select(model => model.direccion).Single();
           
            
            return Json(direccion);

        }
        // GET: Canchas/AdmCanchas
        public ActionResult AdmCanchas()
        {
            Session["reto"] = "";
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
