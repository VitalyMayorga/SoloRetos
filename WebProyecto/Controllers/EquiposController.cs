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
    public class EquiposController : Controller
    {
        private ProyectoWeb db = new ProyectoWeb();

        // GET: Equipos
        public ActionResult Index()
        {
            return View(db.Equipos.ToList());
        }

        // GET: Equipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return View(equipos);
        }

        // GET: Equipos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Equipos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,codAcceso")] Equipos equipos)
        {
            if (ModelState.IsValid)
            {
                db.Equipos.Add(equipos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipos);
        }

        // GET: Equipos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return View(equipos);
        }

        // POST: Equipos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,codAcceso")] Equipos equipos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Administrar", "Home");
            }
            return View(equipos);
        }

        // GET: Equipos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return View(equipos);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipos equipos = db.Equipos.Find(id);
            db.Equipos.Remove(equipos);
            db.SaveChanges();
            return RedirectToAction("Administrar", "Home");
        }
        //Obtiene el telefono de los equipos
        [HttpPost]
        public ActionResult datosEquipos(int equipo1)
        {
            var tel = (from e in db.Usuarios
                          where e.equipo_id == equipo1 
                          select e).Select(model => model.telefono);


            SelectList datos = new SelectList(tel.ToList(), "telefono");
            return Json(datos.ToList());

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Lista()
        {
            Cancha cancha = new Cancha();
            var prov = (from c in db.Canchas
                        select c).Select(model => model.provincia).Distinct();
            ViewBag.provincia = new SelectList(prov.ToList(), "provincia");
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
        public ActionResult llenarLista(string provincia, string canton)
        {

            if (provincia.Equals("--Elija una provincia--", StringComparison.Ordinal))
            {
                if (canton.Equals("--Elija un cantón--", StringComparison.Ordinal))
                {
                    var nombre = (from c in db.Canchas
                                  select c).Select(model => model.nombre);
                    SelectList canchas = new SelectList(nombre.ToList(), "nombre");
                    return Json(canchas);
                }
                else
                {
                    var nombre = (from c in db.Canchas
                                  where c.canton == canton
                                  select c).Select(model => model.nombre);
                    SelectList canchas = new SelectList(nombre.ToList(), "nombre");
                    return Json(canchas);

                }

            }
            else
            {
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
        public ActionResult mostrarEquipos(string nombre)
        {
            DateTime fecha1 = new DateTime(2015,11,30);
            DateTime fecha2 = new DateTime(2016, 1, 1);
            List<Equipos> model = new List<Equipos>();
            var datos = (from c in db.Canchas
                         where c.nombre == nombre
                         select c).Single();
            var equipos =   from e in db.Equipos
                            join r in db.Retos
                            on e.nombre equals r.ganador
                            where (r.id_cancha == datos.id && r.fecha > fecha1 && r.fecha < fecha2)
                            group new {e} by new { e.nombre }
                                into resultSet
                                orderby resultSet.Count() descending
                                select new
                                {
                                    nombre = resultSet.Key.nombre,
                                    victorias = resultSet.Count()
                                };
            foreach(var item in equipos) //retrieve each item and assign to model
            {
                model.Add(new Equipos()
                {
                    nombre = item.nombre,
                    victorias = item.victorias,      
                });
            }
            return PartialView("~/Views/Equipos/DatosCancha.cshtml", model);
        }
    }
}
