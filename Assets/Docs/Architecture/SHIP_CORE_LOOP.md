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

Our first version should remain small. We can later replace or expand individual systems without rebuilding the entire game.

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