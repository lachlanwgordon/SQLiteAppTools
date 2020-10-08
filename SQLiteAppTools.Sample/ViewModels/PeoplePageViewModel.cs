using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SQLiteAppTools.Models;
using SQLiteAppTools.ModelServices;

namespace SQLiteAppTools.ViewModels
{
    public class PeoplePageViewModel
    {
        IPersonService personService = Xamarin.Forms.DependencyService.Resolve<IPersonService>();
        public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();

        public async Task InitializeAsync()
        {
            var people = await personService.GetAllPeopleAsync();
            people.ForEach(x => People.Add(x));
        }
    }
 }