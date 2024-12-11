namespace AdventOfCode._2024.Day11;

public class Solution : BaseSolution
{
    public long BlinkNumbers(Dictionary<long, long> numbers, int blinkCount)
    {
        for (var i = 0; i < blinkCount; i++)
        {
            Dictionary<long, long> blinkNumbers = [];

            foreach (var (number, c) in numbers)
            {
                var stringifiedNumber = number.ToString();

                if (number == 0)
                {
                    AddStones(1, c);
                }
                else if (stringifiedNumber.Length % 2 == 0)
                {
                    var len = stringifiedNumber.Length / 2;
                    var firstNumber = stringifiedNumber.Substring(0, len);
                    var secondNumber = stringifiedNumber.Substring(len, len);

                    AddStones(long.Parse(firstNumber), c);
                    AddStones(long.Parse(secondNumber), c);
                }
                else
                {
                    AddStones(number * 2024, c);
                }
            }

            numbers = blinkNumbers;

            void AddStones(long stone, long c)
            {
                blinkNumbers.TryAdd(stone, 0);
                blinkNumbers[stone] += c;
            }
        }


        return numbers.Values.Sum();
    }

    public override void Solve()
    {
        var input = GetInput()
            .Split(" ")
            .ToDictionary(long.Parse, _ => 1L);

        var res = BlinkNumbers(input, 25);
        var res2 = BlinkNumbers(input, 75);
        Console.WriteLine($"{res}");
        Console.WriteLine($"{res2}");
    }
}