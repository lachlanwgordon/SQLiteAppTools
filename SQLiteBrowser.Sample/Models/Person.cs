using System;
using SQLite;

namespace SQLiteBrowser.Models
{
    public class Person
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Indexed]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int FavouriteNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
        public string Town { get; set; } = "Melbourne";
    }
}
