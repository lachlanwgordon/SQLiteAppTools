using System;
using SQLiteAppTools.Models;
using SQLiteAppTools.Sample.ViewModels;
using Xamarin.Forms;


namespace SQLiteAppTools.Sample.Views
{
    public partial class PersonPage : ContentPage
    {
        public PersonPage(Person person)
        {
            BindingContext = new PersonPageViewModel(person);
            InitializeComponent();
        }
    }
}