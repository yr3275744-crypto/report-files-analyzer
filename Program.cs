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
                Console.WriteLine($"File loaded: {reports.Length} lines found");
                return nullableReports;
            }
            else
            {
                Console.WriteLine("Error:File is empty");
                return null;
            }
        }
        else
        {
            Console.WriteLine("The file did not exists.");
            return null;
        }
    }

    //static int? TryParsToInt(string strtingNumber)
    //{
    //   if (int.TryParse(strtingNumber.Trim(), out int number))
    //    {
    //        int? nullableNumber = number;
    //        return nullableNumber;
    //    }
    //   else
    //    {
    //        return null;
    //    }
    //}

    //static ReportType? TryParsToReportType(string stringReportType)
    //{
    //    if (Enum.TryParse<ReportType>()) ;
    //}


    static int ProcessReports(string[] unitNames, ReportType[] reportTypes, int[] priorities, double[] scores, Status[] statuses, string[] reports)
    {
        int validCount = 0;
        for (int i = 0; i < reports.Length; i ++)
        {
            string[] report = reports[i].Split(',');
            if (report.Length < 5)
            {
                continue;
            }
            if (report[0].Trim() is null)
            {
                continue;
            }
            if (!Enum.TryParse<ReportType>(report[1].Trim(), true, out ReportType reportType))
            {
                continue;
            }
            if (int.TryParse(report[2].Trim(), out int priority))
            {
                if (priority < 1 || priority > 5)
                {
                    continue;
                }
            }
            else
            {
                continue;
            }
            if (double.TryParse(report[3].Trim(), out double score))
            {
                if (score < 0.0 || score > 100.0)
                {
                    continue;
                }
            }
            else
            {
                continue;
            }
            if (! Enum.TryParse<Status>(report[4].Trim(), true, out Status status))
            {
                continue;
            }
            validCount = validCount + 1;
            unitNames[i] = report[0];
            reportTypes[i] = reportType;
            priorities[i] = priority;
            scores[i] = score;
            statuses[i] = status;
        }
        Console.WriteLine($"Processing complete.\n" +
            $"Valid records:{validCount}\nInvalid records:{reports.Length - validCount}\n" +
            $"Stored {validCount} valid records for analysis");
        return validCount;
    }

    static double CalculateAverage(double[] scores, int validCount)
    {
        if (validCount == 0)
        {
            return 0.0;
        }
        double sum = 0.0;
        for(int i = 0; i < validCount; i ++)
        {
            sum = sum + scores[i];
        }
        return sum;
    }

    static double FindByScore(double[] scores)
    {
        double max = 0.0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > max)
            {
                max = scores[i];
            }
        }
        return max;
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

        int validCount = ProcessReports(unitNames, reportTypes, priorities, scores, statuses, reports);
        double avarage = CalculateAverage(scores, validCount);
        Console.WriteLine($"{avarage}");
    }
      
}