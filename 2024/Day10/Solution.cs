namespace AdventOfCode._2024.Day10;

public class Solution : BaseSolution
{
    private List<(int x, int y)> posFound = [];
    private int response1 = 0;
    
    public void getNextCells(List<List<int>> grid, (int x, int y) pos)
    {
        var currentCell = grid[pos.y][pos.x];
        //var nextPossibleCells = new List<(int x, int y)>();
        
        if (currentCell == 9)
        {
            Console.WriteLine($"Found 8 cells at {pos.x}, {pos.y} ");
            posFound.Add(pos);
            return;
        }
        
        List<(int x, int y)> nextPos = [(-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0),  (1, -1), (0, -1)];

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
        
        //return getNextCells(grid, nextPos);
    }
    
    public override void Solve()
    {
        var input = GetInput()
            .Split(Environment.NewLine)
            .Select((row, y) => row
                .Select(cell => int.Parse(cell.ToString())).ToList()
                //.Select((cell, x) => (cell, pos: (x, y)))
                )
            .ToList();

        var startedCells = input
            .SelectMany((row, y) => row
                .Select((cell, x) => new { cell, pos = (x, y) })
                .ToList())
            .Where(cell => cell.cell == 0);

        var solution = 0;

        
        
        foreach (var cell in startedCells)
        {
            getNextCells(input, cell.pos);
        }
        
        Console.WriteLine($"solution {posFound.Count}");
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