using System.Linq;

namespace Game.Models;

public static class CardsActions {
    private static readonly Random rng = new Random();
    private static List<int> Duplicate(int value, int times)
        => Enumerable.Range(0, times).Select(_ => value).ToList();
    public static List<int> Fill(int playerCount) 
        => Enumerable.Range(1, 10).Select(i => Duplicate(value: i, times:playerCount))
                    .Aggregate(new List<int>(), (left, right) => {
                        left.AddRange(right);
                        return left;
                    })
                    .Shuffle().ToList();
    public static List<int> Shuffle(this List<int> deck)
        => deck.OrderBy(_ => rng.Next()).ToList();
    public static List<int[]> Split(this List<int> deck, int size) 
        => deck.Count % size == 0 ? deck.Chunk(deck.Count / size).ToList() : throw new Exception($"Cant split deck into {size} equal pieces");
    public static Dictionary<int, List<int>> Distribute(int playerCount)
        => Fill(playerCount)
        .Split(playerCount)
        .Select((d, i) => new {
            PLayer = i, 
            Deck = d.ToList()
        }).ToDictionary(hande => hande.PLayer, hand => hand.Deck);
    public static (List<int>, Dictionary<int, List<int>>) Distribute(this List<int> deck, int playerCount)
        => (
            new List<int>(), 
            deck.Split(playerCount)
                .Select((d, i) => new {
                    PLayer = i, 
                    Deck = d.ToList()
                }).ToDictionary(hande => hande.PLayer, hand => hand.Deck));
}