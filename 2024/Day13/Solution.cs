using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day13;

public class Solution : BaseSolution
{
    private long? AutoSpin((long X, long Y) winningPoint, Size offsetA, Size offsetB)
    {
        long? smallestCreditsAmount = null;
        
        List<long> maxSpinsPossible = [
            winningPoint.X / offsetA.Width, 
            winningPoint.Y / offsetA.Height,
            winningPoint.X / offsetB.Width, 
            winningPoint.Y / offsetB.Height
        ];

        var maxSpinPossible = maxSpinsPossible.Max();
            
        Console.WriteLine($"Max spins possible: {maxSpinPossible}");
        for (var i = 1; i < maxSpinPossible; i++)
        {
            List<(Size offset, int price)> spins = [];

            for (long a = 0; a < i; a++)
            {
                spins.Add((offsetA, 3));
            }           
            
            for (long a = i; a < maxSpinPossible - i; a++)
            {
                spins.Add((offsetB, 1));
            }
            
            var creditsSpentForTry = 0;
            var currentPoint = new Point(0, 0);
                
            foreach (var spin in spins)
            {
                creditsSpentForTry += spin.price;
                currentPoint += spin.offset;
                    
                var isWiningSpin = winningPoint.X == currentPoint.X && winningPoint.Y == currentPoint.Y;

                if (isWiningSpin && (smallestCreditsAmount is null || creditsSpentForTry < smallestCreditsAmount))
                {
                    smallestCreditsAmount = creditsSpentForTry; 
                    break;
                }
            }
        }
        
        return smallestCreditsAmount;
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
            
            var creditsSpent = AutoSpin(prizePoint, offsetA, offsetB);
            var creditsSpentAdjusted = AutoSpin(prizePointAdjusted, offsetA, offsetB);
            
           if (creditsSpent is not null)
            {
                totalCreditsSpent += (long)creditsSpent;
            }

            if (creditsSpentAdjusted is not null)
            {
                totalCreditsSpentAdjusted += (long)creditsSpentAdjusted;
            }
        }
        
        Console.WriteLine(totalCreditsSpent);
        Console.WriteLine(totalCreditsSpentAdjusted);
    }
}