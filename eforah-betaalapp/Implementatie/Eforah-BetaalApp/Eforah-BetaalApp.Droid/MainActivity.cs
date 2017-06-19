using Android.App;
using Android.Content;
using Android.Views;
using Android.OS;
using Android.Preferences;
using Eforah_BetaalApp.Droid.Controllers;
using Eforah_BetaalApp.Implementation;
using Newtonsoft.Json;

namespace Eforah_BetaalApp.Droid
{
    [Activity(Label = "Eforah Betaal App", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class MainActivity : Activity
    {
        #region Properties
        private ISharedPreferences prefs;
        private ISharedPreferencesEditor editor;
        private string SharedPreferenceUsername;
        private string SharedPreferencePassword;
        private int SharedPreferenceVerenigingID;
        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            editor = prefs.Edit();
            
            // Zet default settings voor login details
            SharedPreferenceUsername = prefs.GetString("Username", "");
            SharedPreferencePassword = prefs.GetString("Password", "");
            SharedPreferenceVerenigingID = prefs.GetInt("VerenigingId", 0);

            // Check of login details in sharedpreference staan. Ga vervolgens door naar QRcode scherm (MenuActivity het aanwezig is in sharedpreference)
            SavedPreferenceCheck();
        }

        #region saved preferences
        /// <summary>
        /// Indien ingelogd het ophalen van gegevens inclusief doorgaan naar volgende activity
        /// </summary>
        private async void SavedPreferenceContinue()
        {
            var tuple = await HttpRestService.LoginRequest(SharedPreferenceUsername, SharedPreferencePassword);
            if (tuple == null)
            {
                editor.Clear();
                editor.Commit();
                StartActivity(typeof(LoginActivity));
            }
            else
            {
                Intent intent = new Intent(this, typeof(QRActivity));

                //Serialize Tuple
                var SerializedTuple = JsonConvert.SerializeObject(tuple);
                intent.PutExtra("loginDetailTuple", SerializedTuple);

                //Serialize VerenigingId
                var SerializedId = JsonConvert.SerializeObject(SharedPreferenceVerenigingID);
                intent.PutExtra("verenigingId", SerializedId);

                StartActivity(intent);
            }
        }
        
        /// <summary>
        /// Check of login details in sharedpreference staan. Ga vervolgens door naar QRcode scherm (MenuActivity het aanwezig is in sharedpreference)
        /// </summary>
        private void SavedPreferenceCheck()
        {
            if (SharedPreferenceUsername != "" && SharedPreferencePassword != "" && SharedPreferenceUsername != null && SharedPreferencePassword != null && SharedPreferenceVerenigingID > 0)
            {
                SavedPreferenceContinue();
            }
            else
            {
                editor.Clear();
                editor.Commit();
                StartActivity(typeof(LoginActivity));
            }
        }
        #endregion
    }
}

