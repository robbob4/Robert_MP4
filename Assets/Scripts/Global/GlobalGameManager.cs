// -------------------------- GlobalGameManager.cs ----------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - April 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a game manager that persists through level 
// changes. Handles passing a totalScore, lastScore, lastTime, and 
// lastEnemyCount to the next level. Also maintains a string for the current 
// level.
// ----------------------------------------------------------------------------
// Notes - Created by MenuBehavior and referenced from there as 
// MenuBehavior.TheGameState.
// ----------------------------------------------------------------------------

using UnityEngine;
//using UnityEngine.SceneManagement; // for SceneManager
using System.Collections;

public class GlobalGameManager : MonoBehaviour
{
    #region Saved variables
    private string currentLevel = "MenuLevel";
    private int lastScore = 0;
    private int totalScore = 0;
    private float lastTime = 0.0f;
    private int lastEnemyCount = 0;
    private int lastLevel = 0;
    #endregion

    //Use this for initialization
    void Start ()
    {
        //prevent destruction
		DontDestroyOnLoad(this);
	}

    #region CurrentLevel
    //Use this to specify the current level
    public void SetCurrentLevel(string level)
    {
        currentLevel = level;
        //Debug.Log("Changed to " + currentLevel);
        //Debug.Log("lastScore: " + lastScore + " totalScore: " + totalScore + " lastTime: " + lastTime + 
        //    " lastEnemyCount: " + lastEnemyCount + " lastLevel: " + lastLevel);
    }

    //Used to return the current level
    public string GetCurrentLevel()
    {
        return currentLevel;
    }
    #endregion

    #region TotalScore
    //Use this to specify the totalScore
    public void SetTotalScore(int newTotalScore)
    {
        totalScore = newTotalScore;
    }

    //Used to return the totalScore
    public int GetTotalScore()
    {
        return totalScore;
    }
    #endregion

    #region LastScore
    //Use this to specify the totalScore
    public void SetLastScore(int score)
    {
        lastScore = score;
    }

    //Used to return the totalScore
    public int GetLastScore()
    {
        return lastScore;
    }
    #endregion

    #region LastEnemyCount
    //Use this to specify the lastEnemyCount
    public void SetLastEnemyCount(int count)
    {
        lastEnemyCount = count;
    }

    //Used to return the lastEnemyCount
    public int GetLastEnemyCount()
    {
        return lastEnemyCount;
    }
    #endregion

    #region LastLevel
    //Use this to specify the lastLevel
    public void SetLastLevel(int lvl)
    {
        lastLevel = lvl;
    }

    //Used to return the lastLevel
    public int GetLastLevel()
    {
        return lastLevel;
    }
    #endregion

    #region LastTime
    //Use this to specify the lastTime
    public void SetLastTime(float newTime)
    {
        lastTime = newTime;
    }

    //Used to return the lastTime
    public float GetLastTime()
    {
        return lastTime;
    }
    #endregion

    #region Reset all
    //used to reset all saved variables for a new game
    public void ResetAll()
    {
        lastScore = 0;
        totalScore = 0;
        lastTime = 0.0f;
        lastEnemyCount = 0;
        lastLevel = 0;
    }
    #endregion
}
