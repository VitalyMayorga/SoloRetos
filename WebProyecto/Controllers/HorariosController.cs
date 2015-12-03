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
    public class HorariosController : Controller
    {
        private ProyectoWeb db = new ProyectoWeb();

        // GET: Horarios
        public ActionResult Index()
        {
            var horarios = db.Horarios.Include(h => h.Cancha);
            return View(horarios.ToList());
        }

        // GET: Horarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horarios.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }

        // GET: Horarios/Create
        public ActionResult Create()
        {
            ViewBag.id_cancha = new SelectList(db.Canchas, "id", "nombre");
            return View();
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_cancha,horaInicio,horaFinal,precio")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                db.Horarios.Add(horario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cancha = new SelectList(db.Canchas, "id", "nombre", horario.id_cancha);
            return View(horario);
        }

        // GET: Horarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horarios.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cancha = new SelectList(db.Canchas, "id", "nombre", horario.id_cancha);
            return View(horario);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_cancha,horaInicio,horaFinal,precio")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(horario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cancha = new SelectList(db.Canchas, "id", "nombre", horario.id_cancha);
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horario = db.Horarios.Find(id);
            if (horario == null)
            {
                return HttpNotFound();
            }
            return View(horario);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Horario horario = db.Horarios.Find(id);
            db.Horarios.Remove(horario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DatosCancha(int id)
        {
            id = 1;
            var horarios = from h in db.Horarios
                           where h.id_cancha == id
                           select h; 
            return View(horarios.ToList());
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
