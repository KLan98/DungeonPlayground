using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HotBarOption : MonoBehaviour
{
    private TMP_Text text;
    [SerializeField] private GameEvent toggleSkillCursor;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        int index = this.transform.GetSiblingIndex() + 1;
        if (index == 10)
        {
            index = 0;
        }

        Debug.Log(index);
        text.text = index.ToString(); 
        text.alignment = TextAlignmentOptions.TopLeft;
    }

    public void TriggerToggleSkillCursor()
    {
        toggleSkillCursor.Raise();
    }
}
