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
using Newtonsoft.Json.Linq;
using Eforah_BetaalApp.Droid.Components;
using Eforah_BetaalApp.Implementation;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "Mededelinglijst")]
    public class MededelingActivity : ToolBarActivity
    {
        private List<MededelingModel> mededelingLijst;
        private ExpandableListView listView;
        int previousGroup = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MededelingView);

            // Set Default toolbar
            InitToolBar(Intent.GetStringExtra("verenigingNaam"));
            
            // Set Data
            fillTextFields(Intent.GetStringExtra("verenigingId"));
        }

        private async void fillTextFields(string verenigingId)
        {
            mededelingLijst = await HttpRestService.mededelingenRequest(verenigingId);
            if (mededelingLijst == null)
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Mededelingen ophalen mislukt");
                alert.SetMessage("Kijk of u een werkende internet connectie heeft of probeer later opnieuw.");
                alert.SetCancelable(false);
                alert.SetPositiveButton("Oke", (senderAlert, args) => { Finish(); });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                // Set de list view.
                listView = (ExpandableListView)FindViewById(Resource.Id.mededelingLijst);
                setMededelingViewAdapter(mededelingLijst);

                // Wanneer een groep geopent wordt wordt de vorige gesloten.
                listView.GroupExpand += delegate (object sender, ExpandableListView.GroupExpandEventArgs e)
                {
                    if (e.GroupPosition != previousGroup)
                        listView.CollapseGroup(previousGroup);
                    previousGroup = e.GroupPosition;
                };
            }
        }

        private void setMededelingViewAdapter(List<MededelingModel> mededelingenModelLijst)
        {
            ExpandableMededelingListAdapter adapter = new ExpandableMededelingListAdapter(this, mededelingenModelLijst);
            listView.SetAdapter(adapter);
        }

        public static String convertDateTime(string datetime, string datetimeformat)
        {
            if (datetime != "")
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(datetime);
                if (datetimeformat != "")
                {
                    return dt.ToString(datetimeformat);
                }else
                {
                    throw new ArgumentNullException("datetimeformat");
                }
            } else
            {
                throw new ArgumentNullException("datetime");
            }
        }
    }
}