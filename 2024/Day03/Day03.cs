using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day03;

public class Solution : BaseSolve
{
    public Solution(string day, string year) : base(day, year)
    {
    }

    public override void Solve()
    {
        var input = GetInput();

        var inputSplittedDont = GetInput().Split("don't()");
        var input2 = inputSplittedDont[0];

        foreach (var str in inputSplittedDont)
        {
            var strSplit = str.Split("do()", 2);

            if (strSplit.Length > 1) input2 += strSplit[1];
        }

        var response1 = GetCountMatchMul(input);
        var response2 = GetCountMatchMul(input2);

        Console.WriteLine($"Result of multiplications: {response1}");
        Console.WriteLine($"Result of multiplications from broken input: {response2}");
    }

    public int GetCountMatchMul(string input)
    {
        var result = 0;
        foreach (Match match in Regex.Matches(input, @"mul\(\d+,\d+\)"))
        {
            var test = Regex.Matches(match.Value, @"(\d+)");

            result += int.Parse(test[0].Value) * int.Parse(test[1].Value);
        }

        return result;
    }
}