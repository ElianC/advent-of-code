using System.Diagnostics;

namespace AdventOfCode;

public sealed class SolutionLoader
{
    private static SolutionLoader? _instance;

    private SolutionLoader(string day, string year)
    {
        Day = day;
        Year = year;
    }

    public string Day { get; }
    public string Year { get; }

    public static SolutionLoader CreateInstance(string day, string year)
    {
        return _instance = new SolutionLoader(day, year);
    }

    public static SolutionLoader GetInstance()
    {
        Debug.Assert(_instance != null, nameof(_instance) + " != null");
        return _instance;
    }
}