# StudyTimer
Study timer is just a timer program that loads your music. To suit my own needs, I added time calculation to calculate the time of UK West US, and China. This is helpful for us to coordinate our time, you could add ones to suit your own need. The timer just sends notification at certain time to remind the user to take a break for noth efficiency and health concern.

Of course you can just use a timer and some music software to do the samething. I just wanted everything to be automized so I don't have to touch my phone ir computer, a possible source of distraction, all the time.

自习计时器仅仅是一个会在特定地方搜索音乐并播放的计时器软件，通过音乐与计时器能够更好的管理自己的时间，比如到时间起来走走啊。总之为了效率和身体的健康，我觉得这么个东西是有必要的。虽然可以用别的软件来代替它，但我认为去使用电脑、手机、切出学习的画面、都可能导致我分散注意力、所以这个软件仅仅是自动化了这些事而已。

## Important
Due to to size of the VLC library, I deleted the VideoLAN.LibVLC.Windows.3.0.12 folder inside Packages. For visual studio to compile, please reintall the library VideoLAN.LibVLC.Windows in the NuGet package Manager for compilation.

因为VLC的库太大了，所以我删掉了最大的那个才上传的，所以使用的时候请去NuGet那边把VideoLAN.LibVLC.Windows重新安装一次。

## Usage
I tried to make it as simple as possible only to discover that more work then I expected is required. 

The program reads from a file call musicList.txt which remembers which song you are on last time. It reads the song from that musicList.txt, so if you changed the songs in the folder you should probably delete this file and have the program generate another one. Doing this allows me to implement a custom list or a random list in the future.

The songs are saved inside a folder called music, there is no deep first search which means that it only reads them in the folder, not subfolder. 

If they do not exists, the program creates them, other than that, there's not any code to prevent something from going wrong.

When playing type and enter:

1. next for next
2. previous for previous
3. pause for pause
4. play for play
5. exit for saving the song index and exit

尽管试着让他尽可能的简单好用，但我发现那样的话要写的东西实在是太多了。这个程序会从一个叫musicList.txt的地方读取所有的音乐列表以及应该从哪一首开始播放，因此，如果你修改了文件夹里的文件最好直接删掉让程序重新生成这个文件，而这个文件的用途就是以后可以自定义列表或者搞随机列表。所有的音乐都应该放在一个叫做music的文件夹下面，而且没有deep first search，所以只有在这个文件夹而非子文件夹中的文件才有效。这两个文件，如果不存在的话软件会自动生成，其余的保护机制都没有写。

在播放中输入并按回车的话会：

1. next 下一首
2. previous 上一首
3. pause 暂停
4. play 播放
5. exit 保存现在正在播放的歌曲的位置并退出
   

By default there's only one song, feel free to add more to your own list.