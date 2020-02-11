using System;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ReadFileContinously
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private const string LogFile = "test.log";
        private StreamWriter _writer;

        public void Run()
        {
            File.Delete(LogFile);

            FileCreationWatcher.WaitFor(LogFile, OnLogFileCreated);
            
            var watcher = new FileCreationWatcher(LogFile);
            watcher.Created += 
            
            using (_writer = new StreamWriter(LogFile))
            {
                _writer.AutoFlush = true;

                var timer = new Timer();
                timer.Interval = 1000;
                timer.Elapsed += OnTimerElapsed;
                timer.Start();
                
                WatchFileForUpdates(10000);
                
                timer.Dispose();
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void WatchFileForUpdates(int durationMs)
        {
            using (var fileFollowReader = new FileFollowReader(LogFile))
            {
                fileFollowReader.DataRead += (s, e) => Console.Write(e.Data);
                fileFollowReader.Start();
                
                Thread.Sleep(durationMs);
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _writer.WriteLine(DateTime.Now + ": Timer tick");
        }
    }
}