// -------------------------- GlobalBehavior.cs -------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 19, 2016
// Modified - April 25, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a global controller class that handles enemy 
// spawning, score, status, and boundaries.
// ----------------------------------------------------------------------------
// Notes - Objects can be bound to this parent's camera boundary using 
// ClampToWorld(transform).
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GlobalBehavior : MonoBehaviour {

    #region World Bound support
    private Bounds worldBounds;  // this is the world bound
	private Vector2 worldMin;	// Better support 2D interactions
	private Vector2 worldMax;
	private Vector2 worldCenter;
	private Camera mainCamera;
    #endregion

    #region Support for runtime enemy creation
    public const float ENEMY_SPAWN_INTERVAL = 3.0f; // in seconds
    public bool Movement = false; //bool for movement and spawning of enemies
    [SerializeField] private int initialSpawn = 50;

    [HideInInspector] public GameObject EnemyToSpawn = null;
    private float preSpawnTime = -1.0f;
    #endregion

    #region Status variables
    private Text scoreText = null;
    private Text statusText = null;
    public int Score = 0;
    #endregion

    #region Tiling variables
    Tiling tiler = null;
    #endregion

    // Use this for initialization
    void Start () {
        #region World bound support
        mainCamera = Camera.main;
        worldBounds = new Bounds(Vector3.zero, Vector3.one);
        UpdateWorldWindowBound();
        #endregion

        #region Enemy spawning
        if (EnemyToSpawn == null)
            EnemyToSpawn = Resources.Load("Prefabs/Enemy") as GameObject;
        if (EnemyToSpawn == null)
            Debug.LogError("Enemy not found.");

        // first 50 enemies
        for (int i = 0; i < initialSpawn; i++)
            SpawnAnEnemy(true);
        #endregion

        #region Other references
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        if (scoreText == null)
            Debug.LogError("Score not found.");
        statusText = GameObject.Find("Status").GetComponent<Text>();
        if (statusText == null)
            Debug.LogError("Status not found.");

        tiler = GameObject.Find("Tiler").GetComponent<Tiling>();
        if (tiler == null)
            Debug.LogError("Tiler not found.");
        else //tile the world
            tiler.TileWorld("Prefabs/Tileset/Tile_Wood_", 11.8f, 11.8f, false, false);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //spawn enemies perodically if movement enabled
        if (Movement)
            SpawnAnEnemy(false);

        //update status UI text
        if (scoreText != null)
            updateScore();
        if (statusText != null)
            updateStatus();
    }

    #region Game Window World size bound support
    public enum WorldBoundStatus {
		CollideTop,
		CollideLeft,
		CollideRight,
		CollideBottom,
		Outside,
		Inside
	};
	
	/// <summary>
	/// This function must be called anytime the MainCamera is moved, or changed in size
	/// </summary>
	public void UpdateWorldWindowBound()
	{
		// get the main 
		if (null != mainCamera) {
			float maxY = mainCamera.orthographicSize;
			float maxX = mainCamera.orthographicSize * mainCamera.aspect;
			float sizeX = 2 * maxX;
			float sizeY = 2 * maxY;
			float sizeZ = Mathf.Abs(mainCamera.farClipPlane - mainCamera.nearClipPlane);
			
			// Make sure z-component is always zero
			Vector3 c = mainCamera.transform.position;
			c.z = 0.0f;
			worldBounds.center = c;
			worldBounds.size = new Vector3(sizeX, sizeY, sizeZ);

			worldCenter = new Vector2(c.x, c.y);
			worldMin = new Vector2(worldBounds.min.x, worldBounds.min.y);
			worldMax = new Vector2(worldBounds.max.x, worldBounds.max.y);
		}
	}

	public Vector2 WorldCenter { get { return worldCenter; } }
	public Vector2 WorldMin { get { return worldMin; }} 
	public Vector2 WorldMax { get { return worldMax; }}
	
	public WorldBoundStatus ObjectCollideWorldBound(Bounds objBound)
	{
		WorldBoundStatus status = WorldBoundStatus.Inside;

		if (worldBounds.Intersects (objBound)) {
			if (objBound.max.x > worldBounds.max.x)
				status = WorldBoundStatus.CollideRight;
			else if (objBound.min.x < worldBounds.min.x)
				status = WorldBoundStatus.CollideLeft;
			else if (objBound.max.y > worldBounds.max.y)
				status = WorldBoundStatus.CollideTop;
			else if (objBound.min.y < worldBounds.min.y)
				status = WorldBoundStatus.CollideBottom;
			else if ((objBound.min.z < worldBounds.min.z) || (objBound.max.z > worldBounds.max.z))
				status = WorldBoundStatus.Outside;
		} else 
			status = WorldBoundStatus.Outside;

		return status;
	}

    //clamps a transform to a world with an additional buffer
    public void ClampToWorld(Transform pos, float buffer)
    {
        //x
        if (pos.position.x + buffer > WorldMax.x)
            pos.position = new Vector3(WorldMax.x - buffer, pos.position.y, pos.position.z);
        else if (pos.position.x - buffer < WorldMin.x)
            pos.position = new Vector3(WorldMin.x + buffer, pos.position.y, pos.position.z);

        //y
        if (pos.position.y + buffer > WorldMax.y)
            pos.position = new Vector3(pos.position.x, WorldMax.y - buffer, pos.position.z);
        else if (pos.position.y - buffer < WorldMin.y)
            pos.position = new Vector3(pos.position.x, WorldMin.y + buffer, pos.position.z);
    }
    #endregion

    #region enemy spawning support
    //spawns an enemy if within allowed spawning time unless overridden with a bool
    private void SpawnAnEnemy(bool ignore)
    {
        if (EnemyToSpawn == null)
            return;

        if ((Time.realtimeSinceStartup - preSpawnTime) > ENEMY_SPAWN_INTERVAL || ignore)
        {
            GameObject e = (GameObject)Instantiate(EnemyToSpawn);

            float randX = Random.Range(WorldMin.x + 5f, WorldMax.x -5f);
            float randY = Random.Range(WorldMin.y +5f, WorldMax.y -5f);
            e.transform.position = new Vector3(randX, randY);

            preSpawnTime = Time.realtimeSinceStartup;
            //Debug.Log("New enemy at: " + preSpawnTime.ToString());
        }
    }
    #endregion

    #region Status functions
    private void updateStatus()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        statusText.text = "Status: " + enemies.Length + " moogle";
        if (enemies.Length != 1)
            statusText.text += "s";
        statusText.text += " " + projectiles.Length + " fireball";
        if (projectiles.Length != 1)
            statusText.text += "s";
    }

    private void updateScore()
    {
        scoreText.text = "Score: " + Score;
    }
    #endregion
}
