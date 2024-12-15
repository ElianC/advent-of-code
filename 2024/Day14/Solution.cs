using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day14;

public class Solution : BaseSolution
{
    private readonly Point boundaries = new(101, 103);

    public override void Solve()
    {
        var regex = new Regex(@"-?\d+");

        var robotsLocations = GetInput()
            .Split(Environment.NewLine)
            .Select(row =>
            {
                var match = regex.Matches(row)
                    .Select(e => e.ToString())
                    .Select(int.Parse)
                    .ToArray();

                var pos = new Point(match[0], match[1]);
                var vel = new Size(match[2], match[3]);
                return (pos, vel);
            })
            .ToList();

        var timeLeft = 10000;
        var timeSpent = 1;
        var timeSpentToFindChristmasTree = 0;

        var easterBunnyHeadquarter = GenerateDictionnary(boundaries, robotsLocations);
        Dictionary<Point, List<Size>> response1Dict = new();
        for (; timeSpent <= timeLeft; timeSpent++)
        {
            List<(Point pos, Size vel)> newRobotPos = [];

            var robotsLoc = easterBunnyHeadquarter
                .Where(e => e.Value.Count >= 1)
                .Select(e => (e.Key, e.Value))
                .ToArray();

            foreach (var robotLoc in robotsLoc)
            foreach (var robotVel in robotLoc.Value)
            {
                var newLoc = robotLoc.Key + robotVel;

                if (newLoc is { X: >= 0, Y: >= 0 } && newLoc.X < boundaries.X && newLoc.Y < boundaries.Y)
                {
                    newRobotPos.Add((newLoc, robotVel));
                }
                else
                {
                    var posX = newLoc.X;
                    var posY = newLoc.Y;

                    if (posX < 0) posX = boundaries.X + posX;
                    if (posX >= boundaries.X) posX = posX - boundaries.X;
                    if (posY < 0) posY = boundaries.Y + posY;
                    if (posY >= boundaries.Y) posY = posY - boundaries.Y;

                    newRobotPos.Add((new Point(posX, posY), robotVel));
                }
            }

            try
            {
                easterBunnyHeadquarter = GenerateDictionnary(boundaries, newRobotPos);
            }
            catch (Exception)
            {
                timeSpentToFindChristmasTree = timeSpent;
                break;
            }

            if (timeSpent == 100) response1Dict = easterBunnyHeadquarter;
        }

        var semiBoundaryX = (boundaries.X - 1) / 2;
        var semiBoundaryY = (boundaries.Y - 1) / 2;

        var quad1Min = new Point(0, 0);
        var quad1Max = new Point(semiBoundaryX, semiBoundaryY);
        var quad2Min = new Point(semiBoundaryX + 1, 0);
        var quad2Max = new Point(2 * semiBoundaryX + 1, semiBoundaryY);
        var quad3Min = new Point(0, semiBoundaryY + 1);
        var quad3Max = new Point(semiBoundaryX, 2 * semiBoundaryY + 1);
        var quad4Min = new Point(semiBoundaryX + 1, semiBoundaryY + 1);
        var quad4Max = new Point(2 * semiBoundaryX + 1, 2 * semiBoundaryY + 1);

        var quad1Count = 0;
        var quad2Count = 0;
        var quad3Count = 0;
        var quad4Count = 0;

        foreach (var robot in response1Dict)
            if (robot.Value.Any())
            {
                if (AreInBoundaries(robot.Key, quad1Min, quad1Max)) quad1Count += robot.Value.Count();

                if (AreInBoundaries(robot.Key, quad2Min, quad2Max)) quad2Count += robot.Value.Count();

                if (AreInBoundaries(robot.Key, quad3Min, quad3Max)) quad3Count += robot.Value.Count();

                if (AreInBoundaries(robot.Key, quad4Min, quad4Max)) quad4Count += robot.Value.Count();
            }

        Console.WriteLine($"Safety factor: {quad1Count * quad2Count * quad3Count * quad4Count}");
        Console.WriteLine($"Time Spent to find the Easter eggs: {timeSpentToFindChristmasTree}");
    }

    private bool AreInBoundaries(Point point, Point boundariesMin, Point boundariesMax)
    {
        if (point.X >= boundariesMin.X && point.Y >= boundariesMin.Y &&
            point.X < boundariesMax.X && point.Y < boundariesMax.Y) return true;
        return false;
    }

    private Dictionary<Point, List<Size>> GenerateDictionnary(Point localBoundaries,
        List<(Point pos, Size vel)> robotsLocations)
    {
        Dictionary<Point, List<Size>> easterBunnyHeadquarter = new();

        List<string> rows = [];

        var points = robotsLocations.Select(e => e.pos).ToList();

        var offsetX = new Size(1, 1);
        var offsetY = new Size(0, 1);

        var displayTree = points
            .Select(point => Enumerable.Range(1, 5)
                .Select(y => Enumerable.Range(1, 5)
                    .Select(x => point + y * offsetY + x * offsetX)))
            .Any(nextPoints =>
                nextPoints
                    .All(p => p
                        .All(pt => points.Contains(pt))));

        for (var y = 0; y < localBoundaries.Y; y++)
        {
            var row = "";

            for (var x = 0; x < localBoundaries.X; x++)
            {
                var point = new Point(x, y);
                var robotsVel = robotsLocations
                    .Where(loc => loc.pos == point)
                    .ToList();

                if (robotsVel.Any())
                    row += robotsVel.Count;
                else
                    row += " ";

                easterBunnyHeadquarter.Add(point, robotsVel.Select(loc => loc.vel).ToList());
            }

            rows.Add(row);
        }

        if (displayTree)
        {
            foreach (var row in rows) Console.WriteLine($"{row}");

            throw new Exception("three found");
        }

        return easterBunnyHeadquarter;
    }
}