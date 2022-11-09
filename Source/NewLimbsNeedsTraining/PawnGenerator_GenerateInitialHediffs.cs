using System.Linq;
using HarmonyLib;
using Verse;

namespace NewLimbsNeedsTraining;

[HarmonyPatch(typeof(PawnGenerator), "GenerateInitialHediffs", typeof(Pawn), typeof(PawnGenerationRequest))]
public class PawnGenerator_GenerateInitialHediffs
{
    [HarmonyPostfix]
    public static void Postfix(ref Pawn pawn, ref PawnGenerationRequest request)
    {
        //Log.Message($"NLNT: PawnGenerator_GenerateInitialHediffs checking if has added parts for {pawn}");
        foreach (var hediffAddedPart in pawn.health.hediffSet.hediffs.Where(hediff =>
                     hediff is Hediff_AddedPart))
        {
            if (hediffAddedPart.ageTicks >= 1)
            {
                continue;
            }

            var hediff = hediffAddedPart as Hediff_AddedPart;

            hediffAddedPart.ageTicks = Rand.Range(NewLimbsNeedsTrainingMod.TicksUntilDone(hediff),
                NewLimbsNeedsTrainingMod.TicksUntilDone(hediff) * 2);
            pawn.health.Notify_HediffChanged(hediffAddedPart);
        }
    }
}
