using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        private ObservableCollection<Details> searchUser;
        public ObservableCollection<Details> SearchUser
        {
            get
            {
                return searchUser;
            }
            set
            {
                searchUser = value;
                OnPropertyChanged();
            }
        }
        public SearchViewModel()
        {
            this.SearchUser = new ObservableCollection<Details>();
        }
        public ICommand PerformSearch => new Command<string>(async (query) =>
        {
            await SearchDataList(query);
        });
        private async Task SearchDataList(string name)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {

                    Authorization auth = new Authorization();
                    string url = "https://api.twitter.com/1.1/users/search.json";
                    var data1 = new Dictionary<string, string>
                    {
                        {"q",name}
                    };

                    httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, data1, "GET"));

                    UriBuilder builder = new UriBuilder(url);
                    builder.Query = "q=" + name;

                    var httpResponse = httpClient.GetAsync(builder.Uri).Result;
                    if (httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {
                        DisplayAlert("sorry", "You are not authorized", "ok");
                    }
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();
                    var Users = JsonConvert.DeserializeObject<List<User>>(httpContent);
                    this.searchUser.Clear();
                    SearchUser = new ObservableCollection<Details>();
                    foreach (var data in Users)
                    {
                        SearchUser.Add(new Details
                        {
                            Name = data.name,
                            Uname = data.screen_name,
                            ProfileImgUrl = data.profile_image_url,
                            Status = "Follow"
                        });
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        public Command Follow
        {
            get
            {
                return new Command(async (obj) =>
                {
                    string Uname = (string)obj.GetType().GetProperty("Uname").GetValue(obj);

                    Authorization auth = new Authorization();
                    string url = "https://api.twitter.com/1.1/friendships/create.json";
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "screen_name", Uname },
                        { "follow", "1" }
                    };
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, data, "POST"));

                        var httpResponse = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));
                        if (!httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                        {
                            DisplayAlert("sorry", "something went wrong", "ok");
                            return;
                        }
                    }
                    DependencyService.Get<iMessage>().Shorttime("user followed successful");
                });
            }
        }
        public class Details
        {
            public Details() { }
            public string Name { get; set; }
            public string Uname { get; set; }
            public string ProfileImgUrl { get; set; }
            public string Status { get; set; }
        }
    }
}
