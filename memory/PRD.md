# Baseball Game Creator — PRD

## Original Problem Statement
UWP/XAML/C# Windows app (built 2020 in VS) that simulates a baseball game.
Two requested changes:

1. **Bug fix** — Editing any XAML/C# triggers `XamlTypeInfo.g.cs` to fail with
   `System.ExecutionEngineException` referencing
   `Windows.UI.Xaml.Controls.RevealBackgroundBrush`.
2. **Refactor** — Make `BaseballGameScreen.xaml.cs` algorithm realistic.
   Add a more in-depth strike zone with more ball/strike options.

User chose: **Light realism** + XAML changes welcome.

## Architecture (post-refactor)
- `App.xaml` — defines an explicit default `Button` style (clean, non-Reveal
  ControlTemplate) that prevents the XAML compiler from chaining into the
  Fluent Reveal types when generating `XamlTypeInfo.g.cs`.
- `BaseballSimulation.cs` *(new)* — pure-C# simulation engine.
  - `StrikeZone` enum (9 strike zones + 1 Ball zone).
  - `PitchOutcome` enum (Hit / GO / FO / Foul / SwStr / CStr / Ball).
  - `HitType` enum (1B / 2B / 3B / HR).
  - MLB-calibrated per-zone outcome tables.
  - Count-aware modifiers (2-strike defense, 3-ball aggression, 3-2 full-count
    foul bias).
  - Zone-conditioned hit-type mix (low pitches = grounders; middle/high =
    more HR / 2B).
  - Single shared `Random` (the old code created a fresh `Random()` per
    method call, which seeds from the system clock and produced biased
    sequences on fast successive calls).
- `BaseballGameScreen.xaml` — strike zone replaced from 5 buttons to a
  9-button 3×3 grid (`HL HM HR / ML MM MR / LL LM LR`) plus the Ball button.
- `BaseballGameScreen.xaml.cs` — UI shell only.
  - Game state (count, outs, runners, score) stays here.
  - One handler per zone forwards to `PitchClicked(zone)`.
  - `ResolveCurrentPitch` translates `BaseballSimulation` results into game
    state + UI updates.
  - `HitOccurred / GroundOut / Walk / HROccurred` are now
    team-parameterized (`"A"` / `"H"`), removing ~1800 lines of
    duplicated `awayX` / `homeX` code.

## What's Been Implemented (Jan 2026)
- [x] RevealBackgroundBrush XamlTypeInfo error — fixed via App.xaml Button
      style override.
- [x] New simulation engine (`BaseballSimulation.cs`).
- [x] 3×3 strike zone XAML layout + new click handlers.
- [x] Team-parameterized hit / out / walk methods (DRY refactor).
- [x] Count-aware probability adjustment.
- [x] `.csproj` updated to include `BaseballSimulation.cs`.
- [x] Foul-with-2-strikes rule fixed (no longer adds 3rd strike — MLB rule).

## What's Deferred
- [ ] Per-batter and per-pitcher stat lines that influence outcomes
      (Medium-realism upgrade).
- [ ] Pitch-type system (FB / CB / SL) with separate location distributions.
- [ ] Full fielding model (errors, double plays from any base config).
- [ ] Visual strike-zone heat map / per-pitch animation.
- [ ] Stat persistence across games.

## Validation
- C# brace balance: balanced (135/135).
- BaseballSimulation.cs brace balance: balanced (60/60).
- Both XAML files: well-formed XML.
- No orphan references to removed methods (`generateAwayHitType`,
  `awaySingleOccurred`, `TheAwayAlgorithm`, etc.) anywhere in the codebase.
- ⚠ Build verification must be done locally in Visual Studio on Windows —
  UWP cannot be compiled in the Linux preview container.

## Files Changed
- `/app/App.xaml` (overwritten)
- `/app/BaseballGameScreen.xaml` (overwritten)
- `/app/BaseballGameScreen.xaml.cs` (overwritten — 2093 → 664 lines)
- `/app/BaseballGameCreator.csproj` (added `BaseballSimulation.cs`)
- `/app/BaseballSimulation.cs` (new)
