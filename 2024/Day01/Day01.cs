namespace AdventOfCode._2024.Day01;

public class Day01 : BaseSolve
{
    public Day01(string day, string year) : base(day, year)
    {
    }

    public override void Solve()
    {
        var input = GetInput();
        var lines = input.Split(Environment.NewLine);

        var leftNumbers = new uint[lines.Length];
        var rightNumbers = new uint[lines.Length];

        for (var i = 0; i < lines.Length; i++)
        {
            var columns = lines[i].Split("   ");

            leftNumbers[i] = uint.Parse(columns[0]);
            rightNumbers[i] = uint.Parse(columns[1]);
        }

        var solution1 = CalculateTotalDistanceBetweenSortedPairs(rightNumbers, leftNumbers);
        Console.WriteLine($"Total distance between the lists: {solution1}");

        var solution2 = CalculateSimilarityScoreBasedOnOccurrences(rightNumbers, leftNumbers);
        Console.WriteLine($"The similarity score between the lists: {solution2}");
    }

    private static uint CalculateTotalDistanceBetweenSortedPairs(uint[] rightNumbers, uint[] leftNumbers)
    {
        uint distance = 0;

        var orderedLeftNumbers = leftNumbers
            .OrderBy(x => x)
            .ToArray();
        var orderedRightNumbers = rightNumbers
            .OrderBy(x => x)
            .ToArray();

        for (var i = 0; i < orderedLeftNumbers.Length; i++)
        {
            var leftNumber = orderedLeftNumbers[i];
            var rightNumber = orderedRightNumbers[i];

            if (leftNumber > rightNumber)
                distance += leftNumber - rightNumber;
            else
                distance += rightNumber - leftNumber;
        }

        return distance;
    }

    private static uint CalculateSimilarityScoreBasedOnOccurrences(uint[] rightNumbers, uint[] leftNumbers)
    {
        uint solution = 0;
        var uniqueLeftNumbers = leftNumbers.Distinct().ToArray();

        foreach (var number in uniqueLeftNumbers)
        {
            var count = rightNumbers.Select(x => x).Count(x => x == number);

            if (count is not 0) solution += (uint)(number * count);
        }

        return solution;
    }
}