using LibVLCSharp.Shared;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;



namespace SelfStudyRoom
{

    class VLCPlayer
    {
        bool initialized = false;
        bool isPlayingNotification = false;
        LibVLC libVLC;
        MediaPlayer mediaPlayer;

        int index = 0;
        //string[] musicList;//First Line is used to store where we last ended;
        List<String> musicList = new List<String>();
        List<String> notificationList = new List<String>();
        string rawText;
        string pathToMusic;
        public async Task startMedia()
        {
            Core.Initialize();
            Initialize("music");
            InitializeNotification("notification");
            initialized = true;
            libVLC = new LibVLC();
            mediaPlayer = new MediaPlayer(libVLC);
            SetSong(index);
            mediaPlayer.Play();
            GrabMediaData();
            //mediaPlayer.EndReached += MediaPlayer_EndReached;
        }
        public void Initialize(string path)
        {
            pathToMusic = path;
            //If path directory does not exist
            if (!File.Exists(pathToMusic))
            {
                Directory.CreateDirectory(pathToMusic);
                //Create one
            }
            try
            {
                rawText = File.ReadAllText("musicList.txt"); // relative path
            }
            catch
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);//File from folder
                FileInfo[] Files = directoryInfo.GetFiles();
                string str = "1";
                foreach (FileInfo file in Files)
                {
                    if (file.Extension != ".txt")
                    {
                        str += "\r\n" + file.Name;
                    }
                }
                rawText = str;
                //Read all existing music and write to a new file
                File.WriteAllText("musicList.txt", str);
            }
            musicList = rawText.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            for (int i = musicList.Count - 1; i > -1; i--)
            {
                if (musicList[i] == string.Empty)
                    musicList.RemoveAt(i);
            }
            index = Convert.ToInt32(musicList[0]);

        }
        public void InitializeNotification(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);//File from folder
            FileInfo[] Files = directoryInfo.GetFiles();
            string str = "";
            foreach (FileInfo file in Files)
            {
                if (file.Extension == ".mp3" || file.Extension == ".wav")
                {
                    notificationList.Add(path+ "\\" + file.Name);
                }
            }
        }
        public void SetSong(int musicIndex)
        {
            Pause();
            musicIndex = clampIndex(musicIndex);
            index = musicIndex;
            string songName = musicList[musicIndex];
            string path = pathToMusic + "/" + songName;
            mediaPlayer.Media = new Media(libVLC, path, FromType.FromPath);
            mediaPlayer.Media.AddOption(":no-video");
        }
        public void Play()
        {
            mediaPlayer.Play();
        }
        public void Pause()
        {
            mediaPlayer.Pause();
        }
        public void Exit()
        {
            mediaPlayer.Pause();
            musicList[0] = index.ToString();
            File.WriteAllLines("\\musicList.txt", musicList);
        }
        public void Next()
        {
            mediaPlayer.Pause();
            SetSong(++index);
            Play();
        }
        public void Previous()
        {
            Pause();
            SetSong(--index);
            Play();
        }
        public string GrabMediaData()
        {
            string data = TimeStemp() + "    " + MediaInfo();
            return data;
        }
        public void IfSongEnded()
        {
            if (isPlayingNotification)
            {
                return;
            }
            long songLength = mediaPlayer.Media.Duration;
            if(songLength < 1000)
            {
                return;
            }
            float timeLeft = (1 - PercentagePlayed()) * songLength;
            if (timeLeft <= 500)
            {
                Next();
            }
        }
        public string TimeStemp()
        {
            TimeSpan timePlayed = TimeSpan.FromSeconds(TimePlayed() / 1000);
            TimeSpan totalTime = TimeSpan.FromSeconds(mediaPlayer.Media.Duration / 1000);
            return timePlayed.ToString(@"mm\:ss") + " / " + totalTime.ToString(@"mm\:ss");
        }
        public string MediaInfo()
        {
            string musicData = new string(' ', 50);
            if (mediaPlayer.Position >= 0)
            {
                string musicName = mediaPlayer.Media.Meta(MetadataType.Title);
                string musicArtist = " by " + mediaPlayer.Media.Meta(MetadataType.Artist);
                string musicAlbum = " in [" + mediaPlayer.Media.Meta(MetadataType.Album) + "] ";
                musicData = "" + musicName + musicArtist + musicAlbum;
                if (musicData.Length <= 11)
                {
                    musicData = "";
                }
                int len = Encoding.UTF8.GetByteCount(musicData);
                if (len > 50)
                {
                    musicData = AsciiArt.LimitByteLength(musicData, 50 - 3) + "...";
                    musicData += new string(' ', 50 - Encoding.UTF8.GetByteCount(musicData));
                }
            }
            return musicData;
        }
        public float TimePlayed()
        {
            return PercentagePlayed() * mediaPlayer.Media.Duration;
        }
        public float PercentagePlayed()
        {
            float percentage = 0;
            if (mediaPlayer.Position != -1)
            {
                percentage = mediaPlayer.Position;
            }
            return percentage;
        }
        int clampIndex(int number)
        {
            int length = musicList.Count;
            if (number < 1)
                number = number + length - 1;
            if (number >= length)
                number = number - length + 1;
            return number;
        }
        public void AudioOutput()
        {
            var AudioOutputs = libVLC.AudioOutputs;
            mediaPlayer.SetAudioOutput(AudioOutputs[0].Name);
            //TODO: GET THIS TO WORK
        }
        public void JumpTo(int millisecondsBeforeEnd)
        {

            mediaPlayer.Pause();
            Thread.Sleep(500);
            mediaPlayer.Time = mediaPlayer.Media.Duration - millisecondsBeforeEnd;
            mediaPlayer.Play();
        }
        void MediaPlayer_EndReached(object sender, EventArgs e)
        {
            Next();
        }

        public void Notification()
        {
            Media originalMedia = mediaPlayer.Media;
            int mediaTimePlayed = Convert.ToInt32(TimePlayed());
            var random = new Random();
            string randomNotification = notificationList[random.Next(notificationList.Count)];
            int sleepTime = 100;

            isPlayingNotification = true;

            mediaPlayer.Media = new Media(libVLC, randomNotification, FromType.FromPath);
            mediaPlayer.Media.AddOption(":no-video");
            mediaPlayer.Play();
            Thread.Sleep(100);
            sleepTime = (int)Math.Abs(mediaPlayer.Media.Duration - 100);
            Thread.Sleep(sleepTime);
            mediaPlayer.Media = originalMedia;
            mediaPlayer.Play();
            mediaPlayer.Time = mediaTimePlayed;

            isPlayingNotification = false;
        }
    }
}
