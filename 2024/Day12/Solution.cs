namespace AdventOfCode._2024.Day12;

public class Solution : BaseSolution
{
    public override void Solve()
    {
        var id = 0;
        var input = GetInput()
                .Split(Environment.NewLine)
                .Select((row, y) => row.Select((cell, x) => new { cell, pos = (x, y) }).ToList())
            .ToList();
                //.GroupBy(e => e, (c, allCells) => c)
            ;
            //.ToDictionary(e => e.cell, e => new { pos = e.pos, perimeter = 0, area = 0 });
        
        Dictionary<int, (char cell, List<(int x, int y)>)> dict = new();
        
        //input.GroupBy((e) => )
        // groups = new List()

        List<(int x, int y)> checkNeighboor = [(-1, 0), (0, -1)];
        
        foreach (var cell in input.SelectMany((row) => row))
        {
            var hasNeighboor = false;
            var cellsNightBoor = checkNeighboor
                .Select(coord => (x: cell.pos.x +coord.x, y: cell.pos.y+coord.y))
                .Where(e => e is { x: >= 0, y: >= 0 })
                .Where(e => e.x < input[0].Count && e.y < input.Count)
                .Where(e => input[e.y][e.x].cell == cell.cell)
                .FirstOrDefault();
                //.FirstOrDefault(e => input[e.y][e.x])
                //.ToList()
                ;

                if (!cellsNightBoor.Equals(default))
                {
                    var group = dict.FirstOrDefault(e => e.Value.Item2.Contains(cellsNightBoor));
                    group.Value.Item2.Add(cell.pos);
                }
                else
                {
                    List<(int x, int y)> value = [( x : cell.pos.x,  y : cell.pos.y)];
                    dict.TryAdd(++id, (cell.cell, value));
                }
                
                
                
            /*
            foreach (var cellNei in cellsNightBoor)
            {
                var celll = input[cellNei.y][cellNei.x].cell;
                Console.WriteLine($"{cell.cell} {celll}");
            }*/
        }

        foreach (var t in dict)
        {
            Console.WriteLine($"{t.Value.cell}, {t.Value.Item2.Count}");
        }

    }
    
}