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
            .Select(e => (cell: e.Key, e.Select(f => f.Item2).ToArray()))
            .ToArray();

        HashSet<(int x, int y)> antiNodesPoints = [];
        HashSet<(int x, int y)> antiNodesPoints2 = [];

        foreach (var group in filteredInput)
        {
            var points = group.Item2.ToList();

            for (var i = 0; i < group.Item2.Length - 1; i++)
            {
                var currentItem = group.Item2[i];

                foreach (var coords in points.Skip(i + 1))
                {
                    var diffs = (x: coords.x - currentItem.x, y: coords.y - currentItem.y);

                    if (ArePointsInsideBounds(currentItem) &&
                        ArePointsInsideBounds(coords))
                    {
                        antiNodesPoints2.Add(currentItem);
                        antiNodesPoints2.Add(coords);
                    }

                    var descTest = (x: coords.x + diffs.x, y: coords.y + diffs.y);
                    var ascTest = (x: currentItem.x - diffs.x, y: currentItem.y - diffs.y);

                    if (ArePointsInsideBounds(descTest)) antiNodesPoints.Add(descTest);

                    while (ArePointsInsideBounds(descTest))
                    {
                        antiNodesPoints2.Add(descTest);
                        descTest.x += diffs.x;
                        descTest.y += diffs.y;
                    }

                    if (ArePointsInsideBounds(ascTest)) antiNodesPoints.Add(ascTest);

                    while (ArePointsInsideBounds(ascTest))
                    {
                        antiNodesPoints2.Add(ascTest);
                        ascTest.x -= diffs.x;
                        ascTest.y -= diffs.y;
                    }
                }
            }
        }

        Console.WriteLine($"Solution 1 {antiNodesPoints.Count}");
        Console.WriteLine($"Solution 2 {antiNodesPoints2.Count}");

        bool ArePointsInsideBounds((int x, int y) pos)
        {
            return pos.x >= 0 && pos.x < input[0].Length && pos.y >= 0 && pos.y < input.Length;
        }
    }
}