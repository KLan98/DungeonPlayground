using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Background : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Demo demo;
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 clickPoint = eventData.position;
        Debug.Log($"Clicked on index {demo.grid.GetCellIndex(clickPoint)}");
    }
}
