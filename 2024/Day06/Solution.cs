namespace AdventOfCode._2024.Day06;

public class Solution : BaseSolution
{
    public Solution(string day, string year) : base(day, year)
    {
    }

    enum Directions
    {
        Forward,
        Right,
        Backward,
        Left,
    }

    private readonly List<(Directions, string)> _cellGuardMatchDirection = [
        (Directions.Forward, "^"),
        (Directions.Right, ">"),
        (Directions.Backward, "v"),
        (Directions.Left, "<")];    

    private Directions GetNextDirection(Directions direction)
    {
        return direction switch
        {
            Directions.Forward => Directions.Right,
            Directions.Right => Directions.Backward,
            Directions.Backward => Directions.Left,
            Directions.Left => Directions.Forward,
        };
    }

    private static (int x, int y, string value)? GetNextCellInfos(Directions direction, List<List<string>> input, int currX, int currY)
    {
        var boundY = input.Count - 1;
        var boundX = input[0].Count - 1;
        var nextY = currY;
        var nextX = currX;
        
        switch (direction)
        {
            case Directions.Forward:
                nextY--;
                if (nextY < 0) return null;
                break;
            case Directions.Right:
                nextX++;
                if (nextX > boundX) return null;
                break;
            case Directions.Backward:
                nextY++;
                if (nextY > boundY) return null;
                break;
            case Directions.Left:
                nextX--;
                if (nextX > boundX) return null;
                break;
        }
        
        return (nextX, nextY, input[nextY][nextX]);
    }
    
    public override void Solve()
    {
        var input = GetInput()
            .Split(Environment.NewLine)
            .Select(row => row.Select(e => e.ToString()).ToList())
            .ToList();

        var solution1 = 0;
        
        var guardY = input.FindIndex(row => 
            row.Any(cell => _cellGuardMatchDirection.Any(e => e.Item2 == cell)));
        var guardX = input[guardY].FindIndex(cell => _cellGuardMatchDirection.Any(e => e.Item2 == cell));
        
        var currentDirection = _cellGuardMatchDirection
            .Where(e => e.Item2 == input[guardY][guardX])
            .Select(e => e.Item1)
            .Single();
        
        var visited = new HashSet<(int x, int y)>();
        visited.Add((guardX, guardY));
        
        do
        {
            var nextCell = GetNextCellInfos(currentDirection, input, guardX, guardY);

            if (nextCell is not null && nextCell.Value.value == "#")
            {
                currentDirection = GetNextDirection(currentDirection);
                nextCell = GetNextCellInfos(currentDirection, input, guardX, guardY);
            }
            
            if (nextCell is null) break;

            guardX = nextCell.Value.x;
            guardY = nextCell.Value.y;
            
            visited.Add((guardX, guardY));
        } while (true);

        solution1 += visited.Count;
        
        Console.WriteLine($"Solution 1: {solution1}");
    }
}