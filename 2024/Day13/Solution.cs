using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day13;

public class Solution : BaseSolution
{
    private HashSet<int> AutoSpin(Point winningPoint, Size offsetA, Size offsetB)
    {
        HashSet<int> creditsSpent = [];
        List<int> maxSpinsPossible = [
            winningPoint.X / offsetA.Width, 
            winningPoint.Y / offsetA.Height,
            winningPoint.X / offsetB.Width, 
            winningPoint.Y / offsetB.Height
        ];

        var maxSpinPossible = maxSpinsPossible.Max();
            
        for (var i = 1; i < maxSpinPossible; i++)
        {
            List<(Size offset, int price)> spins = [];

            Enumerable.Range(0, i)
                .ToList()
                .ForEach(_ => spins.Add((offsetA, 3)));   
            Enumerable.Range(i, maxSpinPossible - i)
                .ToList()
                .ForEach(_ => spins.Add((offsetB, 1)));
                
            var creditsSpentForTry = 0;
            var currentPoint = new Point(0, 0);
                
            foreach (var spin in spins)
            {
                creditsSpentForTry += spin.price;
                currentPoint += spin.offset;
                    
                var isWiningSpin = winningPoint == currentPoint;

                if (isWiningSpin)
                {
                    creditsSpent.Add(creditsSpentForTry);
                    break;
                }
            }
        }
        
        return creditsSpent;
    }
    
    public override void Solve()
    {
        var slotMachines = GetInput()
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(e => e.Split(Environment.NewLine)).ToArray();
        
        long totalCreditsSpent = 0;
        
        foreach (var machineInfos in slotMachines)
        {
            //List<int> creditsSpent = new List<int>();
            
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
            var prizePoint = new Point(prize[0], prize[1]);
            var creditsSpent = AutoSpin(prizePoint, offsetA, offsetB);
            
           if (creditsSpent.Any())
            {
                totalCreditsSpent += creditsSpent.Min();
            }
        }
        
        Console.WriteLine(totalCreditsSpent);
    }
}