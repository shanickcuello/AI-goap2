using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using UnityEngine.Serialization;

public class GoapEnemy : MonoBehaviour
{
    public PatrolState patrolState;
    public ChaseState chaseState;
    public AttackState attackState;
    public IdleState idleState;
    public ReloadState reloadState;
    public AlertState alertState;

    private FiniteStateMachine _fsm;

    private float _lastReplanTime;
    private float _replanRate = .5f;

    private EnemyWorldState enemyWorldState;

    private void Awake()
    {
        enemyWorldState = GetComponent<EnemyWorldState>();
    }

    void Start()
    {
        patrolState.OnNeedsReplan += OnReplan;
        chaseState.OnNeedsReplan += OnReplan;
        attackState.OnNeedsReplan += OnReplan;
        idleState.OnNeedsReplan += OnReplan;
        reloadState.OnNeedsReplan += OnReplan;
        alertState.OnNeedsReplan += OnReplan;

        //OnlyPlan();
        PlanAndExecute();
    }

    private void PlanAndExecute()
    {
        var actions = new List<GOAPAction> //GOAP goap ENTREGA PARCIAL IA2-parcial 2 
            {
                new GOAPAction("Patrol")
                    .Pre("isPatrolling", true)
                    .Effect("foundPlayer", false)//recalculate
                    .LinkedState(patrolState),
                
                new GOAPAction("Chase")
                    .Pre("isPlayerInSight", false)
                    .Pre("isPlayerInRange", false)
                    .Effect("isPlayerInRange", true)
                    .Effect("foundPlayer", true)
                    .Effect("isPlayerInSight", true)
                    .LinkedState(chaseState),

                new GOAPAction("Attack")
                    .Pre("isPlayerInRange", true)
                    .Pre("isPlayerInSight", true)
                    .Effect("hasBullet", false)
                    .Effect("alarmTriggered", true)
                    .LinkedState(attackState),

                new GOAPAction("ReloadWeapon")
                    .Pre("hasBullet", false)
                    .Effect("hasBullet", true) //replan
                    .LinkedState(reloadState),

                new GOAPAction("Alert")
                    .Pre("alarmTriggered", true)
                    .Pre("hasBullet", true)
                    .Effect("isPlayerInSight", false)
                    .Effect("isPlayerInRange", false)
                    .Effect("alarmTriggered", false)
                    .Effect("isPatrolling", true)
                    .LinkedState(alertState),

                new GOAPAction("Idle")
                    .Pre("isPatrolling", true)
                    .Pre("foundPlayer", false)
                    .Effect("isPlayerAlive", 0)
                    .LinkedState(idleState)
            };
        
            var from = new GOAPState();
            from.values["alarmTriggered"] = false;
            from.values["isPatrolling"] = false;
            from.values["isPlayerInSight"] = false;
            from.values["isPlayerInRange"] = false;
            from.values["hasBullet"] = true;
            from.values["isPlayerAlive"] = enemyWorldState.PlayerLife;
            from.values["missionFinished"] = false;
            from.values["isIdle"] = false;
            from.values["doneRoutine"] = false;
            from.values["foundPlayer"] = false;
            
            var to = new GOAPState();
            to.values["isPlayerAlive"] = 0;

        var planner = new GoapPlanner();

        planner.Run(from, to, actions, OnReplan);

        var plan = planner.Run(from, to, actions, OnReplan); //GOAP goap ENTREGA PARCIAL IA2-parcial 2 

        ConfigureFsm(plan); //GOAP goap ENTREGA PARCIAL IA2-parcial 2 
    }

    private void OnReplan()
    {
        if (Time.time >= _lastReplanTime + _replanRate)
        {
            _lastReplanTime = Time.time;
        }
        else
        {
            return;
        }

        PlanAndExecute(); //GOAP goap ENTREGA PARCIAL IA2-parcial 2 
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.Log("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine); //GOAP goap ENTREGA PARCIAL IA2-parcial 2 
        _fsm.Active = true;
    }
}