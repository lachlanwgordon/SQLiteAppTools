using System;
using SQLiteAppTools.Models;
using SQLiteAppTools.Sample.ViewModels;
using Xamarin.Forms;


namespace SQLiteAppTools.Sample.Views
{
    public partial class BikePage : ContentPage
    {
        public BikePage(Bike bike)
        {
            BindingContext = new BikePageViewModel(bike);
            InitializeComponent();
        }
    }
}