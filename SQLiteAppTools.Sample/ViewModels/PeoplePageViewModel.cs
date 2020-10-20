using System;
using System.Collections.ObjectModel;
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
    public class PeoplePageViewModel : BaseViewModel
    {
        IPersonService personService = DependencyService.Resolve<IPersonService>();

        public ObservableRangeCollection<Person> People { get; set; } = new ObservableRangeCollection<Person>();

        public async Task InitializeAsync()
        {
            People.Clear();
            var people = await personService.GetAllPeopleAsync();
            People.AddRange(people);
        }

        public ICommand EditPersonCommand => new AsyncCommand<Person>(AddPerson);
        private async Task AddPerson(Person person)
        {
            person ??= new Person();
            await Shell.Current.Navigation.PushAsync(new PersonPage(person));
        }
    }
 }