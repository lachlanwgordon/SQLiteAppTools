using System.Diagnostics;
using MvvmHelpers;
using SQLite;
using SQLiteBrowser.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Linq;

namespace SQLiteBrowser.ViewModels
{

    public class AltBrowserViewModel : BaseViewModel
    {
        bool isPopupShowing;
        public bool IsPopupShowing
        {
            get => isPopupShowing;
            set => SetProperty(ref isPopupShowing, value);
        }

        Row selectedItem;
        public Row SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public ICommand TableSelected => new Command(LoadTableData);
        public ICommand OpenItemCommand => new Command(OpenItem);
        public ICommand ClosePopupCommand => new Command(ClosePopup);

        private void ClosePopup(object obj)
        {
            SelectedItem = null;
            IsPopupShowing = false;
        }

        private void OpenItem(object obj)
        {
            var item = obj as Row;
            SelectedItem = item;
            IsPopupShowing = true;

        }

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

        public ObservableRangeCollection<Models.Cell> AllCells { get; set; } = new ObservableRangeCollection<Models.Cell>();


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
                LoadTableData();
            }
        }

        private void LoadTableData()
        {
            var watch = new Stopwatch();
            watch.Start();
            _ = SelectedTable.LoadData(connection);
            ColumnHeaders = selectedTable.HeaderRow;
            AltRows.Clear();
            AltRows.AddRange(selectedTable.FormattedRows);

            var allCells = selectedTable.FormattedRows.SelectMany(x => x.Cells);
            var allStrings = allCells;//.Select(x => x.DisplayText);
            AllCells.Clear();
            AllCells.AddRange(allStrings);

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