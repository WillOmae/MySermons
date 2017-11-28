using System.Data.SQLite;
using System.IO;

namespace AppEngine
{
    public static class MainThread
    {
        public static void CheckDBExistence()
        {
            if (File.Exists(FileNames.dbLocation))
            {
                ConfirmDBIntegrity();
            }
            if (!File.Exists(FileNames.dbLocation))
            {
                try
                {
                    Directory.CreateDirectory(FileNames.dbFolder);
                    SQLiteConnection.CreateFile(FileNames.dbLocation);

                    ConfirmDBIntegrity();
                }
                catch
                {

                }
            }
        }
        public static bool ConfirmDBIntegrity()
        {
            try
            {
                bool alright = true;
                string[] tableNames = Tables.TableNames;

                foreach (var table in tableNames)
                {
                    if (!TableExists(table))
                    {
                        alright = false;
                        switch (table)
                        {
                            case "ACTIVITIES":
                                CreateTable(table, Tables.CreateActivities);
                                InitialiseTable(table, Tables.InsertActivities);
                                break;
                            case "PREFERENCES":
                                CreateTable(table, Tables.CreatePreferences);
                                InitialiseTable(table, Tables.InsertPreferences);
                                break;
                            case "RODs":
                                CreateTable(table, Tables.CreateRODs);
                                break;
                            case "SERIES":
                                CreateTable(table, Tables.CreateSeries);
                                InitialiseTable(table, Tables.InsertSeries);
                                break;
                            case "SERIESSPEAKERS":
                                CreateTable(table, Tables.CreateSeriesSpeakers);
                                break;
                            case "SERMONS":
                                CreateTable(table, Tables.CreateSermons);
                                break;
                            case "SPEAKERS":
                                CreateTable(table, Tables.CreateSpeakers);
                                InitialiseTable(table, Tables.InsertSpeakers);
                                break;
                            case "THEMES":
                                CreateTable(table, Tables.CreateThemes);
                                InitialiseTable(table, Tables.InsertThemes);
                                break;
                            case "TOWNS":
                                CreateTable(table, Tables.CreateTowns);
                                InitialiseTable(table, Tables.InsertTowns);
                                break;
                            case "VENUES":
                                CreateTable(table, Tables.CreateVenues);
                                InitialiseTable(table, Tables.InsertVenues);
                                break;
                        }
                    }
                }
                return alright;
            }
            catch
            {
                return false;
            }
        }
        public static bool TableExists(string name)
        {
            try
            {
                bool exists = false;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM sqlite_master WHERE type='table' AND name=@name", connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                                exists = true;
                            else
                                exists = false;
                        }
                    }
                }
                return exists;
            }
            catch
            {
                return false;
            }
        }
        public static void CreateTable(string name, string commandString)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(commandString, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
        }
        public static void InitialiseTable(string name, string commandString)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(commandString, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
        }
        public static void DropTable(string name)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("DROP TABLE IF EXISTS @name;", connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
        }

        public static void InitialChecks()
        {
            if (File.Exists(FileNames.tempBible))
            {
                if (File.Exists(FileNames.Bible))
                {
                    File.Delete(FileNames.Bible);
                }
                File.Move(FileNames.tempBible, FileNames.Bible);
            }
            if (!File.Exists(FileNames.PermVersionFile))
            {
                WriteXMLFile.CreateFile(FileNames.PermVersionFile);
            }
        }

        public struct CHECK_FILE_EXISTENCE_RESULTS
        {
            public bool VersionFile;
            public bool Bible;
            public bool Walkthroughs;
        }
        static public CHECK_FILE_EXISTENCE_RESULTS CheckFileExistence()
        {
            CHECK_FILE_EXISTENCE_RESULTS results;
            results.Bible = results.VersionFile = results.Walkthroughs = true;

            if (!File.Exists(FileNames.PermVersionFile))
            {
                results.VersionFile = false;
                WriteXMLFile.CreateFile(FileNames.PermVersionFile);
            }
            if (File.Exists(FileNames.tempBible))
            {
                if (File.Exists(FileNames.Bible))
                {
                    File.Delete(FileNames.Bible);
                }
                File.Move(FileNames.tempBible, FileNames.Bible);
            }
            if (!File.Exists(FileNames.Bible))
            {
                results.Bible = false;
            }
            if (!Directory.Exists(FileNames.WalkthroughsDirectory))
            {
                results.Walkthroughs = false;
            }

            return results;
        }
    }
}