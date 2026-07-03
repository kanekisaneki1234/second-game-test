# Camping Simulator

![Unity](https://img.shields.io/badge/Unity-2022.3-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-10.0-purple?logo=csharp)
![Platform](https://img.shields.io/badge/Platform-WebGL%20%7C%20macOS-lightgrey)
![Event](https://img.shields.io/badge/Event-AETHRA%20Global%20Gameathon%202025-green)

> A 2D educational simulation game teaching real-world camping skills through interactive gameplay. Built in Unity for the AETHRA Global Gameathon 2025.

**[▶ Play in Browser](https://kanekisaneki1234.github.io/Portfolio_Website/camping-simulator/)** · **[Portfolio Page](https://kanekisaneki1234.github.io/Portfolio_Website/game.html?id=camping-simulator)**

---

## Overview

Camping Simulator guides the player through a complete camping session: pitch a tent, collect resources and build a campfire, forage mushrooms, and cook a meal. All of this is gated behind a 12-step tutorial FSM that teaches Leave No Trace principles as gameplay mechanics rather than text walls.

The project was built under hackathon time constraints (~48 hours), so the architecture prioritises clarity and forward-momentum — each manager owns exactly one system, with a lightweight singleton pattern for cross-system communication. No coroutine soup; every multi-step process runs through an explicit state machine.

---

## Architecture

### Manager Hierarchy

```
GameSceneManager (DontDestroyOnLoad)
│   Owns scene transitions with fade coroutines
│   Scenes: MainMenu → GameScene → Credits
│
├── TutorialManager (Singleton)
│   12-step FSM polling InteractableManager each Update()
│   Steps: Welcome → GoToTent → SetupTent → GoToCampfire
│          → CollectTinder → CollectKindling → CollectLogs
│          → GoBackToCampfire → BuildFire → CollectMushrooms
│          → CookMeal → Complete
│
├── InteractableManager
│   Boolean state store (tentSetUp, campfireLit, foodCooked)
│   Read by TutorialManager to detect step completion
│
├── TentSetupManager
│   3-phase FSM: PlacingStakes → RaisingPoles → FinalSetup
│   Phase transitions driven by coroutines; Time.timeScale unchanged
│
├── CampfireManager
│   Sequential build UI: tinder → kindling → logs → light
│   Pauses Time.timeScale = 0 during the interaction panel
│
├── CookingManager
│   Collects 5 mushrooms; unlocks cooking panel when threshold met
│
└── FireResourceManager
    Tracks tinder / kindling / log pickup counts
    Consumed by CampfireManager to gate build steps
```

### Interaction System

`Interactable.cs` uses `OnTriggerEnter2D`/`OnTriggerExit2D` to detect player proximity, then forwards an E-key press to `InteractionPromptManager` (singleton). Context-sensitive prompts change based on which `Interactable` the player is nearest to. `InteractableResource.cs` extends this for collectible resources (tinder, kindling, logs).

### Tutorial FSM

`TutorialManager` is a 12-state FSM that runs `CheckStepCompletion()` every `Update()`. Each step checks one condition against `InteractableManager` (booleans) or `FireResourceManager` / `CookingManager` (counts). When the condition is met, the FSM advances and fires the appropriate UI update. This polling approach was chosen over events because the step conditions are already maintained as state in their respective managers — no additional callback wiring needed.

### Scene Transitions

`GameSceneManager` is a `DontDestroyOnLoad` singleton that owns all scene transitions. `SceneTransitionManager` handles the fade coroutines (fade-out → `LoadScene` → fade-in). This ensures the fade canvas persists across loads and isn't recreated each scene.

---

## Script Reference

| Script | System | Purpose |
|--------|--------|---------|
| `Manager/GameSceneManager.cs` | Scene | DontDestroyOnLoad singleton; owns all scene transitions |
| `Manager/SceneTransitionManager.cs` | Scene | Fade coroutines for scene transitions |
| `Manager/TutorialManager.cs` | Tutorial | 12-step FSM; polls step completion each Update() |
| `Manager/InteractableManager.cs` | State | Boolean store for cross-system game state |
| `Manager/TentSetupManager.cs` | Tent | 3-phase tent setup FSM |
| `Manager/CampfireManager.cs` | Campfire | Sequential campfire build UI (pauses time during panel) |
| `Manager/CookingManager.cs` | Cooking | Mushroom collection threshold + cooking panel |
| `Manager/FireResourceManager.cs` | Resources | Tinder / kindling / log pickup counts |
| `Mechanics/Interactable.cs` | Interaction | E-key proximity interaction with context-sensitive prompts |
| `Mechanics/InteractableResource.cs` | Interaction | Collectible resource pickup |
| `Mechanics/CollectibleFood.cs` | Interaction | FoodType enum + food collectible pickup |
| `Player/PlayerMovement.cs` | Player | WASD movement |
| `Player/CameraFollow.cs` | Player | Camera follows player transform |
| `UI/InteractionPromptManager.cs` | UI | Singleton; "Press E to interact" prompt management |
| `UI/PanelAnimator.cs` | UI | Reusable panel show/hide animations |
| `UI/AboutPopupManager.cs` | UI | About popup logic |
| `UI/CreditsManager.cs` | UI | Credits scene logic |
| `Tutorial/TutorialManager.cs` | Tutorial | (see above) |
| `Tutorial/BouncingArrow.cs` | Tutorial | Animated guide arrow pointing at next objective |
| `Tutorial/ObjectHighlighter.cs` | Tutorial | Glow highlight on interactive objects |
| `Data/CampfireSprites.cs` | Data | ScriptableObject — 6 campfire stage sprites |

---

## Key Design Decisions

### Why poll in TutorialManager instead of events?

The step conditions (tent set up, fire lit, food cooked, resource counts) are already maintained as persistent state in their respective managers. Subscribing to change events would require wiring callbacks in 5 different managers for what is essentially a one-way read. Polling `CheckStepCompletion()` each frame keeps the tutorial logic self-contained and easier to debug under hackathon conditions.

### Why Time.timeScale = 0 in CampfireManager?

The campfire build UI is a multi-step interaction panel that requires deliberate player action. Pausing game time ensures the player cannot be interrupted by other world events while the panel is open, without needing to disable the player controller or toggle colliders manually.

### Why a 3-phase FSM for tent setup instead of a single interaction?

Tent pitching has three distinct physical steps (stakes → poles → canvas) that each need their own animation trigger, audio cue, and validation. A flat boolean would conflate these; an enum-driven FSM makes each phase explicit and independently testable.

---

## How to Run

> **Note:** This repo contains C# scripts only. It is not the full Unity project.
> A playable WebGL build is available on the [portfolio page](https://kanekisaneki1234.github.io/Portfolio_Website/game.html?id=camping-simulator).

To run locally:

1. Create a new **Unity 2022.3 LTS** project (2D template)
2. Copy `Assets/Scripts/` into your project's `Assets/` folder
3. Import **TextMesh Pro** via Window → Package Manager
4. Recreate the three scenes (`MainMenu`, `GameScene`, `Credits`) and wire up the managers as GameObjects
5. Assign the `CampfireSpritesData` ScriptableObject to `CampfireManager`

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Engine | Unity 2022.3 LTS |
| Language | C# 10 |
| UI | Unity uGUI + TextMesh Pro |
| Physics | Unity 2D (Rigidbody2D, Collider2D) |
| Platforms | WebGL · macOS |
| Event | AETHRA Global Gameathon 2025 |
