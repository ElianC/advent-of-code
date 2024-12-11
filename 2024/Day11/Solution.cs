namespace AdventOfCode._2024.Day11;

public class Solution : BaseSolution
{
    public long BlinkNumbers(Dictionary<long, long> numbers, int blinkCount)
    {
        for (var i = 0; i < blinkCount; i++)
        {
            var nextBlinkNumbers = new Dictionary<long, long>();

            foreach (var (number, count) in numbers) ProcessNumber(number, count, nextBlinkNumbers);

            numbers = nextBlinkNumbers;
        }

        return numbers.Values.Sum();
    }

    private void ProcessNumber(long number, long count, Dictionary<long, long> blinkNumbers)
    {
        if (number == 0)
        {
            AddStones(1, count, blinkNumbers);
            return;
        }

        var numberString = number.ToString();

        if (numberString.Length % 2 == 0)
        {
            var halfLength = numberString.Length / 2;
            var firstHalf = long.Parse(numberString[..halfLength]);
            var secondHalf = long.Parse(numberString[halfLength..]);

            AddStones(firstHalf, count, blinkNumbers);
            AddStones(secondHalf, count, blinkNumbers);
        }
        else
        {
            AddStones(number * 2024, count, blinkNumbers);
        }
    }

    private void AddStones(long stone, long count, Dictionary<long, long> blinkNumbers)
    {
        if (!blinkNumbers.TryAdd(stone, count)) blinkNumbers[stone] += count;
    }

    public override void Solve()
    {
        var input = GetInput()
            .Split(" ")
            .Select(long.Parse)
            .ToDictionary(g => g, g => (long)1);

        var result1 = BlinkNumbers(input, 25);
        var result2 = BlinkNumbers(input, 75);

        Console.WriteLine($"Result after 25 blinks: {result1}");
        Console.WriteLine($"Result after 75 blinks: {result2}");
    }
}