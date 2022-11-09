using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace NewLimbsNeedsTraining;

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

        if (part.def.tags.Any(def => NewLimbsNeedsTraining.VitalBodyPartTags.Contains(def)))
        {
            return;
        }

        var hediffs = diffSet.hediffs.Where(hediff => hediff is Hediff_AddedPart);
        if (hediffs.First(x => x.Part == part) is not Hediff_AddedPart hediffAddedPart)
        {
            return;
        }

        // Werewolf parts
        if (hediffAddedPart.def.defName.StartsWith("ROM_"))
        {
            return;
        }

        if (hediffAddedPart.ageTicks > NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart))
        {
            return;
        }

        __result = getOffset(hediffAddedPart, __result);
    }

    private static float getOffset(Hediff_AddedPart hediffAddedPart, float incomingEfficency)
    {
        var factor = (float)hediffAddedPart.ageTicks / NewLimbsNeedsTrainingMod.TicksUntilDone(hediffAddedPart);
        return Math.Min(incomingEfficency,
            (incomingEfficency * factor) + NewLimbsNeedsTrainingMod.Instance.settings.StartValue);
    }
}