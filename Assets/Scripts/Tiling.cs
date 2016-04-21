// ------------------------------- Tiling.cs ----------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 21, 2016
// Modified - April 21, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a means to apply tiles to the world randomly.
// Process starts at world min and works upward along the y axis first. 
// Automatically loads the prefabs in the path specified.
// ----------------------------------------------------------------------------
// Notes - Assumes prefabs end with names like TR, BR, B, T for things like
// top right, bottom right, bottom, top designations.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

//Loads tilesets from file using path
public class Tiling : MonoBehaviour
{
    #region Tileset variables
    [SerializeField] private string path = "Prefabs/Tileset/";
    [SerializeField] private float width = 10;
    [SerializeField] private float height = 10;
    [SerializeField] private bool randomFlipX = false;
    [SerializeField] private bool randomFlipY = false;

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
        {
            tileWorld();
            delay--;
        }
            
        else if (delay > 0)
            delay--;
    }

    void tileWorld()
    {
        Vector3 max = globalBehavior.WorldMax;
        Vector3 min = globalBehavior.WorldMin;
        Vector3 curPos = globalBehavior.WorldMin;

        //Debug.Log(min.x + " " + max.x + " " + (0 + (int)width));
        //Debug.Log(min.y + " " + max.y + " " + (0 + (int)height));

        int count = 0;

        //span the viewable space
        for (; curPos.x < max.x; curPos.x += (int)width)
        {
            for (; curPos.y < max.y; curPos.y += (int)height)
            {
                //determine the tile
                //if (curPos.y >= (int)max.y + height / 2 && curPos.x >= (int)max.x + width / 2)
                //    tileToSpawn = Resources.Load(path + "BR") as GameObject;
                //else if (curPos.y == (int)min.y && curPos.x >= (int)max.x + width / 2)
                //    tileToSpawn = Resources.Load(path + "TR") as GameObject;
                //else if (curPos.y >= (int)min.y + height / 2 && curPos.x == (int)min.x)
                //    tileToSpawn = Resources.Load(path + "BL") as GameObject;
                //else if (curPos.y == (int)min.y && curPos.x == (int)min.x)
                //    tileToSpawn = Resources.Load(path + "TL") as GameObject;
                //else if (curPos.y == (int)min.y)
                //    tileToSpawn = Resources.Load(path + "T") as GameObject;
                //else if (curPos.y >= (int)max.y + height / 2)
                //    tileToSpawn = Resources.Load(path + "B") as GameObject;
                //else
                //    tileToSpawn = Resources.Load(path + "B") as GameObject;

                int rand = Mathf.RoundToInt(Random.Range(0, 2));
                if(rand == 0)
                    tileToSpawn = Resources.Load(path + "B") as GameObject;
                else
                    tileToSpawn = Resources.Load(path + "T") as GameObject;

                //break if missing item
                if (tileToSpawn == null)
                {
                    Debug.LogError("Tile missing at (" + curPos.x + ", " + curPos.y + ")");
                    return;
                }

                //place the tile
                GameObject tile = (GameObject)Instantiate(tileToSpawn);
                tile.transform.position = new Vector3(curPos.x + width / 2, curPos.y + height / 2);
                tile.GetComponent<SpriteRenderer>().sortingOrder = count++;

                //randomize flipping
                if(randomFlipX)
                {
                    rand = Mathf.RoundToInt(Random.Range(0, 2));
                    if (rand == 0)
                        tile.GetComponent<SpriteRenderer>().flipX = true;
                }
                if (randomFlipY)
                {
                    rand = Mathf.RoundToInt(Random.Range(0, 2));
                    if (rand == 0)
                        tile.GetComponent<SpriteRenderer>().flipY = true;
                }
            }

            curPos.y = min.y;
        }
    }
}
