namespace AdventOfCode._2024.Day10;

public class Solution : BaseSolution
{
    private readonly HashSet<(int x, int y, int startX, int startY)> posFound = [];
    private readonly List<(int x, int y)> posFound2 = [];
    private int solution1;

    public void getNextCells(List<List<int>> grid, (int x, int y) pos, (int x, int y) startPos)
    {
        var currentCell = grid[pos.y][pos.x];

        List<(int x, int y)> nextPos = [(-1, 0), (0, 1), (1, 0), (0, -1)];

        var nextPositions = nextPos
            .Select(p => (x: pos.x + p.x, y: pos.y + p.y))
            .Where(p => p.x >= 0 && p.x < grid[0].Count && p.y >= 0 && p.y < grid.Count)
            .Where(p => grid[p.y][p.x] == currentCell + 1 || (currentCell == 8 && grid[p.y][p.x] == 9))
            .Select(p => (p.x, p.y, grid[p.y][p.x]))
            .ToList();

        foreach (var p in nextPositions)
        {
            //if (!posFound.Add(p)) continue;
            if (p.Item3 == 9)
            {
                Console.WriteLine("Found 9 cells");
                solution1 += 1;
                posFound.Add((p.x, p.y, startPos.x, startPos.y));
                posFound2.Add((p.x, p.y));
                continue;
            }

            Console.WriteLine($"Found next cell {grid[p.y][p.x]} {p.x}, {p.y} ");
            ///count += getNextCells(grid, (p.x, p.y), count);
            getNextCells(grid, (p.x, p.y), startPos);
        }

        //return count;
        /*
        foreach (var next in nextPos)
        {
            var nextX = pos.x + next.x;
            var nextY = pos.y + next.y;

            var isInGrid = nextX >= 0 && nextX < grid[0].Count && nextY >= 0 && nextY < grid.Count;

            if (isInGrid && grid[nextY][nextX] == currentCell + 1)
            {
                Console.WriteLine($"{currentCell} {grid[nextY][nextX]}");

                //Console.WriteLine();
                getNextCells(grid, (nextX, nextY));
                //nextPossibleCells.Add((nextX, nextY));
            }
        }
        */

        //Console.WriteLine($"Solution 1: {solution1}");
        //return getNextCells(grid, nextPos);
    }

    public override void Solve()
    {
        var input = GetInput()
            .Split(Environment.NewLine)
            .Select((row, y) => row
                    .Select(cell => cell == '.' ? -1 : int.Parse(cell.ToString())).ToList()
                //.Select((cell, x) => (cell, pos: (x, y)))
            )
            .ToList();

        var startedCells = input
            .SelectMany((row, y) => row
                .Select((cell, x) => new { cell, pos = (x, y) })
                .ToList())
            .Where(cell => cell.cell == 0);

        //var solution = 0;


        foreach (var cell in startedCells) getNextCells(input, cell.pos, (cell.pos.x, cell.pos.y));

        Console.WriteLine($"solution {posFound.Count}");
        Console.WriteLine($"solution {posFound2.Count}");
        /*
        HashSet<(int x, int y)> cellReached = [];

        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[0].Count; x++)
            {
                var nextCells = getNextCells(input, (x, y));
            }
        }
        */
        Console.WriteLine("Day 10");
    }
}