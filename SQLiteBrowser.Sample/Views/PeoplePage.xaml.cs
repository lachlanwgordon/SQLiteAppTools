using System;
using Xamarin.Forms;


namespace SQLiteBrowser.Sample.Views
{
    public partial class PeoplePage : ContentPage
    {
        public PeoplePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.InitializeAsync();
        }
    }
}