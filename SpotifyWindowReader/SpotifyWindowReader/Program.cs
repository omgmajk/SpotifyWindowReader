using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.IO;
using System.Text;

namespace SpotifyWindowReader
{
    class SpotifyReader
    {
        // Read Spotify Window Title
        public string GetSpotifyTrackInfo()
        {
            var proc = Process.GetProcessesByName("Spotify").FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));

            if (proc == null)
            {
                return "Spotify is not running!";
            }
            // Change "Spotify Premium" to "Spotify" if you are not using premium version.
            if (string.Equals(proc.MainWindowTitle, "Spotify Premium", StringComparison.InvariantCultureIgnoreCase) || string.Equals(proc.MainWindowTitle, "Spotify", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Music paused.";
            }
            else
            {
                return proc.MainWindowTitle;
            }
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            SpotifyReader windowTitle = new SpotifyReader();
            string songTitle;
            string path = @".\Song.txt";
            bool x = true;

            Console.Title = "Spotify Song Reader";
            Console.OutputEncoding = Encoding.UTF8; // Have to set this manually or some songs will register weirdly.

            Console.WriteLine("Song Reader is active and printing to file: Song.txt.");
            Console.WriteLine("Turn off the application by using Ctrl+C or close the window.");

            while (x == true)
            {
                songTitle = windowTitle.GetSpotifyTrackInfo();
                //songTitle = songTitle.Replace("•", string.Empty); // Looks for the system bell character "•"

                Thread.Sleep(800); // 0.8 seconds for now.

                // Write spotify title to file so it can be picked up by OBS
                try
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(songTitle);
                    }
                }
                catch
                {
                    Console.WriteLine("\n\nFile not written, something went wrong...");
                    Console.WriteLine("\n\nPress any key to close the application.");

                    x = false;

                    Console.ReadKey();
                }
            }
        }
    }
}
