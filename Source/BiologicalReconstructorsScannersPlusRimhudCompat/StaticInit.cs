using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimHUD.Data.Configuration;
using RimHUD.Data.Extensions;
using RimHUD.Data.Models;
using RimHUD.Interface.Dialog;
using RimHUD.Interface.HUD;
using Verse;

namespace BiologicalReconstructorsScannersPlus.Rimhud;

public static class StaticInit
{
    private const string ID = "AlphaMemesIsScanned";

    public static void Init()
    {
        HudModel.Widgets.Add(ID, Value);

        BiologicalReconstructorsScannersPlusMod.Harmony.Patch(AccessTools.Method(typeof(GUIExtensions), nameof(GUIExtensions.ShowMenu)),
            prefix: new HarmonyMethod(typeof(StaticInit), nameof(PreShowMenu)));
    }

    private static HudWidgetBase Value(PawnModel model)
    {
        var inDatabase = GetActiveAnimalDatabase.IsInDatabase(model.Base);
        if (inDatabase == null) return null;

        return HudValue.FromTextModel(new TextModel((inDatabase.Value ? "AM_Analyzed" : "AM_NotAnalyzed").Translate(), null, () =>
        {
            var database = GetActiveAnimalDatabase.GetDatabase();
            if (database is { Spawned: true, Destroyed: false }) database.ShowAnimals();
        }), Theme.RegularTextStyle);
    }

    private static void PreShowMenu(ref IEnumerable<FloatMenuOption> self)
    {
        var floatMenuOptions = self as FloatMenuOption[] ?? self.ToArray();
        if (floatMenuOptions.All(x => x.Label != HudModel.ElementComponents[0].Label)) return;

        var custom = new FloatMenuOption("RimHUD.Model.Component.AlphaMemesIsScanned".Translate(), () =>
        {
            var tab = Find.WindowStack.WindowOfType<Dialog_Config>()._tabs._selected as Tab_ConfigContent;
            tab?._editor.Add(new LayoutItem(LayoutItemType.Element, ID));
        });

        self = floatMenuOptions.Concat(custom);
    }
}