using System;
using System.IO;
using SQLiteBrowser.Models;
using SQLiteBrowser.ModelServices;
using SQLiteBrowser.Sample.Views;
using SQLiteBrowser.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLiteBrowser.Sample
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

            SQLiteBrowser.ViewModels.AltBrowserViewModel.Path = db.Connection.DatabasePath;
        }

        public static string DatabasePath(string name)
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(basePath, name);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
