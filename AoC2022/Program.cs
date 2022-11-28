var allDays = GetDays();
var dayType = allDays.Last();

if (args is not null && args.Length > 0 && int.TryParse(args[0], out int day))
{
    dayType = allDays.SingleOrDefault(d => d.Name.EndsWith(day.ToString("D2"))) ?? dayType;
}

IMDay dayInstance = (IMDay)Activator.CreateInstance(dayType)!;

Stopwatch stopwatch = new();

Console.WriteLine(dayType.Name);
Console.WriteLine("-----");

stopwatch.Start();
Console.WriteLine($"Answer Part 1: {await dayInstance.GetAnswerPart1()}");
var part1TimeTaken = stopwatch.Elapsed;

stopwatch.Restart();
Console.WriteLine($"Answer Part 2: {await dayInstance.GetAnswerPart2()}");
var part2TimeTaken = stopwatch.Elapsed;
stopwatch.Stop();

Console.WriteLine();
Console.WriteLine($"Part 1 took: {part1TimeTaken}");
Console.WriteLine($"Part 2 took: {part2TimeTaken}");

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadLine();

static IEnumerable<Type> GetDays() =>
    Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.GetInterfaces().Contains(typeof(IMDay)))
        .OrderBy(t => t.Name);