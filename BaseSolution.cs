namespace AdventOfCode;

public abstract class BaseSolution
{
    private readonly string _day;
    private readonly string _year;

    protected BaseSolution(string day, string year)
    {
        _day = day;
        _year = year;
    }

    protected string GetInput()
    {
        var path = Path.Combine(_year, $"Day{_day}", "Input.txt");

        using StreamReader reader = new(path);
        var text = reader.ReadToEnd();

        return text;
    }

    public abstract void Solve();
}