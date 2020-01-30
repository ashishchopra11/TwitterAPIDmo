using System.Threading.Tasks;
using TwitterAPIDemo.Utils;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.Base
{
    public abstract class BaseViewModel : ExtendedBindableObject
    {
        #region Initialization
        private bool _isTablet;
        private bool _isBusy;
        //private bool _isBusyBlocking;
        private bool _isPageLoaded;
        protected bool _isInitialized;

        // Event to display alert
        public delegate void AlertHandler(string title, string message, string buttonText);
        public delegate Task<bool> AlertHandlerWithResponse(string title, string message, string buttonAcceptText, string buttonCancelText);
        public event AlertHandler DisplayAlertEvent;
        public event AlertHandlerWithResponse DisplayAlertEventWithResponse;

        #endregion
        #region Properties

        protected INavigation Navigation { get; set; }

        public bool IsInitialized => _isInitialized;

        public bool IsPageLoaded
        {
            get => _isPageLoaded;
            set
            {
                _isPageLoaded = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        //public bool IsBusyBlocking
        //{
        //    get => _isBusyBlocking;
        //    set
        //    {
        //        _isBusyBlocking = value;
        //        if (value)
        //        {
        //            UserDialogs.Instance.ShowLoading("", MaskType.Black);
        //        }
        //        else
        //        {
        //            UserDialogs.Instance.HideLoading();
        //        }
        //        OnPropertyChanged();
        //    }
        //}

        public bool IsTablet
        {
            get => _isTablet;
            set
            {
                _isTablet = value;
                OnPropertyChanged();
            }
        }

        private bool _viewHasAppeared;
        public bool ViewHasAppeared
        {
            get => _viewHasAppeared;
            set
            {
                _viewHasAppeared = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        //protected BaseViewModel()
        //{
        //    AppLogger.log("ViewModelBase initialized", this);
        //    IsTablet = Device.Idiom == TargetIdiom.Tablet;
        //}

        #endregion


        public virtual Task InitializeAsync(Page page)
        {
            this.Navigation = page.Navigation;  // Maintain Navigation on initialization
            ViewHasAppeared = true;
            return Task.FromResult(false);
        }

        public virtual void FinalizeAsync()
        {
            ViewHasAppeared = false;
        }


        /// <summary>
        /// Displays the alert.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="buttonText">The button text.</param>
        public void DisplayAlert(string title, string message, string buttonText = "OK")
        {
            if (DisplayAlertEvent != null)
                DisplayAlertEvent?.Invoke(title, message, buttonText);
        }

        //public void DisplayErrorAlert(string message)
        //{
        //    if (DisplayAlertEvent != null)
        //        DisplayAlertEvent?.Invoke(System.Resources.DialogTitleError, message, System.Resources.DialogLabelOk);
        //}

        //public void DisplaySuccessAlert(string message)
        //{
        //    if (DisplayAlertEvent != null)
        //        DisplayAlertEvent?.Invoke(System.Resources.DialogTitleSuccess, message, Resources.DialogLabelOk);
        //}

        public Task<bool> DisplalertAlertWithResponse(string message, string title = "Alert", string buttonAcceptText = "Yes", string buttonCancelText = "No")
        {
            if (DisplayAlertEventWithResponse != null)
                return DisplayAlertEventWithResponse?.Invoke(title, message, buttonAcceptText, buttonCancelText);
            return Task.FromResult(false);
        }

        /// <summary>
        /// Displaies the web API error.
        /// </summary>
        /// <returns><c>true</c>, if web API error was displayed, <c>false</c> if it was successful.</returns>
        /// <param name="baseModel">Base response from Web</param>
        /// <param name="successMsg">Success message.</param>
        /// <param name="failureMsg">If no msg found</param>
        public bool DisplayWebApiRespMessage(BaseViewModel baseModel, string successMsg = null, string failureMsg = null)
        {
            return true;
        }

        /// <summary>
        /// Displays the flashing error.
        /// The message can be shown using Toast or Snackbar, depending upon underlying method definition
        /// </summary>
        /// <param name="message"> The message to display</param>
        //public void DisplayFlashingMessage(string message, bool isError = true)
        //{
        //    if (Device.RuntimePlatform == Device.iOS)
        //    {
        //        if (isError)
        //        {
        //            DisplayErrorAlert(message);
        //        }
        //        else
        //        {
        //            DisplaySuccessAlert(message);
        //        }
        //        return;
        //    }
        //    DependencyService.Get<IPlatformHelperService>().ShowFlashingMessage(message);
        //}
    }
}
