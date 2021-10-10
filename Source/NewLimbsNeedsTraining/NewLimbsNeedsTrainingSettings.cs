using Verse;

namespace NewLimbsNeedsTraining
{
    /// <summary>
    ///     Definition of the settings for the mod
    /// </summary>
    internal class NewLimbsNeedsTrainingSettings : ModSettings
    {
        public int Archotech = 1;
        public int Industrial = 15;
        public int Medieval = 25;
        public int Neolithic = 30;
        public int Spacer = 10;
        public float StartValue;
        public int Ultra = 5;

        /// <summary>
        ///     Saving and loading the values
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref Neolithic, "Neolithic", 30);
            Scribe_Values.Look(ref Medieval, "Medieval", 25);
            Scribe_Values.Look(ref Industrial, "Industrial", 15);
            Scribe_Values.Look(ref Spacer, "Spacer", 10);
            Scribe_Values.Look(ref Ultra, "Ultra", 5);
            Scribe_Values.Look(ref Archotech, "Archotech", 1);
            Scribe_Values.Look(ref StartValue, "StartValue");
        }
    }
}