# MOD010307 Mobile Game Programming

## Microgame Programming Portfolio

**Student ID: 2420768**  
**Module:** MOD010307 Mobile Game Programming  
**Engine / Version: Unity 6.3**  
**Programming Language: C#**  
**Final Release / Tag:**  
**Final Commit:**  

---

## About this repository

This repository contains my assessment work for MOD010307 Mobile Game Programming.

The project consists of four short mobile microgames developed within a shared framework. Each microgame focuses on a different area of game programming.

This repository also contains evidence of:

- programming knowledge and application;
- use of game-engine features;
- student-authored programming;
- testing and debugging;
- iterative development using Git and GitHub;
- performance profiling and optimisation;
- responsible use of generative AI where permitted.

The repository history, branches and pull requests form part of the assessment evidence.


---

# Microgames

## 1. Movement and Control

**Game title: Slalom**  
**Location:** [Microgame documentation](Microgames/01-Movement-Control/README.md)

**Main technical focus:**  
Player movement, navigation using input, vectors, delta time and game state. Extended with data-driven procedural gate generation, configurable movement profiles and medal progression.

### Primary student-authored scripts

- [`SlalomGame.cs`](MobileGameProject/Assets/Scripts/Microgames/Slalom/SlalomGame.cs) – Core microgame logic, Begin() override, score tracking, medal evaluation, framework integration
- [`GateDefinition.cs`](MobileGameProject/Assets/Scripts/Microgames/Slalom/GateDefinition.cs) – ScriptableObject gate configuration (prefab, spawn behaviour, connection rules)
- [`GatePrefabManager.cs`](MobileGameProject/Assets/Scripts/Microgames/Slalom/GatePrefabManager.cs) – Procedural gate selection, compatibility validation and spawning
- [`GatePassed.cs`](MobileGameProject/Assets/Scripts/Microgames/Slalom/GatePassed.cs) / [`GateMissed.cs`](MobileGameProject/Assets/Scripts/Microgames/Slalom/GateMissed.cs) – Success / failure detection and scoring feedback
- [`MoveGates.cs`](MobileGameProject/Assets/Scripts/Microgames/Slalom/MoveGates.cs) – Gate movement, speed control and lifetime management
- [`GateScript.cs`](MobileGameProject/Assets/Scripts/Microgames/Slalom/GateScript.cs) – Trigger-based interaction, side identification via enums, visual feedback
- Player movement / generic player controller – Input handling, vector-based movement, speed and rotation settings driven by `MovementProfile`
- Supporting framework scripts: countdown (`MicrogameSession`), scene navigation (`SceneNavigator`), medal integration

### Key pull requests

- Initial Slalom microgame (scene, player, gates, countdown timer, physics migration from UI Canvas to 3D play area, TextMeshPro UI)
- Player movement controller, progress/feedback text and Android build testing
- Progression / medal framework integration (ScriptableObject thresholds, `MedalManager`, main-menu display) and input refactor (`OnPointerPressed`)
- Data-driven game configuration system (`GameConfiguration`, `InputProfile`, `MovementProfile`) and generic player controller refactor
- Procedural gate generation system (GateDefinition ScriptableObjects, flags-enum connection validation, spawn biasing, GatePassed/GateMissed)
- Unit tests for framework data assets and assembly definition (`MicrogameCourse.asmdef`)

---

## 2. Physics and Interaction

**Game title: Shooting** (Biathlon / Shooting microgame)  
**Location:** [Microgame documentation](Microgames/02-Physics-Interaction/README.md)

**Main technical focus:**  
Physics-based gameplay using raycasting, forces / offsets, camera systems, collisions / hit detection and related systems. Extended with mobile button controls and wind calibration.

### Primary student-authored scripts

- [`ShootingGame.cs`](MobileGameProject/Assets/Scripts/Microgames/Shooting/ShootingGame.cs) – Core gameplay loop, overrides for directional / button input, scoring, medal evaluation, scope calibration state
- Wind / calibration manager – Wind adjustment, scope calibration detection and shot offset logic
- Mobile button input handlers (`IPointerDownHandler` / `IPointerUpHandler`) – Continuous directional movement and action buttons
- Scope zoom camera system (dedicated camera + render texture)
- Raycast-based shooting and target hit detection
- Supporting framework extensions: virtual methods `OnDirectionalInput`, `OnButtonTap`, `OnStopMovement` in `MicrogameBehaviour`

### Key pull requests

- Foundation scene for Biathlon / Shooting
- Full Shooting microgame implementation (mobile directional + action buttons, wind calibration, scope zoom, raycast shooting, score tracking, medal integration, visual hit feedback)
- Framework input extensions for button-based controls and movement lifecycle events
- Camera / framing adjustments and code cleanup

---

## 3. Timing, State and Logic

**Game title: Ice Skating**  
**Location:** [Microgame documentation](Microgames/03-Timing-State-Logic/README.md)

**Main technical focus:**  
Timing, sequencing, changing game states, rules, success/failure conditions, dynamic skill selection and reaction / performance-based gameplay.

### Primary student-authored scripts

- [`IceSkatingGame.cs`](MobileGameProject/Assets/Scripts/Microgames/IceSkating/IceSkatingGame.cs) – Core microgame logic and framework integration
- [`IceSkatingSkillData.cs`](MobileGameProject/Assets/Scripts/Microgames/IceSkating/IceSkatingSkillData.cs) – ScriptableObject skill definitions (name, difficulty, presentation)
- [`GameFeedbackData.cs`](MobileGameProject/Assets/Scripts/...) – Configurable success / failure feedback messages
- Difficulty system (enum + dictionary colour mapping)
- Dynamic skill selection UI generation from asset data
- Judge prefabs and scoring / feedback integration
- Scene navigation and Android build support

### Key pull requests

- First implementation of Ice Skating microgame (scene, sprites/materials, judge prefabs, skill data system, difficulty colour coding, dynamic UI generation, feedback system, Android testing)

---

## 4. Data, Collections and Algorithms

**Game title: Curling** 
  
**Location:** [Microgame documentation](Microgames/04-Data-Collections-Algorithms/README.md)

**Main technical focus:**  
Arrays, lists or other collections together with searching, sorting or ordered data. Strongly evidenced by the data-driven framework (dictionaries for medals and difficulty colours, ScriptableObject collections for gates/skills/profiles, procedural generation with compatibility filtering).

### Primary student-authored scripts

- Framework data systems already in use:
  - `MedalManager` – `Dictionary<string, MedalLevel>`
  - Gate generation – collections of `GateDefinition`, flags-enum filtering, spawn bias
  - Ice Skating – skill asset collections, difficulty-to-colour dictionaries
  - Configuration profiles (`GameConfiguration`, `InputProfile`, `MovementProfile`)
- Unit tests: `MovementProfileTests.cs`, `InputProfileTests.cs`, `MedalThresholdTests.cs`

### Key pull requests

- Progression / medal framework (dictionaries, ScriptableObject thresholds)
- Game configuration system (ScriptableObject profiles)
- Procedural gate generation (collections, flags enums, compatibility validation)
- Unit tests for data assets and assembly definition

---

# Shared Framework

See [Framework documentation](Documentation/Framework.md).

The framework documentation should explain how the microgames are connected and identify any permitted AI-supported development used to create, review or integrate the framework.

### Key framework components

| Component | Purpose | Key files |
|-----------|---------|-----------|
| Countdown timer | Reusable pre-game countdown | `MicrogameSession.cs` |
| Scene navigation | Multi-microgame menu loading | `SceneNavigator.cs` |
| Progression / medals | Bronze/Silver/Gold awards via ScriptableObject thresholds | `MedalManager.cs`, `MedalThreshold.cs`, `MedalDisplay.cs`, `MainMenuController.cs`, `EventResult.cs` |
| Input abstraction | Overridable pointer / directional / button handlers | `MicrogameBehaviour.cs` (`OnPointerPressed`, `OnDirectionalInput`, `OnButtonTap`, `OnStopMovement`) |
| Game configuration | Data-driven controls, movement and game info | `GameConfiguration.cs`, `InputProfile.cs`, `MovementProfile.cs`, `GameInfoDisplay.cs` |
| Feedback system | Configurable success/failure messages | `GameFeedbackData.cs` |
| Assembly definition | Project organisation and faster compilation | `MicrogameCourse.asmdef` |
| Unit tests | Validation of configuration assets | `*ProfileTests.cs`, `MedalThresholdTests.cs` |

AI assistance across framework work was limited to research, explanation of concepts, design suggestions and testing guidance. Core implementation remains student-authored.

---

# Testing and Debugging

See [Testing documentation](Documentation/Testing.md).

Supporting screenshots, logs, recordings or other evidence may be stored in:

`Evidence/Testing/`

### Testing summary (from development PRs)

**Slalom**
- Countdown, gate spawning/movement, physics triggers, UI feedback and scene navigation function correctly
- Procedural generation respects compatibility rules and produces varied courses
- Pass/miss detection and medal awarding work as expected
- Known issues: further balancing of gate speed/difficulty and medal thresholds; tilt controls planned but not implemented

**Shooting**
- Wind calibration, scope zoom, raycast hit detection, score tracking and medal awarding all pass
- Mobile directional and action buttons trigger correct framework events
- Known issues: camera shake partially removed; some debug logging remains; `MoveTarget()` contains placeholder behaviour

**Ice Skating**
- Skill assets load and drive dynamic UI; difficulty colour mapping works; feedback messages display correctly
- Android build launches and operates successfully
- Known issues: skill balancing and additional skills planned; judge feedback logic may be expanded

**Framework data assets**
- Unit tests for Movement Profiles, Input Profiles and Medal Threshold assets all pass
- Configuration assets load and return expected values

Android build testing was performed for player movement, Ice Skating and Shooting systems.

---

# Performance and Optimisation

See [Performance investigation](Documentation/Performance.md).

Supporting profiler captures, measurements and related evidence may be stored in:

`Evidence/Performance/`

---

# References and Credits

See [References and Credits](Documentation/References-and-Credits.md).

Asset credits for Slalom sprites and related artwork have been documented as part of the procedural generation PR.

---

# Learning Outcome Evidence

| Learning Outcome | Main Evidence |
|---|---|
| LO1 – Programming knowledge and syntax | Microgame source code, Microgame READMEs and relevant pull requests |
| LO2 – Game-engine features and mobile framework | Shared framework, mobile input/UI (pointer + button controls), cameras/render textures, physics/raycasting and framework pull requests |
| LO3 – Student-authored mobile game components | Identified source files within the microgames (see Primary student-authored scripts sections). Do **not** identify AI-generated code as LO3 evidence. |
| LO4 – Testing simple game components | Testing documentation, unit tests for data assets, commits, pull requests and supporting evidence |
| LO5 – Performance evaluation and optimisation | Performance investigation and profiler evidence |

**LO3 declaration (consistent across PRs):**  
Core gameplay logic, framework integration, scoring, medal evaluation, procedural generation, input handling and configuration systems were student-authored. AI assistance was limited to research/documentation, explanation of errors/concepts, design/refactoring suggestions, test/debugging guidance and documentation support.

---

# Build and Run Instructions

## Emulator

_Explain how the submitted project can be run using the emulator used during development._

For example:

1. Open the Unity project.
2. 
3. 

## Physical Device

Android build testing was carried out during development of the player movement system, Ice Skating microgame and Shooting microgame. Successful Android builds were generated and run on device.

For example:

1. Open the Unity project and switch build target to Android.
2. Configure Player Settings / package name as required.
3. Build and Run (or install the generated APK) on a connected Android device.

If physical-device testing was not possible for a particular microgame, briefly state this here.

---

# Submission State

**Git tag / release:**  
**Commit hash:**  

The version identified above is the version submitted for assessment.

Any commits made after this point are not part of the submitted assessment unless otherwise agreed with the Module Leader.
