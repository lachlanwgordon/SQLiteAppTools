using System;
namespace SQLiteBrowser.Experiment
{
    public class Manager
    {
        public static string DatabasePath { get; set; }
        public static void Initialize(string databasePath)
        {
            DatabasePath = databasePath;
        }
    }
}
