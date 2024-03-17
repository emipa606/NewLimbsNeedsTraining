using System.Collections.Generic;
using System.Linq;
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
        VitalBodyPartTags =
        [
            BodyPartTagDefOf.BloodPumpingSource,
            BodyPartTagDefOf.BreathingPathway,
            BodyPartTagDefOf.BreathingSource,
            BodyPartTagDefOf.BreathingSourceCage,
            BodyPartTagDefOf.ConsciousnessSource
        ];
        var harmony = new Harmony("Mlie.NewLimbsNeedsTraining");
        harmony.PatchAll(Assembly.GetExecutingAssembly());


        var postfix =
            typeof(PawnGenerator_GenerateInitialHediffs).GetMethod(nameof(PawnGenerator_GenerateInitialHediffs
                .Postfix));
        var genePostfix = typeof(NewLimbsNeedsTraining).GetMethod(nameof(GenePostfix));

        if (postfix == null)
        {
            Log.Message(
                "[NewLimbsNeedTraining]: Failed to find postfix for PawnGenerator_GenerateInitialHediffs. Cannot add any extra patches.");
            return;
        }

        if (ModLister.GetActiveModWithIdentifier("twsta.compressedraid.latest") != null)
        {
            var addBionicsMethod = AccessTools.Method("CompressedRaid.BionicsDataStore:AddBionics",
                [typeof(Pawn), typeof(float)]);
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

        if (ModLister.GetActiveModWithIdentifier("OskarPotocki.VanillaFactionsExpanded.Core") != null)
        {
            var applyGeneEffectsOverrideMethod = AccessTools.Method(
                "VanillaGenesExpanded.VanillaGenesExpanded_Gene_OverrideBy_Patch:ApplyGeneEffects",
                [typeof(Gene)]);
            var applyGeneEffectsPostAddMethod = AccessTools.Method(
                "VanillaGenesExpanded.VanillaGenesExpanded_Gene_PostAdd_Patch:ApplyGeneEffects",
                [typeof(Gene)]);
            if (applyGeneEffectsOverrideMethod != null && applyGeneEffectsPostAddMethod != null)
            {
                Log.Message(
                    "[NewLimbsNeedTraining]: Patching Vanilla Expanded ApplyGeneEffects-method. Pawns spawning with replaced bodyparts using gene-effects will have full efficency in their added hediffs.");
                harmony.Patch(applyGeneEffectsOverrideMethod, null, new HarmonyMethod(genePostfix));
                harmony.Patch(applyGeneEffectsPostAddMethod, null, new HarmonyMethod(genePostfix));
            }
            else
            {
                Log.Message(
                    "[NewLimbsNeedTraining]: Failed to find Vanilla Expanded ApplyGeneEffects-method. Pawns spawning with replaced bodyparts using gene-effects will enter the map with 0% efficency.");
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

    public static void GenePostfix(ref Gene gene)
    {
        foreach (var hediffAddedPart in gene.pawn.health.hediffSet.hediffs.Where(hediff =>
                     hediff is Hediff_AddedPart))
        {
            if (hediffAddedPart.ageTicks >= 1)
            {
                continue;
            }

            var hediff = hediffAddedPart as Hediff_AddedPart;

            hediffAddedPart.ageTicks = Rand.Range(NewLimbsNeedsTrainingMod.TicksUntilDone(hediff),
                NewLimbsNeedsTrainingMod.TicksUntilDone(hediff) * 2);
            gene.pawn.health.Notify_HediffChanged(hediffAddedPart);
        }
    }
}