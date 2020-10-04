using System;
using Xamarin.Forms;


namespace SQLiteBrowser.Pages
{
    public partial class ExpanderPage : ContentPage
    {
        public ExpanderPage()
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