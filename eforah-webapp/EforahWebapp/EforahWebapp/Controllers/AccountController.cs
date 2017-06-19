using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EforahWebapp.Models;
using System.Collections.Generic;
using System.Web.Services.Description;

namespace EforahWebapp.Controllers
{
    public class AccountController : Controller
    {
        #region Private Properties  
        /// <summary>  
        /// Database Store property.    
        /// </summary>  
        private eforahbetaalappEntities db;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>  
        public AccountController()
        {
            db = new eforahbetaalappEntities();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="entity">Enity framework to use</param>
        public AccountController(eforahbetaalappEntities entity) : this()
        {
            db = entity;
        }
        #endregion

        #region Login methods    
        /// <summary>  
        /// GET: /Account/Login    
        /// </summary>  
        /// <param name="returnUrl">Return URL parameter</param>  
        /// <returns>Return login view</returns>  
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                // Verification.    
                if (this.Request.IsAuthenticated)
                {
                    // Info.    
                    return this.RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }

            ViewBag.ReturnURL = returnUrl;

            // Info.    
            return this.View();
        }

        /// <summary>  
        /// POST: /Account/Login    
        /// </summary>  
        /// <param name="model">Model parameter</param>  
        /// <param name="returnUrl">Return URL parameter</param>  
        /// <returns>Return login view</returns>  
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                // Verification.    
                if (ModelState.IsValid)
                    {
                    //Hash
                    model.Wachtwoord = Services.HashServices.GetHashString(model.Wachtwoord);

                    // Initialization.
                    
                    //Haal gebruiker op met correcte gebruikersnaam en wachtwoord.
                    var loginInfo = db.Gebruiker.Where(g => g.gebruikersnaam == model.Gebruikersnaam && g.wachtwoord == model.Wachtwoord).FirstOrDefault();

                    //indien gebruiker gevonden zoek naar rechten binnen leden. Alleen Admin's mogen gebruik maken van webapp.
                    if (loginInfo != null)
                    {
                        //Check of gebruiker ergens een admin is.
                        var lid = db.Lid.Where(l => l.gebruikerId == loginInfo.gebruikerId && l.rol == "Admin").ToList();
                        if (lid.Any())
                        {
                            // Login In.    
                            this.SignInUser(loginInfo.gebruikersnaam, false);
                            
                            //Session onthoud welke verenigignen gebruiker admin is.
                            int[] verenigingen = new int[lid.Count];

                            //alle verenigingen afgaan.
                            for (int i = 0; i < lid.Count; i++)
                            {
                                //  //voeg id toe aan array.
                                verenigingen[i] = lid[i].verenigingId;
                            }
                            //voeg verenigingen ids toe aan sessie
                            SetSessionData("VerenigingIds", verenigingen);
                            // Info.    
                            return this.RedirectToLocal(returnUrl);
                        } else //Als gebruiker geen admin is dan is het geen correcte gebruiker.
                        {
                            // Setting.    
                            ModelState.AddModelError(string.Empty, "Onvoldoende rechten.");
                        }
                    } else
                    {
                        // Setting.    
                        ModelState.AddModelError(string.Empty, "Incorrecte gebruikersnaam of wachtwoord.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info    
                Console.Write(ex);
            }
            // If we got this far, something failed, redisplay form
            return this.View(model);
        }
        #endregion

        #region Log Out method.    
        /// <summary>  
        /// POST: /Account/LogOff    
        /// </summary>  
        /// <returns>Return log off action</returns>  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                // Setting.    
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign Out.    
                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Login", "Account");
        }
        #endregion

        #region Helpers    
        #region Sign In method.    
        /// <summary>  
        /// Sign In User method.    
        /// </summary>  
        /// <param name="username">Username parameter.</param>  
        /// <param name="isPersistent">Is persistent parameter.</param>  
        private void SignInUser(string username, bool isPersistent)
        {
            // Initialization.    
            var claims = new List<Claim>();
            try
            {
                // Setting    
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.    
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }
        #endregion
        #region Redirect to local method.    
        /// <summary>  
        /// Redirect to local method.    
        /// </summary>  
        /// <param name="returnUrl">Return URL parameter.</param>  
        /// <returns>Return redirection action</returns>  
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.    
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.    
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
            // Info.    
            return this.RedirectToAction("Index", "Home");
        }
        #endregion
        #region Session
        /// <summary>
        /// Set data for the session
        /// </summary>
        /// <param name="name">name of the data</param>
        /// <param name="data">the data</param>
        private void SetSessionData(String name, object data)
        {
            Session[name] = data;
        }
        #endregion
        #endregion

        #region Oude code
   
        #endregion
    }
}