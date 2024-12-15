using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day15;

public class Solution : BaseSolution
{
    private readonly List<Size> _movements;
    private readonly Grid<char> _warehouse;
    private readonly Grid<char> _warehouse2;
    private Point _guardPos;
    private Point _guardPos2;

    public Solution()
    {
        var input = GetInput()
            .Split(Environment.NewLine + Environment.NewLine);

        _warehouse = new Grid<char>(input[0]);

        var input2 = input[0]
            .Split(Environment.NewLine)
            .Select(row =>
            {
                var updatedCells = row.Select(cell => cell switch
                {
                    '#' => "##",
                    'O' => "[]",
                    '.' => "..",
                    '@' => "@.",
                    _ => string.Empty
                }).Where(c => c != string.Empty);

                return string.Join(string.Empty, updatedCells);
            });

        _warehouse2 = new Grid<char>(string.Join(Environment.NewLine, input2));

        _guardPos = _warehouse.GetGrid()
            .Select(row => row.SingleOrDefault(cell => cell.Value == '@'))
            .Where(cell => cell != default)
            .Select(cell => cell.Point)
            .Single();
        _guardPos2 = _warehouse2.GetGrid()
            .Select(row => row.SingleOrDefault(cell => cell.Value == '@'))
            .Where(cell => cell != default)
            .Select(cell => cell.Point)
            .Single();
        _warehouse.UpdateGridPointValue(_guardPos, '.');
        _warehouse2.UpdateGridPointValue(_guardPos2, '.');

        _movements = input[1].Split(Environment.NewLine).SelectMany(row => row.Select(direction =>
                direction switch
                {
                    '^' => new Size(0, -1),
                    '>' => new Size(1, 0),
                    'v' => new Size(0, 1),
                    '<' => new Size(-1, 0),
                    _ => Size.Empty
                })
            )
            .Where(size => size != Size.Empty)
            .ToList();
    }

    private void TryMoveBoxes(Point startPos, Size direction)
    {
        var isBoxMoveable = false;
        var nextCellBox = startPos;
        char valueCell;

        do
        {
            nextCellBox += direction;
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

    private bool IsBoxMoveable2(Point point, Size direction)
    {
        var partBox = _warehouse2.GetGridPointValue(point);
        var otherPartBoxPoint = point + new Size(1, 0);
        if (partBox == ']') otherPartBoxPoint = point + new Size(-1, 0);

        var testPartBox = _warehouse2.GetGridPointValue(point + direction);
        var testOtherPartBox = _warehouse2.GetGridPointValue(otherPartBoxPoint + direction);

        if (testPartBox == '#' || testOtherPartBox == '#') return false;
        if (testPartBox == '.' && testOtherPartBox == '.') return true;

        var shouldTestPartBox = testPartBox is ']' or '[';
        var shouldTestOtherPartBox = testOtherPartBox is ']' or '[';
        var isMoveablePartBox = !shouldTestPartBox || IsBoxMoveable2(point + direction, direction);
        var isMoveableOtherPartBox =
            !shouldTestOtherPartBox || IsBoxMoveable2(otherPartBoxPoint + direction, direction);

        return isMoveablePartBox && isMoveableOtherPartBox;
    }

    private void TryMoveBox2(Point point, Size direction)
    {
        var partBox = _warehouse2.GetGridPointValue(point);
        var otherPartBoxPoint = point + new Size(1, 0);
        if (partBox == ']') otherPartBoxPoint = point + new Size(-1, 0);

        var shouldMovePartBox = _warehouse2.GetGridPointValue(point + direction) == (partBox == ']' ? '[' : ']');
        var shouldMoveOtherPartBox =
            _warehouse2.GetGridPointValue(otherPartBoxPoint + direction) == (partBox == ']' ? ']' : '[');

        if (shouldMovePartBox) TryMoveBox2(point + direction, direction);
        if (shouldMoveOtherPartBox) TryMoveBox2(otherPartBoxPoint + direction, direction);

        if (_warehouse2.GetGridPointValue(point + direction) == partBox) TryMoveBox2(point + direction, direction);

        _warehouse2.UpdateGridPointValue(point, '.');
        _warehouse2.UpdateGridPointValue(point + direction, partBox);
        _warehouse2.UpdateGridPointValue(otherPartBoxPoint, '.');
        _warehouse2.UpdateGridPointValue(otherPartBoxPoint + direction, partBox == '[' ? ']' : '[');
    }

    private void TryMoveBoxes2(Point startPos, Size direction)
    {
        Console.WriteLine($"{direction} {startPos} {startPos - direction}");
        if (direction.Width != 0)
        {
            Console.WriteLine("move horizontal ");
            var isBoxMoveable = false;
            var nextCellBox = startPos;
            char valueCell;

            do
            {
                nextCellBox += direction;
                valueCell = _warehouse2.GetGridPointValue(nextCellBox);

                if (valueCell == '.') isBoxMoveable = true;
            } while (valueCell is '[' or ']');

            if (isBoxMoveable)
            {
                var startPoint = startPos + direction;
                var maxX = Math.Abs(nextCellBox.X - startPoint.X);

                for (var i = 0; i <= maxX; startPoint += direction, i++)
                {
                    var toRight = direction.Width == 1;
                    var nextChar = i % 2 != 0 ? '[' : ']';
                    if (toRight) nextChar = i % 2 == 0 ? '[' : ']';
                    _warehouse2.UpdateGridPointValue(startPoint, nextChar);
                }

                _guardPos2 = startPos;
                _warehouse2.UpdateGridPointValue(startPos, '.');
            }
        }
        else
        {
            Console.WriteLine("move vertical");

            var isBoxMoveable = IsBoxMoveable2(startPos, direction);
            if (isBoxMoveable)
            {
                TryMoveBox2(startPos, direction);
                //
                _guardPos2 = startPos;
                ;
            }
        }
    }


    private void TryToMoveGuard(Size movement)
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

    private void TryToMoveGuard2(Size movement)
    {
        var nextGuardPos = _guardPos2 + movement;

        var nextCellValue = _warehouse2.GetGridPoint(nextGuardPos);
        //Debug();

        switch (nextCellValue.Value)
        {
            case '.':
                _guardPos2 = nextGuardPos;
                break;
            case '#':
                break;
            case '[':
            case ']':
                TryMoveBoxes2(nextGuardPos, movement);
                break;
        }
    }

    public override void Solve()
    {
        foreach (var movement in _movements)
        {
            TryToMoveGuard(movement);
            TryToMoveGuard2(movement);
        }

        var response1 = _warehouse.GetGrid()
            .SelectMany(row => row.Where(cell => cell.Value == 'O'))
            .Select(cell => cell.Point)
            .Sum(point => 100 * point.Y + point.X);
        var response2 = _warehouse2.GetGrid()
            .SelectMany(row => row.Where(cell => cell.Value == '['))
            .Select(cell => cell.Point)
            .Sum(point => 100 * point.Y + point.X);

        Console.WriteLine($"GPS: {response1}");
        Console.WriteLine($"GPS: {response2}");
    }

    public void Debug()
    {
        foreach (var row in _warehouse2.GetGrid())
        {
            var line = "";
            foreach (var cell in row)
                if (cell.Point == _guardPos2) line += '@';
                else line += cell.Value;

            Console.WriteLine(line);
        }

        Console.WriteLine("");
    }
}