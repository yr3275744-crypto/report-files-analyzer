// TODO: add prints
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
            if (string.IsNullOrEmpty(report[0].Trim()))
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
            
            unitNames[validCount] = report[0];
            reportTypes[validCount] = reportType;
            priorities[validCount] = priority;
            scores[validCount] = score;
            statuses[validCount] = status;
            validCount = validCount + 1;
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
        return sum / validCount;
    }

    static double FindMaxScore(double[] scores)
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

    static double FindMinScore(double[] scores, int validCount)
    {
        {
            if (validCount == 0)
            {
                return 0.0;
            }
            double min = scores[0];
            for (int i = 0; i < validCount; i++)
            {
                if (scores[i] < min)
                {
                    min = scores[i];
                }
            }
            return min;
        }
    }

    static int CountByStatus(Status[] statuses, Status desiredStatus, int validCount)
    {
        if (validCount == 0)
        {
            return 0;
        }
        int count = 0;
        for(int i = 0; i < validCount; i ++)
        {
            if (statuses[i] == desiredStatus)
            {
                count = count + 1;
            }
        }
        return count;
    }

    static int CountByType(ReportType[] reportTypes, ReportType desiredType, int validCount)
    {
        if (validCount == 0)
        {
            return 0;
        }
        int count = 0;
        for (int i = 0; i < validCount; i++)
        {
            if (reportTypes[i] == desiredType)
            {
                count = count + 1;
            }
        }
        return count;
    }

    static void DisplayBasicStatistics(double[] scores, int validCount)
    {
        double avarage = CalculateAverage(scores, validCount);
        double maxScore = FindMaxScore(scores);
        double minScore = FindMinScore(scores, validCount);
        
        Console.WriteLine($"=== Report Statistics ===\n" +
            $"Total Reports:{validCount}\n" +
            $"Average Score:{avarage:F2}\n" +
            $"Highest Score:{maxScore}\n" +
            $"Lowest Score:{minScore}");
    }

    static void DisplayStatusCounts(Status[] statuses, int validCount)
    {
        int pendingNumber = CountByStatus(statuses, Status.Pending, validCount);
        int approvedNumber = CountByStatus(statuses, Status.Approved, validCount);
        int rejectedNumber = CountByStatus(statuses, Status.Rejected, validCount);

        Console.WriteLine($"=== Reports by Status ===\n" +
            $"Pending:{pendingNumber}\n" +
            $"Approved:{approvedNumber}\n" +
            $"Rejected:{rejectedNumber}");
    }

    static void DisplayTypeCounts(ReportType[] reportTypes, int validCount)
    {
        int collectNumber = CountByType(reportTypes, ReportType.Collect, validCount);
        int analyzeNumber = CountByType(reportTypes, ReportType.Analyze, validCount);
        int reconNumber = CountByType(reportTypes, ReportType.Recon, validCount);
        int intelNumber = CountByType(reportTypes, ReportType.Intel, validCount);

        Console.WriteLine($"=== Reports by Type ===\n" +
            $"Collect:{collectNumber}\n" +
            $"Analyze:{analyzeNumber}\n" +
            $"Recon:{reconNumber}\n" +
            $"Intel:{intelNumber}");
    }

    static bool FindHighesPriorityApprovedIndex(Status[] statuses, int[] priorities, int validCount, out int index)
    {
        int? maxPriority = null;
        bool isFind = false;
        index = -1;

        for (int i = 0; i < validCount; i ++)
        {
            if (statuses[i] == Status.Approved && maxPriority is null) 
            {
                isFind = true;
                index = i;
                maxPriority = priorities[i];
            }
            else if (statuses[i] == Status.Approved && priorities[i] > maxPriority)
            {
                index = i;
                maxPriority = priorities[i];
            }
        }
        return isFind;
    }

    static void DisplayHighestPriorityApproved(string[] unitNames, ReportType[] reportTypes, int[] priorities, double[] scores, Status[] statuses, int validCount)
    {
        Console.WriteLine("=== Highest Priority Approved Report ===");
        int index;
        bool isFind = FindHighesPriorityApprovedIndex(statuses, priorities, validCount, out index);
        if (isFind == false)
        {
            Console.WriteLine("No approved is find.");
            return;
        }
        Console.WriteLine($"Unit:{unitNames[index]}\n" +
            $"Type:{reportTypes[index]}\n" +
            $"Priority:{priorities[index]}\n" +
            $"Score:{scores[index]}");
    }

    static bool GetAverageByPriority(int[] priorities, double[] scores, int validCount, int priority, out double average)
    {
        double sum = 0;
        int counter = 0;
        average = 0;
        bool isFind = false;
        for (int i = 0; i < validCount;i ++)
        {
            if (priorities[i] == priority)
            {
                isFind = true;
                sum = sum + scores[i];
                counter = counter + 1;
            }
        }
        average = sum / counter;
        return isFind;
    }

    static void DisplayAverageByPriority(int[] priorities, double[] scores, int validCount)
    {
        Console.WriteLine("=== Average Score by Priority ===");
        for (int i = 1; i <= 5; i ++)
        {
            double average;
            bool isFind = GetAverageByPriority(priorities, scores, validCount, i, out average);
            if (isFind = true)
            {
                Console.WriteLine($"Priority {i}:{average:F2}");
            }
            else
            {
                Console.WriteLine($"Priority {i}: No reports");
            }
        }
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

        DisplayBasicStatistics(scores, validCount);
        DisplayStatusCounts(statuses, validCount);
        DisplayTypeCounts(reportTypes, validCount);
        DisplayHighestPriorityApproved(unitNames, reportTypes, priorities, scores, statuses, validCount);
        DisplayAverageByPriority(priorities, scores, validCount);
    }      
}