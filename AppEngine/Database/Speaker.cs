using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class Speaker : IDatabase<Speaker>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Speaker() { }
        public Speaker(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(Speaker tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(Speaker tclass)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("DELETE FROM SPEAKERS WHERE Id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", tclass.Id);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        public bool Exists(Speaker tclass)
        {
            bool exists = false;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SPEAKERS WHERE Name=@name", connection))
                {
                    command.Parameters.AddWithValue("@name", tclass.Name);

                    exists = command.ExecuteReader().HasRows;
                }
            }
            return exists;
        }
        public int Insert(Speaker tclass)
        {
            int rowsAffected = 0;
            if (Exists(tclass)) { return 0; }
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("INSERT INTO SPEAKERS (Name) VALUES (@name)", connection))
                {
                    command.Parameters.AddWithValue("@name", tclass.Name);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        public Speaker Select(int id)
        {
            Speaker speaker = new Speaker();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SPEAKERS WHERE Id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            speaker.Id = int.Parse(reader["Id"].ToString());
                            speaker.Name = reader["Name"].ToString();
                        }
                    }
                }
            }
            return speaker;
        }
        public Speaker Select(string name)
        {
            Speaker speaker = new Speaker();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SPEAKERS WHERE Name=@Name", connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            speaker.Id = int.Parse(reader["Id"].ToString());
                            speaker.Name = reader["Name"].ToString();
                        }
                    }
                }
            }
            return speaker;
        }
        public List<Speaker> SelectAll()
        {
            List<Speaker> list = new List<Speaker>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SPEAKERS", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Speaker speaker = new Speaker()
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString()
                            };
                            list.Add(speaker);
                        }
                    }
                }
            }
            return list;
        }
        public List<Speaker> SelectMany(string column, string value)
        {
            List<Speaker> list = new List<Speaker>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SPEAKERS WHERE " + column + "=@value", connection))
                {
                    command.Parameters.AddWithValue("@value", value);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Speaker speaker = new Speaker()
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString()
                            };
                            list.Add(speaker);
                        }
                    }
                }
            }
            return list;
        }
        public void SetObjectValues(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SPEAKERS WHERE Id=@id", connection))
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
        public int Update(Speaker tclass)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("UPDATE SPEAKERS SET Name=@name WHERE Id=@id", connection))
                {
                    command.Parameters.AddWithValue("@name", tclass.Name);
                    command.Parameters.AddWithValue("@id", tclass.Id);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }

        static public void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `SPEAKERS` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `Name` TEXT NOT NULL UNIQUE )", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
