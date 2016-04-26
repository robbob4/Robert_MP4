// --------------------------- SpriteUpdate.cs --------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 19, 2016
// Modified - April 25, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a player sprite animation behavior script. 
// Keeps track of facing, and reads input to determine primary direction.
// ----------------------------------------------------------------------------
// Notes - Determines player facing from input rather than grabbing variable 
// from control due to the necessity of knowing when the facing has changed.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class SpriteUpdate : MonoBehaviour
{
    #region Variables
    public enum Facing
    {
        Unknown = 0,
        South = 1,
        West = 2,
        East = 3,
        North = 4
    }
    private Facing currentFacing = Facing.South;
    private Animator animateComp = null;
    #endregion

    // Use this for initialization
    void Start ()
    {
        animateComp = GetComponent<Animator>();
        if (animateComp == null)
            Debug.LogError("Animator not found for " + this + ".");
    }
	
	// Update is called once per frame
	void Update ()
    {        
        //determine delta
        //float xAxis = transform.position.x - oldPos.x;
        //float yAxis = transform.position.y - oldPos.y;
        //oldPos = transform.position;
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //animate the sprite
        updateSprite(xAxis, yAxis);
    }

    #region Sprite functions
    //update the sprite facing, and animation speed
    private void updateSprite(float x, float y)
    {
        if (animateComp == null)
            return;

        Facing newFacing = Facing.Unknown;

        //set animation speed
        animateComp.SetFloat("Horizontal", x);
        animateComp.SetFloat("Vertical", y);

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x > 0)
                newFacing = Facing.East;
            else
                newFacing = Facing.West;
        }
        else if (Mathf.Abs(y) > Mathf.Abs(x))
        {
            if (y > 0)
                newFacing = Facing.North;
            else
                newFacing = Facing.South;
        }

        //set new facing
        if (newFacing != currentFacing && newFacing != Facing.Unknown)
        {
            if (newFacing == Facing.North || newFacing == Facing.South)
                animateComp.SetBool("VerticalFacing", true);
            else
                animateComp.SetBool("VerticalFacing", false);

            animateComp.SetTrigger("NewFacing");
            currentFacing = newFacing;
        }
    }
    #endregion

    //returns the facing
    public Facing GetFacing()
    {
        return currentFacing;
    }
}
