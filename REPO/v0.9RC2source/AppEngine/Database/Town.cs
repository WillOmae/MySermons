using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class Town : IDatabase<Town>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Town() { }
        public Town(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(Town tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(Town tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("DELETE FROM TOWNS WHERE Id=@id", connection))
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
        public bool Exists(Town tclass)
        {
            try
            {
                bool exists = false;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM TOWNS WHERE Name=@name", connection))
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
        public int Insert(Town tclass)
        {
            try
            {
                int rowsAffected = 0;
                if (Exists(tclass)) { return 0; }
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO TOWNS (Name) VALUES (@name)", connection))
                    {
                        command.Parameters.AddWithValue("@name", tclass.Name);

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
        public Town Select(int id)
        {
            try
            {
                Town town = new Town();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM TOWNS WHERE Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                town.Id = int.Parse(reader["Id"].ToString());
                                town.Name = reader["Name"].ToString();
                            }
                        }
                    }
                }
                return town;
            }
            catch
            {
                return null;
            }
        }
        public Town Select(string name)
        {
            try
            {
                Town town = new Town();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM TOWNS WHERE Name=@name", connection))
                    {
                        command.Parameters.AddWithValue("@name", name);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                town.Id = int.Parse(reader["Id"].ToString());
                                town.Name = reader["Name"].ToString();
                            }
                        }
                    }
                }
                return town;
            }
            catch
            {
                return null;
            }
        }
        public List<Town> SelectAll()
        {
            try
            {
                List<Town> list = new List<Town>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM TOWNS", connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Town town = new Town()
                                {
                                    Id = int.Parse(reader["Id"].ToString()),
                                    Name = reader["Name"].ToString()
                                };
                                list.Add(town);
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
        public List<Town> SelectMany(string column, string value)
        {
            try
            {
                List<Town> list = new List<Town>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM TOWNS WHERE " + column + "=@value", connection))
                    {
                        command.Parameters.AddWithValue("@value", value);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Town town = new Town()
                                {
                                    Id = int.Parse(reader["Id"].ToString()),
                                    Name = reader["Name"].ToString()
                                };
                                list.Add(town);
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
        public int Update(Town tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("UPDATE TOWNS SET Name=@name WHERE Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@name", tclass.Name);
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
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM TOWNS WHERE Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            Id = int.Parse(reader["Id"].ToString());
                            Name = reader["Name"].ToString();
                        }
                    }
                }
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
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `TOWNS` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `Name` TEXT NOT NULL UNIQUE )", connection))
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
