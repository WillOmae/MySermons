namespace AppEngine
{
    public class MyDateTime
    {

        /* struct _structDateFormat
         * This <struct> holds strings of the various components of a date
         * e.g. date, month, year
         */
        public struct _structDateFormat
        {
            public string szIDate;
            public string szIMonth;
            public string szIYear;
        }

        public string DayDate;
        public string DayName;
        public string Month;
        public string Year;
        public string Hours;
        public string Minutes;
        public bool is24Hours;

        public MyDateTime(string daydate, string dayname, string month, string year, string hours, string minutes, bool is24)
        {
            DayDate = daydate;
            DayName = dayname;
            Month = month;
            Year = year;
            Hours = hours;
            Minutes = minutes;
            is24Hours = is24;
        }
        public MyDateTime()
        {

        }
        /* GetSetDate(<string> szDate, <string> szDateSeparator);
         * This function breaks down a string of dates into respective components by watching out for the date separator
         * e.g DD/MM/YYYY with the separator "/" becomes 
         *    1. date = DD
         *    2. month = MM
         *    3. year = YYYY
         * The function accepts two parameters:
         *    1. <string> szDate which holds the date to be subdivided
         *    2. <string> szDateSeparator which holds the date separator character (<string> chosen to avoid the complexities of <char>)
         * and returns a <struct> structDateFormat containing the three strings szIDate, szIMonth and szIYear
         * It is declared as static so that is is usable within other classes without having to declare an object of the base class
         */
        public static _structDateFormat GetSetDate(string szDate, string szDateSeparator)
        {
            _structDateFormat structDateFormat = new _structDateFormat();
            int iPos = 0, iStart = 0;
            if (szDate.Contains(szDateSeparator) == true)
            {
                if (szDate.EndsWith(".") == true)
                {
                    szDate = szDate.Replace(".", "");
                }
                iPos = szDate.IndexOf(szDateSeparator);
                structDateFormat.szIDate = szDate.Substring(iStart, iPos);
                if (structDateFormat.szIDate.Length == 1) { structDateFormat.szIDate = "0" + structDateFormat.szIDate; }

                szDate = szDate.Remove(iStart, iPos + 1);
                iPos = szDate.IndexOf(szDateSeparator);
                structDateFormat.szIMonth = szDate.Substring(iStart, iPos);
                if (structDateFormat.szIMonth.Length == 1) { structDateFormat.szIMonth = "0" + structDateFormat.szIMonth; }

                szDate = szDate.Remove(iStart, iPos + 1);
                structDateFormat.szIYear = szDate;
            }
            else
            {
                structDateFormat.szIDate = "_default_value_";
                structDateFormat.szIMonth = "_default_value_";
                structDateFormat.szIYear = "_default_value_";
            }
            return structDateFormat;
        }
    }
}
