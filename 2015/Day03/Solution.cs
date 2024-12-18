using System.Drawing;

namespace AdventOfCode._2015.Day03;

public class Solution : BaseSolution
{
    private readonly List<( char, Size)> _cellGuardMatchDirection =
    [
        ('^', new Size(0, -1)),
        ('>', new Size(1, 0)),
        ('v', new Size(0, 1)),
        ('<', new Size(-1, 0))
    ];

    private readonly HashSet<Point> points = new();

    public override void Solve()
    {
        var input = GetInput();
        var pos = Point.Empty;

        foreach (var dir in input)
        {
            points.Add(pos);
            var matching = _cellGuardMatchDirection.Where(e => e.Item1 == dir).Select(e => e.Item2).Single();

            pos += matching;
        }

        Console.WriteLine($"{points.Count}");
    }
}