using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day15;

public class Solution : BaseSolution
{
    private readonly List<Size> _movements;
    private readonly Grid<char> _warehouse;
    private Point _guardPos;

    public Solution()
    {
        var input = GetInput()
            .Split(Environment.NewLine + Environment.NewLine);

        _warehouse = new Grid<char>(input[0]);
        _guardPos = _warehouse.GetGrid()
            .Select(row => row.SingleOrDefault(cell => cell.Value == '@'))
            .Where(cell => cell != default)
            .Select(cell => cell.Point)
            .Single();
        _warehouse.UpdateGridPointValue(_guardPos, '.');
        _movements = input[1].Split(Environment.NewLine).SelectMany(row => row.Select(direction =>
                direction switch
                {
                    '^' => new Size(0, -1),
                    '>' => new Size(1, 0),
                    'v' => new Size(0, 1),
                    '<' => new Size(-1, 0),
                    _ => new Size(999, 999)
                })
            )
            .Where(size => size != new Size(999, 999))
            .ToList();
    }

    private void TryMoveBoxes(Point startPos, Size movement)
    {
        var isBoxMoveable = false;
        var nextCellBox = startPos;
        char valueCell;

        do
        {
            nextCellBox += movement;
            valueCell = _warehouse.GetGridPointValue(nextCellBox);

            if (valueCell == '.') isBoxMoveable = true;
        } while (valueCell is 'O');

        if (isBoxMoveable)
        {
            _warehouse.UpdateGridPointValue(startPos, '.');
            _warehouse.UpdateGridPointValue(nextCellBox, 'O');
            _guardPos = startPos;
        }
    }

    public override void Solve()
    {
        foreach (var movement in _movements)
        {
            var nextGuardPos = _guardPos + movement;
            var nextCellValue = _warehouse.GetGridPoint(nextGuardPos);

            switch (nextCellValue.Value)
            {
                case '.':
                    _guardPos = nextGuardPos;
                    break;
                case '#':
                    break;
                case 'O':
                    TryMoveBoxes(nextGuardPos, movement);
                    break;
            }
        }

        var response1 = _warehouse.GetGrid()
            .SelectMany(row => row.Where(cell => cell.Value == 'O'))
            .Select(cell => cell.Point)
            .Sum(point => 100 * point.Y + point.X);

        Console.WriteLine($"GPS: {response1}");
    }
}