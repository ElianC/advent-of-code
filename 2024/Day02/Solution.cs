namespace AdventOfCode._2024.Day02;

public class Solution : BaseSolution
{
    public Solution(string day, string year) : base(day, year)
    {
    }

    public override void Solve()
    {
        var input = GetInput();

        var report = input
            .Split(Environment.NewLine)
            .Select(e => e.Split(" ").Select(int.Parse).ToList())
            .ToList();

        var solution1 = report.Count(IsListValid);

        Console.WriteLine($"There are {solution1} reports safe");

        var solution2 = report
            .Count(r =>
                Enumerable.Range(1, r.Count)
                    .Select(i => r.Take(i - 1).Concat(r.Skip(i)))
                    .Select(IsListValid)
                    .Any(b => b));

        Console.WriteLine($"There are {solution2} reports safe");
    }

    private static bool IsListValid(IEnumerable<int> list)
    {
        list = list.ToList();
        var hasDistanceInferiorTo3 = list
            .Zip(list.Skip(1), (f, s) =>
                Math.Abs(f - s) <= 3)
            .All(b => b);

        var isListAsc = list.SequenceEqual(list.Distinct().Order());
        var isListDesc = list.SequenceEqual(list.Distinct().OrderDescending());
        return hasDistanceInferiorTo3 && (isListAsc || isListDesc);
    }
}