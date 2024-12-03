namespace AdventOfCode._2024.Day02;

public class Solution : BaseSolve
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

        var solution1 = report
            .Where(r => r.Zip(
                            r.Skip(1), (f, s) => Math.Abs(f - s) <= 3).All(b => b) &&
                        (r.SequenceEqual(r.Distinct().Order()) || r.SequenceEqual(r.Distinct().OrderDescending())));
        Console.WriteLine($"There are {solution1.Count()} reports safe");

        var solution2 = report
            .Where(r =>
            {
                var isValid = false;

                for (var i = -1; i < r.Count; i++)
                {
                    var truncatedList = r.ToList();
                    if (i >= 0) truncatedList.RemoveAt(i);

                    var isAscValid = truncatedList
                        .Zip(
                            truncatedList.Skip(1),
                            (f, s) => Math.Abs(f - s) <= 3 && f < s)
                        .All(b => b);
                    var isDescValid = truncatedList
                        .Zip(
                            truncatedList.Skip(1),
                            (f, s) => Math.Abs(f - s) <= 3 && f > s)
                        .All(b => b);

                    isValid = isAscValid || isDescValid;
                    if (isValid) break;
                }

                return isValid;
            });

        Console.WriteLine($"There are {solution2.Count()} reports safe");
    }
}