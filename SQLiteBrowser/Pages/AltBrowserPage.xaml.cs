using System;
using SQLiteBrowser.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLiteBrowser.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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