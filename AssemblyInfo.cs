using System;
using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(ActiveBackground.BuildInfo.Name)]
[assembly: AssemblyDescription(ActiveBackground.BuildInfo.Description)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(ActiveBackground.BuildInfo.Company)]
[assembly: AssemblyProduct(ActiveBackground.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + ActiveBackground.BuildInfo.Author)]
[assembly: AssemblyTrademark(ActiveBackground.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(ActiveBackground.BuildInfo.Version)]
[assembly: AssemblyFileVersion(ActiveBackground.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(ActiveBackground.Main),
    ActiveBackground.BuildInfo.Name,
    ActiveBackground.BuildInfo.Version,
    ActiveBackground.BuildInfo.Author,
    ActiveBackground.BuildInfo.DownloadLink)]
[assembly: MelonColor(ConsoleColor.Blue)]

//[assembly: MelonOptionalDependencies("", "", "", "")]
// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("VRChat", "VRChat")]