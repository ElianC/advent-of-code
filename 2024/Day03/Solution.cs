using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day03;

public class Solution : BaseSolution
{
    public Solution(string day, string year) : base(day, year)
    {
    }

    public override void Solve()
    {
        var input = GetInput();

        var response1 = Regex.Matches(input, @"mul\((\d+),(\d+)\)")
            .Select(e => int.Parse(e.Groups[1].Value) * int.Parse(e.Groups[2].Value))
            .Sum();

        var response2 = Regex.Matches(input, @"(?:don't\(\)(?:[^d]|d(?!o\(\)))*do\(\))|mul\((\d+),(\d+)\)")
            .Where(match => !match.Value.StartsWith("don't()"))
            .Select(e => int.Parse(e.Groups[1].Value) * int.Parse(e.Groups[2].Value))
            .Sum();

        Console.WriteLine($"Result of multiplications: {response1}");
        Console.WriteLine($"Result of multiplications from broken input: {response2}");
    }
}