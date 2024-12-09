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
                //return new { blockId, blocks = Enumerable.Repeat(blockId, e), blockIndex = id };
                return Enumerable.Repeat(blockId, e).Select(block => new { Value = block, BlockIndex = id });
            }).ToList();

        var freeBlocksCount = input
            .Where((block, i) => block.Value is null)
            .Count();

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

            var nextFreeBlock = filledBlockReversed[0];

            listDefragmented.Add(nextFreeBlock.Value ?? 0);
            filledBlockReversed = filledBlockReversed.Skip(1).ToArray();
        }

        var solution1 = listDefragmented.Select((e, f) => e * f).Sum();
        Console.WriteLine($"Day 9: Day 9  {solution1}");
    }
}