using System;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace SQLiteAppTools.Service
{
    public interface IDatabase
    {
        SQLiteAsyncConnection Connection { get; set; }
        Task RegisterTypes(params Type[] types);
    }
    public class Database : IDatabase
    {
        public SQLiteAsyncConnection Connection { get; set; } = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "browser.db3"));

        public async Task RegisterTypes(params Type[] types)
        {
            await Connection.CreateTablesAsync(CreateFlags.None, types);
        }
    }
}
