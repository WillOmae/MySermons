using AppEngine;
using System.Text.RegularExpressions;

namespace AppUI
{
    public class Cleaner
    {
        public class VerseHiddenText
        {
            private delegate void d_DeleteText(string toRemove, RichTextBoxEx rtb);
            public static void RemoveVerseHiddenText(string existingRtf, RichTextBoxEx rtb)
            {
                d_DeleteText myDelegate;

                var textMatches = Regex.Matches(existingRtf, @"\\v(.*?)\\v0");
                if (textMatches.Count > 0)
                {
                    foreach (Match match in textMatches)
                    {
                        myDelegate = new d_DeleteText(InvokeRemoveText);
                        rtb.Invoke(myDelegate, match.Value, rtb);
                    }
                }
            }
            private static void InvokeRemoveText(string toRemove, RichTextBoxEx rtb)
            {
                if (toRemove.Contains(@"\b") && !toRemove.Contains(@"\b0"))
                {
                    rtb.Rtf = rtb.Rtf.Replace(toRemove, @"\b");
                }
                else if (toRemove.Contains(@"\b0") && !toRemove.Contains(@"\b"))
                {
                    rtb.Rtf = rtb.Rtf.Replace(toRemove, @"\b0");
                }
                else
                {
                    rtb.Rtf = rtb.Rtf.Replace(toRemove, string.Empty);
                }
            }
        }
        public class BCVVerses
        {
            private delegate void d_ReplaceOldVerseText(string oldVerseText, string newVerseText, RichTextBoxEx rtb);

            public static void ConvertOldFormat1(string existingRtf, RichTextBoxEx rtb)
            {
                var textMatches1 = Regex.Matches(existingRtf, @"\d?[a-zA-Z]{1,3}\.\d{1,3}\.\d{1,3}-\d?[a-zA-Z]{1,3}\.\d{1,3}\.\d{1,3}");
                var textMatches2 = Regex.Matches(existingRtf, @"\d?[a-zA-Z]{1,3}\.\d{1,3}\.\d{1,3}");

                int arraySize = textMatches1.Count + textMatches2.Count;
                Match[] matches = new Match[arraySize];

                textMatches1.CopyTo(matches, 0);
                textMatches2.CopyTo(matches, textMatches1.Count);

                if (matches.Length > 0)
                {
                    foreach (Match match in matches)
                    {
                        string oldVerseText = match.Value.Trim();
                        XMLBible.BCVSTRUCT start, end;
                        start = new XMLBible.BCVSTRUCT();
                        end = new XMLBible.BCVSTRUCT();
                        XMLBible.ParseForBCVStructs(oldVerseText, ref start, ref end);



                        if (!string.IsNullOrEmpty(start.Book) &&
                            XMLBible.ConfirmBookNameExists(start.Book) &&
                            int.Parse(start.Chapter) <= XMLBible.ChapterCount(start.Book) &&
                            int.Parse(start.Verse) <= XMLBible.VerseCount(start.Book, start.Chapter))
                        {
                            string newVerseText = string.Empty;
                            if (string.IsNullOrEmpty(end.Book))
                            {
                                newVerseText = XMLBible.GetBookName_abbr(start.Book) + " " + start.Chapter + ":" + start.Verse;
                            }
                            else
                            {
                                if (XMLBible.ConfirmBookNameExists(end.Book) &&
                                    int.Parse(end.Chapter) <= XMLBible.ChapterCount(end.Book) &&
                                    int.Parse(end.Verse) <= XMLBible.VerseCount(end.Book, end.Chapter))
                                {
                                    if (start.Verse == "1" && end.Verse == XMLBible.VerseCount(end.Book, end.Chapter).ToString())
                                    {
                                        newVerseText = XMLBible.GetBookName_short(start.Book) + " " + start.Chapter;
                                    }
                                    else
                                    {
                                        newVerseText = XMLBible.GetBookName_abbr(start.Book) + " " + start.Chapter + ":" + start.Verse + "-" + end.Verse;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(newVerseText))
                            {
                                d_ReplaceOldVerseText myDelegate = new d_ReplaceOldVerseText(InvokeReplaceOldVerseText);
                                rtb.Invoke(myDelegate, oldVerseText, newVerseText, rtb);
                            }
                        }



                    }
                }
            }
            private static void InvokeReplaceOldVerseText(string oldText, string newText, RichTextBoxEx rtb)
            {
                rtb.Rtf = rtb.Rtf.Replace(oldText, newText);
            }
        }
    }
}
