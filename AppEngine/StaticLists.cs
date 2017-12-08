using System.Collections.Generic;

namespace AppEngine
{
    public class StaticLists
    {
        public static List<Database.Venue> listOfVenues;
        public static List<Database.Town> listOfTowns;
        public static List<Database.Activity> listOfActivities;
        public static List<Database.Speaker> listOfSpeakers;
        public static List<Database.Theme> listOfThemes;
        public static List<Database.Series> listOfSeries;

        public StaticLists()
        {
            listOfVenues = new Database.Venue().SelectAll();
            listOfTowns = new Database.Town().SelectAll();
            listOfActivities = new Database.Activity().SelectAll();
            listOfSpeakers = new Database.Speaker().SelectAll();
            listOfThemes = new Database.Theme().SelectAll();
            listOfSeries = new Database.Series().SelectAll();

            listOfVenues.TrimExcess();
            listOfTowns.TrimExcess();
            listOfActivities.TrimExcess();
            listOfSpeakers.TrimExcess();
            listOfThemes.TrimExcess();
            listOfSeries.TrimExcess();
        }
    }
}
