using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;
using SQLite;
using SQLiteBrowser.Models;
using static SQLite.SQLiteConnection;
using System.Windows.Input;
using Xamarin.Forms;
using MvvmHelpers.Commands;

namespace SQLiteBrowser.ViewModels
{

    public class AltBrowserViewModel : BaseViewModel
    {
        

        public ICommand TableSelected => new AsyncCommand(LoadTableData);
        public ICommand LoadMoreRows => new AsyncCommand(LoadModeRows);

        private const int ColumnCount = 8;

        private async Task LoadModeRows()
        {
            //if(AllRows.Count() > Rows.Count)
            //{
            //    Debug.WriteLine("Load more");
            //    //var newRows = AllRows.Skip(Rows.Count).Take(20).Select(x => x.Select(y => y?.ToString()).ToList()).ToList();
            //    var newRows = AllRows.Skip(Rows.Count).Take(20);//.Select(x => x.Select(y => y.ToString()).ToList()).ToList();
            //    var stringRows = new List<List<string>>();
            //    foreach (var row in newRows)
            //    {
            //        var stringRow = row.Select(x => x?.ToString()).Take(ColumnCount).ToList();
            //        stringRows.Add(stringRow);
            //    }

            //    Rows.AddRange(stringRows);
            //}
            //else
            //{
            //    Debug.WriteLine("Can't load more");
            //}
        }

        public ObservableRangeCollection<Table> Tables { get; set; }  = new ObservableRangeCollection<Table>();
        public IEnumerable<IEnumerable<object>> AllRows { get; set; }
        public ObservableRangeCollection<List<string>> Rows { get; set; } = new ObservableRangeCollection<List<string>>();
        public ObservableRangeCollection<ColumnInfo> Columns { get; set; } = new ObservableRangeCollection<ColumnInfo>();

        public ObservableRangeCollection<Row> AltRows { get; set; } = new ObservableRangeCollection<Row>();

        Row columnHeaders;
        public Row ColumnHeaders
        {
            get => columnHeaders;
            set
            {
                SetProperty(ref columnHeaders, value) ;
            }
        }


        Table selectedTable;
        public Table SelectedTable
        {
            get => selectedTable;
            set
            {
                SetProperty(ref selectedTable, value);
            }
        }

        private async Task LoadTableData()
        {
            //Columns.Clear();
            //await Task.Delay(10);
            //Debug.WriteLine($"select {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");
            //Rows.Clear();
            //await Task.Delay(10);

            //Debug.WriteLine($"cleared {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

            AltRows.Clear();

            var data = SelectedTable.LoadData(connection);
            ColumnHeaders = selectedTable.HeaderRow;

            try
            {

                AltRows.AddRange(selectedTable.FormattedRows);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to dislay table");
                await Task.Delay(10);
                AltRows = new ObservableRangeCollection<Row>(selectedTable.FormattedRows);
                OnPropertyChanged(nameof(AltRows));
            }


            //ColumnHeaders = new Row
            //{
            //    Property0 = SelectedTable.ColumnInfos.ElementAtOrDefault(0)?.ToString(),
            //    Property1 = SelectedTable.ColumnInfos.ElementAtOrDefault(1)?.ToString(),
            //    Property2 = SelectedTable.ColumnInfos.ElementAtOrDefault(2)?.ToString(),
            //    Property3 = SelectedTable.ColumnInfos.ElementAtOrDefault(3)?.ToString(),
            //    Property4 = SelectedTable.ColumnInfos.ElementAtOrDefault(4)?.ToString(),
            //    Property5 = SelectedTable.ColumnInfos.ElementAtOrDefault(5)?.ToString(),

            //};
            //var newRows = new List<Row>();
            //foreach (var row in data)
            //{
            //    var newRow = new Row
            //    {
            //        Property0 = row.ElementAtOrDefault(0) != null ? row.ElementAtOrDefault(0).ToString() : "...",//  .ToString(),
            //        Property1 = row.ElementAtOrDefault(1)?.ToString(),
            //        Property2 = row.ElementAtOrDefault(2)?.ToString(),
            //        Property3 = row.ElementAtOrDefault(3)?.ToString(),
            //        Property4 = row.ElementAtOrDefault(4)?.ToString(),
            //        Property5 = row.ElementAtOrDefault(5)?.ToString(),
            //    };
            //    newRows.Add(newRow);
            //}
            //AltRows.AddRange(newRows);
            //Columns.AddRange(SelectedTable.ColumnInfos.Take(ColumnCount));
            //Debug.WriteLine($"loaded {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");
            //AllRows = data;




            //var newRows = AllRows.Take(40);//.Select(x => x.Select(y => y.ToString()).ToList()).ToList();
            //var stringRows = new List<List<string>>();

            //foreach (var row in newRows)
            //{
            //    var stringRow = row.Select(x => x?.ToString()).Take(ColumnCount).ToList();
            //    stringRows.Add(stringRow);
            //}

            //Rows.AddRange(stringRows);
            //Debug.WriteLine($"done {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");
        }



        public static string Path { get; set; }

        public static Stopwatch Stopwatch = new Stopwatch();
        private SQLiteConnection connection;

        public async Task OnAppearingAsync()
        {
            Stopwatch.Reset();
            Stopwatch.Start();


            Debug.WriteLine($"On appearing {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");
            connection = new SQLite.SQLiteConnection(Path);
            Debug.WriteLine($"Connected {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

            var tables = Table.GetAll(connection, false);
            Debug.WriteLine($"Loaded {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

            Tables.AddRange(tables);
            Debug.WriteLine($"added {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

            //SelectedTable = tables.FirstOrDefault();
            Debug.WriteLine($"selected {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

        }
    }
}