using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapWaypoint : MonoBehaviour
{
    public PlayerSeeker mySeeker;
    public GameObject playerSeeker;
   
    // Start is called before the first frame update
    void Start()
    {
        playerSeeker = GameObject.FindGameObjectWithTag("PlayerSeeker");
        mySeeker = playerSeeker.GetComponent<PlayerSeeker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
        {
            mySeeker.counter = 0;
            mySeeker.seenPlayer = true;
        }
    }
}
