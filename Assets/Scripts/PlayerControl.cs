using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    #region Player movement variables
    [SerializeField]
    float speed = 1.0f;
    [SerializeField]
    float turnSpeed = 45;
    private GlobalBehavior globalBehavior;
    #endregion

    // Use this for initialization
    //assumes this has a sprite renderer and 4 facing directions named in order
    void Start ()
    {
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
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
        clampToWorld(5.0f);
        #endregion

        #region Fire projectile
        //todo
        #endregion
    }

    #region Collision functions
    //clamps to world with an additional buffer
    private void clampToWorld(float buffer)
    {
        Vector3 pos = transform.position;
        Vector2 max = globalBehavior.WorldMax;
        Vector2 min = globalBehavior.WorldMin;

        //x
        if (pos.x + buffer > max.x )
            transform.position = new Vector3(max.x - buffer, pos.y, pos.z);
        else if (pos.x - buffer < min.x)
            transform.position = new Vector3(min.x + buffer, pos.y, pos.z);

        pos = transform.position;

        //y
        if (pos.y + buffer > max.y )
            transform.position = new Vector3(pos.x, max.y - buffer, pos.z);
        else if (pos.y - buffer < min.y )
            transform.position = new Vector3(pos.x, min.y + buffer, pos.z);
    }
    #endregion
}
