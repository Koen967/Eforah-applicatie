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

namespace Eforah_BetaalApp.Droid.Test.Mocks
{
    [Activity(Label = "LedenActivityMock")]
    public class LedenActivityMock : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Eforah_BetaalApp.Droid.Resource.Layout.LedenLijst);
            // Create your application here
        }
    }
}