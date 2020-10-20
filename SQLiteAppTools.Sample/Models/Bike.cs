using System;
using SQLite;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SQLiteAppTools.Models
{
    public class Bike
    {

        public Bike()
        {
        }

        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Brand { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }
        public decimal Price { get; set; }
        public TimeSpan HoursRidden { get; set; }
        public double DistanceRidden { get; set; }
        public string SerialNo { get; set; } = Guid.NewGuid().ToString();
        public string PhotoUrl { get; set; }
        public string PersonId { get; set; }


        

    }
}
