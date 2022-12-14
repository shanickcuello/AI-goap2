using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IdleState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;
    public float time;
    public bool onState;
    
    public override event Action OnNeedsReplan;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
       
    }

    public override void Enter(IState @from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(@from, transitionParameters);
        
        if (!myMovement.statesTriggers[EStates.IDLE])
        {
            myMovement.SetAllStatesToFalse();
            myMovement.statesTriggers[EStates.IDLE] = true;
        }

        onState = true;

        time = 0;
    }

    public override void UpdateLoop()
    {

    }

    private void Update()
    {
        if (onState)
        {
            time += Time.deltaTime;
        }
    }

    public override IState ProcessInput() {
        
        if (time >= 5f) //si pasan 5 segundos sale de idle y replanea
        {
            onState = false;
            OnNeedsReplan?.Invoke();
         
        }

        if (myLineOfSight.playerOnSight) //si ve al player, replanea
        {
            onState = false;
            OnNeedsReplan?.Invoke();
            
        }

        return this;
    }
}