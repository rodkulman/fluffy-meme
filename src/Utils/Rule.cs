using System.Text.RegularExpressions;

namespace BrowserChoice
{
    public enum SearchType { Domain, Regex }

    public class Rule
    {
        public string FilePath { get; set; }
        public string Arguments { get; set; }
        public SearchType SearchType { get; set; }
        public string Domain { get; set; }
        public string RegexPattern { get; set; }
        public bool IsDefault { get; set; }
        public RegexOptions RegexOptions { get; set; }
    }
}