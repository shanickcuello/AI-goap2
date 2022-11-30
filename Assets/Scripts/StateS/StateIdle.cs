using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle<T> : FSMState<T>
{
    public GameObject myRobot;
    public EnemyBehaviours myBehaviours;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public StateIdle(GameObject robot)
    {
        myRobot = robot;
    }

    public override void OnEnter()
    {
        myBehaviours = myRobot.GetComponent<EnemyBehaviours>();
        myLineOfSight = myRobot.GetComponent<EnemyLineOfSight>();
        myMovement = myRobot.GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        myBehaviours.idleCounter += Time.deltaTime;

        if (myMovement.myAgent.speed >= 0)
        {
            myMovement.myAgent.speed -= 1 * Time.deltaTime;    
        }

        if (myMovement.offsetSpeed >= 0)
        {
            myMovement.offsetSpeed -= 1 * Time.deltaTime;
        }


        if (myBehaviours.idleBehaviour == true)
        {
            if (myMovement.reverseArray == false)
            {
                System.Array.Reverse(myMovement.myPatrolWaypoints);
                myMovement.reverseArray = true;
            }

            myMovement.target = myMovement.myPatrolWaypoints[0];
            myMovement.myAgent.speed = 0;
        }

        if (myBehaviours.idleCounter >= 6)
        {
            myBehaviours.idleCounter = 0;
            myBehaviours.endedPatrol = false;
            myMovement.currentWaypoint = 0;
        }


    }

    public override void OnSleep()
    {
        myBehaviours.idleCounter = 0;
    }
}
