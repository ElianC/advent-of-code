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

    private (System.Drawing.Point Point, char Value)? GetNextCellInfos(Point pos, Directions dir)
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

        return _grid.GetGridPoint(nextPos);
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

            while (nextCell is { Value: '#' })
            {
                guardDir = GetNextDirection(guardDir);
                nextCell = GetNextCellInfos(guardPos, guardDir);
            }

            if (nextCell is null) break;

            guardPos = nextCell.Value.Point;
        } while (true);

        return visitedCells
            .Select(e => e.pos)
            .Distinct()
            .ToList();
    }

    public override void Solve()
    {
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

        var visitedCells = DoGuardRound(guardPos, guardDir);

        var solution1 = visitedCells.Count;
        var solution2 = 0;

        foreach (var visited in visitedCells)
        {
            if (visited.Equals(guardPos)) continue;
            _grid.UpdateGridPointValue(visited, '#');

            try
            {
                DoGuardRound(guardPos, guardDir);
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