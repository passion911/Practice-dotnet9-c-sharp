namespace Practice;

public class LongestUniqueSubstring
{
    public static (int length, string substring) LongestUniqueSubstringAlgorithm(string s)
    {
        if ( string.IsNullOrEmpty(s) ) return (0, "");

        var chars = new HashSet<char>();
        int left = 0, maxLength = 0, maxStart = 0;

        for ( int right = 0; right < s.Length; right++ )
        {
            while ( chars.Contains(s[right]) )
            {
                chars.Remove(s[left]);
                left++;
            }

            chars.Add(s[right]);

            if ( right - left + 1 > maxLength )
            {
                maxLength = right - left + 1;
                maxStart = left;
            }
        }

        return (maxLength, s.Substring(maxStart, maxLength));
    }
}
