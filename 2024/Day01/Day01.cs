namespace AdventOfCode._2024.Day01;

public class Day01 : BaseSolve
{
    public Day01(string day, string year) : base(day, year)
    {
    }

    public override void Solve()
    {
        var input = GetInput();

        var numbers = input
            .Split(Environment.NewLine)
            .Select(ln => ln.Split("   "))
            .Select(col => (LeftNumber: uint.Parse(col[0]), RightNumber: uint.Parse(col[1])))
            .ToList();

        var leftNumbers = numbers
            .Select(e => e.LeftNumber)
            .Order()
            .ToList();
        var rightNumbers = numbers
            .Select(e => e.RightNumber)
            .Order()
            .ToList();

        var solution1 = leftNumbers
            .Zip(rightNumbers, (x, y) => Math.Abs(x - (long)y))
            .Sum();
        Console.WriteLine($"Total distance between the lists: {solution1}");

        var weights = rightNumbers
            .CountBy(x => x)
            .ToDictionary();
        var solution2 = leftNumbers
            .Select(num => weights.GetValueOrDefault(num) * num)
            .Sum();
        Console.WriteLine($"The similarity score between the lists: {solution2}");
    }
}