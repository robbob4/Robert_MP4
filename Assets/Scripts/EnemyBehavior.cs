using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
    #region State variables
    public enum EnemyState
    {
        Normal = 0,
        Run = 1,
        Stunned =2
    }
    public EnemyState currentState = EnemyState.Normal;
    public int lives = 3; //number of lives the entity has
    [SerializeField] private float checkFacingAngle = 0.7f;
    
    private float timeLeft = 0.0f;
    #endregion

    #region Enemy movement variables
    public float speed = 30.0f;
    public float turnSpeed = 9.0f;
    [SerializeField] private float minSpeed = 20.0f;
    [SerializeField] private float maxSpeed = 40.0f;
    [SerializeField] private float towardsCenter = 0.5f;
        // what is the change of enemy flying towards the world center after colliding with world bound
        // 0: no control
        // 1: always towards the world center, no randomness

    private GlobalBehavior globalBehavior;
    private GameObject player;
    #endregion

    #region Sprite variables
    //public Sprite normalSprite;
    //public Sprite stunnedSprite;
    //public Sprite runSprite;

    //private SpriteRenderer spriteComp;
    #endregion

    // Use this for initialization
    void Start () {
        //spriteComp = GetComponent<SpriteRenderer>();
        globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
        player = GameObject.Find("Hero");

        //if (normalSprite == null)
        //    normalSprite = spriteComp.sprite;

        newDirection();
        newSpeed();
	}

	// Update is called once per frame
	void Update () {
        //update timer and change state
        updateTimer();
        changeState();

        //movement based on state
        if (currentState == EnemyState.Run) //running
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            float angle = Vector3.Dot(transform.right.normalized,
            (transform.position - player.transform.position).normalized); //TODO Fix this
            //Debug.Log("distance: " + distance + " angle: " + angle +  " calc: " + (angle * Time.deltaTime - distance));

            transform.Rotate(transform.forward, (45 - distance) / angle * Time.deltaTime);
            //transform.up = (transform.position - player.transform.position);
            transform.position += (speed * Time.smoothDeltaTime) * transform.up;
        }
        else if(currentState == EnemyState.Stunned) //stunned still
        {
            transform.Rotate(transform.forward, Time.deltaTime * turnSpeed);
        }  
        else if (globalBehavior.movement) //normal
        {
            transform.position += (speed * Time.smoothDeltaTime) * transform.up;
        }

        //check boundary collision
        GlobalBehavior.WorldBoundStatus status =
		    globalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
		if (status != GlobalBehavior.WorldBoundStatus.Inside) {
		    //Debug.Log("collided position: " + this.transform.position);
			newDirection();
		}

        //clamp to world
        globalBehavior.clampToWorld(transform, 5.0f);
	}

    #region State functions
    //creates a timer
    void createTimer(float time)
    {
        timeLeft = time;
    }

    //decrements time left
    bool updateTimer()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
            return true;

        return false;
    }

    //collision
    void On2DTriggerEnter(Collider2D other)
    {
        Debug.Log(other);
        //ignore anything other than Projectiles
        if (other.gameObject.name != "Projectile(Clone)")
            return;

        //change to stunned
        currentState = EnemyState.Stunned;
        //spriteComp.sprite = stunnedSprite;
        createTimer(5.0f);

        //destroy projectile
        Destroy(other.gameObject);

        //decrease lives and destroy if out of lives
        if (--lives <= 0)
            Destroy(gameObject);
    }

    //determines state between Normal and Run
    //stunned is handled by trigger event
    void changeState()
    {
        //determine distance and facing of player
        float distance = Vector3.Distance(player.transform.position, transform.position);
        float angle = Vector3.Dot(player.transform.up,
            (transform.position - player.transform.position).normalized);
        
        //determine new state
        if (distance <= 30 && angle > checkFacingAngle)
        {
            currentState = EnemyState.Run;
            //spriteComp.sprite = runSprite;
        }
        else if ((currentState == EnemyState.Run && distance > 30) 
            || (currentState == EnemyState.Stunned && timeLeft <= 0))
        {
            currentState = EnemyState.Normal;
            //spriteComp.sprite = normalSprite;
            newSpeed();
        }  
    }
    #endregion

    #region Movement functions
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
    private void newDirection() {
		// we want to move towards the center of the world
		Vector2 v = globalBehavior.WorldCenter - new Vector2(transform.position.x, transform.position.y);  
				// this is vector that will take us back to world center
		v.Normalize();
		Vector2 vn = new Vector2(v.y, -v.x); // this is a direciotn that is perpendicular to V

		float useV = 1.0f - Mathf.Clamp(towardsCenter, 0.01f, 1.0f);
		float tanSpread = Mathf.Tan( useV * Mathf.PI / 2.0f );

		float randomX = Random.Range(0f, 1f);
		float yRange = tanSpread * randomX;
		float randomY = Random.Range (-yRange, yRange);

		Vector2 newDir = randomX * v + randomY * vn;
		newDir.Normalize();
		transform.up = newDir;
	}

    //changes speed to a random between minSpeed and maxSpeed
    private void newSpeed()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }
    #endregion
}
