using System;
using SQLiteBrowser.ViewModels;
using Xamarin.Forms;


namespace SQLiteBrowser.Pages
{
    public partial class AltBrowserPage : ContentPage
    {
        public AltBrowserViewModel ViewModel => BindingContext as AltBrowserViewModel;
        public AltBrowserPage()
        {
            BindingContext = new AltBrowserViewModel();
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.OnAppearingAsync();
        }
    }
}