// ---------------------- ScoreScreenScript.cs --------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - April 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation of the totalScore screen displayed between level changes
// to tell the player how well they performed.
// ----------------------------------------------------------------------------
// Notes - Continues to specified level after a specified time.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreenScript : MonoBehaviour
{
    public const float SCENE_DURATION = 5.0f; // in seconds
    public string nextLevel = "MP3";
    private Text scoreText = null;
	private float SceneStartTime = -1.0f;

	//Use this for initialization
	void Start ()
    {
		scoreText = GameObject.Find("ScoreDisplayText").GetComponent<Text>();
		SceneStartTime = Time.realtimeSinceStartup;

        //update status
        int lastScore = MenuBehavior.TheGameState.GetLastScore();
        int totalScore = MenuBehavior.TheGameState.GetTotalScore();
        float lastTime = MenuBehavior.TheGameState.GetLastTime();
        scoreText.text = "You scored " + lastScore + " in " + (int)lastTime + " seconds.\n"
            + "Total Score: " + totalScore + ".";
    }
	
	//Update is called once per frame
	void Update ()
    {
        //count down before changing to next level
		if ((Time.realtimeSinceStartup - SceneStartTime) > SCENE_DURATION)
		{
			SceneManager.LoadScene(nextLevel);
			MenuBehavior.TheGameState.SetCurrentLevel(nextLevel);
		}
	}
}
