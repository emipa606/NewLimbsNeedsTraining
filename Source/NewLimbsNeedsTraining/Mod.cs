using UnityEngine;
using Verse;

namespace NewLimbsNeedsTraining
{
    [StaticConstructorOnStartup]
    internal class NewLimbsNeedsTrainingMod : Mod
    {
        /// <summary>
        ///     The private settings
        /// </summary>
        private NewLimbsNeedsTrainingSettings settings;

        /// <summary>
        ///     Cunstructor
        /// </summary>
        /// <param name="content"></param>
        public NewLimbsNeedsTrainingMod(ModContentPack content) : base(content)
        {
        }

        /// <summary>
        ///     The instance-settings for the mod
        /// </summary>
        private NewLimbsNeedsTrainingSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = GetSettings<NewLimbsNeedsTrainingSettings>();
                }

                return settings;
            }
            set => settings = value;
        }

        /// <summary>
        ///     The title for the mod-settings
        /// </summary>
        /// <returns></returns>
        public override string SettingsCategory()
        {
            return "New Limbs Needs Training";
        }

        /// <summary>
        ///     The settings-window
        ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
        /// </summary>
        /// <param name="rect"></param>
        public override void DoSettingsWindowContents(Rect rect)
        {
            var listing_Standard = new Listing_Standard();
            listing_Standard.Begin(rect);
            listing_Standard.Gap();
            listing_Standard.Label("NLNT.DaysUntilRecoveryLabel".Translate(Settings.DaysUntilRecovery), -1,
                "NLNT.DaysUntilRecoveryToolTip".Translate());
            listing_Standard.IntAdjuster(ref Settings.DaysUntilRecovery, 1, 1);
            listing_Standard.End();
            Settings.Write();
        }
    }
}