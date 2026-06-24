using System;
using System.IO;
namespace ReportFilesAnalyzer;

enum ReportType
{
    Intel,
    Recon,
    Analyze,
    Collect
}

enum Status
{
    Pending,
    Approved,
    Rejected
}

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

    static int ProcessReports(string[] unitNames, ReportType[] reportTypes, int[] priorities, double[] scores, Status[] statuses, string[] reports)
    {
        for (int i = 0; i < reports.Length; i ++)
        {
            string[] report = reports[i].Split(',');
            //foreach (string val in report)
            //{
            //    Console.WriteLine(val);
            //}
        }
        return 1;
    }

    static void Main()
    {
        string path = @".\reports.txt";
        string[]? nullableReports = LoadFile(path);
        if (nullableReports is null)
        {
            return;
        }
        string[] reports = nullableReports;
        int reportsNumber = reports.Length;
        string[] unitNames = new string[reportsNumber];
        ReportType[] reportTypes = new ReportType[reportsNumber];
        int[] priorities = new int[reportsNumber];
        double[] scores = new double[reportsNumber];
        Status[] statuses = new Status[reportsNumber];

        ProcessReports(unitNames, reportTypes, priorities, scores, statuses, reports);
    }
      
}