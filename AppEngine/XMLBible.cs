/* Author: Wilbur Omae
 * Date: 
 * XMLBible
 * This is a class that handles various operations on an XML Bible
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace AppEngine
{
    public class XMLBible
    {
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
            set
            {
                xmldocumentbible = value;
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

        static private readonly char blockSeparator = ';', inblockSeparator = ',', rangeSeparator = '-', cvSeparator = ':';
        
        /// <summary>
        /// Loads the XML file into memory.
        /// </summary>
        public static void LoadBibleIntoMemory()
        {
            xmldocumentbible = new XmlDocument();
            xmldocumentbible.Load(FileNames.Bible);
        }
        /// <summary>
        /// Parses user input to a form that the program can later use to display verses
        /// </summary>
        /// <param name="stringToParse">String passed to be parsed</param>
        /// <returns></returns>
        public static List<BIBLETEXTINFO> ParseStringToVerse(string stringToParse)
        {
            List<BIBLETEXTINFO> list = new List<BIBLETEXTINFO>();

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
                        BIBLETEXTINFO bibleTextInfo = new BIBLETEXTINFO()
                        {
                            //bibleTextInfo.verse = VERSE;
                            verse = ""
                        };

                        //Update bcv to be displayed
                        if (END.Book == null)//no range: show single bcv
                        {
                            bibleTextInfo.bcv = START.bcv;
                            list.Add(bibleTextInfo);
                        }
                        else//range: show start and end bcv's
                        {
                            bibleTextInfo.bcv = START.bcv + rangeSeparator + END.bcv;
                            list.Add(bibleTextInfo);
                        }
                        //end of update
                    }
                    catch {; }
                }
            }
            else {; }

            return list;
        }
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
        private static void ParseString(string stringToParse, ref List<BCVRANGE> listOfBCVRanges)
        {
            //ensure uniformity in parsing all strings, add blockseparator to the end of the string
            //the blockSeparator must exist in all strings
            if (!stringToParse.EndsWith(blockSeparator.ToString()))
            {
                stringToParse += blockSeparator;
            }

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
        private static void ParseString1(string stringToParse, ref List<BCVRANGE> listOfBCVRanges)
        {
            //ensure uniformity in parsing all strings: inblockSeparator must exist in all strings
            if (!stringToParse.EndsWith(inblockSeparator.ToString()))
            {
                stringToParse += inblockSeparator;
            }

            BCVSTRUCT bibleTextCurrent = new BCVSTRUCT()
            {
                Book = null
            };
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
        ///     It checks the string passed and correctly supplies missing information
        ///     such as BOOK name and/or CHAPTER.
        /// </summary>
        /// <remarks>
        ///     For example:
        ///     From 12 in HEBREWS11:1-6 and 12 separated in the calling function,
        ///     the function returns HEBREWS11:12 for the inblock separated string "12".
        /// </remarks>
        /// <param name="BCVCurrent">
        ///     BCVstruct containing details of the most previous verse handled.
        ///     It gives information for the sub-block to be parsed.</param>
        /// <param name="stringToParse"></param>
        /// <returns></returns>
        private static string SetStringForInblockSeparator(BCVSTRUCT BCVCurrent, string stringToParse, bool addedVerse)
        {
            string returnString = "";
            BCVSTRUCT BCVNew = new BCVSTRUCT()
            {
                Book = BCVCurrent.Book
            };
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
        private static BCVSTRUCT ParseString2(string stringToParse, ref List<BCVRANGE> listOfBCVRanges, out bool addedVerse)
        {
            BCVSTRUCT bibleText_Start = new BCVSTRUCT()
            {
                bcv = null,
                Book = null,
                Chapter = null,
                Verse = null
            };
            BCVSTRUCT bibleText_End = new BCVSTRUCT()
            {
                bcv = null,
                Book = null,
                Chapter = null,
                Verse = null
            };
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
                {
                    //sample ~Heb13-Jas1~
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
            BCVRANGE BTG = new BCVRANGE()
            {
                Start = bibleText_Start,
                End = bibleText_End
            };
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
        private static void GetEnd(string stringToParse, BCVSTRUCT bibleText_Start, ref BCVSTRUCT bibleText_End)
        {
            if (!string.IsNullOrWhiteSpace(stringToParse) && stringToParse.Length > 0)
            {
                bool chapterFound = false;

                if (stringToParse.Length >= 2)
                {
                    if (char.IsDigit(stringToParse[0]) && char.IsLetter(stringToParse[1]))//e.g 2John1:1-3John1:1
                    {
                        bibleText_End.Book += stringToParse[0];
                    }
                }
                foreach (char c in stringToParse)
                {
                    if (char.IsLetter(c))//e.g Hebrews11:1-Hebrews12:1 or the rest of 2John1:1-3John1:1 if the preceding conditions were met
                    {
                        bibleText_End.Book += c;
                        stringToParse = stringToParse.Remove(0, stringToParse.IndexOf(c) + 1);
                    }
                }

                if (string.IsNullOrWhiteSpace(bibleText_End.Book))//e.g Hebrews11:1-12:1 End.Book should be Hebrews as well
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
        /// Assertains the presence of the Bible book in the search string passed to it.
        /// </summary>
        /// <param name="search">String containing the book to be confirmed.</param>
        /// <returns></returns>
        private static string ConfirmBibleBookName(string search)
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
        public static List<string> GetVerseText(ref BCVSTRUCT start, ref BCVSTRUCT end)
        {
            List<string> listofVerses = new List<string>();
            if (end.Book == null)//No range; get single verse
            {
                listofVerses.Add(GetSingleVerse(start));
            }
            else//There is a range i.e. both start and end are known
            {
                bool startFound = false, endFound = false;
                foreach (XmlElement BOOKsample in KJVBibleNode.ChildNodes)
                {
                    if (BOOKsample.Attributes["NAME"].Value == start.Book)
                    {
                        startFound = true;
                    }
                    if (BOOKsample.Attributes["NAME"].Value == end.Book)
                    {
                        endFound = true;
                    }
                    if (endFound)
                    {
                        if (startFound)
                        {
                            return GetMultipleVerses(start, end);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                return null;
            }
            return listofVerses;
        }
        /// <summary>
        /// Get verse text for a single bcv.
        /// </summary>
        /// <param name="start">Specified bcv.</param>
        /// <returns>Single verse text.</returns>
        private static string GetSingleVerse(BCVSTRUCT start)
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
                                        return VERSE.Attributes["BCV"].Value + " " + VERSE.InnerText;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                return string.Empty;
            }
            catch
            {
                return null;//complete
            }
        }
        /// <summary>
        /// Handler for getting multiple verses.
        /// </summary>
        /// <param name="start">Specified beginning bcv.</param>
        /// <param name="end">Specified ending bcv.</param>
        /// <returns>List of verse texts.</returns>
        private static List<string> GetMultipleVerses(BCVSTRUCT start, BCVSTRUCT end)
        {
            if (start.Book == end.Book)
            {
                if (start.Chapter == end.Chapter)
                {
                    return GetMultipleVersesSameBookSameChapter(start.Book, int.Parse(start.Chapter) - 1, int.Parse(start.Verse) - 1, int.Parse(end.Verse) - 1);
                }
                else
                {
                    return GetMultipleVersesSameBook(start.Book, start, end);
                }
            }
            else
            {
                return GetMultipleVersesDifferentBooks(start, end);
            }
        }
        /// <summary>
        /// Get multiple verses from the same chapter of a book.
        /// </summary>
        /// <param name="book">The common book.</param>
        /// <param name="ch">The common chapter.</param>
        /// <param name="v1">The beginning verse.</param>
        /// <param name="v2">The ending verse.</param>
        /// <returns>List of verse texts.</returns>
        private static List<string> GetMultipleVersesSameBookSameChapter(string book, int ch, int v1, int v2)
        {
            foreach (XmlNode BOOK in KJVBibleNode.ChildNodes)
            {
                if (BOOK.Attributes["NAME"].Value == book)
                {
                    List<string> list = new List<string>();
                    for (int i = v1; i <= v2; i++)
                    {
                        list.Add(BOOK.ChildNodes[ch].ChildNodes[i].Attributes["BCV"].Value + " " + BOOK.ChildNodes[ch].ChildNodes[i].InnerText);
                    }
                    return list;
                }
            }
            return null;
        }
        /// <summary>
        /// Get multiple verses from the a range of chapters of a book.
        /// </summary>
        /// <param name="book">The common book.</param>
        /// <param name="start">The beginning bcv.</param>
        /// <param name="end">The ending bcv.</param>
        /// <returns>List of verse texts.</returns>
        private static List<string> GetMultipleVersesSameBook(string book, BCVSTRUCT start, BCVSTRUCT end)
        {
            foreach (XmlNode BOOKSample in KJVBibleNode.ChildNodes)
            {
                XmlNode BOOK = BOOKSample;
                if (BOOK.Attributes["NAME"].Value == book)
                {
                    List<string> list = new List<string>();
                    int ch1 = int.Parse(start.Chapter) - 1, ch2 = int.Parse(end.Chapter) - 1;
                    int v1, v2;
                    for (int j = ch1; j <= ch2; j++)
                    {
                        var CHAPTER = BOOK.ChildNodes[j];
                        if (j == ch1)//read the first chapter from the specified verse to the end
                        {
                            v1 = int.Parse(start.Verse);
                            v2 = VerseCount(start.Book, start.Chapter);
                        }
                        else if (j == ch2)//read the last chapter from the beginning to the specified verse
                        {
                            v1 = 1;
                            v2 = int.Parse(end.Verse);
                        }
                        else//read the chapter from the beginning to the end
                        {
                            v1 = 1;
                            v2 = VerseCount(start.Book, (j + 1).ToString());
                        }
                        list.AddRange(GetMultipleVersesSameBookSameChapter(book, j, v1 - 1, v2 - 1));
                        CHAPTER = CHAPTER.NextSibling;
                    }
                    return list;
                }
            }
            return null;
        }
        /// <summary>
        /// Get multiple verses from a range of books.
        /// </summary>
        /// <param name="start">The beginning bcv.</param>
        /// <param name="end">The ending bcv.</param>
        /// <returns>List of verse texts.</returns>
        private static List<string> GetMultipleVersesDifferentBooks(BCVSTRUCT start, BCVSTRUCT end)
        {
            XmlNode BOOK = null;
            List<string> list = new List<string>();
            BCVSTRUCT bookStart = new BCVSTRUCT(), bookEnd = new BCVSTRUCT();
            string book;

            foreach (XmlNode BOOKSample in KJVBibleNode.ChildNodes)
            {
                if (BOOKSample.Attributes["NAME"].Value == start.Book)
                {
                    BOOK = BOOKSample;
                }
            }
            do
            {
                book = BOOK.Attributes["NAME"].Value;
                if (book == start.Book)//read the first book from the specified chapter and verse to the end of the book
                {
                    bookStart = start;

                    bookEnd.Book = start.Book;
                    bookEnd.Chapter = ChapterCount(bookEnd.Book).ToString();
                    bookEnd.Verse = VerseCount(bookEnd.Book, bookEnd.Chapter).ToString();
                }
                else if (book == end.Book)//read the last book from the beginning to the specified chapter and verse
                {
                    bookStart.Book = end.Book;
                    bookStart.Chapter = "1";
                    bookStart.Verse = "1";

                    bookEnd = end;
                }
                else//read the chapter from the beginning to the end
                {
                    bookStart.Book = book;
                    bookStart.Chapter = "1";
                    bookStart.Verse = "1";

                    bookEnd.Book = book;
                    bookEnd.Chapter = ChapterCount(bookEnd.Book).ToString();
                    bookEnd.Verse = VerseCount(bookEnd.Book, bookEnd.Chapter).ToString();
                }
                list.AddRange(GetMultipleVersesSameBook(book, bookStart, bookEnd));
                BOOK = BOOK.NextSibling;
            } while (book != end.Book);
            return list;
        }

        /// <summary>
        /// Check if the string is formatted in a way that is usable by the program.
        /// </summary>
        /// <param name="stringToParse">The string to be parsed.</param>
        /// <param name="start">The beginning bcv structure.</param>
        /// <param name="end">The ending bcv structure (for ranges).</param>
        /// <returns>The bcv to display.</returns>
        public static string ParseForBCVStructs(string stringToParse, ref BCVSTRUCT start, ref BCVSTRUCT end)
        {
            if (Regex.IsMatch(stringToParse, @"\w{1,3}\.\d{1,3}\.\d{1,3}\-\w{1,3}\.\d{1,3}\.\d{1,3}"))
            {
                int iPosRange = stringToParse.IndexOf(rangeSeparator);

                var startString = stringToParse.Remove(iPosRange);
                startString = startString.Replace("-", string.Empty);
                start.bcv = startString;
                start.Book = startString.Remove(startString.IndexOf("."));
                start.Chapter = startString.Remove(0, startString.IndexOf(".") + 1);
                start.Chapter = start.Chapter.Remove(start.Chapter.IndexOf("."));
                start.Verse = startString.Remove(0, startString.LastIndexOf(".") + 1);

                startString = stringToParse.Remove(0, (iPosRange + 1));
                end.bcv = startString;
                end.Book = startString.Remove(startString.IndexOf("."));
                end.Chapter = startString.Remove(0, startString.IndexOf(".") + 1);
                end.Chapter = end.Chapter.Remove(end.Chapter.IndexOf("."));
                end.Verse = startString.Remove(0, startString.LastIndexOf(".") + 1);

                startString = stringToParse.Remove(iPosRange);
                
                return start.bcv + "-" + end.bcv;
            }
            else
            {
                var startString = Regex.Match(stringToParse, @"\w{1,3}\.\d{1,3}\.\d{1,3}").Value;
                if (!string.IsNullOrEmpty(startString))
                {
                    var bcv = startString;
                    start.bcv = bcv;
                    start.Book = bcv.Remove(bcv.IndexOf("."));
                    start.Chapter = bcv.Remove(0, bcv.IndexOf(".") + 1);
                    start.Chapter = start.Chapter.Remove(start.Chapter.IndexOf("."));
                    start.Verse = bcv.Remove(0, bcv.LastIndexOf(".") + 1);

                    end.bcv = null;
                    end.Book = null;
                    end.Chapter = null;
                    end.Chapter = null;
                    end.Verse = null;

                    return start.bcv;
                }
            }
            return "NOT_A_VERSE";
        }

        /// <summary>
        /// Returns the number of books in the Bible
        /// </summary>
        /// <returns></returns>
        private static int BookCount()
        {
            int bookCount = KJVBibleNode.ChildNodes.Count;

            return bookCount;
        }
        /// <summary>
        /// Returns the number of chapters in the specified book
        /// </summary>
        /// <param name="BOOK">The specified book</param>
        /// <returns></returns>
        private static int ChapterCount(string BOOK)
        {
            int chapterCount = 0;

            foreach (XmlNode Book in KJVBibleNode.ChildNodes)
            {
                if (Book.Attributes["NAME"].Value == BOOK)
                {
                    chapterCount = Book.ChildNodes.Count;
                    break;
                }
            }
            return chapterCount;
        }
        /// <summary>
        /// Returns the number of verses in the specified chapter of the specified book
        /// </summary>
        /// <param name="BOOK">The specified book</param>
        /// <param name="CHAPTER">The specified chapter</param>
        /// <returns></returns>
        private static int VerseCount(string BOOK, string CHAPTER)
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
    }
}
