# All Aboard! — Core Ship Gameplay Plan

**Branch:** `feature/core-ship-controls`

**Goal:** Make the first playable version of the ship, with steering, forward movement and a camera that follows naturally.

## Phase 1 — Steering input

Use our existing `IMG_ShipWheel_UI`.

- Drag the wheel clockwise or counterclockwise with a finger or mouse.
- Convert wheel rotation into a steering value from -1 (left) to +1 (right).
- Give the wheel configurable sensitivity and rotation limits.
- Experiment with automatic return-to-center when released.
- Keep the wheel input separate from the ship's movement logic.

**Done when:** We can rotate the wheel smoothly on Android and see its steering value change.

## Phase 2 — Ship movement and inertia

Create a world-space PlayerShip using our existing ship sprite.

- The ship moves forward automatically at a configurable speed.
- Steering changes its heading gradually, rather than rotating instantly.
- Turning response and speed are adjustable in the Inspector.
- Releasing the wheel stops adding steering; the ship continues in its new direction.
- Keep the movement model simple and arcade-like initially.

**Done when:** The ship sails forward and turns naturally in response to the wheel.

## Phase 3 — Camera and moving world

Make the camera follow the ship instead of locking it perfectly to the center.

- Keep the ship close to the center of the usable gameplay area.
- Allow a small screen displacement during turns.
- Smoothly move the camera to catch up.
- Keep camera rotation fixed initially.
- Add temporary ocean markers to demonstrate real movement.
- Preserve stable world coordinates for future enemies, islands and minimaps.

**Done when:** The ship feels like it's sailing through the world, with the ocean and objects moving relative to it.

## Phase 4 — Gameplay feel and mobile testing

- Tune steering sensitivity, inertia and camera lag.
- Test quick turns, long drags and releasing the wheel.
- Check that the HUD and wheel don't block gameplay.
- Test different phone aspect ratios and safe areas.
- Confirm that Home and Back navigation still work.
- Check performance on Android.

**Done when:** The prototype feels responsive and consistent on a real phone.

---

## Proposed components

| Script | Responsibility |
|---|---|
| `ShipWheelInput.cs` | Read wheel drag and expose steering input |
| `ShipMotor2D.cs` | Handle ship velocity and turning |
| `FollowCamera2D.cs` | Follow ship with damping and screen offset |
| `ShipDebugHUD.cs` | Show steering, heading, speed and camera offset during testing |
| `FrameRateSettings.cs` (planned) | FPS preferences and refresh-rate diagnostics |
| `HapticsDeveloperPanel.cs` (planned) | Runtime adjustment of haptic presets |
| `HeadingCompassUI.cs` (planned) | Display ship heading as a scrolling top compass |

Our first version should remain small. We can later replace or expand individual systems without rebuilding the entire game.

## Phase 5 — High refresh rate and performance (planned)

**Goal:** Keep gameplay responsive across 30/60/90/120 Hz screens, particularly 120 Hz Android phones, without changing gameplay speed.

- Plan an FPS mode: **Auto / 30 / 60 / 120**, showing only practical choices for the device when possible. 90 FPS can be offered on displays supporting it.
- Use `Application.targetFrameRate` on mobile. Measure the **actual** FPS and display refresh rate; Android may cap performance based on power, thermal and frame-pacing policies.
- Avoid tying input, movement, animation or haptic cooldowns to a frame count. Use time-based calculations; use a consistent fixed physics step and interpolation for the future `Rigidbody2D` ship.
- Maintain touch responsiveness, a stable camera, and smooth wheel animation at both 60 FPS and 120 FPS.
- Profile on a physical phone: average FPS, slow frames / frame time, latency, memory allocations, battery heat and throttling over multiple minutes.
- Test Unity Android **Optimized Frame Pacing** and document the device-specific results.

**Done when:** Selecting an FPS preference works on Android and the same steering input leads to consistent movement at different FPS rates. 120 FPS is an option, not a requirement.

## Phase 6 — Ship Haptics + developer tuning panel (planned)

**Current implementation:** `ShipWheelInput`, `ShipWheelHaptics`, and `ShipHapticsEngine` exist, and Unity Editor logs confirm wheel-tick and end-stop events. Physical vibration still requires testing on the phone.

- **Player-facing setting:** a simple Haptics On/Off accessibility toggle.
- **Developer-only mode:** tune the wheel click preset, click spacing, cooldown, end-stop preset and threshold, direction-change feedback, and master switch **inside the running game**.
- Provide `Test Tick`, `Test End Stop`, and `Reset Defaults` controls. Show feedback-event counts and current settings.
- Keep the panel hidden in normal release gameplay; development builds are the simplest initial option.
- Preserve the current wheel baseline: **1080° / sensitivity 1 / auto-center on / return 480°/s**.
- Preserve current haptic test baseline: **35° per tick / 0.09s cooldown / 3° end tolerance / 40° reset distance / direction feedback off**.
- Android `performHapticFeedback` named patterns do **not** guarantee a specific vibration strength. Advanced amplitude/waveforms require separately evaluating `VibrationEffect` and hardware/permission compatibility.
- Keep controls fully usable even on phones without haptics or when vibration is disabled. Avoid feedback on automatic centering.

**Done when:** Haptics can be tested and adjusted on a real phone without recompiling for every small change, without creating log spam or constant buzzing.

## Phase 7 — Dynamic ship-heading compass (planned)

**Visual direction:** a slim horizontal bearing tape at the top, inspired by COD / Battlefield:

`| · · W · · | · · N · · | · · E · · |`

- The compass follows the **ship's actual world heading**, not the steering wheel's visual angle.
- Use N / E / S / W, optionally NE / SE / SW / NW, degree ticks, and a fixed center reference marker. Scroll bearing labels underneath it.
- Interpolate changes smoothly and correctly handle 359° → 0° wrapping.
- The first camera remains **north-up**, but compass meaning should remain consistent if a rotating camera is tested later.
- Keep the strip readable within the Safe Area without colliding with Back, HP, Score or Coins. Test the current portrait phone layouts.
- Future additions: wind bearing, objectives and enemy markers; the minimap later derives from the same world coordinates and heading information.

**Done when:** Turning the ship updates the compass correctly and smoothly, including at 0/360°.

## Updated implementation order

1. **Android haptic validation:** test physical clicks/end-stop; keep Editor logging off during performance profiling.
2. **ShipMotor2D:** turn the ship with wheel steering and introduce forward movement and controlled inertia.
3. **FollowCamera2D:** near-centered ship with limited camera lag and world-space scrolling.
4. **FPS diagnostics and options:** compare sustained 60/120 FPS on Android; assess frame pacing.
5. **Developer haptics panel:** enable in-build tuning and reset-to-default.
6. **Heading compass:** use ship world rotation and safe-area layout.
7. Wind, water, minimap and upgrades after the control loop feels correct.

**Documentation rule:** This file records plans and experiments. Put only **confirmed team choices** in the root `DECISIONS.md`; don't call proposed features implemented.

---

## Future systems — not part of this first implementation

- **Wind:** Direction, strength, sail efficiency and drift.
- **Water:** Animated sea, currents, resistance, ship wakes and splashes.
- **Ship upgrades:** Hull, sails, cannons, anchor and improvements to movement stats.
- **Combat:** Automatic attacks, Friendly/Aggressive behavior and encounters.
- **HUD:** Compass, minimap, speed, health, coins, score and timer.
- **Accessibility:** Left-handed layout, adjustable controls and readable indicators.

## Things we still need to test

- How much should the wheel rotate?
- Should the steering automatically return to neutral?
- How much delay feels good for the ship versus the camera?
- Should the ship stay exactly centered or slightly above center to leave space for the wheel?
- Should the camera always face north or optionally rotate with the ship?

These are experiments, not final design decisions.
