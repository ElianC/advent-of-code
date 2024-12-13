using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day08;

public class Solution : BaseSolution
{
    private readonly Grid<char> _grid;

    public Solution()
    {
        _grid = new Grid<char>(GetInput());
    }

    public override void Solve()
    {
        var antennasGroupedByFrequencies = _grid.GetGrid()
            .SelectMany(line => line
                .Select(e => new { frequency = e.Value, pos = e.Point })
                .Where(e => e.frequency != '.')
                .ToArray())
            .GroupBy(e => e.frequency)
            .Select(e => new { frequency = e.Key, pos = e.Select(f => f.pos).ToArray() })
            .ToArray();

        HashSet<Point> antiNodesPos = [];
        HashSet<Point> antiNodesPosAndAntennas = [];

        foreach (var antennasFrequencyGroup in antennasGroupedByFrequencies)
            for (var i = 0; i < antennasFrequencyGroup.pos.Length - 1; i++)
            {
                var pos = antennasFrequencyGroup.pos[i];
                antiNodesPosAndAntennas.Add(pos);

                foreach (var comparePos in antennasFrequencyGroup.pos.Skip(i + 1))
                {
                    var posDiff = new Size(comparePos) - new Size(pos);
                    antiNodesPosAndAntennas.Add(comparePos);

                    var posNextAntiNode = comparePos + posDiff;
                    var posPrevAntiNode = pos - posDiff;

                    if (_grid.ContainsPoint(posNextAntiNode)) antiNodesPos.Add(posNextAntiNode);

                    while (_grid.ContainsPoint(posNextAntiNode))
                    {
                        antiNodesPosAndAntennas.Add(posNextAntiNode);
                        posNextAntiNode += posDiff;
                    }

                    if (_grid.ContainsPoint(posPrevAntiNode)) antiNodesPos.Add(posPrevAntiNode);

                    while (_grid.ContainsPoint(posPrevAntiNode))
                    {
                        antiNodesPosAndAntennas.Add(posPrevAntiNode);
                        posPrevAntiNode -= posDiff;
                    }
                }
            }

        Console.WriteLine($"Solution 1 {antiNodesPos.Count}");
        Console.WriteLine($"Solution 2 {antiNodesPosAndAntennas.Count}");
    }
}