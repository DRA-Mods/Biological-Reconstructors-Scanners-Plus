using System.Linq;
using AlphaMemes;
using Verse;

namespace BiologicalReconstructorsScannersPlus;

public static class GetActiveAnimalDatabase
{
    private static Building_AnimalDatabase cached;
    private static int lastChecked = -1;

    public static Building_AnimalDatabase GetDatabase()
    {
        if ((cached == null || (cached.Spawned && !cached.Destroyed)) && lastChecked + 1000 >= Find.TickManager.TicksGame) 
            return cached;

        lastChecked = Find.TickManager.TicksGame;
        foreach (var map in Current.Game.Maps)
        {
            if (map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.AM_AnimalDatabase).FirstOrDefault() is not Building_AnimalDatabase building) 
                continue;

            cached = building;
            return building;
        }

        cached = null;
        return null;
    }

    public static bool? IsInDatabase(Pawn pawn)
    {
        var props = pawn.RaceProps;
        if (!props.Animal || props.Dryad)
            return null;

        var database = GetDatabase();
        return database?.analyzedAnimalList.Contains(pawn.kindDef);
    }
}