namespace AdventOfCode._2024.Day08;

public class Solution : BaseSolution
{
    public Solution(string day, string year) : base(day, year)
    {
    }

    public override void Solve()
    {
        var input = GetInput()
            .Split(Environment.NewLine)
            .ToArray();

        var filteredInput = input
            .SelectMany((line, y) => line
                .Select((e, x) => (cell: e, (x, y)))
                .Where(e => e.cell != '.')
                .ToArray())
            .GroupBy(e => e.cell)
            .Select(e => (e.Key, e.Select(f => f.Item2).ToArray()))
            .ToArray();

        HashSet<(int x, int y)> antiNodesPoints = [];

        foreach (var group in filteredInput)
        {
            var test = group.Item2.ToList();
            var currentIndex = 0;

            while (currentIndex < group.Item2.Length - 1)
            {
                var currentItem = group.Item2[currentIndex];

                foreach (var coords in test.Skip(currentIndex + 1))
                {
                    var diffs = (x: coords.x - currentItem.x, y: coords.y - currentItem.y);

                    if (ArePointsInsideBounds(coords.x + diffs.x, coords.y + diffs.y))
                        antiNodesPoints.Add((coords.x + diffs.x, coords.y + diffs.y));

                    if (ArePointsInsideBounds(currentItem.x - diffs.x, currentItem.y - diffs.y))
                        antiNodesPoints.Add((currentItem.x - diffs.x, currentItem.y - diffs.y));
                }

                currentIndex++;
            }
        }

        Console.WriteLine($"Solution 1 {antiNodesPoints.Count}");
        return;

        bool ArePointsInsideBounds(int x, int y)
        {
            return x >= 0 && x < input[0].Length && y >= 0 && y < input.Length;
        }
    }
}