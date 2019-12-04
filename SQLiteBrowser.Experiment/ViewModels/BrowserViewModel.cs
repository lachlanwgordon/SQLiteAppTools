using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using SQLiteBrowser.Experiment.Service;
using Xamarin.Forms.Internals;
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
        public ObservableCollection<Column> Columns { get; set; } = new ObservableCollection<Column>();
        public TableMapping SelectedMapping
        {
            get => selectedMapping;
            
            set
            {
                selectedMapping = value;
                LoadRecords();
            }
        }

        private async Task LoadRecords()
        {
            Columns.Clear();
            SelectedMapping.Columns.ForEach(x => Columns.Add(x));

            var records = await db.GetRecords(SelectedMapping);
            Records.Clear();
            records.ForEach(x => Records.Add(x));


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