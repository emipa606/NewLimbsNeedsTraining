# GitHub Copilot Instructions for RimWorld Mod: New Limbs Needs Training

## Mod Overview and Purpose

**Mod Name:** New Limbs Needs Training

This mod introduces a more realistic approach to bionic and prosthetic limb transplants in RimWorld. When a pawn receives a new limb, they need an adjustment period to become accustomed to it. During this time, the efficiency of the new limb starts lower and gradually increases to its maximum potential. This adds a strategic layer to managing pawns with new limbs, reflecting their journey toward full recovery and integration of the new body part.

## Key Features and Systems

- **Recovery Time:** The time taken for a pawn to adjust to a new limb depends on the technological sophistication of the limb. This can be customized through the mod settings.
- **Affected Limbs:** Only limbs that are externally visible are affected by this adjustment phenomenon. Implants and limbs crucial to vital functions like blood pumping, breathing, and cognition are excluded.
- **Configuration Settings:** Users can customize the starting efficiency and recovery time through mod settings to suit their gameplay preferences.
- **Compatibility:** The mod is compatible with EPOE (Expanded Prosthetics and Organ Engineering) and is expected to work with other limb-adding mods.
- **Special Cases:** The mod specifically ignores werewolf transformations from the "Rim of Madness - Werewolves" mod, ensuring smooth integration.
- **Translations:** Available in English, French, German, and Portuguese, thanks to community contributors.

## Coding Patterns and Conventions

- **Class Structure:** Classes are organized based on functionality and mod components, such as main mod class, settings, and efficiency calculations.
- **Access Modifiers:** Internal classes are used for mod settings and initialization to encapsulate mod-specific logic, while public classes handle broader functionality.
- **Method Naming:** Methods are named using camelCase to enhance readability and consistency with the C# language conventions.

## XML Integration

- **XML Files:** Utilize XML to define and manage mod settings in a way that integrates seamlessly with the RimWorld mod settings interface.
- **Data-Driven Approach:** Ensure XML data is used to drive configurable aspects, such as recovery time and efficiency settings, allowing players to customize their experience without modifying code.

## Harmony Patching

- **Harmony Library:** Utilize Harmony to apply runtime patches to RimWorld's methods, allowing for dynamic modifications that suit the mod's objectives.
- **Efficiency Calculation Patch:** The `HediffWithComps_CalculatePartEfficiency` class likely contains Harmony patches used to adjust the efficiency calculations for new limbs based on the elapsed recovery time and tech level.

## Suggestions for Copilot

- **Suggestion 1:** When implementing a new feature related to limb adjustment, consider how to use Harmony patches effectively to inject or modify behavior in existing RimWorld methods.
- **Suggestion 2:** For XML-related modifications, ensure data elements are properly structured to be easily parsed by RimWorld's settings system.
- **Suggestion 3:** Always test changes in multiple scenarios, such as different mods, to ensure compatibility beyond just EPOE and default settings.
- **Suggestion 4:** Assist developers by suggesting performance optimizations, such as caching tech level calculations if used repeatedly in efficiency computations.
- **Suggestion 5:** Recommend patterns for supporting additional languages as contributors provide translations.

This guide will help you make effective use of GitHub Copilot in enhancing and maintaining the **New Limbs Needs Training** mod. Happy modding!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
