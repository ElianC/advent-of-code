namespace AdventOfCode._2024.Day09;

public class Solution : BaseSolution
{
    public override void Solve()
    {
        var blockIdInc = 0;

        var input = GetInput()
            .Select(e => int.Parse(e.ToString()))
            .SelectMany((e, id) =>
            {
                int? blockId = null;
                if (id % 2 == 0) blockId = blockIdInc++;
                return Enumerable
                    .Repeat(blockId, e)
                    .Select(block => new { Value = block, BlockIndex = id });
            }).ToList();

        var freeBlocksCount = input
            .Count(block => block.Value is null);

        var filledBlockReversed = input
            .Where(block => block.Value is not null)
            .Reverse()
            .ToArray();

        List<long> listDefragmented = [];

        for (var i = 0; i < input.Count - freeBlocksCount; i++)
        {
            var block = input[i];

            if (block.Value is not null)
            {
                listDefragmented.Add(block.Value ?? 0);
                continue;
            }

            var lastFilledBlock = filledBlockReversed[0];

            listDefragmented.Add(lastFilledBlock.Value ?? 0);
            filledBlockReversed = filledBlockReversed.Skip(1).ToArray();
        }

        var solution1 = listDefragmented.Select((e, f) => e * f).Sum();
        Console.WriteLine($"Day 9: Day 9  {solution1}");
    }
}