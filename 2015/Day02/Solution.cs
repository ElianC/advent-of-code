namespace AdventOfCode._2015.Day02;

public class Solution : BaseSolution
{
    public override void Solve()
    {
        var results = GetInput()
            .Split(Environment.NewLine)
            .Select(row => row.Split("x")
                .Select(int.Parse)
                .ToList())
            .Select(row => new { l = row[0], w = row[1], h = row[2] })
            .Aggregate((wrappingPapperArea: 0, ribbonLength: 0),
                (result, dim) =>
                {
                    List<int> areas =
                    [
                        dim.l * dim.w,
                        dim.h * dim.w,
                        dim.h * dim.l
                    ];

                    List<int> perimeters =
                    [
                        2 * (dim.l + dim.w),
                        2 * (dim.h + dim.w),
                        2 * (dim.h + dim.l)
                    ];

                    result.wrappingPapperArea += areas.Min() + areas.Sum(e => 2 * e);
                    result.ribbonLength += perimeters.Min() + dim.l * dim.w * dim.h;
                    return result;
                });

        Console.WriteLine(
            "The elves should order {0} square feet of wrapping paper and {1} feet of ribbon",
            results.wrappingPapperArea, results.ribbonLength);
    }
}