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
using Android.Support.V7.App;
using Eforah_BetaalApp.Implementation.Models;
using Newtonsoft.Json;
using System.Globalization;
using Eforah_BetaalApp.Implementation;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "Saldo")]
    public class SaldoActivity : ToolBarActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            initActivity("Saldo", savedInstanceState);
            fillTextFields(Intent.GetStringExtra("lidId"), Intent.GetStringExtra("verenigingId"), Intent.GetStringExtra("verenigingNaam"));
        }

        private void initActivity(string title, Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SaldoUI);

            InitToolBar();
        }

        private async void fillTextFields(string lidId, string verenigingId, string verenigingNaam)
        {
            var saldoInfo = await HttpRestService.saldoRequest(lidId, verenigingId);
            if (saldoInfo == null)
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Saldo ophalen mislukt");
                alert.SetMessage("Kijk of u een werkende internet connectie heeft of probeer later opnieuw.");
                alert.SetCancelable(false);
                alert.SetPositiveButton("Oke", (senderAlert, args) => { Finish(); });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                List<TransactieModel> transacties = saldoInfo.Item2;
                List<TextView> transactieTV = new List<TextView>()
                {
                    FindViewById<TextView>(Resource.Id.tvSaldo_transaction1),
                    FindViewById<TextView>(Resource.Id.tvSaldo_transaction2),
                    FindViewById<TextView>(Resource.Id.tvSaldo_transaction3),
                    FindViewById<TextView>(Resource.Id.tvSaldo_transaction4),
                    FindViewById<TextView>(Resource.Id.tvSaldo_transaction5)
                };
                for (int i = 0; i < transacties.Count && i < 5; i++)
                {
                    transactieTV[i].Text = DateTime.ParseExact(transacties[i].transactieDatum, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).ToString("dd-mm-yyy HH:mm") + ":        -" + transacties[i].bedrag;
                }

                TextView tvSaldo_currentSaldo = FindViewById<TextView>(Resource.Id.tvSaldo_currentSaldo);

                VerenigingNaam = verenigingNaam;
                string saldo = saldoInfo.Item1;
                tvSaldo_currentSaldo.Text = saldo.Remove(saldo.Length - 2);
            }
        }
    }
}