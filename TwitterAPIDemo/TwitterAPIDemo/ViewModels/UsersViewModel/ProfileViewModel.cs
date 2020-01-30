using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        public INavigation navigation;
        ProfilePageModel obj;
        bool flag = false;
        public ProfileViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            Task.Run(() => Profile()).Wait();
        }
        public bool Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public Command EditProfile
        {
            get
            {
                return new Command(async (object obj) =>
                {
                    string property = obj.ToString();
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    //data.Add("name", "Name");
                    if (property == "EditName")
                    {
                        data = new Dictionary<string, string>
                    {
                        {"name" , Name }
                    };
                        await Update(data);
                        var message = "Name has been updated";
                        DependencyService.Get<iMessage>().Shorttime(message);
                    }

                    else if (property == "EditDescription")
                    {
                        data = new Dictionary<string, string>
                        {
                            {"description", Description }
                        };
                        await Update(data);
                        var message = "Description has been updated";
                        DependencyService.Get<iMessage>().Shorttime(message);
                    }

                });
            }
        }
        public string Banner
        {
            get { return banner; }
            set
            {
                banner = value;
                OnPropertyChanged();
            }
        }

        public string ProfileImage
        {
            get { return profileImage; }
            set
            {
                profileImage = value;
                OnPropertyChanged();
            }
        }

        public Command UploadImage
        {
            get
            {
                return new Command(async (object obj) =>
                    {
                        string type = obj.ToString();
                        if (type == "banner")
                        {
                            flag = true;
                            Banner = await ClickToUpload();
                            Visibility = true;

                        }
                        else if (type == "ProfileImage")
                        {
                            ProfileImage = await ClickToUpload();
                            Visibility = true;

                        }
                    });
            }
        }
        public Command SaveImage
        {
            get
            {
                return new Command(async (object obj) =>
                {
                    string save = obj.ToString();
                    if (flag == true)
                    {
                        var url = "https://api.twitter.com/1.1/account/update_profile_banner.json";
                        await PostBanner(Banner, url.ToString());
                        var message = "Banner has been Updated";
                        DependencyService.Get<iMessage>().Shorttime(message);
                        flag = false;
                        Visibility = false;
                    }
                    else
                    {

                        var url = "https://api.twitter.com/1.1/account/update_profile_image.json";
                        await PostBanner(ProfileImage, url.ToString());
                        var message = "Profile Image has been updated";
                        DependencyService.Get<iMessage>().Shorttime(message);
                        Visibility = false;
                    }
                });
            }
        }
        private async Task<string> ClickToUpload()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return null;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full
            });


            if (file == null)
                return null;
            else
            {
                return file.Path;
            }
        }
        private async Task Update(Dictionary<string, string> data)
        {
            try
            {
                Authorization auth = new Authorization();
                var url = "https://api.twitter.com/1.1/account/update_profile.json";
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, data, "POST"));

                    var httpResponse = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));
                    if (httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {
                        DisplayAlert("sorry", "You are not authorized", "ok");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task PostBanner(string ImagePath, string url)
        {
            try
            {
                Authorization auth = new Authorization();
                byte[] bannerdata = System.IO.File.ReadAllBytes(ImagePath);
                //string s = Convert.ToBase64String(bannerdata, Base64FormattingOptions.InsertLineBreaks);
                var imgContent = new ByteArrayContent(bannerdata);
                imgContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                var multipartContent = new MultipartFormDataContent();
                if (flag == true)
                {
                    multipartContent.Add(imgContent, "banner");
                }
                else
                {
                    multipartContent.Add(imgContent, "image");
                }
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, null, "POST"));

                    var httpResponse = await httpClient.PostAsync(url, multipartContent);
                    if (httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {

                        DisplayAlert("sorry", "You are not authorized", "ok");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async Task Profile()
        {
            try
            {
                Authorization auth = new Authorization();
                var url = "https://api.twitter.com/1.1/users/show.json";

                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                        {
                            { "screen_name", "ashishchopra01" }
                        };
                    httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, data, "GET"));
                    UriBuilder builder = new UriBuilder(url);
                    builder.Query = "screen_name=ashishchopra01";
                    var httpResponse = httpClient.GetAsync(builder.Uri).Result;
                    Console.WriteLine(httpResponse);
                    if (httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {
                        DisplayAlert("sorry", "You are not authorized", "ok");
                        return;
                    }
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();
                    obj = JsonConvert.DeserializeObject<ProfilePageModel>(httpContent);
                    Banner = obj.profile_banner_url;
                    ProfileImage = obj.profile_image_url;
                    Name = obj.name;
                    Username = obj.screen_name;
                    Location = obj.location;
                    Description = obj.description;
                }
            }
            catch (Exception ex) { }
        }
        private string banner;
        private string profileImage;
        private string name;
        private bool visibility;
        public string Username { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

    }
}

