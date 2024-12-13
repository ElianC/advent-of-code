using System.Drawing;

namespace AdventOfCode.Utils;

public class Grid<T>
{
    private readonly List<List<(Point Point, T Value)>> _grid;

    public Grid(string input)
    {
        _grid = input
            .Split(Environment.NewLine)
            .Select((row, y) => row
                .Select((cellValue, x) =>
                {
                    var point = new Point(x, y);
                    var value = (T)Convert.ChangeType(cellValue, typeof(T));

                    return (point, value);
                }).ToList())
            .ToList();
    }

    public List<(Point Point, T Value)> this[int row] => _grid[row].ToList();
    public T this[int row, int col] => _grid[row][col].Value;

    public bool ContainsPoint(Point p)
    {
        if (p.X < 0 || p.X >= _grid[0].Count) return false;
        return p.Y >= 0 && p.Y < _grid.Count;
    }

    public List<List<(Point Point, T Value)>> GetGrid()
    {
        return _grid;
    }

    public (Point Point, T Value) GetGridPoint(Point point)
    {
        if (ContainsPoint(point) is false) throw new Exception($"Point is not in grid {point.ToString()}");

        return _grid[point.Y][point.X];
    }

    public T GetGridPointValue(Point point)
    {
        if (ContainsPoint(point) is false) throw new Exception($"Point is not in grid {point.ToString()}");

        return _grid[point.Y][point.X].Value;
    }

    public void UpdateGridPointValue(Point point, T value)
    {
        if (ContainsPoint(point) is false) throw new Exception($"Point is not in grid {point.ToString()}");

        var pointSelected = _grid[point.Y].ElementAt(point.X);
        pointSelected.Value = value;

        _grid[point.Y][point.X] = pointSelected;
    }
}