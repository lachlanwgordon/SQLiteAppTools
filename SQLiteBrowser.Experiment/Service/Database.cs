using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using SQLiteBrowser.Experiment.ViewModels;

namespace SQLiteBrowser.Experiment.Service
{
    public interface IDatabase
    {
        //SQLiteAsyncConnection Connection { get; }
        //SQLiteConnection Conn { get; }

        Task<IEnumerable<TableMapping>> GetAllMappings();
        Task RegisterTypes(params Type[] types);
        Task<IEnumerable<object>> GetRecords(TableMapping mapping); 
    }
    public class Database : IDatabase
    {
        public SQLiteAsyncConnection Connection = new SQLiteAsyncConnection(Manager.DatabasePath);
        public SQLiteConnection Conn => new SQLiteConnection(Manager.DatabasePath);

        public async Task RegisterTypes(params Type[] types)
        {
            await Connection.CreateTablesAsync(CreateFlags.None, types);
        }

        public async Task<IEnumerable<TableMapping>> GetAllMappings()
        {
            CheckHandle();
            if(Manager.Types != null && Manager.Types.Count() > 0)
                await RegisterTypes(Manager.Types);
            await Connection.CreateTableAsync(typeof(TestModel));

            var mappings = Connection.TableMappings;
            return mappings;
        }

        public async Task<IEnumerable<object>> GetRecords(TableMapping mapping) 
        {
            await Connection.CreateTableAsync(mapping.MappedType);

            var info = await Connection.GetTableInfoAsync(mapping.TableName);
            var mappings = await Connection.GetMappingAsync(mapping.MappedType);
            var items = await Connection.QueryAsync(mapping, $"select * from {mapping.TableName}");
            var items2 = Conn.Query(mapping, $"select * from {mapping.TableName}");

            return items;
        }

        public void CheckHandle()
        {
//            Conn.Execute(".tables", "");

            var command = Conn.CreateCommand("select * from Experience");
            var experiences = command.ExecuteQuery<>();
            Debug.WriteLine(experiences.Count());
        }
    }
}
