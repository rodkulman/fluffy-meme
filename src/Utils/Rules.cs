using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrowserChoice
{
    public class Rules
    {
        private static Rules rulesInstance;
        public static Rules Instance => rulesInstance ?? (rulesInstance = new Rules());

        public IEnumerable<Rule> List => rules;

        private Rule[] rules;

        private Rules() 
        { 
            EnsureFile("rules.json");

            using (var stream = File.Open("rules.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var textReader = new StreamReader(stream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                var jRules = JObject.Load(jsonReader);

                switch (jRules.Value<int>("version"))
                {
                    case 0:
                        rules = LoadRulesVersion0(jRules["rules"]).ToArray();
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public void Save(IEnumerable<Rule> rules)
        {
            this.rules = rules.ToArray();
            SaveToFile();
        }

        private IEnumerable<Rule> LoadRulesVersion0(IEnumerable<JToken> rules)
        {
            foreach (var rule in rules)
            {
                yield return new Rule()
                {
                    FilePath = rule.Value<string>(nameof(Rule.FilePath)),
                    Arguments = rule.Value<string>(nameof(Rule.Arguments)),
                    Domain = rule.Value<string>(nameof(Rule.Domain)),
                    RegexPattern = rule.Value<string>(nameof(Rule.RegexPattern)),
                    RegexOptions = (RegexOptions)rule.Value<int>(nameof(Rule.RegexOptions)),
                    SearchType = (SearchType)rule.Value<int>(nameof(Rule.SearchType)),
                    IsDefault = rule.Value<bool>(nameof(Rule.IsDefault))
                };
            }
        }        

        private void EnsureFile(string path)
        {
            if (!File.Exists(path))
            {
                rules = new Rule[0];
                SaveToFile();
            }
        }

        private void SaveToFile()
        {
            var jObject = new JObject(
                new JProperty("version", 0),
                new JProperty("rules", new JArray(
                    rules.Select(r => new JObject(
                        new JProperty(nameof(r.FilePath), r.FilePath),
                        new JProperty(nameof(r.Arguments), r.Arguments),
                        new JProperty(nameof(r.SearchType), r.SearchType),
                        new JProperty(nameof(r.Domain), r.Domain),
                        new JProperty(nameof(r.RegexPattern), r.RegexPattern),
                        new JProperty(nameof(r.IsDefault), r.IsDefault),
                        new JProperty(nameof(r.RegexOptions), r.RegexOptions)
                    ))
                ))
            );

            using (var stream = File.Open("rules.json", FileMode.Create, FileAccess.Write, FileShare.None))
            using (var textWriter = new StreamWriter(stream, Encoding.UTF8))
            using (var writer = new JsonTextWriter(textWriter))
            {
                jObject.WriteTo(writer);
            }
        }

        public Rule GetRuleForUri(Uri uri)
        {
            var retVal = rules.FirstOrDefault(rule =>
            {
                return 
                    (rule.SearchType == SearchType.Domain && uri.Host.EndsWith(rule.Domain, StringComparison.OrdinalIgnoreCase)) ||
                    (rule.SearchType == SearchType.Regex && Regex.IsMatch(uri.ToString(), rule.RegexPattern, rule.RegexOptions));
            });

            return retVal ?? rules.Single(rule => rule.IsDefault);
        }
    }
}