using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using Mono.Cecil;
using Verse;

namespace BiologicalReconstructorsScannersPlus;

internal static class AssemblyLoader
{
    public static void LoadAssembly(ModContentPack content, string targetAssembly, string targetModCompat)
    {
        if (!targetAssembly.EndsWith(".dll"))
            targetAssembly += ".dll";
        
        var assemblies = ModContentPack.GetAllFilesForModPreserveOrder(content, "Referenced/", f => f.ToLower() == ".dll");
        var path = assemblies.FirstOrDefault(f => string.Equals(f.Item2.Name, targetAssembly, StringComparison.InvariantCultureIgnoreCase))?.Item2;

        if (path == null)
        {
            Log.Error($"Could not find target assembly: {targetAssembly} - {targetModCompat} patch is not going to work!");
            return;
        }

        var assembly = AssemblyDefinition.ReadAssembly(path.FullName);
        using var stream = new MemoryStream();
        assembly.Write(stream);

        var result = AppDomain.CurrentDomain.Load(stream.ToArray());
        BiologicalReconstructorsScannersPlusMod.Harmony.PatchAll(result);
        var type = result.GetTypes().FirstOrDefault(x => x.Name == "StaticInit");
        if (type != null)
            AccessTools.DeclaredMethod(type, "Init", Type.EmptyTypes)?.Invoke(null, Array.Empty<object>());
    }
}