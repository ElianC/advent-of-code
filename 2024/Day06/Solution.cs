using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day06;

public class Solution : BaseSolution
{
    private readonly List<(Directions, char)> _cellGuardMatchDirection =
    [
        (Directions.Forward, '^'),
        (Directions.Right, '>'),
        (Directions.Backward, 'v'),
        (Directions.Left, '<')
    ];

    private readonly Grid<char> _grid;

    public Solution()
    {
        _grid = new Grid<char>(GetInput());
    }


    private Directions GetNextDirection(Directions dir)
    {
        return dir switch
        {
            Directions.Forward => Directions.Right,
            Directions.Right => Directions.Backward,
            Directions.Backward => Directions.Left,
            Directions.Left => Directions.Forward,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    private (Point pos, string cell)? GetNextCellInfos(
        List<List<string>> input, Point pos, Directions dir)
    {
        Size offset = new();
        switch (dir)
        {
            case Directions.Forward:
                offset = new Size(0, -1);
                break;
            case Directions.Right:
                offset = new Size(1, 0);
                break;
            case Directions.Backward:
                offset = new Size(0, 1);
                break;
            case Directions.Left:
                offset = new Size(-1, 0);
                break;
        }

        var nextPos = pos + offset;

        if (_grid.ContainsPoint(nextPos) is false) return null;
        return (new Point(nextPos.X, nextPos.Y), input[nextPos.Y][nextPos.X]);
    }

    private List<Point> DoGuardRound(List<List<string>> input, Point startPos, Directions startDir)
    {
        var guardDir = startDir;
        var guardPos = startPos;

        HashSet<(Directions dir, Point pos)> visitedCells = [];

        do
        {
            if (!visitedCells.Add((guardDir, guardPos)))
                throw new Exception(
                    "Guard has already patrolled here, he must going around in circles.");

            var nextCell = GetNextCellInfos(input, guardPos, guardDir);

            while (nextCell is { cell: "#" })
            {
                guardDir = GetNextDirection(guardDir);
                nextCell = GetNextCellInfos(input, guardPos, guardDir);
            }

            if (nextCell is null) break;

            guardPos = nextCell.Value.pos;
        } while (true);

        return visitedCells
            .Select(e => e.pos)
            .Distinct()
            .ToList();
    }

    public override void Solve()
    {
        var input = _grid.GetGrid()
            .Select(e => e.Select(f => f.Value.ToString()).ToList()).ToList();

        var guardPos = _grid.GetGrid()
            .Select((row, y) =>
            {
                var x = row
                    .FindIndex(cell => _cellGuardMatchDirection
                        .Any(e => e.Item2 == cell.Value));
                return new Point(x, y);
            })
            .Single(pos => pos is { X: >= 0, Y: >= 0 });

        var guardDir = _cellGuardMatchDirection
            .Where(e => e.Item2 == _grid.GetGridPointValue(guardPos))
            .Select(e => e.Item1)
            .Single();

        var visitedCells = DoGuardRound(input, guardPos, guardDir);

        var solution1 = visitedCells.Count;
        var solution2 = 0;

        foreach (var visited in visitedCells)
        {
            var localInput = new List<List<string>>(input.Select(e => e.ToList()));

            if (visited.Equals(guardPos)) continue;
            localInput[visited.Y][visited.X] = "#";

            try
            {
                DoGuardRound(localInput, guardPos, guardDir);
            }
            catch (Exception)
            {
                solution2++;
            }
        }

        Console.WriteLine($"Solution 1: {solution1}");
        Console.WriteLine($"Solution 2: {solution2}");
    }

    private enum Directions
    {
        Forward,
        Right,
        Backward,
        Left
    }
}