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

    private readonly Dictionary<string, string> _directionsDictionary = new (){
        {Directions.Forward.ToString(), "^"},
        {Directions.Right.ToString(), ">"},
        {Directions.Backward.ToString(), "v"},
        {Directions.Left.ToString(), "<"}};    
    /*private readonly Dictionary<string, string> _directionsDictionary = new (){
        {Directions.Forward.ToString(), "^"},
        {Directions.Right.ToString(), ">"},
        {Directions.Backward.ToString(), "v"},
        {Directions.Left.ToString(), "<"}};*/

    private Directions GetNextDirection(string direction)
    {
        return direction switch
        {
            nameof(Directions.Forward) => Directions.Right,
            nameof(Directions.Right) => Directions.Backward,
            nameof(Directions.Backward) => Directions.Left,
            nameof(Directions.Left) => Directions.Forward,
        };
    }

    private static string? GetNextCellValue(Directions direction, List<List<string>> input, int currX, int currY)
    {
        var boundY = input.Count - 1;
        var boundX = input[0].Count - 1;
        var nextY = currY;
        var nextX = currX;
        
        switch (direction)
        {
            case Directions.Forward:
                if (nextY == boundY) return null;
                nextY++;
                break;
            case Directions.Right:
                if (nextX == boundX) return null;
                nextX++;
                break;
            case Directions.Backward:
                if (currY == 0) return null;
                nextY--;
                break;
            case Directions.Left:
                if (currX == 0) return null;
                nextX--;
                break;
        }
        
        return input[nextY][nextX];
    }
    
    public override void Solve()
    {
        var input = GetInput()
            .Split(Environment.NewLine)
            .Select(row => row.Select(e => e.ToString()).ToList())
            .ToList();

        var solution1 = 0;
        
        var guardY = input.FindIndex(row => 
            row.Any(cell => _directionsDictionary.ContainsValue(cell)));
        var guardX = input[guardY].FindIndex(cell => _directionsDictionary.ContainsValue(cell));
        var boundY = input.Count - 1;
        var boundX = input[0].Count - 1;
        
        
        var currentDirection = _directionsDictionary
            .Single(e => e.Value == input[guardY][guardX]);
        //var nextDirection = GetNextDirection(currentDirection.Key);
        
        do
        {
            /*var isNextCellOutBound = guardY == boundY || guardX == boundX;
            solution1++;
            var nextCellValue = GetNextCellValue(nextDirection, input, guardX, guardY);

            if (nextCellValue == "#")
            {
               // currentDirection = nextDirection;
            }
            Console.WriteLine(nextCellValue);
            */
            break;
        } while (true);
        
        Console.WriteLine($"{solution1} {guardY} {guardX} {currentDirection.Key} {nextDirection}");
    }
}