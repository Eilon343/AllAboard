# All Aboard! 🚢

**Team:** Eilon Grinberg & Dor Mandel  
**Course:** Mobile Game Development, Shenkar  
**Engine:** Unity 6000.6.7f1 · **Platform:** Android, Portrait, API 29+, IL2CPP/ARM64

## The game

A one-thumb, hybrid-casual sailing game. Players will steer a ship, choose whether to befriend or attack passing vessels, and use rewards to upgrade their ship across short 1–3 minute runs. The current build is a mobile greybox; full sailing and encounter mechanics are planned.

## Milestone 3 — Built to change

![All Aboard! architecture — scenes, services and initialization order](Assets/Docs/Images/architecture.png)

- **Bootstrap:** The first scene initializes `Services.Save`, then `Services.Settings`, before opening `Home`.
- **Scenes:** `Home` and `Level` navigate through `ScreenFlow`.
- **Save service:** `ISaveService` + `PlayerPrefsSaveService` persist coins; screens access storage through `Services.Save`.
- **Settings service:** `ISettingsService` + `SettingsService` persist `SoundOn` through the save interface. A reusable `SoundToggleUI` prefab displays mute/unmute sprites.
- **Assemblies:** `Game.asmdef` references `Services.asmdef`; Services never references Game. Screens never call `PlayerPrefs` directly.
- **Documentation:** `DECISIONS.md` records design choices; Eilon is completing `AGENTS.md` for the final architecture.

**Second-service PR:** Add the link here after the PR is reviewed and merged.

## Run the project

Open in **Unity 6000.6.7f1**, choose the **Android** Build Profile, and verify the enabled Scene List is `Bootstrap → Home → Level`. Run from Bootstrap or build to an Android device.

## Milestone 2 — Mobile greybox

Built phone-oriented `Home` and `Level` screens, including the HUD, ship placeholder, steering-wheel UI and Friendly/Aggressive toggle. Applied lecturer feedback to Android orientation/API, Level Canvas scaling, Back button size and Safe Area handling.

**Evidence:** [Phone recording](Assets/Docs/Videos/VID-20261111-WA0008.mp4) · [Simulator screenshot 1](Assets/Docs/Images/SimulatorScreenshot1.png) · [Simulator screenshot 2](Assets/Docs/Images/SimulatorScreenshot2.png)

## Links

[Game pitch](pitch.md) · [Decisions](DECISIONS.md) · [Agent guidelines](AGENTS.md) · [Milestone 3 branch](https://github.com/Eilon343/AllAboard/tree/milestone-3)