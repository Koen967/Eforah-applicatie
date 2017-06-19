using System.Collections.Generic;

using Android.App;
using Android.OS;
using Newtonsoft.Json.Linq;
using System.Net;
using Eforah_BetaalApp.Implementation.Models;
using Android.Widget;
using Android.Content;
using Android.Support.V4.Widget;

namespace Eforah_BetaalApp.Droid.Controllers
{
    [Activity(Label = "Facebook")]
    public class FacebookActivity : ToolBarActivity
    {
        private List<FacebookFeedModel> facebookFeedList;
        private ListView listView;

        private VerenigingModel vereniging;

        private SwipeRefreshLayout refresher;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Ophalen van vereniging vanuit QRActivity
            var JsonStringVereniging = Intent.GetStringExtra("Vereniging");
            var verenigingData = JObject.Parse(JsonStringVereniging);
            vereniging = verenigingData.ToObject<VerenigingModel>();

            // Controle of er een facebook is gekoppeld aan vereniging.
            if (vereniging.facebookGroupId == "" || vereniging.facebookAdminId == "")
            {
                SetContentView(Resource.Layout.FacebookFeedUnavailableView);
                // Het instellen van vereniging naam onder de toolbar.
                InitToolBar(vereniging.naam);
            } else
            {
                SetContentView(Resource.Layout.FacebookFeedView);

                // Het instellen van vereniging naam onder de toolbar.
                InitToolBar(vereniging.naam);

                // Set de list view.
                listView = (ListView)FindViewById(Resource.Id.facebookFeedLijst);

                try
                {
                    // Get and set the Facebookfeed
                    facebookFeedList = getFaceBookFeed();
                    var facebookFeedAdapter = FeedToAdapter(facebookFeedList);
                    listView.Adapter = facebookFeedAdapter;

                    // Begint click event.
                    listView.ItemClick += List_ItemClick;

                    // Ververs op swipte
                    refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
                    refresher.Refresh += delegate
                    {
                        WebClient fbaccess = new WebClient();
                        fbaccess.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;

                        fbaccess.DownloadStringCompleted += (sender, e) =>
                        {
                            var jData = e.Result;
                            var feedlist = ParseFacebookJDataToList(jData);
                            listView.Adapter = FeedToAdapter(feedlist);
                            refresher.Refreshing = false;
                        };

                        fbaccess.DownloadStringAsync(new System.Uri("https://graph.facebook.com/" + vereniging.facebookGroupId + "?fields=feed{link,message,story,created_time}&access_token=" + vereniging.facebookAdminId));
                    };
                }
                catch
                {
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                    alert.SetTitle("Facebook feed ophalen mislukt");
                    alert.SetMessage("Kijk of u een werkende internet connectie heeft of probeer later opnieuw.");
                    alert.SetCancelable(false);
                    alert.SetPositiveButton("Oke", (senderAlert, args) => { Finish(); });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
            }
        }

        /// <summary>
        /// Get Data from the Facebook feed
        /// </summary>
        /// <returns>Lijst van facebook feed</returns>
        private List<FacebookFeedModel> getFaceBookFeed()
        {
            // Ophalen van facebook feed aan de hand van het user_id en het stabiele access token.
            WebClient fbaccess = new WebClient();
            fbaccess.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            var jData = fbaccess.DownloadString("https://graph.facebook.com/" + vereniging.facebookGroupId + "?fields=feed{link,message,story,created_time}&access_token=" + vereniging.facebookAdminId);

            return ParseFacebookJDataToList(jData);
        }

        private void RefreshFeedList()
        {

        }

        /// <summary>
        /// Parse Data naar een lijst van faccebook feed modellen
        /// </summary>
        /// <param name="jData"></param>
        /// <returns></returns>
        private List<FacebookFeedModel> ParseFacebookJDataToList(string jData)
        {
            // Omzetten van Json object naar een lijst van FacebookFeed's.
            JObject jparse = JObject.Parse(jData);

            JObject feed = jparse["feed"] as JObject;

            return JArrayToList<FacebookFeedModel>("data", feed);
        }

        /// <summary>
        /// Zet een lijst van Facebookfeed modellen om in adapter die in een listview gezet can worden.
        /// </summary>
        /// <param name="facebookFeedList">Facebook feed</param>
        /// <returns>Adapter die in een Listview gezet kan worden</returns>
        private ArrayAdapter<string> FeedToAdapter(List<FacebookFeedModel> facebookFeedList)
        {
            List<string> facebookFeedString = new List<string>();

            // Uitschrijven van Facebook feed data. Inclusief controle welk soort Facebook feed uitgeschreven.
            foreach (FacebookFeedModel m in facebookFeedList)
            {
                string link;
                string message;
                string story;
                if (m.link != null)
                {
                    link = m.link + "\n \n";

                }
                else
                {
                    link = "";
                }
                if (m.message != null)
                {
                    message = m.message + "\n \n";

                }
                else
                {
                    message = "";
                }
                if (m.story != null)
                {
                    story = m.story + "\n \n";

                }
                else
                {
                    story = "";
                }
                facebookFeedString.Add(link + message + story + m.created_time);
            }

            return new ArrayAdapter<string>(this, Resource.Layout.FacebookFeedListViewRow, facebookFeedString);
        }

        /// <summary>
        /// Het doorsturen naar link wanneer er op een Facebook feed item geklikt wordt.
        /// </summary>
        private void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            // Controle of facebook feed een link heeft
            if (facebookFeedList[e.Position].link != null)
            {
                // Doorsturen naar link over standaard browser
                Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(facebookFeedList[e.Position].link));
                StartActivity(browserIntent);
            }
        }

        /// <summary>
        /// Het conventeren van Json object naar reguliere List.
        /// </summary>
        private static List<T> JArrayToList<T>(string objectName, JObject body)
        {
            JArray JsonArray = (JArray)body[objectName];
            List<T> list = new List<T>();
            for (int i = 0; i < JsonArray.Count; i++)
            {
                list.Add(JsonArray[i].ToObject<T>());
            }
            return list;
        }
    }
}