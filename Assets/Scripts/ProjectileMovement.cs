using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100.0f;

    private GlobalBehavior globalBehavior = null;

    // Use this for initialization
    void Start ()
    {
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
        if (globalBehavior == null)
            Debug.LogError("GameManager not found.");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //move forward
        transform.position += (speed * Time.smoothDeltaTime) * transform.up;

        //check boundary collision
        GlobalBehavior.WorldBoundStatus status =
            globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
        if (status != GlobalBehavior.WorldBoundStatus.Inside)
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
