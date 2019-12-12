using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SQLiteBrowser.Models;
using SQLiteBrowser.ModelServices;

namespace SQLiteBrowser.ViewModels
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