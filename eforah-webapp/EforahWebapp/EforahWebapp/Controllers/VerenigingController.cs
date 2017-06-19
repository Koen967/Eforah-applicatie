using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EforahWebapp.Models;
using System.Collections;

namespace EforahWebapp.Controllers
{
    [Authorize]
    public class VerenigingController : Controller
    {
        private eforahbetaalappEntities db;
        private HttpSessionStateBase session;

        /// <summary>
        /// Default constructor
        /// </summary>
        public VerenigingController()
        {
            db = new eforahbetaalappEntities();
            session = Session;
        }

        /// <summary>
        /// Constructor voornamelijk bedoelt voor testen
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pSession"></param>
        public VerenigingController(eforahbetaalappEntities entity) : this()
        {
            db = entity;
        }

        /// <summary>
        /// Constructor voornamelijk bedoelt voor testen van Sessies
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pSession"></param>
        public VerenigingController(eforahbetaalappEntities entity, HttpSessionStateBase pSession) : this(entity)
        {
            session = pSession;
        }

        // GET: Vereniging
        public ViewResult Index()
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];
            var verenigingen = new List<Vereniging>();

            if (verenigingIds != null)
            {
                for (int i = 0; i < verenigingIds.Length; i++)
                {
                    int vid = verenigingIds[i];
                    List<Vereniging> vList = db.Vereniging.Include(v => v.Locatie).Where(v => v.verenigingId.Equals(vid)).ToList();

                    verenigingen.AddRange(vList);
                }
            }

            return View(verenigingen);
        }

        // GET: Vereniging/Details/5
        public ActionResult Details(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vereniging vereniging = db.Vereniging.Find(id);

            if (vereniging != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];

                        if (vereniging.verenigingId.Equals(vid))
                        {
                            return View(vereniging);
                        }
                    }
                }
            }
            return View("Error");
        }

        // GET: Vereniging/Create
        public ActionResult Create()
        {
            ViewBag.locatieId = new SelectList(db.Locatie, "locatieId", "postcode");
            return View();
        }

        // POST: Vereniging/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "verenigingId,locatieId,naam,facebookAdminId,facebookGroupId,agendaLink,telefoonnummer,email")] Vereniging vereniging)
        public ActionResult Create([Bind(Include = "naam,telefoonnummer,email")] Vereniging vereniging, [Bind(Include = "postcode,huisnummer,adres,plaats")] Locatie locatie)
        {
            if (ModelState.IsValid)
            {
                //creeer nieuwe locatie, zet naar database
                db.Locatie.Add(locatie);
                db.SaveChanges();

                //haal de gegenereerde id op
                vereniging.locatieId = locatie.locatieId;

                db.Vereniging.Add(vereniging);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.locatieId = new SelectList(db.Locatie, "locatieId", "postcode", vereniging.locatieId);
            return View(vereniging);
        }

        // GET: Vereniging/Edit/5
        public ActionResult Edit(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vereniging vereniging = db.Vereniging.Find(id);

            if (vereniging != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];

                        if (vereniging.verenigingId.Equals(vid))
                        {
                            ViewBag.locatieId = new SelectList(db.Locatie, "locatieId", "UniqueAdres", vereniging.locatieId);
                            return View(vereniging);
                        }
                    }
                }
            }
            return View("Error");
        }

        // POST: Vereniging/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "verenigingId,locatieId,wachtwoord,naam,facebookAdminId,facebookGroupId,agendaLink,telefoonnummer,email")] Vereniging vereniging)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vereniging).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.locatieId = new SelectList(db.Locatie, "locatieId", "UniqueAdres", vereniging.locatieId);
            return View(vereniging);
        }

        // GET: Vereniging/Delete/5
        public ActionResult Delete(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vereniging vereniging = db.Vereniging.Find(id);

            if (vereniging != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];

                        if (vereniging.verenigingId.Equals(vid))
                        {
                            return View(vereniging);
                        }
                    }
                }
            }
            return View("Error");
        }

        // POST: Vereniging/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vereniging vereniging = db.Vereniging.Find(id);
            db.Vereniging.Remove(vereniging);
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
