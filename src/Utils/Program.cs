using System;
using System.Diagnostics;

namespace BrowserChoice
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            System.Diagnostics.Debugger.Launch();
            
            if (args.Length == 0)
            {
                BrowserChoice.App app = new BrowserChoice.App();
                app.InitializeComponent();
                app.Run();
            }
            else if (args.Length == 1 && IsUri(args[0], out Uri uri))
            {
                var rules = new Rules();
                var rule = rules.GetRule(uri);

                LaunchRule(rule, uri);
            }
        }

        private static void LaunchRule(Rule rule, Uri uri)
        {
            using (var p = new Process())
            {
                p.StartInfo.Arguments = uri.ToString();
                p.StartInfo.FileName = rule.FilePath;
                p.StartInfo.UseShellExecute = true;

                p.Start();
            }
        }

        private static bool IsUri(string arg, out Uri uri)
        {
            return IsMSEdgeUri(arg, out uri) || Uri.TryCreate(arg, UriKind.Absolute, out uri);
        }

        private static bool IsMSEdgeUri(string arg, out Uri uri)
        {
            if (arg.StartsWith("microsoft-edge:", StringComparison.OrdinalIgnoreCase) && !arg.Contains(" "))
            {
                uri = new Uri(arg.Substring(15), UriKind.Absolute);
                return true;
            }
            else
            {
                uri = null;
                return false;
            }
        }
    }
}