using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Main menu buttons")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button characterButton;
    [SerializeField] Button inventoryButton;
    [SerializeField] Button controlsButton;
    [SerializeField] Button quitButton;
    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject characterMenu;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] GameObject controlsMenu;
    public Canvas canvas { get; private set; }
    public static MenuController Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        canvas = GetComponent<Canvas>();
        resumeButton.onClick.AddListener(ResumeGame);
        characterButton.onClick.AddListener(OpenCharacterMenu);
        inventoryButton.onClick.AddListener(OpenInventoryMenu);
        controlsButton.onClick.AddListener(OpenControlsMenu);
        quitButton.onClick.AddListener(Quit);
    }

    private void Start()
    {
        InputManager.Instance.inputActions.UI.Cancel.performed += OnCancelInput;
    }

    #region MenuFunctions
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        CloseAll();
    }
    public void CloseAll()
    {
        mainMenu.SetActive(false);
        characterMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }
    public void OpenMainMenu()
    {
        CloseAll();
        mainMenu.SetActive(true);
    }
    public void OpenCharacterMenu()
    {
        CloseAll();
        characterMenu.SetActive(true);
    }
    public void OpenInventoryMenu()
    {
        CloseAll();
        inventoryMenu.SetActive(true);
    }
    public void OpenControlsMenu()
    {
        CloseAll();
        controlsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void OnCancelInput(InputAction.CallbackContext callbackContext)
    {
        if (characterMenu.activeSelf || inventoryMenu.activeSelf || controlsMenu.activeSelf)
        {
            CloseAll();
            mainMenu.SetActive(true);
        }else if (mainMenu.activeSelf)
        {
            CloseAll();
            ResumeGame();
        }
        else
        {
            PauseGame();
            OpenMainMenu();
        }
    }
    #endregion
}
