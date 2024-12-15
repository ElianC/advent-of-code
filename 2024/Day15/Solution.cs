using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day15;

public class Solution : BaseSolution
{
    public enum WareHouseElements
    {
        Wall = '#',
        Floor = '.',
        Box = 'O'
    }

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

    public override void Solve()
    {
        foreach (var movement in _movements)
        {
            var nextGuardPos = _guardPos + movement;
            var nextCellValue = _warehouse.GetGridPoint(nextGuardPos);

            switch (nextCellValue.Value)
            {
                case '.':
                {
                    _guardPos = nextGuardPos;

                    break;
                }
                case '#':
                {
                    break;
                }
                case 'O':
                {
                    var isBoxMoveable = false;
                    var nextCellBox = nextGuardPos;
                    char valueCell;

                    do
                    {
                        nextCellBox += movement;
                        valueCell = _warehouse.GetGridPointValue(nextCellBox);

                        if (valueCell == '.') isBoxMoveable = true;
                    } while (valueCell is 'O');

                    if (isBoxMoveable)
                    {
                        _warehouse.UpdateGridPointValue(nextGuardPos, '.');
                        _warehouse.UpdateGridPointValue(nextCellBox, 'O');
                        _guardPos = nextGuardPos;
                    }

                    break;
                }
            }
        }

        foreach (var row in _warehouse.GetGrid())
        {
            var _row = "";
            foreach (var VARIABLE in row)
            {
                _row += VARIABLE.Value;
                ;
            }

            ;

            Console.WriteLine(_row);
        }

        var response1 = _warehouse.GetGrid()
            .SelectMany(row => row.Where(cell => cell.Value == 'O'))
            .Select(cell => cell.Point)
            .Sum(point => 100 * point.Y + point.X);

        Console.WriteLine($"GPS: {response1}");
    }
}