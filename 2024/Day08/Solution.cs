using System.Text;

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
            //.Select(row => row.Select(e => e).ToArray())
            .ToArray();

        var response = 0;

        var filteredInput = input
            .SelectMany((line, y) => line
                .Select((e, x) => (e, (x, y)))
                .Where(e => e.e != '#' && e.e != '.')
                .ToArray())
            .GroupBy(e => e.e)
            .Select(e => (e.Key, e.Select(f => f.Item2).ToArray()))
            .ToArray();

        List<(int x, int y)> points = [];

        foreach (var group in filteredInput)
        {
            var test = group.Item2.ToList();
            var currentIndex = 0;

            Console.WriteLine(group.Key + "-" + string.Join(',', group.Item2));

            while (currentIndex < group.Item2.Length - 1)
            {
                var currentItem = group.Item2[currentIndex];

                foreach (var coords in test.Skip(currentIndex + 1))
                {
                    var diffX = coords.x - currentItem.x;
                    var diffY = coords.y - currentItem.y;

                    if (coords.y + diffY >= 0 && coords.y + diffY < input.Length &&
                        coords.x + diffX >= 0 && coords.x + diffX < input[0].Length)
                    {
                        //response++;
                        if (input[coords.y + diffY][coords.x + diffX] == '.')
                        {
                            response++;
                            points.Add((coords.x + diffX, coords.y + diffY));
                        }

                        ;
                        Console.WriteLine(
                            $"{currentItem} {coords} {(coords.x + diffX, coords.y + diffY)} {diffX},{diffY}, {input[coords.y + diffY][coords.x + diffX]}");
                    }
                    else
                    {
                        Console.WriteLine($"out of bounds 1 {(coords.y + diffY, coords.x + diffX)}");
                    }

                    if (currentItem.y - diffY >= 0 && currentItem.y - diffY < input.Length &&
                        currentItem.x - diffX >= 0 && currentItem.x - diffX < input[0].Length)
                    {
                        if (input[currentItem.y - diffY][currentItem.x - diffX] == '.')
                        {
                            response++;
                            points.Add((currentItem.x - diffX, currentItem.y - diffY));
                        }

                        Console.WriteLine(
                            $"2 {currentItem} {coords} {(currentItem.x - diffX, currentItem.y - diffY)} {diffX},{diffY}, {input[currentItem.y - diffY][currentItem.x - diffX]}");
                    }
                    else
                    {
                        Console.WriteLine($"out of bounds 2 {(currentItem.y - diffY, currentItem.x - diffX)}");
                    }

                    Console.WriteLine("------");
                }

                currentIndex++;
            }
        }

        var tests = input.ToList();
        foreach (var point in points)
        {
            var sb = new StringBuilder(tests[point.y]);
            sb[point.x] = '#';
            tests[point.y] = sb.ToString();
        }

        foreach (var tst in tests) Console.WriteLine(tst);
        Console.WriteLine($"Total response: {points.Count}");
        Console.WriteLine(response);
    }
}