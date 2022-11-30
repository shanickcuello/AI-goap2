using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{

    public GameObject player;

    public float seePlayerDist;
    public Vector3 playerPos;
    public float angle;
    public float viewAngle = 110f;
    RaycastHit hit;
    public bool playerOnAngle;
    public bool playerOnSight = false;
    public Animator myAnim;
 
    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myAnim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyVision();
    }

    void EnemyVision()
    {
        Vector3 direction = (player.transform.position - transform.position);
        angle = Vector3.Angle(direction, transform.forward);


        if (angle <= viewAngle * 0.5f)
        {
            playerOnAngle = true;

        }

        if (angle > viewAngle * 0.5f)
        {
            playerOnAngle = false;
            playerOnSight = false;
        }

        if (playerOnAngle == true)
        {
            if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, seePlayerDist))
            {
                if (hit.collider.gameObject == player)
                {
                   
                    playerOnSight = true;
                  
                }

                if (hit.collider.gameObject != player || Vector3.Distance(transform.position, player.transform.position) > seePlayerDist)
                {
                    playerOnSight = false;
                 
                }
               
            }

        }

    }
}
