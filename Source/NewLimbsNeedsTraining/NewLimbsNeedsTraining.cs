using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace NewLimbsNeedsTraining
{
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
        }
    }
}