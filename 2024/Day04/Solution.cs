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
        var count = 0;
        var boundX = inputs[0].Length;
        var boundY = inputs.Length;

        foreach (var (dirX, dirY) in _dir.Values.ToArray())
        {
            var seq = Enumerable.Range(0, 4)
                .Select(i =>
                {
                    var posX = x + dirX * i;
                    var posY = y + dirY * i;

                    if (posX >= 0 && posX < boundX && posY >= 0 && posY < boundY)
                        return inputs[posY][posX];
                    return '_';
                }).ToArray();
            var str = new string(seq);

            if (str is "XMAS") count++;
        }

        return count;
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