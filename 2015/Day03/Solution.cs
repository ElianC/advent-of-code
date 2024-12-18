using System.Drawing;

namespace AdventOfCode._2015.Day03;

public class Solution : BaseSolution
{
    private readonly Dictionary<char, Size> _directions = new()
    {
        { '^', new Size(0, -1) },
        { '>', new Size(1, 0) },
        { 'v', new Size(0, 1) },
        { '<', new Size(-1, 0) }
    };

    private readonly HashSet<Point> _housesVisitedBySanta = [];
    private readonly HashSet<Point> _housesVisitedBySantaAndRobot = [];


    public override void Solve()
    {
        var instructions = GetInput();

        var santaAlonePosition = Point.Empty;
        var santaPosition = Point.Empty;
        var robotPosition = Point.Empty;

        _housesVisitedBySantaAndRobot.Add(santaPosition);
        _housesVisitedBySanta.Add(santaAlonePosition);

        for (var i = 0; i < instructions.Length; i++)
        {
            var instruction = instructions[i];
            var direction = _directions[instruction];

            santaAlonePosition += direction;
            _housesVisitedBySanta.Add(santaAlonePosition);

            if (i % 2 == 0)
            {
                santaPosition += direction;
                _housesVisitedBySantaAndRobot.Add(santaPosition);
            }
            else
            {
                robotPosition += direction;
                _housesVisitedBySantaAndRobot.Add(robotPosition);
            }
        }

        Console.WriteLine("{0} houses have received a gift by santa.",
            _housesVisitedBySanta.Count
        );
        Console.WriteLine("{0} houses have received a gift by santa and santa robot.",
            _housesVisitedBySantaAndRobot.Count);
    }
}