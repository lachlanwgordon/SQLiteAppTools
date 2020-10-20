using System;
using SQLiteAppTools.Models;
using System.Windows.Input;
using Xamarin.Forms;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using SQLiteAppTools.Sample.ModelServices;

namespace SQLiteAppTools.Sample.ViewModels
{
    public class PersonPageViewModel : BaseViewModel
    {
        IPersonService personService = DependencyService.Resolve<IPersonService>();

        public Person Person { get; set; }
        public PersonPageViewModel(Person person)
        {
            Person = person;
        }
        public ICommand SaveCommand => new AsyncCommand(Save);

        private async Task Save()
        {
            await personService.SavePersonAsync(Person);
            await Shell.Current.Navigation.PopAsync();
        }
    }
}