using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Eforah_BetaalApp.Implementation.Models;
using Newtonsoft.Json;
using Eforah_BetaalApp.Implementation;
using Android.Preferences;
using System.Security.Cryptography;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "LoginActivity", NoHistory = true)]
    public class LoginActivity : Activity
    {
        #region properties
        private Button loginButton;
        private EditText uni;
        private string usernameinput;
        private EditText unp;
        private string passwordinput;
        private TextView output;
        private string hashedPassword;

        private ISharedPreferences prefs;
        private ISharedPreferencesEditor editor;
        private string SharedPreferenceUsername;
        private string SharedPreferencePassword;
        private int SharedPreferenceVerenigingID;
        #endregion

        #region Constants
        private static string OUTPUTTEXTSUCCESS = "Het inloggen is gelukt";
        private static string OUTPUTTEXTFAILURE = "Het wachtwoord of gebruikersnaam is onjuist";
        private static string ERRORMESSAGEPASSWORDNULL = "Password is null";
        private static string ERRORMESSAGEUSERNAMENULL = "Username is null";
        private static string ERRORMESSAGEPASSWORD = "Password is empty";
        private static string ERRORMESSAGEUSERNAME = "Username is empty";
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            editor = prefs.Edit();

            // Maak de Main.axml binnen de layout resource de view.
            SetContentView(Resource.Layout.Main);

            // Haal knop uit Main.axml layout resource om vervolgens mee te werken.
            loginButton = FindViewById<Button>(Resource.Id.loginButton);


            // Volgende actie begint wanneer login knop wordt ingedrukt.
            loginButton.Click += async delegate
            {
                // Haal EditText/EditText/TextView uit Main.axml layout resource om vervolgens mee te werken.
                uni = FindViewById<EditText>(Resource.Id.username);
                usernameinput = uni.Text;
                unp = FindViewById<EditText>(Resource.Id.password);
                passwordinput = unp.Text;

                // Wachtwoord hashen
                hashedPassword = GetHashStringLogin(passwordinput);

                // Ophalen van login detail tuple
                var tuple = await HttpRestService.LoginRequest(usernameinput, hashedPassword);
                if (tuple == null)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Inloggen mislukt");
                    alert.SetMessage("controller uw gegevens en kijk of u een werkende internet connectie heeft.");
                    alert.SetPositiveButton("Oke", (senderAlert, args) => { });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    // Controle of ingevulde wachtwoord en gebruikersnaam een bestaande valide combinatie zijn en vervolgens doorsturen naar volgende activity.
                    TupleControle(tuple);
                }
            };
        }

        #region login details
        /// <summary>
        /// Controle of ingevulde wachtwoord en gebruikersnaam een bestaande valide combinatie zijn.
        /// </summary>
        /// <param name="tuple"></param>
        public void TupleControle(Tuple<GebruikerModel, List<VerenigingModel>> tuple)
        {
            if (tuple != null)
            {
                // Inlog gegevens opslaan
                editor.PutString("Username", usernameinput);
                editor.PutString("Password", hashedPassword);
                editor.Apply();

                // Aanroepen van volgende activity
                Intent intent = new Intent(this, typeof(VerenigingSelectionActivity));
                var MySerializedObject = JsonConvert.SerializeObject(tuple);
                intent.PutExtra("loginDetailTuple", MySerializedObject);

                StartActivity(intent);
            }
        }

        /// <summary>
        /// Password & username input controle
        /// </summary>
        /// <param name="passwordinput"></param>
        /// <param name="usernameinput"></param>
        public void NoLoginDetailsControle(string passwordinput, string usernameinput)
        {
            if (passwordinput == null)
            {
                throw new System.ArgumentException(ERRORMESSAGEPASSWORDNULL, "passwordinput");
            }
            else if (passwordinput == "")
            {
                throw new System.ArgumentException(ERRORMESSAGEPASSWORD, "passwordinput");
            }
            else if (usernameinput == null)
            {
                throw new System.ArgumentException(ERRORMESSAGEUSERNAMENULL, "usernameinput");
            }
            else if (usernameinput == "")
            {
                throw new System.ArgumentException(ERRORMESSAGEUSERNAME, "usernameinput");
            }
        }
        #endregion

        #region Hash
        /// <summary>
        /// Het hashen van het wachtwoord
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static byte[] GetHashLogin(string inputString)
        {
            HashAlgorithm algorithm = SHA512.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        
        /// <summary>
        /// Het hashen van het wachtwoord
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string GetHashStringLogin(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHashLogin(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        #endregion
    }
}