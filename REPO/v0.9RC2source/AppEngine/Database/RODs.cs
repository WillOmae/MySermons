using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class RODs : IDatabase<RODs>
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public RODs() { }
        public RODs(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(RODs tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(RODs tclass)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM RODs WHERE DocId=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", tclass.Id);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        public bool Exists(RODs tclass)
        {
            bool exists = false;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM RODS WHERE DocId=@id AND DocTitle=@title", connection))
                {
                    command.Parameters.AddWithValue("@id", tclass.Id);
                    command.Parameters.AddWithValue("@title", tclass.Title);

                    exists = command.ExecuteReader().HasRows;
                }
            }
            return exists;
        }
        public int Insert(RODs tclass)
        {
            int rowsAffected = 0;
            if (Exists(tclass)) { return 0; }
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("INSERT INTO RODs (DocId,DocTitle) VALUES (@id,@title)", connection))
                {
                    command.Parameters.AddWithValue("@id", tclass.Id);
                    command.Parameters.AddWithValue("@title", tclass.Title);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        public RODs Select(int id)
        {
            RODs rod = new RODs();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM RODs WHERE DocId=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rod.Id = int.Parse(reader["DocId"].ToString());
                            rod.Title = reader["DocTitle"].ToString();
                        }
                    }
                }
            }
            return rod;
        }
        public RODs Select(string title)
        {
            RODs rod = new RODs();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM RODs WHERE DocTitle=@title", connection))
                {
                    command.Parameters.AddWithValue("@title", title);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rod.Id = int.Parse(reader["DocId"].ToString());
                            rod.Title = reader["DocTitle"].ToString();
                        }
                    }
                }
            }
            return rod;
        }
        public List<RODs> SelectAll()
        {
            List<RODs> list = new List<RODs>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM RODs", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RODs rod = new RODs()
                            {
                                Id = int.Parse(reader["DocId"].ToString()),
                                Title = reader["DocTitle"].ToString()
                            };
                            list.Add(rod);
                        }
                    }
                }
            }
            return list;
        }
        public List<RODs> SelectMany(string column, string value)
        {
            List<RODs> list = new List<RODs>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM RODs WHERE " + column + "=@value", connection))
                {
                    command.Parameters.AddWithValue("@value", value);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RODs rod = new RODs()
                            {
                                Id = int.Parse(reader["DocId"].ToString()),
                                Title = reader["DocTitle"].ToString()
                            };
                            list.Add(rod);
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
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM RODs WHERE DocId=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Id = int.Parse(reader["DocId"].ToString());
                            Title = reader["DocTitle"].ToString();
                        }
                    }
                }
            }
        }
        public int Update(RODs tclass)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("UPDATE RODs SET DocTitle=@title WHERE DocId=@id", connection))
                {
                    command.Parameters.AddWithValue("@title", tclass.Title);
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
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `RODs` ( `DocId` INTEGER NOT NULL, `DocTitle` TEXT, PRIMARY KEY(`DocId`), FOREIGN KEY(`DocId`) REFERENCES Sermons(Id), FOREIGN KEY(`DocTitle`) REFERENCES Sermons(Title) )", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
