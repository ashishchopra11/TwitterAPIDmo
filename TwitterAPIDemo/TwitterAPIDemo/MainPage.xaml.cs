using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitterAPIDemo.Oauth;
using Xamarin.Forms;

namespace TwitterAPIDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                Authorization auth = new Authorization();
                string url = "https://api.twitter.com/oauth/request_token";
                var client = new RestClient("https://api.twitter.com/oauth/request_token");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("oauth_callback", "http://mobile.twitter.com");
                string str = auth.PrepareOAuth(url, null, "POST");
                request.AddHeader("Authorization", auth.PrepareOAuth(url,null,"POST"));
                IRestResponse response = client.Execute(request);
                //using (var httpClient = new HttpClient())
                //{

                //    var data = new Dictionary<string, string>
                //        {
                //            { "oauth_callback", "https://mobile.twitter.com" }
                //     };
                //    string str = auth.PrepareOAuth(url, data, "GET");
                //    httpClient.DefaultRequestHeaders.Add("Authorization", str);
                //    httpClient.DefaultRequestHeaders.Add("oauth_callback", "https://mobile.twitter.com");
                //    var httpResponse = await httpClient.PostAsync(url,null);
                //    //if (!httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                //    //{
                //    //    DisplayAlert("sorry", "something went wrong", "ok");
                //    //    return;
                //    //}
                //}
            }
            catch(Exception ex)
            {

            }
        }
    }
}
