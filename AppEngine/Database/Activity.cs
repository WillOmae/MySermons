using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class Activity : IDatabase<Activity>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Activity() { }
        public Activity(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(Activity tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(Activity tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("DELETE FROM ACTIVITIES WHERE Id=@id", connection))
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
        public bool Exists(Activity tclass)
        {
            try
            {
                bool exists = false;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM ACTIVITIES WHERE Name=@name", connection))
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
        public int Insert(Activity tclass)
        {
            try
            {
                int rowsAffected = 0;
                if (Exists(tclass)) { return 0; }
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO ACTIVITIES (Name) VALUES (@name)", connection))
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
        public Activity Select(int id)
        {
            try
            {
                Activity activity = new Activity();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM ACTIVITIES WHERE Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                activity.Id = int.Parse(reader["Id"].ToString());
                                activity.Name = reader["Name"].ToString();
                            }
                        }
                    }
                }
                return activity;
            }
            catch
            {
                return null;
            }
        }
        public Activity Select(string name)
        {
            try
            {
                Activity activity = new Activity();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM ACTIVITIES WHERE Name=@name", connection))
                    {
                        command.Parameters.AddWithValue("@name", name);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                activity.Id = int.Parse(reader["Id"].ToString());
                                activity.Name = reader["Name"].ToString();
                            }
                        }
                    }
                }
                return activity;
            }
            catch
            {
                return null;
            }
        }
        public List<Activity> SelectAll()
        {
            try
            {
                List<Activity> list = new List<Activity>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM ACTIVITIES", connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Activity activity = new Activity()
                                {
                                    Id = int.Parse(reader["Id"].ToString()),
                                    Name = reader["Name"].ToString()
                                };
                                list.Add(activity);
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
        public List<Activity> SelectMany(string column, string value)
        {
            try
            {
                List<Activity> list = new List<Activity>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM ACTIVITIES WHERE " + column + "=@value", connection))
                    {
                        command.Parameters.AddWithValue("@value", value);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Activity activity = new Activity()
                                {
                                    Id = int.Parse(reader["Id"].ToString()),
                                    Name = reader["Name"].ToString()
                                };
                                list.Add(activity);
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
        public void SetObjectValues(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM ACTIVITIES WHERE Id=@id", connection))
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
        public int Update(Activity tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("UPDATE ACTIVITIES SET Name=@name WHERE Id=@id", connection))
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
        static public void CreateTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `ACTIVITIES` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `Name` TEXT NOT NULL UNIQUE )", connection))
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