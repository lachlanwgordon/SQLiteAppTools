using System;
using System.IO;
using System.Reflection;
using SQLiteAppTools.Models;
using SQLiteAppTools.Sample.ModelServices;
using SQLiteAppTools.Sample.Views;
using SQLiteAppTools.Service;
using Xamarin.Forms;
namespace SQLiteAppTools.Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IDatabase, Database>();
            DependencyService.Register<IPersonService, PersonService>();
            DependencyService.Register<IBikeService, BikeService>();
            Device.SetFlags(new string[] { "Expander_Experimental", "Markup_Experimental" });
            MainPage = new ContentPage();
        }

        protected override async void OnStart()
        {
            var db = DependencyService.Resolve<IDatabase>();
            await db.RegisterTypes(typeof(Person), typeof(Bike));

            var sampleName = "browser.db3";
            var nwindName = "Northwind_small.sqlite";
            var nwpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nwindName);

            if (!File.Exists(nwpath))
            {
                var nwindResourcePath = "SQLiteAppTools.Sample.Resources.Northwind_small.sqlite";
                WriteResourceToFile(nwindResourcePath, nwpath);
            }
            var samplePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), sampleName);

            SQLiteAppTools.Init(samplePath);

            MainPage = new AppShell();
        }

        public void WriteResourceToFile(string resourceName, string fileName)
        {
            using var resource = typeof(App).Assembly.GetManifestResourceStream(resourceName);
            using var file = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            resource.CopyTo(file);
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
