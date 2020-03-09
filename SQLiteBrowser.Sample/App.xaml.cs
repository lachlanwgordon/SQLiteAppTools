using System;
using System.IO;
using System.Reflection;
using SQLiteBrowser.Models;
using SQLiteBrowser.ModelServices;
using SQLiteBrowser.Pages;
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

            MainPage = new AltBrowserPage();
        }


        protected override async void OnStart()
        {
            var db = DependencyService.Resolve<IDatabase>();
            await db.RegisterTypes(typeof(Person), typeof(Bike));

            var nwindName = "Northwind_small.sqlite";
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nwindName);
            SQLiteBrowser.ViewModels.AltBrowserViewModel.Path = path;
            //SQLiteBrowser.ViewModels.AltBrowserViewModel.Path = db.Connection.DatabasePath;
            //"/Users/lachlangordon/Library/Developer/CoreSimulator/Devices/ED153C3A-26D3-4723-90C3-779277A53213/data/Containers/Data/Application/27F40C23-50BE-4ED8-818A-3BC9D93CA7CC/Documents/Northwind_small.sqlite"
            if (!File.Exists(path))
            {
                var nwindResourcePath = "SQLiteBrowser.Sample.Resources.Northwind_small.sqlite";
                WriteResourceToFile(nwindResourcePath, path);

            }


        }

        public void WriteResourceToFile(string resourceName, string fileName)
        {
            using (var resource = typeof(App).Assembly.GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
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
