using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100f;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += (speed * Time.smoothDeltaTime) * transform.up;
    }

    public void SetForwardDirection(Vector3 f)
    {
        transform.up = f;
    }
}
