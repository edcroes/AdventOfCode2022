using System.Collections;
using System.Collections.Concurrent;
using AoC.Common.Graphs;

namespace AoC2022.Day16;

public partial class Day16 : IMDay
{
    public string FilePath { private get; init; } = "Day16\\input.txt";

    private static readonly Regex _lineRegex = GetRegex();
    [GeneratedRegex("^Valve (?<valve>[A-Z]+) has flow rate=(?<rate>\\d+); tunnel[s]? lead[s]? to valve[s]? (?<toValves>[A-Z, ]+)$")]
    private static partial Regex GetRegex();

    public async Task<string> GetAnswerPart1()
    {
        var input = await GetInput();
        var maxTime = 30;
        var start = input.Single(v => v.Name == "AA");

        List<List<Valve>> allPaths = GetAllPathsWithinTime(input, start, maxTime);
        int maxReleasedPressure = allPaths.Select(p => GetReleasedPressure(p, maxTime)).Max();

        return maxReleasedPressure.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var input = await GetInput();
        var maxTime = 26;
        var start = input.Single(v => v.Name == "AA");
        List<List<Valve>> allPaths = GetAllPathsWithinTime(input, start, maxTime);
        Dictionary<Signature, int> signatureSizes = new();

        foreach (var path in allPaths)
        {
            var signature = GetPathSignature(path);
            var releasedPressure = GetReleasedPressure(path, maxTime);

            signatureSizes.AddOrReplaceIfGreaterThan(signature, releasedPressure);
        }

        var maxCombi = 0;
        var sigsToCheck = signatureSizes.Keys.ToList();
        
        while (sigsToCheck.Count > 0)
        {
            var sig = sigsToCheck.First();
            sigsToCheck.Remove(sig);

            foreach (var other in sigsToCheck.Where(s => !s.HasOverlap(sig)))
            {
                var val = signatureSizes[sig] + signatureSizes[other];
                maxCombi = Math.Max(maxCombi, val);
            }
        }

        return maxCombi.ToString();
    }

    private static Signature GetPathSignature(List<Valve> path) =>
        new() { Value = path.Where(p => p.Name != "AA").Select(v => v.Name).OrderBy(n => n).ToArray() };

    private static List<List<Valve>> GetAllPathsWithinTime(Valve[] input, Valve start, int maxTime)
    {
        var graph = GetWorkingValvesGraph(input, start, maxTime);
        ConcurrentBag<List<Valve>> allPaths = new();
        var edges = graph.Edges.Where(e => !e.Equals(start));

        Parallel.ForEach(Partitioner.Create(edges), other =>
        {
            foreach (var path in graph.GetAllPathsWithAllEdges(start, other, maxTime))
                allPaths.Add(path);
        });

        return allPaths.ToList();
    }

    private static WeightedGraph<Valve> GetWorkingValvesGraph(Valve[] input, Valve start, int maxTime)
    {
        var workingValves = input
            .Where(v => v.FlowRate > 0 || v.Equals(start))
            .OrderBy(v => v.Name)
            .ToList();

        // Build the entire graph first where each stop costs 1
        WeightedGraph<Valve> fullGraph = new();
        foreach (var valve in input)
        {
            foreach (var to in valve.LeadsTo)
            {
                fullGraph.AddEdge(valve, input.Single(v => v.Name == to));
            }
        }

        // Build a graph with only the valves that can be opened where cost is the time it takes to get there
        WeightedGraph<Valve> workingValvesGraph = new();
        while (workingValves.Count > 0)
        {
            var next = workingValves.First();
            Valve from = new() { Name = next.Name, FlowRate = next.FlowRate, Weight = 0 };

            foreach (var valve in workingValves.Skip(1))
            {
                var weight = fullGraph.GetShortestPath(next, valve) + 1;
                workingValvesGraph.AddDirectedEdge(from, new() { Name = valve.Name, FlowRate = valve.FlowRate, Weight = weight });
                workingValvesGraph.AddDirectedEdge(new() { Name = valve.Name, FlowRate = valve.FlowRate, Weight = 0 }, new() { Name = from.Name, FlowRate = from.FlowRate, Weight = weight });
            }

            workingValves.Remove(next);
        }

        return workingValvesGraph;
    }

    private static int GetReleasedPressure(List<Valve> path, int maxSteps)
    {
        var releasedPressure = 0;
        var totalWeight = 0;
        foreach (var valve in path.Skip(1))
        {
            totalWeight += valve.Weight;          
            if (totalWeight > maxSteps)
                break;

            releasedPressure += (maxSteps - totalWeight) * valve.FlowRate;
        }

        return releasedPressure;
    }

    private async Task<Valve[]> GetInput() =>
        (await FileParser.ReadLinesAsString(FilePath))
            .Select(ParseValve)
            .ToArray();

    private static Valve ParseValve(string line)
    {
        var match = _lineRegex.Match(line);
        return new()
        {
            Name = match.GetString("valve"),
            FlowRate = match.GetInt("rate"),
            Weight = 1,
            LeadsTo = match.GetStringArray("toValves", ", ")
        };
    }

    private struct Valve : IEdge, IEquatable<Valve>
    {
        public required string Name { get; init; }

        public required int FlowRate { get; init; }

        public required int Weight { get; init; }

        public string[] LeadsTo { get; set; }

        public bool Equals(IEdge? other) =>
            (other is Valve otherValve) && Equals(otherValve);

        public bool Equals(Valve other) =>
            other.Name == Name;

        public override string ToString() =>
            $"{Name} - {Weight}";
    }

    private readonly struct Signature : IEquatable<Signature>
    {
        public required string[] Value { get; init; }

        public override bool Equals(object? obj) =>
            (obj is Signature other) && Equals(other);

        public bool Equals(Signature other) =>
            Value.SequenceEqual(other.Value);

        public override int GetHashCode() =>
            ((IStructuralEquatable)Value).GetHashCode(EqualityComparer<string>.Default);

        public bool HasOverlap(Signature other) =>
            Value.Any(v => other.Value.Contains(v));

        public override string ToString() =>
            string.Join("-", Value);
    }
}
