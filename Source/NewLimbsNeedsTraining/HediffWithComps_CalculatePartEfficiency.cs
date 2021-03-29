using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace NewLimbsNeedsTraining
{
    [HarmonyPatch(typeof(PawnCapacityUtility), "CalculatePartEfficiency", typeof(HediffSet), typeof(BodyPartRecord),
        typeof(bool), typeof(List<PawnCapacityUtility.CapacityImpactor>))]
    public class HediffWithComps_CalculatePartEfficiency
    {
        private static readonly int oneDayInTicks = 60000;

        private static readonly int ticksUntilDone = LoadedModManager.GetMod<NewLimbsNeedsTrainingMod>()
            .GetSettings<NewLimbsNeedsTrainingSettings>().DaysUntilRecovery * oneDayInTicks;

        private static float getOffset(int age, float incomingEfficency)
        {
            // 60000 ticks is one day
            // 900000 ticks is one quandrum (15 days)
            var factor = (float) age / ticksUntilDone;
            return Math.Min(incomingEfficency, incomingEfficency * factor);
        }

        [HarmonyPostfix]
        public static void Postfix(HediffSet diffSet, BodyPartRecord part, ref float __result)
        {
            if (!diffSet.pawn.IsColonist && !diffSet.pawn.IsPrisonerOfColony)
            {
                // Log.Message("Probably shouldnt affect raiders and travelers");
                return;
            }

            if (part.depth != BodyPartDepth.Outside)
            {
                // Log.Message("Dont affect things we cannot see");
                return;
            }

            if (!diffSet.HasDirectlyAddedPartFor(part))
            {
                // Log.Message($"No directly added part for {part.def.defName}");
                return;
            }

            var hediffs = diffSet.GetHediffs<Hediff_AddedPart>();
            var hediff_AddedPart = hediffs.First(x => x.Part == part);
            if (hediff_AddedPart.ageTicks > ticksUntilDone)
            {
                // Log.Message($"Hediff trained {part.def.defName}");
                return;
            }

            __result = getOffset(hediff_AddedPart.ageTicks, __result);
        }
    }
}