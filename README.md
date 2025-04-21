# VisibilityFix Mod for Schedule I

**Author:** volcomtx  
**Game:** Schedule I  
**Framework:** [MelonLoader](https://melonwiki.xyz/#/?id=what-is-melonloader) (IL2CPP compatible)  
**Version:** 1.0.0  

---

## 📌 Description

This mod overhauls the way **player visibility** is calculated in *Schedule I*, offering more precise control and improving stealth mechanics.

It is fully compatible with **IL2CPP builds** of the game.

---

## ⚙️ Features

- ✅ Customizable **base visibility**
- ✅ Adjustable multipliers for:
  - Sneaky movement
  - Crouched posture
  - Flashlight use
- ✅ Toggle to **enable or disable flashlight effects**
- ✅ Optional visibility **clamp ceiling**
- ✅ Built-in debug logging to inspect visibility calculation in real-time
- ✅ Config file is auto-generated on first launch

---

## 🔧 Configuration

When the game is run with the mod for the first time, a config file will be createdin the games `Mods` folder.

You can modify the following settings:

### 🔹 Top-Level Options

| Setting                  | Type    | Description                                                  |
|--------------------------|---------|--------------------------------------------------------------|
| `EnableDebugLogs`        | bool    | Enables console output for debugging visibility calculations |
| `FlashlightAffectsSneak`| bool    | Controls whether flashlight use affects visibility           |
| `BaseVisibility`         | float   | The base visibility value before modifiers are applied       |
| `MaxVisibility`          | float   | The max allowed visibility (after modifiers are applied)     |

### 🔹 Multipliers Section

|  Sub-Setting | Description |
|-------------|---------------------|
Sneaky | Multiplier applied when moving sneaky
Crouched | Multiplier applied when crouched
Flashlight | Multiplier applied when the flashlight is enabled

## 🧪 Debugging

To help tune the config, enable `EnableDebugLogs`. This will output:

- The list of raw visibility attributes  
- The filtered list used in calculations  
- Each step of the visibility math  
- Final visibility result after clamping

---

## 📦 Installation

1. Install [MelonLoader](https://melonwiki.xyz/#/?id=installation) for **IL2CPP**.
2. Drop `VisibilityFix.dll` into the game's `Mods` folder
3. Run the game. Edit the generated config file if desired.
4. Optionally view the console log or `MelonLoader/Latest.log` for debug output.
