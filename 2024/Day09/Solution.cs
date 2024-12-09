namespace AdventOfCode._2024.Day09;

public class Solution : BaseSolution
{
    public override void Solve()
    {
        var blockIdInc = 0;

        var input = GetInput()
            .Select(e => int.Parse(e.ToString()))
            .Select((e, id) =>
            {
                int? blockId = null;
                if (id % 2 == 0) blockId = blockIdInc++;
                var block = Enumerable
                    .Repeat(blockId, e)
                    .Select(block => block)
                    .ToList();

                return (Value: block, BlockIndex: id);
            }).ToList();

        var inputBlocksFlattened = input
            .SelectMany(blocks =>
                blocks.Value
                    .Select(block => block)
                    .ToList())
            .ToList();

        var freeBlocksFlattened = inputBlocksFlattened
            .Where(block => block is null)
            .ToList();

        var filledBlockReversed = input
            .Where(blocks => blocks.Value.All(block => block is not null))
            .Reverse()
            .ToList();

        var filledBlockFlattenedReversed = inputBlocksFlattened
            .Where(block => block is not null)
            .Reverse()
            .ToList();

        List<long> listDefragmented = [];


        for (var i = 0; i < inputBlocksFlattened.Count - freeBlocksFlattened.Count; i++)
        {
            var block = inputBlocksFlattened[i];

            if (block is null)
            {
                var lastFilledBlock = filledBlockFlattenedReversed[0];
                listDefragmented.Add(lastFilledBlock ?? 0);
                filledBlockFlattenedReversed.RemoveAt(0);
            }
            else
            {
                listDefragmented.Add(block ?? 0);
            }
        }

        List<long?> listDefragmented2 = [];
        HashSet<int> freedIndexes = [];

        for (var i = 0; i < input.Count; i++)
        {
            var blocks = input[i];

            if (i % 2 == 0)
            {
                filledBlockReversed.Remove(blocks);

                foreach (var block in blocks.Value)
                    if (freedIndexes.Contains(blocks.BlockIndex))
                        listDefragmented2.Add(null);
                    else
                        listDefragmented2.Add(block);
            }
            else
            {
                var freeBlockCount = blocks.Value.Count;

                if (freeBlockCount == 0) continue;

                do
                {
                    var firstFilledBlock = filledBlockReversed
                        .FirstOrDefault(e => e.Value.Count <= freeBlockCount);

                    if (firstFilledBlock.Equals(default)) break;

                    var count = firstFilledBlock.Value.Count;

                    foreach (var block in firstFilledBlock.Value) listDefragmented2.Add(block);

                    filledBlockReversed.Remove(firstFilledBlock);

                    freedIndexes.Add(firstFilledBlock.BlockIndex);
                    freeBlockCount -= count;
                } while (true);

                for (var j = 0; j < freeBlockCount; j++) listDefragmented2.Add(null);
            }
        }

        var solution1 = listDefragmented
            .Select((block, index) => block * index)
            .Sum();
        var solution2 = listDefragmented2
            .Select((block, index) => block is not null ? block * index : 0)
            .Sum();

        Console.WriteLine($"solution1 {solution1}");
        Console.WriteLine($"solution2 {solution2}");
    }
}