using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace ProjectX
{
    public class AudioProcessor
    {
        private readonly string apikey = "";
        private Process ffplay, ffmpeg;
        private readonly List<string> final_input_cutted = new List<string>();
        private readonly int gender;
        private readonly List<string> linksOfM3u8 = new List<string>();
        private readonly int maxLenghtText = 500;
        private List<string> outputTexts = new List<string>();
        private readonly string path = Directory.GetCurrentDirectory();
        private string processMes = "";
        private Thread ReadingThread, DownloadingThread;
        private readonly string speed = "1.0";
        private string text = "";

        public AudioProcessor(string _text, int _gender = 1, string _speed = "", string _apikey = "")
        {
            gender = _gender;
            text = _text;
            speed = _speed;
            apikey = _apikey;
        }

        public void MainRun()
        {
            ReadingThread = new Thread(() => Read());
            ReadingThread.Start();
        }

        public void MainDown()
        {
            DownloadingThread = new Thread(() => Down());
            DownloadingThread.Start();
        }

        public void Down()
        {
            processMes = "Đang khởi động...";
            DeleteAllFile(path + "\\audio");
            string fname;
            if (text.Length > maxLenghtText)
            {
                linksOfM3u8.Clear();
                var getLink = new Thread(() => GetDataM3u8());
                getLink.Start();
                while (!(linksOfM3u8.Count > 0)) Thread.Sleep(2000);
                var maxdown = outputTexts.Count;
                for (var i = 0; i < maxdown; i++)
                {
                    fname = i.ToString();
                    processMes = "Đang tải file -> " + fname + ".mp3...";
                    Thread.Sleep(2000);
                    DownFileM3U8toMP3(linksOfM3u8.ElementAt(i), fname + ".mp3");
                    processMes = "Đã tải xong file -> " + fname + ".mp3";
                }

                processMes = "Done";
                MessageBox.Show("Đã tải xong\nVui lòng check thư mục audio");
            }
            else
            {
                fname = "output";
                processMes = "Đang tải file -> " + fname + ".mp3...";
                DownFileM3U8toMP3(GetTTS_URL(text), fname + ".mp3");
                processMes = "Đã tải xong file -> " + fname + ".mp3";
                MessageBox.Show("Đã tải xong\nVui lòng check thư mục audio");
            }
        }

        private void DeleteAllFile(string folderPath)
        {
            var di = new DirectoryInfo(folderPath);

            foreach (var file in di.GetFiles()) file.Delete();
            foreach (var dir in di.GetDirectories()) dir.Delete(true);
        }

        public void Read()
        {
            processMes = "Đang khởi động...";
            if (text.Length > maxLenghtText)
            {
                linksOfM3u8.Clear();

                var getLink = new Thread(() => GetDataM3u8());
                getLink.Start();
                while (!(linksOfM3u8.Count > 0)) Thread.Sleep(1000);
                for (var i = 0; i < outputTexts.Count; i++)
                {
                    processMes = "Đang chạy trình phát...";
                    PlayM3U8FromUrl(linksOfM3u8.ElementAt(i));
                }
            }
            else
            {
                processMes = "Đang chạy trình phát...";
                PlayM3U8FromUrl(GetTTS_URL(text));
            }

            processMes = "Đã xong!";
        }

        public void GetDataM3u8()
        {
            var index = 0;
            while (text.Contains("..")) text = text.Replace("..", ".");
            outputTexts = text.Split(new[] { "." }, StringSplitOptions.None).OfType<string>().ToList();
            var doanDuoi2000 = "";
            while (index < outputTexts.Count)
                if (doanDuoi2000.Length + outputTexts.ElementAt(index).Length < 2000)
                {
                    doanDuoi2000 += outputTexts.ElementAt(index) + ".";
                    index += 1;
                }
                else if (doanDuoi2000.Length + outputTexts.ElementAt(index).Length > 2000)
                {
                    final_input_cutted.Add(doanDuoi2000);
                    doanDuoi2000 = "";
                }

            if (doanDuoi2000.Length > 0) final_input_cutted.Add(doanDuoi2000);
            linksOfM3u8.Clear();
            outputTexts.Clear();
            outputTexts = final_input_cutted.ToList();

            foreach (var itemText in final_input_cutted)
            {
                linksOfM3u8.Add(GetTTS_URL(itemText));
                Thread.Sleep(2000);
            }
        }

        public void StopDown()
        {
            try
            {
                ffmpeg?.Kill();
            }
            catch
            {
            }

            try
            {
                DownloadingThread?.Abort();
            }
            catch
            {
            }

            processMes = "Đã dừng tiến trình!";
            MessageBox.Show("Đã dừng tiến trình!");
        }

        public void StopRead()
        {
            try
            {
                ffplay?.Kill();
            }
            catch
            {
            }

            try
            {
                ReadingThread?.Abort();
            }
            catch
            {
            }
        }


        private void PlayM3U8FromUrl(string url)
        {
            var cml = @" -autoexit -nodisp """ + url + @"""";
            ffplay = new Process
            {
                StartInfo =
                {
                    FileName = path + "\\ffplay.exe",
                    Arguments = cml,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = path + "\\audio"
                }
            };

            ffplay.EnableRaisingEvents = true;
            ffplay.OutputDataReceived += (s, e) => Debug.WriteLine(e.Data);
            ffplay.ErrorDataReceived += (s, e) => Debug.WriteLine($@"Error: {e.Data}");
            ffplay.Start();
            ffplay.BeginOutputReadLine();
            ffplay.BeginErrorReadLine();
            ffplay.WaitForExit();
        }

        private string GetTTS_URL(string _text)
        {
            File.WriteAllText(path + "\\zalo_tts\\output.txt", "");
            File.WriteAllText(path + "\\zalo_tts\\text.txt", _text);
            File.WriteAllText(path + "\\zalo_tts\\setting.txt", gender + "|" + speed + "|" + apikey);
            var appPath = path + "\\zalo_tts\\zalo_tts.exe";
            var ffmpeg = new Process
            {
                StartInfo =
                {
                    FileName = appPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = path + "\\zalo_tts"
                }
            };

            ffmpeg.EnableRaisingEvents = true;
            ffmpeg.OutputDataReceived += (s, e) => Debug.WriteLine(e.Data);
            ffmpeg.ErrorDataReceived += (s, e) => Debug.WriteLine($@"Error: {e.Data}");
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            var output = File.ReadAllText(path + "\\zalo_tts\\output.txt");
            if (output.Contains("API rate limit exceeded"))
            {
                MessageBox.Show(
                    "Bạn đã hết giới hạn sử dụng API Key, vui lòng đổi key khác hoặc liên hệ ZaloAI để được hỗ trợ.\nMã lỗi: " +
                    output, "Lỗi");
                Environment.Exit(1);
            }

            var stuff = JObject.Parse(output);
            if (stuff["data"]["url"].ToString().Contains("chunk"))
                return stuff["data"]["url"].ToString();
            return output;
        }

        private void DownFileM3U8toMP3(string url, string saveName = "audio.mp3")
        {
            var cml = @" -i """ + url + @""" -ab 256k """ + saveName + @"""";
            Console.WriteLine(cml);
            ffmpeg = new Process
            {
                StartInfo =
                {
                    FileName = path + "\\ffmpeg.exe",
                    Arguments = cml,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = path + "\\audio"
                }
            };

            ffmpeg.EnableRaisingEvents = true;
            ffmpeg.OutputDataReceived += (s, e) => Debug.WriteLine(e.Data);
            ffmpeg.ErrorDataReceived += (s, e) => Debug.WriteLine($@"Error: {e.Data}");
            ffmpeg.Start();
            ffmpeg.BeginOutputReadLine();
            ffmpeg.BeginErrorReadLine();
            ffmpeg.WaitForExit();
        }


        public string GetProcessMes()
        {
            return processMes;
        }
    }
}