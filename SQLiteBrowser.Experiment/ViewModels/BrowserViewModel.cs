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
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace SQLiteBrowser.Experiment.ViewModels
{
    public class BrowserViewModel : BaseViewModel
    {
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get;
            set;
        }

        public ICommand TableSelectedCommand => new Command(TableSelected);

        private async void TableSelected(object obj)
        {
            try
            {
                Debug.WriteLine("table selected");
                await LoadRecords();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading {ex}\n{ex.StackTrace}");
            }

        }

        public string MappingSummary { get; set; } = "Table info";

        private TableMapping selectedMapping;
        IDatabase db;

       

        public ObservableRangeCollection<TableMapping> Mappings { get; private set; } = new ObservableRangeCollection<TableMapping>();
        public ObservableRangeCollection<Row> Rows { get; set; } = new ObservableRangeCollection<Row>();
        public ObservableRangeCollection<ColumnHeader> Columns { get; set; } = new ObservableRangeCollection<ColumnHeader>();


        public TableMapping SelectedMapping
        {
            get => selectedMapping;

            set
            {
                selectedMapping = value;
                //_ = LoadRecords();
                OnPropertyChanged(nameof(SelectedMapping));
            }
        }

        public async Task LoadRecords()
        {
            if (selectedMapping == null)
                return;
            Debug.WriteLine("Load Records");

            Rows.Clear();
            Columns.Clear();

            var columns = new List<ColumnHeader>();
            var rows = new List<Row>();
            SelectedMapping.Columns.ForEach(x => columns.Add(new ColumnHeader(x) { ColumnNumber = SelectedMapping.Columns.IndexOf(x)}));

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
                    column.MaxLength = Math.Max(column.MaxLength, property.Text.Length);
                }
                rows.Add(row);
            }
            //foreach (var column in columns)
            //{
            //    var alltheProps = rows.SelectMany(x => x.Properties);
            //    var filterProps = alltheProps.Where(x => x.ColumnHeader.Name == column.Name);
            //    //var max = filterProps.Max(x => x.Text.Length);
            //    //column.MaxLength = Math.Max(max, column.MaxLength);
            //    //column.MaxLength = rows.SelectMany(x => x.Properties).Where(x => x.ColumnHeader.Name == column.Name).Max(x => x.Text.Length);

            //}

            Columns = new ObservableRangeCollection<ColumnHeader>(columns);
            //Columns.AddRange(columns);
            Rows.AddRange(rows);
            //OnPropertyChanged(nameof(ColumnLengths));
            OnPropertyChanged(nameof(Columns));



        }



        public async Task InitializeAsync()
        {
            Debug.WriteLine("Init");
            await Task.Delay(500);


            db = new Database();



            var mappings = (await db.GetAllMappings()).ToList();
            mappings.Remove(mappings.FirstOrDefault(x => x.TableName == "ColumnInfo"));
            Mappings.Clear();
            Mappings.AddRange(mappings);
            //if(selectedMapping == null)
            //    SelectedMapping = Mappings.FirstOrDefault();
            //if (SelectedMapping != null)
            //    MappingSummary = selectedMapping.TableName;
        }


    }
}