using System;
using Xamarin.Forms;
using System.Windows.Input;
using SQLiteAppTools.Models;

namespace SQLiteAppTools.Sample.Views
{
    public partial class BikesPage : ContentPage
    {
        public BikesPage()
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