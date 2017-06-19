using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Content.PM;
using Eforah_BetaalApp.Implementation.Models;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Content;
using Android.Preferences;
using Newtonsoft.Json.Linq;
using Eforah_BetaalApp.Implementation;
using Eforah_BetaalApp.Droid.Components;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "QR Code", ScreenOrientation = ScreenOrientation.Portrait)]
    public class QRActivity : AppCompatActivity
    {
        DrawerLayout drawerLayout;
        Tuple<GebruikerModel, List<VerenigingModel>> localTuple;
        VerenigingModel localVereniging;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Create UI
            SetContentView(Resource.Layout.QRView);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.mainToolbar);
            SetSupportActionBar(toolbar);
            toolbar.FindViewById<TextView>(Resource.Id.toolbarTitle).Text = SupportActionBar.Title;
            SupportActionBar.Title = "";

            drawerLayout.CloseDrawers();

            // Attach item selected handler to navigation view
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            // var tuple = JsonSerializer.DeserializeFromString<Tuple<GebruikerModel, List<VerenigingModel>, HttpResponseMessage>>(Intent.GetStringExtra("loginDetailTuple"));
            var JsonStringLoginDetail = Intent.GetStringExtra("loginDetailTuple");
            var loginDetailData = JObject.Parse(JsonStringLoginDetail);
            Tuple<GebruikerModel, List<VerenigingModel>> tuple = loginDetailData.ToObject<Tuple<GebruikerModel, List<VerenigingModel>>>();
            //var tuple = JsonConvert.DeserializeObject<Tuple<GebruikerModel, List<VerenigingModel>>>(MyJsonString);
            localTuple = tuple;


            //Vereniging Id
            var JsonStringVerenigingId = Intent.GetStringExtra("verenigingId");
            int verenigingId = int.Parse(JsonStringVerenigingId);

            //Vind gekozen vereniging
            foreach (var v in localTuple.Item2)
            {
                if(verenigingId == v.verenigingId)
                {
                    localVereniging = v;
                }
            }

            var menu = navigationView.Menu;
            var scannerItem = menu.FindItem(Resource.Id.nav_scanner);
            if (localVereniging.rol == "Lid")
            {
                scannerItem.SetVisible(false);
            }

            FindViewById<TextView>(Resource.Id.vereniging_naam).Text = localVereniging.naam;
            //navigationView.GetHeaderView(0).FindViewById<TextView>(Resource.Id.headerText).Text = localVereniging.naam;

            string contentQR = localVereniging.lidId.ToString();
            //Get the ImageView where the QR code will be displayed
            ImageView QRView = FindViewById<ImageView>(Resource.Id.QRView);
            //Generate the QR Code and display it
            QRView.SetImageBitmap(GetQRCode(contentQR));
        }

        #region navigation
        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            Intent intent;
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_scanner):
                    intent = new Intent(this, typeof(AfrekenenActivity));
                    var SerializedVereniging = JsonConvert.SerializeObject(localVereniging);
                    intent.PutExtra("verenigingId", SerializedVereniging);
                    StartActivity(intent);
                    break;
                case (Resource.Id.nav_club_select):
                    intent = new Intent(this, typeof(VerenigingSelectionActivity));
                    intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

                    //Serialize Tuple
                    var SerializedTuple = JsonConvert.SerializeObject(localTuple);
                    intent.PutExtra("loginDetailTuple", SerializedTuple);

                    StartActivity(intent);
                    break;

                case (Resource.Id.nav_announcements):
                    NavigateToMededelingAsync();
                    break;

                case (Resource.Id.nav_saldo):
                    NavigateToSaldoUI();
                    break;

                case (Resource.Id.nav_members):
                    NavigateToMembersAsync();
                    break;

                case (Resource.Id.nav_facebook):
                    NavigateToFacebookFeedUI();
                    break;

                case (Resource.Id.nav_login):
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.Clear();
                    editor.Commit();

                    //Ga terug naar login
                    intent = new Intent(this, typeof(MainActivity));
                    intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                    StartActivity(intent);
                    //Finish();
                    break;

                case (Resource.Id.nav_agenda):
                    intent = new Intent(this, typeof(AgendaActivity));
                    var SerializedVereniging1 = JsonConvert.SerializeObject(localVereniging);
                    intent.PutExtra("vereniging", SerializedVereniging1);
                    StartActivity(intent);
                    break;
            }

            // Close drawer
            drawerLayout.CloseDrawers();
        }

        /// <summary>
        /// Navigeer naar de Leden Lijst van de huidige vereniging.
        /// </summary>
        private void NavigateToMembersAsync()
        {
            Intent intent = new Intent(this, typeof(LedenActivity));
            intent.PutExtra("verenigingNaam", localVereniging.naam);
            intent.PutExtra("verenigingId", localVereniging.verenigingId.ToString());

            StartActivity(intent);
        }
        private void NavigateToMededelingAsync()
        {
            Intent intent = new Intent(this, typeof(MededelingActivity));
            intent.PutExtra("verenigingNaam", localVereniging.naam);
            intent.PutExtra("verenigingId", localVereniging.verenigingId.ToString());

            StartActivity(intent);
        }

        private async void NavigateToFacebookFeedUI()
        {
            Intent intent = new Intent(this, typeof(FacebookActivity));
            var SerializedVereniging = JsonConvert.SerializeObject(localVereniging);
            intent.PutExtra("Vereniging", SerializedVereniging);
            StartActivity(intent);
        }

        /// <summary>
        /// Navigeer naar de saldo activity. Die laat de gebruikers saldo van de huidige vereniging zien.
        /// </summary>
        private void NavigateToSaldoUI()
        {
            Intent saldoIntent = new Intent(this, typeof(SaldoActivity));
            saldoIntent.PutExtra("lidId", localVereniging.lidId.ToString());
            saldoIntent.PutExtra("verenigingNaam", localVereniging.naam);
            saldoIntent.PutExtra("verenigingId", localVereniging.verenigingId.ToString());

            StartActivity(saldoIntent);
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    this.MenuInflater.Inflate(Resource.Menu.OptionsMenu, menu);

        //    //var searchItem = menu.FindItem(Resource.Id.action_search);

        //    //searchView = searchItem.ActionProvider.JavaCast<Android.Widget.SearchView>();

        //    //searchView.QueryTextSubmit += (sender, args) =>
        //    //{
        //    //    Toast.MakeText(this, "You searched: " + args.Query, ToastLength.Short).Show();

        //    //};


        //    return base.OnCreateOptionsMenu(menu);
        //}
        #endregion

        /// <summary>
        /// Generates a QR Code of the given data.
        /// It's size will be filled in.
        /// Size is 1024
        /// </summary>
        /// <param name="Data">The data to be encoded into a QR Code</param>
        /// <returns>A Bitmap image of the QR Code</returns>
        private Bitmap GetQRCode(String Data)
        {
            var size = 1024;

            //Get screen size and look for smallest to make a square QR Code
            //var metrics = Resources.DisplayMetrics;
            //if (metrics.HeightPixels < metrics.WidthPixels)
            //    { size = metrics.HeightPixels; }
            //else{ size = metrics.WidthPixels; }

            //ImageView QRView = FindViewById<ImageView>(Resource.Id.QRView);
            //if (QRView.Height < QRView.Width)
            //{ size = QRView.Height; }
            //else { size = QRView.Width; }

            QRCode QRGenerator = new QRCode(Application);
            return QRGenerator.GenerateQRCode(Data, size, size);
        }
    }
}