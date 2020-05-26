using Microsoft.Win32;

namespace BrowserChoice
{
    public static class RegistryHelper
    {
        private const string AppName = "RodkulmanBrowserChoice";

        private static RegistryKey CreateOrOpenSubKey(RegistryKey parent, string keyName)
        {
            return parent.OpenSubKey(keyName, true) ?? parent.CreateSubKey(keyName, true);
        }

        public static void RegisterApplication()
        {
            using (var appKey = CreateOrOpenSubKey(Registry.ClassesRoot, AppName))
            {
                appKey.SetValue(string.Empty, "Rodkulman's Browser Choice");

                using (var cmdKey = CreateOrOpenSubKey(appKey, @"shell\open\command"))
                {
                    cmdKey.SetValue(string.Empty, $"{System.Reflection.Assembly.GetExecutingAssembly().Location} \"%1\"");
                }
            }

            using (var appKey = CreateOrOpenSubKey(Registry.LocalMachine, $@"SOFTWARE\Clients\StartMenuInternet\{AppName}"))
            {
                using (var capabilitiesKey = CreateOrOpenSubKey(appKey, "Capabilities"))
                {
                    capabilitiesKey.SetValue("ApplicationDescription", "Opens links to your prefered browser");
                    capabilitiesKey.SetValue("ApplicationName", "Rodkulman's Browser Choice");                                        

                    using (var fileAssociationsKey = CreateOrOpenSubKey(capabilitiesKey, "FileAssociations"))
                    {
                        fileAssociationsKey.SetValue(".htm", "HTML File");
                        fileAssociationsKey.SetValue(".html", "HTML File");
                    }

                    using (var urlAssociationsKey = CreateOrOpenSubKey(capabilitiesKey, "UrlAssociations"))
                    {
                        urlAssociationsKey.SetValue("http", "URL");
                        urlAssociationsKey.SetValue("https", "URL");
                        urlAssociationsKey.SetValue("microsoft-edge", "Edge URL");
                    }
                }                

                using (var cmdKey = CreateOrOpenSubKey(appKey, @"shell\open\command"))
                {
                    cmdKey.SetValue(string.Empty, System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
            }

            using (var registeredAppsKey = CreateOrOpenSubKey(Registry.LocalMachine, @"SOFTWARE\RegisteredApplications"))
            {
                registeredAppsKey.SetValue(AppName, $@"SOFTWARE\Clients\StartMenuInternet\{AppName}\Capabilities");                
            }
        }
    }
}