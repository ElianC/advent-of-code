namespace AdventOfCode._2024.Day05;

public class Solution : BaseSolution
{
    public Solution(string day, string year) : base(day, year)
    {
    }

    private static int[] SortArrayInSpecificOrder(int[] input, int mustBeBefore, int mustBeAfter)
    {
        var output = (int[]) input.Clone();
        
        if (!input.Contains(mustBeBefore) || !input.Contains(mustBeAfter)) return input;
        
        var indexMustBeBefore = Array.IndexOf(input, mustBeBefore);
        var indexMustBeAfter = Array.IndexOf(input, mustBeAfter);
        
        
        if (indexMustBeBefore > indexMustBeAfter)
        {
            output[indexMustBeAfter] = mustBeBefore;
            output[indexMustBeBefore] = mustBeAfter;
        }

        //Console.WriteLine($"{indexMustBeBefore} {indexMustBeAfter} inccorect ordre {string.Join(",", input)}, correct: {string.Join(",", output)}");

        return output;
    }
    
    private static bool CheckIfArrayIsInSpecificOrder(int[] input, int mustBeBefore, int mustBeAfter)
    {
        if (!input.Contains(mustBeBefore) || !input.Contains(mustBeAfter)) return true;
        
        var indexMustBeBefore = Array.IndexOf(input, mustBeBefore);
        var indexMustBeAfter = Array.IndexOf(input, mustBeAfter);

        return indexMustBeBefore < indexMustBeAfter;
    }
    
    public override void Solve()
    {
        var solution1 = 0;
        var solution2 = 0;
        
        var inputs = GetInput().Split(Environment.NewLine + Environment.NewLine);
        var pageOrderingRules = inputs[0].Split(Environment.NewLine);
        var pageNumberUpdates = inputs[1].Split(Environment.NewLine);

        var orderingRules = pageOrderingRules
            .Select(e => e.Split("|").Select(int.Parse).ToArray())
            .ToArray();
        
        var updatesPages = pageNumberUpdates
            .Select(e => e.Split(",").Select(int.Parse).ToArray())
            .ToArray();

        foreach (var updatePages in updatesPages)
        {
            List<bool> arrayCorrectOrder = new();
            var arrayToSort = (int[]) updatePages.Clone();
            
            foreach (var orderingRule in orderingRules)
            {
                var isInCorrectOrder = CheckIfArrayIsInSpecificOrder(updatePages, orderingRule[0], orderingRule[1]);
                arrayCorrectOrder.Add(isInCorrectOrder);
                
                if (!isInCorrectOrder)
                {
                    var mustBeBefore = orderingRule[0];
                    var mustBeAfter = orderingRule[1];
                    
                    if (!arrayToSort.Contains(mustBeBefore) || !arrayToSort.Contains(mustBeAfter)) continue;
                    
                    var indexMustBeBefore = Array.IndexOf(arrayToSort, mustBeBefore);
                    var indexMustBeAfter = Array.IndexOf(arrayToSort, mustBeAfter);
        
                    if (indexMustBeBefore > indexMustBeAfter)
                    {
                        arrayToSort[indexMustBeAfter] = mustBeBefore;
                        arrayToSort[indexMustBeBefore] = mustBeAfter;
                    }
                }
            }

            if (arrayCorrectOrder.All(b => b))
            {
                solution1 += updatePages[(updatePages.Length-1) / 2];
            }
            else
            {
                Console.WriteLine($"inccorect ordre {string.Join(",", updatePages)}, correct: {string.Join(",", arrayToSort)}");
                solution2 += arrayToSort[(arrayToSort.Length - 1) / 2];
            }
        }
        
        Console.WriteLine($"Solution 01: {solution1}");
        Console.WriteLine($"Solution 02: {solution2}");
    }
}