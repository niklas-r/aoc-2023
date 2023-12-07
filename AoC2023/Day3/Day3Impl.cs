using System.Diagnostics;
using System.Text.RegularExpressions;
using Utils;

namespace Day3;

public static class Day3Impl
{
    public static void Run()
    {
        var rows = "../../../input.txt".GetRows();
        Console.WriteLine("p1: " + P1(rows));
        Console.WriteLine("p2: " + P2(rows));
    }

    public static readonly Func<List<string>, int> P1 = (theRows) =>
    {
        var partNumbers = GetPartNumbers(theRows);

        // Dear god what I have done
        var validPartNumbers = partNumbers.Where(partNumber =>
        {
            var pattern = new Regex("[^0-9.]", RegexOptions.Compiled);
            var colWidth = theRows[0].Length;
            var rowHeight = theRows.Count;

            // scan top
            var x = partNumber.RowNumber;
            var y1 = partNumber.ColumnStart;
            var y2 = partNumber.ColumnEnd;

            if (x > 0)
            {
                var topSlice = theRows[x - 1][y1..(y2 + 1)];
                if (pattern.Match(topSlice).Success)
                {
                    return true;
                }
            }

            // scan right
            if (y2 + 1 < colWidth)
            {
                var rightSlice = theRows[x][y2 + 1].ToString();
                if (x > 0)
                {
                    rightSlice += theRows[x - 1][y2 + 1];
                }

                if (x + 1 < rowHeight)
                {
                    rightSlice += theRows[x + 1][y2 + 1];
                }

                if (pattern.Match(rightSlice).Success)
                {
                    return true;
                }
            }

            // scan bottom
            if (x + 1 < rowHeight)
            {
                var bottomSlice = theRows[x + 1][y1..(y2 + 1)];
                if (pattern.Match(bottomSlice).Success)
                {
                    return true;
                }
            }

            // scan left
            if (y1 > 0)
            {
                var leftSlice = theRows[x][y1 - 1].ToString();
                if (x > 0)
                {
                    leftSlice += theRows[x - 1][y1 - 1];
                }

                if (x + 1 < rowHeight)
                {
                    leftSlice += theRows[x + 1][y1 - 1];
                }

                if (pattern.Match(leftSlice).Success)
                {
                    return true;
                }
            }

            return false;
        }).ToArray();

        return validPartNumbers.Sum(x => x.Number);
    };

    public static readonly Func<List<string>, int> P2 = (theRows) =>
    {
        var partNumbers = GetPartNumbers(theRows);
        var gears = GetGears(theRows);
        var matches = gears.Select(gear =>
        {
            return gear with
            {
                AdjacentPartNumbers = partNumbers.Where(partNumber =>
                {
                    return partNumber.RowNumber - 1 <= gear.RowNumber && partNumber.RowNumber + 1 >= gear.RowNumber
                           &&
                           partNumber.ColumnStart - 1 <= gear.ColumnNumber && partNumber.ColumnEnd + 1 >= gear.ColumnNumber;
                })
            };
        }).Where(gear => gear.AdjacentPartNumbers.Count() == 2).ToArray();
        return matches.Sum(x => x.AdjacentPartNumbers.First().Number * x.AdjacentPartNumbers.Last().Number);
    };

    private static IEnumerable<PartNumber> GetPartNumbers(IEnumerable<string> theRows)
    {
        var partNumbers = Array.Empty<PartNumber>();

        foreach (var (row, i) in theRows.Select((row, i) => (row, i)))
        {
            PartNumber? lastPartNumber = null;
            foreach (var (c, j) in row.Select((c, j) => (c, j)))
            {
                if (char.IsDigit(c))
                {
                    if (lastPartNumber is null)
                    {
                        lastPartNumber = new PartNumber(c - '0', i, j, j);
                        continue;
                    }

                    lastPartNumber = lastPartNumber with
                    {
                        Number = Convert.ToInt32($"{lastPartNumber.Number}{c}"),
                        ColumnEnd = j
                    };
                }
                else
                {
                    if (lastPartNumber is not null)
                    {
                        Debug.Assert(lastPartNumber.Number is > 0 and < 1000);
                        partNumbers = partNumbers.Append(lastPartNumber).ToArray();
                        lastPartNumber = null;
                    }
                }
            }

            if (lastPartNumber is not null)
            {
                Debug.Assert(lastPartNumber.Number is > 0 and < 1000);
                partNumbers = partNumbers.Append(lastPartNumber).ToArray();
            }
        }

        return partNumbers;
    }

    private static IEnumerable<Gear> GetGears(IEnumerable<string> theRows)
    {
        var gears = Array.Empty<Gear>();

        foreach (var (row, i) in theRows.Select((row, i) => (row, i)))
        {
            foreach (var (c, j) in row.Select((c, j) => (c, j)))
            {
                if (c == '*')
                {
                    gears = gears.Append(new Gear(i, j, Array.Empty<PartNumber>())).ToArray();
                }
            }
        }

        return gears;
    }
}

internal record PartNumber(int Number, int RowNumber, int ColumnStart, int ColumnEnd);

internal record Gear(int RowNumber, int ColumnNumber, IEnumerable<PartNumber> AdjacentPartNumbers);