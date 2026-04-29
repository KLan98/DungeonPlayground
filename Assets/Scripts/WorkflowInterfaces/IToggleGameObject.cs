using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IToggleGameObject
{
    bool IsActive { get; set; }

    void ToggleActive();
}