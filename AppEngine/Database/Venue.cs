using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class Venue : IDatabase<Venue>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TownId { get; set; }
        public string Town { get; set; }

        public Venue() { }
        public Venue(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(Venue tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(Venue tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("DELETE FROM VENUES WHERE Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", tclass.Id);

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
                return rowsAffected;
            }
            catch
            {
                return 0;
            }
        }
        public bool Exists(Venue tclass)
        {
            try
            {
                bool exists = false;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM VENUES WHERE Name=@name", connection))
                    {
                        command.Parameters.AddWithValue("@name", tclass.Name);

                        exists = command.ExecuteReader().HasRows;
                    }
                }
                return exists;
            }
            catch
            {
                return true;
            }
        }
        public int Insert(Venue tclass)
        {
            try
            {
                int rowsAffected = 0;
                if (Exists(tclass)) { return 0; }
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO VENUES (Name,Town) VALUES (@name,@town)", connection))
                    {
                        command.Parameters.AddWithValue("@name", tclass.Name);
                        command.Parameters.AddWithValue("@town", tclass.TownId);

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
                return rowsAffected;
            }
            catch
            {
                return 0;
            }
        }
        public Venue Select(int id)
        {
            try
            {
                Venue venue = new Venue();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT VENUES.Id AS venueid,VENUES.Name AS name,TOWNS.Name AS town FROM VENUES JOIN TOWNS ON VENUES.Town=TOWNS.Id WHERE VENUES.Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                venue.Id = int.Parse(reader["venueid"].ToString());
                                venue.Name = reader["name"].ToString();
                                venue.Town = reader["town"].ToString();
                            }
                        }
                    }
                }
                return venue;
            }
            catch
            {
                return null;
            }
        }
        public Venue Select(string name)
        {
            try
            {
                Venue venue = new Venue();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT VENUES.Id AS venueid,VENUES.Name AS name,TOWNS.Name AS town FROM VENUES JOIN TOWNS ON VENUES.Town=TOWNS.Id WHERE VENUES.Name=@name", connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                venue.Id = int.Parse(reader["venueid"].ToString());
                                venue.Name = reader["name"].ToString();
                                venue.Town = reader["town"].ToString();
                            }
                        }
                    }
                }
                return venue;
            }
            catch
            {
                return null;
            }
        }
        public List<Venue> SelectAll()
        {
            try
            {
                List<Venue> list = new List<Venue>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT VENUES.Id AS venueid,VENUES.Name AS name,TOWNS.Name AS town FROM VENUES JOIN TOWNS ON VENUES.Town=TOWNS.Id", connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Venue venue = new Venue()
                                {
                                    Id = int.Parse(reader["venueid"].ToString()),
                                    Name = reader["name"].ToString(),
                                    Town = reader["town"].ToString(),
                                };
                                list.Add(venue);
                            }
                        }
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
        public List<Venue> SelectMany(string column, string value)
        {
            try
            {
                List<Venue> list = new List<Venue>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT VENUES.Id AS venueid,VENUES.Name AS name,TOWNS.Name AS town FROM VENUES JOIN TOWNS ON VENUES.Town=TOWNS.Id WHERE " + column + "=@value", connection))
                    {
                        command.Parameters.AddWithValue("@value", value);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Venue venue = new Venue()
                                {
                                    Id = int.Parse(reader["venueid"].ToString()),
                                    Name = reader["name"].ToString(),
                                    Town = reader["town"].ToString()
                                };
                                list.Add(venue);
                            }
                        }
                    }
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
        public int Update(Venue tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("UPDATE VENUES SET Name=@name,Town=@town WHERE Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@name", tclass.Name);
                        command.Parameters.AddWithValue("@town", tclass.TownId);
                        command.Parameters.AddWithValue("@id", tclass.Id);

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
                return rowsAffected;
            }
            catch
            {
                return 0;
            }
        }
        public void SetObjectValues(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT VENUES.Id AS venueid,VENUES.Name AS name, VENUES.Town AS townid,TOWNS.Name AS town FROM VENUES JOIN TOWNS ON VENUES.Town=TOWNS.Id WHERE VENUES.Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Id = int.Parse(reader["venueid"].ToString());
                            Name = reader["name"].ToString();
                            TownId = int.Parse(reader["townid"].ToString());
                            Town = reader["town"].ToString();
                        }
                    }
                }
                Town = new Town(TownId).Name;
            }
            catch
            {

            }
        }
        static public void CreateTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `VENUES` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `Name` TEXT NOT NULL, `Town` INTEGER NOT NULL DEFAULT 0, FOREIGN KEY(`Town`) REFERENCES `TOWNS`(`Id`) )", connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
        }
    }
}
