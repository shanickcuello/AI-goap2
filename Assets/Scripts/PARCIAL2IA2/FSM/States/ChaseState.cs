using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FSM;
using UnityEngine;

public class ChaseState : MonoBaseState {
    
    public override event Action OnNeedsReplan;
    public EnemyWorldState myWorldState;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public GameObject player;
    public float counter;
    public float sqrDistance;
    public bool onState;

    private void Awake()
    {
        
        myWorldState = GetComponent<EnemyWorldState>();
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void Enter(IState @from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(@from, transitionParameters);
        onState = true;
        myMovement.SetAllStatesToFalse();
        myMovement.statesTriggers[EStates.CHASE] = true;
    }


    public override void UpdateLoop()
    {
        
      
    }

    public void Update()
    {
        if (onState)
        {
            sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
        }

    }

    public override IState ProcessInput() {
        
            if (myWorldState.seenPlayer) //comportamiento si vio al player
            {
                if (sqrDistance <= 6f)
                {
                    onState = false;
                    return  Transitions["AttackState"];
                }
            
                if (sqrDistance >= 100f)
                {
                    onState = false;
                    OnNeedsReplan?.Invoke();
                }
            }

            else //si no lo vi sigo con las transiciones
            {
                onState = false;
                return Transitions["AttackState"];
            } 
        
        return this;

    }

}