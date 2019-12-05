using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using SQLiteBrowser.Experiment.Service;
using Xamarin.Forms.Internals;
using System.Reflection;
using static SQLite.TableMapping;

namespace SQLiteBrowser.Experiment.ViewModels
{
    public class BrowserViewModel
    {
        public string MappingSummary
        {
            get;
            set;
        } = "Table info";

        private TableMapping selectedMapping;
        IDatabase db;


        public ObservableCollection<TableMapping> Mappings { get; private set; } = new ObservableCollection<TableMapping>();
        public ObservableCollection<object> Records { get; set; } = new ObservableCollection<object>();
        public ObservableCollection<Row> Rows { get; set; } = new ObservableCollection<Row>();
        public ObservableCollection<ColumnHeader> Columns { get; set; } = new ObservableCollection<ColumnHeader>();
        public int TotalCharacterLength => Columns.Sum(x => x.Name.Length);
        public TableMapping SelectedMapping
        {
            get => selectedMapping;
            
            set
            {
                selectedMapping = value;
                _ = LoadRecords();
            }
        }

        private async Task LoadRecords()
        {
            Columns.Clear();
            SelectedMapping.Columns.ForEach(x => Columns.Add(new ColumnHeader(x)));

            var records = await db.GetRecords(SelectedMapping);
            Records.Clear();
            Rows.Clear();
            records.ForEach(x => Records.Add(x));



            foreach (var record in records)
            {
                var row = new Row
                {
                     Properties = new List<Property>()
                };
                foreach (var column in Columns)
                {
                    var props = new List<object>();
                    var prop = record.GetType().GetProperty(column.Name).GetValue(record);
                    row.Properties.Add(new Property { Value = prop, Width = column.Width });
                }
                Rows.Add(row);
            }



        }

        public async Task InitializeAsync()
        {
            db = new Database();
            var mappings = db.GetAllMappings();
            mappings.ForEach(x => Mappings.Add(x));
            SelectedMapping = Mappings.FirstOrDefault();
            if (selectedMapping != null)
                MappingSummary = selectedMapping.TableName;
        }
    }
}