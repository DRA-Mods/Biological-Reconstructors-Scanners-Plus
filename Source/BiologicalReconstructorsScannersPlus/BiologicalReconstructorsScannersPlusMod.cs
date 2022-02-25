using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace BiologicalReconstructorsScannersPlus
{
    [UsedImplicitly]
    public class BiologicalReconstructorsScannersPlusMod : Mod
    {
        public BiologicalReconstructorsScannersPlusMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("Dra.BiologicalReconstructorsScannersPlus");
            harmony.PatchAll();

            if (ModLister.GetActiveModWithIdentifier("Jaxe.RimHUD") != null) 
                AssemblyLoader.LoadAssembly(content, "BiologicalReconstructorsScannersPlusRimhudCompat.dll", harmony);
        }
    }
}