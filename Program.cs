using System.Reflection;
using AdventOfCode;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var currentYear = DateTime.Now.Year.ToString();
var currentDay = DateTime.Now.Day;

var argYear = config["year"];
int.TryParse(config["day"], out var argDay);

var targetYear = argYear ?? currentYear;
var targetDay = (argDay > 0 ? argDay : currentDay).ToString("00");

SolutionLoader.CreateInstance(targetDay, targetYear);
var instanceSolution = SolutionLoader.GetInstance();
var assembly = Assembly.GetExecutingAssembly();
var className = $"AdventOfCode._{targetYear}.Day{targetDay}.Solution";

var type = assembly.GetType(className);

if (type is null)
{
    Console.WriteLine($"The class for the day {targetDay} of year {targetYear} was not found. '{className}'");
    return;
}

var instance = Activator.CreateInstance(type);
if (instance is null)
{
    Console.WriteLine($"Unable to create an instance of '{className}'.");
    return;
}

// Find a method to invoke (e.g., a Run or Execute method)
var method = type.GetMethod("Solve");

if (method == null)
{
    Console.WriteLine(
        $"Method 'Solve' not found in class '{className}'. Ensure that the class extend the class BaseSolution");
    return;
}

// Invoke the method
method.Invoke(instance, null);