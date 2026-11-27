# Decision log
### - Dor && Eilon -

A few decisions we made while putting together the first version of **All Aboard!**. We're keeping this log short and adding to it as we actually make choices.

## 2026-11-27 — Shared Ship Health and Hull-Based HUD

**Options:** Display maximum HP only / use a conventional health bar / use the equipped ship hull as a dynamic health indicator.

**Chose:** Separate ship health logic from its visual representation. Track current and maximum HP through ShipHealth and display both through a reusable HealthBox prefab.

The hull meter uses two overlapping UI Images:

- Hull background: represents the damaged or empty portion.
- Hull fill: represents remaining health.
- TextMeshPro label: displays CurrentHP/MaxHP.

The fill ratio is calculated as:

`NormalizedHP = CurrentHP / MaxHP`

**Why:** This creates a health indicator that fits the game's visual identity while keeping health logic independent from artwork and UI.

**Debug controls:** F6 for damage, F7 for repair and F8 for restoring full health. Developer controls are restricted to Editor and Development Builds.

**Future expansion:** Dynamically bind the HUD to the equipped hull sprite, support interchangeable hulls, and introduce damage animations, flooding, wreckage and combat feedback.

**Validation:** Health changes, debug controls and hull filling were tested locally in Unity. Additional hull variants and Android regression testing remain pending.

**AI:** ChatGPT assisted with architecture, implementation guidance, debugging and UI configuration.

**Who:** Dor.

## 2026-11-27 — Responsive UI and Encounter Mode Protection

**Options:** Independently position UI elements / organize UI into responsive containers with reusable components.

**Chose:** Organize the Level UI into RectTransform-based containers: TopBar, ButtonBar and PopUps, under SafeAreaPanel.

Reuse CurrencyBox and HealthBox prefabs across screens.

The encounter mode UI dynamically changes its text, color and icon between Friendly and Aggressive. The interaction button reflects the selected action.

Lock the encounter mode during resolution to prevent changing the selected action after committing to an encounter.

**Why:** Responsive containers improve maintainability and screen alignment. Encounter locking prevents inconsistent gameplay states.

**Future expansion:** Navigation back stack, reusable screen management, customization panels and device-specific layout validation.

**AI:** ChatGPT assisted with UI architecture, Inspector configuration, event handling and debugging.

**Who:** Dor.

## 2026-11-27 — Responsive UI and Reusable HUD Components

**Options:** Use independent UI elements with manually adjusted positions in each scene / organize the UI into responsive RectTransform containers and reusable prefabs.

**Chose:** Organize the gameplay UI into three main RectTransform containers under SafeAreaPanel:

- `TopBar` — currency, health and navigation.
- `ButtonBar` — steering and interaction controls.
- `PopUps` — encounter and result panels.

Reuse `CurrencyBox.prefab` and `HealthBox.prefab` across screens rather than duplicating their implementations.

**Why:** Responsive containers simplify alignment across screen sizes, while shared prefabs maintain consistent appearance and behavior across Home, Level and future Shipyard screens.

**Trade-off:** Anchors, pivots, parent dimensions and prefab overrides must be managed carefully. Visual animations should affect child objects rather than layout containers.

**AI:** ChatGPT assisted with RectTransform troubleshooting, prefab architecture and UI alignment. Dor implemented and tested the changes in Unity.

**Who:** Dor.

## 2026-11-27 — Dynamic Encounter Mode and Health Indicators

**Options:** Static text and colored checkmarks / dynamic labels, icons and colors driven by the current interaction state.

**Chose:** Use `EncounterModeUI` to synchronize the Friendly/Aggressive toggle with its visual representation. Friendly uses a handshake icon and green background; Aggressive uses crossed swords and a red background.

Use transparent white UI sprites so their colors can be controlled through Unity without generating separate textures for every color.

Replace the textual HP prefix with a red heart icon and numeric health value inside the reusable HealthBox.

**Why:** Icons, text and colors communicate the selected state clearly and provide consistent visual feedback.

**Current limitation:** Health currently represents upgrade-derived maximum HP. Actual combat damage, current run health, hull-specific health visualization and impact effects are not implemented yet.

**AI:** ChatGPT assisted with icon conversion, UI scripting, alignment and debugging. Dor integrated and tested the components in Unity.

**Who:** Dor.

## 2026-11-25 — Shared Economy and Persistent Ship Upgrades

**Options:** Maintain separate economy configurations for gameplay and upgrades / hardcode economy values in gameplay scripts / use one shared `EconomyConfig` through `IConfigService`.

**Chose:** Use a single `EconomyConfig` ScriptableObject, accessed through `Services.Config.Economy`, with values defined according to our agreed economy sheet.

- Stage completion: 20 coins.
- Successful ship plunder: 5 additional coins.
- Befriending a ship alone: no coin reward.
- Failed encounters: no reward.
- Base upgrade cost: 150 coins.
- Upgrade growth multiplier: 1.5.
- Base ship HP: 100.
- HP per upgrade: 10.
- Maximum upgrade level: 10.

Upgrade prices follow the exponential curve:

`Cost(level) = RoundToInt(150 × 1.5^level)`

Maximum ship health follows:

`MaxHP(level) = 100 + 10 × level`

Both coins and upgrade levels are persisted through `Services.Save`. The Home and Level scenes display the same saved values.

**Why:** Keeping economy values in one configuration prevents inconsistencies between gameplay, rewards and upgrades. The exponential price curve makes early upgrades accessible while providing longer-term progression. Separating stage completion rewards from plunder rewards allows us to expand the encounter system without redesigning the economy.

**Current limitations:** Encounters currently use fixed success probabilities of 70% for Friendly and 45% for Aggressive. These are configurable but do not yet depend on ship strength, HP or upgrades. HP upgrades currently increase maximum health; actual combat damage is not implemented.

**Future expansion:** Friendly encounters may recruit allied ships that assist in combat or escort missions. A candidate dynamic combat probability model is:

`P(win) = PlayerFleetStrength / (PlayerFleetStrength + EnemyFleetStrength)`

This model assumes positive combined fleet strength and may incorporate HP, cannons, upgrades and allied ships. It requires further balancing and testing before implementation.

**AI:** Claude assisted Eilon with the economy configuration, Home UI and HP upgrade implementation. ChatGPT assisted Dor with merge-conflict resolution, gameplay integration, reward calculations, reusable UI components and debugging. We selected the shared economy rules and manually integrated and tested the systems in Unity.

**Who:** Dor and Eilon.

## 2026-11-25 — Reuse shared UI through prefabs

**Options:** Maintain separate copies of UI elements in every scene / use reusable Unity Prefabs.

**Chose:** Convert the `CurrencyBox` into a shared Prefab used by both Home and Level. Organize the Level UI into TopBar, ButtonBar and PopUps containers.

**Why:** Shared prefabs reduce duplicated UI components, maintain consistent visual design and simplify future changes. Both scenes read the saved currency balance through `CoinCounter`, and the encounter system refreshes the Level HUD immediately after awarding coins.

**Trade-off:** Scene-specific references and prefab overrides must be managed carefully. In the future, an event-driven currency system could remove the direct dependency between encounter logic and the HUD.

**AI:** Helped explain prefab reuse, identify incorrect Inspector references, and troubleshoot live currency updates and popup alignment.

**Who:** Dor.

## 2026-11-18 — Save through an interface, not PlayerPrefs

**Options:** Call `PlayerPrefs` directly from each screen / put saving behind an `ISaveService` interface

**Chose:** Every script saves through **`Services.Save`** (`ISaveService`). Today the only implementation is `PlayerPrefsSaveService`; screens like `CoinCounter` and the sound toggle never touch `PlayerPrefs`.

**Why:** All saving goes through one place, so every save key lives in one class. The `"Coins"` vs `"coins"` bug happened because a screen wrote its own key. If we ever change how we save (a JSON file instead of PlayerPrefs, for example), we write one new class and change one line in Bootstrap, and no screen has to change. It also let us build `SettingsService` on top of Save without touching how saving works.

**AI:** Explained the pattern from the lesson and found the `"Coins"`/`"coins"` key bug in CoinCounter. We chose to keep every screen behind `Services`.

**Who:** Dor and Eilon.

## 2026-11-17 — Minimum Android API 29 (Android 10)

**Options:** API 26 (Android 8) / API 29 (Android 10) / higher

**Chose:** Set the minimum API level to **29 (Android 10)**, and lock the game to **Portrait**.

**Why:** Only very old phones are left out, and the casual players we're aiming for mostly use newer ones. From Android 9 the system reports the notch (DisplayCutout), so our SafeAreaFitter behaves the same on every phone that can install the game. Supporting fewer old Android versions also means less to test and no very weak devices running our effects. The trade-off: players still on Android 8–9 can't install the game.

**AI:** Explained what the minimum API controls and the trade-off. We made the choice and changed it in Player Settings.

**Who:** Dor and Eilon.

## 2026-11-17 — Android package name: com.eilondor.allaboard

**Options:** Keep Unity's default (com.DefaultCompany…) / com.eilondor.allaboard / a name tied to a domain we don't own

**Chose:** **com.eilondor.allaboard**: our team name plus the game name, all lowercase.

**Why:** The package name is the game's permanent ID on Google Play. Once the game is published it can't be changed, so we picked it now instead of shipping Unity's default. Using our names keeps it unique without needing a website domain.

**AI:** Explained what the package name is used for and why it can't change after publishing. We picked the name.

**Who:** Dor and Eilon.

## 2026-11-11 — Put the controls where they feel natural

**Options:** Spread the controls evenly around the screen / group them around how someone actually holds a phone

**Chose:** Design the Level screen around one-thumb play, starting with a right-handed layout:

- **Steering wheel:** Bottom-center, so it's easy to grab and turn without covering the ship.
- **Friendly/Aggressive toggle:** Lower-right, close to the thumb for quick decisions.
- **Back button:** Top-right, easy to find and separate from the controls used during play.

**Why:** We want steering and changing modes to feel quick, not like the player has to hunt for buttons. It's our first pass at the *rule of thumb* 😄, not the final layout. We still need to test actual reach on different phone sizes and think about a mirrored layout for left-handed players.

**AI:** Helped us troubleshoot the phone layout. We chose the control positions while testing the scene.

**Who:** Dor (current Level layout).

## 2026-11-11 — Use simple Unity UI to show the gameplay idea

**Options:** Build custom controls immediately / use Unity's existing UI components first

**Chose:** Use a regular **Toggle** with red and green images for Friendly/Aggressive mode, and a **UI Image** for the ship's wheel. We also kept a simple ship shape in the Level scene.

**Why:** We can already show what the player is supposed to do without getting stuck making art or a complicated control system. The toggle visibly switches states; the wheel is placed in the UI, but it doesn't steer the ship yet. We'll add proper steering and ship behavior after the assignment.

**AI:** Helped us understand the UI components and discussed a possible wheel-drag script. We kept this version simple on purpose.

**Who:** Dor (Level controls).

## 2026-11-11 — Keep the HUD basic, but leave room to grow

**Options:** Put every planned indicator on screen now / begin with only the essentials

**Chose:** Start with **HP, Score, and Coins** as simple HUD elements. We'll decide later where to put the compass, minimap, speed, wind, and other information.

**Why:** We want players to see the most important information without the top of the screen becoming crowded. Right now these are placeholders, not a finished health, scoring, or currency system. We can adjust the HUD once the sailing mechanics are real.

**AI:** Helped with UI layout, screen scaling, and safe-area placement.

**Who:** Dor (Level HUD); Eilon worked on the Home-screen foundation.

## 2026-11-11 — Get the two screens working on a real phone first

**Options:** Start with ship physics and combat / build the Home and Level screens as a greybox first

**Chose:** Finish **Home → Level → Home** navigation using `ScreenFlow`, keep the graphics simple, and test the layout in Unity's Device Simulator and on Android.

**Why:** It gives us a working starting point we can build on. We can check the phone layout and basic navigation before spending time on movement, encounters, and upgrades.

**AI:** Helped us troubleshoot scene navigation, Canvas scaling, and Android setup. We built and tested the scenes in Unity.

**Who:** Dor and Eilon.

## 2026-10-31 — Build the game around steering and quick choices

**Options:** A more involved sailing game / a short mobile game with simple steering and encounters

**Chose:** A portrait, one-thumb ship game where the player will steer, meet other ships, and choose between attacking and befriending them. Rewards and ship upgrades are part of the plan.

**Why:** We liked the idea of short 1–3 minute runs that are easy to start but still give players something to work toward.

**AI:** Helped us brainstorm and refine the pitch. The game idea came from our team discussions.

**Who:** Dor and Eilon.

---

## Still to decide (not implemented yet)

- **Steering:** How much the wheel can rotate, how it returns to center, and how it affects the ship.
- **Left-handed support:** Whether to mirror controls or offer customizable positions.
- **HUD:** Where the compass, minimap, timer, speed, and wind indicators should go without blocking gameplay.
- **Sea and wind:** How to create convincing movement and ship wakes while keeping Android performance smooth.
- **Game systems:** How HP, scores, coins, encounters, bonuses, and ship upgrades will work together.

We'll add new dated entries when we make these decisions, rather than treating the plans as finished features.
