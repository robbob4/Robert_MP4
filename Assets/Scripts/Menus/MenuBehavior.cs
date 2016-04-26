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
    public Button LevelOneButton;
    public Button LevelTwoButton;

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
        LevelOneButton = GameObject.Find("ButtonLevel1").GetComponent<Button>();
        if (LevelOneButton == null)
            Debug.LogError("ButtonLevel1 not found.");
        LevelTwoButton = GameObject.Find("ButtonCredits").GetComponent<Button>();
        if (LevelTwoButton == null)
            Debug.LogError("ButtonCredits not found.");

        //add listeners to the buttons
        LevelOneButton.onClick.AddListener(ButtonOneService);
        LevelTwoButton.onClick.AddListener(ButtonTwoService);
        #endregion

        #region Tile the background
        tiler = GetComponent<Tiling>();
        if (tiler == null)
            Debug.LogError("Tiler not found.");
        else //tile the world
            tiler.TileWorld("Prefabs/Tileset/Tile_White_", 11.8f, 11.8f, false, false);
        #endregion
    }

    //Update is called once per frame
    void Update()
    {

    }

    #region Button service functions
    private void ButtonOneService()
    {
		LoadScene("MP3");
	}

	private void ButtonTwoService()
    {
		LoadScene("Credits");
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