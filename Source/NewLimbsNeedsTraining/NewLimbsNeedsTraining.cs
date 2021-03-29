using System.Reflection;
using HarmonyLib;
using Verse;

namespace NewLimbsNeedsTraining
{
    [StaticConstructorOnStartup]
    public static class NewLimbsNeedsTraining
    {
        private static readonly int oneDayInTicks = 60000;

        public static readonly int ticksUntilDone = LoadedModManager.GetMod<NewLimbsNeedsTrainingMod>()
            .GetSettings<NewLimbsNeedsTrainingSettings>().DaysUntilRecovery * oneDayInTicks;

        static NewLimbsNeedsTraining()
        {
            var harmony = new Harmony("Mlie.NewLimbsNeedsTraining");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}