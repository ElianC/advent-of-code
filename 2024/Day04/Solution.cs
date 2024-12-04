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

    private int Search(char[][] inputs, int x, int y)
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
                    return '_';
                }).ToArray())
            .Select(c => new string(c))
            .Count(e => e == "XMAS");
    }

    public override void Solve()
    {
        var input = GetInput();

        var inputs = input.Split("\n")
            .Select(e => e.Select(c => c).ToArray()).ToArray();

        var countRows = inputs.Length;
        var countCols = inputs[0].Length;

        var result = 0;

        for (var i = 0; i < countRows; i++)
        for (var j = 0; j < countCols; j++)
            result += Search(inputs, i, j);

        Console.WriteLine($"{result}");
    }
}