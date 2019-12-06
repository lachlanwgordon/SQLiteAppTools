using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteBrowser.Experiment.Service
{
    public interface IDatabase
    {
        SQLiteAsyncConnection Connection { get; }

        IEnumerable<TableMapping> GetAllMappings();
        Task RegisterTypes(params Type[] types);
        Task<IEnumerable<object>> GetRecords(TableMapping mapping); 
    }
    public class Database : IDatabase
    {
        public SQLiteAsyncConnection Connection => new SQLiteAsyncConnection(Manager.DatabasePath);

        public async Task RegisterTypes(params Type[] types)
        {
            await Connection.CreateTablesAsync(CreateFlags.None, types);
        }

        public IEnumerable<TableMapping> GetAllMappings()
        {
            var mappings = Connection.TableMappings;
            return mappings;
        }

        public async Task<IEnumerable<object>> GetRecords(TableMapping mapping) 
        {
            var items = await Connection.QueryAsync(mapping, $"select * from {mapping.TableName}");

            return items;
        }
    }
}
