using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventChannel", menuName = "Events/GameObjectChannel")]
public class GameObjectChannel : EventChannel<GameObject>
{
    public GameObject gameObject;
}
