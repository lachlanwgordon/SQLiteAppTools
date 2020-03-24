using System;
using SQLiteBrowser.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLiteBrowser.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AltBrowserPage : ContentPage
    {
        public AltBrowserPage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.OnAppearing();
        }
    }
}