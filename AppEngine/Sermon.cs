using System;
using System.Collections.Generic;

namespace AppEngine
{
    public class Sermon
    {
        //static public string ID;
        //static public DateTime date_time;
        //static public Venue venue;
        public int Id;
        public int SeriesId;
        public DateTime DateCreated;
        public string Venue;
        public string Town;
        public string Activity;
        public string Speaker;
        public string KeyText;
        public string Hymn;
        public string Title;
        public string Theme;
        public string Content;
        //#region ****************** DESCRIPTIVE CONSTANTS (MACROS) ******************
        public const int iID = 0;
        public const int iSeries = 1;
        public const int iDateCreated = 2;
        public const int iVenue = 3;
        public const int iVenueTown = 4;
        public const int iVenueActivity = 5;
        public const int iSpeaker = 6;
        public const int iTitle = 7;
        public const int iTheme = 8;
        public const int iKeyText = 9;
        public const int iHymn = 10;
        public const int iContent = 11;
        //#endregion

        public Sermon(int id, int seriesid, DateTime dateCreated, string venue, string town, string activity, string speaker, string keytext, string hymn, string title, string theme, string content)
        {
            Id = id;
            SeriesId = seriesid;
            DateCreated = dateCreated;
            Venue = venue;
            Town = town;
            Activity = activity;
            Speaker = speaker;
            KeyText = keytext;
            Hymn = hymn;
            Title = title;
            Theme = theme;
            Content = content;
        }
        public Sermon()
        {

        }
        static public string[,] Search(string column,string searchString,bool considerCase,bool matchWhole)
        {
            try
            {
                List<string> foundItems_Speaker = new List<string>();
                List<string> foundItems_Title = new List<string>();
                List<string> foundItems_ID = new List<string>();

                Database.Sermon dummy = new Database.Sermon();
                List<Database.Sermon> sermons = dummy.SelectAll();

                if (!considerCase)//considerCase == false: do not match case
                {
                    if (matchWhole)
                    {
                        foreach(Database.Sermon sermon in sermons)
                        {
                            if (SermonSwitch(sermon, column).ToLower() == searchString.ToLower())
                            {
                                SearchGetValues(ref foundItems_ID, ref foundItems_Speaker, ref foundItems_Title, sermon);
                            }
                        }
                    }
                    else
                    {
                        foreach (Database.Sermon sermon in sermons)
                        {
                            if (StringSearch.AllExist_InOrder(SermonSwitch(sermon,column).ToLower(), searchString.ToLower()))
                            {
                                SearchGetValues(ref foundItems_ID, ref foundItems_Speaker, ref foundItems_Title, sermon);
                            }
                        }
                    }
                }
                else//considerCase == true: match case
                {
                    if (matchWhole)
                    {
                        foreach (Database.Sermon sermon in sermons)
                        {
                            if (SermonSwitch(sermon, column) == searchString)
                            {
                                SearchGetValues(ref foundItems_ID, ref foundItems_Speaker, ref foundItems_Title, sermon);
                            }
                        }
                    }
                    else
                    {
                        foreach (Database.Sermon sermon in sermons)
                        {
                            if (StringSearch.AllExist_InOrder(SermonSwitch(sermon, column), searchString))
                            {
                                SearchGetValues(ref foundItems_ID, ref foundItems_Speaker, ref foundItems_Title, sermon);
                            }
                        }
                    }
                }
                string[,] foundItems = new string[3, foundItems_Title.Count];
                for (int i = 0; i < foundItems_ID.Count; i++)
                {
                    foundItems[0, i] = foundItems_Title[i];
                    foundItems[1, i] = foundItems_Speaker[i];
                    foundItems[2, i] = foundItems_ID[i];
                }
                return foundItems;
            }
            catch
            {
                return null;
            }
        }
        static private void SearchGetValues(ref List<string> foundIDs, ref List<string> foundSpeakers, ref List<string> foundTitles, Database.Sermon sermon)
        {
            try
            {
                foundIDs.Add(sermon.Id.ToString());
                foundSpeakers.Add(sermon.Speaker);
                foundTitles.Add(sermon.Title);
            }
            catch
            {
                //the property is null maybe???
            }
        }
        static public void CreateSermonTable()
        {
            Database.Sermon.CreateTable();
        }
        static public string[] ComponentsToString(Sermon sermon)
        {
            try
            {
                string[] arraySermonComponents = { sermon.Id.ToString(), sermon.SeriesId.ToString(), sermon.DateCreated.ToString(), sermon.Venue, sermon.Town, sermon.Activity, sermon.Speaker, sermon.Title, sermon.Theme, sermon.KeyText, sermon.Hymn, sermon.Content };
                return arraySermonComponents;
            }
            catch
            {
                return null;
            }
        }
        static public string GetSermonComponent(int id, string column)
        {
            string value = string.Empty;
            Database.Sermon sermon = new Database.Sermon(id);
            value = SermonSwitch(sermon, column);
            return value;
        }
        static public string[] GetSermonComponents(int id)
        {
            Database.Sermon dummy = new Database.Sermon(id);
            Sermon sermon = new Sermon(dummy.Id, dummy.SeriesId, dummy.DateCreated, dummy.Venue, dummy.Town, dummy.Activity, dummy.Speaker, dummy.KeyText, dummy.Hymn, dummy.Title, dummy.Theme, dummy.Content);
            return ComponentsToString(sermon);
        }
        static public void DeleteSermon(int id)
        {
            try
            {
                Database.Sermon sermon = new Database.Sermon(id);
                sermon.Delete(sermon);
                RecentlyOpenedDocs.DeleteSermonFromID(id);
            }
            catch {; }
        }
        static public int GetSermonCount()
        {
            return new Database.Sermon().Count;
        }
        static private string SermonSwitch(Database.Sermon sermon, string column)
        {
            string value = string.Empty;
            switch (column.ToUpper())
            {
                case "ACTIVITY":
                    value = sermon.Activity;
                    break;
                case "CONTENT":
                    value = sermon.Content;
                    break;
                case "YEAR":
                    value = sermon.DateCreated.Year.ToString();
                    break;
                case "HYMN":
                    value = sermon.Hymn;
                    break;
                case "KEYTEXT":
                    value = sermon.KeyText;
                    break;
                case "SERIES":
                    value = sermon.Series;
                    break;
                case "SPEAKER":
                    value = sermon.Speaker;
                    break;
                case "TITLE":
                    value = sermon.Title;
                    break;
                case "THEME":
                    value = sermon.Theme;
                    break;
                case "TOWN":
                    value = sermon.Town;
                    break;
                case "VENUE":
                    value = sermon.Venue;
                    break;
                default:
                    value = String.Empty;
                    break;
            }
            return value;
        }
        static public string[] GetParentNodes(string filterColumn, List<Database.Sermon> list)
        {
            string[] parentNodes = new string[list.Count];
            int index = 0;
            foreach (Database.Sermon sermon in list)
            {
                parentNodes[index] = SermonSwitch(sermon, filterColumn);
                index++;
            }
            return parentNodes;
        }
        static public string[,] GetChildNodes(string filterColumn, string filterValue, List<Database.Sermon> list)
        {
            int index = 0;

            string[,] childNodes = new string[2, list.Count];
            foreach (var sermon in list)
            {
                if (SermonSwitch(sermon, filterColumn) == filterValue)
                {
                    childNodes[0, index] = sermon.Title;
                    childNodes[1, index] = sermon.Id.ToString();
                    index++;
                }
            }
            return childNodes;
        }
        static public int WriteSermon(string[] arraySermonComponents)
        {
            Database.Sermon sermon = new Database.Sermon()
            {
                Activity = arraySermonComponents[iVenueActivity],
                Content = arraySermonComponents[iContent],
                DateCreated = DateTime.Parse(arraySermonComponents[iDateCreated]),
                Hymn = arraySermonComponents[iHymn],
                KeyText = arraySermonComponents[iKeyText],
                SeriesId = int.Parse(arraySermonComponents[iSeries]),
                Speaker = arraySermonComponents[iSpeaker],
                Theme = arraySermonComponents[iTheme],
                Title = arraySermonComponents[iTitle],
                Town = arraySermonComponents[iVenueTown],
                Venue = arraySermonComponents[iVenue]
            };
            return sermon.Insert(sermon);
        }
        static public void OverwriteSermon(string[] arraySermonComponents)
        {
            Database.Sermon sermon = new Database.Sermon()
            {
                Activity = arraySermonComponents[iVenueActivity],
                Content = arraySermonComponents[iContent],
                DateCreated = DateTime.Parse(arraySermonComponents[iDateCreated]),
                Hymn = arraySermonComponents[iHymn],
                Id = int.Parse(arraySermonComponents[iID]),
                KeyText = arraySermonComponents[iKeyText],
                SeriesId = int.Parse(arraySermonComponents[iSeries]),
                Speaker = arraySermonComponents[iSpeaker],
                Theme = arraySermonComponents[iTheme],
                Title = arraySermonComponents[iTitle],
                Town = arraySermonComponents[iVenueTown],
                Venue = arraySermonComponents[iVenue]
            };
            sermon.Update(sermon);
        }
    }
}