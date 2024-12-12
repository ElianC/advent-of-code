namespace AdventOfCode._2024.Day12;

public class Solution : BaseSolution
{
    private readonly Dictionary<int, (char cell, List<(int x, int y)>)> Dict = new();
    public readonly List<(int x, int y)> NeighbourCellsOffset = [(-1, 0), (0, 1), (1, 0), (0, -1)];
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

            solution1 += perimeter * area;
            //Console.WrfiteLine($"{elem.Value.Item2.Count} {perimeter}");
        }

        Console.WriteLine(solution1);
    }
}