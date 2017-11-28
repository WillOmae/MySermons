using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class Theme : IDatabase<Theme>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Theme()
        {

        }
        public Theme(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(Theme tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(Theme tclass)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("DELETE FROM THEMES WHERE Id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", tclass.Id);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        public bool Exists(Theme tclass)
        {
            bool exists = false;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM THEMES WHERE Name=@name", connection))
                {
                    command.Parameters.AddWithValue("@name", tclass.Name);

                    exists = command.ExecuteReader().HasRows;
                }
            }
            return exists;
        }
        public int Insert(Theme tclass)
        {
            int rowsAffected = 0;
            if (Exists(tclass)) { return 0; }
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("INSERT INTO THEMES (Name) VALUES (@name)", connection))
                {
                    command.Parameters.AddWithValue("@name", tclass.Name);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        public Theme Select(int id)
        {
            Theme theme = new Theme();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM THEMES WHERE Id=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            theme.Id = int.Parse(reader["Id"].ToString());
                            theme.Name = reader["Name"].ToString();
                        }
                    }
                }
            }
            return theme;
        }
        public Theme Select(string name)
        {
            Theme theme = new Theme();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM THEMES WHERE Name=@name", connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            theme.Id = int.Parse(reader["Id"].ToString());
                            theme.Name = reader["Name"].ToString();
                        }
                    }
                }
            }
            return theme;
        }
        public List<Theme> SelectAll()
        {
            try
            {
                List<Theme> list = new List<Theme>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM THEMES", connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Theme theme = new Theme()
                                {
                                    Id = int.Parse(reader["Id"].ToString()),
                                    Name = reader["Name"].ToString()
                                };
                                list.Add(theme);
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
        public List<Theme> SelectMany(string column, string value)
        {
            List<Theme> list = new List<Theme>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM THEMES WHERE " + column + "=@value", connection))
                {
                    command.Parameters.AddWithValue("@value", value);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Theme theme = new Theme()
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString()
                            };
                            list.Add(theme);
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
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM THEMES WHERE Id=@id", connection))
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
        public int Update(Theme tclass)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("UPDATE THEMES SET Name=@name WHERE Id=@id", connection))
                {
                    command.Parameters.AddWithValue("@name", tclass.Name);
                    command.Parameters.AddWithValue("@id", tclass.Id);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
    }
}
