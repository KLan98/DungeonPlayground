using System;
using UnityEngine;
using System.Collections.Generic;

public static class PrimAlgorithm
{
	public static List<Corridor> Prim(List<Vector2Int> listOfCenters)
	{
		// pick a random room as starting point
		HashSet<Vector2Int> connectedRooms = new HashSet<Vector2Int>() { listOfCenters[UnityEngine.Random.Range(0, listOfCenters.Count)] };
		List<Corridor> corridors = new List<Corridor>();

		// as long as every rooms are not visited
		while (connectedRooms.Count < listOfCenters.Count)
		{
			float shortestDistance = float.MaxValue;
			Vector2Int bestConnectedRoom = Vector2Int.zero; // the room in connectedRooms that produces the shortest distance to unconnected room
			Vector2Int bestUnconnectedRoom = Vector2Int.zero; // the room with the shorted distance to the connected room

            // compute the best connected room and best unconnected room
            foreach (var connectedRoom in connectedRooms)
			{
				foreach (var unconnectedRoom in listOfCenters)
				{
                    // If this unconnectedRoom is already in "connectedRooms", SKIP it
                    if (connectedRooms.Contains(unconnectedRoom))
					{
						continue;
					}

					// Weight function - for decision making, if 2 rooms are able to produce a distance < shortest distance then they are the new best connected room and best unconnected room
					float distance = Vector2Int.Distance(connectedRoom, unconnectedRoom); // euclidean distance
					if (distance < shortestDistance)
					{
						shortestDistance = distance;
						bestConnectedRoom = connectedRoom;
						bestUnconnectedRoom = unconnectedRoom;
					}
				}
            }

			// create the new corridor with best connected room and best unconnected room
			connectedRooms.Add(bestUnconnectedRoom);
			Corridor newCorridor = new Corridor(bestConnectedRoom, bestUnconnectedRoom);
			corridors.Add(newCorridor);
		}

		return corridors;	
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
