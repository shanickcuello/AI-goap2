using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class GoapEnemy : MonoBehaviour
{
   
    public PatrolState patrolState;
    public ChaseState chaseState;
    public AttackState attackState;
    
    private FiniteStateMachine _fsm;

    private float lastReplanTime;
    private float _replanRate = .5f;


    void Start()
    {
        patrolState.OnNeedsReplan += OnReplan;
        chaseState.OnNeedsReplan += OnReplan;

        //OnlyPlan();
        PlanAndExecute();
    }

    private void OnlyPlan()
    {
        SetAndRunPlan();
    }

    private static IEnumerable<GOAPAction> SetAndRunPlan()
    {
        var actions = new List<GOAPAction>{
                                              new GOAPAction(EActions.PATROL)
                                                 .Pre(EEffects.ALARMTRIGGERED, false)
                                                 .Effect(EEffects.PLAYERINSIGHT, true),

                                              new GOAPAction(EActions.CHASE)
                                                 .Pre(EEffects.PLAYERINSIGHT, true)
                                                 .Pre(EEffects.INALERT, false)
                                                 .Effect(EEffects.PLAYERINRANGE, true),

                                              new GOAPAction(EActions.ATTACK)
                                                 .Pre(EEffects.PLAYERINRANGE, true)
                                                 .Pre(EEffects.PLAYERINSIGHT, true)
                                                 .Pre(EEffects.HASBULLET, true)
                                                 .Effect(EEffects.PLAYERALIVE, false)
                                                 .Cost(2f),

                                              new GOAPAction(EActions.IDLE)
                                              .Pre(EEffects.PLAYERALIVE, false)
                                              .Effect(EEffects.INALERT, false)
                                              .Effect(EEffects.FINISHEDMISION, true),

                                              new GOAPAction(EActions.RELOADWEAPON)
                                                 .Pre(EEffects.HASBULLET, false)
                                                 .Pre(EEffects.PLAYERALIVE, false)
                                                 .Effect(EEffects.HASBULLET, true),

                                              new GOAPAction(EActions.ALERT)
                                                 .Pre(EEffects.ALARMTRIGGERED, true)
                                                 .Pre(EEffects.PLAYERALIVE, true)
                                                 .Effect(EEffects.INALERT, true),

        };

        var from = new GOAPState();
        from.values[EEffects.PLAYERINSIGHT] = false;
        from.values[EEffects.PLAYERINRANGE] = false;
        from.values[EEffects.PLAYERALIVE] = true;
        from.values[EEffects.HASBULLET] = true;
        from.values[EEffects.FINISHEDMISION] = false;

        var to = new GOAPState();
        to.values[EEffects.FINISHEDMISION] = true;

        var planner = new GoapPlanner();

        planner.Run(from, to, actions);
        var plan = planner.Run(from, to, actions);

        return plan;
    }

    private void PlanAndExecute() => ConfigureFsm(SetAndRunPlan());

    private void OnReplan()
    {
        if (Time.time >= lastReplanTime + _replanRate)
            lastReplanTime = Time.time;
        else
            return;
        ConfigureFsm(SetAndRunPlan());
    }

    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        Debug.Log("Completed Plan");
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

}
