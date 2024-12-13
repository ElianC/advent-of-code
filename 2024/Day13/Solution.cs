using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day13;

// first button 10, 10
// second button 2, 3
// winning point = 20, 30

public class Solution : BaseSolution
{
    private (long? a, long? b) SolveLinearEquations((long X, long Y) winningPoint, Size offsetA, Size offsetB)
    {
        long determinant = offsetA.Width * offsetB.Height - offsetA.Height * offsetB.Width;

        if (determinant == 0)
            return (null, null);

        var deltaA = winningPoint.X * offsetB.Height - winningPoint.Y * offsetB.Width;
        var deltaB = offsetA.Width * winningPoint.Y - offsetA.Height * winningPoint.X;

        if (deltaA % determinant != 0 || deltaB % determinant != 0)
            return (null, null);

        var a = deltaA / determinant;
        var b = deltaB / determinant;

        if (a < 0 || b < 0) return (null, null);

        return (a, b);
    }

    private long? FindSmallestCredits((long X, long Y) winningPoint, Size offsetA, Size offsetB)
    {
        var (a, b) = SolveLinearEquations(winningPoint, offsetA, offsetB);

        if (a.HasValue && b.HasValue)
            return a.Value * 3 + b.Value;

        return null;
    }

    public override void Solve()
    {
        var slotMachines = GetInput()
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(e => e.Split(Environment.NewLine)).ToArray();

        long totalCreditsSpent = 0;
        long totalCreditsSpentAdjusted = 0;

        foreach (var machineInfos in slotMachines)
        {
            var regex = new Regex(@"\d+");
            var buttonA = regex.Matches(machineInfos[0])
                .Select(e => e.ToString())
                .Select(int.Parse)
                .ToArray();
            var buttonB = regex.Matches(machineInfos[1])
                .Select(e => e.ToString())
                .Select(int.Parse)
                .ToArray();
            var prize = regex.Matches(machineInfos[2])
                .Select(e => e.ToString())
                .Select(int.Parse)
                .ToArray();

            var offsetA = new Size(buttonA[0], buttonA[1]);
            var offsetB = new Size(buttonB[0], buttonB[1]);
            var prizePoint = (prize[0], prize[1]);

            var offsetWinningPoint = 10000000000000;
            var prizePointAdjusted = (prize[0] + offsetWinningPoint, prize[1] + offsetWinningPoint);

            var creditsSpent = FindSmallestCredits(prizePoint, offsetA, offsetB);
            var creditsSpentAdjusted = FindSmallestCredits(prizePointAdjusted, offsetA, offsetB);

            if (creditsSpent is not null) totalCreditsSpent += (long)creditsSpent;

            if (creditsSpentAdjusted is not null) totalCreditsSpentAdjusted += (long)creditsSpentAdjusted;
        }

        Console.WriteLine(totalCreditsSpent);
        Console.WriteLine(totalCreditsSpentAdjusted);
    }
}