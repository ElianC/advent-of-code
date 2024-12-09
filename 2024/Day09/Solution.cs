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
            .Where(blocks => blocks.Value.Any() && blocks.Value.All(block => block is not null))
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

        // var listDefragmented2 = input.ToList();
        List<long?> listDefragmented2 = [];
        HashSet<int> freedIndexes = [];

        for (var i = 0; i < input.Count; i++)
        {
            var blocks = input[i];

            if (i % 2 == 0)
            {
                foreach (var block in blocks.Value)
                    if (freedIndexes.Contains(blocks.BlockIndex))
                        listDefragmented2.Add(null);
                    else
                        listDefragmented2.Add(block);
            }
            else
            {
                var freeBlockCount = blocks.Value.Count;
                int firstFilledBlockIndex;
                do
                {
                    firstFilledBlockIndex = filledBlockReversed
                        .FindIndex(e => e.Value.Count <= freeBlockCount);
                    if (firstFilledBlockIndex.Equals(-1)) break; // Exit the loop if the tuple is default
                    //Console.WriteLine($"{firstFilledBlock.BlockIndex}, {string.Join("", firstFilledBlock.Value)}");
                    var firstFilledBlock = filledBlockReversed[firstFilledBlockIndex];
                    var count = firstFilledBlock.Value.Count;
                    foreach (var block in firstFilledBlock.Value) listDefragmented2.Add(block);

                    filledBlockReversed.RemoveAt(firstFilledBlockIndex);
                    //Console.WriteLine($"count: {filledBlockReversed.Count}");
                    Console.WriteLine($"index {firstFilledBlock.BlockIndex}");
                    freedIndexes.Add(firstFilledBlock.BlockIndex);
                    freeBlockCount -= count;
                } while (true);

                //Console.WriteLine($"freeBlockCount {freeBlockCount}");
                for (var j = 0; j < freeBlockCount; j++) listDefragmented2.Add(null);

                /*do
                {
                    firstFilledBlock = filledBlockReversed
                        .FirstOrDefault(e => e.Value.Count <= freeBlockCount);

                    if (!firstFilledBlock.Equals(default((int?, int))))
                    {
                        var count = firstFilledBlock.Value.Count;
                        foreach (var block in blocks.Value)
                            listDefragmented2.Add(block);

                        freedIndexes.Add(firstFilledBlock.BlockIndex);
                        freeBlockCount -= count;
                    }
                } while (firstFilledBlock.Equals(default((int?, int))));*/
            }
        }

        /*
        foreach (var filledBlock in filledBlockReversed.ToList())
        {
            //Console.WriteLine($"Defragmented filled blocks: {string.Join("", filledBlock.Value)}");

            var firstFreeBlock = freeBlocks.FirstOrDefault(e => e.Value.Count >= filledBlock.Value.Count);

            if (firstFreeBlock.Value is not null)
            {
                freedIndexes.Add(filledBlock.BlockIndex);

                listDefragmented2[firstFreeBlock.BlockIndex] = filledBlock;

                var diff = firstFreeBlock.Value.Count - filledBlock.Value.Count;

                listDefragmented2[filledBlock.BlockIndex] = (
                    Enumerable.Repeat<int?>(null, filledBlock.Value.Count).ToList(), filledBlock.BlockIndex);

                if (diff > 0)
                {
                    Console.WriteLine($"Defragmented filled blocks: {diff}");
                    //freeBlocks
                    freeBlocks.RemoveAt(0);
                    freeBlocks.Prepend((Enumerable.Repeat<int?>(null, diff).ToList(), firstFreeBlock.BlockIndex));
                }
                else
                {
                    freeBlocks.RemoveAt(0);
                }
                //freeBlocks.Prepend((Enumerable.Repeat<int?>(null, diff).ToList(), firstFreeBlock.BlockIndex + 1));
            }
        }
*/
        var solution1 = listDefragmented
            .Select((block, index) => block * index)
            .Sum();
        var solution2 = listDefragmented2
            .Select((block, index) => block is not null ? block * index : 0)
            .Sum();

        Console.WriteLine($"solution1 {solution1} ");
        Console.WriteLine(
            $"{solution2} {string.Join("", listDefragmented2.Select(e => e?.ToString() ?? "."))}");
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