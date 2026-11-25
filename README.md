# All Aboard! 🚢

**Team:** Eilon Grinberg & Dor Mandel  
**Course:** Mobile Game Development, Shenkar  
**Engine:** Unity 6000.6.7f1 · **Platform:** Android, Portrait, API 29+, IL2CPP/ARM64

## The game

A one-thumb, hybrid-casual sailing game. Players will steer a ship, choose whether to befriend or attack passing vessels, and use rewards to upgrade their ship across short 1–3 minute runs.
The current build features a playable sailing and encounter loop, Friendly/Aggressive interactions, persistent coin rewards, HP upgrades, reusable UI prefabs and a shared economy configuration. Combat, advanced sailing physics and procedural world generation are planned for future milestones.

## Mechanics Lab — Research & Prototyping

All Aboard! has a separate web-based Mechanics Lab for experimenting with gameplay systems before integrating them into Unity.

The lab supports our iterative development process through interactive prototypes, configurable parameters and visual demonstrations.

- [All Aboard! — Mechanics Lab](https://dormandel.github.io/AllAboard/preview/v0.11)
- [Mechanics Lab source repository](https://github.com/DorManDel/AllAboard)

Experimental systems include ship steering, sailing, wind interaction, encounters, health, modular ship components and ocean rendering.

The Mechanics Lab is a separate experimental environment. Not all demonstrated mechanics are implemented in the current Unity build.

## Milestone 4 — Playable Loop and Progression

- Playable sailing with a temporary encounter system.
- Friendly and Aggressive interactions with configurable success probabilities.
- Stage completion and plunder rewards read from `Services.Config.Economy`.
- Persistent HP upgrades with an exponential price curve.
- Shared CurrencyBox prefab and live currency updates.
- Synchronized HP and currency displays between Home and Level.
- Encounter and result popups using the shared visual style.

**Documentation:** [Decisions](DECISIONS.md) · [Credits](CREDITS.md)

**Integration:** [Milestone 4 gameplay PR #6](https://github.com/Eilon343/AllAboard/pull/6).

## Milestone 4 — Economy

**[Economy sheet](https://docs.google.com/spreadsheets/d/1MK3pVl9tQjG8-5nP4lOEgUonI_AhnZWor8q7vUXKwgg/edit?usp=sharing)**: one currency (coins). Per session (~3 runs): 320 coins in (stages cleared, daily gift, plundered ships), 50 out (Continue), 270 left for upgrades. Upgrade price = 150 × 1.5^(upgrade − 1). Both checks: **Yes**.

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

[Game pitch](pitch.md) · [Decisions](DECISIONS.md) · [Economy sheet](https://docs.google.com/spreadsheets/d/1MK3pVl9tQjG8-5nP4lOEgUonI_AhnZWor8q7vUXKwgg/edit?usp=sharing) · [Agent guidelines](AGENTS.md) · [Milestone 3 branch](https://github.com/Eilon343/AllAboard/tree/milestone-3)