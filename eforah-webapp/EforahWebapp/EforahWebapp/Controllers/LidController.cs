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
    [Authorize]
    public class LidController : Controller
    {
        private eforahbetaalappEntities db;
        private HttpSessionStateBase session;

        public LidController()
        {
            db = new eforahbetaalappEntities();
            session = Session;
        }

        public LidController(eforahbetaalappEntities entity) : this()
        {
            db = entity;
        }

        public LidController(eforahbetaalappEntities entity, HttpSessionStateBase pSession) : this(entity)
        {
            session = pSession;
        }
        
        /// <summary>
        /// GET: Lid
        /// </summary>
        /// <param name="id">Id van de vereniging voorwaar de lijst moet worden gezien.</param>
        /// <returns>Theview</returns>
        public ViewResult Index()
        {
            if (session == null)
                session = Session;
                var verenigingIds = session["VerenigingIds"] as int[];
                var leden = new List<Lid>();
      

                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];
                        List<Lid> vList = db.Lid.Where(l => l.verenigingId.Equals(vid)).ToList();

                        leden.AddRange(vList);
                    }
                }
            return View(leden);
        }

        // GET: Lid/Details/5
        public ActionResult Details(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lid lid = db.Lid.Find(id);

            if (lid != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];
                    
                        if (lid.verenigingId.Equals(vid))
                        {
                            return View(lid);
                        }
                    }
                }
            }     
            return View("Error"); 
        }

        // GET: Lid/Create
        public ActionResult Create()
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            ViewBag.rol = new SelectList(getRolDropdown());
            ViewBag.gebruikerId = new SelectList(db.Gebruiker, "gebruikerId", "gebruikersnaam");

            var verenigingen = new List<Vereniging>();
            for (int i = 0; i < verenigingIds.Length; i++)
            {
                var count = verenigingIds[i];
                var ver = db.Vereniging.Where(v => v.verenigingId.Equals(count)).ToList();
                verenigingen.AddRange(ver);
            }

            ViewBag.verenigingId = new SelectList(verenigingen, "verenigingId", "naam");
            return View();
        }

        // POST: Lid/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lidId,gebruikerId,verenigingId,saldo,rol,foto")] Lid lid)
        {
            if (ModelState.IsValid)
            {
                db.Lid.Add(lid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.rol = new SelectList(getRolDropdown(), lid.rol);
            ViewBag.gebruikerId = new SelectList(db.Gebruiker, "gebruikerId", "gebruikersnaam", lid.gebruikerId);
            ViewBag.verenigingId = new SelectList(db.Vereniging, "verenigingId", "naam", lid.verenigingId);
            return View(lid);
        }

        // GET: Lid/Edit/5
        public ActionResult Edit(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lid lid = db.Lid.Find(id);

            if (lid != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];

                        if (lid.verenigingId.Equals(vid))
                        {
                            ViewBag.rol = new SelectList(getRolDropdown(), lid.rol);
                            return View(lid);
                        }
                    }
                }
            }
            return View("Error");
        }

        // POST: Lid/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lidId,gebruikerId,verenigingId,saldo,rol,foto")] Lid lid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.rol = new SelectList(getRolDropdown(), lid.rol);
            return View(lid);
        }

        // GET: Lid/Delete/5
        public ActionResult Delete(int? id)
        {
            if (session == null)
                session = Session;
            var verenigingIds = session["VerenigingIds"] as int[];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lid lid = db.Lid.Find(id);

            if (lid != null)
            {
                if (verenigingIds != null)
                {
                    for (int i = 0; i < verenigingIds.Length; i++)
                    {
                        int vid = verenigingIds[i];

                        if (lid.verenigingId.Equals(vid))
                        {
                            return View(lid);
                        }
                    }
                }
            }
            return View("Error");
        }

        // POST: Lid/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lid lid = db.Lid.Find(id);
            db.Lid.Remove(lid);
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

        private IEnumerable<string> getRolDropdown()
        {
            List<string> rollenList = new List<string>();

            rollenList.Add("Lid");
            rollenList.Add("Admin");
            rollenList.Add("Medewerker");

            IEnumerable<string> rollen = rollenList;

            return rollen;
        }
    }
}
