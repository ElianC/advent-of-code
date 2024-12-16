using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day16;

public class TreeNode
{
    public Point Value { get; set; }
    public TreeNode? Branch1 { get; set; }
    public TreeNode? Branch2 { get; set; }
    public TreeNode? Branch3 { get; set; }
    public TreeNode? Branch4 { get; set; }
}

public class Solution : BaseSolution
{
    private readonly HashSet<Point> _deadEnds = [];
    private readonly Point _end;
    private readonly HashSet<Point> _exploredCells = [];
    private readonly Grid<char> _grid;

    private readonly List<Size> _offsets =
    [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    private readonly Point _start;
    private readonly Dictionary<Point, Point> _visitedPoints = new();

    public Solution()
    {
        _grid = new Grid<char>(GetInput());

        _start = _grid.GetGrid()
            .SelectMany(e => e)
            .Where(e => e.Value == 'S')
            .Select(e => e.Point)
            .Single();

        _end = _grid.GetGrid()
            .SelectMany(e => e)
            .Where(e => e.Value == 'E')
            .Select(e => e.Point)
            .Single();
    }

    public TreeNode? AddLeafs(Point? point)
    {
        if (point == _end) return new TreeNode { Value = _end };
        if (point == null) return null;

        var adjacentsPoints = GetAdjacentCells(point.Value);
        if (!adjacentsPoints.Any()) return null;

        _exploredCells.Add(point.Value);
        return new TreeNode
        {
            Value = point.Value,
            Branch1 = adjacentsPoints.Count >= 1 ? AddLeafs(adjacentsPoints[0]) : null,
            Branch2 = adjacentsPoints.Count >= 2 ? AddLeafs(adjacentsPoints[1]) : null,
            Branch3 = adjacentsPoints.Count >= 3 ? AddLeafs(adjacentsPoints[2]) : null,
            Branch4 = adjacentsPoints.Count == 4 ? AddLeafs(adjacentsPoints[3]) : null
        };
    }

    private List<Point> GetAdjacentCells(Point point)
    {
        return _offsets
            .Select(off => _grid.GetGridPoint(point + off))
            .Where(el => !_exploredCells.Contains(el.Point))
            .Where(el => el.Value != '#')
            .Select(el => el.Point)
            .ToList();
    }

    private bool CanReachEnd(Point start)
    {
        Queue<Point> queue = new();
        _exploredCells.Add(start);
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var point = queue.Dequeue();
            if (point == _end)
                return true;

            var adjacentCells = GetAdjacentCells(point);
            foreach (var adjacentCell in adjacentCells)
                if (_exploredCells.Add(adjacentCell))
                {
                    _visitedPoints.Add(adjacentCell, point);
                    queue.Enqueue(adjacentCell);
                }
        }

        return false;
    }

    public bool CheckTreeLeafs(TreeNode node)
    {
        if (node == null) return false;

        if (node.Value == _end) return true;

        Console.WriteLine(node.Value);
        Console.WriteLine("hello world");
        var answer = false;
        if (node.Branch1 != null)
        {
            var testTree = CheckTreeLeafs(node.Branch1);
            if (testTree) return true;
        }

        if (node.Branch2 != null)
        {
            var testTree = CheckTreeLeafs(node.Branch2);
            if (testTree) return true;
        }

        if (node.Branch3 != null)
        {
            var testTree = CheckTreeLeafs(node.Branch3);
            if (testTree) return true;
        }

        if (node.Branch4 != null)
        {
            var testTree = CheckTreeLeafs(node.Branch4);
            if (testTree) return true;
        }

        return false;
    }

    public override void Solve()
    {
        //CanReachEnd(_start);
        var tree = AddLeafs(_start);
        CheckTreeLeafs(tree);

        /*
        var currentPoint = _visitedPoints[_end];

        while (currentPoint != _start)
        {
            Debug(currentPoint);
            currentPoint = _visitedPoints[currentPoint];
        }
        */
    }

    private void Debug(Point point)
    {
        var output = "";
        foreach (var row in _grid.GetGrid())
        {
            var line = "";
            foreach (var cell in row)
                if (cell.Point == point) line += '@';
                else line += cell.Value;

            output += line + "\n";
        }

        Console.Clear();

        Console.Write("\r{0}", output);
        Thread.Sleep(500);
    }
}