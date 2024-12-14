using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day14;

public class Solution : BaseSolution
{
    //private readonly Point boundaries = new(11, 7);
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

        var timeLeft = 100;
        //var boundaries = new Point(101, 103);

        //Dictionary<Point,1 > = new();
        var easterBunnyHeadquarter = GenerateDictionnary(boundaries, robotsLocations);

        //Console.WriteLine(easterBunnies.Count);

        for (; timeLeft > 0; timeLeft--)
        {
            //var newDict = easterBunnyHeadquarter.ToDictionary();
            List<(Point pos, Size vel)> newRobotPos = [];

            var robotsLoc = easterBunnyHeadquarter
                .Where(e => e.Value.Count >= 1)
                .Select(e => (e.Key, e.Value))
                .ToArray();
            //Console.WriteLine($"New robot location: {robotsLoc.Sum(e => e.Value.Count())}");


            foreach (var robotLoc in robotsLoc)
            foreach (var robotVel in robotLoc.Value)
            {
                var newLoc = robotLoc.Key + robotVel;
                if (newLoc.X >= 0 && newLoc.Y >= 0 && newLoc.X < boundaries.X && newLoc.Y < boundaries.Y)
                {
                    newRobotPos.Add((newLoc, robotVel));
                }
                else
                {
                    var posX = newLoc.X;
                    var posY = newLoc.Y;
                    if (posX < 0)
                    {
                        var test = boundaries.X + posX;
                        posX = test;
                        Console.WriteLine($"< PosX: {posX} {newLoc.X} {robotLoc.Key.X} {robotVel.Width}");
                        //Console.WriteLine($"posX: {posX} {boundaries.X} {newLoc.X}");
                    }

                    if (posX >= boundaries.X)
                    {
                        var test = posX - boundaries.X;
                        posX = test;
                        Console.WriteLine($">= PosX: {posX} {newLoc.X}  {robotLoc.Key.X} {robotVel.Width}");

                        //Console.WriteLine($"posX: {posX} {boundaries.X} {newLoc.X}");
                    }

                    if (posY < 0)
                    {
                        var test = boundaries.Y + posY;
                        posY = test;
                        Console.WriteLine($"< PosY: {posY} {newLoc.Y}  {robotLoc.Key.Y} {robotVel.Height}");

                        //Console.WriteLine($"posY: {posY} {boundaries.Y} {newLoc.Y}");
                    }

                    if (posY >= boundaries.Y)
                    {
                        var test = posY - boundaries.Y;
                        posY = test;
                        Console.WriteLine($">= PosY: {posY} {newLoc.Y}  {robotLoc.Key.Y} {robotVel.Height}");

                        //Console.WriteLine($"posY: {posY} {boundaries.Y} {newLoc.Y}");
                    }


                    newRobotPos.Add((new Point(posX, posY), robotVel));
                }
            }

            easterBunnyHeadquarter = GenerateDictionnary(boundaries, newRobotPos);
        }

        var countRobots = 0;

        var semiBoundaryX = (boundaries.X - 1) / 2;
        var semiBoundaryY = (boundaries.Y - 1) / 2;

        Console.WriteLine($"semi boundaries: {semiBoundaryX} {semiBoundaryY}");
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

        foreach (var robot in easterBunnyHeadquarter)
        {
            if (robot.Value.Any())
            {
                Console.WriteLine($"{robot.Key.X} {robot.Key.Y} {robot.Value.Count}");
                if (AreInBoundaries(robot.Key, quad1Min, quad1Max))
                {
                    Console.WriteLine($"fuck you bastard {robot.Value.Count}");
                    quad1Count += robot.Value.Count();
                }

                if (AreInBoundaries(robot.Key, quad2Min, quad2Max)) quad2Count += robot.Value.Count();

                if (AreInBoundaries(robot.Key, quad3Min, quad3Max)) quad3Count += robot.Value.Count();

                if (AreInBoundaries(robot.Key, quad4Min, quad4Max)) quad4Count += robot.Value.Count();
            }

            //Console.WriteLine($"Robots: {countRobots}");
            countRobots += robot.Value.Count;
        }

        //Console.WriteLine($"EasterBunnies: {robot.Value.Count}");
        Console.WriteLine($"Robotsdd: {quad1Count} {quad2Count} {quad3Count} {quad4Count}");
        Console.WriteLine($"robots count: {quad1Count * quad2Count * quad3Count * quad4Count}");
    }

    private bool AreInBoundaries(Point point, Point boundariesMin, Point boundariesMax)
    {
        if (point.X >= boundariesMin.X && point.Y >= boundariesMin.Y &&
            point.X < boundariesMax.X && point.Y < boundariesMax.Y) return true;

        //Console.WriteLine($"Invalid point, {point.X}, {point.Y} {boundariesMin}, {boundariesMax}");
        return false;
    }

    private Dictionary<Point, List<Size>> GenerateDictionnary(Point boundaries,
        List<(Point pos, Size vel)> robotsLocations)
    {
        Console.WriteLine($"Generating dictionnary...{robotsLocations.Count}");
        Dictionary<Point, List<Size>> easterBunnyHeadquarter = new();

        for (var y = 0; y < boundaries.Y; y++)
        for (var x = 0; x < boundaries.X; x++)
        {
            var point = new Point(x, y);
            var robotsVel = robotsLocations
                .Where(loc => loc.pos == point)
                .Select(loc => loc.vel)
                .ToList();

            easterBunnyHeadquarter.Add(point, robotsVel);
        }

        return easterBunnyHeadquarter;
    }
}