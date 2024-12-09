namespace AdventOfCode._2024.Day08;

public class Solution : BaseSolution
{
    public override void Solve()
    {
        var input = GetInput()
            .Split(Environment.NewLine)
            .ToArray();

        var antennasGroupedByFrequencies = input
            .SelectMany((line, y) => line
                .Select((e, x) => new { frequency = e, pos = (x, y) })
                .Where(e => e.frequency != '.')
                .ToArray())
            .GroupBy(e => e.frequency)
            .Select(e => new { frequency = e.Key, pos = e.Select(f => f.pos).ToArray() })
            .ToArray();

        HashSet<(int x, int y)> antiNodesPos = [];
        HashSet<(int x, int y)> antiNodesPosAndAntennas = [];

        foreach (var antennasFrequencyGroup in antennasGroupedByFrequencies)
            for (var i = 0; i < antennasFrequencyGroup.pos.Length - 1; i++)
            {
                var pos = antennasFrequencyGroup.pos[i];
                antiNodesPosAndAntennas.Add(pos);

                foreach (var comparePos in antennasFrequencyGroup.pos.Skip(i + 1))
                {
                    var posDiff = (x: comparePos.x - pos.x, y: comparePos.y - pos.y);
                    antiNodesPosAndAntennas.Add(comparePos);

                    var posNextAntiNode = (x: comparePos.x + posDiff.x, y: comparePos.y + posDiff.y);
                    var posPrevAntiNode = (x: pos.x - posDiff.x, y: pos.y - posDiff.y);

                    if (ArePosInsideBounds(posNextAntiNode)) antiNodesPos.Add(posNextAntiNode);

                    while (ArePosInsideBounds(posNextAntiNode))
                    {
                        antiNodesPosAndAntennas.Add(posNextAntiNode);
                        posNextAntiNode.x += posDiff.x;
                        posNextAntiNode.y += posDiff.y;
                    }

                    if (ArePosInsideBounds(posPrevAntiNode)) antiNodesPos.Add(posPrevAntiNode);

                    while (ArePosInsideBounds(posPrevAntiNode))
                    {
                        antiNodesPosAndAntennas.Add(posPrevAntiNode);
                        posPrevAntiNode.x -= posDiff.x;
                        posPrevAntiNode.y -= posDiff.y;
                    }
                }
            }

        Console.WriteLine($"Solution 1 {antiNodesPos.Count}");
        Console.WriteLine($"Solution 2 {antiNodesPosAndAntennas.Count}");

        bool ArePosInsideBounds((int x, int y) pos)
        {
            return pos.x >= 0 && pos.x < input[0].Length && pos.y >= 0 && pos.y < input.Length;
        }
    }
}