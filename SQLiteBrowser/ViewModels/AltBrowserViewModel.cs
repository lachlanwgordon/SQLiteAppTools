using System.Diagnostics;
using SQLite;
using SQLiteBrowser.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SQLiteBrowser.ViewModels
{

    public class AltBrowserViewModel : BaseViewModel
    {
        //public ICommand TableSelected => new Command(LoadTableData);

        private List<Table> tables;
        public List<Table> Tables
        {
            get => tables;
            set => SetProperty(ref tables, value);

        }

        List<Row> rows = new List<Row>();
        public List<Row> Rows
        {
            get => rows;
            set
            {
                SetProperty(ref rows, value);
            }
        }

        private List<Models.Cell> allCells;
        public List<Models.Cell> AllCells
        {
            get => allCells;
            set => SetProperty(ref allCells, value);
        }


        Row columnHeaders;
        public Row ColumnHeaders
        {
            get => columnHeaders;
            set
            {
                SetProperty(ref columnHeaders, value);
            }
        }

        Table selectedTable;
        public Table SelectedTable
        {
            get => selectedTable;
            set
            {
                //LoadTableData(value);
                SetProperty(ref selectedTable, value);
            }
        }

        public async Task LoadTableData(Table table)
        {
            table.LoadData(connection);
            ColumnHeaders = table.HeaderRow;
            Rows = table.FormattedRows;

            AllCells = Rows.SelectMany(x => x.Cells).ToList();
        }

        public static string Path { get; set; }

        private SQLiteConnection connection;

        public void LoadTables()
        {
            if (Path == null)
                return;

            connection = new SQLiteConnection(Path);

            var tables = Table.GetAll(connection, false);

            Tables = tables;
        }
    }
}