using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLiteAppTools.Models;
using SQLiteAppTools.Service;
using Xamarin.Forms;

namespace SQLiteAppTools.Sample.ModelServices
{
    public interface IPersonService
    {
        Task<List<Person>> GetAllPeopleAsync();
        Task SavePersonAsync(Person person);
    }

    public class PersonService : IPersonService
    {
        IDatabase database = DependencyService.Resolve<IDatabase>();

        public async Task<List<Person>> GetAllPeopleAsync()
        {
            await Task.Delay(500);

            var people = await database.Connection.Table<Person>().ToListAsync();
            if(people.Count == 0)
                await SeedDemoDataAsync();
            people = await database.Connection.Table<Person>().ToListAsync();


            return people;
        }
        public static Person NetBot;
        public static Person Clippy;
        public static Person GreenDroid;
        public static Person Monkey;
        private async Task SeedDemoDataAsync()
        {
            Monkey = new Person
            {
                DateOfBirth = new DateTime(1950, 12, 03),
                FavouriteNumber = 10,
                FirstName = "Xamarin",
                LastName = "Monkey",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(Monkey);
            GreenDroid = new Person
            {
                DateOfBirth = new DateTime(1980, 05, 02),
                FavouriteNumber = 7,
                FirstName = "Green",
                LastName = "Android",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(GreenDroid);
            NetBot = new Person
            {
                DateOfBirth = new DateTime(1970, 05, 02),
                FavouriteNumber = 459155949,
                FirstName = ".Net",
                LastName = "Robot",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(NetBot);
            
            Clippy = new Person
            {
                DateOfBirth = new DateTime(1980, 05, 02),
                FavouriteNumber = 7,
                FirstName = "Clippy",
                LastName = "Friend",
                UpdatedTimeStamp = DateTime.UtcNow
            };
            await database.Connection.InsertOrReplaceAsync(Clippy);
        }

        public async Task SavePersonAsync(Person person)
        {
            await database.Connection.InsertOrReplaceAsync(person);
        }
    }
}
