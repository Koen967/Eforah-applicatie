using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
//using TypeMock.ArrangeActAssert;

namespace Eforah_BetaalApp.Droid.Test.Mocks
{
    public class LedenViewMock : View
    {
        public Dictionary<int, View> views;
        private Context _context;

        public LedenViewMock(Context context) :
            base(context)
        {
            _context = context;
            Initialize();
        }

        private void Initialize()
        {
            views = new Dictionary<int, View>();
            views.Add(Eforah_BetaalApp.Droid.Resource.Id.lid_email, new TextView(_context));
            views.Add(Eforah_BetaalApp.Droid.Resource.Id.lid_telefoonnummer, new TextView(_context));
            views.Add(Eforah_BetaalApp.Droid.Resource.Id.lid_altNummer, new TextView(_context));
            views.Add(Eforah_BetaalApp.Droid.Resource.Id.lid_straat, new TextView(_context));
            views.Add(Eforah_BetaalApp.Droid.Resource.Id.lid_postcode, new TextView(_context));

            //Isolate.WhenCalled(() => FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_email)).WillReturn(views[0]);
            //Isolate.WhenCalled(() => FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_telefoonnummer)).WillReturn(views[1]);
            //Isolate.WhenCalled(() => FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_altNummer)).WillReturn(views[2]);
            //Isolate.WhenCalled(() => FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_straat)).WillReturn(views[3]);
            //Isolate.WhenCalled(() => FindViewById(Eforah_BetaalApp.Droid.Resource.Id.lid_postcode)).WillReturn(views[4]);
        }
    }
}