using System.Reflection;

var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "../../../input.txt");
var input = File.ReadLines(path);

var total = 0;

foreach (var l in input)
{
    int? first = null;
    int? last = null;

    foreach (var c in l.Where(char.IsDigit))
    {
        last = c - '0';
        first ??= last;
    }

    total += int.Parse(first + "" + last);
}

Console.WriteLine(total);