namespace AdventOfCode._2024.Day07;

public class Solution : BaseSolution
{
    private static bool CheckOperatorsRecursively(List<long> numbers, long target)
    {
        if (numbers.Count == 1) return numbers.First() == target;
        var nextTry = numbers.Skip(2).ToList();

        var nextTryMul = nextTry.Prepend(numbers[0] * numbers[1]).ToList();
        var nextTryAdd = nextTry.Prepend(numbers[0] + numbers[1]).ToList();

        return CheckOperatorsRecursively(nextTryMul, target) || CheckOperatorsRecursively(nextTryAdd, target);
    }

    private static bool CheckOperatorsRecursively2(List<long> numbers, long target)
    {
        if (numbers.Count == 1) return numbers.First() == target;
        var nextTry = numbers.Skip(2).ToList();

        var concatNumber = long.Parse($"{numbers[0]}{numbers[1]}");
        var nextTryMul = nextTry.Prepend(numbers[0] * numbers[1]).ToList();
        var nextTryAdd = nextTry.Prepend(numbers[0] + numbers[1]).ToList();
        var nextTryConcat = nextTry.Prepend(concatNumber).ToList();

        return CheckOperatorsRecursively2(nextTryMul, target) || CheckOperatorsRecursively2(nextTryAdd, target) ||
               CheckOperatorsRecursively2(nextTryConcat, target);
    }


    public override void Solve()
    {
        var input = GetInput()
            .Split(Environment.NewLine)
            .Select(e => e.Split(':', ' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(long.Parse).ToList())
            .Select(e => (res: e.First(), ops: e.Skip(1).ToList()))
            .ToList();

        var solution1 = input
            .Where(e => CheckOperatorsRecursively(e.ops, e.res))
            .Sum(e => e.res);

        var Solution2 = input
            .Where(e => CheckOperatorsRecursively2(e.ops, e.res))
            .Sum(e => e.res);

        Console.WriteLine($"{solution1}");
        Console.WriteLine($"{Solution2}");
    }
}