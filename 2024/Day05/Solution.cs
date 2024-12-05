namespace AdventOfCode._2024.Day05;

public class Solution : BaseSolution
{
    public Solution(string day, string year) : base(day, year)
    {
    }

    public bool IsOrdered(List<int> updatePages, List<List<int>> orderingRules)
    {
        var isOrdered = orderingRules
            .Where(rule => updatePages.Contains(rule[0]) && updatePages.Contains(rule[1]))
            .All(rule => updatePages.IndexOf(rule[0]) < updatePages.IndexOf(rule[1]));
                
        return isOrdered;

    }
    
    public override void Solve()
    {
        var inputs = GetInput().Split(Environment.NewLine + Environment.NewLine);
        
        var pageOrderingRules = inputs[0].Split(Environment.NewLine);
        var pageNumberUpdates = inputs[1].Split(Environment.NewLine);

        var orderingRules = pageOrderingRules
            .Select(e => e.Split("|").Select(int.Parse).ToList())
            .ToList();
        
        var updatesPages = pageNumberUpdates
            .Select(e => e.Split(",").Select(int.Parse).ToList())
            .ToList();

        var solution1 = updatesPages
            .Where(updatePages => IsOrdered(updatePages, orderingRules))
            .Select(updatePages => updatePages[(updatePages.Count - 1) / 2])
            .Sum();
        
        var solution2 = updatesPages
            .Where(updatePages => IsOrdered(updatePages, orderingRules) == false)
            .Select(updatePages =>
            {
                updatePages.Sort((left, right) => orderingRules
                    .Any(rule => rule[0] == left && rule[1] == right) ? 1 : -1);
                return updatePages[(updatePages.Count - 1) / 2];
            })
            .Sum();
        
        Console.WriteLine($"Solution 1: {solution1}");
        Console.WriteLine($"Solution 2: {solution2}");
    }
}