using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day10;

public class Solution : BaseSolution
{
    private readonly HashSet<(Point endTrailPosition, Point startingPosition)> _completedTrails = [];
    private readonly Grid<int> _grid;
    private readonly List<Point> _reachableEndPoints = [];

    public Solution()
    {
        _grid = new Grid<int>(GetInput());
    }

    private void ExploreNextCells(Point currentPosition,
        Point trailStartPosition)
    {
        var currentCell = _grid[currentPosition.Y][currentPosition.X].Value;
        List<Size> directionOffsets = [new(-1, 0), new(0, 1), new(1, 0), new(0, -1)];

        var nextValidCells = directionOffsets
            .Select(offset => currentPosition + offset)
            .Where(_grid.ContainsPoint)
            .Where(pos => _grid[pos.Y][pos.X].Value == currentCell + 1)
            .Select(pos => new { position = pos, value = _grid[pos.Y][pos.X].Value })
            .ToList();

        foreach (var nextCell in nextValidCells)
        {
            if (nextCell.value == 9)
            {
                _completedTrails.Add((nextCell.position, trailStartPosition));
                _reachableEndPoints.Add(nextCell.position);
                continue;
            }

            ExploreNextCells(nextCell.position, trailStartPosition);
        }
    }

    public override void Solve()
    {
        var trailsHead = _grid.GetGrid()
            .SelectMany(row => row)
            .Where(cell => cell.Value == 0)
            .ToList();

        foreach (var trailHead in trailsHead)
            ExploreNextCells(trailHead.Point, trailHead.Point);

        Console.WriteLine($"part 1 {_completedTrails.Count}");
        Console.WriteLine($"part 2 {_reachableEndPoints.Count}");
    }
}