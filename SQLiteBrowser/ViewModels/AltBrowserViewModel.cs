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
using SQLiteBrowser.Services;

namespace SQLiteBrowser.ViewModels
{
    public class AltBrowserViewModel : BaseViewModel
    {
        private List<Table> tables;
        public List<Table> Tables
        {
            get => tables;
            set => SetProperty(ref tables, value);
        }

        private List<Models.Cell> allCells;
        public List<Models.Cell> AllCells
        {
            get => allCells;
            set => SetProperty(ref allCells, value);
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

        public async Task LoadTableData(Table table)
        {
            await TableService.LoadData(table);

            AllCells = table.Rows.SelectMany(x => x.Cells).ToList();
        }

        public async Task LoadTables()
        {
            var tables = await TableService.GetAll();

            Tables = tables;
        }
    }
}
