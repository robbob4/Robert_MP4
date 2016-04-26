// ----------------------------- MenuBehavior.cs ------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - April 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for main menu behavior where the buttons are used 
// to change scenes. The globalGameManager is created here that survives 
// transitions to new levels.
// ----------------------------------------------------------------------------
// Notes - Script used in Main Menu.
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement; //for SceneManager
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour
{
    #region Variables
    public bool Movement = true; //bool for movement of enemies
    [HideInInspector] public Button StartButton;
    [HideInInspector] public Button CreditsButton;
    [HideInInspector] public Button QuitButton;

    private GameObject[] menuElements;
    private static GlobalGameManager globalGameManager = null;
    private Tiling tiler = null;
    #endregion

    // Called even if the script component is not enableds
    void Awake()
    {
        if (null == globalGameManager)
        {
            CreateGlobalManager();
        }
    }

    // Use this for initialization
    void Start ()
    {
        #region Button init
        //find buttons
        StartButton = GameObject.Find("ButtonStart").GetComponent<Button>();
        if (StartButton == null)
            Debug.LogError("ButtonStart not found.");
        CreditsButton = GameObject.Find("ButtonCredits").GetComponent<Button>();
        if (CreditsButton == null)
            Debug.LogError("ButtonCredits not found.");
        QuitButton = GameObject.Find("ButtonQuit").GetComponent<Button>();
        if (QuitButton == null)
            Debug.LogError("ButtonQuit not found.");

        //add listeners to the buttons
        StartButton.onClick.AddListener(StartButtonService);
        CreditsButton.onClick.AddListener(CreditsButtonService);
        QuitButton.onClick.AddListener(QuitButtonService);
        #endregion

        #region Hide credits
        menuElements = GameObject.FindGameObjectsWithTag("UI");

        for (int i = 0; i < menuElements.Length; i++)
        {
            if (menuElements[i].name == "Credits")
                menuElements[i].SetActive(false);
        }
        #endregion

        #region Tile the background
        tiler = GetComponent<Tiling>();
        if (tiler == null)
            Debug.LogError("Tiler not found.");
        else //tile the world
            tiler.TileWorld("Prefabs/Tileset/Tile_Roof_", 11.8f, 11.8f, false, false);
        #endregion
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //show buttons and props but no credits
            
            for (int i = 0; i < menuElements.Length; i++)
            {
                if (menuElements[i].name == "Credits")
                    menuElements[i].SetActive(false);
                else
                    menuElements[i].SetActive(true);
            }
        }
    }

    #region Button service functions
    private void StartButtonService()
    {
		LoadScene("LevelOne");
	}

	private void CreditsButtonService()
    {
        //hide buttons and props but show credits
        for (int i = 0; i < menuElements.Length; i++)
        {
            if(menuElements[i].name == "Credits")
                menuElements[i].SetActive(true);
            else
                menuElements[i].SetActive(false);
        }
	}

    private void QuitButtonService()
    {
        Application.Quit(); //only works in build - not debug
    }
    #endregion

    //Change scene support
    void LoadScene(string theLevel)
    {
		SceneManager.LoadScene(theLevel);
		globalGameManager.SetCurrentLevel(theLevel);
	}

    #region GlobalGameManager functions
    private static void CreateGlobalManager()
    {
        GameObject newGameState = new GameObject();
        newGameState.name = "GlobalStateManager";
        newGameState.AddComponent<GlobalGameManager>();
        globalGameManager = newGameState.GetComponent<GlobalGameManager>();
        globalGameManager.SetCurrentLevel("MainMenu");
    }

    public static GlobalGameManager TheGameState
    {
        get
        {
            //create game manager if it somehow doesn't exist
            if (globalGameManager == null)
                CreateGlobalManager();

            return globalGameManager;
        }
    }
    #endregion
}