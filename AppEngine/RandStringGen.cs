using System;

namespace AppEngine
{
    public class RandStringGen
    {
        static Random rand = new Random();
        static public string GenerateRandString(string prefix)
        {
            string randString = prefix + "_A";//The prefix identifies doctype; A- identifies beginning of random letters of the alphabet
            char[] alphabet = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f',
                'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r',
                's', 't', 'u', 'v', 'w', 'x',
                'y', 'z'
            };
            randString += alphabet[rand.Next(0, 26)];
            randString += alphabet[rand.Next(0, 26)];
            randString += alphabet[rand.Next(0, 26)];

            randString += "S";//S- identifies beginning of difference in seconds
            randString += DateTime.Now.Subtract(DateTime.Today).TotalSeconds.ToString();

            randString += "D";//D- identifies the current date and time
            randString += DateTime.Now;

            foreach (char c in randString)
            {
                if (char.IsPunctuation(c) || char.IsWhiteSpace(c))
                {
                    if (c != '_')//remove all punctuation marks save the underscore
                    {
                        randString = randString.Replace(c.ToString(), "");
                    }
                }
            }
            return randString;
        }
    }
}
