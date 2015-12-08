using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
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
        public ActionResult Edit(int id,int id2)
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

        //muestra los datos del reto seleccionado
        [HttpPost]
        public ActionResult cargarRetos(string fecha)
        {
            string formato = "MM/dd/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime fechita = DateTime.ParseExact(fecha,formato,provider);
            int idCancha = Convert.ToInt32(Session["idCancha"]);
            
            var retos = from h in db.Retos
                           where h.id_cancha == idCancha && h.fecha == fechita /*&& h.id_equipo1 != h.id_equipo2*/
                           select h;



            return PartialView("~/Views/Retos/mostrarRetos.cshtml", retos.ToList());

        }

        [HttpPost]
        public ActionResult datosReto(int equipo1, int equipo2, int cancha, string fech, string horaI, string horaF)
        {
            Session["reto"] = equipo1 + "," + equipo2 + "," + cancha + "," + fech + "," + horaI + "," + horaF;
            var nombre = (from e in db.Equipos
                         where e.id == equipo1 || e.id == equipo2
                         select e).Select(model => model.nombre);


            SelectList datos = new SelectList(nombre.ToList(), "nombre");
            return Json(datos.ToList());

        }
        [HttpPost]
        public ActionResult borrarReto(int valor)
        {
            var datos = Session["reto"].ToString().Split(',');
            int eq1 = Convert.ToInt32(datos[0]);
            int eq2 = Convert.ToInt32(datos[1]);
            int cancha = Convert.ToInt32(datos[2]);
            DateTime fecha = DateTime.ParseExact(datos[3],"MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan horaI = TimeSpan.Parse(datos[4]);
            TimeSpan horaF = TimeSpan.Parse(datos[5]);
            Reto reto = db.Retos.Find(eq1,eq2,cancha,fecha,horaI,horaF);
            db.Retos.Remove(reto);
            db.SaveChanges();
            return RedirectToAction("AdmCanchas","Canchas");
        }

        [HttpPost]
        public ActionResult editarReto(string marcador, string ganador)
        {
            var datos = Session["reto"].ToString().Split(',');
            int eq1 = Convert.ToInt32(datos[0]);
            int eq2 = Convert.ToInt32(datos[1]);
            int cancha = Convert.ToInt32(datos[2]);
            DateTime fecha = DateTime.ParseExact(datos[3], "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan horaI = TimeSpan.Parse(datos[4]);
            TimeSpan horaF = TimeSpan.Parse(datos[5]);
            
            Reto reto = db.Retos.Find(eq1, eq2, cancha, fecha, horaI, horaF);
            reto.ganador = ganador;
            reto.resultado = marcador;
            db.SaveChanges();
            return RedirectToAction("AdmCanchas", "Canchas");
        }

        // GET: Retos/MiEquipo/5
        public ActionResult MiEquipo()
        {
            int idEquipo = Convert.ToInt32(Session["idEquipo"]);
            var nombreE = (from e in db.Equipos
                           where e.id == idEquipo
                           select e).Select(model => model.nombre).Single();
            ViewBag.Equipo = nombreE;
            var retos = from h in db.Retos
                         where h.id_equipo1 == idEquipo || h.id_equipo2 == idEquipo
                         orderby h.fecha ascending
                         select h;

            var datos = (from e in db.Equipos
                         where e.id == idEquipo
                         select e).Single();
            ViewBag.victorias = datos.victorias;
            ViewBag.empates = datos.empates;
            ViewBag.derrotas = datos.derrotas;
            return View(retos);
        }

        // GET: Retos/BuscarRetos/5
        public ActionResult BuscarRetos()
        {
            var prov = (from c in db.Canchas
                        select c).Select(model => model.provincia).Distinct();
            ViewBag.provincia = new SelectList(prov.ToList(), "provincia");
            var canton = (from c in db.Canchas
                          select c).Select(model => model.canton).Distinct();
            ViewBag.canton = new SelectList(canton.ToList(), "canton");
            var nombre = (from c in db.Canchas
                          select c).Select(model => model.nombre);
            ViewBag.nombre = new SelectList(nombre.ToList(), "nombre");

            return View();
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
        //carga los retos de la Cancha seleccionada en la fecha seleccionada
        [HttpPost]
        public ActionResult cargarRetosCancha(string fecha,string cancha)
        {
            string formato = "MM/dd/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime fechita = DateTime.ParseExact(fecha, formato, provider);
            int idEquipo = Convert.ToInt32(Session["idEquipo"]);
            var idCancha = (from c in db.Canchas
                           where c.nombre == cancha
                           select c).Select(model => model.id).Single();

            var retos = (from h in db.Retos
                        where h.id_cancha == idCancha && h.fecha == fechita && h.id_equipo1 != idEquipo && h.id_equipo2 !=idEquipo && h.id_equipo1== h.id_equipo2
                        orderby h.fecha ascending
                         select h);



            return PartialView("~/Views/Retos/retosDisponibles.cshtml", retos.ToList());

        }
        //Acepta un reto publicado
        [HttpPost]
        public ActionResult aceptarReto(int equipo1, int cancha, string fech, string horaI, string horaF)
        {
            DateTime fecha = DateTime.ParseExact(fech, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan horaInicio = TimeSpan.Parse(horaI);
            TimeSpan horaFinal = TimeSpan.Parse(horaF);
            int idEquipo = Convert.ToInt32(Session["idEquipo"]);
            Reto reto = db.Retos.Find(equipo1, equipo1, cancha, fecha, horaInicio, horaFinal);
            Reto reto2 = new Reto();
            reto2.id_equipo1 = reto.id_equipo1;
            reto2.id_equipo2 = idEquipo;
            reto2.id_cancha = reto.id_cancha;
            reto2.fecha = reto.fecha;
            reto2.horaInicio = reto.horaInicio;
            reto2.horaFinal = reto.horaFinal;
            reto2.ganador = reto.ganador;
            reto2.resultado = reto.resultado;
            reto2.precio = reto.precio;
            db.Retos.Add(reto2);
            db.SaveChanges();
            db.Retos.Remove(reto);
            db.SaveChanges();
            
            return RedirectToAction("BuscarRetos", "Retos");

        }

        //Publicar un nuevo reto
        [HttpPost]
        public ActionResult publicarReto(string cancha, string horaInicio, string horaFinal, string fecha)
        {
            string horaItmp = horaInicio.Substring(0, 5);
            string horaFtmp = horaFinal.Substring(0, 5);
            DateTime fech = DateTime.ParseExact(fecha, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            TimeSpan horaI = DateTime.ParseExact(horaItmp, "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay;
            TimeSpan horaF = DateTime.ParseExact(horaFtmp, "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay;
            int idEquipo = Convert.ToInt32(Session["idEquipo"]);
            var idCancha = (from c in db.Canchas
                            where c.nombre == cancha
                            select c).Select(model => model.id).Single();

            var precio = (from h in db.Horarios
                          where h.horaInicio <= horaI && horaF <= h.horaFinal && h.id_cancha == idCancha
                          select h).Select(model => model.precio).Single();
            Reto reto = new Reto();
            reto.id_equipo1 = idEquipo;
            reto.id_equipo2 = idEquipo;
            reto.id_cancha = idCancha;
            reto.fecha = fech;
            reto.horaInicio = horaI;
            reto.horaFinal = horaF;
            reto.ganador = "Pendiente";
            reto.resultado = "N/A";
            reto.precio = precio;
            db.Retos.Add(reto);
            db.SaveChanges();
            return RedirectToAction("BuscarRetos", "Retos");

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
