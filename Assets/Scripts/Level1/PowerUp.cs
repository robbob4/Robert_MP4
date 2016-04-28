

using UnityEngine;
using System.Collections;


public class PowerUp : MonoBehaviour
{
    private GameObject player = null;
    private PlayerControl playerController = null;
    
    void Start ()
    {
        player = GameObject.Find("Hero");
        if (player == null)
        {
            Debug.LogError("Hero not found for " + this + ".");
        }
        else
        {
            playerController = player.GetComponent<PlayerControl>();
            if (playerController == null)
                Debug.LogError("PlayerControl not found for " + player + ".");
        }
    }

    void Update ()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name != "Hero")
            return;

        playerController.MaxCharges += 4;

        Destroy(gameObject);
    }
}
