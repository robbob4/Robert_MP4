// ------------------------------- Tiling.cs ----------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 21, 2016
// Modified - April 25, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a means to apply tiles to the world randomly.
// Process starts at world min and works upward along the y axis first. 
// Automatically loads the prefabs in the path specified.
// ----------------------------------------------------------------------------
// Notes - Assumes prefabs end with names like TL, TM, TR, ML, MM, MR, BL, BM, 
// BR for things like top right, bottom right, etc. Vertical component must 
// come first (unless tilesHigh = 0).
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

//Loads tilesets from file using path
public class Tiling : MonoBehaviour
{
    #region Tileset variables
    [SerializeField] private string path = "Prefabs/Tileset/";
    [SerializeField] private float width = 10.0f;
    [SerializeField] private float height = 10.0f;
    [SerializeField] private bool randomFlipX = false;
    [SerializeField] private bool randomFlipY = false;
    [SerializeField] private bool randomizeMode = true;
    [SerializeField] private int tilesHigh = 3; //0-3 changes what tiles to load
    [SerializeField] private int tilesWide = 3; //0-3 changes what tiles to load
    #endregion

    #region References
    [HideInInspector] public GameObject tileToSpawn = null;
    private WorldBound sceneBoundary = null;
    #endregion

    void Awake()
    {
        sceneBoundary = GameObject.Find("GameManager").GetComponent<WorldBound>();
        if (sceneBoundary == null)
        {
            Debug.LogError("GameManager's WorldBound not found for " + this + ".");
            Application.Quit();
        }
    }

    #region Tiling
    // Actually place the tiles
    public void TileWorld()
    {
        Vector3 max = sceneBoundary.WorldMax;
        Vector3 min = sceneBoundary.WorldMin;
        Vector3 curPos = sceneBoundary.WorldMin;

        //Debug.Log(min.x + " " + max.x + " " + (0 + (int)width));
        //Debug.Log(min.y + " " + max.y + " " + (0 + (int)height));

        int count = 0;

        //span the viewable space starting at bottom left going up then right
        for (; curPos.x < max.x; curPos.x += (int)width)
        {
            for (; curPos.y < max.y; curPos.y += (int)height)
            {
                string tempPath = path;
                int rand = 0;
                
                if (randomizeMode)
                {
                    rand = Mathf.RoundToInt(Random.Range((Mathf.RoundToInt(tilesHigh / 3.0f) - 1), (tilesHigh))); //-1-2
                    if (rand == 0)
                        tempPath += "T";
                    else if (rand == 1)
                        tempPath += "B";
                    else if (rand == 2)
                        tempPath += "M";

                    rand = Mathf.RoundToInt(Random.Range((Mathf.RoundToInt(tilesWide / 3.0f) - 1), (tilesWide))); //-1-2
                    if (rand == 0)
                        tempPath += "L";
                    else if (rand == 1)
                        tempPath += "R";
                    else if (rand == 2)
                        tempPath += "M";
                }
                else
                {
                    //TODO: fix this
                    if (curPos.y >= (int)max.y + height / 2 && curPos.x >= (int)max.x + width / 2)
                        tempPath += "BR";
                    else if (curPos.y == (int)min.y && curPos.x >= (int)max.x + width / 2)
                        tempPath += "TR";
                    else if (curPos.y >= (int)min.y + height / 2 && curPos.x == (int)min.x)
                        tempPath += "BL";
                    else if (curPos.y == (int)min.y && curPos.x == (int)min.x)
                        tempPath += "TL";
                    else if (curPos.y == (int)min.y)
                        tempPath += "T";
                    else if (curPos.y >= (int)max.y + height / 2)
                        tempPath += "B";
                    else
                        tempPath += "B";
                }

                tileToSpawn = Resources.Load(tempPath) as GameObject;

                //break if missing item
                if (tileToSpawn == null)
                {
                    Debug.LogError("Tile " + tempPath + "missing (" + curPos.x + ", " + curPos.y + ")");
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
    #endregion

    #region Overloads
    // Overloaded for a path, width, and height change
    public void TileWorld(string pathChange, float w, float h)
    {
        path = pathChange;
        width = w;
        height = h;
        TileWorld();
    }

    // Overloaded for a path, width, height, flipX, and flipY change
    public void TileWorld(string pathChange, float w, float h, bool x, bool y)
    {
        path = pathChange;
        width = w;
        height = h;
        randomFlipX = x;
        randomFlipY = y;
        TileWorld();
    }

    // Overloaded for a path, width, height, flipX, flipY, randomizeMode, tilesHigh, and tilesWide change
    public void TileWorld(string pathChange, float w, float h, bool x, bool y, bool random, int high, int wide)
    {
        path = pathChange;
        width = w;
        height = h;
        randomFlipX = x;
        randomFlipY = y;
        randomizeMode = random;
        tilesHigh = Mathf.Clamp(high, 0, 3);
        tilesWide = Mathf.Clamp(wide, 0, 3);
        TileWorld();
    }
    #endregion
}
