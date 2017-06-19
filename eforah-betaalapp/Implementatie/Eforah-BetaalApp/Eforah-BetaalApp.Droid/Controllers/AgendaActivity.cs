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
using Android.Webkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Eforah_BetaalApp.Implementation.Models;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "AgendaActivity", Theme = "@android:style/Theme.NoTitleBar")]
    public class AgendaActivity : Activity
    {
        WebView web_view;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Agendalayout);

            var verenigingJson = JObject.Parse(Intent.GetStringExtra("vereniging"));
            var model = verenigingJson.ToObject<VerenigingModel>();
            var agendaLink = model.agendaLink;

            web_view = FindViewById<WebView>(Resource.Id.webview);
            web_view.SetWebViewClient(new WebViewClient());
            web_view.Settings.JavaScriptEnabled = true;
            web_view.Settings.UserAgentString = "Mozilla/5.0 (Linux; Android 5.1.1; Nexus 5 Build/LMY48B; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/43.0.2357.65 Mobile Safari/537.36";
            web_view.LoadUrl(string.Concat("http://eforah-webapp.azurewebsites.net/agenda/index?agenda=", agendaLink));
        }
    }
}