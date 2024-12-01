using System.Reflection;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var currentYear = DateTime.Now.Year.ToString();
var currentDay = DateTime.Now.Day.ToString("00");

var argYear = config["year"];
var argDay = config["day"];

var targetYear = argYear ?? currentYear;
var targetDay = argDay ?? currentDay;

var assembly = Assembly.GetExecutingAssembly();
var className = $"AdventOfCode._{targetYear}.Day{targetDay}.Day{targetDay}";

var type = assembly.GetType(className);

if (type is null)
{
    Console.WriteLine($"Class '{className}' not found.");
    return;
}

var instance = Activator.CreateInstance(type, targetDay, targetYear);
if (instance is null)
{
    Console.WriteLine($"Unable to create an instance of '{className}'.");
    return;
}

// Find a method to invoke (e.g., a Run or Execute method)
var method = type.GetMethod("Solve");

if (method == null)
{
    Console.WriteLine($"Method 'Solve' not found in class '{className}'.");
    return;
}

// Invoke the method
method.Invoke(instance, null);