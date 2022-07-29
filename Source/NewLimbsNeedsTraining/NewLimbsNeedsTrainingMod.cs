using System;
using System.Collections.Generic;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;

namespace NewLimbsNeedsTraining;

[StaticConstructorOnStartup]
internal class NewLimbsNeedsTrainingMod : Mod
{
    private static readonly Dictionary<TechLevel, int> DaysUntilRecovery = new Dictionary<TechLevel, int>();

    private static string currentVersion;

    public static NewLimbsNeedsTrainingMod Instance;

    /// <summary>
    ///     The private settings
    /// </summary>
    public NewLimbsNeedsTrainingSettings settings;


    /// <summary>
    ///     Cunstructor
    /// </summary>
    /// <param name="content"></param>
    public NewLimbsNeedsTrainingMod(ModContentPack content) : base(content)
    {
        Instance = this;
        updateTechLevels();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.NewLimbsNeedsTraining"));
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

    private void updateTechLevels()
    {
        foreach (var techLevel in (TechLevel[])Enum.GetValues(typeof(TechLevel)))
        {
            DaysUntilRecovery[techLevel] = techLevel switch
            {
                TechLevel.Neolithic => Settings.Neolithic,
                TechLevel.Medieval => Settings.Medieval,
                TechLevel.Industrial => Settings.Industrial,
                TechLevel.Spacer => Settings.Spacer,
                TechLevel.Ultra => Settings.Ultra,
                TechLevel.Archotech => Settings.Archotech,
                _ => 15
            };
        }
    }

    public static int TicksUntilDone(Hediff_AddedPart part)
    {
        var ticksUntilDone = GenDate.TicksPerDay * DaysUntilRecovery[TechLevel.Industrial];
        if (part.def.spawnThingOnRemoved == null ||
            !DaysUntilRecovery.ContainsKey(part.def.spawnThingOnRemoved.techLevel))
        {
            return ticksUntilDone;
        }

        ticksUntilDone = GenDate.TicksPerDay * DaysUntilRecovery[part.def.spawnThingOnRemoved.techLevel];
        //Log.Message(
        //$"{part.def.spawnThingOnRemoved.defName} - {part.def.spawnThingOnRemoved.techLevel} - {ticksUntilDone}");

        return ticksUntilDone;
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
        var spacer = 30;
        listing_Standard.Label("NLNT.DaysUntilRecoveryLabel".Translate(), -1,
            "NLNT.DaysUntilRecoveryToolTip".Translate());
        listing_Standard.Gap();
        Settings.Neolithic = (int)Widgets.HorizontalSlider(listing_Standard.GetRect(spacer), Settings.Neolithic, 0,
            100f, false, $"{"Neolithic".Translate()}: {"NLNT.Days".Translate(Settings.Neolithic)}", null, null, 1);
        Settings.Medieval = (int)Widgets.HorizontalSlider(listing_Standard.GetRect(spacer), Settings.Medieval, 0,
            100f,
            false, $"{"Medieval".Translate()}: {"NLNT.Days".Translate(Settings.Medieval)}", null, null, 1);
        Settings.Industrial = (int)Widgets.HorizontalSlider(listing_Standard.GetRect(spacer), Settings.Industrial,
            0,
            100f, false, $"{"Industrial".Translate()}: {"NLNT.Days".Translate(Settings.Industrial)}", null, null,
            1);
        Settings.Spacer = (int)Widgets.HorizontalSlider(listing_Standard.GetRect(spacer), Settings.Spacer, 0, 100f,
            false, $"{"Spacer".Translate()}: {"NLNT.Days".Translate(Settings.Spacer)}", null, null, 1);
        Settings.Ultra = (int)Widgets.HorizontalSlider(listing_Standard.GetRect(spacer), Settings.Ultra, 0, 100f,
            false, $"{"Ultra".Translate()}: {"NLNT.Days".Translate(Settings.Ultra)}", null, null, 1);
        Settings.Archotech = (int)Widgets.HorizontalSlider(listing_Standard.GetRect(spacer), Settings.Archotech, 0,
            100f, false, $"{"Archotech".Translate()}: {"NLNT.Days".Translate(Settings.Archotech)}", null, null, 1);

        listing_Standard.Gap();
        if (listing_Standard.ButtonText("Reset".Translate()))
        {
            Settings.StartValue = 0;
            Settings.Neolithic = 30;
            Settings.Medieval = 25;
            Settings.Industrial = 15;
            Settings.Spacer = 10;
            Settings.Ultra = 5;
            Settings.Archotech = 1;
            listing_Standard.Gap();
        }

        listing_Standard.Gap();
        listing_Standard.Label("NLNT.StartValue.Label".Translate());
        Settings.StartValue = Widgets.HorizontalSlider(listing_Standard.GetRect(spacer), Settings.StartValue, 0, 1f,
            false, "NLNT.StartValue".Translate(Math.Round(Settings.StartValue * 100)));

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("NLNT.version.label".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        Settings.Write();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        updateTechLevels();
    }
}