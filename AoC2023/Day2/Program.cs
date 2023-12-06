var input = File.ReadLines("../../../input.txt");

Func<List<string>, List<Game>> parseGame = (rows) =>
{
    var games = new List<Game>();

    foreach (var row in rows)
    {
        var gameNumber = int.Parse(row.Split(":")[0].Replace("Game ", ""));
        var gameSets = row.Split(":")[1].Split(";");

        var sets = new List<GameSet>();

        foreach (var gameSet in gameSets)
        {
            var hands = gameSet.Split(",").Select(x => x.Trim()).ToArray();
            var set = new GameSet(0, 0, 0);

            foreach (var hand in hands)
            {
                var amount = int.Parse(hand[..2]);
                var color = hand[2..].Trim();
                set = color switch
                {
                    "red" => set with { Red = amount + set.Red },
                    "green" => set with { Green = amount + set.Green },
                    "blue" => set with { Blue = amount + set.Blue },
                    _ => throw new Exception("Unknown color")
                };
            }

            sets.Add(set);
        }

        games.Add(new Game(gameNumber, sets.ToArray()));
    }

    return games;
};

Func<List<string>, int> p1 = (rows) =>
{
    var games = parseGame(rows);

    var validGames = games.Where(x => x.Sets.All(y => y is { Red: <= 12, Green: <= 13, Blue: <= 14 })).ToList();
    return validGames.Sum(x => x.Number);
};

Func<List<string>, int> p2 = (rows) =>
{
    var games = parseGame(rows);

    var validGames = games.Select(x =>
        x with { Sets = new []
        {
            x.Sets.Aggregate(new GameSet(0, 0, 0),
                (acc, set) => new GameSet(
                    Red: int.Max(acc.Red, set.Red),
                    Green: int.Max(acc.Green, set.Green),
                    Blue: int.Max(acc.Blue, set.Blue)))
        }
    });
    return validGames.Sum(x => x.Sets.Sum(y => y.Red * y.Green * y.Blue));
};

var inputList = input.ToList();
if (inputList.Count == 0)
{
    throw new Exception("No input");
}

Console.WriteLine("p1: " + p1(inputList));
Console.WriteLine("p2: " + p2(inputList));

internal record GameSet(int Red, int Green, int Blue);
internal record Game(int Number, GameSet[] Sets);