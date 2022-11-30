using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{

    public Transform target;
    public LayerMask obstaclesLayers;
    public float speed;
    public float radius;
    public float avoidQty;
    public float rotationSpeed;
    public Transform myObstacle;
    public NavMeshAgent myAgent;
    public Transform[] myPatrolWaypoints;
    public Transform[] mySeekWaypoints;
    public Transform[] reversedWaypoints;
    public Collider[] nearWaypoints;
    public Transform nearestWaypoint;
    public Transform playerNearestWaypoint;
    public float distanceToNearest = 10;
    public float distanceToPlayerNearest = 10;
    public float distanceOffset = 0.5f;
    public int waypointCount;
    public int currentWaypoint;
    public bool reverseArray;
    public Vector3 startPos;
    public float animSpeed;
    public float angularSpeed;
    public Animator myAnim;
    public EnemyBehaviours myBehaviours;
    public float offsetSpeed;
    public AgentASTAR myAstarAgent;
    public LayerMask myLayermask;
    public GameObject player;
    public EnemyLineOfSight myLineOfSight;

    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myAgent = GetComponent<NavMeshAgent>();
        myAstarAgent = GetComponent<AgentASTAR>();
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myAnim = GetComponent<Animator>();
        startPos = transform.position;
        myBehaviours = GetComponent<EnemyBehaviours>();
        
    }
    void Start()
    {
        //inicializa los waypoints con un pathfinding del nodo incial al final, luego los asigna a una array de waypoints que el robot recorrerá y a la cual volverá en su estado idle.
      //  myAstarAgent.PathFindingA();
        // System.Array.Resize(ref myWaypoints, myAstarAgent._list.Count);
      //  myPatrolWaypoints = new Transform[myAstarAgent._list.Count];
        waypointCount = myPatrolWaypoints.Length;
       

       /* for (int i = 0; i < myAstarAgent._list.Count; i++)
        {         
            myPatrolWaypoints[i] = myAstarAgent._list[i].transform;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        GetDir();
    }

    void GetDir()
    {


        Vector3 direction = myAgent.transform.InverseTransformDirection(myAgent.velocity).normalized;
        float turnForce = direction.x;
        myAnim.SetFloat("AngularSpeed", turnForce * 2);


        myAgent.SetDestination(target.transform.position);
        myAnim.SetFloat("Speed", myAgent.speed + offsetSpeed);


        if (myBehaviours.patrolBehaviour == true)
        {
            target = myPatrolWaypoints[currentWaypoint].transform;

            if (Vector3.Distance(transform.position, myPatrolWaypoints[currentWaypoint].transform.position) <= 1f && currentWaypoint < waypointCount - 1)
            {
                currentWaypoint++;
            }

            if (currentWaypoint == waypointCount - 1 && Vector3.Distance(transform.position, myPatrolWaypoints[currentWaypoint].transform.position) <= 1f)
            {
                myBehaviours.endedPatrol = true;
                reverseArray = false;
            }

        }

        if (myBehaviours.seekPlayerBehaviour == true)
        {
            for (int i = 0; i < 1; i++)
            {
               
            }
               
        }

        if (myBehaviours.idleBehaviour == true)
        {
            if (reverseArray == false)
            {
                System.Array.Reverse(myPatrolWaypoints);
                reverseArray = true;
            }

            target = myPatrolWaypoints[0];
            myAgent.speed = 0;
        }

        if (myBehaviours.chasePlayerBehaviour == true || myBehaviours.alertBehaviour == true)
        {

            nearWaypoints = Physics.OverlapSphere(transform.position, 10, myLayermask);

            foreach (var item in nearWaypoints)
            {
                var shorterDistance = Vector3.Distance(item.transform.position, transform.position);
                if (shorterDistance < distanceToNearest)
                {
                    distanceToNearest = shorterDistance;
                    nearestWaypoint = item.transform;
                                   
                }

                
            }

            distanceToNearest = 10;



            var playerNearWaypoints = Physics.OverlapSphere(player.transform.position, 10, myLayermask);

            foreach (var item in playerNearWaypoints)
            {
                var shorterDistance = Vector3.Distance(item.transform.position, player.transform.position);
                if (shorterDistance < distanceToPlayerNearest)
                {
                    distanceToPlayerNearest = shorterDistance;
                    playerNearestWaypoint = item.transform;
                    
                }
            }

            distanceToPlayerNearest = 10;
          
            myAstarAgent.starterNode = nearestWaypoint.GetComponent<Node>();          
            myAstarAgent.endNode = playerNearestWaypoint.GetComponent<Node>();

            
            
        
            myAstarAgent.PathFindingA();


          //  if (myBehaviours.chasePlayerBehaviour)
               // distanceOffset = 1;



            if (Vector3.Distance(transform.position, myAstarAgent._list[0].transform.position) <= distanceOffset && myAstarAgent._list.Count > 1)
            {
                target = myAstarAgent._list[1].transform;
            }

            if (Vector3.Distance(transform.position, myAstarAgent._list[0].transform.position) > distanceOffset && myAstarAgent._list.Count <= 1)
            {
                target = myAstarAgent._list[0].transform;
            }

            else if (Vector3.Distance(transform.position, myAstarAgent._list[0].transform.position) <= distanceOffset && myAstarAgent._list.Count <= 1)
            {
                target = player.transform;
            }
               

            
          

            


        }

    }

   

}
