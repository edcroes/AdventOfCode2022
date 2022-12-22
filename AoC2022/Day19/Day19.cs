namespace AoC2022.Day19;

public partial class Day19 : IMDay
{
    public string FilePath { private get; init; } = "Day19\\input.txt";

    private static readonly Regex _lineRegex = GetRegex();
    [GeneratedRegex("^Blueprint (?<id>\\d+): Each ore robot costs (?<oreRobotOreCost>\\d+) ore\\. Each clay robot costs (?<clayRobotOreCost>\\d+) ore\\. Each obsidian robot costs (?<obsidianRobotOreCost>\\d+) ore and (?<obsidianRobotClayCost>\\d+) clay\\. Each geode robot costs (?<geodeRobotOreCost>\\d+) ore and (?<geodeRobotObsidianCost>\\d+) obsidian.$")]
    private static partial Regex GetRegex();

    public async Task<string> GetAnswerPart1()
    {
        const int minutes = 24;
        var input = await GetInput();

        var result = input
            .Select(b => GetQualityLevel(b, minutes))
            .Sum();

        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        const int minutes = 32;
        var input = (await GetInput()).Where(b => b.Id < 4).ToList();

        var result = input
            .Select(b => GetMaxNumberOfGeodesCracked(b, new(OreBots: 1), new(), minutes))
            .Aggregate(1, (left, right) => left * right);

        return result.ToString();
    }

    private int GetQualityLevel(Blueprint blueprint, int totalMinutes)
    {
        var max = GetMaxNumberOfGeodesCracked(blueprint, new(OreBots: 1), new(), totalMinutes);
        return blueprint.Id * max;
    }

    /// <summary>
    /// Trying to stop early if a path is not relevant:
    /// - If we can create a geode bot, do it and don't try anything else
    /// - We skipped creating all types of bots in previous steps so there was no reason for skipping, stop
    /// - We skipped a type of bot in the previous step, we're note creating it now
    /// - We don't try to create a bot in the last 2 minutes, unless it's a geode bot in the minute before the last minute
    /// </summary>
    private int GetMaxNumberOfGeodesCracked(Blueprint blueprint, State state, SkippedState skipped, int totalMinutes)
    {
        var maxNumberOfGeodesCracked = state.GeodesCracked;

        if (state.Minute == totalMinutes)
            return maxNumberOfGeodesCracked;

        if (state.OreInStock >= blueprint.GeodeRobotOreCost && state.ObsidianInStock >= blueprint.GeodeRobotObsidianCost)
        {
            var maxCracked = GetMaxNumberOfGeodesCrackedForNextState(blueprint, state, new(), totalMinutes, geodeBotsCreated: 1);
            maxNumberOfGeodesCracked = Math.Max(maxNumberOfGeodesCracked, maxCracked);
            
            return maxNumberOfGeodesCracked;
        }
        else if (state.Minute >= totalMinutes - 2)
        {
            return maxNumberOfGeodesCracked + ((totalMinutes - state.Minute) * state.GeodeBots);
        }

        if (state.OreInStock >= blueprint.ObsidianRobotOreCost && state.ClayInStock >= blueprint.ObsidianRobotClayCost &&
            state.ObsidianBots < blueprint.GeodeRobotObsidianCost)
        {
            if (!skipped.OreBot)
            {
                var maxCracked = GetMaxNumberOfGeodesCrackedForNextState(blueprint, state, new(), totalMinutes, obsidianBotsCreated: 1);
                maxNumberOfGeodesCracked = Math.Max(maxNumberOfGeodesCracked, maxCracked);
            }

            skipped.OreBot = true;
        }

        if (state.OreInStock >= blueprint.ClayRobotOreCost && state.ClayBots < blueprint.ObsidianRobotClayCost)
        {
            if (!skipped.ClayBot)
            {
                var maxCracked = GetMaxNumberOfGeodesCrackedForNextState(blueprint, state, new(), totalMinutes, clayBotsCreated: 1);
                maxNumberOfGeodesCracked = Math.Max(maxNumberOfGeodesCracked, maxCracked);
            }

            skipped.ClayBot = true;
        }

        if (state.OreInStock >= blueprint.OreRobotOreCost && state.OreBots < Math.Max(blueprint.ClayRobotOreCost, Math.Max(blueprint.ObsidianRobotOreCost, blueprint.GeodeRobotOreCost)))
        {
            if (!skipped.ObsidianBot)
            {
                var maxCracked = GetMaxNumberOfGeodesCrackedForNextState(blueprint, state, new(), totalMinutes, oreBotsCreated: 1);
                maxNumberOfGeodesCracked = Math.Max(maxNumberOfGeodesCracked, maxCracked);
            }

            skipped.ObsidianBot = true;
        }

        if (!skipped.OreBot || !skipped.ClayBot || !skipped.ObsidianBot)
        {
            var maxCracked = GetMaxNumberOfGeodesCrackedForNextState(blueprint, state, skipped, totalMinutes);
            maxNumberOfGeodesCracked = Math.Max(maxNumberOfGeodesCracked, maxCracked);
        }

        return maxNumberOfGeodesCracked;
    }

    private int GetMaxNumberOfGeodesCrackedForNextState(Blueprint blueprint, State previous, SkippedState skipped, int totalMinutes, int oreBotsCreated = 0, int clayBotsCreated = 0, int obsidianBotsCreated = 0, int geodeBotsCreated = 0)
    {
        var newState = previous with
        {
            OreBots = previous.OreBots + oreBotsCreated,
            ClayBots = previous.ClayBots + clayBotsCreated,
            ObsidianBots = previous.ObsidianBots + obsidianBotsCreated,
            GeodeBots = previous.GeodeBots + geodeBotsCreated,

            OreInStock = previous.OreInStock + previous.OreBots - (
                    blueprint.OreRobotOreCost * oreBotsCreated +
                    blueprint.ClayRobotOreCost * clayBotsCreated +
                    blueprint.ObsidianRobotOreCost * obsidianBotsCreated +
                    blueprint.GeodeRobotOreCost * geodeBotsCreated),
            ClayInStock = previous.ClayInStock + previous.ClayBots - blueprint.ObsidianRobotClayCost * obsidianBotsCreated,
            ObsidianInStock = previous.ObsidianInStock + previous.ObsidianBots - blueprint.GeodeRobotObsidianCost * geodeBotsCreated,
            GeodesCracked = previous.GeodesCracked + previous.GeodeBots,

            Minute = previous.Minute + 1
        };

        return GetMaxNumberOfGeodesCracked(blueprint, newState, skipped, totalMinutes);
    }

    private async Task<Blueprint[]> GetInput() =>
        (await FileParser.ReadLinesAsString(FilePath))
            .Select(ParseBlueprint)
            .ToArray();

    private static Blueprint ParseBlueprint(string line)
    {
        var match = _lineRegex.Match(line);
        return new(
            match.GetInt("id"),
            match.GetInt("oreRobotOreCost"),
            match.GetInt("clayRobotOreCost"),
            match.GetInt("obsidianRobotOreCost"),
            match.GetInt("obsidianRobotClayCost"),
            match.GetInt("geodeRobotOreCost"),
            match.GetInt("geodeRobotObsidianCost"));
    }

    private record struct State(
        int OreInStock = 0,
        int ClayInStock = 0,
        int ObsidianInStock = 0,
        int GeodesCracked = 0,
        int OreBots = 0,
        int ClayBots = 0,
        int ObsidianBots = 0,
        int GeodeBots = 0,
        int Minute = 0);

    private record struct SkippedState(bool OreBot, bool ClayBot, bool ObsidianBot);

    private record struct Blueprint(
        int Id,
        int OreRobotOreCost,
        int ClayRobotOreCost,
        int ObsidianRobotOreCost,
        int ObsidianRobotClayCost,
        int GeodeRobotOreCost,
        int GeodeRobotObsidianCost);
}
