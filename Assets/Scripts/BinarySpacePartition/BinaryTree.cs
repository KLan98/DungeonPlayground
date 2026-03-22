using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BinaryTree
{
    public static TreeNode RootNode;
    public static List<BoundsInt> RoomTraversalOrder = new List<BoundsInt>();

    // Insert room to the tree
    public static void Insert(BoundsInt roomData)
    {
        RootNode = InsertRecursively(RootNode, roomData);
    }

    private static TreeNode InsertRecursively(TreeNode rootNode, BoundsInt roomData)
    {
        // base case, if the rootNode is null then create a new TreeNode
        if (rootNode == null)
        {
            return new TreeNode(roomData);
        }

        // check for position of the room, if x of room < x of rootNode then the room is the leftNode of rootNode
        if (roomData.position.x < rootNode.RoomData.position.x)
        {
            rootNode.LeftNode = InsertRecursively(rootNode.LeftNode, roomData);
        }

        else if (roomData.position.y < rootNode.RoomData.position.y)
        {
            rootNode.LeftNode = InsertRecursively(rootNode.LeftNode, roomData);
        }

        else if (roomData.position.y > rootNode.RoomData.position.y)
        {
            rootNode.RightNode = InsertRecursively(rootNode.RightNode, roomData);
        }

        else if (roomData.position.x > rootNode.RoomData.position.x)
        {
            rootNode.RightNode = InsertRecursively(rootNode.RightNode, roomData);
        }

        return rootNode;
    }

    public static List<BoundsInt> InorderTraversal(TreeNode rootNode)
    {
        // base case, when the rootNode is null means the leaf node has been reached in previous interation
        if (rootNode == null)
        {
            return RoomTraversalOrder;
        }

        // traversal left first
        InorderTraversal(rootNode.LeftNode);
        RoomTraversalOrder.Add(rootNode.RoomData);
        InorderTraversal(rootNode.RightNode);

        return RoomTraversalOrder;
    }

    public static void Reset()
    {
        RootNode = null;
        RoomTraversalOrder.Clear();
    }
}
