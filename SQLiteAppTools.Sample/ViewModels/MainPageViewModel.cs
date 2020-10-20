using System;
using System.IO;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Shell = Xamarin.Forms.Shell;

namespace SQLiteAppTools.Sample.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public Command OpenNorthwind => new Command(() =>
        {
            var nwindName = "Northwind_small.sqlite";
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nwindName);

            Shell.Current.Navigation.PushAsync(new BrowserPage(path));
        });
    }
}