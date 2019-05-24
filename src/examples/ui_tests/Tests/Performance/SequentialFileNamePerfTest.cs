using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Medja.Utils.IO;

namespace MedjaOpenGlTestApp.Tests.Performance
{
    public class SequentialFileNamePerfTest
    {
        // default implementation
        // Generating 1000000 names took: 00:00:00.5883297
        // Loading 10000 names took: 00:00:00.2649378
        // Generating 1000000 names took: 00:00:00.5277217
        // Loading 10000 names took: 00:00:00.1668804
        // Generating 1000000 names took: 00:00:00.5444851
        // Loading 10000 names took: 00:00:00.1571033

        // reimplemented the number parsing (no regex, sorting just the numbers in a seperate list)
        // Generating 1000000 names took: 00:00:00.2308543
        // Loading 10000 names took: 00:00:00.1516331
        // Generating 1000000 names took: 00:00:00.2344299
        // Loading 10000 names took: 00:00:00.1343011
        // Generating 1000000 names took: 00:00:00.2442441
        // Loading 10000 names took: 00:00:00.1415609
        
        
        public void Run()
        {
            var sequentialFileNames = new SequentialFileNames("baseName");

            var nameCount = 1000000;
            var fileCount = 10000;
            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < nameCount; i++)
                sequentialFileNames.AddNext();
            
            sw.Stop();
            Console.WriteLine($"Generating {nameCount} names took: {sw.Elapsed}");
            
            sequentialFileNames.Clear();
            
            for (int i = 0; i < fileCount; i++)
                sequentialFileNames.AddNext();

            // copy list because we will modify them eventually
            var createdFileNames = sequentialFileNames.FileNames.ToList();
            
            foreach (var fileName in createdFileNames)
            {
                File.Create(fileName).Dispose();
            }

            sequentialFileNames.Clear();
            
            sw.Restart();
            
            sequentialFileNames.Load();
            
            sw.Stop();
            
            Console.WriteLine($"Loading {createdFileNames.Count} names took: {sw.Elapsed}");
            
            
            foreach (var fileName in createdFileNames)
            {
                File.Delete(fileName);
            }
        }
    }
}