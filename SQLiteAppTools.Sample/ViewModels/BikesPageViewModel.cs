using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SQLiteAppTools.Models;
using SQLiteAppTools.Sample.ModelServices;
using SQLiteAppTools.Sample.Views;
using Xamarin.Forms;

namespace SQLiteAppTools.Sample.ViewModels
{
    public class BikesPageViewModel : BaseViewModel
    {
        IBikeService bikeService = DependencyService.Resolve<IBikeService>();

        public ObservableRangeCollection<Bike> Bikes { get; set; } = new ObservableRangeCollection<Bike>();

        public async Task InitializeAsync()
        {
            Bikes.Clear();
            var bikes = await bikeService.GetAllBikesAsync();
            Bikes.AddRange(bikes);
        }

        public ICommand EditBikeCommand => new AsyncCommand<Bike>(EditBike);

        private async Task EditBike(Bike bike)
        {
            bike ??= new Bike();
            await Shell.Current.Navigation.PushAsync(new BikePage(bike));
        }

    }
}