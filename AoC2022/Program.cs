using TextCopy;

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
var part1 = await dayInstance.GetAnswerPart1();
Console.WriteLine($"Answer Part 1: {part1}");
var part1TimeTaken = stopwatch.Elapsed;

stopwatch.Restart();
var part2 = await dayInstance.GetAnswerPart2();
Console.WriteLine($"Answer Part 2: {part2}");
var part2TimeTaken = stopwatch.Elapsed;
stopwatch.Stop();

ClipboardService.SetText(part2 == "TODO" ? part1 : part2);

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