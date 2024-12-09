namespace AdventOfCode._2024.Day06;

public class Solution : BaseSolution
{
    private readonly List<(Directions, string)> _cellGuardMatchDirection =
    [
        (Directions.Forward, "^"),
        (Directions.Right, ">"),
        (Directions.Backward, "v"),
        (Directions.Left, "<")
    ];

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

    private static ((int x, int y) pos, string cell)? GetNextCellInfos(
        List<List<string>> input, (int x, int y) pos, Directions dir)
    {
        (int x, int y) boundaries = (input[0].Count - 1, input.Count - 1);
        var nextPos = pos;

        switch (dir)
        {
            case Directions.Forward:
                nextPos.y--;
                if (nextPos.y < 0) return null;
                break;
            case Directions.Right:
                nextPos.x++;
                if (nextPos.x > boundaries.x) return null;
                break;
            case Directions.Backward:
                nextPos.y++;
                if (nextPos.y > boundaries.y) return null;
                break;
            case Directions.Left:
                nextPos.x--;
                if (nextPos.x < 0) return null;
                break;
        }

        return (nextPos, input[nextPos.y][nextPos.x]);
    }

    private List<(int x, int y)> DoGuardRound(List<List<string>> input, (int x, int y) startPos, Directions startDir)
    {
        var guardDir = startDir;
        var guardPos = startPos;

        HashSet<(Directions dir, (int x, int y) pos)> visitedCells = [];

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
        var input = GetInput()
            .Split(Environment.NewLine)
            .Select(row => row.Select(e => e.ToString()).ToList())
            .ToList();

        var guardPos = input
            .Select((row, y) => (x: row.FindIndex(cell => _cellGuardMatchDirection.Any(e => e.Item2 == cell)), y))
            .Single(pos => pos is { x: >= 0, y: >= 0 });

        var guardDir = _cellGuardMatchDirection
            .Where(e => e.Item2 == input[guardPos.y][guardPos.x])
            .Select(e => e.Item1)
            .Single();

        var visitedCells = DoGuardRound(input, guardPos, guardDir);

        var solution1 = visitedCells.Count;
        var solution2 = 0;

        foreach (var visited in visitedCells)
        {
            var localInput = new List<List<string>>(input.Select(e => e.ToList()));

            if (visited.Equals(guardPos)) continue;
            localInput[visited.y][visited.x] = "#";

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