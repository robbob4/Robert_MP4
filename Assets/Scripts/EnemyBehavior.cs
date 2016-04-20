using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	
	public float mSpeed = 20f;
	public float mTowardsCenter = 0.5f;  
		// what is the change of enemy flying towards the world center after colliding with world bound
		// 0: no control
		// 1: always towards the world center, no randomness
		
	// Use this for initialization
	void Start () {
		NewDirection();	
	}

	// Update is called once per frame
	void Update () {
		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
		GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();
		
		GlobalBehavior.WorldBoundStatus status =
		globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
			
		if (status != GlobalBehavior.WorldBoundStatus.Inside) {
		Debug.Log("collided position: " + this.transform.position);
			NewDirection();
		}
	}

	// New direction will be something randomly within +- 45-degrees away from the direction
	// towards the center of the world
	//
	// To find an angle within +-45 degree of a direction: 
	//     1. consider the simplist case of 45-degree above or below the x-direction
	//	   2. we compute random.X: a randomly generate x-value between +1 and -1
	//     3. To ensure within 45 degrees, we simply need to make sure generating a y-value that is within the (-random.X to +random.X) range
	//     4. Now a direction towards the (random.X, random.Y) is guaranteed to be within 45-degrees from x-direction
	// Apply the above logic, only now:
	//		X-direciton is V (from current direciton towards the world center)
	//		Y-direciton is (V.y, -V.x)
	//
	// Lastly, 45-degree is nice because X=Y, we can do this for any angle that is less than 90-degree
	private void NewDirection() {
		GlobalBehavior globalBehavior = GameObject.Find ("GameManager").GetComponent<GlobalBehavior>();

		// we want to move towards the center of the world
		Vector2 v = globalBehavior.WorldCenter - new Vector2(transform.position.x, transform.position.y);  
				// this is vector that will take us back to world center
		v.Normalize();
		Vector2 vn = new Vector2(v.y, -v.x); // this is a direciotn that is perpendicular to V

		float useV = 1.0f - Mathf.Clamp(mTowardsCenter, 0.01f, 1.0f);
		float tanSpread = Mathf.Tan( useV * Mathf.PI / 2.0f );

		float randomX = Random.Range(0f, 1f);
		float yRange = tanSpread * randomX;
		float randomY = Random.Range (-yRange, yRange);

		Vector2 newDir = randomX * v + randomY * vn;
		newDir.Normalize();
		transform.up = newDir;
	}
}
