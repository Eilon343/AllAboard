# All Aboard! — Ship Haptics v1

## Goal

Make the wheel *feel* mechanical on a real Android phone without changing the steering logic .

- **Steering tick:** subtle feedback every ~35° while dragging (maximum about 11 ticks/second).
- **Hard stop:** a distinct event when the wheel reaches ±1080°, once per visit to the limit.
- **Direction change:** optional; off by default to avoid buzzing from small finger movements.
- **Return to center:** intentionally silent.

These are experiments, not final decisions. 

--- **(NEED: Test them on the phone.)** ---

## Files

| File | Responsibility |
| --- | --- |
| `ShipWheelInput.cs` | The existing wheel behavior, with three new read-only properties: `CurrentAngle`, `MaxAngle`, `IsDragging`. |
| `ShipWheelHaptics.cs` | Looks at wheel motion and determines *when* to play a tick or hard stop. |
| `ShipHapticsEngine.cs` | Requests *which* Android haptic pattern to play. No ship movement here. |

---

Connect `ShipWheelHaptics` references in Inspector:
- `Wheel Input` → your `IMG_ShipWheel_UI` (ShipWheelInput component)
- `Haptics` → `HapticsSystem` (ShipHapticsEngine component)

Suggested first values: degreesPerTick=35, tickCooldown=0.09, endTolerance=3, endResetDistance=40.

---

## Why separate the engine?

When we implement cannon fire, collisions, mode switching and upgrades, they can all call named functions on `ShipHapticsEngine` without knowing about Android, and the settings toggle can call `SetHapticsEnabled(bool)`.

---

## Android platform notes

Uses Unity 6's `AndroidApplication.currentActivity` and Android `View.performHapticFeedback()`.
- Does not need the `android.permission.VIBRATE` permission.
- Respects system touch-feedback settings and phone hardware.
- `CLOCK_TICK`, `KEYBOARD_TAP`, `LONG_PRESS` are **named patterns**, not exact intensity values. Actual results differ across phones.
- Not every phone has the same vibration motor; physical left/right spatial vibration cannot be guaranteed on a single phone.
- A Unity Editor Play run is silent. Enable `Log Events In Editor` on the engine to see intended events in Console.
- This proof of concept makes an AndroidJavaRunnable for each triggered event, so throttling matters. If performance profiling shows a problem, optimize the bridge later.

---
## Inspector knobs

**ShipHapticsEngine**
- `Enable Haptics` — master switch for eventual Accessibility settings.
- `Wheel Tick` — try ClockTick (default) or KeyboardTap if ClockTick feels too weak.
- `Direction Change` — KeyboardTap by default.
- `Wheel End Stop` — LongPress by default; VirtualKey can feel sharper on some phones.
- `Log Events In Editor` — logs the planned effects when testing without a phone.

**ShipWheelHaptics**
- `Degrees Per Tick` — smaller = more ticks.
- `Tick Cooldown` — minimum seconds between ticks.
- `End Tolerance` — near which ±1080° stops count as the end.
- `End Reset Distance` — prevents repeated hard-stop vibration while still holding the wheel at its limit.
- `Vibrate On Direction Change` — experimental; start disabled.

---

## Test on Samsung/Android

1. In Unity Play Mode, verify the wheel still turns and auto-centers as before.
2. Build And Run to your Android phone (Editor cannot vibrate the phone).
3. Drag slowly clockwise: feel discrete small clicks.
4. Turn rapidly: make sure it never feels like continuous buzzing.
5. Reach ±1080°: feel **one** stronger end-stop event, not dozens.
6. Stay at the edge and drag: there should be no repeated end-stop buzz.
7. Release: no haptics while the wheel returns to zero.
8. Change `Wheel Tick` or `Wheel End Stop` preset if the phone feels too soft/strong.
9. Turn off the phone's system touch feedback or `Enable Haptics` and confirm the app respects that.

---

## Code-level notes

`[SerializeField] private` exposes tuning in the Inspector without making the field publicly writable.

`CurrentAngle` is the **accumulated** rotation in degrees, so it can reach 1080 without losing revolutions. Reading `transform.eulerAngles.z` instead would lose full rotations, because Euler angles wrap around 360°.

`LateUpdate()` only *observes* the wheel and requests feedback. It never changes steering.

`Time.unscaledTime` measures cooldowns independently of game timeScale.

The haptics engine dispatches a native call on Android's UI thread; it does not request raw vibration duration/amplitude. More advanced amplitude/waveform control would require Android VibrationEffect and additional setup later.
