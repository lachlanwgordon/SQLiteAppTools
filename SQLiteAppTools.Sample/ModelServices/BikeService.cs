using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLiteAppTools.Models;
using SQLiteAppTools.Service;
using Xamarin.Forms;

namespace SQLiteAppTools.Sample.ModelServices
{
    public interface IBikeService
    {

        Task<List<Bike>> GetAllBikesAsync();
        Task SaveBikeAsync(Bike bike);
    }

    public class BikeService : IBikeService
    {
        IDatabase database = DependencyService.Resolve<IDatabase>();

        public async Task<List<Bike>> GetAllBikesAsync()
        {
            await Task.Delay(500);

            var bikes = await database.Connection.Table<Bike>().ToListAsync();
            if (bikes.Count == 0)
                await SeedDemoDataAsync();
            bikes = await database.Connection.Table<Bike>().ToListAsync();

            return bikes;
        }
        private async Task SeedDemoDataAsync()
        {
            var bikes = new List<Bike>
            {
                new Bike
                {
                      Brand = "Surly",
                      Model = "Straggler",
                      DistanceRidden = 10000,
                      HoursRidden = TimeSpan.FromHours(21000),
                      NumberOfWheels = 2,
                      Price = 2200,
                      PhotoUrl = $"https://picsum.photos/200/201",
                      PersonId = PersonService.Clippy.Id,
                },
                new Bike
                {
                      Brand = "Kennedy",
                      Model = "N/a",
                      DistanceRidden = 12000,
                      HoursRidden = TimeSpan.FromHours(2100),
                      NumberOfWheels = 2,
                      Price = 520,
                      PhotoUrl = $"https://picsum.photos/200/202",
                      PersonId = PersonService.GreenDroid.Id,

                },
                new Bike
                {
                      Brand = "Apollo",
                      Model = "Tandemania",
                      DistanceRidden = 2000,
                      HoursRidden = TimeSpan.FromHours(35000),
                      NumberOfWheels = 2,
                      Price = 450,
                      PhotoUrl = $"https://picsum.photos/200/203",
                      PersonId = PersonService.NetBot.Id,

                },
                new Bike
                {
                      Brand = "Orbea",
                      Model = "Dakar",
                      DistanceRidden = 2000,
                      HoursRidden = TimeSpan.FromHours(28000),
                      NumberOfWheels = 2,
                      Price = 420,
                      PhotoUrl = $"https://picsum.photos/200/204",
                      PersonId = PersonService.Monkey.Id,
                },
                 new Bike
                {
                      Brand = "Olmo",
                      Model = "San Remo",
                      DistanceRidden = 500,
                      HoursRidden = TimeSpan.FromHours(280),
                      NumberOfWheels = 2,
                      Price = 450,
                      PhotoUrl = $"https://picsum.photos/200/205",
                      PersonId = PersonService.GreenDroid.Id,
                },

            };
            await database.Connection.InsertAllAsync(bikes);

        }

        public async Task SaveBikeAsync(Bike bike)
        {
            await database.Connection.InsertOrReplaceAsync(bike);
        }
    }
}