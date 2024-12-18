using System.Drawing;

namespace AdventOfCode._2015.Day03;

public class Solution : BaseSolution
{
    private readonly HashSet<Point> _housesVisitedBySanta = [];
    private readonly HashSet<Point> _housesVisitedBySantaAndRobot = [];

    private readonly List<(char instruction, Size direction)> _instructionsDirectionsPair =
    [
        ('^', new Size(0, -1)),
        ('>', new Size(1, 0)),
        ('v', new Size(0, 1)),
        ('<', new Size(-1, 0))
    ];

    public override void Solve()
    {
        var instructions = GetInput();

        var positionSantaAlone = Point.Empty;
        var positionSanta = Point.Empty;
        var positionRobot = Point.Empty;

        _housesVisitedBySantaAndRobot.Add(positionSanta);

        for (var i = 0; i < instructions.Length; i++)
        {
            var instruction = instructions[i];
            var direction = _instructionsDirectionsPair
                .Where(e => e.instruction == instruction)
                .Select(e => e.direction)
                .Single();

            positionSantaAlone += direction;
            _housesVisitedBySanta.Add(positionSantaAlone);

            if (i % 2 == 0)
            {
                positionSanta += direction;
                _housesVisitedBySantaAndRobot.Add(positionSanta);
            }
            else
            {
                positionRobot += direction;
                _housesVisitedBySantaAndRobot.Add(positionRobot);
            }
        }

        Console.WriteLine("{0} houses have received a gift by santa.",
            _housesVisitedBySanta.Count
        );
        Console.WriteLine("{0} houses have received a gift by santa and santa robot.",
            _housesVisitedBySantaAndRobot.Count);
    }
}