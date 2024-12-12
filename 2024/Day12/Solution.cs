using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day12;

public class Solution : BaseSolution
{
    private readonly Dictionary<int, (char cell, List<Point>)> _dict = new();
    private readonly Grid<char> _grid;
    public readonly List<Size> NeighbourCellsOffset = [new(0, -1), new(0, 1), new(-1, 0), new(1, 0)];

    public readonly List<Size> NeighbourDiagonalCellsOffset = [new(-1, -1), new(1, -1), new(-1, 1), new(1, 1)];

    public HashSet<Point> CellsAlreadyGrouped = [];

    public Solution()
    {
        _grid = new Grid<char>(GetInput());
    }

    private void CheckAdjacentCells(Point cellToCheck, char target, int dictIndex)
    {
        var neighbourCells = NeighbourCellsOffset
            .Select(size => cellToCheck + size)
            .Where(point => !CellsAlreadyGrouped.Contains(point))
            .Where(point => _grid.ContainsPoint(point))
            .Where(point => _grid[point.Y][point.X].value == target)
            .ToList();

        foreach (var neightboorPoint in neighbourCells)
        {
            if (!CellsAlreadyGrouped.Add(neightboorPoint)) continue;
            _dict[dictIndex].Item2.Add(neightboorPoint);
            CheckAdjacentCells(neightboorPoint, target, dictIndex);
        }
    }

    public override void Solve()
    {
        var id = 0;
        long solution1 = 0;
        long solution2 = 0;

        foreach (var cell in _grid.GetGrid().SelectMany(row => row))
        {
            if (!CellsAlreadyGrouped.Add(cell.point)) continue;

            var newIndex = id++;
            _dict.TryAdd(newIndex, (cell.value, [cell.point]));
            CheckAdjacentCells(cell.point, cell.value, newIndex);
        }

        foreach (var elem in _dict)
        {
            var area = elem.Value.Item2.Count;
            var perimeter = elem.Value.Item2.Select(point =>
                NeighbourCellsOffset
                    .Select(size => point + size)
                    .Where(el =>
                    {
                        if (_grid.ContainsPoint(el))
                            return _grid[el.Y][el.X].value != elem.Value.cell;
                        return true;
                    })
                    .Count()
            ).Sum();

            var sides = elem.Value.Item2.Sum(GetCountEdges);

            solution1 += perimeter * area;
            solution2 += sides * area;
        }

        Console.WriteLine(solution1);
        Console.WriteLine(solution2);
    }

    private int GetCountEdges(Point cell)
    {
        var value = _grid[cell.Y][cell.X].value;

        var directNeighbors = NeighbourCellsOffset
            .Select(coord => cell + coord)
            .ToList();

        var diagonalNeighbors = NeighbourDiagonalCellsOffset
            .Select(size => cell + size)
            .ToList();

        char GetCellValue(Point point)
        {
            if (_grid.ContainsPoint(point))
                return _grid[point.Y][point.X].value;
            return ' ';
        }

        var directValues = directNeighbors.Select(GetCellValue).ToArray();
        var diagonalValues = diagonalNeighbors.Select(GetCellValue).ToArray();

        // Calculate concave vertices
        var concave = new[]
        {
            diagonalValues[0] != value && directValues[0] == value && directValues[2] == value, // UL
            diagonalValues[1] != value && directValues[0] == value && directValues[3] == value, // UR
            diagonalValues[3] != value && directValues[1] == value && directValues[3] == value, // DR
            diagonalValues[2] != value && directValues[1] == value && directValues[2] == value // DL
        }.Count(isConcave => isConcave);

        var borderCount = directValues.Count(v => v != value);

        if (borderCount == 4) return 4;

        if (borderCount == 3) return 2 + concave;


        var isConvex = borderCount == 2 &&
                       !(directValues[0] == directValues[1] && directValues[0] == value) &&
                       !(directValues[2] == directValues[3] && directValues[2] == value);
        var convex = isConvex ? 1 : 0;

        return concave + convex;
    }
}