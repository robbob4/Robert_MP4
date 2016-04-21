using UnityEngine;
using System.Collections;

//Loads tilesets from file using path
public class Tiling : MonoBehaviour
{
    #region Tileset variables
    [SerializeField] private string path = "Prefabs/Tileset/";
    [SerializeField] private float width;
    [SerializeField] private float height;

    [HideInInspector] public GameObject tileToSpawn = null;
    private int delay = 1;
    #endregion

    private GlobalBehavior globalBehavior = null;

    // Use this for initialization
    void Start ()
    {
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
        if (globalBehavior == null)
            Debug.LogError("GameManager not found.");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (delay == 0)
            tileWorld();
        else
            delay--;
    }

    void tileWorld()
    {
        Vector3 max = globalBehavior.WorldMax;
        Vector3 min = globalBehavior.WorldMin;

        Debug.Log((int)min.x + " " + max.x + (0 + (int)width));
        Debug.Log((int)min.y + " " + max.y + (0 + (int)height));

        //span the viewable space
        for (int x = (int)min.x; x < max.x; x += (int)width)
        {
            for (int y = (int)min.y; y < max.y; y += (int)height)
            {
                Debug.Log(x + "," + y);
                //todo randomization
                //determine the tile
                if (y >= (int)max.y + height / 2 && x >= (int)max.x + width / 2)
                    tileToSpawn = Resources.Load(path + "BR") as GameObject;
                else if (y == (int)min.y && x >= (int)max.x + width / 2)
                    tileToSpawn = Resources.Load(path + "TR") as GameObject;
                else if (y >= (int)min.y + height / 2 && x == (int)min.x)
                    tileToSpawn = Resources.Load(path + "BL") as GameObject;
                else if (y == (int)min.y && x == (int)min.x)
                    tileToSpawn = Resources.Load(path + "TL") as GameObject;
                else if (y == (int)min.y)
                    tileToSpawn = Resources.Load(path + "T") as GameObject;
                else if (y >= (int)max.y + height / 2)
                    tileToSpawn = Resources.Load(path + "B") as GameObject;
                else
                    tileToSpawn = Resources.Load(path + "B") as GameObject;

                //break if missing item
                if (tileToSpawn == null)
                {
                    Debug.LogError("Tile missing at (" + x + ", " + y);
                    return;
                }

                //place the tile
                GameObject tile = (GameObject)Instantiate(tileToSpawn);
                tile.transform.position = new Vector3(x + width / 2, y + height / 2);
            }
        }
    }
}
