using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics.Tracing;

[CustomEditor(typeof(GameObjectChannel))]
public class GameObjectChannelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameObjectChannel channel = (GameObjectChannel)target;
        if(GUILayout.Button("Raise"))
        {
            channel.RaiseEvent(channel.gameObject);
        }
    }
}
