// ------------------------ SpriteUpdateEnemy.cs ------------------------------
// Author - Robert Griswold CSS 385
// Created - Apr 19, 2016
// Modified - April 25, 2016
// ----------------------------------------------------------------------------
// Purpose - Implementation for a enemy sprite animation behavior script. 
// Keeps track of facing, and reads change in position to determine primary 
// direction.
// ----------------------------------------------------------------------------
// Notes - None.
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class SpriteUpdateEnemy : MonoBehaviour
{
    #region Sprite variables
    enum Facing
    {
        Unknown = 0,
        South = 1,
        West = 2,
        East = 3,
        North = 4
    }

    private Animator animateComp = null;
    private Facing currentFacing;
    private Vector3 oldPos;
    #endregion

    private EnemyBehavior.EnemyState curState;

    // Use this for initialization
    void Start()
    {
        animateComp = GetComponent<Animator>();
        if (animateComp == null)
            Debug.LogError("Animator not found.");

        curState = GetComponent<EnemyBehavior>().currentState;

        currentFacing = Facing.South;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //determine delta
        float xAxis = transform.position.x - oldPos.x;
        float yAxis = transform.position.y - oldPos.y;
        oldPos = transform.position;

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
        EnemyBehavior.EnemyState newState = GetComponent<EnemyBehavior>().currentState;
        bool run = false;
        bool walk = false;
        bool stun = false;

        //set animation speed
        animateComp.SetFloat("Horizontal", x);
        animateComp.SetFloat("Vertical", y);

        //update state setting
        switch (newState)
        {
            case EnemyBehavior.EnemyState.Run:
                run = true;
                break;
            case EnemyBehavior.EnemyState.Stunned:
                stun = true;
                break;
            default:
                walk = true;
                break;
        }

        //determine strongest directional
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

        if (newState != curState)
        {
            animateComp.SetBool("Run", run);
            animateComp.SetBool("Stunned", stun);
            animateComp.SetBool("Walk", walk);

            if (curState == EnemyBehavior.EnemyState.Stunned) //exit stunned
            {
                animateComp.SetTrigger("NewFacing");
            }


            curState = newState;
        }

        if (newFacing != currentFacing)
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
}