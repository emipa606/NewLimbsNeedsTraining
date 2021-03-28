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
        private static float getOffset(int age, float incomingEfficency)
        {
            // 60000 ticks is one day
            // 900000 ticks is one quandrum (15 days)
            var day = 60000;
            var daysUntilFullEfficency = LoadedModManager.GetMod<NewLimbsNeedsTrainingMod>()
                .GetSettings<NewLimbsNeedsTrainingSettings>().DaysUntilRecovery;
            var factor = (float) age / (daysUntilFullEfficency * day);
            return Math.Min(incomingEfficency, incomingEfficency * factor);
        }

        [HarmonyPostfix]
        public static void Postfix(HediffSet diffSet, BodyPartRecord part, ref float __result)
        {
            if (part.depth != BodyPartDepth.Outside)
            {
                //Log.Message("Dont affect things we cannot see");
                return;
            }

            if (!diffSet.HasDirectlyAddedPartFor(part))
            {
                //Log.Message($"No directly added part for {part.def.defName}");
                return;
            }

            var hediffs = diffSet.GetHediffs<Hediff_AddedPart>();
            var hediff_AddedPart = hediffs.First(x => x.Part == part);
            var partEfficencyOffset = getOffset(hediff_AddedPart.ageTicks, __result);
            //Log.Message(
            //    $"{hediff_AddedPart.def.defName}: efficency set to {partEfficencyOffset}, down from {__result}");

            __result = partEfficencyOffset;
        }
    }
}