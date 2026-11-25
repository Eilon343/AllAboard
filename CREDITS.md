# All Aboard! - Credits

**Development team:** Eilon Grinberg and Dor Mandel  
**Course:** Mobile Game Development, Shenkar  
**Milestone:** 4 - A loop, an economy, and real art

This document records the provenance of the project artwork, audio, and fonts. AI assistance is identified separately from third-party assets. Attribution does not grant redistribution permission.

## Original project artwork

### Ship and modular components

Designed specifically for All Aboard! by Dor Mandel, based on his own sketches, concepts and visual direction, with ChatGPT assistance for asset creation and refinement. Dor selected, edited, exported and integrated the artwork in Unity.

- Ship hull, front and rear sails, four cannons and anchor: `Assets/Sprites/Ship/Ship_Parts/` (PNG and editable SVG files).
- Main ship sprite: `Assets/Sprites/Ship/IMG_SHIP.png`.
- App icon: `Assets/Sprites/Icon/ICN_IMG_AllAboard.png`.

Classification: original project-specific, AI-assisted artwork. The main ship sprite and app icon are attributed to the team's AI-assisted design process; confirm that the final exported files correspond to those designs.

### Home and shared UI artwork

Created for All Aboard! with Claude Code assistance, integrated by Eilon Grinberg and reused in Level by Dor Mandel. The sea tile is now used in both Home and the Level ocean, replacing the previous third-party ocean texture in the active Level ocean renderer.

| Asset | Path |
|---|---|
| Sea tile | `Assets/Sprites/UI/Home/IMG_SeaTile.png` |
| Coin icon | `Assets/Sprites/UI/Home/IMG_Coin.png` |
| Light wooden button | `Assets/Sprites/UI/Home/BTN_WoodLight.png` |
| Dark wooden button | `Assets/Sprites/UI/Home/BTN_WoodDark.png` |
| Dark panel | `Assets/Sprites/UI/Home/IMG_PanelDark.png` |

Development history: [PR #4](https://github.com/Eilon343/AllAboard/pull/4).

## Fonts and bundled resources

| Font | Source | License notice retained in repository |
|---|---|---|
| Bangers | TextMesh Pro Examples & Extras | `Assets/TextMesh Pro/Examples & Extras/Fonts/Bangers - OFL.txt` |
| Oswald Bold | TextMesh Pro Examples & Extras | `Assets/TextMesh Pro/Examples & Extras/Fonts/Oswald-Bold - OFL.txt` |
| Liberation Sans | TextMesh Pro bundled font | `Assets/TextMesh Pro/Fonts/LiberationSans - OFL.txt` |

These fonts are distributed under the SIL Open Font License as documented by their bundled notices. Other Unity/TextMesh Pro example resources remain subject to their accompanying licenses and terms; their presence does not imply use in the game.

## Legacy prototype assets - provenance and permissions

The following entries document the assets known to be in the project before final submission cleanup. They are not claimed to be freely redistributable. Verify permission or replace/remove them before distributing the project. Update each entry after replacement to identify the actual submitted asset and its source.

| Asset | Source / modifications | Permission status |
|---|---|---|
| `Assets/Sprites/Wheel/Ship-Wheel.png` | Likely [SVG Heart - The Ship's Helm](https://www.svgheart.com/product/the-ships-helm-free-svg-file-for-members-ships-steering-wheel-vector-image-cruising-svg/). Recolored by Dor in Paint.NET. | Source match not conclusively verified; raw digital redistribution permission not established. |
| `Assets/Sprites/BG/SEA_GIF.gif` | Found through Google Images; original creator and URL unknown. Replaced in the active Level ocean by `IMG_SeaTile.png`; removal of unused files and remaining references has not yet been confirmed. | Unverified; do not redistribute the old asset without permission. |
| `Assets/Sprites/UI/SoundToggle/SoundMute.png` | Screenshot of Microsoft Windows emoji artwork; cropped and background removed by Dor. | Redistribution permission unverified. |
| `Assets/Sprites/UI/SoundToggle/SoundUnmute.png` | Screenshot of Microsoft Windows emoji artwork; cropped and background removed by Dor. | Redistribution permission unverified. |
| `Assets/Audio/Music/Sound_8bitMenuLoop.wav` | Found online; original creator, URL and license unknown. | Unverified. |

SVG Heart license references: [Free SVG license](https://www.svgheart.com/free-svgs-license/) and [Single design license](https://www.svgheart.com/single-design-license/).

## Replacement and future soundtrack references

The following are candidate sources, not a statement that the assets have been integrated:

- [Ship Steering Helm (Vector+Sprite)](https://opengameart.org/content/ship-steering-helm-vectorsprite) - hackcraft.de / OpenGameArt, CC0.
- [Microsoft Fluent Emoji](https://github.com/microsoft/fluentui-emoji) - Microsoft, MIT. Retain the [license notice](https://github.com/microsoft/fluentui-emoji/blob/main/LICENSE) if integrated.
- [High Quality 8-bit / Chiptune Musics](https://hydrogene.itch.io/high-quality-8-bit-musics) - HydroGene, CC0. Candidate tracks: Truth, Jester Theme, Jester Battle, Slay The Evil and Strong Boss.
- [Free Pirate Game Music Pack](https://alkakrab.itch.io/free-pirate-game-music-pack) - AlkaKrab. Candidate cinematic soundtrack; verify the applicable redistribution terms before committing raw audio files.

Future soundtrack and water systems are outside the Milestone 4 scope.

## Development and AI assistance

- Unity: game engine, mobile deployment and scene editing.
- JetBrains Rider: C# development and Git integration.
- ChatGPT: Dor's gameplay prototyping, original ship-design assistance, integration, debugging and documentation.
- Claude Code: Eilon's Home UI, economy configuration, upgrade implementation and art assistance.
- GitHub and Jira: version control, review and task tracking.

The team is responsible for reviewing, integrating and testing the code and artwork. See [DECISIONS.md](DECISIONS.md) for architectural decisions and AI contributions.
