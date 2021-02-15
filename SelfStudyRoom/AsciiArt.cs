using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfStudyRoom
{
    public class AsciiArt
    {
        string[][] numbers = new string[][]{
        new string[] {
            " ######  ",
            "##    ## ",
            "##  #### ",
            "## ## ## ",
            "####  ## ",
            "##    ## ",
            " ######  "
        },
        new string[] {
            "    ##   ",
            "  ####   ",
            "    ##   ",
            "    ##   ",
            "    ##   ",
            "    ##   ",
            " ####### "
        },
        new string[] {
            " ######  ",
            "##    ## ",
            "      ## ",
            " ######  ",
            "##       ",
            "##       ",
            "######## "
        },
        new string[] {
            " ######  ",
            "##    ## ",
            "      ## ",
            " ######  ",
            "      ## ",
            "##    ## ",
            " ######  "
        },
        new string[] {
            "   ##    ",
            "  ##  ## ",
            " ##   ## ",
            "##    ## ",
            "#########",
            "      ## ",
            "      ## "
        },
        new string[] {
            "######## ",
            "##       ",
            "##       ",
            "#######  ",
            "      ## ",
            "##    ## ",
            " ######  "
        },
        new string[] {
            " ######  ",
            "##    ## ",
            "##       ",
            "#######  ",
            "##    ## ",
            "##    ## ",
            " ######  "
        },
        new string[] {
            "######## ",
            "##    ## ",
            "    ##   ",
            "   ##    ",
            "  ##     ",
            "  ##     ",
            "  ##     "
        },
        new string[] {
            " ######  ",
            "##    ## ",
            "##    ## ",
            " ######  ",
            "##    ## ",
            "##    ## ",
            " ######  "
        },
        new string[] {
            " ######  ",
            "##    ## ",
            "##    ## ",
            " ####### ",
            "      ## ",
            "##    ## ",
            " ######  "
        }};
        string[] coloum = { "         ",
                            "  ###    ",
                            "  ###    ",
                            "         ",
                            "  ###    ",
                            "  ###    ",
                            "         " };
        public string AsciiArtNumbers(int num)
        {
            int[] intArray = Array.ConvertAll(num.ToString().ToCharArray(), c => (int)Char.GetNumericValue(c));
            string AsciiArt = "";
            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < intArray.Length; j++)
                {
                    AsciiArt += numbers[intArray[j]][i];
                }
                AsciiArt += "\n";
            }
            return AsciiArt;
        }
        public string AsciiArtTime(int hours, int minutes)
        {
            string asciiArtTime = "";
            string[] H = AsciiArtNumbers(hours).Split('\n');
            string[] M = AsciiArtNumbers(minutes).Split('\n');
            if (hours <= 9)
            {
                for (int i = 0; i < 7; i++)
                {
                    H[i] = numbers[0][i] + H[i];
                }
            }
            if (minutes <= 9)
            {
                for (int i = 0; i < 7; i++)
                {
                    M[i] = numbers[0][i] + M[i];
                }
            }
            for (int i = 0; i < 7; i++)
            {
                asciiArtTime += H[i] + coloum[i] + M[i] + "\n";
            }
            return asciiArtTime;
        }

        public string ProgressBar(int barLength, float percentage)
        {
            int size = Convert.ToInt16(barLength * percentage);
            string progress = "♪";
            if (size > 0)
            {
                progress += string.Concat(Enumerable.Repeat("─", size));
            }
            return progress;
        }
        public static String LimitByteLength(String input, Int32 maxLength)
        {
            return new String(input
                .TakeWhile((c, i) =>
                    Encoding.UTF8.GetByteCount(input.Substring(0, i + 1)) <= maxLength)
                .ToArray());
        }
    }
}
