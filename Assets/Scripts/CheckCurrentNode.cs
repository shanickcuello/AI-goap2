using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCurrentNode : MonoBehaviour
{

    public EnemyMovement myMovement;
    public EnemyBehaviours myBehaviours;
    public AgentASTAR myAstar;
    public LayerMask myLayermask;
    public Vector3 toPositionSeek;
    public Vector3 toPositionChaseAlert;
    public Vector3 direction;
    public Vector3 direction2;

    // Start is called before the first frame update
    void Start()
    {
        myMovement = gameObject.GetComponent<EnemyMovement>();
        myBehaviours = gameObject.GetComponent<EnemyBehaviours>();
        myAstar = gameObject.GetComponent<AgentASTAR>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fromPosition = transform.position;
        
        if (myMovement.statesTriggers[EStates.ALERT] && myMovement.mySeekWaypoints.Length >= 2)
        {
            Vector3 toPositionSeek = myMovement.mySeekWaypoints[1].transform.position;
            direction = toPositionSeek - fromPosition;
            GetNeightbor(direction);
        }

        if (myMovement.statesTriggers[EStates.CHASE] || myMovement.statesTriggers[EStates.ALERT])
        {
            if (myAstar._list.Count >= 2)
            {
                Vector3 toPositionChaseAlert = myAstar._list[1].transform.position;
                direction2 = toPositionChaseAlert - fromPosition;
                GetNeightbor(direction2);
            }
           

        }
                  
    }

    void GetNeightbor(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, myLayermask))
        {
           
            if (myMovement.currentWaypoint == 0 && myMovement.statesTriggers[EStates.ALERT])
            {
                myMovement.currentWaypoint = 1;
            
            }

            if (myMovement.statesTriggers[EStates.CHASE] || myMovement.statesTriggers[EStates.ALERT])
            {
                myMovement.target = myAstar._list[1].transform;

            }


        }


    }
}
