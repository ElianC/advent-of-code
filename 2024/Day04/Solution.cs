namespace AdventOfCode._2024.Day04;

public class Solution : BaseSolution
{
    private readonly Dictionary<string, (int, int)> _dir = new();

    public Solution(string day, string year) : base(day, year)
    {
        _dir.Add("up", (0, -1));
        _dir.Add("down", (0, 1));
        _dir.Add("right", (1, 0));
        _dir.Add("left", (-1, 0));
        _dir.Add("up-right", (1, -1));
        _dir.Add("down-right", (1, 1));
        _dir.Add("down-left", (-1, 1));
        _dir.Add("up-left", (-1, -1));
    }

    private int GetCountOccurrencesXmas(char[][] inputs, int x, int y)
    {
        var maxX = inputs[0].Length;
        var maxY = inputs.Length;

        return _dir.Values.Select(dirs => Enumerable.Range(0, 4)
                .Select(i =>
                {
                    var posX = x + dirs.Item1 * i;
                    var posY = y + dirs.Item2 * i;

                    if (posX >= 0 && posX < maxX && posY >= 0 && posY < maxY)
                        return inputs[posY][posX];
                    return ' ';
                }).ToArray())
            .Select(c => new string(c))
            .Count(e => e == "XMAS");
    }

    public bool IsMas(char[][] inputs, (int, int) dirs, int x, int y, int maxY, int maxX)
    {
        var seq = Enumerable.Range(0, 3).Select(i =>
        {
            var posX = x + dirs.Item1 * i;
            var posY = y + dirs.Item2 * i;

            if (posX >= 0 && posX < maxX && posY >= 0 && posY < maxY)
                return inputs[posY][posX];
            return ' ';
        }).ToArray();

        var str = new string(seq);

        return str == "MAS";
    }

    public int GetCountOccurrencesCruiseMas(char[][] inputs, int y, int x)
    {
        var maxX = inputs[0].Length;
        var maxY = inputs.Length;

        var test = _dir.Keys
            .Skip(4)
            .Select(dir =>
            {
                if (inputs[y][x] is not 'A') return false;

                var dir1 = _dir[dir];
                var dir2 = _dir[dir];
                var dir3 = _dir[dir];
                var x1 = x;
                var y1 = y;
                var x2 = x;
                var y2 = y;
                var x3 = x;
                var y3 = y;

                switch (dir)
                {
                    case "up-left":
                        dir2 = _dir["up-right"];
                        dir3 = _dir["down-left"];
                        x1 += 1;
                        y1 += 1;
                        x2 -= 1;
                        y2 += 1;
                        x3 += 1;
                        y3 -= 1;
                        break;
                    case "up-right":
                        dir2 = _dir["up-left"];
                        dir3 = _dir["down-right"];
                        x1 -= 1;
                        y1 += 1;
                        x2 += 1;
                        y2 += 1;
                        x3 -= 1;
                        y3 -= 1;
                        break;
                    case "down-left":
                        dir2 = _dir["down-right"];
                        dir3 = _dir["up-left"];
                        x1 += 1;
                        y1 -= 1;
                        x2 -= 1;
                        y2 -= 1;
                        y3 += 1;
                        x3 += 1;
                        break;
                    case "down-right":
                        dir2 = _dir["down-left"];
                        dir3 = _dir["up-right"];
                        x1 -= 1;
                        y1 -= 1;
                        x2 += 1;
                        y2 -= 1;
                        x3 -= 1;
                        y3 += 1;
                        break;
                }

                var isFirstMas = IsMas(inputs, dir1, x1, y1, maxY, maxX);
                var isSecondMas = IsMas(inputs, dir2, x2, y2, maxY, maxX);
                var isThirdMas = IsMas(inputs, dir3, x3, y3, maxY, maxX);
                return isFirstMas && (isSecondMas || isThirdMas);
            }).Any(e => e);

        if (!test) return 0;
        return 1;
    }

    public override void Solve()
    {
        var input = GetInput();

        var inputs = input.Split("\n")
            .Select(e => e.Select(c => c).ToArray())
            .ToArray();

        var countRows = inputs.Length;
        var countCols = inputs[0].Length;

        var result1 = 0;
        var result2 = 0;

        for (var y = 0; y < countRows; y++)
        for (var x = 0; x < countCols; x++)
        {
            result1 += GetCountOccurrencesXmas(inputs, y, x);
            result2 += GetCountOccurrencesCruiseMas(inputs, y, x);
        }


        Console.WriteLine($"result1 {result1}");
        Console.WriteLine($"result2 {result2}");
    }
}