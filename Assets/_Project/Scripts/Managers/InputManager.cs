using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public PlayerInputActions inputActions { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        inputActions = new PlayerInputActions();
    }

    private void Start()
    {
        inputActions.CharacterControls.Enable();
        inputActions.UI.Enable();
    }
}
