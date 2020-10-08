using System;
using System.IO;
using System.Reflection;
using SQLiteAppTools.Models;
using SQLiteAppTools.ModelServices;
using SQLiteAppTools.Pages;
using SQLiteAppTools.Sample.Views;
using SQLiteAppTools.Service;
using SQLiteAppTools.Services;
using SQLiteAppTools.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SQLiteAppTools.Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IDatabase, Database>();
            DependencyService.Register<IPersonService, PersonService>();
            Device.SetFlags(new string[] { "Expander_Experimental", "Markup_Experimental" });
            MainPage = new ContentPage();


        }


        protected override async void OnStart()
        {
            var db = DependencyService.Resolve<IDatabase>();
            await db.RegisterTypes(typeof(Person), typeof(Bike));

            var nwindName = "Northwind_small.sqlite";
            var sampleName = "browser.db3";
            //var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nwindName);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), sampleName);

            if (!File.Exists(path))
            {
                var nwindResourcePath = "SQLiteAppTools.Sample.Resources.Northwind_small.sqlite";
                WriteResourceToFile(nwindResourcePath, path);
            }
            TableService.Init(path);
            MainPage = new MyTabbedPage();
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
