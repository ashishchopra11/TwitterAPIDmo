using TwitterAPIDemo.ViewModels.UsersViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TwitterAPIDemo.Views.UsersView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FollowingPage : ContentView
    {
        public FollowingPage()
        {
            InitializeComponent();
            BindingContext = new FollowingViewModel();
        }
    }
}