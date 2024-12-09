namespace AdventOfCode;

public abstract class BaseSolution
{
    protected string GetInput()
    {
        var solutionInstance = SolutionLoader.GetInstance();
        var path = Path.Combine(solutionInstance.Year, $"Day{solutionInstance.Day}", "Input.txt");

        if (Path.GetFullPath(path).Contains("Debug"))
            path = Path.Combine(
                Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? "",
                path);
        return File.ReadAllText(path);
    }

    public abstract void Solve();
}