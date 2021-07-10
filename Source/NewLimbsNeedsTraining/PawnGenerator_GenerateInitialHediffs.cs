using HarmonyLib;
using Verse;

namespace NewLimbsNeedsTraining
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateInitialHediffs", typeof(Pawn), typeof(PawnGenerationRequest))]
    public class PawnGenerator_GenerateInitialHediffs
    {
        [HarmonyPostfix]
        public static void Postfix(ref Pawn pawn, ref PawnGenerationRequest request)
        {
            //Log.Message($"NLNT: PawnGenerator_GenerateInitialHediffs checking if has added parts for {pawn}");
            foreach (var hediffAddedPart in pawn.health.hediffSet.GetHediffs<Hediff_AddedPart>())
            {
                if (hediffAddedPart.ageTicks >= 1)
                {
                    continue;
                }

                hediffAddedPart.ageTicks = Rand.Range(NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart),
                    NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart) * 2);
                pawn.health.Notify_HediffChanged(hediffAddedPart);
            }
        }
    }


    //[HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", typeof(PawnGenerationRequest))]
    //public class PawnGenerator_GeneratePawn
    //{
    //    [HarmonyPostfix]
    //    public static void Postfix(ref Pawn __result)
    //    {
    //        Log.Message($"NLNT: PawnGenerator_GeneratePawn resetting hediffs for {__result}");
    //        foreach (var hediffAddedPart in __result.health.hediffSet.GetHediffs<Hediff_AddedPart>())
    //        {
    //            if (hediffAddedPart.ageTicks >= 1)
    //            {
    //                continue;
    //            }

    //            hediffAddedPart.ageTicks = Rand.Range(NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart),
    //                NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart) * 2);
    //            __result.health.Notify_HediffChanged(hediffAddedPart);
    //        }
    //    }
    //}
}