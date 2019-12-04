using System;
using Xamarin.Forms;


namespace SQLiteBrowser.Experiment.Pages
{
    public partial class BrowserPage : ContentPage
    {
        public BrowserPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await VM.InitializeAsync();
        }
    }
}