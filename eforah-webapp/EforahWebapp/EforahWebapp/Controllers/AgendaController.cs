using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EforahWebapp.Controllers
{
    public class AgendaController : Controller
    {
        // GET: Agenda
        public ActionResult Index(string agenda)
        {
            ViewBag.agendaLink = agenda;
            return View();
        }
    }
}