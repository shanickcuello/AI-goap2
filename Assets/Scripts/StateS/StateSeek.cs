using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSeek<T> : FSMState<T>
{

    public EnemyLineOfSight myLineOfSight;
    public GameObject playerHealth;
    public GameObject myRobot;
    public PlayerHealth myPlayerHealth;
    public float shootCount = 4;
    public EnemyBehaviours myBehaviours;
    public GameObject player;
    public Transform spawnPos;
    public GameObject bullet;
    public EnemyMovement myMovement;
    public ShootPlayer myShootPlayer;
    public AgentASTAR myAstarAgent;
    public GameObject playerSeeker;
    public PlayerSeeker myPlayerSeeker;
    public float timesDone = 0;
    public float recalculate = 0;
    public bool gotDirections = false;
    public bool donePath = false;


    public StateSeek(GameObject robot)
    {
        myRobot = robot;
    }

    public override void OnEnter()
    {
        myAstarAgent = myRobot.GetComponent<AgentASTAR>();
        myShootPlayer = myRobot.GetComponent<ShootPlayer>();
        playerSeeker = GameObject.FindGameObjectWithTag("PlayerSeeker");
        myPlayerSeeker = playerSeeker.GetComponent<PlayerSeeker>();
        myLineOfSight = myRobot.GetComponent<EnemyLineOfSight>();
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth");
        myPlayerHealth = playerHealth.GetComponent<PlayerHealth>();
        myBehaviours = myRobot.GetComponent<EnemyBehaviours>();
        player = GameObject.FindGameObjectWithTag("Player");
        myMovement = myRobot.GetComponent<EnemyMovement>();   


      


    }
    public override void OnUpdate()
    {

       
      
        myMovement.nearWaypoints = Physics.OverlapSphere(myRobot.transform.position, 10, myMovement.myLayermask);

        foreach (var item in myMovement.nearWaypoints)
        {
            var shorterDistance = Vector3.Distance(item.transform.position, myRobot.transform.position);
            if (shorterDistance < myMovement.distanceToNearest)
            {
                myMovement.distanceToNearest = shorterDistance;
                myMovement.nearestWaypoint = item.transform;

            }


        }

        myMovement.distanceToNearest = 10;


        var playerNearWaypoints = Physics.OverlapSphere(player.transform.position, 10, myMovement.myLayermask);

        foreach (var item in playerNearWaypoints)
        {
            var shorterDistance = Vector3.Distance(item.transform.position, player.transform.position);
            if (shorterDistance < myMovement.distanceToPlayerNearest)
            {
                myMovement.distanceToPlayerNearest = shorterDistance;
                myMovement.playerNearestWaypoint = item.transform;

            }
        }


        myMovement.distanceToPlayerNearest = 10;

        if (myPlayerSeeker.seenPlayer == true)
        {

            recalculate += Time.deltaTime;

            if (recalculate >= 2)
            {
                timesDone = 0;
                donePath = false;
                gotDirections = false;
                recalculate = 0;
            }


        }

        if (timesDone == 0)
        {
            if (gotDirections == false)
            {
                myAstarAgent.starterNode = myMovement.nearestWaypoint.GetComponent<Node>();

                myAstarAgent.endNode = myMovement.playerNearestWaypoint.GetComponent<Node>();

                gotDirections = true;

            }

        }

          if (donePath == false && timesDone == 0 && gotDirections == true)
        {
            myAstarAgent.PathFindingA();

            System.Array.Resize(ref myMovement.mySeekWaypoints, myAstarAgent._list.Count);
            myMovement.mySeekWaypoints = new Transform[myAstarAgent._list.Count];
            //myMovement.waypointCount = myMovement.mySeekWaypoints.Length;
            for (int i = 0; i < myAstarAgent._list.Count; i++)
            {
                myMovement.mySeekWaypoints[i] = myAstarAgent._list[i].transform;
            }

            myMovement.currentWaypoint = 0;

            donePath = true;
        }

       


        if (timesDone == 0)
        {
            if (myMovement.currentWaypoint < myMovement.mySeekWaypoints.Length - 1)
            {
                if (myMovement.myAgent.speed <= 0.75f)
                    myMovement.myAgent.speed += Time.deltaTime;

                if (myMovement.offsetSpeed <= 2f)
                    myMovement.offsetSpeed += Time.deltaTime;
            }

            if (myMovement.currentWaypoint >= myMovement.mySeekWaypoints.Length - 1)
                {
                    if (myMovement.myAgent.speed >= 0.5f)
                        myMovement.myAgent.speed -= Time.deltaTime / 3;

                    if (myMovement.offsetSpeed >= 1.5f)
                        myMovement.offsetSpeed -= Time.deltaTime / 3;
                }

        }

       /*   if (timesDone > 0)
        {
            if (myMovement.myAgent.speed >= 0.5f)
                myMovement.myAgent.speed -= Time.deltaTime / 2;

            if (myMovement.offsetSpeed <= 0.75f)
                myMovement.offsetSpeed -= Time.deltaTime / 3;
        }
        */

        

        if (Vector3.Distance(myRobot.transform.position, myMovement.mySeekWaypoints[myMovement.currentWaypoint].transform.position) <= 1f && myMovement.currentWaypoint < myMovement.mySeekWaypoints.Length - 1)
        {        
            myMovement.currentWaypoint++;
        }

 

        if (myMovement.currentWaypoint == myMovement.mySeekWaypoints.Length - 1 && Vector3.Distance(myRobot.transform.position, myMovement.mySeekWaypoints[myMovement.currentWaypoint].transform.position) <= 1f && timesDone <= 2)
        {
            timesDone++;

            if (timesDone == 1)
            {
                System.Array.Reverse(myMovement.mySeekWaypoints);

                //myAstarAgent.PathFindingA();
                myMovement.currentWaypoint = 0;            
            }
            
            if (timesDone == 2)
            {
                donePath = false;
                gotDirections = false;
            }

        

        }

       

        if (timesDone == 2 && gotDirections == false)
        {
            
                myAstarAgent.starterNode = myMovement.nearestWaypoint.GetComponent<Node>();

                myAstarAgent.endNode = myMovement.myPatrolWaypoints[0].GetComponent<Node>();

                gotDirections = true;

                myMovement.currentWaypoint = 0;


           // System.Array.Reverse(myMovement.mySeekWaypoints);
        }

        if (timesDone == 2 && donePath == false && gotDirections == true)
        {

            myAstarAgent.PathFindingA();

           // System.Array.Clear(myMovement.mySeekWaypoints, 0, myMovement.mySeekWaypoints.Length);
            System.Array.Resize(ref myMovement.mySeekWaypoints, myAstarAgent._list.Count);
            myMovement.mySeekWaypoints = new Transform[myAstarAgent._list.Count];
            //myMovement.waypointCount = myMovement.mySeekWaypoints.Length;
           

            for (int i = 0; i < myAstarAgent._list.Count; i++)
            {
                myMovement.mySeekWaypoints[i] = myAstarAgent._list[i].transform;
            }

            myMovement.currentWaypoint = 0;
            myMovement.currentWaypoint = 0;
            donePath = true;

        }




        if (timesDone > 2)
        {
            myBehaviours.endedSeeking = true;                   
        }

        myMovement.target = myMovement.mySeekWaypoints[myMovement.currentWaypoint].transform;
    }

    public override void OnSleep()
    {
        myMovement.currentWaypoint = 0;
        myBehaviours.endedPatrol = false;
        donePath = false;
        timesDone = 0;
        gotDirections = false;
    }
}
