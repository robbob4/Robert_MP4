// -------------------------- ReturnToMain.cs ---------------------------------
// Author - Samuel Williams CSS 385
// Author - Robert Griswold CSS 385
// Created - Apr 25, 2016
// Modified - April 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation to return to the main menu.
// ----------------------------------------------------------------------------
// Notes - Script for credits scene to press ESC key to return to MainMenuScene
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMain : MonoBehaviour
{
	//Use this for initialization
	void Start ()
    {
	
	}
	
	//Update is called once per frame
	void Update ()
    {
		//if (Input.GetKey (KeyCode.Escape))
        if (Input.GetAxis("Cancel") == 1)
        {
			SceneManager.LoadScene("MainMenu");
			MenuBehavior.TheGameState.SetCurrentLevel("MainMenu");
		}
	}
}
