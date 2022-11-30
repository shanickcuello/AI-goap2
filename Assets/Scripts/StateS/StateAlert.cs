using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAlert<T> : FSMState<T>{

    public GameObject myRobot;
    public EnemyBehaviours myBehaviours;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;
    public StateAlert(GameObject robot)
    {
        myRobot = robot;
    }

    public override void OnEnter()
    {
        myBehaviours = myRobot.GetComponent<EnemyBehaviours>();
        myLineOfSight = myRobot.GetComponent<EnemyLineOfSight>();
        myMovement = myRobot.GetComponent<EnemyMovement>();
    }


    public override void OnUpdate()
    {
        if (myMovement.myAgent.speed >= 0.75f)
            myMovement.myAgent.speed -= Time.deltaTime / 3;

        if (myMovement.offsetSpeed >= 2f)
            myMovement.offsetSpeed -= Time.deltaTime / 2;

        myBehaviours.alerCounter += Time.deltaTime;

        if (myLineOfSight.playerOnSight == true)
        {
            myBehaviours.chaseCounter = 0;
            myBehaviours.alerCounter = 0;

        }

        else if (myLineOfSight.playerOnSight == false)
        {

            if (myBehaviours.chaseCounter > 5)
            {
                myBehaviours.alerCounter = 0;
                myBehaviours.seenPlayer = false;

            }
        }

    }

}
