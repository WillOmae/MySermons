/* Author: Wilbur Omae
 * Date: 
 * XMLBible
 * This is a class that handles various operations on an XML Bible
 */
using System.Collections.Generic;
using System.Text;
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

        private const char BLOCK_SEPARATOR = ';', INBLOCK_SEPARATOR = ',', RANGE_SEPARATOR = '-', CV_SEPARATOR = ':';

        /// <summary>
        /// Single chapter e.g. Hebrews 1
        /// </summary>
        private const string regexBC = @"\d?\s*[a-zA-Z]{1,}\s*\d{1,3}";
        /// <summary>
        /// Single verse e.g. Hebrews 11:1
        /// </summary>
        private const string regexBCV = @"\d?\s*[a-zA-Z]{1,}\s*\d{1,3}\s*:\s*\d{1,3}";
        /// <summary>
        /// Range of verses within a chapter e.g. Psalms 119:105 - 150
        /// </summary>
        private const string regexBCVrV = @"\d?\s*[a-zA-Z]{1,}\s*\d{1,3}\s*:\s*\d{1,3}\s*-\s*\d{1,3}";
        /// <summary>
        /// Range of chapters within the same book e.g. 1John 1 - 3
        /// </summary>
        private const string regexBCrC = @"\d?\s*[a-zA-Z]{1,}\s*\d{1,3}\s*-\s*\d{1,3}";
        /// <summary>
        /// Range of verses across chapters of the same book e.g. Revelation 20:10 - 21:10
        /// </summary>
        private const string regexBCVrCV = @"\d?\s*[a-zA-Z]{1,}\s*\d{1,3}\s*:\s*\d{1,3}-\s*\d{1,3}\s*:\s*\d{1,3}";
        /// <summary>
        /// Range of chapters across books e.g. 2John 1 - 3John 1
        /// </summary>
        private const string regexBCrBC = @"\d?\s*[a-zA-Z]{1,}\s*\d{1,3}\s*-\d?\s*[a-zA-Z]{1,}\s*\d{1,3}";
        /// <summary>
        /// Range of verses across chapters across books e.g. 2John 1:1 - 3John 1:3
        /// </summary>
        private const string regexBCVrBCV = @"\d?\s*[a-zA-Z]{1,}\s*\d{1,3}\s*:\s*\d{1,3}-\d?\s*[a-zA-Z]{1,}\s*\d{1,3}\s*:\s*\d{1,3}";

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
            if (!string.IsNullOrEmpty(stringToParse) && !string.IsNullOrWhiteSpace(stringToParse))
            {
                List<BIBLETEXTINFO> list = new List<BIBLETEXTINFO>();

                List<BCVRANGE> listOfBCVRanges = new List<BCVRANGE>();

                listOfBCVRanges = ParseString(stringToParse);

                for (int i = 0; i < listOfBCVRanges.Count; i++)
                {
                    BCVSTRUCT START = listOfBCVRanges[i].Start;
                    BCVSTRUCT END = listOfBCVRanges[i].End;
                    BIBLETEXTINFO bibleTextInfo = new BIBLETEXTINFO()
                    {
                        verse = string.Empty
                    };

                    //Update bcv to be displayed
                    if (END.Book == null)//no range: show single bcv
                    {
                        bibleTextInfo.bcv = START.bcv;
                        list.Add(bibleTextInfo);
                    }
                    else//range: show start and end bcv's
                    {
                        bibleTextInfo.bcv = START.bcv + RANGE_SEPARATOR + END.bcv;
                        list.Add(bibleTextInfo);
                    }
                    //end of update
                }
                CreateFriendlyTexts(list);
                return list;
            }
            else
            {
                return null;
            }
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
        private static List<BCVRANGE> ParseString(string stringToParse)
        {
            if (stringToParse.Length < 1)
            {
                return null;
            }

            //ensure uniformity in parsing all strings, add blockseparator to the end of the string
            stringToParse = stringToParse.Trim();
            if (!stringToParse.EndsWith(BLOCK_SEPARATOR.ToString()))
            {
                stringToParse += BLOCK_SEPARATOR;
            }

            //build a string to hold one block to be passed to the next parsing function
            StringBuilder builder = new StringBuilder(stringToParse.Length);
            List<BCVRANGE> listOfBCVRanges = new List<BCVRANGE>();

            for (int i = 0; i < stringToParse.Length; i++)
            {
                if (stringToParse[i] == BLOCK_SEPARATOR)
                {
                    if (builder.ToString().Length > 0)
                    {
                        listOfBCVRanges = ParseString1(builder.ToString());
                    }
                    builder.Clear();//clear build string to accommodate next string
                }
                else
                {
                    builder.Append(stringToParse[i]);
                }
            }

            return listOfBCVRanges;
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
        private static List<BCVRANGE> ParseString1(string stringToParse)
        {
            if (stringToParse.Length < 1)
            {
                return null;
            }

            stringToParse = stringToParse.Trim();
            //ensure uniformity in parsing all strings: inblockSeparator must exist in all strings
            if (!stringToParse.EndsWith(INBLOCK_SEPARATOR.ToString()))
            {
                stringToParse += INBLOCK_SEPARATOR;
            }

            BCVSTRUCT bibleTextCurrent = new BCVSTRUCT()
            {
                Book = null
            };
            List<BCVRANGE> listOfBCVRanges = new List<BCVRANGE>();
            bool addedVerse = false;
            StringBuilder builder = new StringBuilder(stringToParse.Length);
            for (int i = 0; i < stringToParse.Length; i++)
            {
                if (stringToParse[i] == INBLOCK_SEPARATOR)
                {
                    if (builder.ToString().Length > 0)
                    {
                        if (!string.IsNullOrEmpty(bibleTextCurrent.Book))
                        {
                            builder = new StringBuilder(SetStringForInblockSeparator(bibleTextCurrent, builder.ToString(), addedVerse));
                        }
                        bibleTextCurrent = ParseString2(builder.ToString(), ref listOfBCVRanges, out addedVerse);
                    }
                    builder.Clear();//clear builder to accommodate next string
                }
                else
                {
                    builder.Append(stringToParse[i]);
                }
            }
            return listOfBCVRanges;
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
            string returnString = string.Empty;
            BCVSTRUCT BCVNew = new BCVSTRUCT()
            {
                Book = BCVCurrent.Book
            };
            if (stringToParse.Contains(CV_SEPARATOR.ToString()))//only the book is shared; it contains its own chapter and verse
            {
                BCVNew.Chapter = stringToParse.Remove(stringToParse.IndexOf(CV_SEPARATOR));
                BCVNew.Verse = stringToParse.Remove(0, stringToParse.IndexOf(CV_SEPARATOR) + 1);
                returnString = BCVNew.Book + BCVNew.Chapter + CV_SEPARATOR + BCVNew.Verse;
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
                        returnString = BCVNew.Book + BCVNew.Chapter + CV_SEPARATOR + BCVNew.Verse;
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
            addedVerse = false;

            if (new Regex(regexBCVrBCV).IsMatch(stringToParse))
            {
                int separatorIndex = stringToParse.IndexOf(RANGE_SEPARATOR);

                string start = stringToParse.Remove(separatorIndex);
                bibleText_Start = StringToBCV(start);
                bibleText_Start.Book = ConfirmBibleBookName(bibleText_Start.Book);
                addedVerse = false;

                string end = stringToParse.Substring(separatorIndex + 1);
                bibleText_End = StringToBCV(end);
                bibleText_End.Book = ConfirmBibleBookName(bibleText_End.Book);
            }
            else if (new Regex(regexBCrBC).IsMatch(stringToParse))
            {
                int separatorIndex = stringToParse.IndexOf(RANGE_SEPARATOR);

                string start = stringToParse.Remove(separatorIndex);
                bibleText_Start = StringToBCV(start);
                bibleText_Start.Book = ConfirmBibleBookName(bibleText_Start.Book);
                bibleText_Start.Verse = "1";
                addedVerse = true;

                string end = stringToParse.Substring(separatorIndex + 1);
                bibleText_End = StringToBCV(end);
                bibleText_End.Book = ConfirmBibleBookName(bibleText_End.Book);
                bibleText_End.Verse = VerseCount(bibleText_End.Book, bibleText_End.Chapter).ToString();
            }
            else if (new Regex(regexBCVrCV).IsMatch(stringToParse))
            {
                int separatorIndex = stringToParse.IndexOf(RANGE_SEPARATOR);

                string start = stringToParse.Remove(separatorIndex);
                bibleText_Start = StringToBCV(start);
                bibleText_Start.Book = ConfirmBibleBookName(bibleText_Start.Book);
                addedVerse = false;

                string end = bibleText_Start.Book + stringToParse.Substring(separatorIndex + 1);
                bibleText_End = StringToBCV(end);
                bibleText_End.Book = ConfirmBibleBookName(bibleText_End.Book);
            }
            else if (new Regex(regexBCrC).IsMatch(stringToParse))
            {
                int separatorIndex = stringToParse.IndexOf(RANGE_SEPARATOR);

                string start = stringToParse.Remove(separatorIndex);
                bibleText_Start = StringToBCV(start);
                bibleText_Start.Book = ConfirmBibleBookName(bibleText_Start.Book);
                bibleText_Start.Verse = "1";
                addedVerse = true;

                string end = bibleText_Start.Book + stringToParse.Substring(separatorIndex + 1);
                bibleText_End = StringToBCV(end);
                bibleText_End.Book = ConfirmBibleBookName(bibleText_End.Book);
                bibleText_End.Verse = VerseCount(bibleText_End.Book, bibleText_End.Chapter).ToString();
            }
            else if (new Regex(regexBCVrV).IsMatch(stringToParse))
            {
                int separatorIndex = stringToParse.IndexOf(RANGE_SEPARATOR);

                string start = stringToParse.Remove(separatorIndex);
                bibleText_Start = StringToBCV(start);
                bibleText_Start.Book = ConfirmBibleBookName(bibleText_Start.Book);
                addedVerse = false;

                string end = bibleText_Start.Book + bibleText_Start.Chapter + CV_SEPARATOR + stringToParse.Substring(separatorIndex + 1);
                bibleText_End = StringToBCV(end);
                bibleText_End.Book = ConfirmBibleBookName(bibleText_End.Book);
            }
            else if (new Regex(regexBCV).IsMatch(stringToParse))
            {
                bibleText_Start = StringToBCV(stringToParse);
                bibleText_Start.Book = ConfirmBibleBookName(bibleText_Start.Book);
                addedVerse = false;
            }
            else if (new Regex(regexBC).IsMatch(stringToParse))
            {
                bibleText_Start = StringToBCV(stringToParse);
                bibleText_Start.Book = ConfirmBibleBookName(bibleText_Start.Book);
                bibleText_Start.Verse = "1";
                addedVerse = true;

                bibleText_End.Book = bibleText_Start.Book;
                bibleText_End.Chapter = bibleText_Start.Chapter;
                bibleText_End.Verse = VerseCount(bibleText_End.Book, bibleText_End.Chapter).ToString();
            }
            bibleText_Start.bcv = GenerateBCVString(bibleText_Start);
            bibleText_End.bcv = GenerateBCVString(bibleText_End);

            BCVRANGE range = new BCVRANGE()
            {
                Start = bibleText_Start,
                End = bibleText_End
            };

            if (bibleText_Start.Book != null)
            {
                listOfBCVRanges.Add(range);
            }

            if (bibleText_End.Book != null)
            {
                return bibleText_End;
            }
            else if (bibleText_Start.Book != null)
            {
                return bibleText_Start;
            }
            else
            {
                return new BCVSTRUCT()
                {
                    bcv = null,
                    Book = null,
                    Chapter = null,
                    Verse = null
                };
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
        /// Parses a string e.g. 1John1:1 and extracts the book, chapter and verse
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <returns>BCVStruct containing the separate components.</returns>
        private static BCVSTRUCT StringToBCV(string input)
        {
            BCVSTRUCT bcv;
            bcv.Book = bcv.Chapter = bcv.Verse = bcv.bcv = string.Empty;
            bool cvSeparatorFound = false;

            StringBuilder buildBook = new StringBuilder(12);
            StringBuilder buildChapter = new StringBuilder(3);
            StringBuilder buildVerse = new StringBuilder(3);

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c == CV_SEPARATOR)
                {
                    cvSeparatorFound = true;
                }
                else if (char.IsLetter(c))
                {
                    buildBook.Append(c);
                    bcv.Book += c;
                }
                else if (char.IsDigit(c))
                {
                    if (i == 0)
                    {
                        buildBook.Append(c);
                    }
                    else
                    {
                        if (cvSeparatorFound)
                        {
                            buildVerse.Append(c);
                        }
                        else
                        {
                            buildChapter.Append(c);
                        }
                    }
                }
            }
            bcv.Book = buildBook.ToString();
            bcv.Chapter = buildChapter.ToString();
            bcv.Verse = buildVerse.ToString();

            return bcv;
        }
        /// <summary>
        /// Concatenates book, chapter and verse to form a single BCV.
        /// </summary>
        /// <param name="input">The BCVStruct.</param>
        /// <returns>bcv</returns>
        private static string GenerateBCVString(BCVSTRUCT input)
        {
            if (!string.IsNullOrEmpty(input.Book) && !string.IsNullOrEmpty(input.Chapter) && !string.IsNullOrEmpty(input.Verse))
            {
                return input.Book.ToUpper() + "." + input.Chapter + "." + input.Verse;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Ascertains the presence of the Bible book in the search string passed to it.
        /// </summary>
        /// <param name="search">String containing the book to be confirmed.</param>
        /// <returns></returns>
        private static string ConfirmBibleBookName(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
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
                            return XMLElement.Attributes["c"].Value;
                        }
                        else if (XMLElement.Attributes["short"].Value.ToLower() == search.ToLower())
                        {
                            return XMLElement.Attributes["c"].Value;
                        }
                    }
                    /* 2nd best search:
                     * First letter match
                     * searchedString contains the searchString in order i.e.
                        searchedString == searchStringxyzabc
                     */
                    foreach (XmlElement XMLElement in BibleBookNames.ChildNodes)
                    {
                        if (StringSearch.AllExist_InOrder(XMLElement.Attributes["abbr"].Value.ToLower(), search.ToLower())
                            && XMLElement.Attributes["abbr"].Value.ToLower()[0] == search.ToLower()[0])
                        {
                            return XMLElement.Attributes["c"].Value;
                        }
                        else if (StringSearch.AllExist_InOrder(XMLElement.Attributes["short"].Value.ToLower(), search.ToLower())
                            && XMLElement.Attributes["short"].Value.ToLower()[0] == search.ToLower()[0])
                        {
                            return XMLElement.Attributes["c"].Value;
                        }
                    }
                    /* 3rd best search:
                     * searchedString contains the searchString in order i.e.
                        searchedString == abcsearchStringxyz
                     */
                    foreach (XmlElement XMLElement in BibleBookNames.ChildNodes)
                    {
                        if (StringSearch.AllExist_InOrder(XMLElement.Attributes["abbr"].Value.ToLower(), search.ToLower()))
                        {
                            return XMLElement.Attributes["c"].Value;
                        }
                        else if (StringSearch.AllExist_InOrder(XMLElement.Attributes["short"].Value.ToLower(), search.ToLower()))
                        {
                            return XMLElement.Attributes["c"].Value;
                        }
                    }
                    /* Worst search
                     * searchedString contains the searchString in whichever order of occurrence of letters
                     */
                    foreach (XmlElement XMLElement in BibleBookNames.ChildNodes)
                    {
                        if (StringSearch.AllExist(XMLElement.Attributes["abbr"].Value.ToLower(), search.ToLower()))
                        {
                            return XMLElement.Attributes["c"].Value;
                        }
                        else if (StringSearch.AllExist(XMLElement.Attributes["short"].Value.ToLower(), search.ToLower()))
                        {
                            return XMLElement.Attributes["c"].Value;
                        }
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets verse text for the given range.
        /// </summary>
        /// <param name="start">The beginning of the range.</param>
        /// <param name="end">The end of the range (values optional).</param>
        /// <returns></returns>
        public static List<string> GetVerseText(ref BCVSTRUCT start, ref BCVSTRUCT end)
        {
            if (end.Book == null)//No range; get single verse
            {
                return new List<string>()
                {
                    GetSingleVerse(start)
                };
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
                return null;
            }
            catch
            {
                return null;
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
                    if (ch < BOOK.ChildNodes.Count)
                    {
                        List<string> list = new List<string>();
                        for (int i = v1; i <= v2; i++)
                        {
                            if (i < BOOK.ChildNodes[ch].ChildNodes.Count)
                            {
                                try
                                {
                                    var text = BOOK.ChildNodes[ch].ChildNodes[i].Attributes["BCV"].Value + " " + BOOK.ChildNodes[ch].ChildNodes[i].InnerText;
                                    list.Add(text);
                                }
                                catch
                                {
                                    //error occurred
                                }
                            }
                        }
                        return list;
                    }
                    else
                    {
                        return null;
                    }
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
                int iPosRange = stringToParse.IndexOf(RANGE_SEPARATOR);

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
        private static void CreateFriendlyTexts(List<BIBLETEXTINFO> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                var item = list[i];
                BCVSTRUCT start = new BCVSTRUCT();
                BCVSTRUCT end = new BCVSTRUCT();

                ParseForBCVStructs(item.bcv, ref start, ref end);
                if (string.IsNullOrEmpty(start.Book))
                {
                    continue;
                }
                else
                {
                    if (string.IsNullOrEmpty(end.Book))
                    {
                        item.FriendlyText = GetBookName_abbr(start.Book) + " " + start.Chapter + ":" + start.Verse;
                    }
                    else
                    {
                        if (start.Verse == "1" && end.Verse == VerseCount(end.Book, end.Chapter).ToString())
                        {
                            item.FriendlyText = GetBookName_short(start.Book) + " " + start.Chapter;
                        }
                        else
                        {
                            item.FriendlyText = GetBookName_abbr(start.Book) + " " + start.Chapter + ":" + start.Verse + "-" + end.Verse;
                        }
                    }
                }
                list[i] = item;
            }
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

        private static string GetBookName_c(string name_short)
        {
            foreach (XmlNode Book in BibleBookNames.ChildNodes)
            {
                if (Book.Attributes["short"].Value == name_short)
                {
                    return Book.Attributes["c"].Value;
                }
            }
            return null;
        }
        private static string GetBookName_abbr(string name_c)
        {
            foreach (XmlNode Book in BibleBookNames.ChildNodes)
            {
                if (Book.Attributes["c"].Value == name_c)
                {
                    return Book.Attributes["abbr"].Value;
                }
            }
            return null;
        }
        private static string GetBookName_short(string name_c)
        {
            foreach (XmlNode Book in BibleBookNames.ChildNodes)
            {
                if (Book.Attributes["c"].Value == name_c)
                {
                    return Book.Attributes["short"].Value;
                }
            }
            return null;
        }
    }
}
