using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevHotBarController : MonoBehaviour
{
    public void ToggleDevHotBar()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }

        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
