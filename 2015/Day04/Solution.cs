using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2015.Day04;

public class Solution : BaseSolution
{
    public override void Solve()
    {
        var input = GetInput();

        string output;
        var i = 0;

        do
        {
            i++;

            var message = input + i;
            var inputBytes = Encoding.UTF8.GetBytes(message);
            var hashBytes = MD5.Create().ComputeHash(inputBytes);
            output = Convert.ToHexString(hashBytes);
        } while (!output.StartsWith(new string('0', 5)));

        Console.WriteLine("The least number to produce an hash that starts with 5 Zeros is {0}, and the hash is {1}",
            i,
            output);
    }
}