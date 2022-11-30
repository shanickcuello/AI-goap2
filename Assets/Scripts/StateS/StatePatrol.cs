using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrol<T> : FSMState<T>
{

    public GameObject myRobot;
    public EnemyBehaviours myBehaviours;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public StatePatrol(GameObject robot)
    {
        myRobot = robot;
    }

    public override void OnEnter()
    {
        myBehaviours = myRobot.GetComponent<EnemyBehaviours>();
        myLineOfSight = myRobot.GetComponent<EnemyLineOfSight>();
        myMovement = myRobot.GetComponent<EnemyMovement>();
        myBehaviours.seekPlayerBehaviour = false;
        myBehaviours.endedSeeking = true;
        myBehaviours.patrolBehaviour = true;
    }

    public override void OnUpdate()
    {
        myBehaviours.alerCounter = 0;
        myBehaviours.chaseCounter = 0;


        if (myMovement.myAgent.speed >= 0.5f)
            myMovement.myAgent.speed -= Time.deltaTime / 3;

        if (myMovement.myAgent.speed < 0.5f)
            myMovement.myAgent.speed += Time.deltaTime;

        if (myMovement.offsetSpeed >= 0f)
            myMovement.offsetSpeed -= Time.deltaTime / 2;


        if (myLineOfSight.playerOnSight == false)
        {
            if (myBehaviours.animWeight >= 0) myBehaviours.animWeight -= Time.deltaTime;
            myBehaviours.myAnim.SetLayerWeight(1, myBehaviours.animWeight);
            myBehaviours.myAnim.SetBool("PlayerInSight", false);
            myBehaviours.seenPlayer = false;
        }
    }

}
