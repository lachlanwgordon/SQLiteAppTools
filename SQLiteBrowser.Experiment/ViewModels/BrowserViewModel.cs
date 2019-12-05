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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MvvmHelpers;

namespace SQLiteBrowser.Experiment.ViewModels
{
    public class BrowserViewModel : BaseViewModel
    {
        public string MappingSummary { get; set; } = "Table info";

        private TableMapping selectedMapping;
        IDatabase db;

       

        public ObservableRangeCollection<TableMapping> Mappings { get; private set; } = new ObservableRangeCollection<TableMapping>();
        public List<object> Records { get; set; } = new List<object>();
        public ObservableRangeCollection<Row> Rows { get; set; } = new ObservableRangeCollection<Row>();
        public ObservableRangeCollection<ColumnHeader> Columns { get; set; } = new ObservableRangeCollection<ColumnHeader>();
        public int TotalCharacterLength
        {
            get
            {
                var length = Columns.Sum(x => x.MaxLength);
                return length;
            }
        }
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
            Rows.Clear();

            var columns = new List<ColumnHeader>();
            var rows = new List<Row>();
            SelectedMapping.Columns.ForEach(x => columns.Add(new ColumnHeader(x)));

            var records = await db.GetRecords(SelectedMapping);

            

            foreach (var record in records)
            {
                var row = new Row
                {
                    Properties = new List<Property>()
                };
                foreach (var column in columns)
                {
                    var props = new List<object>();
                    var prop = record.GetType().GetProperty(column.Name).GetValue(record);
                    var property = new Property { Value = prop, ColumnHeader = column };
                    row.Properties.Add(property);
                }
                rows.Add(row);
            }
            foreach (var column in columns)
            {
                var alltheProps = rows.SelectMany(x => x.Properties);
                var filterProps = alltheProps.Where(x => x.ColumnHeader.Name == column.Name);
                var max = filterProps.Max(x => x.Text.Length);
                column.MaxLength = Math.Max(max, column.MaxLength);
                //column.MaxLength = rows.SelectMany(x => x.Properties).Where(x => x.ColumnHeader.Name == column.Name).Max(x => x.Text.Length);

            }

            Columns = new ObservableRangeCollection<ColumnHeader>(columns);
            OnPropertyChanged(nameof(TotalCharacterLength));
            OnPropertyChanged(nameof(Columns));
            Rows.AddRange(rows);



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