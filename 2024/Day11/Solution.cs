namespace AdventOfCode._2024.Day11;

public class Solution : BaseSolution
{
    private const int blinkCount = 25;
    private int currentBlink = 0;
    
    public List<ulong> BlinkNumbers(List<ulong> numbers)
    {
        currentBlink++;
        var blinkNumbers = new List<ulong>();
        
        foreach (var number in numbers)
        {
            var stringifiedNumber = number.ToString();
            
            if (number == 0)
            {
                blinkNumbers.Add(1);
            } 
            else if (stringifiedNumber.Length % 2 == 0)
            {
                var len = stringifiedNumber.Length / 2;
                var firstNumber = stringifiedNumber.Substring(0, len);
                var secondNumber = stringifiedNumber.Substring(len, len);

                blinkNumbers.Add(ulong.Parse(firstNumber));
                blinkNumbers.Add(ulong.Parse(secondNumber));
            }
            else
            {
                blinkNumbers.Add(number * 2024);
            }
        }
        
        
        if (blinkCount > currentBlink)
        {
            return BlinkNumbers(blinkNumbers);
        } 
        
        currentBlink++;
        return blinkNumbers;
    }
    
    public override void Solve()
    {
        var input = GetInput()
            .Split(" ")
            .Select(ulong.Parse)
            .ToList();

        long res = BlinkNumbers(input).Count();
        Console.WriteLine($"{res}");
    }
}