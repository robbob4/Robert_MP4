// ----------------------- ProjectileMovement.cs ------------------------------
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

    private WorldBound sceneBoundary = null;

    // Use this for initialization
    void Start ()
    {
        sceneBoundary = GameObject.Find("GameManager").GetComponent<WorldBound>();
        if (sceneBoundary == null)
        {
            Debug.LogError("WorldBound not found for levelManager in " + this + ".");
            Application.Quit();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //move forward
        transform.position += (speed * Time.smoothDeltaTime) * transform.up;

        //check boundary collision
        WorldBound.WorldBoundStatus status =
            sceneBoundary.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
        if (status != WorldBound.WorldBoundStatus.Inside)
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
