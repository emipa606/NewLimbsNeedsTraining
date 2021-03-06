using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace NewLimbsNeedsTraining
{
    [HarmonyPatch(typeof(PawnCapacityUtility), "CalculatePartEfficiency", typeof(HediffSet), typeof(BodyPartRecord),
        typeof(bool), typeof(List<PawnCapacityUtility.CapacityImpactor>))]
    public class HediffWithComps_CalculatePartEfficiency
    {
        [HarmonyPostfix]
        public static void Postfix(HediffSet diffSet, BodyPartRecord part, ref float __result)
        {
            if (part.depth != BodyPartDepth.Outside)
            {
                return;
            }

            if (!diffSet.HasDirectlyAddedPartFor(part))
            {
                return;
            }

            if (GenTicks.TicksGame < 1)
            {
                return;
            }

            var vitalBodyPartTags = new List<BodyPartTagDef>
            {
                BodyPartTagDefOf.BloodPumpingSource,
                BodyPartTagDefOf.BreathingPathway,
                BodyPartTagDefOf.BreathingSource,
                BodyPartTagDefOf.BreathingSourceCage,
                BodyPartTagDefOf.ConsciousnessSource
            };

            if (part.def.tags.Any(def => vitalBodyPartTags.Contains(def)))
            {
                return;
            }

            var hediffs = diffSet.GetHediffs<Hediff_AddedPart>();
            var hediffAddedPart = hediffs.First(x => x.Part == part);
            if (hediffAddedPart.ageTicks > NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart))
            {
                return;
            }

            __result = getOffset(hediffAddedPart, __result);
        }

        private static float getOffset(Hediff_AddedPart hediffAddedPart, float incomingEfficency)
        {
            var factor = (float) hediffAddedPart.ageTicks / NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart);
            return Math.Min(incomingEfficency, incomingEfficency * factor);
        }
    }
}