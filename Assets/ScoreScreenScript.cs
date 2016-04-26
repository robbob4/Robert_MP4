using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreenScript : MonoBehaviour {

	private Text scoreText = null;
	public const float SceneDurationTime = 5.0f; // in seconds
	private float SceneStartTime = -1.0f;


	// Use this for initialization
	void Start () {
		scoreText = GameObject.Find("ScoreDisplayText").GetComponent<Text>();
		scoreText.text = "SCORE TEXT";
		SceneStartTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.realtimeSinceStartup - SceneStartTime) > SceneDurationTime)
		{
			SceneManager.LoadScene("MP3");
			MenuBehavior.TheGameState.SetCurrentLevel("MP3");
		}
	}
}
