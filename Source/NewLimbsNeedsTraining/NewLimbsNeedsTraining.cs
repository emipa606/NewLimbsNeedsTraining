using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace NewLimbsNeedsTraining;

[StaticConstructorOnStartup]
public static class NewLimbsNeedsTraining
{
    public static readonly List<BodyPartTagDef> VitalBodyPartTags;

    static NewLimbsNeedsTraining()
    {
        VitalBodyPartTags = new List<BodyPartTagDef>
        {
            BodyPartTagDefOf.BloodPumpingSource,
            BodyPartTagDefOf.BreathingPathway,
            BodyPartTagDefOf.BreathingSource,
            BodyPartTagDefOf.BreathingSourceCage,
            BodyPartTagDefOf.ConsciousnessSource
        };
        var harmony = new Harmony("Mlie.NewLimbsNeedsTraining");
        harmony.PatchAll(Assembly.GetExecutingAssembly());


        var postfix =
            typeof(PawnGenerator_GenerateInitialHediffs).GetMethod(nameof(PawnGenerator_GenerateInitialHediffs
                .Postfix));

        if (postfix == null)
        {
            Log.Message(
                "[NewLimbsNeedTraining]: Failed to find postfix for PawnGenerator_GenerateInitialHediffs. Cannot add any extra patches.");
            return;
        }

        if (ModLister.GetActiveModWithIdentifier("twsta.compressedraid.latest") != null)
        {
            var addBionicsMethod = AccessTools.Method("CompressedRaid.BionicsDataStore:AddBionics",
                new[] { typeof(Pawn), typeof(float) });
            if (addBionicsMethod != null)
            {
                Log.Message(
                    "[NewLimbsNeedTraining]: Patching Compressed Raid AddBionics-method. Raiders should now have full efficency in their added hediffs.");
                harmony.Patch(addBionicsMethod, null, new HarmonyMethod(postfix));
            }
            else
            {
                Log.Message(
                    "[NewLimbsNeedTraining]: Failed to find Compressed Raid AddBionics-method. Raiders will enter the map with 0% efficency.");
            }
        }

        if (ModLister.GetActiveModWithIdentifier("VanillaStorytellersExpanded.WinstonWave") == null)
        {
            return;
        }

        var installPartMethod = AccessTools.Method("VSEWW.NextRaidInfo:InstallPart");
        if (installPartMethod == null)
        {
            Log.Message(
                "[NewLimbsNeedTraining]: Failed to find Winston Waves hediffgenerator. Raiders will enter the map with 0% efficency.");
            return;
        }

        Log.Message(
            "[NewLimbsNeedTraining]: Patching Winston Waves extra hediff-giver. Raiders should now have full efficency in their added hediffs.");
        harmony.Patch(installPartMethod, null, new HarmonyMethod(postfix));
    }
}