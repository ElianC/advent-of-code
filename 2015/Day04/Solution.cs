using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2015.Day04;

public class Solution : BaseSolution
{
    public (int result, string output) FindHashStartsWith(string startsWith)
    {
        var input = GetInput();

        string output;
        var i = 0;

        do
        {
            i++;

            var message = input + i;
            var inputBytes = Encoding.UTF8.GetBytes(message);
            var hashBytes = MD5.HashData(inputBytes);
            output = Convert.ToHexString(hashBytes);
        } while (!output.StartsWith(startsWith));

        return (i, output);
    }

    public override void Solve()
    {
        var solution1 = FindHashStartsWith(new string('0', 5));
        var solution2 = FindHashStartsWith(new string('0', 6));

        Console.WriteLine("The least number to produce an hash that starts with 5 Zeros is {0}, and the hash is {1}",
            solution1.result,
            solution1.output);
        Console.WriteLine("The least number to produce an hash that starts with 6 Zeros is {0}, and the hash is {1}",
            solution2.result,
            solution2.output);
    }
}