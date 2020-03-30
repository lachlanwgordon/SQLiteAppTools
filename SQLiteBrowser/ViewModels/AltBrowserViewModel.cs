using System.Diagnostics;
using MvvmHelpers;
using SQLite;
using SQLiteBrowser.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace SQLiteBrowser.ViewModels
{

    public class AltBrowserViewModel : BaseViewModel
    {
        public ICommand TableSelected => new Command(LoadTableData);

        public ObservableRangeCollection<Table> Tables { get; set; }  = new ObservableRangeCollection<Table>();


        ObservableRangeCollection<Row> altRows = new ObservableRangeCollection<Row>(); 
        public ObservableRangeCollection<Row> AltRows
        {
            get => altRows;
            set
            {
                SetProperty(ref altRows, value);
            }
        }

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

        private void LoadTableData()
        {
            var watch = new Stopwatch();
            watch.Start();
            Debug.WriteLine($"LoadTableData1 {watch.ElapsedMilliseconds}ms");
            _ = SelectedTable.LoadData(connection);
            Debug.WriteLine($"LoadTableData2 {watch.ElapsedMilliseconds}ms");
            ColumnHeaders = selectedTable.HeaderRow;
            Debug.WriteLine($"LoadTableData3 {watch.ElapsedMilliseconds}ms");

            AltRows.AddRange(selectedTable.FormattedRows);
            //AltRows = new ObservableRangeCollection<Row>(selectedTable.FormattedRows);
            Debug.WriteLine($"LoadTableData {watch.ElapsedMilliseconds}ms");
        }

        public static string Path { get; set; }

        public static Stopwatch Stopwatch = new Stopwatch();
        private SQLiteConnection connection;

        public void OnAppearing()
        {
            if (Path == null)
                return;

            connection = new SQLiteConnection(Path);

            var tables = Table.GetAll(connection, false);

            Tables.AddRange(tables);
        }
    }
}