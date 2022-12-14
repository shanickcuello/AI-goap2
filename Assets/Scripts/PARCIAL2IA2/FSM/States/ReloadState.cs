using System;
using System.Collections.Generic;
using System.IO.Compression;
using FSM;
using UnityEngine;

public class ReloadState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;
    public GameObject player;

    public float reloadTime;
    public EnemyWorldState myWorldState;
    public override event Action OnNeedsReplan;
    public float sqrDistance;

    public bool onState;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        myMovement = GetComponent<EnemyMovement>();
        myWorldState = GetComponent<EnemyWorldState>();

    }
    
    public override void Enter(IState @from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(@from, transitionParameters);

        onState = true;
        reloadTime = 0;
        
    }

    public override void UpdateLoop()
    {
       
    }

    private void Update()
    {
        if (onState)
        {
            if (myWorldState.seenPlayer)
            {
                if (myMovement.myAgent.speed >= 0.2f)
                    myMovement.myAgent.speed -= Time.deltaTime / 3;

                if (myMovement.offsetSpeed >= 0f)
                    myMovement.offsetSpeed -= Time.deltaTime / 2;
            
                reloadTime += Time.deltaTime;

                if (reloadTime >= 3)
                    reloadTime = 0;

            }
            
            sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
        }
    }

    public override IState ProcessInput() {
        
        if (!myWorldState.seenPlayer)
        {
            onState = false;
            return Transitions["AlertState"];
        }
        
        if (myLineOfSight.playerOnSight) //si paso el tiempo de recarga y lo ve, replanea
        {
            onState = false; 
            OnNeedsReplan?.Invoke();
            Debug.Log("a");
        }

        if (reloadTime >= 3)
        {
            onState = false;
            return Transitions["AlertState"];
            Debug.Log("b");
        }


        return this;

    }
    
}