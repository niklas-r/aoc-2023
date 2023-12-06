var input = File.ReadLines("../../../input.txt");

Func<List<string>, int> p1 = (rows) =>
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
    var validGames = games.Where(x => x.Sets.All(y => y is { Red: <= 12, Green: <= 13, Blue: <= 14 })).ToList();
    return validGames.Sum(x => x.Number);
};

var inputList = input.ToList();
if (inputList.Count == 0)
{
    throw new Exception("No input");
}

Console.WriteLine(p1(inputList));

internal record GameSet(int Red, int Green, int Blue);
internal record Game(int Number, GameSet[] Sets);