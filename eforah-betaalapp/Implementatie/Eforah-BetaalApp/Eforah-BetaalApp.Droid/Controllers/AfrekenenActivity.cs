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
using Newtonsoft.Json;
using Eforah_BetaalApp.Implementation.Models;
using Newtonsoft.Json.Linq;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "Afrekenen")]
    public class AfrekenenActivity : ToolBarActivity
    {
        private VerenigingModel localVereniging;
        private VerenigingModel verenigingmodel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // bundle is al gecreeerd dus hier wordt gebruik gemaakt van een savedInstanceState
            base.OnCreate(savedInstanceState);

            // Maak de Scanner.axml binnen de layout resource de view.
            SetContentView(Resource.Layout.Afrekenen);

            Button AfrekenenScanButton = FindViewById<Button>(Resource.Id.AfrekenenScanButton);

            // Ophalen van alle vereniging informatie.
            var verenigingIdJsonString = Intent.GetStringExtra("verenigingId");
            var verenigingIdAfrekenen = JObject.Parse(verenigingIdJsonString);
            verenigingmodel = verenigingIdAfrekenen.ToObject<VerenigingModel>();
            localVereniging = verenigingmodel;

            // Set Toolbar
            InitToolBar(localVereniging.naam);

            // Volgende methode indien scannen knop wordt ingedrukt.
            AfrekenenScanButton.Click += delegate
            {
                EditText bdgi = FindViewById<EditText>(Resource.Id.AfrekenenBedragEditText);

                BedragValidationControle(bdgi.Text);

                // Controle of er een bedrag is ingevuld
                if (bdgi.Text != "")
                {
                    // Ophalen van ingevulde bedrag.
                    string bedraginput = bdgi.Text;

                    // Doorsturen van ingevulde bedrag en vereniging informatie.
                    Intent intent = new Intent(this, typeof(ScannerActivity));
                    var SerializedVerenigingIdObject = JsonConvert.SerializeObject(localVereniging);
                    intent.PutExtra("verenigingId", SerializedVerenigingIdObject);
                    intent.PutExtra("bedrag", bedraginput);
                    StartActivity(intent);
                }
                else
                {
                    Toast.MakeText(this, "Geen bedrag ingevuld", ToastLength.Short).Show();
                }
            };
        }
        public void BedragValidationControle(string teBetalenBedrag)
        {
            if (teBetalenBedrag == null)
            {
                throw new System.ArgumentException("Te betalen bedrag is null", "teBetalenBedrag");
            }
        }
    }
}