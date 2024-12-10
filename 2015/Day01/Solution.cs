namespace AdventOfCode._2015.Day01;

public class Solution : BaseSolution
{
    private const int BasementLevel = -1;
    private const char GoUpOneLevel = '(';
    private const char GoDownOneLevel = ')';

    public override void Solve()
    {
        var instructions = GetInput();
        int floorsClimbedCount = default;
        int floorsWentDownCount = default;
        int levelReached = default;
        int firstInstructionNumberToBasement = default;

        for (var instructionNumber = 1; instructionNumber <= instructions.Length; instructionNumber++)
        {
            var instruction = instructions[instructionNumber - 1];

            if (instruction == GoUpOneLevel) floorsClimbedCount++;
            else if (instruction == GoDownOneLevel) floorsWentDownCount++;

            levelReached = floorsClimbedCount - floorsWentDownCount;

            if (levelReached is BasementLevel && firstInstructionNumberToBasement == default)
                firstInstructionNumberToBasement = instructionNumber;
        }

        Console.WriteLine("The instructions led Santa to floor {0}", levelReached);
        Console.WriteLine("The first instruction that led Santa to the basement was instruction number {0}",
            firstInstructionNumberToBasement);
    }
}