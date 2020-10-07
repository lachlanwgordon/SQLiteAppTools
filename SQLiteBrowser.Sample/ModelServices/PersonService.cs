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
            await SeedDemoDataAsync();

            var people =  await database.Connection.Table<Person>().ToListAsync();
            
            return people;
        }
        public static Person Richard;
        public static Person Bob;
        public static Person Janet;
        public static Person Monkey;
        private async Task SeedDemoDataAsync()
        {
            Bob = new Person
            {
                DateOfBirth = new DateTime(1980, 05, 02),
                FavouriteNumber = 7,
                FirstName = "Bob",
                LastName = "Brown",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(Bob);
            Janet = new Person
            {
                DateOfBirth = new DateTime(1980, 05, 02),
                FavouriteNumber = 7,
                FirstName = "Janet",
                LastName = "Rice",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(Janet);
            Richard = new Person
            {
                DateOfBirth = new DateTime(1970, 05, 02),
                FavouriteNumber = 459155949,
                FirstName = "Richard",
                LastName = "Dinatale",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(Richard);
            Monkey = new Person
            {
                DateOfBirth = new DateTime(1950, 12, 03),
                FavouriteNumber = 10,
                FirstName = "Xamarin",
                LastName = "Monkey",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(Monkey);
            await database.Connection.InsertAllAsync(Bike.SeedData);
            //This seed data for other classes doesn't really belong here but it's just nonsense to put some stuff in the db, so who really cares


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
