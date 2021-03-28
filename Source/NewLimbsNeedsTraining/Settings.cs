using Verse;

namespace NewLimbsNeedsTraining
{
    /// <summary>
    ///     Definition of the settings for the mod
    /// </summary>
    internal class NewLimbsNeedsTrainingSettings : ModSettings
    {
        public int DaysUntilRecovery = 15;

        /// <summary>
        ///     Saving and loading the values
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref DaysUntilRecovery, "DaysUntilRecovery", 3);
        }
    }
}