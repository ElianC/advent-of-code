namespace AdventOfCode._2024.Day05;

public class Solution : BaseSolution
{
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
            var arrayToSort = updatePages.ToList();

            foreach (var orderingRule in orderingRules.Reverse())
            {
                var isInCorrectOrder = CheckIfArrayIsInSpecificOrder(updatePages, orderingRule[0], orderingRule[1]);
                arrayCorrectOrder.Add(isInCorrectOrder);
            }

            if (arrayCorrectOrder.All(b => b))
            {
                solution1 += updatePages[(updatePages.Length - 1) / 2];
            }
            else
            {
                arrayToSort
                    .Sort((left, right) => orderingRules
                        .Any(rule => rule[0] == left && rule[1] == right)
                        ? 1
                        : -1);

                solution2 += arrayToSort[(arrayToSort.Count - 1) / 2];
            }
        }

        Console.WriteLine($"Solution 01: {solution1}");
        Console.WriteLine($"Solution 02: {solution2}");
    }
}