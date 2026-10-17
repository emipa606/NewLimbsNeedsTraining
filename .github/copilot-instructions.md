# GitHub Copilot Instructions for New Limbs Needs Training Mod Development

## Mod Overview and Purpose

**Mod Name**: New Limbs Needs Training  
**Author**: Mlie  
**Description**:  
The "New Limbs Needs Training" mod introduces a realistic recovery mechanic for pawns receiving limb transplants in RimWorld. Upon transplanting a new limb, the affected pawn will experience reduced limb efficiency, which gradually improves over time until it reaches the maximum value. This simulates the adjustment period required for the pawn to get accustomed to the new limb.

## Key Features and Systems

- **Recovery Mechanism**: The mod calculates a recovery time based on the technology level of the transplanted limb. The efficiency of the limb starts low after surgery and improves as the pawn gets accustomed to using it.
- **Settings Customization**: Players can modify recovery times and starting efficiency values in the mod settings, tailoring the experience to their preference.
- **Exclusion Criteria**: The mod does not affect internal implants, nor does it impact any limbs associated with critical biological functions such as blood pumping, breathing, or mental faculties.
- **Compatibility and Localization**: The mod is designed to work with other mods that introduce new limbs, such as "EPOE," and includes translations for French, German, and Portuguese.

## Coding Patterns and Conventions

- **C# Practices**:
  - Follow consistent naming conventions using PascalCase for class names and camelCase for method variables.
  - When writing methods, maintain a clear structure: initialization, execution, and cleanup sections.
  - Handle exceptions gracefully to ensure the mod does not cause game-breaking errors.

- **Code Organization**:
  - Each C# file is focused on a specific task or function within the mod to ensure single responsibility and clarity.

## XML Integration

Although the current version of "New Limbs Needs Training" does not use XML files directly, understanding XML is crucial if future expansions necessitate definition files for custom content.

## Harmony Patching

The mod employs Harmony for safely modifying and extending game behavior without altering the original code:

- **Targeted Patches**: Use the `Postfix` patches to augment existing methods that handle pawn limb efficiency calculations (`HediffWithComps_CalculatePartEfficiency`).
- **Harmony Annotations**: Utilize attributes like `[HarmonyPatch]` to specify target methods precisely, ensuring compatibility and stability.
- **Selective Modifications**: Carefully construct patches to modify only the necessary components, reducing overhead and avoiding unintended side effects on the base game or other mods.

## Suggestions for GitHub Copilot

- **Code Completion**: Use Copilot to suggest boilerplate code for initializing mod settings and configuration windows. This includes UI element creation and typical C# constructs for handling settings persistence.
- **Type Inference**: Allow Copilot to assist in predicting types for method parameters and local variables, particularly when working with the game's complex class hierarchy.
- **Error Handling**: Leverage Copilot to propose standardized error handling patterns for Harmony patching, ensuring robust and fail-safe operations.
- **Best Practices**: Encourage Copilot to suggest best practices in C# development, especially when dealing with asynchronous operations or interacting with Unity's game loop.

By adhering to these guidelines, Copilot can become an invaluable tool in maintaining and enhancing the quality and functionality of the "New Limbs Needs Training" mod. 


This set of instructions aims to provide developers working on the mod with a comprehensive understanding of its features, coding standards, and the role of GitHub Copilot in assisting the development process.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

