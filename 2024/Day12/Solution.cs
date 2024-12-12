namespace AdventOfCode._2024.Day12;

public class Solution : BaseSolution
{
    private readonly Dictionary<int, (char cell, List<(int x, int y)>)> Dict = new();
    public readonly List<(int x, int y)> NeighbourCellsOffset = [(0, -1), (0, 1), (-1, 0), (1, 0)];

    public readonly List<(int x, int y)> NeighbourDiagonalCellsOffset =
    [
        (x: -1, y: -1),
        (x: 1, y: -1),
        (x: -1, y: 1),
        (x: 1, y: 1)
    ];

    public HashSet<(int x, int y)> CellsAlreadyGrouped = [];

    private void CheckAdjacentCells((int x, int y) cellToCheck, char target, int dictIndex,
        List<List<(char cell, (int x, int y) pos)>> input)
    {
        var neighbourCells = NeighbourCellsOffset
            .Select(coord => (x: cellToCheck.x + coord.x, y: cellToCheck.y + coord.y))
            .Where(cell => !CellsAlreadyGrouped.Contains(cell))
            .Where(e => e is { x: >= 0, y: >= 0 })
            .Where(e => e.x < input[0].Count && e.y < input.Count)
            .Where(e => input[e.y][e.x].cell == target)
            .ToList();

        foreach (var neightboor in neighbourCells)
        {
            if (!CellsAlreadyGrouped.Add(neightboor)) continue;
            Dict[dictIndex].Item2.Add(neightboor);
            CheckAdjacentCells(neightboor, target, dictIndex, input);
        }
    }

    public override void Solve()
    {
        var id = 0;
        var input = GetInput()
            .Split(Environment.NewLine)
            .Select((row, y) => row.Select((cell, x) => (cell, pos: (x, y))).ToList())
            .ToList();

        long solution1 = 0;
        long solution2 = 0;


        foreach (var row in input)
        foreach (var cell in row)
        {
            if (!CellsAlreadyGrouped.Add(cell.pos)) continue;

            var newIndex = id++;
            Dict.TryAdd(newIndex, (cell.cell, [cell.pos]));
            CheckAdjacentCells(cell.pos, cell.cell, newIndex, input);
        }

        foreach (var elem in Dict)
        {
            var area = elem.Value.Item2.Count;


            var perimeter = elem.Value.Item2.Select(e => NeighbourCellsOffset
                .Select(coord => (x: e.x + coord.x, y: e.y + coord.y))
                .Where(el =>
                {
                    if (el is { x: >= 0, y: >= 0 } && el.x < input[0].Count && el.y < input.Count)
                        return input[el.y][el.x].cell != elem.Value.cell;
                    return true;
                })
                .Count()
            ).Sum();

            var sides = elem.Value.Item2.Sum(cell => GetCountEdges(cell, input));

            solution1 += perimeter * area;
            solution2 += sides * area;
        }

        Console.WriteLine(solution1);
        Console.WriteLine(solution2);
    }

    private int GetCountEdges((int x, int y) cell, List<List<(char cell, (int x, int y) pos)>> input)
    {
        var value = input[cell.y][cell.x].cell;

        var directNeighbors = NeighbourCellsOffset
            .Select(coord => (x: cell.x + coord.x, y: cell.y + coord.y))
            .ToList();

        var diagonalNeighbors = NeighbourDiagonalCellsOffset
            .Select(coord => (x: cell.x + coord.x, y: cell.y + coord.y))
            .ToList();

        char GetCellValue((int x, int y) pos)
        {
            if (pos.x >= 0 && pos.y >= 0 && pos.x < input[0].Count && pos.y < input.Count)
                return input[pos.y][pos.x].cell;
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

        if (borderCount == 4) return 4 + concave;

        if (borderCount == 3) return 2 + concave;

        var convex = 0;

        var isConvex = borderCount == 2 &&
                       !(directValues[0] == directValues[1] && directValues[0] == value) &&
                       !(directValues[2] == directValues[3] && directValues[2] == value);
        if (isConvex) convex++;

        return concave + convex;
    }
}