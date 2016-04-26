﻿// ----------------------- ProjectileMovement.cs ------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 19, 2016
// Modified - April 25, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a projectile that moves forward and collides.
// ----------------------------------------------------------------------------
// Notes - Despawned when collided.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100.0f;

    private LevelBehavior levelBehavior = null;

    // Use this for initialization
    void Start ()
    {
        levelBehavior = GameObject.Find("GameManager").GetComponent<LevelBehavior>();
        if (levelBehavior == null)
        {
            Debug.LogError("GameManager not found for " + this + ".");
            Application.Quit();
        }
            
    }
	
	// Update is called once per frame
	void Update ()
    {
        //move forward
        transform.position += (speed * Time.smoothDeltaTime) * transform.up;

        //check boundary collision
        LevelBehavior.WorldBoundStatus status =
            levelBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
        if (status != LevelBehavior.WorldBoundStatus.Inside)
        {
            //Debug.Log("collided position: " + this.transform.position);
            Destroy(gameObject);
        }
    }

    public void SetForwardDirection(Vector3 f)
    {
        transform.up = f;
    }
}
