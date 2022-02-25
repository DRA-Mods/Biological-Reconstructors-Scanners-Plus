using System.Text;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace BiologicalReconstructorsScannersPlus
{
    internal static class HarmonyPatches
    {
        [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetInspectString))]
        private static class PatchPawnGetInspectString
        {
            [UsedImplicitly]
            private static void Postfix(Pawn __instance, ref string __result)
            {
                var inDatabase = GetActiveAnimalDatabase.IsInDatabase(__instance);

                if (inDatabase == null)
                    return;

                var output = new StringBuilder(__result);
                output.AppendLine();
                output.Append((inDatabase.Value ? "AM_Analyzed" : "AM_NotAnalyzed").Translate());
                __result = output.ToString();
            }
        }
    }
}
