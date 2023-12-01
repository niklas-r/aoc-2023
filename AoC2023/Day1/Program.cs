using System.Reflection;

var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "../../../input.txt");
var input = File.ReadLines(path);

var total = 0;
var words = new[]
{
    "one",
    "two",
    "three",
    "four",
    "five",
    "six",
    "seven",
    "eight",
    "nine"
};

foreach (var l in input)
{
    int? first = null;
    int? last = null;

    foreach (var (c, i) in l.Select((c, i) => (c, i)))
    {
        if (char.IsDigit(c))
        {
            last = c - '0';
            first ??= last;
            continue;
        }

        foreach (var (w, j) in words.Select((w, j) => (w, j)))
        {
            if (l[i..].StartsWith(w))
            {
                last = j + 1;
                first ??= j + 1;
            }
        }
    }

    total += int.Parse(first + "" + last);
}

Console.WriteLine(total);