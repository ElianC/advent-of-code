namespace AdventOfCode;

public abstract class BaseSolve
{
    private readonly string _day;
    private readonly string _year;

    public BaseSolve(string day, string year)
    {
        _day = day;
        _year = year;
    }

    protected string GetInput()
    {
        var path = Path.Combine(_year, $"Day{_day}", "input.txt");

        using StreamReader reader = new(path);
        var text = reader.ReadToEnd();

        return text;
    }

    public abstract void Solve();
}