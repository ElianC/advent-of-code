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
}