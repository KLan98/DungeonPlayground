# DungeonPlayground

Implemented procedurely generate rogue-like dungeon using binary space partitioning. Goal is to create replayability for dungeon crawler games, through that increase player's engagment. The method is described in: https://roguebasin.com/index.php/Basic_BSP_Dungeon_generation

Core idea:
- Rooms first approach.
- Recursively divide a given space into 2 sub-spaces.
- Division stopped when constraints of width and height are satisfied. Demonstration code snippet: 
```csharp
// Only attempt to split if the room meets the minimum size requirements
if (room.size.x >= minWidth && room.size.y >= minHeight)
{
    bool splitSucceeded = false;
    // Try vertical split first
    if (room.size.x >= minWidth * scaler)
    {
        splitSucceeded = SplitVertically(room, minWidth, roomsQueue);
    }

    // If previous split not successful then try to do this
    if (!splitSucceeded && room.size.y >= minHeight * scaler)
    {
        splitSucceeded = SplitHorizontally(room, minHeight, roomsQueue);
    }

    // If rooms can no longer be splitted further (leaf node in binary tree)
    else
    {
        // Add room to list-of-non-splitable-rooms
    }
}

return list-of-non-splitable-rooms;
```
- Render the rooms and corridors using Unity's tiles.

Below are results of 2 generated dungeons. When a player is placed in a given room, it is guaranteed that they can reach every other room from there.

First dungeon | Second dungeon
:-------------------------:|:-------------------------:
<img width="200" height="200" alt="First dungeon" src="https://github.com/user-attachments/assets/350a442f-dbe0-46aa-9afe-c34e3f82f43a" /> | <img width="200" height="200" alt="Second dungeon" src="https://github.com/user-attachments/assets/47bce357-dd6a-4801-8c7b-3e8cce64cc0a" />

Implemented spatial hash grid to optimize collision detection. Core idea:
- Divide the world into uniform grid cells.
- Compute a hash key from each object's position to determine which cell it belongs to.
- Store objects in a hash table (dictionary) indexed by that cell key.
- When querying neighbors, check only the object’s cell and nearby cells.

Benchmark:
Brute force using binary search | Spatial hash grid
:-------------------------:|:-------------------------:
