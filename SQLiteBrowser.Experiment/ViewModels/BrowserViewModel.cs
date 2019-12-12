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

namespace SQLiteBrowser.ViewModels
{
    public class BrowserViewModel : BaseViewModel
    {
        private Database db;

        public ObservableRangeCollection<TableMapping> Mappings { get; private set; } = new ObservableRangeCollection<TableMapping>();
        public ObservableRangeCollection<Row> Rows { get; set; } = new ObservableRangeCollection<Row>();
        public ObservableRangeCollection<ColumnHeader> Columns { get; set; } = new ObservableRangeCollection<ColumnHeader>();


        private TableMapping selectedMapping;
        public TableMapping SelectedMapping
        {
            get => selectedMapping;

            set
            {
                selectedMapping = value;
                OnPropertyChanged(nameof(SelectedMapping));
            }
        }

        public async Task LoadRecords()
        {
            if (selectedMapping == null)
                return;

            Rows.Clear();
            Columns.Clear();

            var columns = new List<ColumnHeader>();
            var rows = new List<Row>();
            SelectedMapping.Columns.ForEach(x => columns.Add(new ColumnHeader(x) { ColumnNumber = SelectedMapping.Columns.IndexOf(x) }));

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
                    if (!string.IsNullOrWhiteSpace(property.Text))
                        column.MaxLength = Math.Max(column.MaxLength, property.Text.Length);
                }
                rows.Add(row);
            }

            Columns = new ObservableRangeCollection<ColumnHeader>(columns);
            Rows.AddRange(rows);
            OnPropertyChanged(nameof(Columns));
        }



        public async Task InitializeAsync(SQLiteConnection connection, SQLiteAsyncConnection asyncConnection)
        {
            if (db == null)
            {
                db = new Database(connection, asyncConnection);

                var mappings = (await db.GetAllMappings()).ToList();
                mappings.Remove(mappings.FirstOrDefault(x => x.TableName == "ColumnInfo"));
                Mappings.Clear();
                Mappings.AddRange(mappings);
            }
        }


    }
}