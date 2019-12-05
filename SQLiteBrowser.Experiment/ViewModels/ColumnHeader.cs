using SQLite;
using static SQLite.TableMapping;

namespace SQLiteBrowser.Experiment.ViewModels
{
    public class ColumnHeader
    {
        public Column Column { get; set; }
        public string Name => Column.Name;
        public int MaxLength { get; set; }

        public ColumnHeader(Column column)
        {
            Column = column;
            MaxLength = Name.Length;
        }
    }
}