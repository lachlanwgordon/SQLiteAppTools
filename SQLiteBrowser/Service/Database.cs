using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using SQLiteBrowser.ViewModels;

namespace SQLiteBrowser.Experiment.Service
{
   
    internal class Database
    {
        public string Path;
        public SQLiteAsyncConnection AsyncConnection;// => new SQLiteAsyncConnection(Path);
        public SQLiteConnection Connection;// => new SQLiteConnection(Path);

        public Type[] Types { get; }

        public Database(string path, params Type[] types)
        {
            Types = types;
            Path = path;
        }

        public Database(SQLiteConnection connection, SQLiteAsyncConnection asyncConnection)
        {
            Connection = connection;
            AsyncConnection = asyncConnection;
        }

        public async Task RegisterTypes(params Type[] types)
        {
            await AsyncConnection.CreateTablesAsync(CreateFlags.None, types);
        }

        public async Task<IEnumerable<TableMapping>> GetAllMappings()
        {
            if(Types != null && Types.Count() > 0)
                await RegisterTypes(Types);
            await AsyncConnection.CreateTableAsync(typeof(TestModel));

            var mappings = Connection.TableMappings;
            return mappings;
        }

        public async Task<IEnumerable<object>> GetRecords(TableMapping mapping) 
        {
            await AsyncConnection.CreateTableAsync(mapping.MappedType);

            //var info = await Connection.GetTableInfoAsync(mapping.TableName);
            //var mappings = await Connection.GetMappingAsync(mapping.MappedType);
            var items = await AsyncConnection.QueryAsync(mapping, $"select * from {mapping.TableName}");
            //var items2 = Conn.Query(mapping, $"select * from {mapping.TableName}");

            return items;
        }


    }
}
