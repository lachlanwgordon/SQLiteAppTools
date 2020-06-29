using System;
using System.IO;
using System.Reflection;
using SQLiteBrowser.Models;
using SQLiteBrowser.ModelServices;
using SQLiteBrowser.Pages;
using SQLiteBrowser.Sample.Views;
using SQLiteBrowser.Service;
using SQLiteBrowser.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SQLiteBrowser.Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IDatabase, Database>();
            DependencyService.Register<IPersonService, PersonService>();
            Device.SetFlags(new string[] { "Expander_Experimental" });
            MainPage = new ContentPage();


        }


        protected override async void OnStart()
        {
            var db = DependencyService.Resolve<IDatabase>();
            await db.RegisterTypes(typeof(Person), typeof(Bike));

            var nwindName = "Northwind_small.sqlite";
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nwindName);
            if (!File.Exists(path))
            {
                var nwindResourcePath = "SQLiteBrowser.Sample.Resources.Northwind_small.sqlite";
                WriteResourceToFile(nwindResourcePath, path);
            }
            AltBrowserViewModel.Path = path;
            //MainPage = new MyTabbedPage();
            //MainPage = new TestPage();
            MainPage = new ExpanderPage();

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
