using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// for SceneManager

public class MenuBehavior : MonoBehaviour {

	private static GlobalGameManager globalGameManager = null;

	public string LevelName = null;

	public Button mLevelOneButton;
	public Button mLevelTwoButton;

	// Use this for initialization
	void Start () {
		
		mLevelOneButton = GameObject.Find("ButtonLevel1").GetComponent<Button>();
		mLevelTwoButton = GameObject.Find("ButtonCredits").GetComponent<Button>();

		mLevelOneButton.onClick.AddListener(ButtonOneService);
		mLevelTwoButton.onClick.AddListener(ButtonTwoService);
	}

	#region Button service function
	private void ButtonOneService() {
		Debug.Log("Button 1");
		LoadScene("MP3");
	}
	#endregion


	#region Button service function
	private void ButtonTwoService() {
		Debug.Log("Button 2");
		LoadScene("Credits");
	}
	#endregion

	// Update is called once per frame
	void Update () {

	}

	void LoadScene(string theLevel) {
		SceneManager.LoadScene(theLevel);
		globalGameManager.SetCurrentLevel(theLevel);
		//GlobalGameManager.TheGameState.PrintCurrentLevel();
	}
		
	private static void CreateGlobalManager()
	{
		GameObject newGameState = new GameObject();
		newGameState.name = "GlobalStateManager";
		newGameState.AddComponent<GlobalGameManager>();
		globalGameManager = newGameState.GetComponent<GlobalGameManager>();
	}

	public static GlobalGameManager TheGameState
	{
		get
		{
			// going to assume allready created.
			// Otherwise should do:
			//
			if (globalGameManager == null)
				CreateGlobalManager ();
			//
			// before the return statement
			return globalGameManager;
		}
	}

	// Called even if the script component is not enableds
	void Awake() {
		if (null == globalGameManager) { // not here yet
			CreateGlobalManager();
			Debug.Log("Creating Global Manager!");
		}
	}



}                                                                                                                                                