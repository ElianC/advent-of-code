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
SolutionLoader.InvokeSolutionSolve();