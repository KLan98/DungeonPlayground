# DungeonPlayground

Learn how to procedurely generate rogue-like dungeon using binary space partitioning. The method is described in: https://roguebasin.com/index.php/Basic_BSP_Dungeon_generation

Implemented room first approach


Learn how to implement spatial hash grid. Core Idea

- Divide the world into uniform grid cells.
- Compute a hash key from each object's position to determine which cell it belongs to.
- Store objects in a hash table (dictionary) indexed by that cell key.
- When querying neighbors, check only the object’s cell and nearby cells.

Benchmark:

