using System;
using SQLiteBrowser.Experiment;
using SQLiteBrowser.Experiment.Pages;
using SQLiteBrowser.Models;
using SQLiteBrowser.ModelServices;
using SQLiteBrowser.Service;
using SQLiteBrowser.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLiteBrowser
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IDatabase, Database>();
            DependencyService.Register<IPersonService, PersonService>();

            MainPage = new MyTabbedPage();
        }


        protected override async void OnStart()
        {
            var db = DependencyService.Resolve<IDatabase>();
            await db.RegisterTypes(typeof(Person), typeof(Bike));
            Manager.Initialize(db.Connection.DatabasePath);

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
