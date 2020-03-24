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

        private void LoadTableData()
        {
            AltRows.Clear();

            _ = SelectedTable.LoadData(connection);
            ColumnHeaders = selectedTable.HeaderRow;

            AltRows.AddRange(selectedTable.FormattedRows);
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