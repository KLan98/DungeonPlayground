using System;
using UnityEngine;
using System.Collections.Generic;

public static class PrimAlgorithm
{
	public static List<Corridor> Prim(List<Vector2Int> listOfCenters)
	{
		// pick the init point as the first element of centers
		HashSet<Vector2Int> connectedRooms = new HashSet<Vector2Int>() { listOfCenters[0] };
		List<Corridor> corridors = new List<Corridor>();

		// as long as every rooms are not visited
		while (connectedRooms.Count < listOfCenters.Count)
		{
			float shortestDistance = float.MaxValue;
			Vector2Int bestConnectedRoom = Vector2Int.zero; // the room in connectedRooms that produces the shortest distance to unconnected room
			Vector2Int bestUnconnectedRoom = Vector2Int.zero; // the room with the shorted distance to the connected room

			foreach (var connectedRoom in connectedRooms)
			{
				foreach (var unconnectedRoom in listOfCenters)
				{
                    // If this unconnectedRoom is already in "connectedRooms", SKIP it
                    if (connectedRooms.Contains(unconnectedRoom))
					{
						continue;
					}

					// Weight function - for decision making 
					float distance = Weight(connectedRoom, unconnectedRoom);
					if (distance < shortestDistance)
					{
						shortestDistance = distance;
						bestConnectedRoom = connectedRoom;
						bestUnconnectedRoom = unconnectedRoom;
					}
				}
			}

			connectedRooms.Add(bestUnconnectedRoom);
			Corridor newCorridor = new Corridor(bestConnectedRoom, bestUnconnectedRoom);
			corridors.Add(newCorridor);
		}

		return corridors;	
	}

	// weight function - use manhattan distance
	private static int Weight(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}

public struct Corridor
{
	public Vector2Int startPoint;
	public Vector2Int endPoint;

	public Corridor(Vector2Int startPoint, Vector2Int endPoint)
	{
		this.startPoint = startPoint;	
		this.endPoint = endPoint;
	}
}
