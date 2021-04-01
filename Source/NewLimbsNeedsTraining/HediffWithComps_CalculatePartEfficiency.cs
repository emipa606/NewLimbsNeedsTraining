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
        private static float getOffset(Hediff_AddedPart hediffAddedPart, float incomingEfficency)
        {
            var factor = (float) hediffAddedPart.ageTicks / NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart);
            return Math.Min(incomingEfficency, incomingEfficency * factor);
        }

        [HarmonyPostfix]
        public static void Postfix(HediffSet diffSet, BodyPartRecord part, ref float __result)
        {
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
            var hediffAddedPart = hediffs.First(x => x.Part == part);
            if (hediffAddedPart.ageTicks > NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart))
            {
                // Log.Message($"Hediff trained {part.def.defName}");
                return;
            }

            __result = getOffset(hediffAddedPart, __result);
        }
    }
}