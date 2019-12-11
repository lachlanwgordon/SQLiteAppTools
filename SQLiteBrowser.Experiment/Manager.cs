using System;
namespace SQLiteBrowser.Experiment
{
    public class Manager
    {
        public static string DatabasePath { get; set; }
        public static Type[] Types { get; private set; }

        public static void Initialize(string databasePath)
        {
            DatabasePath = databasePath;
        }
        public static void RegisterTypes(params Type[] types)
        {
            Types = types;
        }
    }
}
