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
using ZXing.Net.Mobile;
using ZXing.Net.Mobile.Forms;
using Xamarin.Forms;
using ZXing.Mobile;
using Newtonsoft.Json;
using Eforah_BetaalApp.Implementation.Models;
using Eforah_BetaalApp.Implementation;
using Newtonsoft.Json.Linq;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "Scanner")]
    public class ScannerActivity : Activity
    {
        private string teBetalenBedrag;
        private VerenigingModel verenigingmodel;
        private string transactionRequestlidId;
        private string transactionRequestverenigingId;
        private string transactionRequestbedrag;
        private string transactionRequestmessage;
        VerenigingModel localVereniging;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // bundle is al gecreëerd dus hier wordt gebruik gemaakt van een savedInstanceState
            base.OnCreate(savedInstanceState);

            // Maak de Scanner.axml binnen de layout resource de view.
            SetContentView(Resource.Layout.Scanner);

            // Ophalen van bedrag vanuit vorige activity aan de hand van intent
            teBetalenBedrag = Intent.GetStringExtra("bedrag");

            // Bedrag input controle
            BedragLessThanZeroControle(teBetalenBedrag);

            // Ophalen van vereniging Id vanuit vorige activity aan de hand van intent
            var VerenigingIdJsonString = Intent.GetStringExtra("verenigingId");

            // Vereniging ID input controle
            VerenigingIdNullControle(VerenigingIdJsonString);

            // Vereniging JSon string omzetten naar verenigingmodel object.
            var verenigingIdScanner = JObject.Parse(VerenigingIdJsonString);
            verenigingmodel = verenigingIdScanner.ToObject<VerenigingModel>();
            localVereniging = verenigingmodel;

            // Laten zien van de scanner 
            ShowScanner();
        }

        public async void ShowScanner()
        {
            // Standaard code voor het aanmaken van de scanner. Onderstaande code is aan de hand van ZXing library opgelost.
            MobileBarcodeScanner.Initialize(Application);
            var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
            options.PossibleFormats = new List<ZXing.BarcodeFormat>() {
                ZXing.BarcodeFormat.QR_CODE
            };
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();
            // Wanneer de scanner een QR-code heeft gescanned gaat onderstaande if statement van stand.
            if (result != null)
            {
                Console.WriteLine("Scanned Barcode: " + result.Text);

                GebruikerModel klant = await HttpRestService.GebruikerRequest(Int32.Parse(result.Text));
                if (klant == null)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Connectie fout");
                    alert.SetMessage("controller of u een werkende internet connectie heeft of probeer het later opnieuw.");
                    alert.SetPositiveButton("Oke", (senderAlert, args) => { Finish();  });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {

                    // Pop-up bericht voor transactie confirmatie.
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Bevestiging");
                    alert.SetMessage("Weet u zeker dat u €" + teBetalenBedrag + " van " + klant.voornaam + " " + klant.achternaam + " wilt aftrekken bij " + localVereniging.naam + "?");
                    // Transactie voltooid.
                    alert.SetPositiveButton("Ja", async (senderAlert, args) =>
                    {
                        transactionRequestlidId = result.Text; //localVereniging.lidId.ToString();
                    transactionRequestverenigingId = localVereniging.verenigingId.ToString();
                        transactionRequestbedrag = teBetalenBedrag;
                        transactionRequestmessage = await HttpRestService.transactionRequest(transactionRequestlidId, transactionRequestverenigingId, transactionRequestbedrag);
                        if(transactionRequestmessage == null)
                        {
                            Toast.MakeText(this, "Transactie mislukt!", ToastLength.Short).Show();
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, transactionRequestmessage, ToastLength.Short).Show();
                            Finish();
                        }
                    });
                    // Transactie geannuleerd.
                    alert.SetNegativeButton("Nee", (senderAlert, args) =>
                    {
                        Toast.MakeText(this, "Transactie geannuleerd", ToastLength.Short).Show();
                        Finish();
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            }
        }

        //Bedrag input controle
        public void BedragLessThanZeroControle(string teBetalenBedrag)
        {
            if (teBetalenBedrag == null)
            {
                throw new System.ArgumentException("Te betalen bedrag is null", "teBetalenBedrag");
            } else if (teBetalenBedrag == "")
            {
                throw new System.ArgumentException("Te betalen bedrag is niet ingevuld", "teBetalenBedrag");
            }
        }

        //VerenigingId input controle
        public void VerenigingIdNullControle(string verenigingId)
        {
            if (verenigingId == null)
            {
                throw new System.ArgumentException("VerenigingId is null", "verenigingId");
            } else if (verenigingId == "")
            {
                throw new System.ArgumentException("VerenigingId is niet ingevuld", "verenigingId");
            }
        }
    }
}