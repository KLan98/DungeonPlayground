using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DevHotBarOption : MonoBehaviour
{
    private TMP_Text text;
    [SerializeField] private GameEvent gameEvent;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        text.textWrappingMode = TextWrappingModes.Normal;
        //Debug.Log(index);
        text.text = gameEvent.name.ToString();
        text.alignment = TextAlignmentOptions.Center;
    }

    public void TriggerGameEvent()
    {
        //gameEvent.Raise();
    }
}