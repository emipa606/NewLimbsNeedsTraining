using System.Reflection;
using HarmonyLib;
using Verse;

namespace NewLimbsNeedsTraining
{
    [StaticConstructorOnStartup]
    public static class NewLimbsNeedsTraining
    {
        static NewLimbsNeedsTraining()
        {
            var harmony = new Harmony("Mlie.NewLimbsNeedsTraining");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}