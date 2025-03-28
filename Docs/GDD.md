# 2D Platformer Game - Game Design Document (MVP)

## Table of Contents
1. [Game Overview](#game-overview)
2. [Core Mechanics](#core-mechanics)
3. [Character](#character)
4. [Weapons System](#weapons-system)
5. [Environment](#environment)
6. [Level Design](#level-design)
7. [Technical Specifications](#technical-specifications)
8. [Architecture & Design Principles](#architecture--design-principles)
9. [Display & Camera](#display--camera)

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
- **Architectural Independence**
  - No runtime game logic dependencies
  - Clean interface for data exchange
  - Independent validation system

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

## Architecture & Design Principles

### Core Architecture Guidelines
- Strictly follow SOLID principles throughout the entire codebase
- Implement modular design with clear separation of concerns
- Use dependency injection for better testability and maintenance
- Follow the Interface Segregation Principle for component interactions
- Implement proper abstraction layers between systems

### Modularity Requirements
- Core game systems must be independent and interchangeable
- Systems should communicate through well-defined interfaces
- Implement event-driven architecture for loose coupling
- Use ScriptableObjects for configuration and data management
- Create clear boundaries between different game systems

### Level Builder Architecture
- **Strict Isolation Requirements**
  - Level Builder must be completely isolated from core game logic
  - No direct dependencies between Level Builder and game systems
  - Implement separate assembly definition for Level Builder
  - Use data contracts (interfaces) for communication
  - Level Builder should only know about data models, not game logic

### System Independence
1. **Core Game Systems**
   - Character system
   - Weapon system
   - Physics and collision
   - Game state management
   - Input handling
   
2. **Level Builder System**
   - Editor UI and controls
   - Level data management
   - Object placement system
   - Property editors
   - Level validation
   
3. **Shared Components**
   - Data models and interfaces
   - Level serialization contracts
   - Common utilities
   - Asset references

### Best Practices Implementation
- Use proper namespacing for different modules
- Implement clean architecture layers:
  - Presentation Layer (UI, Input)
  - Domain Layer (Game Logic)
  - Data Layer (Persistence, Configuration)
- Follow Unity's component-based architecture
- Use composition over inheritance
- Implement proper error handling and logging

### Testing Architecture
- Support unit testing through proper dependency injection
- Create mock implementations for interfaces
- Separate editor and runtime logic
- Implement test automation framework
- Support integration testing between modules 

## Display & Camera

### Resolution Specifications
- Target Resolution: 1920x1080 (Full HD)
- Orientation: Landscape
- Aspect Ratio: 16:9
- Support for common 16:9 resolutions:
  - 1920x1080 (Full HD)
  - 1600x900
  - 1366x768

### Camera Requirements
- Fixed camera bounds for each level
- Level design must fit within camera viewport
- No camera movement/scrolling
- All gameplay elements must remain within visible bounds
- Clear view of:
  - Character
  - Platforms
  - Projectiles
  - Collectibles
  - Start/End points

### Level Boundary Guidelines
- All platforms must be fully visible
- Ensure sufficient space for projectile trajectories
- Account for UI elements in layout
- Maintain safe zones for essential gameplay elements
- Consider weapon rotation radius in level bounds

### Visual Considerations
- Clear visibility of all interactive elements
- Proper contrast between foreground and background
- Adequate space for particle effects
- UI elements positioned within safe zones
- Consistent scale of game objects 