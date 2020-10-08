using System;
namespace InAppDbViewer
{
    public static class InAppDbViewer
    {
        internal static string DatabasePath;
        public static void Init(string databasePath)
        {
            DatabasePath = databasePath;
        }
    }
}
