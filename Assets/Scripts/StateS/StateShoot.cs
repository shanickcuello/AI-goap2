using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateShoot<T> : FSMState<T>
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
 

    public StateShoot(GameObject robot)
    {
        myRobot = robot;
    }

    public override void OnEnter()
    {
        myShootPlayer = myRobot.GetComponent<ShootPlayer>();
        myLineOfSight = myRobot.GetComponent<EnemyLineOfSight>();
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth");
        myPlayerHealth = playerHealth.GetComponent<PlayerHealth>();
        myBehaviours = myRobot.GetComponent<EnemyBehaviours>();
        player = GameObject.FindGameObjectWithTag("Player");
        myMovement = myRobot.GetComponent<EnemyMovement>();
    }
    public override void OnUpdate()
    {
        myMovement.target = myLineOfSight.player.transform;

        if (myBehaviours.shootingPlayerBehaviour == true)
        {
            if (Vector3.Distance(myMovement.target.transform.position, myRobot.transform.position) <= 4f)
            {
                myShootPlayer.shootCount += Time.deltaTime;
                if (myBehaviours.animWeight <= 1) myBehaviours.animWeight += Time.deltaTime;
                myBehaviours.myAnim.SetLayerWeight(1, myBehaviours.animWeight);
                myBehaviours.myAnim.SetBool("PlayerInSight", true);
            }

            if (Vector3.Distance(myMovement.target.transform.position, myRobot.transform.position) > 4f)
            {
                myShootPlayer.shootCount = 2.5f;
            }
        }

    }

    public override void OnSleep()
    {
        myBehaviours.chasePlayerBehaviour = true;
    }
}
