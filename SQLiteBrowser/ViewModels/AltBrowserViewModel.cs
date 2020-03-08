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

        public async Task OnAppearingAsync()
        {
            //var connection = new SQLite.SQLiteConnection(Browser.Connection.DatabasePath);
            var connection = new SQLite.SQLiteConnection(Path);


            
            var tables = Table.GetAll(connection);
            Tables.AddRange(tables);
            SelectedTable = tables.FirstOrDefault();
        }
    }
}