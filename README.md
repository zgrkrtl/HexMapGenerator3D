# HexGridMapGenerator
A **flexible hex grid map generation system** for Unity that allows runtime and editor-based grid generation. Designed for modularity and ease of integration into custom game levels.

---

## Features

- Generate hex grids directly through the Unity Editor.
- Individual hex cells can be removed or reshaped to match intended level layouts.
- All hex cells, its coordinates-worldpositions and their neighbors are fully accessible via scripts, allowing custom logic and gameplay implementations.
- Provide a hex prefab(HexCell) or use existing one in the project.
- Works in both 2D and 3D projects.
- Editor scripts included for efficient grid management.


## Installation

1. Clone the repository or download as ZIP:
2. Copy the HexGridToolKit folder into your Unity project Assets/ folder.
3. Open the included demo scene to see a working example of hex cell accessibility.

Usage

Generating a Grid
1. Add the HexGridManager prefab to your scene.
2. Configure the grid radius and cell radius in the Inspector.
3. Click Generate Grid to populate your hex grid.

Hex cells can be individually removed to create custom level shapes.
All hex cells and their neighbors are accessible via the HexGridManager dictionary and HexCell scripts for advanced logic.

The included HexCellClickTester scene demonstrates programmatic access to hex cells and their neighbors.

This project is MIT licensed – free to use, modify, and distribute.

## Don't forget to install TMP for visual feedback of HexCell coordinates. (HexCell script should be attached to your or provided hexagon prefab with also TMP attached to it)

## Referenced hex grid system logic: https://www.redblobgames.com/grids/hexagons/
