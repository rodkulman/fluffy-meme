using System;

namespace BrowserChoice
{
    public class Rules
    {
        public Rules()
        {
        }

        public Rule GetRule(Uri uri)
        {
            return new Rule() { FilePath = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe" };
        }
    }
}