using UnityEngine;
using System.Collections;

public class DoneLaserPlayerDetection : MonoBehaviour
{
    private GameObject player;								// Reference to the player.
    private DoneLastPlayerSighting lastPlayerSighting;		// Reference to the global last sighting of the player.
    private GameObject myPlayerSeeker;


    void Awake ()
    {
		// Setting up references.
		player = GameObject.FindGameObjectWithTag(DoneTags.player);
        myPlayerSeeker = GameObject.FindGameObjectWithTag("PlayerSeeker");
		lastPlayerSighting = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneLastPlayerSighting>();
    }


    void OnTriggerStay(Collider other)
    {
		// If the beam is on...
        if(GetComponent<Renderer>().enabled)
			// ... and if the colliding gameobject is the player...
            if(other.gameObject == player)
            {
                lastPlayerSighting.position = other.transform.position;
                myPlayerSeeker.GetComponent<PlayerSeeker>().counter = 0;
                myPlayerSeeker.GetComponent<PlayerSeeker>().seenPlayer = true;
            }
				// ... set the last global sighting of the player to the colliding object's position.
                
    }
}