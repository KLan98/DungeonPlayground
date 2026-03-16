# DungeonPlayground

Learn how to procedurely generate rogue-like dungeon using binary space partitioning. The method is described in: https://roguebasin.com/index.php/Basic_BSP_Dungeon_generation

Implemented room first approach, above are results when 2 dugeons are generated. When a player is placed in a given room, it is guaranteed that they can reach every other room from there.

First dungeon | Second dungeon
:-------------------------:|:-------------------------:
<img width="200" height="200" alt="First dungeon" src="https://github.com/user-attachments/assets/350a442f-dbe0-46aa-9afe-c34e3f82f43a" /> | <img width="200" height="200" alt="Second dungeon" src="https://github.com/user-attachments/assets/47bce357-dd6a-4801-8c7b-3e8cce64cc0a" />

Learn how to implement spatial hash grid. Core idea:
- Divide the world into uniform grid cells.
- Compute a hash key from each object's position to determine which cell it belongs to.
- Store objects in a hash table (dictionary) indexed by that cell key.
- When querying neighbors, check only the object’s cell and nearby cells.

Benchmark:
Brute force using binary search | Spatial hash grid
:-------------------------:|:-------------------------:
