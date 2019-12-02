using System;
using SQLite;

namespace SQLiteBrowser.Models
{
    public class Person
    { 
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        [Indexed]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int FavouriteNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
    }
}
