# AGENTS.md — context for AI coding tools

AI tools read this file before working on the repo. Keep it short, true and up to date.

## The game
All Aboard! is a hybrid-casual ship game for Android: portrait, one thumb, 1–3 minute runs.
The player will steer a ship, meet passing vessels and choose quickly between Friendly
(befriend) and Aggressive (plunder), then spend rewards on ship upgrades between runs.
Right now the repo is a mobile greybox (Home and Level screens) with a small services
architecture. Ship physics, encounters, combat, upgrades and full audio are planned, not built.

## Tech
- Unity 6000.6.7f1, C#, URP 2D
- Android, portrait only, minimum API 29, IL2CPP / ARM64, package `com.eilondor.allaboard`
- UI: uGUI + TextMeshPro, built for 1080×1920 (Canvas Scaler: Scale With Screen Size, Match 0.5)
- Input System, Device Simulator devices
- Unity Pipeline package (`com.unity.pipeline`): lets the Unity CLI and agents drive the Editor

## Architecture
- **Scenes:** Bootstrap (scene 0, no UI) → Home (scene 1) ⇄ Level (scene 2).
  Bootstrap is first in the Scene List and runs once; after that only Home and Level load.
- **Init order** in `Bootstrap.Awake()` (`Assets/Scripts/Game/Boostrap.cs`): `DontDestroyOnLoad`, then
  1. `Services.Save = new PlayerPrefsSaveService()`
  2. `Services.Config = new ScriptableObjectConfigService(economyConfig)` (the asset from Bootstrap's
     `economyConfig` field in the Bootstrap scene)
  3. `Services.Settings = new SettingsService(Services.Save)` (needs Save, so it comes after)
  4. load Home
- **Services** (`Assets/Scripts/Services`), each behind an interface:
  - `ISaveService` (GetInt / SetInt) → `PlayerPrefsSaveService`. The only class that touches PlayerPrefs.
  - `ISettingsService` (SoundOn) → `SettingsService`. Saves `SoundOn` through `ISaveService`
    (key `settings.soundOn`, on by default) and mutes or unmutes the game with `AudioListener.volume`.
  - `IConfigService` (Economy) → `ScriptableObjectConfigService`. Hands out `EconomyConfig`, a
    ScriptableObject with every economy number (rewards, prices, upgrade curve, `UpgradeCost(level)`).
    Values live in `Assets/Config/EconomyConfig.asset` and match our economy sheet (linked in the README).
  - `Services`: the static class where every script finds them.
- **Screens use `Services`, never `PlayerPrefs`.**
  - `CoinCounter` (Home) adds and shows coins through `Services.Save` (key `coins`).
  - `SoundToggleUI` (`Assets/Scripts/Game/UI`) on the reusable prefab
    `Assets/Prefabs/UI/SoundToggle.prefab` reads and sets `Services.Settings.SoundOn` and swaps the
    sound on/off sprite (`Assets/Sprites/UI/SoundToggle`).
- **Two assemblies:** `Game.asmdef` (`Assets/Scripts/Game`) references `Services`,
  `Unity.TextMeshPro` and `Unity.InputSystem`. `Services.asmdef` references nothing:
  Services never references Game.
- **Editor only** (`Assets/Editor/PlayFromBootstrap.cs`): Tools ▸ Play From Bootstrap makes Play
  start in Bootstrap, then return to the scene you were editing. Not in builds.
- **Screen fit:** each screen has `SafeAreaFitter` on its main panel only, never on the Canvas.
  Buttons are at least 140×140.
- Diagram: `Assets/Docs/Images/architecture.png`, shown in the README.

## Rules for AI
- Read AGENTS.md, README.md and DECISIONS.md before you start.
- Follow the existing folder structure and naming. Don't create new top-level folders
  (`Assets/Config` exists because the course asks for it).
- Economy numbers (rewards, prices, curves) live in `EconomyConfig`, never in scripts. Read them with
  `Services.Config.Economy`. A new number is a new field in `EconomyConfig`, filled in on the asset.
- Don't invent APIs. If unsure about an SDK or Unity API, say so and point to the docs.
- Don't change Player Settings, ProjectSettings, Build Profiles, the package name, API levels or
  assembly references unless asked, and say how to test the change.
- Keep changes small, explain each one, and change nothing you weren't asked to.
- Screens never call `PlayerPrefs`; save through `Services.Save`. Services never references Game.
- A new service: an interface and a class in `Assets/Scripts/Services`, a property in
  `Services.cs`, and one line in Bootstrap, after the services it needs.
- Never load the Bootstrap scene again after start.
- Treat planned systems as proposals, not existing code.
- Keep `.meta` files with their assets. Prefer driving the open Editor with the Unity CLI over
  hand-editing `.unity` / `.prefab` YAML.
- Don't mark real-device testing as done from Editor or Device Simulator results.
- Never commit secrets (keystores, passwords, API keys) or media we don't have the rights to.

## Conventions
- Code and comments in English.
- Game code in `Assets/Scripts/Game` (UI scripts in `Game/UI`), services in
  `Assets/Scripts/Services`, editor tools in `Assets/Editor`, prefabs in `Assets/Prefabs`.
- One MonoBehaviour per file, named after its class.
- UI objects: `BTN_` for buttons, `TGL_` for toggles.
- Save keys live in the class that owns them, as constants.
- Work on a branch (`feature/…`, `fix/…`) and merge through a pull request. A PR made with AI ends
  with "What the AI did / what I checked"; one written by hand ends with "What I wrote / how I tested it".
- Every real decision gets a `DECISIONS.md` entry.
- Milestones: feature branches merge through pull requests into `milestone-N`, the branch we submit.
  After the deadline it never changes.
