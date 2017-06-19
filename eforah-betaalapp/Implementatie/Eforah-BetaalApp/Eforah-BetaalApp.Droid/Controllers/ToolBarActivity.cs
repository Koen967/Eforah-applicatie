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

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "ToolBarActivity")]
    public class ToolBarActivity : AppCompatActivity
    {
        public string VerenigingNaam
        {
            get { return FindViewById<TextView>(Resource.Id.vereniging_naam).Text; }
            set { FindViewById<TextView>(Resource.Id.vereniging_naam).Text = value; }
        }

        protected void InitToolBar()
        {
            // Set Toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.mainToolbar);
            TextView toolbarTitle = toolbar.FindViewById<TextView>(Resource.Id.toolbarTitle);
            SetSupportActionBar(toolbar);

            toolbarTitle.Text = SupportActionBar.Title;
            SupportActionBar.Title = "";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            toolbar.NavigationClick += delegate
            {
                Finish();
            };
        }

        protected void InitToolBar(string verenigingNaam)
        {
            InitToolBar();
            VerenigingNaam = verenigingNaam;
        }
    }
}