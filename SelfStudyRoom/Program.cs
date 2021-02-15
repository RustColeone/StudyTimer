using System;
using System.Globalization;
using System.Timers;
using System.Threading;
using System.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfStudyRoom
{
    class Program
    {
        static int initialPositionY = Console.CursorTop;
        static int initialPositionX = Console.CursorLeft;
        static void Main(string[] args)
        {
            VLCPlayer vlcPlayer = new VLCPlayer();
            vlcPlayer.startMedia();
            //vlcPlayer.AudioOutput();
            int originalWidth = Console.WindowWidth;
            int originalHeight = Console.WindowHeight;

            int x = initialPositionX;
            int y = initialPositionY;
            if(y > 11)
            {
                y -= 11;
            }
            Console.Clear();
            Console.WriteLine(new string('\n',11));
            Console.OutputEncoding = Encoding.Unicode;
            AsciiArt asciiArt = new AsciiArt();

            int previousTime = DateTime.Now.Second;
            string mediaData = "";
            float percentagePlayed = 0;
            Thread displayThread = new Thread(() =>
            {
                while (true)
                {
                    if (Console.WindowWidth != originalWidth)
                    {
                        originalWidth = Console.WindowWidth;
                        ClearInRegion(x, y, Console.WindowWidth, Console.WindowHeight);
                    }
                    if (Console.WindowHeight != originalHeight)
                    {
                        originalHeight = Console.WindowHeight;
                        ClearInRegion(x, y, Console.WindowWidth, Console.WindowHeight);
                        y = Console.WindowHeight - originalHeight;
                    }
                    if (previousTime != DateTime.Now.Second)
                    {
                        percentagePlayed = vlcPlayer.PercentagePlayed();
                        mediaData = vlcPlayer.GrabMediaData();

                        previousTime = DateTime.Now.Second;
                        WriteInRegion(x + 3, y, "SELF STUDY ROOM", ConsoleColor.Red);
                        WriteInRegion(x + 3, y + 1, "├─\n├─\n├─\n│\n│\n│", ConsoleColor.Red);
                        WriteInRegion(x + 5, y + 1, GetDateTime(), ConsoleColor.White);
                        WriteInRegion(x + 5, y + 5, "Study / Break\n45min / 15min", ConsoleColor.White);
                        WriteInRegion(x + 3, y + 7, "├─────────────────────────\n└─ \n", ConsoleColor.Red);
                        if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                        {
                            vlcPlayer.Notification();
                        }
                        if(DateTime.Now.Minute == 45 && DateTime.Now.Second == 00)
                        {
                            vlcPlayer.Notification();
                        }
                        if (DateTime.Now.Minute < 45)
                        {
                            WriteInRegion(x + 59, y + 10, "好好学习冲啊啊啊", ConsoleColor.White);
                        }
                        else
                        {
                            WriteInRegion(x + 59, y + 10, "稍微起来喝点水吧", ConsoleColor.Cyan);
                        }
                        Clock(x + 30, y + 1, DateTime.Now);
                        musicProgressBar(percentagePlayed, mediaData, x + 6, y + 8);
                        //WriteInRegion(initialPositionX + 5, initialPositionY + 10, ProgressBar(percentagePlayed), ConsoleColor.White);
                    }
                    vlcPlayer.IfSongEnded();
                }
            });
            displayThread.Start();
            string userInput = "";
            while (true)
            {
                try
                {
                    Console.SetCursorPosition(x, y + 11);
                } catch { continue; }
                userInput = Console.ReadLine();
                if (userInput == "next")
                {
                    vlcPlayer.Next();
                    ClearInRegion(x, y + 10, 10, 2);
                }
                else if (userInput == "previous")
                {
                    vlcPlayer.Previous();
                    ClearInRegion(x, y + 10, 10, 2);
                }
                else if (userInput == "pause")
                {
                    vlcPlayer.Pause();
                    ClearInRegion(x, y + 10, 10, 2);
                }
                else if (userInput == "play")
                {
                    vlcPlayer.Play();
                    ClearInRegion(x, y + 10, 10, 2);
                }
                else if (userInput == "exit")
                {
                    vlcPlayer.Exit();
                    displayThread.Abort();
                    ClearInRegion(x, y + 10, 10, 2);
                    break;
                }
            }

        }
        static string GetDateTime()
        {
            DateTime localDate = DateTime.Now;
            DateTime utcDate = DateTime.UtcNow;
            String[] systemTimeZoneID = { "Pacific Standard Time", "GMT Standard Time", "China Standard Time" };
            String[] systemTimeZonePlace = { "US-W ", "UK   ", "CN   " };
            string times = "";
            for (int i = 0; i < 3; i ++)
            {
                DateTime UTCTime = DateTime.UtcNow;
                TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(systemTimeZoneID[i]);
                DateTime tstTime = TimeZoneInfo.ConvertTime(UTCTime, TimeZoneInfo.Utc, tst);
                //Console.WriteLine(tstTime.ToString("MM/dd/yyyy HH:mm"));
                times += systemTimeZonePlace[i] + tstTime.ToString("MM/dd/yy HH:mm") + "\n";
            }
            return times;
        }
        static void WriteInRegion(int x, int y, string rawText, ConsoleColor consoleColor)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            int curTop = Console.CursorTop;
            int curLeft = Console.CursorLeft;
            string[] textLines = rawText.Split('\n');
            for (int i = 0; i < textLines.Length; i ++)
            {
                try
                {
                    Console.SetCursorPosition(x, y + i);
                    Console.Write(textLines[i]);
                }
                catch
                {
                    continue;
                }
            }
            Console.SetCursorPosition(curLeft, curTop);
            Console.ForegroundColor = originalColor;
        }
        static void ClearInRegion(int x, int y, int width, int height)
        {
            int curTop = Console.CursorTop;
            int curLeft = Console.CursorLeft;
            for (; height > 0;)
            {
                try { 
                    Console.SetCursorPosition(x, y + --height);
                    Console.Write(new string(' ', width));
                }
                catch
                {
                    continue;
                }
            }
            Console.SetCursorPosition(curLeft, curTop);
        }
        static void WriteDigit(int x, int y, int asciiArtDigit, ConsoleColor consoleColor)
        {
            ClearInRegion(x, y, 9, 7);
            AsciiArt asciiArt = new AsciiArt();
            WriteInRegion(x, y, asciiArt.AsciiArtNumbers(asciiArtDigit), consoleColor);
        }
        static void Clock(int x, int y, DateTime now)
        {
            AsciiArt asciiArt = new AsciiArt();
            ConsoleColor color = ConsoleColor.DarkCyan;
            if (now.Minute >= 45)
            {
                color = ConsoleColor.DarkYellow;
            }
            WriteInRegion(x, y, asciiArt.AsciiArtTime(now.Minute, now.Second), color);
        }
        static void musicProgressBar(float percentagePlayed, string musicData, int x, int y)
        {
            AsciiArt asciiArt = new AsciiArt();
            //int barWidth = Math.Abs(Console.WindowWidth - 53);
            int barWidth = 65;
            string progress = asciiArt.ProgressBar(barWidth, percentagePlayed);

            WriteInRegion(x, y, "\n" + "│\n" + "└─ \n", ConsoleColor.Red);
            WriteInRegion(x, y, musicData, ConsoleColor.Red);
            WriteInRegion(x + 2, y + 1, new string('─', barWidth + 1) + "┤", ConsoleColor.Gray);
            WriteInRegion(x + 2, y + 1, progress, ConsoleColor.Red);

            string percentageProgressed = Convert.ToInt16(percentagePlayed * 100) + "% progressed ";
            if (percentagePlayed * 100 <= 9 && percentagePlayed >= 0)
            {
                percentageProgressed = "0" + percentageProgressed;
            }
            WriteInRegion(x + 4, y + 2, percentageProgressed, ConsoleColor.Red);
            Thread.Sleep(250);
        }
        static void beepMario()
        {
            Action beep;
            beep = delegate ()
            {
                Console.Beep(659, 125);
                Console.Beep(659, 125);
                Thread.Sleep(125);
                Console.Beep(659, 125);
                Thread.Sleep(167);
                Console.Beep(523, 125);
                Console.Beep(659, 125);
                Thread.Sleep(125);
                Console.Beep(784, 125);
                Thread.Sleep(375); 
                Console.Beep(392, 125); 
                Thread.Sleep(375); 
                Console.Beep(523, 125); 
                Thread.Sleep(250); 
                Console.Beep(392, 125); 
                Thread.Sleep(250); 
                Console.Beep(330, 125); 
                Thread.Sleep(250); 
                Console.Beep(440, 125); 
                Thread.Sleep(125); 
                Console.Beep(494, 125); 
                Thread.Sleep(125); 
                Console.Beep(466, 125); 
                Thread.Sleep(42); 
                Console.Beep(440, 125); 
                Thread.Sleep(125); 
                Console.Beep(392, 125); 
                Thread.Sleep(125); 
                Console.Beep(659, 125); 
                Thread.Sleep(125); 
                Console.Beep(784, 125);
                Thread.Sleep(125); 
                Console.Beep(880, 125); 
                Thread.Sleep(125); 
                Console.Beep(698, 125); 
                Console.Beep(784, 125); 
                Thread.Sleep(125); 
                Console.Beep(659, 125); 
                Thread.Sleep(125); 
                Console.Beep(523, 125); 
                Thread.Sleep(125); 
                Console.Beep(587, 125); 
                Console.Beep(494, 125);
            };
            beep.BeginInvoke((a) => { beep.EndInvoke(a); }, null);
        }
    }
}
