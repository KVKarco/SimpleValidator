namespace SimpleValidator.Internal.ExpressionHelpers;

using SimpleValidator.Internal.Keys;
using System.Text;

internal static class ComparisonDefinitionUpdater
{
    public static RuleKey UpdateDefinition(string oldName, string displayName)
    {
        List<int> res = FindAll(oldName, "prop1.");
        string path = displayName + ".";

        if (res.Count > 0)
        {
            StringBuilder sb = new StringBuilder(oldName, oldName.Length + (res.Count * path.Length));
            int occurred = 0;

            foreach (var item in res)
            {
                int index = item + (path.Length * occurred);
                sb.Insert(index, path);
                occurred++;
            }

            return RuleKey.FromString(sb.ToString());
        }

        return RuleKey.FromString(oldName);
    }

    private static List<int> FindAll(string text, string searchText)
    {
        var spanText = text.AsSpan();
        var spanSearch = searchText.AsSpan();

        List<int> positions = [];

        var offset = 0;
        var index = spanText.IndexOf(spanSearch, StringComparison.OrdinalIgnoreCase);

        while (index != -1)
        {
            positions.Add(index + offset + searchText.Length);
            offset += index + searchText.Length;
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
            spanText = spanText[(index + searchText.Length)..];
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly
            index = spanText.IndexOf(spanSearch, StringComparison.OrdinalIgnoreCase);
        }

        return positions;
    }
}
