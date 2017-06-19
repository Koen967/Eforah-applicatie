using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EforahWebapp.Models;

namespace EforahWebapp.Controllers
{
    public class MededelingController : Controller
    {
        private eforahbetaalappEntities db;
        private HttpSessionStateBase session;

        public MededelingController()
        {
            db = new eforahbetaalappEntities();
        }

        public MededelingController(eforahbetaalappEntities db)
        {
            this.db = db;
        }

        public MededelingController(eforahbetaalappEntities db, HttpSessionStateBase pSession) : this(db)
        {
            session = pSession;

        }

        // GET: Mededeling
        public ActionResult Index()
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];
            var mededelingen = new List<Mededeling>();

            if (verenigingIds != null)
            {
                for (int i = 0; i < verenigingIds.Length; i++)
                {
                    int vid = verenigingIds[i];
                    List<Mededeling> vList = db.Mededeling.Where(m => m.verenigingId.Equals(vid)).ToList();

                    mededelingen.AddRange(vList);
                }
            }
            return View(mededelingen);
        }

        // GET: Mededeling/Details/5
        public ActionResult Details(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mededeling mededeling = db.Mededeling.Find(id);

            if (mededeling != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];

                        if (mededeling.verenigingId.Equals(vid))
                        {
                            return View(mededeling);
                        }
                    }
                }
            }
            return View("Error");
        }

        // GET: Mededeling/Create
        public ActionResult Create()
        {
            ViewBag.verenigingId = new SelectList(db.Vereniging, "verenigingId", "naam");
            return View();
        }

        // POST: Mededeling/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "mededelingId,verenigingId,plaatsingDatum,titel,mededeling1")] Mededeling mededeling)
        {
            if (ModelState.IsValid)
            {
                mededeling.plaatsingDatum = DateTime.Now;
                db.Mededeling.Add(mededeling);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.verenigingId = new SelectList(db.Vereniging, "verenigingId", "naam", mededeling.verenigingId);
            return View(mededeling);
        }

        // GET: Mededeling/Edit/5
        public ActionResult Edit(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mededeling mededeling = db.Mededeling.Find(id);

            if (mededeling != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];

                        if (mededeling.verenigingId.Equals(vid))
                        {
                            ViewBag.verenigingId = new SelectList(db.Vereniging, "verenigingId", "naam", mededeling.verenigingId);
                            return View(mededeling);
                        }
                    }
                }
            }
            return View("Error");
        }

        // POST: Mededeling/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "mededelingId,verenigingId,plaatsingDatum,titel,mededeling1")] Mededeling mededeling)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mededeling).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.verenigingId = new SelectList(db.Vereniging, "verenigingId", "naam", mededeling.verenigingId);
            return View(mededeling);
        }

        // GET: Mededeling/Delete/5
        public ActionResult Delete(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mededeling mededeling = db.Mededeling.Find(id);

            if (mededeling != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];

                        if (mededeling.verenigingId.Equals(vid))
                        {
                            return View(mededeling);
                        }
                    }
                }
            }
            return View("Error");
        }

        // POST: Mededeling/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mededeling mededeling = db.Mededeling.Find(id);
            db.Mededeling.Remove(mededeling);
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
