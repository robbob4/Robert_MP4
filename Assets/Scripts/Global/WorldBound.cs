// -------------------------- WorldBound.cs -------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 26, 2016
// Modified - April 26, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a reusable script for world bound support.
// ----------------------------------------------------------------------------
// Notes - Place this script in a scene with a "Main Camera". Objects can be 
// bound to this parent's camera boundary using ClampToWorld(transform).
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class WorldBound : MonoBehaviour
{
    #region Variables
    private Bounds worldBounds;  // this is the world bound
    private Vector2 worldMin;   // Better support 2D interactions
    private Vector2 worldMax;
    private Vector2 worldCenter;
    private Camera mainCamera;
    #endregion

    //Initialization
    void Awake ()
    {
        mainCamera = Camera.main;
        worldBounds = new Bounds(Vector3.zero, Vector3.one);
        UpdateWorldWindowBound();
    }

    //Update is called once per frame
    void Update ()
    {
	
	}

    #region Game Window World size bound support functions
    public enum WorldBoundStatus
    {
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
        if (null != mainCamera)
        {
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
    public Vector2 WorldMin { get { return worldMin; } }
    public Vector2 WorldMax { get { return worldMax; } }

    public WorldBoundStatus ObjectCollideWorldBound(Bounds objBound)
    {
        WorldBoundStatus status = WorldBoundStatus.Inside;

        if (worldBounds.Intersects(objBound))
        {
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
        }
        else
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

}
