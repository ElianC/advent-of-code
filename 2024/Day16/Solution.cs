using System.Drawing;
using AdventOfCode.Utils;

namespace AdventOfCode._2024.Day16;

public class Solution : BaseSolution
{
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


    /*
        Breadth-first search

        1  procedure BFS(G, root) is
        2      let Q be a queue
        3      label root as explored
        4      Q.enqueue(root)
        5      while Q is not empty do
        6          v := Q.dequeue()
        7          if v is the goal then
        8              return v
        9          for all edges from v to w in G.adjacentEdges(v) do
        10              if w is not labeled as explored then
        11                  label w as explored
        12                  w.parent := v
        13                  Q.enqueue(w)
    */
    private List<Point> GetAdjacentCells(Point point)
    {
        return _offsets
            .Select(off => _grid.GetGridPoint(point + off))
            .Where(el => el.Value != '#')
            .Select(el => el.Point)
            .ToList();
    }

    private void FindBestPathMaze(Point start)
    {
        Queue<Point> queue = new();
        _exploredCells.Add(start);
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var point = queue.Dequeue();
            if (point == _end)
                break;

            var adjacentCells = GetAdjacentCells(point);
            foreach (var adjacentCell in adjacentCells)
                if (_exploredCells.Add(adjacentCell))
                {
                    _visitedPoints.Add(adjacentCell, point);
                    queue.Enqueue(adjacentCell);
                }
        }
    }

    public override void Solve()
    {
        FindBestPathMaze(_start);

        var currentPoint = _visitedPoints[_end];

        while (currentPoint != _start)
        {
            Debug(currentPoint);
            currentPoint = _visitedPoints[currentPoint];
        }
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