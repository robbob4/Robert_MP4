// ---------------------- ScoreScreenScript.cs --------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - April 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation of the totalScore screen displayed between level 
// changes to tell the player how well they performed.
// ----------------------------------------------------------------------------
// Notes - Assumes stars are named Star1 to Star3.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreenScript : MonoBehaviour
{
    #region Variables
    public const float SCENE_DURATION = 6.0f; // in seconds
    [SerializeField] private string nextLevel = "LevelOne";
    [SerializeField] private Sprite newStarSprite = null;

    private int lastScore = 0; //read from global
    private int totalScore = 0; //read from global
    private float lastTime = 0.0f; //read from global
    private int progress = 0; //used to keep track of what stars are checked
    private float SceneStartTime = -1.0f;
    private Text scoreText = null;
    private GameObject[] stars;
    private Tiling tiler = null;
    #endregion

    //Use this for initialization
    void Start ()
    {
        #region References
        scoreText = GameObject.Find("ScoreDisplayText").GetComponent<Text>();
        if (scoreText == null)
            Debug.LogError("ScoreDisplayText not found.");

        if (newStarSprite == null)
            Debug.LogError("newStarSprite not assigned.");

        stars = GameObject.FindGameObjectsWithTag("UI");
        if (stars.Length != 3)
            Debug.LogError("Only " + stars.Length + " stars found.");
        #endregion

        #region Tile the background
        tiler = GetComponent<Tiling>();
        if (tiler == null)
            Debug.LogError("Tiler not found.");
        else //tile the world
            tiler.TileWorld("Prefabs/Tileset/Tile_Roof_", 10.0f, 10.0f, false, false, false, true, 3, 3);
        #endregion

        //set timer
        SceneStartTime = Time.realtimeSinceStartup;

        //read status variables
        lastScore = MenuBehavior.TheGameState.GetLastScore();
        totalScore = MenuBehavior.TheGameState.GetTotalScore();
        lastTime = MenuBehavior.TheGameState.GetLastTime();
        int level = MenuBehavior.TheGameState.GetLastLevel();

        //echo level complete
        scoreText.text = "Level " + level + " complete!";
    }
	
	//Update is called once per frame
	void Update ()
    {
        #region Star progress
        //count down for star 1
        if (progress == 0 && (Time.realtimeSinceStartup - SceneStartTime) > (SCENE_DURATION / 12) )
        {
            if (lastScore / lastTime >= 0.16f) //5 in 30 seconds
                updateStar("Star1");

            progress++;
        }

        //count down for star 2
        if (progress == 1 && (Time.realtimeSinceStartup - SceneStartTime) > (SCENE_DURATION / 6) )
        {
            if (lastScore / lastTime >= 0.25f) //5 in 20 seconds
                updateStar("Star2");

            progress++;
        }

        //count down for star 3
        if (progress == 2 && (Time.realtimeSinceStartup - SceneStartTime) > (SCENE_DURATION / 4) )
        {
            if (lastScore / lastTime >= 0.5f) //5 in 10 seconds
                updateStar("Star3");

            progress++;

            scoreText.text = "You scored " + lastScore + " in " + (int)lastTime + " seconds";
            if (lastScore / lastTime >= 0.5f)
                scoreText.text += "!\n";
            else
                scoreText.text += ".\n";
            scoreText.text += "Total score is now " + totalScore + ".";
        }
        #endregion

        //count down before changing to next level
        if ((Time.realtimeSinceStartup - SceneStartTime) > SCENE_DURATION)
            LoadScene(nextLevel);

        #region Escape to Main Menu
        //if (progress > 2 && Input.GetKey(KeyCode.Escape))
        if (progress > 2 && Input.GetAxis("Cancel") == 1) 
        {
            LoadScene("MainMenu");
            MenuBehavior.TheGameState.ResetAll(); //start a new game
        }
            
        #endregion
    }

    //update the star
    private void updateStar(string starName)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i].name == starName)
            {
                stars[i].GetComponent<Image>().sprite = newStarSprite;
                stars[i].GetComponent<AudioSource>().Play();
                break;
            } 
        }
    }

    //Change scene support
    void LoadScene(string theLevel)
    {
        SceneManager.LoadScene(theLevel);
        MenuBehavior.TheGameState.SetCurrentLevel(theLevel);
    }
}
