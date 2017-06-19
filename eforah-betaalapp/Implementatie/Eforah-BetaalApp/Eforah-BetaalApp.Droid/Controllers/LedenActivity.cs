using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Eforah_BetaalApp.Implementation.Models;
using Eforah_BetaalApp.Implementation;
using Eforah_BetaalApp.Droid.Components;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "Ledenlijst")]
    public class LedenActivity : ToolBarActivity, SearchView.IOnQueryTextListener
    {
        private List<LidModel> ledenLijst;
        private ExpandableListView listView;
        int previousGroup = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.LedenLijst);

            // Set Default toolbar
            InitToolBar(Intent.GetStringExtra("verenigingNaam"));

            // Set Data
            fillTextFields(Intent.GetStringExtra("verenigingId"));
        }

        private async void fillTextFields(string verenigingId)
        {
            ledenLijst = await HttpRestService.ledenRequest(verenigingId);
            if (ledenLijst == null)
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Ledelijst ophalen mislukt");
                alert.SetMessage("Kijk of u een werkende internet connectie heeft of probeer later opnieuw.");
                alert.SetCancelable(false);
                alert.SetPositiveButton("Oke", (senderAlert, args) => { Finish(); });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                // Sorteer lijst op achternaam
                ledenLijst = ledenLijst.OrderBy(l => l.achternaam).ToList();

                // Set de list view.
                listView = (ExpandableListView)FindViewById(Resource.Id.ledenLijst);
                setLidViewAdapter(ledenLijst);

                // Wanneer een groep geopent wordt wordt de vorige gesloten.
                listView.GroupExpand += delegate (object sender, ExpandableListView.GroupExpandEventArgs e)
                {
                    if (e.GroupPosition != previousGroup)
                        listView.CollapseGroup(previousGroup);
                    previousGroup = e.GroupPosition;
                };

                //Zoeken
                SearchView Search = (SearchView)FindViewById(Resource.Id.ledenSearchbar);
                Search.SetOnQueryTextListener(this);
            }
        }

        #region ledenlijst
        /// <summary>
        /// set the adapter to show the given leden
        /// </summary>
        /// <param name="leden"></param>
        private void setLidViewAdapter(List<LidModel> leden)
        {
            ExpandableLedenListAdapter adapter = new ExpandableLedenListAdapter(this, leden);
            listView.SetAdapter(adapter);
        }

        #region Leden zoeken
        private void LedenSearchQuery(String query)
        {
            if(query == null || query.Length == 0)
            {
                setLidViewAdapter(this.ledenLijst);
                return;
            }

            List<LidModel> queryLeden = new List<LidModel>();

            foreach(LidModel l in ledenLijst)
            {
                string naam = (l.voornaam + " " + l.achternaam).ToLower();
                if (naam.Contains(query.ToLower()))
                    queryLeden.Add(l);
            }

            setLidViewAdapter(queryLeden);
        }

        bool SearchView.IOnQueryTextListener.OnQueryTextSubmit(string query)
        {
            LedenSearchQuery(query);
            return true;
        }

        bool SearchView.IOnQueryTextListener.OnQueryTextChange(string newText)
        {
            LedenSearchQuery(newText);
            return true;
        }
        #endregion
        #endregion
    }
}