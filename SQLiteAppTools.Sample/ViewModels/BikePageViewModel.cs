using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SQLiteAppTools.Models;
using SQLiteAppTools.Sample.ModelServices;
using Xamarin.Forms;

namespace SQLiteAppTools.Sample.ViewModels
{
    public class BikePageViewModel : BaseViewModel
    {
        IPersonService personService = DependencyService.Resolve<IPersonService>();
        IBikeService bikeService = DependencyService.Resolve<IBikeService>();
        private Person selectedPerson;

        public ObservableRangeCollection<Person> People { get; set; } = new ObservableRangeCollection<Person>();
        public Person SelectedPerson
        {
            get => selectedPerson;
            set => SetProperty(ref selectedPerson, value);
        }

        public Bike Bike { get; set; }
        public BikePageViewModel(Bike bike)
        {
            Bike = bike;
            _ = Init();
        }

        private async Task Init()
        {
            var people = await personService.GetAllPeopleAsync();
            People.AddRange(people);
            SelectedPerson = People.FirstOrDefault(x => x.Id == Bike.PersonId);
        }

        public ICommand SaveCommand => new AsyncCommand(Save);

        private async Task Save()
        {
            Bike.PersonId = selectedPerson.Id;
            await bikeService.SaveBikeAsync(Bike);
        }
    }
}