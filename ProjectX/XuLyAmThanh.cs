using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace ProjectX
{
    public class XuLyAmThanh
    {
        List<string> linksOfM3u8 = new List<string>();
        List<string> outputTexts = new List<string>();
        List<string> final_input_cutted = new List<string>();
        string[] outputTexts2;
        private int gender = 0;
        private string apikey = "";
        private string speed = "1.0";
        private string text = "";
        private string processMes = "";
        private int processNow = 0;
        private int processFull = 0;
        private Process ffplay, ffmpeg;
        private int maxLenghtText = 500;
        private Thread ReadingThread, DownloadingThread;
        string path = Directory.GetCurrentDirectory();
        public XuLyAmThanh(string _text, int _gender = 1, string _speed = "", string _apikey = "")
        {
            this.gender = _gender;
            this.text = _text;
            this.speed = _speed;
            this.apikey = _apikey;
        }
        public void mainRun()
        {
            ReadingThread = new Thread(() => Read());
            ReadingThread.Start();
        }
        public void mainDown()
        {

            DownloadingThread = new Thread(() => Down());
            DownloadingThread.Start();
        }
        public void Down()
        {
            this.processMes = "Đang khởi động...";
            DeleteAllFile(path + "\\audio");
            this.processNow = 0;
            string fname;
            if (text.Length > maxLenghtText)
            {

                linksOfM3u8.Clear();
                Thread getLink = new Thread(() => GetDataM3u8());
                getLink.Start();
                while (!(linksOfM3u8.Count > 0))
                {
                    Thread.Sleep(2000);
                }
                int maxdown = outputTexts.Count;
                for (int i = 0; i < maxdown; i++)
                {
                    fname = i.ToString();
                    this.processNow = (i + 1) * 100 / (maxdown + 1);
                    this.processMes = "Đang tải file -> " + fname + ".mp3...";
                    Thread.Sleep(2000);
                    DownFileM3U8toMP3(linksOfM3u8.ElementAt(i), fname + ".mp3");
                    this.processMes = "Đã tải xong file -> " + fname + ".mp3";

                }
                this.processMes = "Done";
                this.processNow = 100;
                MessageBox.Show("Đã tải xong\nVui lòng check thư mục audio");
            }
            else
            {
                fname = "output";
                this.processMes = "Đang tải file -> " + fname + ".mp3...";
                DownFileM3U8toMP3(getTTS_URL(text), fname + ".mp3");
                this.processNow = 100;
                this.processMes = "Đã tải xong file -> " + fname + ".mp3";
                MessageBox.Show("Đã tải xong\nVui lòng check thư mục audio");
            }
        }
        private void DeleteAllFile(string folderPath)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

        }
        public void Read()
        {
            this.processMes = "Đang khởi động...";
            if (text.Length > maxLenghtText)
            {
                linksOfM3u8.Clear();

                Thread getLink = new Thread(() => GetDataM3u8());
                getLink.Start();
                while (!(linksOfM3u8.Count > 0))
                {
                    Thread.Sleep(1000);
                }
                for (int i = 0; i < outputTexts.Count; i++)
                {
                    this.processMes = "Đang chạy trình phát...";
                    PlayM3U8FromUrl(linksOfM3u8.ElementAt(i));
                }

            }
            else
            {
                this.processMes = "Đang chạy trình phát...";
                PlayM3U8FromUrl(getTTS_URL(text));
            }
            this.processMes = "Đã xong!";
        }
        public void GetDataM3u8()
        {
            int index = 0;
            while (text.Contains(".."))
            {
                text = text.Replace("..", ".");
            }
            outputTexts = text.Split(new[] { "." }, StringSplitOptions.None).OfType<string>().ToList();
            string doanDuoi2000 = "";
            while (index < outputTexts.Count)
            {
                if ((doanDuoi2000.Length + outputTexts.ElementAt(index).Length) < 2000)
                {

                    doanDuoi2000 += outputTexts.ElementAt(index) + ".";
                    index += 1;
                }
                else if ((doanDuoi2000.Length + outputTexts.ElementAt(index).Length) > 2000)
                {
                    final_input_cutted.Add(doanDuoi2000);
                    doanDuoi2000 = "";
                }
            }
            if (doanDuoi2000.Length > 0)
            {
                final_input_cutted.Add(doanDuoi2000);
            }
            linksOfM3u8.Clear();
            outputTexts.Clear();
            outputTexts = final_input_cutted.ToList();

            foreach (string itemText in final_input_cutted)
            {
                linksOfM3u8.Add(getTTS_URL(itemText));
                Thread.Sleep(2000);
            }
        }
        public void StopDown()
        {
            try
            {
                if (ffmpeg != null)
                {
                    ffmpeg.Kill();
                }

            }
            catch { }
            try
            {
                if (DownloadingThread != null)
                {
                    DownloadingThread.Abort();
                }


            }
            catch { }
            this.processNow = 100;
            this.processMes = "Đã dừng tiến trình!";
            MessageBox.Show("Đã dừng tiến trình!");
        }
        public void StopRead()
        {
            try
            {
                if (ffplay != null)
                {
                    ffplay.Kill();
                }

            }
            catch { }
            try
            {
                if (ReadingThread != null)
                {
                    ReadingThread.Abort();
                }


            }
            catch { }
        }
        public bool CheckReadDone()
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains("ffplay"))
                {
                    return true;
                }
            }
            return false;
        }

        private void PlayM3U8FromUrl(string url)
        {
            string cml = @" -autoexit -nodisp """ + url + @"""";
            ffplay = new Process
            {
                StartInfo = {
                    FileName = path+"\\ffplay.exe",
                    Arguments = cml,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = path+"\\audio"
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
        private string getTTS_URL(string _text)
        {
            File.WriteAllText(path + "\\zalo_tts\\output.txt", "");
            File.WriteAllText(path + "\\zalo_tts\\text.txt", _text);
            File.WriteAllText(path + "\\zalo_tts\\setting.txt", gender + "|" + speed + "|" + apikey);
            //var process = Process.Start(path + "\\zalo_tts\\zalo_tts.exe");
            string appPath = path + "\\zalo_tts\\zalo_tts.exe";
            Process ffmpeg = new Process
            {
                StartInfo = {
                    FileName = appPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = path+"\\zalo_tts"
                }
            };

            ffmpeg.EnableRaisingEvents = true;
            ffmpeg.OutputDataReceived += (s, e) => Debug.WriteLine(e.Data);
            ffmpeg.ErrorDataReceived += (s, e) => Debug.WriteLine($@"Error: {e.Data}");
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            string output = System.IO.File.ReadAllText(path + "\\zalo_tts\\output.txt");
            if (output.Contains("API rate limit exceeded"))
            {
                MessageBox.Show("Bạn đã hết giới hạn sử dụng API Key, vui lòng đổi key khác hoặc liên hệ ZaloAI để được hỗ trợ.\nMã lỗi: " + output, "Lỗi");
                System.Environment.Exit(1);
            }
            var stuff = JObject.Parse(output);
            if (stuff["data"]["url"].ToString().Contains("chunk"))
            {
                return stuff["data"]["url"].ToString();
            }
            else
            {
                return output;
            }

        }
        private void DownFileM3U8toMP3(string url, string saveName = "audio.mp3")
        {
            string cml = @" -i """ + url + @""" -ab 256k """ + saveName + @"""";
            Console.WriteLine(cml);
            ffmpeg = new Process
            {
                StartInfo = {
                    FileName = path+"\\ffmpeg.exe",
                    Arguments = cml,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = path+"\\audio"
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

        public int getProcessNow()
        {
            return this.processNow;
        }
        public string getProcessMes()
        {
            return processMes;
        }
        string getText()
        {
            return this.text;
        }
        void setText(string _text)
        {
            this.text = _text;
        }
        string getSpeed()
        {
            return this.speed;
        }
        void setSpeed(string _speed)
        {
            this.speed = _speed;
        }

        public List<string> SplitStringEveryNth(string input, int chunkSize)
        {
            var output = new List<string>();
            var flag = chunkSize;
            var tempString = string.Empty;
            var lenght = input.Length;

            for (var i = 0; i < lenght; i++)
            {
                if (Int32.Equals(flag, 0))
                {
                    output.Add(tempString);
                    tempString = string.Empty;
                    flag = chunkSize;
                }
                else
                {
                    tempString += input[i];
                    flag--;
                }

                if ((input.Length - 1) == i && flag != 0)
                {
                    tempString += input[i];
                    output.Add(tempString);
                }
            }
            return output;
        }


    }
}