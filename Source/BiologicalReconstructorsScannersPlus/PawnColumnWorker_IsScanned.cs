using RimWorld;
using UnityEngine;
using Verse;

namespace BiologicalReconstructorsScannersPlus
{
    [StaticConstructorOnStartup]
    public class PawnColumnWorker_IsScanned : PawnColumnWorker
    {
        private static Texture2D animalAnalysisTexture = ContentFinder<Texture2D>.Get("UI/Issues/AM_AnimalAnalysis");

        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            var isInDatabase = GetActiveAnimalDatabase.IsInDatabase(pawn);

            if (isInDatabase == null)
                return;
            
            var checkboxRect = new Rect(rect.x + (int)((rect.width - 24f) / 2f), rect.y + Mathf.Max(3, 0), 24f, 24f);

            var color = GUI.color;

            GUI.color = isInDatabase.Value ? Color.green : Color.red;
            GUI.DrawTexture(checkboxRect, animalAnalysisTexture);

            GUI.color = color;

            if (Mouse.IsOver(checkboxRect))
            {
                var tip = GetTip(isInDatabase);
                if (!tip.NullOrEmpty())
                    TooltipHandler.TipRegion(checkboxRect, tip);
            }
		}

        public override void HeaderClicked(Rect headerRect, PawnTable table) 
            => CameraJumper.TryJumpAndSelect(GetActiveAnimalDatabase.GetDatabase());

        public string GetTip(bool? isInDatabase)
        {
            if (isInDatabase == null) 
                return null;
            return (isInDatabase.Value ? "AM_Analyzed" : "AM_NotAnalyzed").Translate();
        }

        public override string GetHeaderTip(PawnTable table) => null;
    }
}