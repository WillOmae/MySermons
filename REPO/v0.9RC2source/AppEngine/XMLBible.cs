using System;
using System.Collections.Generic;
using System.Xml;

namespace AppEngine
{
    public class XMLBible
    {
        #region GLOBAL VARIABLES
        /// <summary>
        /// Structure holding information for a single verse.
        /// </summary>
        public struct BCVSTRUCT
        {
            public string Book;
            public string Chapter;
            public string Verse;
            public string bcv;
        }
        /// <summary>
        /// Structure holding information for a range of verses.
        /// </summary>
        public struct BCVRANGE
        {
            public BCVSTRUCT Start;
            public BCVSTRUCT End;
            public string BCV;
        }
        /// <summary>
        /// Structure holding bcv and verse details of a BCVRange struct.
        /// </summary>
        public struct BIBLETEXTINFO
        {
            public string FriendlyText; //What is actually displayed e.g. Hebrews11:6
            public string bcv;          //What is used internally e.g. HEB.11.6
            public string verse;        //The content of the passage under consideration
        }
        /// <summary>
        /// List of the BibleTextInfo Structures for all the BCVRange structures parsed from the stringToParse.
        /// </summary>
        public List<BIBLETEXTINFO> listOfBibleTextInfo = new List<BIBLETEXTINFO>();

        static public XmlDocument XMLDocument_Bible
        {
            get
            {
                if (xmldocumentbible == null)
                {
                    xmldocumentbible = new XmlDocument();
                    xmldocumentbible.Load(FileNames.Bible);
                }
                return xmldocumentbible;
            }
        }
        static private XmlDocument xmldocumentbible = null;
        static private XmlNode KJVBibleNode
        {
            get
            {
                return XMLDocument_Bible.DocumentElement.ChildNodes[1];
            }
        }
        static private XmlNode BibleBookNames
        {
            get
            {
                return XMLDocument_Bible.DocumentElement.ChildNodes[0];
            }
        }

        private char blockSeparator = ';', inblockSeparator = ',', rangeSeparator = '-', cvSeparator = ':';

        #endregion
        #region CONSTRUCTOR
        /// <summary>
        ///     Initialises a new instance of the XMLBible class.
        /// </summary>
        /// <param name="stringToParse">String passed to be parsed.</param>
        public XMLBible(string stringToParse)
        {
            listOfBibleTextInfo.Clear();

            if (!string.IsNullOrEmpty(stringToParse) && !string.IsNullOrWhiteSpace(stringToParse))
            {
                List<BCVRANGE> listOfBCVRanges = new List<BCVRANGE>();
                ParseString(stringToParse, ref listOfBCVRanges);

                for (int i = 0; i < listOfBCVRanges.Count; i++)
                {
                    try
                    {
                        BCVSTRUCT START = listOfBCVRanges[i].Start;
                        BCVSTRUCT END = listOfBCVRanges[i].End;
                        //string VERSE = GetVerseText(ref START, ref END);
                        BIBLETEXTINFO bibleTextInfo = new BIBLETEXTINFO();
                        //bibleTextInfo.verse = VERSE;
                        bibleTextInfo.verse = "";

                        //Update bcv to be displayed
                        if (END.Book == null)//no range: show single bcv
                        {
                            bibleTextInfo.bcv = START.bcv;
                            listOfBibleTextInfo.Add(bibleTextInfo);
                        }
                        else//range: show start and end bcv's
                        {
                            bibleTextInfo.bcv = START.bcv + rangeSeparator + END.bcv;
                            listOfBibleTextInfo.Add(bibleTextInfo);
                        }
                        //end of update
                    }
                    catch {; }
                }
                return;
            }
            else {; }
        }
        #endregion
        #region FUNCTIONS OF THE CLASS

/// <summary>
        ///     Carries out the first step of parsing, by separating the string into blocks.
        ///     It makes use of the block separator. Each block is then passed to the second parsing function.
        /// <para>For example:</para>
        /// <para>
        ///     HEBREWS11:1-6,12;REVELATION22:19-21 
        ///     becomes two blocks:
        ///     HEBREWS11:1-6,12 and REVELATION22:19-21
        /// </para>
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <param name="listOfBCVRanges">The list to be updated passed by reference.</param>
        private void ParseString(string stringToParse, ref List<BCVRANGE> listOfBCVRanges)
        {
            //ensure uniformity in parsing all strings, add blockseparator to the end of the string
            //the blockSeparator must exist in all strings
            stringToParse = stringToParse.Insert(stringToParse.Length, blockSeparator.ToString());

            //create a string variable to hold one block to be passed to the next parsing function
            string stringToPass = "";

            for (int i = 0; i < stringToParse.Length; i++)
            {
                if (stringToParse[i] == blockSeparator)
                {
                    if (stringToPass.Length > 0)
                    {
                        ParseString1(stringToPass, ref listOfBCVRanges);
                    }
                    stringToPass = "";//clear stringToPass to accommodate next string
                }
                else
                {
                    stringToPass += stringToParse[i];
                }
            }

            return;
        }
        /// <summary>
        ///     Carries out the second step of parsing, by separating the string into sub-blocks.
        ///     It makes use of the inblock separator. Each sub-block is first passed to a subsidiary function to properly parse it,
        ///     then passed to the third parsing function.
        /// <para>For example:</para>
        /// <para>
        ///     HEBREWS11:1-6,12 becomes: HEBREWS11:1-6 and 12
        /// </para>
        /// </summary>
        /// <param name="stringToParse">The string to parse.</param>
        /// <param name="listOfBCVRanges">The list to be updated passed by reference.</param>
        private void ParseString1(string stringToParse, ref List<BCVRANGE> listOfBCVRanges)
        {
            //ensure uniformity in parsing all strings: inblockSeparator must exist in all strings
            stringToParse = stringToParse.Insert(stringToParse.Length, inblockSeparator.ToString());

            BCVSTRUCT bibleTextCurrent = new BCVSTRUCT();
            bibleTextCurrent.Book = null;

            bool addedVerse = false;
            string stringToPass = "";
            for (int i = 0; i < stringToParse.Length; i++)
            {
                if (stringToParse[i] == inblockSeparator)
                {
                    if (stringToPass.Length > 0)
                    {
                        if (bibleTextCurrent.Book != null)
                        {
                            if (bibleTextCurrent.Book.Length > 1)
                            {
                                stringToPass = SetStringForInblockSeparator(bibleTextCurrent, stringToPass, addedVerse);
                            }
                        }
                        bibleTextCurrent = ParseString2(stringToPass, ref listOfBCVRanges, out addedVerse);
                    }
                    stringToPass = "";//clear stringToPass to accommodate next string
                }
                else
                {
                    stringToPass += stringToParse[i];
                }
            }
        }
        /// <summary>
        ///     A subsidiary parsing function to the inblock separator function.
        ///     It checks the string passed and correctly supplies missing information such as BOOK name and/or CHAPTER.
        /// <para>For example:</para>
        /// <para>
        ///     From 12 in HEBREWS11:1-6 and 12 separated in the calling function,
        ///     the function returns HEBREWS11:12 for the inblock separated string "12".
        /// </para>
        /// </summary>
        /// <param name="BCVCurrent">
        ///     BCVstruct containing details of the most previous verse handled.
        ///     It gives information for the sub-block to be parsed.</param>
        /// <param name="stringToParse"></param>
        /// <returns></returns>
        private string SetStringForInblockSeparator(BCVSTRUCT BCVCurrent, string stringToParse, bool addedVerse)
        {
            string returnString = "";
            BCVSTRUCT BCVNew = new BCVSTRUCT();

            BCVNew.Book = BCVCurrent.Book;

            if (stringToParse.Contains(cvSeparator.ToString()))//only the book is shared; it contains its own chapter and verse
            {
                BCVNew.Chapter = stringToParse.Remove(stringToParse.IndexOf(cvSeparator));
                BCVNew.Verse = stringToParse.Remove(0, stringToParse.IndexOf(cvSeparator) + 1);
                returnString = BCVNew.Book + BCVNew.Chapter + cvSeparator + BCVNew.Verse;
            }
            else
            {
                if (BCVCurrent.Verse == null || BCVCurrent.Verse.Length < 1)//only chapter is recorded, so the string represents a chapter as well
                {
                    BCVNew.Chapter = stringToParse;
                    returnString = BCVNew.Book + BCVNew.Chapter;
                }
                else
                {
                    if (addedVerse)//only the book is shared e.g. PSA140,141
                    {
                        BCVNew.Chapter = stringToParse;
                        returnString = BCVNew.Book + BCVNew.Chapter;
                    }
                    else//both book and chapter are shared e.g. HEB11:1,6
                    {
                        BCVNew.Chapter = BCVCurrent.Chapter;
                        BCVNew.Verse = stringToParse;
                        returnString = BCVNew.Book + BCVNew.Chapter + cvSeparator + BCVNew.Verse;
                    }
                }
            }
            return returnString;
        }
        private BCVSTRUCT ParseString2(string stringToParse, ref List<BCVRANGE> listOfBCVRanges, out bool addedVerse)
        {
            BCVSTRUCT bibleText_Start = new BCVSTRUCT();
            bibleText_Start.bcv = null;
            bibleText_Start.Book = null;
            bibleText_Start.Chapter = null;
            bibleText_Start.Verse = null;
            BCVSTRUCT bibleText_End = new BCVSTRUCT();
            bibleText_End.bcv = null;
            bibleText_End.Book = null;
            bibleText_End.Chapter = null;
            bibleText_End.Verse = null;

            int index = 0;
            bool foundChapter = false, foundCVSeparator = false;
            foreach (char c in stringToParse)
            {
                if (char.IsDigit(c) == true && index == 0)
                {
                    bibleText_Start.Book += c;
                }
                else if (char.IsLetter(c) == true)
                {
                    bibleText_Start.Book += c;
                }
                else if (char.IsDigit(c) == true && foundChapter == false)
                {
                    bibleText_Start.Chapter += c;
                }
                else if (char.IsPunctuation(c) == true && c == cvSeparator)
                {
                    foundChapter = true;
                    foundCVSeparator = true;
                }
                else if (char.IsDigit(c) == true)
                {
                    bibleText_Start.Verse += c;
                }
                else if (char.IsPunctuation(c) == true && c == rangeSeparator)//get the end BCVstruct
                {//sample ~Heb13-Jas1~
                    if (foundCVSeparator)
                    {
                        GetEnd(stringToParse.Remove(0, stringToParse.IndexOf(c) + 1), bibleText_Start, ref bibleText_End);
                    }
                    else
                    {
                        bibleText_Start.Verse = "1";
                        string pass = stringToParse.Remove(0, stringToParse.IndexOf(c) + 1);
                        int firstDigitIndex = 0;
                        for (int i = 0; i < pass.Length; i++)
                        {
                            char x = pass[i];
                            if (char.IsDigit(x))
                            {
                                try
                                {
                                    if (!char.IsLetter(pass[i + 1]))
                                    {
                                        firstDigitIndex = pass.IndexOf(x);
                                        break;
                                    }
                                }
                                catch
                                {
                                    firstDigitIndex = pass.IndexOf(x);
                                }
                            }
                        }
                        string book = pass.Remove(firstDigitIndex), chapter = pass.Remove(0, firstDigitIndex);
                        book = ConfirmBibleBookName(book);
                        pass = book + chapter + ":" + VerseCount(book, chapter);

                        GetEnd(pass, bibleText_Start, ref bibleText_End);
                    }
                    break;
                }
                index++;
            }

            if (bibleText_Start.Book != null)
            {
                //Confirm the existence of this book
                bibleText_Start.Book = ConfirmBibleBookName(bibleText_Start.Book);
                //Reconfirm the existence of this book
                if (bibleText_Start.Book != null)
                {
                    //No verse stated, no range given e.g. ~james1~
                    if (bibleText_Start.Verse == null && bibleText_End.Book == null)
                    {
                        bibleText_Start.Verse = "1";
                        bibleText_End.Book = bibleText_Start.Book;
                        bibleText_End.Chapter = bibleText_Start.Chapter;
                        bibleText_End.Verse = VerseCount(bibleText_End.Book, bibleText_End.Chapter).ToString();
                        addedVerse = true;//***********NOTE
                    }
                    else
                    {
                        addedVerse = false;//***********NOTE
                    }
                    bibleText_Start.bcv = bibleText_Start.Book.ToUpper() + "." + bibleText_Start.Chapter + "." + bibleText_Start.Verse;//generate bcv
                }
                else
                {
                    addedVerse = false;
                }
            }
            else
            {
                bibleText_Start.Book = null;
                bibleText_Start.bcv = null;
                addedVerse = false;//***********NOTE
            }
            if (bibleText_End.Book != null)
            {
                bibleText_End.Book = ConfirmBibleBookName(bibleText_End.Book);//Confirm the existence of this book
                if (bibleText_End.Book != null)
                {
                    bibleText_End.bcv = bibleText_End.Book.ToUpper() + "." + bibleText_End.Chapter + "." + bibleText_End.Verse;//generate bcv
                }
            }
            else
            {
                bibleText_End.Book = null;
                bibleText_End.bcv = null;
            }
            BCVRANGE BTG = new BCVRANGE();
            BTG.Start = bibleText_Start;
            BTG.End = bibleText_End;
            listOfBCVRanges.Add(BTG);

            if (bibleText_End.Book != null)
            {
                return bibleText_End;
            }
            else
            {
                return bibleText_Start;
            }
        }
        private void GetEnd(string stringToParse, BCVSTRUCT bibleText_Start, ref BCVSTRUCT bibleText_End)
        {
            bool chapterFound = false;

            try
            {
                if (char.IsDigit(stringToParse[0]) && char.IsLetter(stringToParse[1]))//e.g 2John1:1-3John1:1
                {
                    bibleText_End.Book += stringToParse[0];
                }
            }
            catch { }
            foreach (char c in stringToParse)
            {
                if (char.IsLetter(c))//e.g Hebrews11:1-Hebrews12:1
                {
                    bibleText_End.Book += c;
                    stringToParse = stringToParse.Remove(0, stringToParse.IndexOf(c) + 1);
                }
            }
            
            if (bibleText_End.Book == null)//e.g Hebrews11:1-12:1 End.Book should be Hebrews as well
            {
                bibleText_End.Book = bibleText_Start.Book;
            }
            
            foreach (char c in stringToParse)
            {
                if (char.IsPunctuation(c) && c == ':')//e.g Hebrews11:1-12:1
                {
                    chapterFound = true;
                }
            }
            if (chapterFound)//e.g Hebrews11:1-12:1 chapter is 12
            {
                bibleText_End.Chapter = stringToParse.Remove(stringToParse.IndexOf(':'));
                stringToParse = stringToParse.Remove(0, stringToParse.IndexOf(':') + 1);
            }
            else//e.g Hebrews11:1-12 or //e.g Hebrews11-12
            {
                if (bibleText_Start.Verse == null)//e.g Hebrews11-12
                {
                    foreach (char c in stringToParse)
                    {
                        bibleText_End.Chapter += c;
                        stringToParse = stringToParse.Remove(0, stringToParse.IndexOf(c) + 1);
                    }
                }
                else//e.g Hebrews11:1-12 chapter should be 11 as well i.e Hebrews 11:12 for End
                {
                    bibleText_End.Chapter = bibleText_Start.Chapter;
                }
            }
            if (stringToParse != null && stringToParse.Length > 0)
            {
                foreach (char c in stringToParse)
                {
                    if (bibleText_Start.Verse == null)
                    {
                        bibleText_End.Chapter += c;
                    }
                    else
                    {
                        bibleText_End.Verse += c;
                    }
                }
            }
        }
        /// <summary>
        /// Assertains the presence of the Bible book in the search string passed to it by comparing it to the file containing Bible books.
        /// </summary>
        /// <param name="search">String containing the book to be confirmed.</param>
        /// <returns></returns>
        private string ConfirmBibleBookName(string search)
        {
            string szBookName = null;

            try
            {

                /* Best search:
                 * Entire search string occurs as it is i.e.
                    searchedString == searchString
                 */
                foreach (XmlElement XMLElement in BibleBookNames.ChildNodes)
                {
                    if (XMLElement.Attributes["abbr"].Value.ToLower() == search.ToLower())
                    {
                        szBookName = XMLElement.Attributes["c"].Value;
                        break;
                    }
                    else if (XMLElement.Attributes["short"].Value.ToLower() == search.ToLower())
                    {
                        szBookName = XMLElement.Attributes["c"].Value;
                        break;
                    }
                }
                /* 2nd best search:
                 * First letter match
                 * searchedString contains the searchString in order i.e.
                    searchedString == searchStringxyzabc
                 */
                if (szBookName == null)
                {
                    foreach (XmlElement XMLElement in BibleBookNames.ChildNodes)
                    {
                        if (StringSearch.AllExist_InOrder(XMLElement.Attributes["abbr"].Value.ToLower(), search.ToLower())
                            && XMLElement.Attributes["abbr"].Value.ToLower()[0] == search.ToLower()[0])
                        {
                            szBookName = XMLElement.Attributes["c"].Value;
                            break;
                        }
                        else if (StringSearch.AllExist_InOrder(XMLElement.Attributes["short"].Value.ToLower(), search.ToLower())
                            && XMLElement.Attributes["short"].Value.ToLower()[0] == search.ToLower()[0])
                        {
                            szBookName = XMLElement.Attributes["c"].Value;
                            break;
                        }
                    }
                }
                /* 3rd best search:
                 * searchedString contains the searchString in order i.e.
                    searchedString == abcsearchStringxyz
                 */
                if (szBookName == null)
                {
                    foreach (XmlElement XMLElement in BibleBookNames.ChildNodes)
                    {
                        if (StringSearch.AllExist_InOrder(XMLElement.Attributes["abbr"].Value.ToLower(), search.ToLower()))
                        {
                            szBookName = XMLElement.Attributes["c"].Value;
                            break;
                        }
                        else if (StringSearch.AllExist_InOrder(XMLElement.Attributes["short"].Value.ToLower(), search.ToLower()))
                        {
                            szBookName = XMLElement.Attributes["c"].Value;
                            break;
                        }
                    }
                }
                /* Worst search
                 * searchedString contains the searchString in whichever order of occurrence of letters
                 */
                if (szBookName == null)
                {
                    foreach (XmlElement XMLElement in BibleBookNames.ChildNodes)
                    {
                        if (StringSearch.AllExist(XMLElement.Attributes["abbr"].Value.ToLower(), search.ToLower()))
                        {
                            szBookName = XMLElement.Attributes["c"].Value;
                            break;
                        }
                        else if (StringSearch.AllExist(XMLElement.Attributes["short"].Value.ToLower(), search.ToLower()))
                        {
                            szBookName = XMLElement.Attributes["c"].Value;
                            break;
                        }
                    }
                }
            }
            catch{ }
            return szBookName;//if book was not found, null is returned.
        }
        /// <summary>
        /// Gets verse text for the given range.
        /// </summary>
        /// <param name="start">The beginning of the range.</param>
        /// <param name="end">The end of the range (values optional).</param>
        /// <returns></returns>
        public static string GetVerseText(ref BCVSTRUCT start, ref BCVSTRUCT end)
        {
            string verses = "";

            if (end.Book == null)//No range; get single verse
            {
                try
                {
                    foreach (XmlElement BOOK in KJVBibleNode.ChildNodes)
                    {
                        if (BOOK.Attributes["NAME"].Value == start.Book)
                        {
                            foreach (XmlElement CHAPTER in BOOK.ChildNodes)
                            {
                                if (CHAPTER.Attributes["NAME"].Value == start.Chapter)
                                {
                                    foreach (XmlElement VERSE in CHAPTER.ChildNodes)
                                    {
                                        if (VERSE.Attributes["BCV"].Value == start.bcv)
                                        {
                                            return (VERSE.Attributes["BCV"].Value + " " + VERSE.InnerText);
                                        }
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    return null;
                }
                catch
                {
                    return null;//complete
                }
            }
            else//There is a range i.e. both start and end are known. Need to determine which type of range
            {
                if (start.Verse == null)//Starting verse not given: start from beginning of the chapter
                {
                    start.Verse = "1";
                    start.bcv = start.Book.ToUpper() + "." + start.Chapter + "." + start.Verse;//update bcv to include verse
                }
                if (end.Verse == null)//Ending verse not given: read until the end of the chapter
                {
                    foreach (XmlElement BOOKsample in KJVBibleNode.ChildNodes)
                    {
                        if (BOOKsample.Attributes["NAME"].Value == end.Book)
                        {
                            foreach (XmlElement CHAPTERsample in BOOKsample.ChildNodes)
                            {
                                if (CHAPTERsample.Attributes["NAME"].Value == start.Chapter)
                                {
                                    end.Verse = CHAPTERsample.ChildNodes.Count.ToString();
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    end.bcv = end.Book.ToUpper() + "." + end.Chapter + "." + end.Verse;//update bcv to include verse
                }
                
                int chapterStart = Convert.ToInt32(start.Chapter);
                int chapterEnd = Convert.ToInt32(end.Chapter);
                int verseStart = Convert.ToInt32(start.Verse);
                int verseEnd = Convert.ToInt32(end.Verse);
                int verseCount = 1;
                int numberofVerses = 0;

                XmlNode startBOOK, endBOOK, BOOK;
                BOOK = XMLDocument_Bible.CreateElement("GEN");
                startBOOK = XMLDocument_Bible.CreateElement("GEN");
                endBOOK = XMLDocument_Bible.CreateElement("REV");

                foreach (XmlElement BOOKsample in KJVBibleNode.ChildNodes)
                {
                    if (BOOKsample.Attributes["NAME"].Value == start.Book)
                    {
                        startBOOK = BOOKsample;
                        BOOK = startBOOK;
                    }
                    if (BOOKsample.Attributes["NAME"].Value == end.Book)
                    {
                        endBOOK = BOOKsample;
                    }
                }

                while (true)//this is an infinite loop: please ensure it does not become an infinite loop
                {
                    if (BOOK != null)//prevent NULL exceptions
                    {
                        if (BOOK == startBOOK)
                        {
                            for (int chapterCount = chapterStart; chapterCount <= BOOK.ChildNodes.Count; chapterCount++)
                            {
                                numberofVerses = BOOK.ChildNodes[chapterCount - 1].ChildNodes.Count;

                                if (chapterCount == chapterStart)
                                {
                                    verseCount = verseStart;
                                }
                                else
                                {
                                    verseCount = 1;
                                }
                                for (; verseCount <= numberofVerses; verseCount++)
                                {
                                    if (start.Book == end.Book && chapterCount == chapterEnd && verseCount == verseEnd)
                                    {
                                        verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " ";
                                        verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText + Environment.NewLine;
                                        return verses;/*************///potential danger
                                    }
                                    verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " ";
                                    verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText + Environment.NewLine;
                                }
                            }
                        }
                        else if (BOOK == endBOOK && BOOK != startBOOK)//only read up to specified chapter and verse
                        {
                            for (int chapterCount = 1; chapterCount <= BOOK.ChildNodes.Count; chapterCount++)
                            {
                                numberofVerses = BOOK.ChildNodes[chapterCount - 1].ChildNodes.Count;

                                for (verseCount = 1; verseCount <= numberofVerses; verseCount++)
                                {
                                    if (chapterCount == chapterEnd && verseCount == verseEnd)
                                    {
                                        verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " ";
                                        verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText + Environment.NewLine;
                                        return verses;/*************///potential danger
                                    }
                                    verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " ";
                                    verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText + Environment.NewLine;
                                }
                            }
                            break;
                        }
                        else//read every chapter and verse unless it is the starting or ending BCVstruct
                        {
                            for (int chapterCount = 1; chapterCount <= BOOK.ChildNodes.Count; chapterCount++)
                            {
                                numberofVerses = BOOK.ChildNodes[chapterCount - 1].ChildNodes.Count;

                                for (verseCount = 1; verseCount <= numberofVerses; verseCount++)
                                {
                                    verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " ";
                                    verses += BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText + Environment.NewLine;
                                }
                            }
                        }
                        BOOK = BOOK.NextSibling;
                    }
                    else
                    {
                        break;
                    }
                }
                return verses;
            }
        }
        /// <summary>
        /// Gets verse text for the given range.
        /// </summary>
        /// <param name="start">The beginning of the range.</param>
        /// <param name="end">The end of the range (values optional).</param>
        /// <returns></returns>
        public static List<string> GetVerseText2(ref BCVSTRUCT start, ref BCVSTRUCT end)
        {
            List<string> listofVerses = new List<string>();
            try
            {
                if (end.Book == null)//No range; get single verse
                {
                    try
                    {
                        foreach (XmlElement BOOK in KJVBibleNode.ChildNodes)
                        {
                            if (BOOK.Attributes["NAME"].Value == start.Book)
                            {
                                foreach (XmlElement CHAPTER in BOOK.ChildNodes)
                                {
                                    if (CHAPTER.Attributes["ID"].Value == start.Chapter)
                                    {
                                        foreach (XmlElement VERSE in CHAPTER.ChildNodes)
                                        {
                                            if (VERSE.Attributes["BCV"].Value == start.bcv)
                                            {
                                                listofVerses.Add(VERSE.Attributes["BCV"].Value + " " + VERSE.InnerText);
                                                return listofVerses;
                                            }
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        return null;
                    }
                    catch
                    {
                        return null;//complete
                    }
                }
                else//There is a range i.e. both start and end are known. Need to determine which type of range
                {
                    if (start.Verse == null)//Starting verse not given: start from beginning of the chapter
                    {
                        start.Verse = "1";
                        start.bcv = start.Book.ToUpper() + "." + start.Chapter + "." + start.Verse;//update bcv to include verse
                    }
                    if (end.Verse == null)//Ending verse not given: read until the end of the chapter
                    {
                        foreach (XmlElement BOOKsample in KJVBibleNode.ChildNodes)
                        {
                            if (BOOKsample.Attributes["NAME"].Value == end.Book)
                            {
                                foreach (XmlElement CHAPTERsample in BOOKsample.ChildNodes)
                                {
                                    if (CHAPTERsample.Attributes["ID"].Value == start.Chapter)
                                    {
                                        end.Verse = CHAPTERsample.ChildNodes.Count.ToString();
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        end.bcv = end.Book.ToUpper() + "." + end.Chapter + "." + end.Verse;//update bcv to include verse
                    }
                    
                    int chapterStart = Convert.ToInt32(start.Chapter);
                    int chapterEnd = Convert.ToInt32(end.Chapter);
                    int verseStart = Convert.ToInt32(start.Verse);
                    int verseEnd = Convert.ToInt32(end.Verse);
                    int verseCount = 1;
                    int numberofVerses = 0;

                    XmlNode startBOOK, endBOOK, BOOK;
                    BOOK = XMLDocument_Bible.CreateElement("GEN");
                    startBOOK = XMLDocument_Bible.CreateElement("GEN");
                    endBOOK = XMLDocument_Bible.CreateElement("REV");

                    foreach (XmlElement BOOKsample in KJVBibleNode.ChildNodes)
                    {
                        if (BOOKsample.Attributes["NAME"].Value == start.Book)
                        {
                            startBOOK = BOOKsample;
                            BOOK = startBOOK;
                        }
                        if (BOOKsample.Attributes["NAME"].Value == end.Book)
                        {
                            endBOOK = BOOKsample;
                        }
                    }

                    while (true)//this is an infinite loop: please ensure it does not become an infinite loop
                    {
                        if (BOOK != null)//prevent NULL exceptions
                        {
                            if (BOOK == startBOOK)
                            {
                                for (int chapterCount = chapterStart; chapterCount <= BOOK.ChildNodes.Count; chapterCount++)
                                {
                                    numberofVerses = BOOK.ChildNodes[chapterCount - 1].ChildNodes.Count;

                                    if (chapterCount == chapterStart)
                                    {
                                        verseCount = verseStart;
                                    }
                                    else
                                    {
                                        verseCount = 1;
                                    }
                                    for (; verseCount <= numberofVerses; verseCount++)
                                    {
                                        if (start.Book == end.Book && chapterCount == chapterEnd && verseCount == verseEnd)
                                        {
                                            listofVerses.Add(BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " " + BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText);
                                            return listofVerses;/*************///potential danger
                                        }
                                        listofVerses.Add(BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " " + BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText);
                                    }
                                }
                            }
                            else if (BOOK == endBOOK && BOOK != startBOOK)//only read up to specified chapter and verse
                            {
                                for (int chapterCount = 1; chapterCount <= BOOK.ChildNodes.Count; chapterCount++)
                                {
                                    numberofVerses = BOOK.ChildNodes[chapterCount - 1].ChildNodes.Count;

                                    for (verseCount = 1; verseCount <= numberofVerses; verseCount++)
                                    {
                                        if (chapterCount == chapterEnd && verseCount == verseEnd)
                                        {
                                            listofVerses.Add(BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " " + BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText);
                                            return listofVerses;/*************///potential danger
                                        }
                                        listofVerses.Add(BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " " + BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText);
                                    }
                                }
                                break;
                            }
                            else//read every chapter and verse unless it is the starting or ending BCVstruct
                            {
                                for (int chapterCount = 1; chapterCount <= BOOK.ChildNodes.Count; chapterCount++)
                                {
                                    numberofVerses = BOOK.ChildNodes[chapterCount - 1].ChildNodes.Count;

                                    for (verseCount = 1; verseCount <= numberofVerses; verseCount++)
                                    {
                                        listofVerses.Add(BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].Attributes["BCV"].Value + " " + BOOK.ChildNodes[chapterCount - 1].ChildNodes[verseCount - 1].InnerText);
                                    }
                                }
                            }
                            BOOK = BOOK.NextSibling;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return listofVerses;
                }
            }
            catch{; }
            return listofVerses;
        }

        public static string ParseForBCVStructs(string stringToParse, ref BCVSTRUCT start, ref BCVSTRUCT end)
        {
            string bcv = "";
            bool isRange = false;
            int iPosRange = 0;

            start.bcv = null;
            start.Book = null;
            start.Chapter = null;
            start.Verse = null;
            end.bcv = null;
            end.Book = null;
            end.Chapter = null;
            end.Verse = null;

            for (int i = 0; i < stringToParse.Length; i++)
            {
                if (stringToParse[i] == '-')
                {
                    isRange = true;
                    iPosRange = i;
                }
            }
            if (isRange)
            {
                bcv = stringToParse.Remove(0, (iPosRange + 1));
                end.bcv = bcv;
                end.Book = bcv.Remove(bcv.IndexOf("."));
                end.Chapter = bcv.Remove(0, bcv.IndexOf(".") + 1);
                end.Chapter = end.Chapter.Remove(end.Chapter.IndexOf("."));
                end.Verse = bcv.Remove(0, bcv.LastIndexOf(".") + 1);

                bcv = stringToParse.Remove(iPosRange);
            }
            else
            {
                bcv = stringToParse;
            }
            start.bcv = bcv;
            start.Book = bcv.Remove(bcv.IndexOf("."));
            start.Chapter = bcv.Remove(0, bcv.IndexOf(".") + 1);
            start.Chapter = start.Chapter.Remove(start.Chapter.IndexOf("."));
            start.Verse = bcv.Remove(0, bcv.LastIndexOf(".") + 1);

            string bcvToDisplay = "";
            if (end.Book == null)//no range: show single bcv
            {
                bcvToDisplay = start.bcv;
            }
            else//range: show start and end bcv's
            {
                bcvToDisplay = start.bcv + "-" + end.bcv;
            }
            //end of update

            return bcvToDisplay;
        }
        #region STATISTICAL FUNCTIONS
        private int VerseCount(string BOOK, string CHAPTER)
        {
            int verseCount = 0;

            foreach (XmlNode Book in KJVBibleNode.ChildNodes)
            {
                if (Book.Attributes["NAME"].Value == BOOK)
                {
                    foreach (XmlNode Chapter in Book)
                    {
                        if (Chapter.Attributes["ID"].Value == CHAPTER)
                        {
                            verseCount = Chapter.ChildNodes.Count;
                            break;
                        }
                    }
                    break;
                }
            }
            return verseCount;
        }
        #endregion
        #endregion
    }
}
