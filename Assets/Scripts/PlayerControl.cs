// -------------------------- PlayerControl.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 19, 2016
// Modified - April 21, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a player controlled hero that can pause enemy 
// movement with space, fire, move up/down, rotate left/right, and is 
// clamped to the boundaries.
// ----------------------------------------------------------------------------
// Notes - None.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    #region Player movement variables
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float turnSpeed = 45.0f;
    [SerializeField] private int recastDelay = 10; //number of frames to be able to fire a projectile

    private GlobalBehavior globalBehavior = null;
    private int delayCount = 0;
    private AudioSource audioComp = null;
    #endregion

    public GameObject Projectile = null;

    // Use this for initialization
    //assumes this has a sprite renderer and 4 facing directions named in order
    void Start ()
    {
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
        if (globalBehavior == null)
            Debug.LogError("GameManager not found.");

        // initialize projectile spawning
        if (Projectile == null)
            Projectile = Resources.Load("Prefabs/Projectile") as GameObject;
        if (Projectile == null)
            Debug.LogError("Projectile not found.");

        audioComp = GetComponentInChildren<AudioSource>();
        if (audioComp == null)
            Debug.LogError("AudioSource not found.");
    }

    // Update is called once per frame
    void Update ()
    {
        #region User movement input
        //translate/rotate
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        //transform.Translate(xAxis * Time.deltaTime * speed, yAxis * Time.deltaTime * speed, 0.0f);
        transform.Translate(0.0f, yAxis * Time.deltaTime * speed, 0.0f);
        transform.Rotate(transform.forward, - xAxis * Time.deltaTime * turnSpeed);

        //clamp to world
        globalBehavior.ClampToWorld(transform, 5.0f);
        #endregion

        #region Fire projectile
        if (Input.GetAxis("Fire1") > 0f) //Left-Control
        {
            if(delayCount-- <= 0)
            {
                GameObject e = Instantiate(Projectile) as GameObject;
                ProjectileMovement proj = e.GetComponent<ProjectileMovement>(); // Shows how to get the script from GameObject
                if (null != proj)
                {
                    e.transform.position = transform.position;
                    proj.SetForwardDirection(transform.up);
                }
                delayCount = recastDelay;
                audioComp.Play();
            }
        }
        #endregion

        #region Toggle movement
        if (Input.GetButtonUp("Jump"))
            globalBehavior.Movement = !globalBehavior.Movement;
        #endregion
    }
}