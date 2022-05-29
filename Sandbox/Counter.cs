namespace Sandbox;

public class Counter
{
    private  double? _percentage;
    public Counter(string name, int count)
    {
        Name = name;
        Count = count;
    }

    public string Name { get; private set; }
    public int Count { get; private set; }
    public double GetPercent(int total) => _percentage ?? (_percentage = Math.Round(Count * 100.0 / total, 2)).Value;
    public void Increment() => Count++;
    public void AddExcess(double excess) => _percentage += excess;
}

public class CounterManager
{
    public CounterManager(params Counter[] counters)
    {
        Counters = new List<Counter>(counters);
    }
    public List<Counter> Counters { get; set; }
    public int Total() => Counters.Sum(x => x.Count);
    public double TotalPercentage() => Counters.Sum(x => x.GetPercent(Total()));
    public void AnnounceWinner()
    {
        var excess = Math.Round(100 - TotalPercentage(), 2);
        Console.WriteLine($"Excess: {excess}");

        var biggestAmountOfVotes = Counters.Max(x => x.Count);
        var winners = Counters.Where(x => x.Count == biggestAmountOfVotes).ToList();
        if (winners.Count == 1)
        {
            var winner=winners.First();
            winner.AddExcess(excess);
            Console.WriteLine($"{winner.Name} win");
        }
        else
        {
            if (winners.Count != Counters.Count)
            {
                var lowestAmountOfVotes = Counters.Min(x => x.Count);
                var loser = Counters.First(x => x.Count == lowestAmountOfVotes);
                loser.AddExcess(excess);
            }
            Console.WriteLine(string.Join(" - DRAW - ", winners.Select(x => x.Name)));
        }
        foreach (var counter in Counters)
        {
            Console.WriteLine($"{counter.Name} Counts: {counter.Count}, Percentage: {counter.GetPercent(Total())}%");
        }

        Console.WriteLine($"Total Percentage:  {Math.Round(TotalPercentage(), 2)}%");
    }
}