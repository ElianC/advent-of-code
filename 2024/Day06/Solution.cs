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

    private (Point pos, char cell)? GetNextCellInfos(Point pos, Directions dir)
    {
        var size = new Size(0, 0);

        switch (dir)
        {
            case Directions.Forward:
                size = new Size(0, -1);
                break;
            case Directions.Right:
                size = new Size(1, 0);
                break;
            case Directions.Backward:
                size = new Size(0, 1);
                break;
            case Directions.Left:
                size = new Size(-1, 0);
                break;
        }

        var nextPos = pos + size;

        if (_grid.ContainsPoint(nextPos) is false) return null;
        var nextCell = _grid.GetGridPoint(nextPos);

        return (pos: nextCell.Point, cell: nextCell.Value);
    }

    private List<Point> DoGuardRound(Point startPos, Directions startDir)
    {
        var guardDir = startDir;
        var guardPos = startPos;

        HashSet<(Directions dir, Point pos)> visitedCells = [];

        do
        {
            if (!visitedCells.Add((guardDir, guardPos)))
                throw new Exception(
                    "Guard has already patrolled here, he must going around in circles.");

            var nextCell = GetNextCellInfos(guardPos, guardDir);

            while (nextCell is { cell: '#' })
            {
                guardDir = GetNextDirection(guardDir);
                nextCell = GetNextCellInfos(guardPos, guardDir);
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
        var guardPos = _grid.GetGrid()
            .SelectMany(e => e)
            .Single(e => _cellGuardMatchDirection
                .Any(dir => dir.Item2 == e.Value));

        var guardDir = _cellGuardMatchDirection
            .Where(e => e.Item2 == _grid.GetGridPointValue(guardPos.Point))
            .Select(e => e.Item1)
            .Single();

        Console.WriteLine(guardDir);
        Console.WriteLine(guardPos);
        var visitedCells = DoGuardRound(guardPos.Point, guardDir);

        var solution1 = visitedCells.Count;
        var solution2 = 0;

        foreach (var visited in visitedCells)
        {
            if (visited == guardPos.Point) continue;

            try
            {
                _grid.UpdateGridPointValue(visited, '#');
                DoGuardRound(guardPos.Point, guardDir);
            }
            catch (Exception)
            {
                solution2++;
            }
            finally
            {
                _grid.UpdateGridPointValue(visited, '.');
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