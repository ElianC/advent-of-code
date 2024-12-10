namespace AdventOfCode._2015.Day01;

public class Solution : BaseSolution
{
    private const int Basement = -1;
    private const int GoUpOneFloor = '(';
    private const int GoDownOneFloor = ')';

    public override void Solve()
    {
        var input = GetInput();

        var openingParenthesisCount = input.Count(parenthesis => parenthesis == GoUpOneFloor);
        var closingParenthesisCount = input.Count(parenthesis => parenthesis == GoDownOneFloor);
        var floorReached = openingParenthesisCount - closingParenthesisCount;

        Console.WriteLine($"The instructions led Santa to floor {floorReached}");

        var currenFloor = 0;
        var characterPosition = 0;

        for (; characterPosition < input.Length; characterPosition++)
        {
            if (currenFloor == Basement) break;

            if (input[characterPosition] == GoUpOneFloor) currenFloor++;
            if (input[characterPosition] == GoDownOneFloor) currenFloor--;
        }

        Console.WriteLine(
            $"The position of the first character that causes Santa to enter in the basement is {characterPosition}");
    }
}