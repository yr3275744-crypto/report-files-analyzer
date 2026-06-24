using System;
using System.IO;
namespace ReportFilesAnalyzer;

class Manager
{
    static string[]? LoadFile(string? aPath)
    {
        if (File.Exists(aPath))
        {
            string[] reports = File.ReadAllLines(aPath);
            if (reports.Length > 0)
            {
                string[]? nullableReports = reports;
                return nullableReports;
            }
            else
            {
                Console.WriteLine("Empty file.");
                return null;
            }
        }
        else
        {
            Console.WriteLine("The file did not exists.");
            return null;
        }
    }

    static void Main()
    {
        string path = @".\reports.txt";
        string[]? reports = LoadFile(path);
    }
      
}