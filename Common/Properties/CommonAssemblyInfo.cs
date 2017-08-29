using System.Resources;
using System.Reflection;

[assembly: AssemblyCompany("DevUtils.Net")]
[assembly: AssemblyProduct("ETWIMBA")]
[assembly: AssemblyCopyright("Copyright © 2015 DevUtils. All rights reserved.")]
[assembly: AssemblyTrademark("Copyright ©")]
[assembly: AssemblyInformationalVersion("Manual Build")]
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]

#if !DEBUG
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("2.0.1.0")]
#else
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyFileVersion("2.0.1.1")]
#endif