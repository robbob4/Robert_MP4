using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    #region Player movement variables
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float turnSpeed = 45;
    [SerializeField] private int recastDelay = 10; //number of frames to be able to fire a projectile

    private GlobalBehavior globalBehavior;
    private int delayCount;
    #endregion

    public GameObject projectile = null;

    // Use this for initialization
    //assumes this has a sprite renderer and 4 facing directions named in order
    void Start ()
    {
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();

        // initialize projectile spawning
        if (null == projectile)
            projectile = Resources.Load("Prefabs/Projectile") as GameObject;
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
        globalBehavior.clampToWorld(transform, 5.0f);
        #endregion

        #region Fire projectile
        if (Input.GetAxis("Fire1") > 0f) //Left-Control
        {
            if(delayCount-- <= 0)
            {
                GameObject e = Instantiate(projectile) as GameObject;
                ProjectileMovement proj = e.GetComponent<ProjectileMovement>(); // Shows how to get the script from GameObject
                if (null != proj)
                {
                    e.transform.position = transform.position;
                    proj.SetForwardDirection(transform.up);
                }
                delayCount = recastDelay;
            }
        }
        #endregion

        #region Toggle movement
        if (Input.GetButtonUp("Jump"))
            globalBehavior.movement = !globalBehavior.movement;
        #endregion
    }
}