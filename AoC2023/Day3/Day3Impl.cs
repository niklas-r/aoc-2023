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
    }

    public static readonly Func<List<string>, int> P1 = (theRows) =>
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
                        lastPartNumber = new PartNumber(c - '0', i, j);
                        continue;
                    }

                    lastPartNumber = lastPartNumber with
                    {
                        Number = Convert.ToInt32($"{lastPartNumber.Number}{c}")
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
                lastPartNumber = null;
            }
        }

        // Dear god what I have done
        var validPartNumbers = partNumbers.Where(partNumber =>
        {
            var pattern = new Regex("[^0-9.]", RegexOptions.Compiled);
            var colWidth = theRows[0].Length;
            var rowHeight = theRows.Count;

            // scan top
            var x = partNumber.RowNumber;
            var y = partNumber.ColumnNumber;
            var pLen = partNumber.Number.ToString().Length;

            if (x > 0)
            {
                var topSlice = string.Join("", theRows[x - 1].Skip(y).Take(pLen));
                if (pattern.Match(topSlice).Success)
                {
                    return true;
                }
            }

            // scan right
            if (y + pLen < colWidth)
            {
                var rightSlice = theRows[x][y + pLen].ToString();
                if (x > 0)
                {
                    rightSlice += theRows[x - 1][y + pLen].ToString();
                }

                if (x + 1 < rowHeight)
                {
                    rightSlice += theRows[x + 1][y + pLen].ToString();
                }

                if (pattern.Match(rightSlice).Success)
                {
                    return true;
                }
            }

            // scan bottom
            if (x + 1 < rowHeight)
            {
                var bottomSlice = string.Join("", theRows[x + 1].Skip(y).Take(pLen));
                if (pattern.Match(bottomSlice).Success)
                {
                    return true;
                }
            }

            // scan left
            if (y > 0)
            {
                var leftSlice = theRows[x][y - 1].ToString();
                if (x > 0)
                {
                    leftSlice += theRows[x - 1][y - 1].ToString();
                }

                if (x + 1 < rowHeight)
                {
                    leftSlice += theRows[x + 1][y - 1].ToString();
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
}

internal record PartNumber(int Number, int RowNumber, int ColumnNumber);