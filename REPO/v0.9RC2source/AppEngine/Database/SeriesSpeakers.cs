using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class SeriesSpeakers : IDatabase<SeriesSpeakers>
    {
        public int SeriesId { get; set; }
        public int SpeakerId { get; set; }

        public SeriesSpeakers() { }
        public SeriesSpeakers(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(SeriesSpeakers tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(SeriesSpeakers tclass)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("DELETE FROM SERIESSPEAKERS WHERE SeriesId=@seriesid AND SpeakerId=@speakerid", connection))
                {
                    command.Parameters.AddWithValue("@seriesid", tclass.SeriesId);
                    command.Parameters.AddWithValue("@speakerid", tclass.SpeakerId);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        public bool Exists(SeriesSpeakers tclass)
        {
            bool exists = false;
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SERIESSPEAKERS WHERE SeriesId=@seriesid AND SpeakerId=@speakerid", connection))
                {
                    command.Parameters.AddWithValue("@seriesid", tclass.SeriesId);
                    command.Parameters.AddWithValue("@speakerid", tclass.SpeakerId);

                    exists = command.ExecuteReader().HasRows;
                }
            }
            return exists;
        }
        public int Insert(SeriesSpeakers tclass)
        {
            int rowsAffected = 0;
            if (Exists(tclass)) { return 0; }
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("INSERT INTO SERIESSPEAKERS (SeriesId,SpeakerId) VALUES (@seriesid,@speakerid)", connection))
                {
                    command.Parameters.AddWithValue("@seriesid", tclass.SeriesId);
                    command.Parameters.AddWithValue("@speakerid", tclass.SpeakerId);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }
        public SeriesSpeakers Select(int id)
        {
            SeriesSpeakers seriesSpeaker = new SeriesSpeakers();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SERIESSPEAKERS WHERE SeriesId=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            seriesSpeaker.SeriesId = int.Parse(reader["SeriesId"].ToString());
                            seriesSpeaker.SpeakerId = int.Parse(reader["SpeakerId"].ToString());
                        }
                    }
                }
            }
            return seriesSpeaker;
        }
        public SeriesSpeakers Select(string name)
        {
            throw new NotImplementedException();
        }
        public List<SeriesSpeakers> SelectAll()
        {
            List<SeriesSpeakers> list = new List<SeriesSpeakers>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SERIESSPEAKERS", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SeriesSpeakers town = new SeriesSpeakers()
                            {
                                SeriesId = int.Parse(reader["SeriesId"].ToString()),
                                SpeakerId = int.Parse(reader["SpeakerId"].ToString())
                            };
                            list.Add(town);
                        }
                    }
                }
            }
            return list;
        }
        public List<SeriesSpeakers> SelectMany(string column, string value)
        {
            List<SeriesSpeakers> list = new List<SeriesSpeakers>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SERIESSPEAKERS WHERE " + column + "=@value", connection))
                {
                    command.Parameters.AddWithValue("@value", value);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SeriesSpeakers town = new SeriesSpeakers()
                            {
                                SeriesId = int.Parse(reader["SeriesId"].ToString()),
                                SpeakerId = int.Parse(reader["SpeakerId"].ToString())
                            };
                            list.Add(town);
                        }
                    }
                }
            }
            return list;
        }
        public List<SeriesSpeakers> SelectMany(string column, int value)
        {
            List<SeriesSpeakers> list = new List<SeriesSpeakers>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SERIESSPEAKERS WHERE " + column + "=@value", connection))
                {
                    command.Parameters.AddWithValue("@value", value);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SeriesSpeakers town = new SeriesSpeakers()
                            {
                                SeriesId = int.Parse(reader["SeriesId"].ToString()),
                                SpeakerId = int.Parse(reader["SpeakerId"].ToString())
                            };
                            list.Add(town);
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
        public int Update(SeriesSpeakers tclass)
        {
            throw new NotImplementedException();
        }

        static public void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `SERIESSPEAKERS` ( `SeriesId` INTEGER NOT NULL, `SpeakerId` INTEGER NOT NULL, PRIMARY KEY(`SeriesId`,`SpeakerId`), FOREIGN KEY(`SeriesId`) REFERENCES Series(Id), FOREIGN KEY(`SpeakerId`) REFERENCES Speakers(Id) )", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}