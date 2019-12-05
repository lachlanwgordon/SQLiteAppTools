using SQLite;
using static SQLite.TableMapping;

namespace SQLiteBrowser.Experiment.ViewModels
{
    public class ColumnHeader
    {
        public Column Column { get; set; }
        public string Name => Column.Name;
        public double Width { get; set; }

        public ColumnHeader(Column column)
        {
            Column = column;

            switch (column.ColumnType.Name)
            {
                case "String":
                    Width = 100;
                    break;
                case "Double":
                    Width = 50;
                    break;
                default:
                    Width = 150;
                    break;
            }
        }
    }
}