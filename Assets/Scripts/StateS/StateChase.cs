using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChase<T> : FSMState<T>
    {

    public GameObject myRobot;
    public EnemyBehaviours myBehaviours;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public StateChase(GameObject robot)
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
        myBehaviours.alerCounter = 0;

        myBehaviours.seenPlayer = true;


        if (myMovement.myAgent.speed <= 1f)
            myMovement.myAgent.speed += Time.deltaTime;

        if (myMovement.offsetSpeed <= 3f)
            myMovement.offsetSpeed += Time.deltaTime;

        if (myLineOfSight.playerOnSight == false)
        {

            myBehaviours.chaseCounter += Time.deltaTime;
            if (myBehaviours.animWeight >= 0) myBehaviours.animWeight -= Time.deltaTime;
            myBehaviours.myAnim.SetLayerWeight(1, myBehaviours.animWeight);
            myBehaviours.myAnim.SetBool("PlayerInSight", false);

        }

        if (myLineOfSight.playerOnSight == true)
        {
            myBehaviours.chaseCounter = 0;
            if (myBehaviours.animWeight <= 1) myBehaviours.animWeight += Time.deltaTime;
            myBehaviours.myAnim.SetLayerWeight(1, myBehaviours.animWeight);
            myBehaviours.myAnim.SetBool("PlayerInSight", true);
        }       

    }

    public override void OnSleep()
    {
        myBehaviours.chaseCounter = 0;
    }
}

