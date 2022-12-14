using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class AlertState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;
    public EnemyWorldState myWorldState;

    public float timer;
    
    public override event Action OnNeedsReplan;

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
        
        if (!myMovement.statesTriggers[EStates.ALERT])
        {
            myMovement.SetAllStatesToFalse();
            myMovement.statesTriggers[EStates.ALERT] = true;
        }

        onState = true;
    }

    public override void UpdateLoop() {

    }

    public void Update()
    {

    }


    public override IState ProcessInput() {
        
            if (!myWorldState.seenPlayer) //si no vio al player previamente, va directo a patrol
            {
                onState = false;
                return Transitions["PatrolState"];
            }

            if (myLineOfSight.playerOnSight)
            {
                OnNeedsReplan?.Invoke();
            }

            return this;
    }
}