using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using Eforah_BetaalApp.Implementation.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "Verenigingen", NoHistory = true)]
    public class VerenigingSelectionActivity : Activity
    {
        private ListView listView;
        private List<String> verenigingListString;
        private List<VerenigingModel> verenigingList;

        private ISharedPreferences prefs;
        private ISharedPreferencesEditor editor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VerenigingSelectionView);
            
            //Get preferences editor
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            editor = prefs.Edit();

            //Haal lijst met verenigingen op
            //Door gegeven uit login
            var JsonStringLoginGegevens = Intent.GetStringExtra("loginDetailTuple");
            var jsonData = JObject.Parse(JsonStringLoginGegevens);
            var loginTuple = jsonData.ToObject<Tuple<GebruikerModel, List<VerenigingModel>>>();
            verenigingList = loginTuple.Item2;

            //Maak List en vul die
            listView = (ListView)FindViewById(Resource.Id.verenigingList);
            
            verenigingListString = new List<string>();
            foreach(VerenigingModel m in verenigingList)
            {
                verenigingListString.Add(m.naam);
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.ListViewRow, verenigingListString); //Android.Resource.Layout.SimpleListItem1

            listView.Adapter = adapter;
            listView.ItemClick += List_ItemClick;
        }

        /// <summary>
        /// Wanneer een list item wordt geklikt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">geklikte item</param>
        private void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Maak keuze zichtbaar
            Toast.MakeText(this, verenigingList[e.Position].naam, ToastLength.Long).Show();

            //Sla keuze als preference op
            editor.PutInt("VerenigingId", verenigingList[e.Position].verenigingId);
            editor.Apply();

            // Aanroepen van volgende activity
            Intent intent = new Intent(this, typeof(QRActivity));

            var JsonStringAanmeldingsGegevens = Intent.GetStringExtra("loginDetailTuple");
            intent.PutExtra("loginDetailTuple", JsonStringAanmeldingsGegevens);

            var SerializedVereniging = JsonConvert.SerializeObject(verenigingList[e.Position].verenigingId);
            intent.PutExtra("verenigingId", SerializedVereniging);

            StartActivity(intent);
        }
    }
}