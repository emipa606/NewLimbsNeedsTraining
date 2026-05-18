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
    private static readonly Dictionary<TechLevel, int> daysUntilRecovery = new();

    private static string currentVersion;

    public static NewLimbsNeedsTrainingMod Instance;


    /// <summary>
    ///     Cunstructor
    /// </summary>
    /// <param name="content"></param>
    public NewLimbsNeedsTrainingMod(ModContentPack content) : base(content)
    {
        Instance = this;
        updateTechLevels();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    public NewLimbsNeedsTrainingSettings Settings
    {
        get
        {
            field ??= GetSettings<NewLimbsNeedsTrainingSettings>();

            return field;
        }
    }

    private void updateTechLevels()
    {
        foreach (var techLevel in (TechLevel[])Enum.GetValues(typeof(TechLevel)))
        {
            daysUntilRecovery[techLevel] = techLevel switch
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
        var ticksUntilDone = GenDate.TicksPerDay * daysUntilRecovery[TechLevel.Industrial];
        if (part.def.spawnThingOnRemoved == null ||
            !daysUntilRecovery.TryGetValue(part.def.spawnThingOnRemoved.techLevel, out var value))
        {
            return ticksUntilDone;
        }

        ticksUntilDone = GenDate.TicksPerDay * value;
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
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Gap();
        const int spacer = 30;
        listingStandard.Label("NLNT.DaysUntilRecoveryLabel".Translate(), -1,
            "NLNT.DaysUntilRecoveryToolTip".Translate());
        listingStandard.Gap();
        Settings.Neolithic = (int)Widgets.HorizontalSlider(listingStandard.GetRect(spacer), Settings.Neolithic,
            0,
            100f, false, $"{"Neolithic".Translate()}: {"NLNT.Days".Translate(Settings.Neolithic)}", null, null, 1);
        Settings.Medieval = (int)Widgets.HorizontalSlider(listingStandard.GetRect(spacer), Settings.Medieval,
            0,
            100f,
            false, $"{"Medieval".Translate()}: {"NLNT.Days".Translate(Settings.Medieval)}", null, null, 1);
        Settings.Industrial = (int)Widgets.HorizontalSlider(listingStandard.GetRect(spacer),
            Settings.Industrial,
            0,
            100f, false, $"{"Industrial".Translate()}: {"NLNT.Days".Translate(Settings.Industrial)}", null, null,
            1);
        Settings.Spacer = (int)Widgets.HorizontalSlider(listingStandard.GetRect(spacer), Settings.Spacer, 0,
            100f,
            false, $"{"Spacer".Translate()}: {"NLNT.Days".Translate(Settings.Spacer)}", null, null, 1);
        Settings.Ultra = (int)Widgets.HorizontalSlider(listingStandard.GetRect(spacer), Settings.Ultra, 0,
            100f,
            false, $"{"Ultra".Translate()}: {"NLNT.Days".Translate(Settings.Ultra)}", null, null, 1);
        Settings.Archotech = (int)Widgets.HorizontalSlider(listingStandard.GetRect(spacer), Settings.Archotech,
            0,
            100f, false, $"{"Archotech".Translate()}: {"NLNT.Days".Translate(Settings.Archotech)}", null, null, 1);

        listingStandard.Gap();
        if (listingStandard.ButtonText("Reset".Translate()))
        {
            Settings.StartValue = 0;
            Settings.Neolithic = 30;
            Settings.Medieval = 25;
            Settings.Industrial = 15;
            Settings.Spacer = 10;
            Settings.Ultra = 5;
            Settings.Archotech = 1;
            listingStandard.Gap();
        }

        listingStandard.Gap();
        listingStandard.Label("NLNT.StartValue.Label".Translate());
        Settings.StartValue = Widgets.HorizontalSlider(listingStandard.GetRect(spacer), Settings.StartValue, 0,
            1f,
            false, "NLNT.StartValue".Translate(Math.Round(Settings.StartValue * 100)));

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("NLNT.version.label".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        Settings.Write();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        updateTechLevels();
    }
}