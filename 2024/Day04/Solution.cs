using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day04;

public class Solution : BaseSolution
{
    public Solution(string day, string year) : base(day, year)
    {
    }

    public override void Solve()
    {
        var input = GetInput();

        var inputs = input.Split("\n")
            .Select(e => e.Split("")).ToArray();

        var rows = inputs.Select(e => string.Join("", e).ToList()).ToList();

        var cols = Enumerable.Range(0, rows[0].Count)
            .Select(i => Enumerable.Range(0, rows.Count)
                .Select(e => rows[e][i]).Select(e=>string.Join("", e))).ToList();
        

        //Console.WriteLine($"{cols[1]}");
        Console.WriteLine(string.Join("", cols[1]));
      
        var diags = Enumerable.Range(0, rows[0].Count)
            .Select(i =>
            {
                var x = i;
                
                return Enumerable.Range(0, rows.Count)
                    .Select(e =>
                    {
                        var y = e;

                        if (x < rows[0].Count && y < rows.Count && x < rows[x][y] && y < rows[x][y])
                        {
                            var res=  rows[x][y];

                            y++;
                            x++;
                            
                            return res;
                        }

                        return 'Z';
                    });
            }).ToArray();
        Console.WriteLine(string.Join("", diags[2]));
        /*

var colsLenght = rows[0].Split("").Length;

string[,] cols = new string[colsLenght,rows.Count];

for (int row = 0; row < rows.Count; row++)
{
    for (int col = 0; col < colsLenght; col++)
    {

        cols[col, row] = rows.Select(e => e.Split("")).ToArray()[row][col];
        //cols[col] += rows[row][col];
    }
}

var count = 0;

Console.WriteLine($"{cols[0,0]}");
var regex = new Regex("XMAS|SAMX");

var countRows= rows
    .Select(row => regex.Match(row))
    .Select(match => match.Length)
    .Sum();
        Console.WriteLine($"2024 - Day 4 {countRows}");
*/
    }
}