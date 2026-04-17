using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DevController : MonoBehaviour
{
    private DevInputActions actions;
    [SerializeField] private GameEvent toggleDevHotBar;

    private void Awake()
    {
        actions = new DevInputActions();
        actions.Enable();
    }

    private void OnEnable()
    {
        actions.DevActions.ToggleDevHotBar.performed += ToggleDevHotBar;
    }

    private void OnDisable()
    {
        actions.DevActions.ToggleDevHotBar.performed -= ToggleDevHotBar;
    }

    private void ToggleDevHotBar(InputAction.CallbackContext context)
    {
        toggleDevHotBar.Raise();
    }
}
