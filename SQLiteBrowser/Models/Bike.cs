using System;
using SQLite;
using System.Collections.Generic;

namespace SQLiteBrowser.Models
{
    public class Bike
    {
        public Bike()
        {
        }

        [PrimaryKey]
        public int? Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }
        public decimal Price { get; set; }
        public TimeSpan HoursRidden { get; set; }
        public double DistanceRidden { get; set; }
        public string SerialNo { get; set; } = Guid.NewGuid().ToString();
        public string PhotoUrl { get; set; } 


        public static List<Bike> SeedData => new List<Bike>
        {

            new Bike
            {
                  Brand = "Surly",
                  Model = "Straggler",
                  DistanceRidden = 10000,
                  HoursRidden = TimeSpan.FromHours(21000),
                  NumberOfWheels = 2,
                  Price = 2200,
                  PhotoUrl = $"https://picsum.photos/200/201"
            },
            new Bike
            {
                  Brand = "Kennedy",
                  Model = "N/a",
                  DistanceRidden = 12000,
                  HoursRidden = TimeSpan.FromHours(2100),
                  NumberOfWheels = 2,
                  Price = 520,
                  PhotoUrl = $"https://picsum.photos/200/202"
            },
            new Bike
            {
                  Brand = "Apollo",
                  Model = "Tandemania",
                  DistanceRidden = 2000,
                  HoursRidden = TimeSpan.FromHours(35000),
                  NumberOfWheels = 2,
                  Price = 450,
                  PhotoUrl = $"https://picsum.photos/200/203"
            },
            new Bike
            {
                  Brand = "Orbea",
                  Model = "Dakar",
                  DistanceRidden = 2000,
                  HoursRidden = TimeSpan.FromHours(28000),
                  NumberOfWheels = 2,
                  Price = 420,
                  PhotoUrl = $"https://picsum.photos/200/204"
            },
             new Bike
            {
                  Brand = "Olmo",
                  Model = "San Remo",
                  DistanceRidden = 500,
                  HoursRidden = TimeSpan.FromHours(280),
                  NumberOfWheels = 2,
                  Price = 450,
                  PhotoUrl = $"https://picsum.photos/200/205"
            },

        };

    }
}
