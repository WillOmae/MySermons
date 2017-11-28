using System.Collections.Generic;

/* This class deals with the recently opened documents treeview of the start page
 * When a document is opened during the app's runtime, it should be recorded as a recently opened document
 * It can therefore be viewed on the startpage when the app is run later
 * 
 * This means that critical properties of the document should be noted to enable the reopening of the document later on
 * Further, different types of documents are viewed differently. Therefore, it is important to know the type of document
 * The treeview nodes will bear the title of the document. Therefore, this should also be noted
 * 
 * This information is held in an XML file; consequently, a method to correctly store this info is implemented
 * 
 * The XML file thus created will be read when the parent form (consequently, the startpage) loads, in order to display the recently opened documents treeview nodes; consequently, a method to implement this is required
 * 
 * Certain issues arise when implementing this class:
 * 1. When the same document is opened more than once in the course of the app's runtime, it is bound to be recorded more than once; consequently, a means of preventing multiple recording of the same opened document should be implemented
 * 2. The number of recently opened documents should be checked i.e. there should not be more than X number of recently opened documents
 */

namespace AppEngine
{
    public class RecentlyOpenedDocs
    {
        public static int MaxNumber
        {
            get
            {
                return Preferences.ROD_MaxNumber;
            }
        }

        public RecentlyOpenedDocs()
        {
            //Ensure entries still exist in sermons table
            List<Database.RODs> rods = new Database.RODs().SelectAll();
            List<Database.Sermon> sermons = new Database.Sermon().SelectAll();

            bool found = false;
            for (int i = rods.Count - 1; i >= 0; i--)
            {
                found = false;
                for (int j = 0; j < sermons.Count; j++)
                {
                    if ((sermons[j].Id == rods[i].Id) && (sermons[j].Title == rods[i].Title))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Database.RODs rod = new Database.RODs(rods[i].Id);
                    rod.Delete(rod);
                }
            }
        }
        static public void AddNewNode(int id,string title)
        {
            Database.RODs rods = new Database.RODs()
            {
                Id = id,
                Title = title,
            };
            if (rods.Exists(rods))
            {
                rods.Delete(rods);
            }
            rods.Insert(rods);
        }
        static public string[] OpenSermonFromID(int id)
        {
            return Sermon.GetSermonComponents(id);
        }
        static public void DeleteSermonFromID(int id)
        {
            Database.RODs rods = new Database.RODs(id);
            rods.Delete(rods);
        }
        static public void CreateNewRODTable()
        {
            //CREATE TABLE `RODs` (`DocId`	INTEGER NOT NULL,`DocTitle`	TEXT, PRIMARY KEY(`DocId`), FOREIGN KEY(`DocId`)REFERENCES Sermons(Id),FOREIGN KEY(`DocTitle`) REFERENCES Sermons(Title))
            
        }
    }
}