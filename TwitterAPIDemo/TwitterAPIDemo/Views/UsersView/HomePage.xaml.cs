using TwitterAPIDemo.ViewModels.UsersViewModel;
using TwitterAPIDemo.Views.Base;
using Xamarin.Forms.Xaml;

namespace TwitterAPIDemo.Views.UsersView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : BaseContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel(Navigation);
        }
    }
}