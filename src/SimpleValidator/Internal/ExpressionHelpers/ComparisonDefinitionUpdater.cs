using SimpleValidator.Internal.Keys;
using System.Text;

namespace SimpleValidator.Internal.ExpressionHelpers;

/// <summary>
/// Updates predicate definitions to new form when rule is copied to another property validator.
/// Adds the missing members to the rule definition if any.
/// </summary>
internal static class ComparisonDefinitionUpdater
{
    public static RuleKey UpdateDefinition(string oldName, string displayName)
    {
        List<int> res = FindAll(oldName, "prop1.");
        string path = displayName + ".";

        if (res.Count > 0)
        {
            StringBuilder sb = new(oldName, oldName.Length + (res.Count * path.Length));
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
            spanText = spanText[(index + searchText.Length)..];
            index = spanText.IndexOf(spanSearch, StringComparison.OrdinalIgnoreCase);
        }

        return positions;
    }
}
