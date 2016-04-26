// -------------------------- GlobalBehavior.cs -------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 19, 2016
// Modified - April 25, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a global controller class that handles enemy 
// spawning, totalScore, and status.
// ----------------------------------------------------------------------------
// Notes - ESC key can be pressed to return to main menu.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //scene manager

public class LevelBehavior : MonoBehaviour
{
    #region Support for runtime enemy creation
    public float EnemySpawnInterval = 9.0f; // in seconds
    public bool Movement = true; //bool for movement of enemies
    public bool Spawning = false; //bool for spawning of enemies
    
    [HideInInspector] public GameObject EnemyToSpawn = null;
    private int initialSpawn = 5;
    private float preSpawnTime = -1.0f;
    private WorldBound sceneBoundary = null;
    #endregion

    #region Status variables
    [HideInInspector] public int Score = 0;

    private int totalScore = 0;
    private float timeElapsed = 0.0f;
    private int level = 1;
    private Text scoreText = null;
    private Text statusText = null;
    #endregion

    #region Tiling variables
    Tiling tiler = null;
    #endregion

    // Use this for initialization
    void Start ()
    {

        #region Other references
        sceneBoundary = GameObject.Find("GameManager").GetComponent<WorldBound>();
        if (sceneBoundary == null)
        {
            Debug.LogError("WorldBound not found for levelManager in " + this + ".");
            Application.Quit();
        }

        scoreText = GameObject.Find("Score").GetComponent<Text>();
        if (scoreText == null)
            Debug.LogError("Score not found.");
        statusText = GameObject.Find("Status").GetComponent<Text>();
        if (statusText == null)
            Debug.LogError("Status not found.");

        tiler = GetComponent<Tiling>();
        if (tiler == null)
            Debug.LogError("Tiler not found.");
        else //tile the world
            tiler.TileWorld("Prefabs/Tileset/Tile_Wood_", 11.8f, 11.8f, false, false);
        #endregion

        #region Get data from GlobalGameManager
        totalScore = MenuBehavior.TheGameState.GetTotalScore();
        level = MenuBehavior.TheGameState.GetLastLevel() + 1;
        initialSpawn = MenuBehavior.TheGameState.GetLastEnemyCount() + level * initialSpawn;

        //allow spawning past level 1
        if (level != 1)
            Spawning = true;

        //increase spawn interval by 1s per level
        EnemySpawnInterval += level;
        #endregion

        #region Enemy spawning
        if (EnemyToSpawn == null)
            EnemyToSpawn = Resources.Load("Prefabs/Enemy") as GameObject;
        if (EnemyToSpawn == null)
            Debug.LogError("Enemy not found.");

        // first x enemies
        for (int i = 0; i < initialSpawn; i++)
            SpawnAnEnemy(true);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
		// change scene to main menu if escape key pressed
		if (Input.GetKey (KeyCode.Escape)) {
			LoadScene("MainMenu");
            MenuBehavior.TheGameState.ResetAll(); //start a new game
		}

        //spawn enemies perodically if Spawning enabled
        if (Spawning)
            SpawnAnEnemy(false);

        //update TimeElapsed
        timeElapsed += Time.deltaTime;

        //update status UI text
        if (scoreText != null)
            updateScore();
        if (statusText != null)
            updateStatus();
    }

    #region Enemy spawning support
    //spawns an enemy if within allowed spawning time unless overridden with a bool
    private void SpawnAnEnemy(bool ignore)
    {
        if (EnemyToSpawn == null)
            return;

        if ((Time.realtimeSinceStartup - preSpawnTime) > EnemySpawnInterval || ignore)
        {
            GameObject e = (GameObject)Instantiate(EnemyToSpawn);

            float randX = Random.Range(sceneBoundary.WorldMin.x + 5f, sceneBoundary.WorldMax.x -5f);
            float randY = Random.Range(sceneBoundary.WorldMin.y +5f, sceneBoundary.WorldMax.y -5f);
            e.transform.position = new Vector3(randX, randY);

            preSpawnTime = Time.realtimeSinceStartup;
            //Debug.Log("New enemy at: " + preSpawnTime.ToString());
        }
    }
    #endregion

    #region Status functions
    private void updateStatus()
    {
        //GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        statusText.text = "Status: " + enemies.Length + " moogle";
        if (enemies.Length != 1)
            statusText.text += "s";
        statusText.text += " left!";
        //+ projectiles.Length + " fireball";
        //if (projectiles.Length != 1)
        //    statusText.text += "s";

		if (enemies.Length == 0)
        {
            LoadScene("ScoreScreen");
        }
			
    }

    private void updateScore()
    {
        scoreText.text = "Level: " + level + " Score: " + Score;
    }
    #endregion

	#region Level change support
	void LoadScene(string theLevel)
    {
        //load the scene
		SceneManager.LoadScene(theLevel);
		MenuBehavior.TheGameState.SetCurrentLevel(theLevel);

        //update global variables
        MenuBehavior.TheGameState.SetLastScore(Score);
        MenuBehavior.TheGameState.SetTotalScore(totalScore + Score);
        MenuBehavior.TheGameState.SetLastTime(timeElapsed);
        MenuBehavior.TheGameState.SetLastEnemyCount(initialSpawn);
        MenuBehavior.TheGameState.SetLastLevel(level);
    }
	#endregion
}
