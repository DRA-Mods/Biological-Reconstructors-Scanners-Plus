using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace BiologicalReconstructorsScannersPlus;

[UsedImplicitly]
public class BiologicalReconstructorsScannersPlusMod : Mod
{
    private static Harmony harmony;
    public static Harmony Harmony => harmony ??= new Harmony("Dra.BiologicalReconstructorsScannersPlus");

    public BiologicalReconstructorsScannersPlusMod(ModContentPack content) : base(content)
    {
        Harmony.PatchAll();

        if (ModLister.GetActiveModWithIdentifier("Jaxe.RimHUD") != null)
            AssemblyLoader.LoadAssembly(content, "BiologicalReconstructorsScannersPlusRimhudCompat");
    }
}