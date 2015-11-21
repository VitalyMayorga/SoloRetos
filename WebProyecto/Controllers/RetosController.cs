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
    public class RetosController : Controller
    {
        private ProyectoWeb db = new ProyectoWeb();

        // GET: Retos
        public ActionResult Index()
        {
            var retos = db.Retos.Include(r => r.Cancha).Include(r => r.Equipos).Include(r => r.Equipos1);
            return View(retos.ToList());
        }

        // GET: Retos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reto reto = db.Retos.Find(id);
            if (reto == null)
            {
                return HttpNotFound();
            }
            return View(reto);
        }

        // GET: Retos/Create
        public ActionResult Create()
        {
            ViewBag.id_cancha = new SelectList(db.Canchas, "id", "nombre");
            ViewBag.id_equipo1 = new SelectList(db.Equipos, "id", "nombre");
            ViewBag.id_equipo2 = new SelectList(db.Equipos, "id", "nombre");
            return View();
        }

        // POST: Retos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_equipo1,id_equipo2,id_cancha,fecha,horaInicio,horaFinal,precio")] Reto reto)
        {
            if (ModelState.IsValid)
            {
                db.Retos.Add(reto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cancha = new SelectList(db.Canchas, "id", "nombre", reto.id_cancha);
            ViewBag.id_equipo1 = new SelectList(db.Equipos, "id", "nombre", reto.id_equipo1);
            ViewBag.id_equipo2 = new SelectList(db.Equipos, "id", "nombre", reto.id_equipo2);
            return View(reto);
        }

        // GET: Retos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reto reto = db.Retos.Find(id);
            if (reto == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cancha = new SelectList(db.Canchas, "id", "nombre", reto.id_cancha);
            ViewBag.id_equipo1 = new SelectList(db.Equipos, "id", "nombre", reto.id_equipo1);
            ViewBag.id_equipo2 = new SelectList(db.Equipos, "id", "nombre", reto.id_equipo2);
            return View(reto);
        }

        // POST: Retos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_equipo1,id_equipo2,id_cancha,fecha,horaInicio,horaFinal,precio")] Reto reto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cancha = new SelectList(db.Canchas, "id", "nombre", reto.id_cancha);
            ViewBag.id_equipo1 = new SelectList(db.Equipos, "id", "nombre", reto.id_equipo1);
            ViewBag.id_equipo2 = new SelectList(db.Equipos, "id", "nombre", reto.id_equipo2);
            return View(reto);
        }

        // GET: Retos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reto reto = db.Retos.Find(id);
            if (reto == null)
            {
                return HttpNotFound();
            }
            return View(reto);
        }

        // POST: Retos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reto reto = db.Retos.Find(id);
            db.Retos.Remove(reto);
            db.SaveChanges();
            return RedirectToAction("Index");
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
