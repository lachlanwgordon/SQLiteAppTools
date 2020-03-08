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
            SQLiteBrowser.Browser.Init(db.Connection);

            SQLiteBrowser.ViewModels.AltBrowserViewModel.Path = db.Connection.DatabasePath;
            //"/Users/lachlangordon/Library/Developer/CoreSimulator/Devices/ED153C3A-26D3-4723-90C3-779277A53213/data/Containers/Data/Application/1367D711-5E20-41BC-B379-4DE3D91E916E/Documents/Northwind_small.sqlite"
            //"/Users/lachlangordon/Library/Developer/CoreSimulator/Devices/ED153C3A-26D3-4723-90C3-779277A53213/data/Containers/Data/Application/20AA959F-BCB3-45C4-A1C0-EBB586D3F414/Documents/Northwind_small.sqlite"
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
