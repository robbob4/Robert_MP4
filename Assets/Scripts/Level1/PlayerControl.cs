// -------------------------- PlayerControl.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 19, 2016
// Modified - April 28, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a player controlled hero that can pause enemy 
// movement with space, fire, move up/down/left/right, and is calmped to the 
// world boundaries.
// ----------------------------------------------------------------------------
// Notes - Maintains a current facing direction.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    #region Player movement variables
    public enum Facing
    {
        Unknown = 0,
        South = 1,
        West = 2,
        East = 3,
        North = 4
    }
    [SerializeField] private float speed = 1.0f;
    //[SerializeField] private float turnSpeed = 45.0f;

    private Facing currentFacing = Facing.South;
    #endregion

    #region Projectile variables
    public int MaxCharges = 6; // max number of stored charges
    public int RechargeDelay = 40; //number of frames to be able to recharge a projectile
    public int RecastDelay = 20; //number of frames to be able to cast a projectile

    private int charges = 6; //current number of charges
    private int recastDelayCount = 0; //current recast delay
    private int rechargeDelayCount = 0; //current recharge delay
    #endregion

    #region Other references
    public GameObject Projectile = null;
    private WorldBound sceneBoundary = null;
    #endregion

    // Use this for initialization
    //assumes this has a sprite renderer and 4 facing directions named in order
    void Start ()
    {
        #region References
        sceneBoundary = GameObject.Find("GameManager").GetComponent<WorldBound>();
        if (sceneBoundary == null)
        {
            Debug.LogError("WorldBound not found for levelManager in " + this + ".");
            Application.Quit();
        }

        // initialize projectile spawning
        if (Projectile == null)
            Projectile = Resources.Load("Prefabs/Projectile") as GameObject;
        if (Projectile == null)
            Debug.LogError("Projectile not found for " + this + ".");
        #endregion

        #region Get data from GlobalGameManager
        //increase rechargeDelay 4 * level
        RechargeDelay += (MenuBehavior.TheGameState.GetLastLevel() + 1) * 4;
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        #region User movement input
        //translate/rotate
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        transform.Translate(xAxis * Time.deltaTime * speed, yAxis * Time.deltaTime * speed, 0.0f);
        //transform.Translate(0.0f, yAxis * Time.deltaTime * speed, 0.0f);
        //transform.Rotate(transform.forward, - xAxis * Time.deltaTime * turnSpeed);

        //clamp to world
        sceneBoundary.ClampToWorld(transform, 5.0f);
        #endregion

        #region Recharge charges
        //increase number of charges if delay is up and not at max already
        if (rechargeDelayCount-- <= 0 && charges < MaxCharges) //side effect: decreases rechargeDelayCount
        {
            charges++;
            rechargeDelayCount = RechargeDelay;
            //Debug.Log(charges + "/" + MaxCharges);
        }
        #endregion

        #region Update recastDelay
        if (recastDelayCount > 0)
            recastDelayCount--;
        #endregion

            #region Fire projectile
        if (Input.GetAxis("Fire1") > 0f) //Left-Control
        {
            //check if recast delay is up, there is a charge, and projectile loaded
            if (recastDelayCount <= 0 && charges > 0 && Projectile != null)
            {
                //Debug.Log(charges + "/" + MaxCharges + " " + recastDelayCount + "/" + RecastDelay);
                charges--;
                GameObject e = Instantiate(Projectile) as GameObject;
                ProjectileMovement proj = e.GetComponent<ProjectileMovement>();
                if (null != proj)
                {
                    e.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

                    //change projectile direction
                    switch (currentFacing)
                    {
                        case Facing.East:
                            proj.SetForwardDirection(transform.right);
                            break;
                        case Facing.West:
                            proj.SetForwardDirection(-transform.right);
                            break;
                        case Facing.South:
                            proj.SetForwardDirection(-transform.up);
                            e.GetComponent<SpriteRenderer>().sortingLayerName = "Player"; //put fireball ontop of player
                            break;
                        default:
                            proj.SetForwardDirection(transform.up);
                            break;
                    }
                    
                }

                recastDelayCount = RecastDelay;
            }
        }
        #endregion

        //Update facing
        updateFacing(xAxis, yAxis);
    }

    #region Facing functions
    //determine strongest directional from input
    private void updateFacing(float x, float y)
    {
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
                currentFacing = Facing.East;
            else
                currentFacing = Facing.West;
        }
        else if (Mathf.Abs(y) > Mathf.Abs(x))
        {
            if (y > 0)
                currentFacing = Facing.North;
            else
                currentFacing = Facing.South;
        }

        //determine direction from rotation
        //Vector3 direction = transform.right;
        //Facing newFacing = Facing.Unknown;

        //if (direction.x > direction.y) //more vertical than horizontal
        //{
        //    if (direction.x > 0)
        //        currentFacing = Facing.North;
        //    else if (direction.x < 0)
        //        currentFacing = Facing.South;
        //}
        //else //more horizontal than vertical
        //{
        //    if (direction.y > 0)
        //        currentFacing = Facing.West;
        //    else if (direction.y < 0)
        //        currentFacing = Facing.East;
        //}
    }

    public Facing GetFacing()
    {
        return currentFacing;
    }
    #endregion

    #region Powerup modification functions
    public void ChargeBoost(int additionalCharges)
    {
        charges += additionalCharges;
    }
    #endregion
}