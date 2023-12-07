namespace Utils;

public static class AocExtensions
{
    public static List<string> GetRows(this string path)
    {
        var rows = File.ReadLines(path);

        var rowsList = rows.ToList();
        if (rowsList.Count == 0)
        {
            throw new Exception("No input");
        }

        return rowsList;
    }
}