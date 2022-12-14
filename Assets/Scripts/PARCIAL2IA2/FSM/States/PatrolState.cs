using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Random = System.Random;

public class PatrolState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public float timeToIdle;
    public override event Action OnNeedsReplan;
    public EnemyWorldState myWorldState;

    public bool onState;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        myWorldState = GetComponent<EnemyWorldState>();

    }
    
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        
        if (!myMovement.statesTriggers[EStates.PATROL])
        {
            myMovement.SetAllStatesToFalse();
            myMovement.statesTriggers[EStates.PATROL] = true;
        }

        onState = true;
        timeToIdle = 0;
    }

    public override void UpdateLoop() {

    }

    private void Update()
    {
        if (onState)
        {
            timeToIdle += Time.deltaTime;
        }
        
    }


    public override IState ProcessInput() {

        if (onState)
        {
            if (timeToIdle >= 10) //cada 10 segundos va a IDLE
            {
                Debug.Log("fui a idle");
                timeToIdle = 0;
                onState = false;
                return Transitions["IdleState"];
            }

            if (myWorldState.seenPlayer)
            {
                onState = false;
                timeToIdle = 0;
                OnNeedsReplan?.Invoke();
            }
 
        }
        
        return this;
    }
}