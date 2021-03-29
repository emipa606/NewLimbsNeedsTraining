using HarmonyLib;
using Verse;

namespace NewLimbsNeedsTraining
{
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", typeof(PawnGenerationRequest))]
    public class PawnGenerator_GeneratePawn
    {
        [HarmonyPostfix]
        public static void Postfix(ref Pawn __result)
        {
            foreach (var hediffAddedPart in __result.health.hediffSet.GetHediffs<Hediff_AddedPart>())
            {
                hediffAddedPart.ageTicks = Rand.Range(NewLimbsNeedsTraining.ticksUntilDone,
                    NewLimbsNeedsTraining.ticksUntilDone * 2);
            }
        }
    }
}