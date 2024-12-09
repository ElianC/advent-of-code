using System.Diagnostics;
using System.Reflection;

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

    public static void InvokeSolutionSolve()
    {
        var instance = GetInstance();

        var assembly = Assembly.GetExecutingAssembly();
        var className = $"AdventOfCode._{instance.Year}.Day{instance.Day}.Solution";

        var type = assembly.GetType(className);

        if (type is null)
        {
            Console.WriteLine(
                $"The class for the day {instance.Day} of year {instance.Year} was not found. '{className}'");
            return;
        }

        var activatorInstance = Activator.CreateInstance(type);
        if (activatorInstance is null)
        {
            Console.WriteLine($"Unable to create an instance of '{className}'.");
            return;
        }

        // Find a method to invoke (e.g., a Run or Execute method)
        var method = type.GetMethod("Solve");

        if (method is null)
        {
            Console.WriteLine(
                $"Method 'Solve' not found in class '{className}'. Ensure that the class extend the class BaseSolution");
            return;
        }

        // Invoke the method
        method.Invoke(activatorInstance, null);
    }
}