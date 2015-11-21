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
            return View(db.Canchas.ToList());
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
            return View();
        }

        // POST: Canchas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,direccion,cantCanchas,telefono")] Cancha cancha)
        {
            if (ModelState.IsValid)
            {
                db.Canchas.Add(cancha);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            return View(cancha);
        }

        // POST: Canchas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,direccion,cantCanchas,telefono")] Cancha cancha)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cancha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
