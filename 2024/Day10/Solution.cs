namespace AdventOfCode._2024.Day10;

public class Solution : BaseSolution
{
    private readonly HashSet<((int x, int y) endTrailPosition, (int x, int y) startingPosition)> _completedTrails = [];
    private readonly List<(int x, int y)> _reachableEndPoints = [];

    private void ExploreNextCells(List<List<int>> grid, (int x, int y) currentPosition,
        (int x, int y) trailStartPosition)
    {
        var currentCell = grid[currentPosition.y][currentPosition.x];
        List<(int x, int y)> directionOffsets = [(-1, 0), (0, 1), (1, 0), (0, -1)];

        var nextValidCells = directionOffsets
            .Select(offset => (x: currentPosition.x + offset.x, y: currentPosition.y + offset.y))
            .Where(pos => pos.x >= 0 && pos.x < grid[0].Count && pos.y >= 0 && pos.y < grid.Count)
            .Where(pos => grid[pos.y][pos.x] == currentCell + 1)
            .Select(pos => new { position = (pos.x, pos.y), value = grid[pos.y][pos.x] })
            .ToList();

        foreach (var nextCell in nextValidCells)
        {
            if (nextCell.value == 9)
            {
                _completedTrails.Add((nextCell.position, trailStartPosition));
                _reachableEndPoints.Add(nextCell.position);
                continue;
            }

            ExploreNextCells(grid, nextCell.position, trailStartPosition);
        }
    }

    public override void Solve()
    {
        var input = GetInput()
            .Split(Environment.NewLine)
            .Select(row => row
                .Select(cell => int.Parse(cell.ToString()))
                .ToList())
            .ToList();

        var trailsHead = input
            .SelectMany((row, y) => row
                .Select((cellValue, x) => new { value = cellValue, position = (x, y) })
                .ToList())
            .Where(cell => cell.value == 0);

        foreach (var trailHead in trailsHead)
            ExploreNextCells(input, trailHead.position, trailHead.position);

        Console.WriteLine($"part 1 {_completedTrails.Count}");
        Console.WriteLine($"part 2 {_reachableEndPoints.Count}");
    }
}