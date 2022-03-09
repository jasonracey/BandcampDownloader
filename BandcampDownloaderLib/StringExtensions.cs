namespace BandcampDownloaderLib;

public static class StringExtensions
{
    public static string RepeatedlyReplace(this string input, string oldValue, string newValue)
    {
        while (input.Contains(oldValue))
        {
            input = input.Replace(oldValue, newValue);
        }

        return input;
    }
}