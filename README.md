<div align="center">

# 🚀 Spaceship Battle

<table>
  <tr>
    <td><img src="docs/Spaceship_Battle1.gif" alt="Gameplay Wave Start" width="420"/></td>
    <td><img src="docs/Spaceship_Battle2.gif" alt="Gameplay Combat" width="420"/></td>
  </tr>
</table>

### ⚔️ *3D Space Shooter — Rebuilt from Scratch. Zero Shortcuts.* ⚔️

<br/>

[![Unity](https://img.shields.io/badge/Unity-2022.x-000000?style=for-the-badge&logo=unity&logoColor=white)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-95.6%25-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![WebGL](https://img.shields.io/badge/WebGL-Playable-E34F26?style=for-the-badge&logo=webgl&logoColor=white)](https://xchrmas.github.io/Spaceship-Battle/)
[![Architecture](https://img.shields.io/badge/Architecture-MVP-7B68EE?style=for-the-badge)]()
[![Status](https://img.shields.io/badge/Status-In%20Development-orange?style=for-the-badge)]()

<br/>

> **Not just another Unity shooter. This is a precision-engineered combat system —**
> **built with production-grade architecture, zero magic numbers, and zero compromise.**

</div>

---

## 🏗️ Architecture

This project is built **entirely from scratch** with a deliberate, opinionated architecture.
No bloated frameworks. No shortcuts. Every system earns its place.

```
┌─────────────────────────────────────────────────────────────────────┐
│                        ARCHITECTURE OVERVIEW                         │
├──────────────────────────┬──────────────────────────────────────────┤
│  Pattern                 │  MVP (Model · View · Presenter)           │
│  Dependency Injection    │  Custom Service Locator  ← zero external  │
│  Enemy Behaviour         │  Finite State Machine (FSM)               │
│  Object Pooling          │  HashSet — O(1) acquire & release         │
│  Event System            │  Native C# events  ← zero UniRx           │
│  Configuration           │  GameConstants  ← zero magic numbers      │
└──────────────────────────┴──────────────────────────────────────────┘
```

### Why these choices?

| Decision | What was rejected | Why |
|---|---|---|
| **Custom Service Locator** | Zenject / VContainer | Full control, zero overhead, no reflection magic |
| **Native C# events** | UniRx / R3 | Readable, debuggable, no hidden allocations |
| **HashSet Object Pool** | Unity's built-in pool | O(1) contains-check, no array bounds |
| **GameConstants** | Scattered magic numbers | Single source of truth, refactor-safe |
| **MVP** | MonoBehaviour spaghetti | Testable, decoupled, scalable |

---

## 🛠️ Tech Stack

### Engine & Language
[![Unity](https://img.shields.io/badge/Unity-2022.x-000000?style=flat-square&logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-.NET-239120?style=flat-square&logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Shader Graph](https://img.shields.io/badge/Shader_Graph-URP-7B68EE?style=flat-square&logo=unity)](https://unity.com/features/shader-graph)
[![Rider](https://img.shields.io/badge/JetBrains_Rider-IDE-000000?style=flat-square&logo=rider)](https://www.jetbrains.com/rider/)

### Unity Packages
[![Addressables](https://img.shields.io/badge/Addressables-Asset_Management-0078D7?style=flat-square&logo=unity)](https://docs.unity3d.com/Packages/com.unity.addressables@latest)
[![UniTask](https://img.shields.io/badge/UniTask-Async%2FAwait-00BFFF?style=flat-square)](https://github.com/Cysharp/UniTask)
[![DOTween](https://img.shields.io/badge/DOTween-Tweening-FF6B35?style=flat-square)](http://dotween.demigiant.com/)

```
┌─────────────────────────────────────────────────────────────────────┐
│  Addressables  →  Runtime asset loading, memory-efficient bundles    │
│  UniTask       →  Zero-allocation async/await on Unity's main thread │
│  DOTween       →  High-performance tweening for UI & VFX             │
│  Shader Graph  →  Node-based custom shaders (shields, thruster FX)   │
└─────────────────────────────────────────────────────────────────────┘
```

---

## ✨ Core Systems

### 🔧 Service Locator
Zero external dependencies. Services register themselves on Awake, consumers resolve at Start. Clean, fast, no reflection.

```csharp
// Registration
ServiceLocator.Register<IWeaponService>(this);

// Resolution
var weapons = ServiceLocator.Get<IWeaponService>();
```

### ♻️ Object Pool — HashSet O(1)
Enemies, bullets, VFX — all pooled. `HashSet<T>` means O(1) acquire/release with instant duplicate detection.

```csharp
// No List.Contains() == no O(n) scan
_activeObjects.Add(obj);    // O(1)
_activeObjects.Remove(obj); // O(1)
```

### 🤖 Enemy FSM
Every enemy is a state machine. States are self-contained, transitions are explicit.

```
  [Idle] ──► [Patrol] ──► [Chase] ──► [Attack]
                              ▲            │
                              └────────────┘
                                  [Dead]
```

### 📡 C# Events — Zero Allocations
No UniRx observables, no lambdas stored on heap. Struct-based event args keep GC pressure at zero.

```csharp
public static event Action<EnemyDeathArgs> OnEnemyDied;
```

### 📦 Addressables
Assets load asynchronously via labels. Memory is released the moment a scene unloads — no stale references.

### ⚡ UniTask
`async/await` without the Unity coroutine ceremony. Cancellation tokens on every async flow. Zero thread switches.

### 🎨 Shader Graph
Custom URP shaders for thruster glow, shield dissolve, hit flash, and deep-space backgrounds — all node-based, artist-friendly.

---

## 🚀 Getting Started

### 🎮 Play in Browser

```
Open: /WebGL_Build/index.html
```

No install. No plugin. Open in any modern browser and fight.

### 🖥️ Open in Unity

```bash
git clone https://github.com/xchrmas/Spaceship-Battle.git
```

Then: **Unity Hub → Add → Select cloned folder**

> Requires Unity **2022.x** or newer with **URP** enabled.

---

## 🎯 Controls

```
  W / ↑  ───── Move Forward          LEFT CLICK ── Fire
  S / ↓  ───── Move Backward         MOUSE ─────── Aim
  A / ←  ───── Strafe Left           ESC ────────── Pause
  D / →  ───── Strafe Right
```

---

## 📁 Project Structure

```
Spaceship-Battle/
│
├── 📁 Assets/
│   ├── Scripts/
│   │   ├── Architecture/   → ServiceLocator, GameConstants
│   │   ├── MVP/            → Models, Views, Presenters
│   │   ├── Pool/           → HashSet ObjectPool
│   │   ├── FSM/            → Enemy states & transitions
│   │   └── Systems/        → Weapons, Health, Scoring
│   │
│   ├── Shaders/            → Shader Graph assets (URP)
│   ├── Prefabs/            → Addressable prefabs
│   └── Scenes/
│
├── 📁 Packages/            → Addressables, UniTask, DOTween
├── 📁 WebGL_Build/         → 🎮 index.html — play now
└── 📁 ProjectSettings/
```

---

## 🗺️ Roadmap

```
[✅] MVP Architecture
[✅] Custom Service Locator
[✅] HashSet Object Pool
[✅] Enemy FSM
[✅] C# event system
[✅] GameConstants
[✅] Addressables integration
[✅] UniTask async pipeline
[✅] DOTween animations
[✅] Shader Graph VFX
[✅] WebGL Build

[ ] Multiple enemy types
[ ] Boss fights
[ ] Weapon upgrade system
[ ] Score & leaderboard
[ ] Audio system
[ ] Multiplayer (PvP)
```

---
</div>
