using System.Drawing;
using System.Windows.Forms;

namespace AppEngine
{
    public class ColorExtractor
    {
        static Form formTrial = new Form();
        static public Color ExtractColor(string parseString)
        {
            Color colorToReturn;
            if ((colorToReturn = ColorFromARGB(parseString)) == Color.Transparent)
            {
                if ((colorToReturn = ColorFromConvertor(parseString)) == Color.Transparent)
                {
                    if ((colorToReturn = ColorFromName(parseString)) == Color.Transparent)
                    {
                        return Color.Red;
                    }
                }
            }
            return colorToReturn;
        }
        static private Color ColorFromName(string colorParse)
        {
            Color color;
            try
            {
                color = Color.FromName(colorParse);
                formTrial.BackColor = color;
                return color;
            }
            catch
            {
                return Color.Transparent;
            }
        }
        static private Color ColorFromConvertor(string colorParse)
        {
            Color color = new Color();
            try
            {
                color = (Color)new ColorConverter().ConvertFromString(colorParse);
                formTrial.BackColor = color;
                return color;
            }
            catch
            {
                return Color.Transparent;
            }
        }
        static private Color ColorFromARGB(string colorParse)
        {
            try
            {
                Color color;
                if (colorParse.Length == 6)
                {
                    int red = int.Parse(colorParse.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    int green = int.Parse(colorParse.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    int blue = int.Parse(colorParse.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    color = Color.FromArgb(red, green, blue);
                }
                else if (colorParse.Length == 8)
                {
                    int alpha = int.Parse(colorParse.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    int red = int.Parse(colorParse.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    int green = int.Parse(colorParse.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    int blue = int.Parse(colorParse.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    color = Color.FromArgb(alpha, red, green, blue);
                }
                else if (colorParse.Length == 9)
                {
                    int red = int.Parse(colorParse.Substring(0, 3), System.Globalization.NumberStyles.Integer);
                    int green = int.Parse(colorParse.Substring(3, 3), System.Globalization.NumberStyles.Integer);
                    int blue = int.Parse(colorParse.Substring(6, 3), System.Globalization.NumberStyles.Integer);
                    color = Color.FromArgb(red, green, blue);
                }
                else if (colorParse.Length == 12)
                {
                    int alpha = int.Parse(colorParse.Substring(0, 3), System.Globalization.NumberStyles.Integer);
                    int red = int.Parse(colorParse.Substring(3, 3), System.Globalization.NumberStyles.Integer);
                    int green = int.Parse(colorParse.Substring(6, 3), System.Globalization.NumberStyles.Integer);
                    int blue = int.Parse(colorParse.Substring(9, 3), System.Globalization.NumberStyles.Integer);
                    color = Color.FromArgb(alpha, red, green, blue);
                }
                else
                {
                    color = Color.Transparent;
                }
                formTrial.BackColor = color;
                return color;
            }
            catch
            {
                return Color.Transparent;
            }
        }
        static public string ToParseableString(Color colorToParse)
        {
            string r, g, b, result;
            r = colorToParse.R.ToString();
            g = colorToParse.G.ToString();
            b = colorToParse.B.ToString();
            if (colorToParse.R < 100)
            {
                if (colorToParse.R == 0)
                {
                    r = "00" + r;
                }
                else
                {
                    r = "0" + r;
                }
            }
            if (colorToParse.G < 100)
            {
                if (colorToParse.G == 0)
                {
                    g = "00" + g;
                }
                else
                {
                    g = "0" + g;
                }
            }
            if (colorToParse.B < 100)
            {
                if (colorToParse.B == 0)
                {
                    b = "00" + b;
                }
                else
                {
                    b = "0" + b;
                }
            }
            result = r + g + b;
            return result;
        }
    }
}
