using System.Diagnostics;

namespace youtube_dl_ez
{
    internal class YouTube_dl_ez
    {
        static void Main(string[] args)
        {
            String ytURL;
            int downloadOption;

            Console.WriteLine("Input video URL: ");
            ytURL = Console.ReadLine();
            if (ytURL == null) return;
            Console.Clear();

            Console.WriteLine("Select download option: ");
            Console.WriteLine("1 -> MP4 (Video)\n2 -> MP3 (Audio)");
            try
            {
                downloadOption = Int32.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.Clear();

            switch (downloadOption) // ADD PLAYLIST AND THUMBNAIL DOWNLOAD
            {
                case 1:
                    standardVideoDownload(ytURL);
                    return;
                case 2:
                    standardAudioDownload(ytURL);
                    return;
                default:
                    Console.WriteLine("Invalid download option input!!!");
                    Console.ReadKey();
                    return;
            }
        }





        static void standardVideoDownload(String ytURL)
        {
            int qualityInput;
            String quality;

            Console.WriteLine("Loading available video qualities...");

            String arg0 = $"-F {ytURL} --skip-download --no-playlist";
            Process p0 = new Process();
            p0.StartInfo.FileName = @".\yt-dlp";
            p0.StartInfo.Arguments = arg0;
            p0.StartInfo.RedirectStandardOutput = true;
            p0.StartInfo.RedirectStandardError = true;
            string eOut = null;
            p0.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>{ eOut += e.Data; });
            p0.Start();
            p0.BeginErrorReadLine();
            string stdout = p0.StandardOutput.ReadToEnd();
            p0.WaitForExit();


            Console.Clear();
            List<String> availableQ = new List<string>(); 
            bool mod1 = false;
            bool mod2 = false;
            Console.WriteLine("Select video quality: ");
            //if (stdout.Contains("701 mp4")) availableQ.Add("701"); // 4K
            //if (stdout.Contains("700 mp4")) availableQ.Add("700"); // 2K
            if (stdout.Contains("299 mp4")) { mod1 = true; availableQ.Add("299"); } // 1080p60
            if (stdout.Contains("298 mp4")) { mod2 = true; availableQ.Add("298"); } // 720p60
            if (stdout.Contains("137 mp4") && !mod1) availableQ.Add("137"); // 1080p
            if (stdout.Contains("136 mp4") && !mod2) availableQ.Add("136"); // 720p
            if (stdout.Contains("135 mp4")) availableQ.Add("135"); // 480p
            if (stdout.Contains("134 mp4")) availableQ.Add("134"); // 360p
            if (stdout.Contains("133 mp4")) availableQ.Add("133"); // 240p
            if (stdout.Contains("160 mp4")) availableQ.Add("160"); // 144p
            if (availableQ.Count == 0)
            {
                Console.WriteLine("ERROR: Video unavailable!!!");
                Console.ReadKey();
                return;
            } 

            int currentQCounter = 0; 
            foreach (String Q in availableQ)
            {
                switch (Q)
                {
                    /*case "701":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 4K");
                        break;
                    case "700":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 2K");
                        break;*/
                    case "299":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 1080p60");
                        break;
                    case "298":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 720p60");
                        break;
                    case "137":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 1080p");
                        break;
                    case "136":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 720p");
                        break;
                    case "135":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 480p");
                        break;
                    case "134":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 360p");
                        break;
                    case "133":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 240p");
                        break;
                    case "160":
                        currentQCounter++;
                        Console.WriteLine($"{currentQCounter} -> 144p");
                        break;
                    default:
                        Console.WriteLine("ERROR: Invalid quality value processed!!!");
                        Console.ReadKey();
                        return;
                }
            }


            try { qualityInput = Int32.Parse(Console.ReadLine()); }
            catch (Exception ex) { Console.WriteLine(ex.Message); return; }

            try { quality = availableQ[qualityInput - 1]; }
            catch { Console.WriteLine("Invalid quality input!!!"); return; }

            Console.Clear();


            String arg1 = $"{ytURL} -f {quality} --no-playlist -r 1000M -R 10 --write-description --write-sub --sub-langs en --add-metadata --embed-chapters";
            Process p1 = new Process();
            p1.StartInfo.FileName = @".\yt-dlp";
            p1.StartInfo.Arguments = arg1;
            p1.Start();
            p1.WaitForExit();

            Console.Write("\n");


            var txtFiles = Directory.EnumerateFiles(".\\", "*.mp4");
            List<string> files = new List<string>();

            foreach (string currentFile in txtFiles)
            {
                string fileName = currentFile.Substring(2);
                files.Add(fileName);
            }

            String fileNameLatestCreationTime = "";
            DateTime latestCreationTime = new DateTime(2000, 1, 1);

            foreach (string file in files)
            {
                DateTime currentCreationTime = File.GetCreationTime(file);
                if (currentCreationTime > latestCreationTime)
                {
                    latestCreationTime = currentCreationTime;
                    fileNameLatestCreationTime = file;
                }
            }

            try
            {
                fileNameLatestCreationTime = fileNameLatestCreationTime.Substring(0, fileNameLatestCreationTime.Length - 4);
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to download .mp4 file!!!");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{fileNameLatestCreationTime}.mp4"))
            {
                Console.WriteLine("ERROR: Unable to download .mp4 file!!!");
                Console.ReadKey();
                return;
            }


            String arg15 = $"{ytURL} -f {quality} --write-thumbnail --no-playlist --skip-download";
            Process p15 = new Process();
            p15.StartInfo.FileName = @".\youtube-dl";
            p15.StartInfo.Arguments = arg15;
            p15.StartInfo.RedirectStandardError = true;
            p15.Start();
            p15.WaitForExit();
            Console.Write("\n");


            String arg2 = $"{ytURL} -x --audio-format mp3 --audio-quality 0 --no-playlist";
            Process p2 = new Process();
            p2.StartInfo.FileName = @".\yt-dlp";
            p2.StartInfo.Arguments = arg2;
            p2.Start();
            p2.WaitForExit();
            Console.Write("\n");


            String arg3 = $"-i \"{fileNameLatestCreationTime}.mp4\" -i \"{fileNameLatestCreationTime}.mp3\" -c copy -map 0:v:0 -map 1:a:0 \"{fileNameLatestCreationTime}-temp.mp4\" -y";
            Process p3 = new Process();
            p3.StartInfo.FileName = @".\ffmpeg";
            p3.StartInfo.Arguments = arg3;
            p3.Start();
            p3.WaitForExit();


            var webpFiles = Directory.EnumerateFiles(".\\", "*.webp");

            foreach (string currentFile in webpFiles)
            {
                try
                {
                    if (currentFile.Substring(currentFile.Length - 16, 11) == fileNameLatestCreationTime.Substring(fileNameLatestCreationTime.Length - 12, 11))
                    {
                        File.Copy($"{currentFile}", $"{fileNameLatestCreationTime}.webp", true);
                        File.Delete(currentFile);
                    }
                }
                catch { }
            }

            var jpgFiles = Directory.EnumerateFiles(".\\", "*.jpg");

            foreach (string currentFile in jpgFiles)
            {
                try
                {
                    if (currentFile.Substring(currentFile.Length - 15, 11) == fileNameLatestCreationTime.Substring(fileNameLatestCreationTime.Length - 12, 11))
                    {
                        File.Copy($"{currentFile}", $"{fileNameLatestCreationTime}.jpg", true);
                        File.Delete(currentFile);
                    }
                }
                catch { }
            }


            bool noThumbnail = false;
            if (File.Exists($"{fileNameLatestCreationTime}.jpg"))
            {
                File.Copy($"{fileNameLatestCreationTime}.jpg", $"{fileNameLatestCreationTime}.webp", true);
                File.Delete($"{fileNameLatestCreationTime}.jpg");

                String arg4 = $"-format JPG *.webp";
                Process p4 = new Process();
                p4.StartInfo.FileName = @".\mogrify";
                p4.StartInfo.Arguments = arg4;
                p4.Start();
                p4.WaitForExit();
            }
            else if (File.Exists($"{fileNameLatestCreationTime}.webp"))
            {
                String arg4 = $"-format jpg *.webp";
                Process p4 = new Process();
                p4.StartInfo.FileName = @".\mogrify";
                p4.StartInfo.Arguments = arg4;
                p4.Start();
                p4.WaitForExit();
            }
            else 
            {
                Console.Write("\n\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("WARNING: Unable to process thumbnail!!!");
                Console.ResetColor();
                Console.Write("\n\n");

                noThumbnail = true;
            }


            var descArray = File.ReadAllText($".\\{fileNameLatestCreationTime}.description");
            String desc = String.Join('\n', descArray);
            desc = desc.Replace("\"", "''");

            String arg5;
            if (!noThumbnail)
            {
                arg5 = $"-i \"{fileNameLatestCreationTime}-temp.mp4\" -i \"{fileNameLatestCreationTime}.jpg\" -map 1 -map 0 -c copy -disposition:0 attached_pic -metadata comment=\"{desc}\" \"{fileNameLatestCreationTime}.mp4\" -y";
            }
            else
            {
                arg5 = $"-i \"{fileNameLatestCreationTime}-temp.mp4\" -c copy -metadata comment=\"{desc}\" \"{fileNameLatestCreationTime}.mp4\" -y";
            }

            Process p5 = new Process();
            p5.StartInfo.FileName = @".\ffmpeg";
            p5.StartInfo.Arguments = arg5;
            p5.Start();
            p5.WaitForExit();


            Directory.CreateDirectory(@".\Downloads\");
            File.Move($"{fileNameLatestCreationTime}.mp4", $".\\downloads\\{fileNameLatestCreationTime.Substring(0, fileNameLatestCreationTime.Length - 14)}.mp4", true);
            if (File.Exists($"{fileNameLatestCreationTime}.en.vtt"))
            {
                File.Move($"{fileNameLatestCreationTime}.en.vtt", $".\\downloads\\{fileNameLatestCreationTime.Substring(0, fileNameLatestCreationTime.Length - 14)}.en.vtt", true);
            }
            File.Delete($"{fileNameLatestCreationTime}-temp.mp4");
            File.Delete($"{fileNameLatestCreationTime}.mp3");
            File.Delete($"{fileNameLatestCreationTime}.webp");
            File.Delete($"{fileNameLatestCreationTime}.jpg");
            File.Delete($"{fileNameLatestCreationTime}.description");


            Console.WriteLine("\nVideo Download Complete!!!");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Made by exosZoldyck");
            Console.ResetColor();
            return;
        }





        static void standardAudioDownload(String ytURL)
        {
            String arg1 = $"{ytURL} -x --audio-format mp3 --audio-quality 0 --no-playlist";
            Process p1 = new Process();
            p1.StartInfo.FileName = @".\yt-dlp";
            p1.StartInfo.Arguments = arg1;
            p1.StartInfo.RedirectStandardError = true;
            p1.Start();
            p1.WaitForExit();

            string stderr = p1.StandardError.ReadToEnd();
            if (stderr.Contains("requested format not available"))
            {
                Console.WriteLine($"\n{stderr}");
                Console.ReadKey();
                return;
            }
            else if (stderr.Contains("Video unavailable"))
            {
                Console.WriteLine($"\n{stderr}");
                Console.ReadKey();
                return;
            }
            Console.Write("\n");


            var txtFiles = Directory.EnumerateFiles(".\\", "*.mp3");
            List<string> files = new List<string>();

            foreach (string currentFile in txtFiles)
            {
                string fileName = currentFile.Substring(2);
                files.Add(fileName);
            }

            String fileNameLatestCreationTime = "";
            DateTime latestCreationTime = new DateTime(2000, 1, 1);

            foreach (string file in files)
            {
                DateTime currentCreationTime = File.GetCreationTime(file);
                if (currentCreationTime > latestCreationTime)
                {
                    latestCreationTime = currentCreationTime;
                    fileNameLatestCreationTime = file;
                }
            }

            try
            {
                fileNameLatestCreationTime = fileNameLatestCreationTime.Substring(0, fileNameLatestCreationTime.Length - 4);
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to download.mp3 file!!!");
                Console.ReadKey();
                return;
            }

            if (!File.Exists($"{fileNameLatestCreationTime}.mp3"))
            {
                Console.WriteLine("ERROR: Unable to download .mp3 file!!!");
                Console.ReadKey();
                return;
            }


            String arg2 = $"{ytURL} --write-thumbnail --no-playlist --skip-download";
            Process p2 = new Process();
            p2.StartInfo.FileName = @".\youtube-dl";
            p2.StartInfo.Arguments = arg2;
            p2.StartInfo.RedirectStandardError = true;
            p2.Start();
            p2.WaitForExit();
            Console.Write("\n");


            var webpFiles = Directory.EnumerateFiles(".\\", "*.webp");

            foreach (string currentFile in webpFiles)
            {
                try
                {
                    if (currentFile.Substring(currentFile.Length - 16, 11) == fileNameLatestCreationTime.Substring(fileNameLatestCreationTime.Length - 12, 11))
                    {
                        File.Copy($"{currentFile}", $"{fileNameLatestCreationTime}.webp", true);
                        File.Delete(currentFile);
                    }
                }
                catch { }
            }

            var jpgFiles = Directory.EnumerateFiles(".\\", "*.jpg");

            foreach (string currentFile in jpgFiles)
            {
                try
                {
                    if (currentFile.Substring(currentFile.Length - 15, 11) == fileNameLatestCreationTime.Substring(fileNameLatestCreationTime.Length - 12, 11))
                    {
                        File.Copy($"{currentFile}", $"{fileNameLatestCreationTime}.jpg", true);
                        File.Delete(currentFile);
                    }
                }
                catch { }
            }


            if (File.Exists($"{fileNameLatestCreationTime}.jpg"))
            {
                File.Copy($"{fileNameLatestCreationTime}.jpg", $"{fileNameLatestCreationTime}.webp", true);
                File.Delete($"{fileNameLatestCreationTime}.jpg");

                String arg4 = $"-format JPG *.webp";
                Process p4 = new Process();
                p4.StartInfo.FileName = @".\mogrify";
                p4.StartInfo.Arguments = arg4;
                p4.Start();
                p4.WaitForExit();
            }
            else if (File.Exists($"{fileNameLatestCreationTime}.webp"))
            {
                String arg4 = $"-format JPG *.webp";
                Process p4 = new Process();
                p4.StartInfo.FileName = @".\mogrify";
                p4.StartInfo.Arguments = arg4;
                p4.Start();
                p4.WaitForExit();
            }


            String arg5 = $"-i \"{fileNameLatestCreationTime}.mp3\" -i \"{fileNameLatestCreationTime}.jpg\" -map 0:0 -map 1:0 -codec copy -id3v2_version 3 -metadata:s:v title=\"Album cover\" -metadata:s:v comment=\"Cover (front)\" \"{fileNameLatestCreationTime}-temp.mp3\" -y";
            Process p5 = new Process();
            p5.StartInfo.FileName = @".\ffmpeg";
            p5.StartInfo.Arguments = arg5;
            p5.Start();
            p5.WaitForExit();


            try
            {
                File.Move($"{fileNameLatestCreationTime}-temp.mp3", $".\\downloads\\{fileNameLatestCreationTime.Substring(0, fileNameLatestCreationTime.Length - 14)}.mp3");
            }
            catch
            {
                try
                {
                    File.Move($"{fileNameLatestCreationTime}.mp3", $".\\downloads\\{fileNameLatestCreationTime.Substring(0, fileNameLatestCreationTime.Length - 14)}.mp3", true);
                }
                catch
                {
                    Console.WriteLine("Critical Error!!!");
                    Console.ReadKey();
                    return;
                }
            }
            File.Delete($"{fileNameLatestCreationTime}-temp.mp3");
            File.Delete($"{fileNameLatestCreationTime}.mp3");
            File.Delete($"{fileNameLatestCreationTime}.webp");
            File.Delete($"{fileNameLatestCreationTime}.jpg");


            Console.WriteLine("\nVideo Download Complete!!!");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Made by exosZoldyck");
            Console.ResetColor();
            return;
        }
    }
}