# 2D Platformer Game - Game Design Document (MVP)

## Table of Contents
1. [Game Overview](#game-overview)
2. [Core Mechanics](#core-mechanics)
3. [Character](#character)
4. [Weapons System](#weapons-system)
5. [Environment](#environment)
6. [Level Design](#level-design)
7. [Technical Specifications](#technical-specifications)

## Game Overview

### Game Concept
A 2D platformer featuring a character with unique weapon mechanics, focusing on environmental interaction and strategic weapon usage to navigate through levels.

### Genre
- 2D Platformer
- Action
- Puzzle elements

### Target Platform
- PC (Windows)

## Core Mechanics

### Basic Gameplay Loop
1. Start level at designated spawn point
2. Navigate through obstacles using movement and weapon mechanics
3. Collect items and power-ups
4. Reach the end point to complete the level

## Character

### Movement Mechanics
- **Basic Movement**
  - Left-right movement
  - Basic jump
  - Double jump (unlockable via achievement)
  
### Character States
- Idle
- Walking
- Jumping
- Double Jumping
- Shooting

### Collectibles System
- Coins (basic collectible)
- Double Jump power-up
- Additional collectibles (expandable system)

## Weapons System

### General Weapon Mechanics
- Weapons float above character's head
- Rotation follows mouse position
- 180-degree rotation limitation (cannot point below character)
- Weapon switching system

### Weapon Types

#### 1. Nail Gun
- **Functionality**
  - Fires nails
  - Nails stick only to wooden walls
  - Nails serve as temporary platforms
  - Nails disappear on collision with non-wooden surfaces
  
- **Gameplay Usage**
  - Create temporary platforms
  - Reach higher areas
  - Solve platforming puzzles

#### 2. Bomb Gun
- **Functionality**
  - Fires bouncing bombs
  - 2-second explosion timer
  - Manual detonation with right-click
  - Physics-based projectile movement
  
- **Gameplay Usage**
  - Destroy destructible objects
  - Create new paths
  - Trigger chain reactions

## Environment

### Platform Types
1. **Static Platforms**
   - Immovable
   - Basic level structure

2. **Moving Platforms**
   - Ping-pong movement pattern
   - Directional variants:
     - Horizontal movement
     - Vertical movement
   - Configurable speed and distance

### Obstacles

#### Destructible Objects
- Wooden walls (Nail-Gun target)
- Explosive objects
- Balloon-type objects with rewards

#### Interactive Elements
- Coins
- Power-ups
- Achievement triggers

## Level Design

### Level Structure
- Start point (configurable)
- End point (configurable)
- Multiple paths possible
- Strategic placement of:
  - Platforms
  - Collectibles
  - Obstacles
  - Power-ups

### Level Builder Specifications

#### Features
- Separate scene for level creation
- Visual object placement
- Property adjustment interface
- Level save/load system

#### Editable Elements
- Platform placement and properties
- Collectible positioning
- Start/End point locations
- Obstacle placement
- Power-up locations

#### Save System
- JSON-based level storage
- Level naming system
- Editor UI with save button
- Level validation before saving

## Technical Specifications

### Save Data Structure
```json
{
  "levelName": "string",
  "startPoint": {"x": float, "y": float},
  "endPoint": {"x": float, "y": float},
  "platforms": [
    {
      "type": "static/moving",
      "position": {"x": float, "y": float},
      "properties": {
        "movementType": "horizontal/vertical",
        "speed": float,
        "distance": float
      }
    }
  ],
  "collectibles": [
    {
      "type": "coin/powerup",
      "position": {"x": float, "y": float}
    }
  ],
  "obstacles": [
    {
      "type": "wooden/destructible",
      "position": {"x": float, "y": float},
      "properties": {
        "health": int,
        "reward": string
      }
    }
  ]
}
```

### Performance Considerations
- Object pooling for projectiles
- Efficient collision detection
- Optimized physics calculations
- Memory management for level loading

### Future Extensibility
- New weapon types
- Additional collectibles
- More platform types
- Extended achievement system
- Additional level mechanics

## Development Priorities (MVP)

### Phase 1: Core Mechanics
1. Basic character movement
2. Basic platform implementation
3. Simple level loading

### Phase 2: Weapon System
1. Weapon rotation mechanics
2. Nail Gun implementation
3. Bomb Gun implementation

### Phase 3: Level Builder
1. Basic level editor UI
2. Object placement system
3. Save/Load functionality

### Phase 4: Polish
1. Basic UI
2. Simple sound effects
3. Basic particle effects
4. Essential bug fixes 