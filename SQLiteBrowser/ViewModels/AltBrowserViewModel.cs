using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmHelpers;
using SQLiteBrowser.Models;

namespace SQLiteBrowser.ViewModels
{

    public class AltBrowserViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Table> Tables { get; set; }  = new ObservableRangeCollection<Table>();
        Table selectedTable;
        public Table SelectedTable
        {
            get => selectedTable;
            set => SetProperty(ref selectedTable, value);
        }
        public static string Path { get; set; }

        public static Stopwatch Stopwatch = new Stopwatch();
        public async Task OnAppearingAsync()
        {
            Stopwatch.Reset();
            Stopwatch.Start();


            Debug.WriteLine($"On appearing {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");
            var connection = new SQLite.SQLiteConnection(Path);
            Debug.WriteLine($"Connected {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

            var tables = Table.GetAll(connection);
            Debug.WriteLine($"Loaded {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

            Tables.AddRange(tables);
            Debug.WriteLine($"added {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

            SelectedTable = tables.FirstOrDefault();
            Debug.WriteLine($"selected {Stopwatch.ElapsedMilliseconds}ms   memory: {GC.GetTotalMemory(false)}");

        }
    }
}