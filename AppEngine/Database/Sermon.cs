using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace AppEngine.Database
{
    public class Sermon : IDatabase<Sermon>
    {
        public int Id { get; set; }
        private int _SeriesId = 0;
        public int SeriesId
        {
            get
            {
                return _SeriesId;
            }
            set
            {
                _SeriesId = value;
                Series = new Series(value).Name;
            }
        }
        public string Series { get; set; }
        public DateTime DateCreated { get; set; }
        public int VenueId { get; set; }
        public string Venue
        {
            get
            {
                return _venue;
            }
            set
            {
                Venue venue = new Venue()
                {
                    Name = value
                };
                if (!venue.Exists(venue))//set venue does not exist. Create it
                {
                    venue.TownId = TownId;
                    venue.Insert(venue);
                }
                venue = venue.Select(value);//get the created venue

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
                Town town = new Town()
                {
                    Name = value
                };
                if (!town.Exists(town))
                {
                    town.Insert(town);
                }
                town = town.Select(value);
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
                Activity activity = new Activity()
                {
                    Name = value
                };
                if (!activity.Exists(activity))
                {
                    activity.Insert(activity);
                }
                activity = activity.Select(value);

                _activity = activity.Name;
                ActivityId = activity.Id;
            }
        }
        private string _activity = string.Empty;
        public int SpeakerId { get; set; }
        public string Speaker
        {
            get
            {
                return _speaker;
            }
            set
            {
                Speaker speaker = new Speaker()
                {
                    Name = value
                };
                if (!speaker.Exists(speaker))
                {
                    speaker.Insert(speaker);
                }
                speaker = speaker.Select(value);

                _speaker = speaker.Name;
                SpeakerId = speaker.Id;
            }
        }
        private string _speaker = string.Empty;
        public string Title { get; set; }
        public int ThemeId { get; set; }
        public string Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                Theme theme = new Theme()
                {
                    Name = value
                };
                if (!theme.Exists(theme))//set venue does not exist. Create it
                {
                    theme.Insert(theme);
                }
                theme = theme.Select(value);//get the created venue

                _theme = theme.Name;
                ThemeId = theme.Id;
            }
        }
        private string _theme = string.Empty;
        public string KeyText { get; set; }
        public string Hymn { get; set; }
        public string Content { get; set; }
        public int Count
        {
            get
            {
                try
                {
                    int count = 0;
                    using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                    {
                        connection.Open();
                        using (SQLiteCommand command = new SQLiteCommand("SELECT count(Id) FROM SERMONS", connection))
                        {
                            count = Convert.ToInt32(command.ExecuteScalar());
                        }
                    }
                    return count;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public Sermon() { }
        public Sermon(int id)
        {
            SetObjectValues(id);
        }
        public int Alter(Sermon tclass)
        {
            throw new NotImplementedException();
        }
        public int Delete(Sermon tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand("DELETE FROM `SERMONS` WHERE `Id`=@id", connection))
                        {
                            command.Parameters.AddWithValue("@id", tclass.Id);

                            rowsAffected = command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                return rowsAffected;
            }
            catch
            {
                return 0;
            }
        }
        public bool Exists(Sermon tclass)
        {
            try
            {
                bool exists = false;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM SERMONS WHERE Venue=@venueid AND Town=@townid AND Activity=@activityid AND Title=@title AND Speaker=@speakerid", connection))
                    {
                        command.Parameters.AddWithValue("@venueid", tclass.VenueId);
                        command.Parameters.AddWithValue("@townid", tclass.TownId);
                        command.Parameters.AddWithValue("@activityid", tclass.ActivityId);
                        command.Parameters.AddWithValue("@title", tclass.Title);
                        command.Parameters.AddWithValue("@speakerid", tclass.SpeakerId);
                        command.Parameters.AddWithValue("@series", tclass.SeriesId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (exists = reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Id = int.Parse(reader["Id"].ToString());
                                }
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
        public int Insert(Sermon tclass)
        {
            try
            {
                int rowsAffected = 0;
                if (Exists(tclass)) { return 0; }
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("INSERT INTO SERMONS(Series,DateCreated,Venue,Town,Activity,Speaker,Title,Theme,Text,Hymn,Content) VALUES (@seriesid,@datecreated,@venueid,@townid,@activityid,@speakerid,@title,@themeid,@text,@hymn,@content);", connection))
                    {
                        command.Parameters.AddWithValue("@seriesid", tclass.SeriesId);
                        command.Parameters.AddWithValue("@datecreated", tclass.DateCreated.ToString(@"yyyy\/MM\/dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@venueid", tclass.VenueId);
                        command.Parameters.AddWithValue("@townid", tclass.TownId);
                        command.Parameters.AddWithValue("@activityid", tclass.ActivityId);
                        command.Parameters.AddWithValue("@speakerid", tclass.SpeakerId);
                        command.Parameters.AddWithValue("@title", tclass.Title);
                        command.Parameters.AddWithValue("@themeid", tclass.ThemeId);
                        command.Parameters.AddWithValue("@text", tclass.KeyText);
                        command.Parameters.AddWithValue("@hymn", tclass.Hymn);
                        command.Parameters.AddWithValue("@content", tclass.Content);

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
        public Sermon Select(int id)
        {
            try
            {
                Sermon sermon = new Sermon();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT SERMONS.Id AS sermonid,SERMONS.DateCreated AS datecreated,SERMONS.Title AS title,SERMONS.Text AS text,SERMONS.Hymn AS hymn,SERMONS.Content AS content,SERIES.Id AS seriesid,VENUES.Name AS venue,TOWNS.Name AS town,ACTIVITIES.Name AS activity,SPEAKERS.Name AS speaker, THEMES.Name AS theme FROM SERMONS JOIN SERIES ON SERMONS.Series=SERIES.Id JOIN VENUES ON SERMONS.Venue=VENUES.Id JOIN TOWNS ON SERMONS.Town=TOWNS.Id JOIN ACTIVITIES ON SERMONS.Activity=ACTIVITIES.Id JOIN SPEAKERS ON SERMONS.Speaker=SPEAKERS.Id JOIN THEMES ON SERMONS.Theme=THEMES.Id WHERE SERMONS.Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sermon.Id = int.Parse(reader["sermonid"].ToString());
                                sermon.SeriesId = int.Parse(reader["seriesid"].ToString());
                                sermon.DateCreated = DateTime.Parse(reader["datecreated"].ToString());
                                sermon.Title = reader["title"].ToString();
                                sermon.Theme = reader["theme"].ToString();
                                sermon.KeyText = reader["text"].ToString();
                                sermon.Hymn = reader["hymn"].ToString();
                                sermon.Content = reader["content"].ToString();
                                sermon.Activity = reader["activity"].ToString();
                                sermon.Speaker = reader["speaker"].ToString();
                                sermon.Town = reader["town"].ToString();
                                sermon.Venue = reader["venue"].ToString();
                            }
                        }
                    }
                }
                return sermon;
            }
            catch
            {
                return null;
            }
        }
        public Sermon Select(string name)
        {
            throw new NotImplementedException();
        }
        public List<Sermon> SelectAll()
        {
            try
            {
                List<Sermon> list = new List<Sermon>();
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT SERMONS.Id AS sermonid,SERMONS.DateCreated AS datecreated,SERMONS.Title AS title,SERMONS.Text AS text,SERMONS.Hymn AS hymn,SERMONS.Content AS content,SERIES.Id AS seriesid,VENUES.Name AS venue,TOWNS.Name AS town,ACTIVITIES.Name AS activity,SPEAKERS.Name AS speaker, THEMES.Name AS theme FROM SERMONS JOIN SERIES ON SERMONS.Series=SERIES.Id JOIN VENUES ON SERMONS.Venue=VENUES.Id JOIN TOWNS ON SERMONS.Town=TOWNS.Id JOIN ACTIVITIES ON SERMONS.Activity=ACTIVITIES.Id JOIN SPEAKERS ON SERMONS.Speaker=SPEAKERS.Id JOIN THEMES ON SERMONS.Theme=THEMES.Id", connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Sermon sermon = new Sermon()
                                {
                                    Id = int.Parse(reader["sermonid"].ToString()),
                                    SeriesId = int.Parse(reader["seriesid"].ToString()),
                                    DateCreated = DateTime.Parse(reader["datecreated"].ToString()),
                                    Title = reader["title"].ToString(),
                                    Theme = reader["theme"].ToString(),
                                    KeyText = reader["text"].ToString(),
                                    Hymn = reader["hymn"].ToString(),
                                    Content = reader["content"].ToString(),
                                    Activity = reader["activity"].ToString(),
                                    Speaker = reader["speaker"].ToString(),
                                    Town = reader["town"].ToString(),
                                    Venue = reader["venue"].ToString()
                                };
                                list.Add(sermon);
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
        public List<Sermon> SelectMany(string column, string value)
        {
            List<Sermon> list = new List<Sermon>();
            using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
            {
                connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT SERMONS.Id AS sermonid,SERMONS.DateCreated AS datecreated,SERMONS.Title AS title,SERMONS.Text AS text,SERMONS.Hymn AS hymn,SERMONS.Content AS content,SERIES.Id AS seriesid,VENUES.Name AS venue,TOWNS.Name AS town,ACTIVITIES.Name AS activity,SPEAKERS.Name AS speaker, THEMES.Name AS theme FROM SERMONS JOIN SERIES ON SERMONS.Series=SERIES.Id JOIN VENUES ON SERMONS.Venue=VENUES.Id JOIN TOWNS ON SERMONS.Town=TOWNS.Id JOIN ACTIVITIES ON SERMONS.Activity=ACTIVITIES.Id JOIN SPEAKERS ON SERMONS.Speaker=SPEAKERS.Id JOIN THEMES ON SERMONS.Theme=THEMES.Id WHERE " + column + "=@value", connection))
                {
                    command.Parameters.AddWithValue("@value", value);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sermon sermon = new Sermon()
                            {
                                Id = int.Parse(reader["sermonid"].ToString()),
                                SeriesId = int.Parse(reader["seriesid"].ToString()),
                                DateCreated = DateTime.Parse(reader["datecreated"].ToString()),
                                Title = reader["title"].ToString(),
                                Theme = reader["theme"].ToString(),
                                KeyText = reader["text"].ToString(),
                                Hymn = reader["hymn"].ToString(),
                                Content = reader["content"].ToString(),
                                Activity = reader["activity"].ToString(),
                                Speaker = reader["speaker"].ToString(),
                                Town = reader["town"].ToString(),
                                Venue = reader["venue"].ToString()
                            };
                            list.Add(sermon);
                        }
                    }
                }
            }
            return list;
        }
        public void SetObjectValues(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("SELECT SERMONS.Id AS sermonid,SERMONS.DateCreated AS datecreated,SERMONS.Title AS title,SERMONS.Text AS text,SERMONS.Hymn AS hymn,SERMONS.Content AS content,SERIES.Id AS seriesid,VENUES.Name AS venue,TOWNS.Name AS town,ACTIVITIES.Name AS activity,SPEAKERS.Name AS speaker, THEMES.Name AS theme FROM SERMONS JOIN SERIES ON SERMONS.Series=SERIES.Id JOIN VENUES ON SERMONS.Venue=VENUES.Id JOIN TOWNS ON SERMONS.Town=TOWNS.Id JOIN ACTIVITIES ON SERMONS.Activity=ACTIVITIES.Id JOIN SPEAKERS ON SERMONS.Speaker=SPEAKERS.Id JOIN THEMES ON SERMONS.Theme=THEMES.Id WHERE SERMONS.Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Id = int.Parse(reader["sermonid"].ToString());
                                SeriesId = int.Parse(reader["seriesid"].ToString());
                                DateCreated = DateTime.Parse(reader["datecreated"].ToString());
                                Title = reader["title"].ToString();
                                Theme = reader["theme"].ToString();
                                KeyText = reader["text"].ToString();
                                Hymn = reader["hymn"].ToString();
                                Content = reader["content"].ToString();
                                Activity = reader["activity"].ToString();
                                Speaker = reader["speaker"].ToString();
                                Town = reader["town"].ToString();
                                Venue = reader["venue"].ToString();
                            }
                        }
                    }
                }
                Series = new Series(SeriesId).Name;
            }
            catch
            {

            }
        }
        public int Update(Sermon tclass)
        {
            try
            {
                int rowsAffected = 0;
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("UPDATE SERMONS SET Series=@seriesid,DateCreated=@datecreated,Venue=@venueid,Town=@townid,Activity=@activityid,Speaker=@speakerid,Title=@title,Theme=@themeid,Text=@text,Hymn=@hymn,Content=@content WHERE Id=@id", connection))
                    {
                        command.Parameters.AddWithValue("@id", tclass.Id);
                        command.Parameters.AddWithValue("@seriesid", tclass.SeriesId);
                        command.Parameters.AddWithValue("@datecreated", tclass.DateCreated.ToString(@"yyyy\/MM\/dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@venueid", tclass.VenueId);
                        command.Parameters.AddWithValue("@townid", tclass.TownId);
                        command.Parameters.AddWithValue("@activityid", tclass.ActivityId);
                        command.Parameters.AddWithValue("@speakerid", tclass.SpeakerId);
                        command.Parameters.AddWithValue("@title", tclass.Title);
                        command.Parameters.AddWithValue("@themeid", tclass.ThemeId);
                        command.Parameters.AddWithValue("@text", tclass.KeyText);
                        command.Parameters.AddWithValue("@hymn", tclass.Hymn);
                        command.Parameters.AddWithValue("@content", tclass.Content);

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
                    connection.Open(); using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `SERMONS` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `SERIES` INTEGER NOT NULL DEFAULT 0, `DateCreated` TEXT NOT NULL, `Venue` INTEGER NOT NULL DEFAULT 0, `Town` INTEGER NOT NULL DEFAULT 0, `Activity` INTEGER NOT NULL DEFAULT 0, `Speaker` INTEGER NOT NULL DEFAULT 0, `Title` TEXT NOT NULL, `Text` TEXT, `Hymn` TEXT, `Content` TEXT NOT NULL, FOREIGN KEY(`SERIES`) REFERENCES `SERIES`(`Id`), FOREIGN KEY(`Venue`) REFERENCES `VENUES`(`Id`), FOREIGN KEY(`Town`) REFERENCES `TOWNS`(`Id`), FOREIGN KEY(`Activity`) REFERENCES `ACTIVITIES`(`Id`), FOREIGN KEY(`Speaker`) REFERENCES `SPEAKERS`(`Id`) )", connection))
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