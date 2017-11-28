using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class Preferences : IDatabase<Preferences>
    {
        public string PrinterName { get; set; }
        public string PrinterScheme { get; set; }
        public string SortingFilter { get; set; }
        public int ROD_MaxNumber { get; set; }
        public string ColourFont { get; set; }
        public string ColourControls { get; set; }
        public string FontSystem { get; set; }
        public string FontReader { get; set; }
        public string FontWriter { get; set; }
        public bool ShowWelcomeScreen { get; set; }

        public int Alter(Preferences tclass)
        {
            throw new NotImplementedException();
        }

        static public void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `PREFERENCES` ( `PrinterName` TEXT, `PrinterScheme` TEXT DEFAULT 'White/Black', `ColourFont` TEXT DEFAULT 'AACCFF', `ColourControls` TEXT DEFAULT 222233, `FontSystem` TEXT DEFAULT 'Times New Roman', `FontReader` TEXT DEFAULT 'Times New Roman', `FontWriter` TEXT DEFAULT 'Times New Roman', `RODMaxNumber` INTEGER DEFAULT 10, `SortingFilter` TEXT DEFAULT 'SPEAKER', `ShowWelcomeScreen` TEXT DEFAULT 'True' )", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public int Delete(Preferences tclass)
        {
            throw new NotImplementedException();
        }
        public bool Exists(Preferences tclass)
        {
            throw new NotImplementedException();
        }
        public int Insert(Preferences tclass)
        {
            throw new NotImplementedException();
        }
        public Preferences Select(int id)
        {
            throw new NotImplementedException();
        }
        public Preferences Select(string name)
        {
            throw new NotImplementedException();
        }
        public List<Preferences> SelectAll()
        {
            List<Preferences> list = new List<Preferences>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM PREFERENCES", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Preferences preferences = new Preferences()
                            {
                                ColourControls = reader["ColourControls"].ToString(),
                                ColourFont = reader["ColourFont"].ToString(),
                                FontReader = reader["FontReader"].ToString(),
                                FontSystem = reader["FontSystem"].ToString(),
                                FontWriter = reader["FontWriter"].ToString(),
                                PrinterName = reader["PrinterName"].ToString(),
                                PrinterScheme = reader["PrinterScheme"].ToString(),
                                ROD_MaxNumber = int.Parse(reader["RODMaxNumber"].ToString()),
                                ShowWelcomeScreen = bool.Parse(reader["ShowWelcomeScreen"].ToString()),
                                SortingFilter = reader["SortingFilter"].ToString()
                            };
                            list.Add(preferences);
                        }
                    }
                }
            }
            return list;
        }
        static public List<Preferences> SelectAllStatic()
        {
            List<Preferences> list = new List<Preferences>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM PREFERENCES", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Preferences preferences = new Preferences()
                            {
                                ColourControls = reader["ColourControls"].ToString(),
                                ColourFont = reader["ColourFont"].ToString(),
                                FontReader = reader["FontReader"].ToString(),
                                FontSystem = reader["FontSystem"].ToString(),
                                FontWriter = reader["FontWriter"].ToString(),
                                PrinterName = reader["PrinterName"].ToString(),
                                PrinterScheme = reader["PrinterScheme"].ToString(),
                                ROD_MaxNumber = int.Parse(reader["RODMaxNumber"].ToString()),
                                ShowWelcomeScreen = bool.Parse(reader["ShowWelcomeScreen"].ToString()),
                                SortingFilter = reader["SortingFilter"].ToString()
                            };
                            list.Add(preferences);
                        }
                    }
                }
            }
            return list;
        }
        public List<Preferences> SelectMany(string column, string value)
        {
            List<Preferences> list = new List<Preferences>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM PREFERENCES WHERE " + column + "=@value", connection))
                {
                    command.Parameters.AddWithValue("@value", value);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Preferences preferences = new Preferences()
                            {
                                ColourControls=reader["ColourControls"].ToString(),
                                ColourFont = reader["ColourFont"].ToString(),
                                FontReader = reader["FontReader"].ToString(),
                                FontSystem = reader["FontSystem"].ToString(),
                                FontWriter = reader["FontWriter"].ToString(),
                                PrinterName = reader["PrinterName"].ToString(),
                                PrinterScheme = reader["PrinterScheme"].ToString(),
                                ROD_MaxNumber = int.Parse(reader["RODMaxNumber"].ToString()),
                                ShowWelcomeScreen = bool.Parse(reader["ShowWelcomeScreen"].ToString()),
                                SortingFilter = reader["SortingFilter"].ToString()
                            };
                            list.Add(preferences);
                        }
                    }
                }
            }
            return list;
        }
        public void SetObjectValues(int id)
        {
            throw new NotImplementedException();
        }
        public int Update(Preferences tclass)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("UPDATE PREFERENCES SET ColourControls=@colourcontrols,ColourFont=@colourfont,FontReader=@fontreader,FontSystem=@fontsystem,FontWriter=@fontwriter,PrinterName=@printername,PrinterScheme=@printerscheme,RODMaxNumber=@rodmaxnumber,ShowWelcomeScreen=@showwelcomescreen,SortingFilter=@sortingfilter", connection))
                {
                    command.Parameters.AddWithValue("@colourcontrols", tclass.ColourControls);
                    command.Parameters.AddWithValue("@colourfont", tclass.ColourFont);
                    command.Parameters.AddWithValue("@fontreader", tclass.FontReader);
                    command.Parameters.AddWithValue("@fontsystem", tclass.FontSystem);
                    command.Parameters.AddWithValue("@fontwriter", tclass.FontWriter);
                    command.Parameters.AddWithValue("@printername", tclass.PrinterName);
                    command.Parameters.AddWithValue("@printerscheme", tclass.PrinterScheme);
                    command.Parameters.AddWithValue("@rodmaxnumber", tclass.ROD_MaxNumber);
                    command.Parameters.AddWithValue("@showwelcomescreen", tclass.ShowWelcomeScreen.ToString());
                    command.Parameters.AddWithValue("@sortingfilter", tclass.SortingFilter);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
    }
}
