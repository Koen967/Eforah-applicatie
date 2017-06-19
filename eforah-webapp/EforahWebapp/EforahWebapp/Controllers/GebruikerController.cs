using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EforahWebapp.Models;
using EforahWebapp.Services;

namespace EforahWebapp.Controllers
{
    public class GebruikerController : Controller
    {
        private eforahbetaalappEntities db;
        public GebruikerController()
        {
            db = new eforahbetaalappEntities();
        }

        public GebruikerController(eforahbetaalappEntities db) : this()
        {
            this.db = db;
        }

        // GET: Gebruiker
        public ActionResult Index()
        {
            var gebruiker = db.Gebruiker.Include(g => g.Locatie);
            return View(gebruiker.ToList());
        }

        // GET: Gebruiker/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gebruiker gebruiker = db.Gebruiker.Find(id);
            if (gebruiker == null)
            {
                return HttpNotFound();
            }
            return View(gebruiker);
        }

        // GET: Gebruiker/Create
        public ActionResult Create()
        {
            ViewBag.locatieId = new SelectList(db.Locatie, "locatieId", "postcode");
            return View();
        }

        // POST: Gebruiker/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "gebruikerId,locatieId,gebruikersnaam,wachtwoord,email,telefoonnummer,voornaam,achternaam,telefoonnummerAlt,foto")] Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                gebruiker.wachtwoord = HashServices.GetHashString(gebruiker.wachtwoord);
                db.Gebruiker.Add(gebruiker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.locatieId = new SelectList(db.Locatie, "locatieId", "postcode", gebruiker.locatieId);
            return View(gebruiker);
        }

        // GET: Gebruiker/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gebruiker gebruiker = db.Gebruiker.Find(id);
            if (gebruiker == null)
            {
                return HttpNotFound();
            }
            ViewBag.locatieId = new SelectList(db.Locatie, "locatieId", "postcode", gebruiker.locatieId);
            return View(gebruiker);
        }

        // POST: Gebruiker/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "gebruikerId,locatieId,gebruikersnaam,wachtwoord,email,telefoonnummer,voornaam,achternaam,telefoonnummerAlt,foto")] Gebruiker gebruiker)
        {
            if (ModelState.IsValid)
            {
                
                //gebruiker.wachtwoord = HashService.GetHashString(gebruiker.wachtwoord);
                db.Entry(gebruiker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.locatieId = new SelectList(db.Locatie, "locatieId", "postcode", gebruiker.locatieId);
            return View(gebruiker);
        }

        // GET: Gebruiker/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gebruiker gebruiker = db.Gebruiker.Find(id);
            if (gebruiker == null)
            {
                return HttpNotFound();
            }
            return View(gebruiker);
        }

        // POST: Gebruiker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gebruiker gebruiker = db.Gebruiker.Find(id);
            db.Gebruiker.Remove(gebruiker);
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
