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
// come first (unless tilesHigh = 0). Priority to T/L and then B/R.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

//Loads tilesets from file using path
public class Tiling : MonoBehaviour
{
    #region Tileset variables
    [SerializeField] private string path = "Prefabs/Tileset/";
    [SerializeField] private float height = 10.0f;
    [SerializeField] private float width = 10.0f;
    [SerializeField] private bool randomFlipY = false;
    [SerializeField] private bool randomFlipX = false;
    [SerializeField] private bool randomizeY = false;
    [SerializeField] private bool randomizeX = false;
    [SerializeField] private int tilesHigh = 1; //0-3 changes what tiles to load
    [SerializeField] private int tilesWide = 1; //0-3 changes what tiles to load
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

                //height
                if (randomizeY)
                {
                    rand = Mathf.RoundToInt(Random.Range((Mathf.CeilToInt(tilesHigh / 3.0f) - 1), (tilesHigh))); //-1-2
                    if (rand == 0)
                        tempPath += "T";
                    else if (rand == 1)
                        tempPath += "B";
                    else if (rand == 2)
                        tempPath += "M";

                    
                }
                else
                {
                    if (curPos.y + height >= max.y) //top
                    {
                        if (tilesHigh >= 1)
                            tempPath += "T";
                        //Debug.Log("top at " + curPos.x + "," + curPos.y + " (" + max.x + "," + max.y + ")");
                    }
                    else if (curPos.y - height < -max.y) //bottom
                    {
                        if (tilesHigh >= 2)
                            tempPath += "B";
                        else if (tilesHigh >= 1)
                            tempPath += "T";
                        //Debug.Log("bottom at " + curPos.x + "," + curPos.y + " (" + max.x + "," + max.y + ")");
                    }
                    else //middle
                    {
                        if (tilesHigh >= 3)
                            tempPath += "M";
                        else if (tilesHigh >= 2)
                            tempPath += "B";
                        else if (tilesHigh >= 1)
                            tempPath += "T";
                        //Debug.Log("middle at " + curPos.x + "," + curPos.y + " (" + max.x + "," + max.y + ")");
                    }
                }

                //width
                if (randomizeX)
                {
                    rand = Mathf.RoundToInt(Random.Range((Mathf.CeilToInt(tilesWide / 3.0f) - 1), (tilesWide))); //-1-2
                    if (rand == 0)
                        tempPath += "L";
                    else if (rand == 1)
                        tempPath += "R";
                    else if (rand == 2)
                        tempPath += "M";
                }
                else
                {
                    if (curPos.x - height < -max.x) //left
                    {
                        if (tilesHigh >= 1)
                            tempPath += "L";
                        //Debug.Log("left at " + curPos.x + "," + curPos.y + " (" + max.x + "," + max.y + ")");
                    }
                    else if (curPos.x + height >= max.x) //right
                    {
                        if (tilesHigh >= 2)
                            tempPath += "R";
                        else if (tilesHigh >= 1)
                            tempPath += "L";
                        //Debug.Log("right at " + curPos.x + "," + curPos.y + " (" + max.x + "," + max.y + ")");
                    }
                    else //middle
                    {
                        if (tilesHigh >= 3)
                            tempPath += "M";
                        else if (tilesHigh >= 2)
                            tempPath += "R";
                        else if (tilesHigh >= 1)
                            tempPath += "L";
                        //Debug.Log("middle at " + curPos.x + "," + curPos.y + " (" + max.x + "," + max.y + ")");
                    }
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
    public void TileWorld(string pathChange, float h, float w)
    {
        path = pathChange;
        height = h;
        width = w;
        TileWorld();
    }

    // Overloaded for a path, width, height, flipX, and flipY change
    public void TileWorld(string pathChange, float h, float w, bool flipY, bool flipX)
    {
        path = pathChange;
        height = h;
        width = w;
        randomFlipY = flipY;
        randomFlipX = flipX;
        TileWorld();
    }

    // Overloaded for a path, width, height, flipX, flipY, randomizeMode, tilesHigh, and tilesWide change
    public void TileWorld(string pathChange, float h, float w, bool flipY, bool flipX, bool randomY, bool randomX, int tilesHigh, int tilesWide)
    {
        path = pathChange;
        height = h;
        width = w;
        randomFlipY = flipY;
        randomFlipX = flipX;
        randomizeY = randomY;
        randomizeX = randomX;
        this.tilesHigh = Mathf.Clamp(tilesHigh, 0, 3);
        this.tilesWide = Mathf.Clamp(tilesWide, 0, 3);
        TileWorld();
    }
    #endregion
}
