using System;
using System.Collections.Generic;
using System.Text;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.Views.Base
{
    public class BaseContentView:ContentView
    {
        //private BaseViewModel ViewModel
        //{
        //    get { return BindingContext as BaseViewModel; }
        //}

        //public BaseViewModel getViewModel()
        //{
        //    return ViewModel;
        //}

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    if (ViewModel != null)
        //    {
        //        // Subscribe display alert event
        //        ViewModel.DisplayAlertEvent += ViewModel_DisplayAlert;
        //        ViewModel.DisplayAlertEventWithResponse += ViewModel_DisplayAlertResponse;
        //        await ViewModel.InitializeAsync(this);
        //    }
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();

        //    if (ViewModel != null)
        //    {
        //        // Unsubscribe display alert event
        //        ViewModel.DisplayAlertEvent -= ViewModel_DisplayAlert;
        //        ViewModel.DisplayAlertEventWithResponse -= ViewModel_DisplayAlertResponse;
        //        ViewModel.FinalizeAsync();
        //    }
        //}

        //private void ViewModel_DisplayAlert(string title, string message, string buttonText)
        //{
        //    DisplayAlert(title, message, buttonText);
        //}
        //async Task<bool> ViewModel_DisplayAlertResponse(string title, string message, string buttonAcceptText, string buttonCancelText)
        //{
        //    return await DisplayAlert(title, message, buttonAcceptText, buttonCancelText);
        //}
    }
}
