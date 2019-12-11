using System;
using SQLite;
using Xamarin.Forms;

namespace SQLiteBrowser.Experiment.ViewModels
{
    public class TestModel 
    {
        [PrimaryKey]
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}

