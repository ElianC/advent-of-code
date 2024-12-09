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
                    var firstFilledBlockIndex = filledBlockReversed
                        .FindIndex(e => e.Value.Count <= freeBlockCount);
                    if (firstFilledBlockIndex.Equals(-1)) break; // Exit the loop if no index matching

                    var firstFilledBlock = filledBlockReversed[firstFilledBlockIndex];
                    var count = firstFilledBlock.Value.Count;
                    foreach (var block in firstFilledBlock.Value) listDefragmented2.Add(block);

                    filledBlockReversed.RemoveAt(firstFilledBlockIndex);

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

        Console.WriteLine($"solution1 {solution1} ");
        //Console.WriteLine(
        //  $"{solution2} {string.Join("", listDefragmented2.Select(e => e?.ToString() ?? "."))}");
        Console.WriteLine($"solution2 {solution2} ");

        //00...111...2...333.44.5555.6666.777.888899
        //0099.111...2...333.44.5555.6666.777.8888..
        //0099.1117772...333.44.5555.6666.....8888..
        //0099.111777244.333....5555.6666.....8888..
        //00992111777.44.333....5555.6666.....8888..
        //00992111777.44.333....5555.6666.....8888..

        //10898456634743 too high
    }
}