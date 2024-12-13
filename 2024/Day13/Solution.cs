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
        var minSpinPossible = maxSpinsPossible.Min();
            
            for (long a = 0; a < maxSpinPossible; a++)
            {
                var xa = offsetA.Width * a;
                var ya = offsetA.Height * a;
                
                if (xa > winningPoint.X || ya > winningPoint.Y) 
                    break;

                var shouldBreak = false;
                
                for (long b = 0; b < maxSpinPossible - a; b++)
                {
                    var xb = offsetB.Width * b;
                    var yb = offsetB.Height * b;
                    var creditsSpentForTry = a * 3 + b;
                    
                    if (xb > winningPoint.X || yb > winningPoint.Y) 
                        break;
                    if (xa + xb == winningPoint.X && ya + yb == winningPoint.Y)
                    {
                        if (smallestCreditsAmount is null || creditsSpentForTry < smallestCreditsAmount)
                        {
                            smallestCreditsAmount = creditsSpentForTry;
                            shouldBreak = true;
                            break;
                        }
                    }
                }

                if (shouldBreak) break;
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
            
            Console.WriteLine($"{machineInfos[0]}: {machineInfos[1]}");
            
            var offsetA = new Size(buttonA[0], buttonA[1]);
            var offsetB = new Size(buttonB[0], buttonB[1]);
            var prizePoint = (prize[0], prize[1]);

           // var offsetWinningPoint = 1;
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