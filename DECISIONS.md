# Decision log
### - Dor && Eilon -

A few decisions we made while putting together the first version of **All Aboard!**. We're keeping this log short and adding to it as we actually make choices.

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
