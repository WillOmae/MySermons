using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AppEngine.Database
{
    public class Series : IDatabase<Series>
    {
        public int Id { get; set; }
        public string Theme { get; set; }
        public int VenueId { get; set; }
        public string Venue
        {
            get
            {
                return _venue;
            }
            set
            {
                Venue venue = new Venue();
                if ((venue = StaticLists.listOfVenues.Find(x => x.Name == value)) == null)
                {
                    venue = new Venue()
                    {
                        Name = value,
                        TownId = TownId
                    };
                    venue.Insert(venue);
                    venue = venue.Select(value);
                    StaticLists.listOfVenues.Add(venue);
                }
                _venue = venue.Name;
                VenueId = venue.Id;
            }
        }
        private string _venue = string.Empty;
        public int TownId { get; set; }
        public string Town
        {
            get
            {
                return _town;
            }
            set
            {
                Town town = new Town();
                if ((town = StaticLists.listOfTowns.Find(x => x.Name == value)) == null)
                {
                    town = new Town()
                    {
                        Name = value
                    };
                    town.Insert(town);
                    town = town.Select(value);
                    StaticLists.listOfTowns.Add(town);
                }
                _town = town.Name;
                TownId = town.Id;
            }
        }
        private string _town = string.Empty;
        public int ActivityId { get; set; }
        public string Activity
        {
            get
            {
                return _activity;
            }
            set
            {
                Activity activity = new Activity();
                if ((activity = StaticLists.listOfActivities.Find(x => x.Name == value)) == null)
                {
                    activity = new Activity()
                    {
                        Name = value
                    };
                    activity.Insert(activity);
                    activity = activity.Select(value);
                    StaticLists.listOfActivities.Add(activity);
                }
                _activity = activity.Name;
                ActivityId = activity.Id;
            }
        }
        private string _activity = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SpeakerId { get; set; }
        public string Speaker
        {
            get
            {
                return _speaker;
            }
            set
            {
                Speaker speaker = new Speaker();
                if ((speaker = StaticLists.listOfSpeakers.Find(x => x.Name == value)) == null)
                {
                    speaker = new Speaker()
                    {
                        Name = value
                    };
                    speaker.Insert(speaker);
                    speaker = speaker.Select(value);
                    StaticLists.listOfSpeakers.Add(speaker);
                }
                _speaker = speaker.Name;
                SpeakerId = speaker.Id;
            }
        }
        private string _speaker = string.Empty;
        public string Name
        {
            get
            {
                if (Theme == "")
                {
                    return "none";
                }
                else
                {
                    return Theme + " by " + Speaker;
                }
            }
        }

        public Series() { }
        public Series(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(Series tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(Series tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("DELETE FROM SERIES WHERE Id=@id", connection))
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
        public bool Exists(Series tclass)
        {
            try
            {
                bool exists = false;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SERIES WHERE Speaker=@speakerid AND Venue=@venueid AND Town=@townid AND Activity=@activityid AND Theme=@theme", connection))
                    {
                        command.Parameters.AddWithValue("@speakerid", tclass.SpeakerId);
                        command.Parameters.AddWithValue("@venueid", tclass.VenueId);
                        command.Parameters.AddWithValue("@townid", tclass.TownId);
                        command.Parameters.AddWithValue("@activityid", tclass.ActivityId);
                        command.Parameters.AddWithValue("@theme", tclass.Theme);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            exists = reader.HasRows;
                            while (reader.Read())
                            {
                                Id = int.Parse(reader["Id"].ToString());
                            }
                        }
                    }
                }
                return exists;
            }
            catch
            {
                return true;
            }
        }
        public int Insert(Series tclass)
        {
            try
            {
                int rowsAffected = 0;
                if (Exists(tclass)) { return 0; }
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO `SERIES`(`Theme`,`Speaker`,`Venue`,`Town`,`Activity`,`StartDate`,`EndDate`) VALUES (@theme,@speakerid,@venueid,@townid,@activityid,@startdate,@enddate);", connection))
                    {
                        command.Parameters.AddWithValue("@theme", tclass.Theme);
                        command.Parameters.AddWithValue("@speakerid", tclass.SpeakerId);
                        command.Parameters.AddWithValue("@venueid", tclass.VenueId);
                        command.Parameters.AddWithValue("@townid", tclass.TownId);
                        command.Parameters.AddWithValue("@activityid", tclass.ActivityId);
                        command.Parameters.AddWithValue("@startdate", tclass.StartDate.ToString(@"yyyy\/MM\/dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@enddate", tclass.EndDate.ToString(@"yyyy\/MM\/dd HH:mm:ss"));

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
                return rowsAffected;
            }
            catch
            {
                return -1;
            }
        }
        public Series Select(int id)
        {
            try
            {
                Series series = new Series();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT SERIES.Id AS seriesid,SERIES.Theme AS theme,SERIES.StartDate AS startdate,SERIES.EndDate AS enddate,SPEAKERS.Name AS speaker,VENUES.Name AS venue,TOWNS.Name AS town,ACTIVITIES.Name AS activity FROM SERIES JOIN SPEAKERS ON SERIES.Speaker=SPEAKERS.Id JOIN VENUES ON SERIES.Venue=VENUES.Id JOIN TOWNS ON SERIES.Town=TOWNS.Id JOIN ACTIVITIES ON SERIES.Activity=ACTIVITIES.Id WHERE SERIES.Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                series.Id = int.Parse(reader["seriesid"].ToString());
                                series.Activity = reader["activity"].ToString();
                                series.EndDate = DateTime.Parse(reader["enddate"].ToString());
                                series.StartDate = DateTime.Parse(reader["startdate"].ToString());
                                series.Speaker = reader["speaker"].ToString();
                                series.Venue = reader["venue"].ToString();
                                series.Theme = reader["theme"].ToString();
                                series.Town = reader["town"].ToString();
                            }
                        }
                    }
                }
                return series;
            }
            catch
            {
                return null;
            }
        }
        public Series Select(string name)
        {
            throw new NotImplementedException();
        }
        public List<Series> SelectAll()
        {
            try
            {
                List<Series> list = new List<Series>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT SERIES.Id AS seriesid,SERIES.Theme AS theme,SERIES.StartDate AS startdate,SERIES.EndDate AS enddate,SPEAKERS.Name AS speaker,VENUES.Name AS venue,TOWNS.Name AS town,ACTIVITIES.Name AS activity FROM SERIES JOIN SPEAKERS ON SERIES.Speaker=SPEAKERS.Id JOIN VENUES ON SERIES.Venue=VENUES.Id JOIN TOWNS ON SERIES.Town=TOWNS.Id JOIN ACTIVITIES ON SERIES.Activity=ACTIVITIES.Id", connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Series series = new Series()
                                {
                                    Id = int.Parse(reader["seriesid"].ToString()),
                                    Activity = reader["activity"].ToString(),
                                    EndDate = DateTime.Parse(reader["enddate"].ToString()),
                                    StartDate = DateTime.Parse(reader["startdate"].ToString()),
                                    Speaker = reader["speaker"].ToString(),
                                    Venue = reader["venue"].ToString(),
                                    Theme = reader["theme"].ToString(),
                                    Town = reader["town"].ToString()
                                };
                                list.Add(series);
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                return null;
            }
        }
        public List<Series> SelectMany(string column, string value)
        {
            try
            {
                List<Series> list = new List<Series>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT SERIES.Id AS seriesid,SERIES.Theme AS theme,SERIES.StartDate AS startdate,SERIES.EndDate AS enddate,SPEAKERS.Name AS speaker,VENUES.Name AS venue,TOWNS.Name AS town,ACTIVITIES.Name AS activity FROM SERIES JOIN SPEAKERS ON SERIES.Speaker=SPEAKERS.Id JOIN VENUES ON SERIES.Venue=VENUES.Id JOIN TOWNS ON SERIES.Town=TOWNS.Id JOIN ACTIVITIES ON SERIES.Activity=ACTIVITIES.Id WHERE " + column + "=@value", connection))
                    {
                        command.Parameters.AddWithValue("@value", value);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Series series = new Series()
                                {
                                    Id = int.Parse(reader["seriesid"].ToString()),
                                    Activity = reader["activity"].ToString(),
                                    EndDate = DateTime.Parse(reader["enddate"].ToString()),
                                    StartDate = DateTime.Parse(reader["startdate"].ToString()),
                                    Speaker = reader["speaker"].ToString(),
                                    Venue = reader["venue"].ToString(),
                                    Theme = reader["theme"].ToString(),
                                    Town = reader["town"].ToString()
                                };
                                list.Add(series);
                            }
                        }
                    }
                }
                foreach (Series series in list)
                {
                    List<SeriesSpeakers> listSeriesSpeakers = new SeriesSpeakers().SelectMany("SeriesId", series.Id);
                    foreach (SeriesSpeakers ss in listSeriesSpeakers)
                    {
                        series.Speaker = new Speaker(ss.SpeakerId).Name;
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
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT SERIES.Id AS seriesid,SERIES.Theme AS theme,SERIES.StartDate AS startdate,SERIES.EndDate AS enddate,SPEAKERS.Name AS speaker,VENUES.Name AS venue,TOWNS.Name AS town,ACTIVITIES.Name AS activity FROM SERIES JOIN SPEAKERS ON SERIES.Speaker=SPEAKERS.Id JOIN VENUES ON SERIES.Venue=VENUES.Id JOIN TOWNS ON SERIES.Town=TOWNS.Id JOIN ACTIVITIES ON SERIES.Activity=ACTIVITIES.Id WHERE SERIES.Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = int.Parse(reader["seriesid"].ToString());
                                Activity = reader["activity"].ToString();
                                EndDate = DateTime.Parse(reader["enddate"].ToString());
                                StartDate = DateTime.Parse(reader["startdate"].ToString());
                                Speaker = reader["speaker"].ToString();
                                Venue = reader["venue"].ToString();
                                Theme = reader["theme"].ToString();
                                Town = reader["town"].ToString();
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public int Update(Series tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("UPDATE SERIES SET Theme=@theme,Speaker=@speaker,Venue=@venueid,Town=@townid,Activity=@activityid,StartDate=@startdate,EndDate=@enddate", connection))
                    {
                        command.Parameters.AddWithValue("@theme", tclass.Theme);
                        command.Parameters.AddWithValue("@speaker", tclass.SpeakerId);
                        command.Parameters.AddWithValue("@venueid", tclass.VenueId);
                        command.Parameters.AddWithValue("@townid", tclass.TownId);
                        command.Parameters.AddWithValue("@activityid", tclass.ActivityId);
                        command.Parameters.AddWithValue("@startdate", tclass.StartDate.ToString(@"yyyy\/MM\/dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@enddate", tclass.EndDate.ToString(@"yyyy\/MM\/dd HH:mm:ss"));

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
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `SERIES` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `Theme` TEXT NOT NULL, `Speaker` INTEGER NOT NULL DEFAULT 0, `Venue` INTEGER NOT NULL DEFAULT 0, `Town` INTEGER NOT NULL DEFAULT 0, `Activity` INTEGER NOT NULL DEFAULT 0, `StartDate` TEXT NOT NULL, `EndDate` TEXT NOT NULL, FOREIGN KEY(`Venue`) REFERENCES `VENUES`(`Id`), FOREIGN KEY(`Town`) REFERENCES `TOWNS`(`Id`), FOREIGN KEY(`Activity`) REFERENCES `ACTIVITIES`(`Id`) )", connection))
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
