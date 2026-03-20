using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    private TreeNode leftNode;
    private TreeNode rightNode;
    private BoundsInt roomData;

    public TreeNode LeftNode
    {
        get { return leftNode; }
        set { leftNode = value; }
    }

    public TreeNode RightNode
    {
        get { return rightNode; }
        set {  rightNode = value; }
    }

    public BoundsInt RoomData
    {
        get { return roomData; }
    }

    public TreeNode(BoundsInt roomData, TreeNode leftNode = null, TreeNode rightNode = null)
    {
        this.roomData = roomData;
        this.leftNode = leftNode;
        this.rightNode = rightNode;
    }
}
