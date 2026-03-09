# 🧟 BaseZombies2D

> A top-down 2D zombie survival game built in Unity, powered by the **Base Network** (Ethereum L2).

---

## 🎮 Game Overview

BaseZombies2D is a browser-playable (Unity WebGL) top-down zombie survival game with real blockchain token rewards. Players fight endless waves of zombies, collect token fragments, and claim on-chain rewards — or risk losing everything by pushing further.

A Steam version (Web2) is also planned with the same gameplay but no wallet required.

---

## 🕹️ Gameplay Loop

1. Player enters a room → zombie waves begin spawning
2. Kill zombies → earn token fragments
3. Every **5 waves**, the game pauses:
   - **Continue** → push further for more rewards (risk losing pending tokens if you die)
   - **Claim** → wallet popup appears, tokens are transferred to your wallet on Base

---

## 🧟 Zombie Types

| Type | Speed | Health | Special | Appears From |
|------|-------|--------|---------|--------------|
| Basic | Normal | 3 | — | Wave 1 |
| Runner | ~Player speed | 2 | Fast movement | Wave 6 |
| Smoker | Slow | 4 | Shoots acid projectiles every 5s | Wave 11 |
| Tank | Slow | 6 | High health, higher drops | Wave 16 |
| Boss | Medium | High | Spawns every 10 waves, drops blind box | Wave 10 |

- Boss must be defeated before the next wave begins
- All other zombie types continue spawning alongside bosses
- Room 1 has a maximum of **40 waves** and **8 bosses**

---

## ⚔️ Weapon System

### Weapon Categories

| Category | Weapons |
|----------|---------|
| Pistol | Pistol, Glock, Baretta |
| SMG | MP5, UMP45, Vector |
| Rifle | AK47, M4, SCAR |
| LMG | M249, RPK |

### Weapon Tiers

- **Basic** (Soulbound) — Purchased with in-game tokens, unlimited supply, not tradeable
- **Legendary Skin** (NFT) — Limited supply, obtained from blind boxes only, tradeable on secondary markets

Each legendary skin has identical stats to its basic counterpart but provides a **token drop rate bonus** (+5% for AK47 Legendary, etc.)

---

## 🪙 Token System

- Game token is tradeable on the **Base network**
- Tokens are **NOT minted infinitely** — a reward pool holds all tokens
- When a player claims, tokens are transferred **from the pool to their wallet**

### Token Uses
- Buy weapons
- Open blind boxes
- Unlock rooms
- Upgrade equipment
- Repairs / barricades

### Token Drop Mechanics
- Each zombie has a base drop chance
- Better weapons increase drop chance
- Drop happens in-game, but **blockchain transaction only happens on CLAIM**
- If player dies before claiming → **pending tokens are burned**

---

## 📦 Blind Box System

| Rarity | Drop Chance |
|--------|-------------|
| Common | 60% |
| Rare | 30% |
| Epic | 9% |
| Legendary | 1% |

- Legendary weapon skins: **~0.25% effective drop chance**
- Limited supply (e.g. Epic: 1000, Legendary: 100)
- Blind boxes can be purchased with tokens or dropped by bosses

---

## 🏠 Room Progression

| Room | Zombie Types | Max Waves | Max Bosses |
|------|-------------|-----------|------------|
| Room 1 | Basic, Runner, Smoker, Tank, Boss | 40 | 8 |
| Room 2 | Fast zombies introduced | TBD | TBD |
| Room 3 | Tank zombies, higher drops | TBD | TBD |
| Room 4 | Boss arena | TBD | TBD |

- Rooms are connected
- Unlocking a new room temporarily resets drop balance
- Higher rooms = higher drop rates

---

## ⚠️ Risk System

- If the player **dies before claiming** → all pending tokens are **burned**
- Player loses the run
- The Continue/Claim decision is the core risk loop of the game

---

## 🔐 Anti-Cheat Architecture

```
Unity Client → Game Server → Blockchain
```

1. Unity reports kills and run data to the server
2. Server validates gameplay
3. Server signs a reward message
4. Player claims tokens on-chain using the server signature

---

## 🌐 Web3 Version

- Runs in browser (Unity WebGL)
- Wallet connection: MetaMask / Coinbase Wallet
- Token claims on **Base network**

## 🎮 Steam Version

- No wallet required
- Tokens replaced by in-game currency
- NFT skins replaced by normal cosmetic skins
- Identical gameplay

---

## 🛠️ Tech Stack

- **Engine:** Unity 6 (2D)
- **Blockchain:** Base Network (Ethereum L2)
- **Wallet:** MetaMask, Coinbase Wallet
- **Language:** C#

---

## 📋 Current Development Status

### ✅ Completed
- Player movement + shooting
- Wave/spawn system
- Zombie AI (movement + damage)
- Player health system
- HP bars (player + zombie)
- Continue or Claim screen
- Camera follow system
- Weapon system (fire rate, ammo, reload)
- Inventory UI
- Zombie types (Basic, Runner, Tank)

### 🔧 In Progress
- Smoker zombie (acid projectile)
- Boss zombie
- Game Over screen
- Token drop mechanics
- Room system

### 📅 Planned
- Web3 wallet integration
- NFT skin system
- Steam build
- Anti-cheat server

---

## 👨‍💻 Development

This project is being actively developed. Commits are made daily as new features are added.

---

*Built on Base. Survive the horde. Claim your tokens.*
