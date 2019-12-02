using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLiteBrowser.Models;
using SQLiteBrowser.Service;
using Xamarin.Forms;

namespace SQLiteBrowser.ModelServices
{
    public interface IPersonService
    {

        Task<List<Person>> GetAllPeopleAsync();
        Task<Person> GetPersonByIdAsync(int id);
        Task SavePersonAsync(Person person);
    }

    public class PersonService : IPersonService
    {
        IDatabase database = DependencyService.Resolve<IDatabase>();

        public async Task<List<Person>> GetAllPeopleAsync()
        {
            await Task.Delay(500);
            var people =  await database.Connection.Table<Person>().ToListAsync();
            if(!people.Any())
            {
                await SeedDemoDataAsync();
                people = await database.Connection.Table<Person>().ToListAsync();
            }
            return people;
        }

        private async Task SeedDemoDataAsync()
        {
            var person = new Person
            {
                DateOfBirth = new DateTime(1980, 05, 02),
                FavouriteNumber = 7,
                FirstName = "Oreo",
                LastName = "Button",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(person);
            var person2 = new Person
            {
                DateOfBirth = new DateTime(1980, 05, 02),
                FavouriteNumber = 7,
                FirstName = "Gravy",
                LastName = "Butt",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(person2);
        }

        public async Task<Person> GetPersonByIdAsync(int id)
        {
            return await database.Connection.GetAsync<Person>(id);
        }

        public async Task SavePersonAsync(Person person)
        {
            await database.Connection.InsertOrReplaceAsync(person);
        }
    }
}
