using System;
using System.Collections.Generic;
using SQLiteBrowser.Models;
using Xamarin.Forms;

namespace SQLiteBrowser.Pages
{
    public partial class NewPage : ContentPage
    {
        public NewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.OnAppearing();
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var view = sender as View;
            var item = view.BindingContext as Row;

        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }
    }
}
