﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FSM;
using UnityEngine;

public class GoapPlanner
{
    //GOAP goap ENTREGA PARCIAL IA2-parcial 2 

    private const int _WATCHDOG_MAX = 200;

    private int _watchdog;

    public IEnumerable<GOAPAction> Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions, Action onReplan)
    {
        _watchdog = _WATCHDOG_MAX;

        var astar = new AStar<GOAPState>();
        //aca crear un path de emergencia.
        var path = astar.Run(from,
                             state => Satisfies(state, to),
                             node => Explode(node, actions, ref _watchdog),
                             state => GetHeuristic(state, to));
        
            return CalculateGoap(path);
    }

    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine)
    {
        var prevState = plan.First().linkedState;

        var fsm = new FiniteStateMachine(prevState, startCoroutine);

        foreach (var action in plan.Skip(1))
        {
            if (prevState == action.linkedState) continue;
            fsm.AddTransition(action.linkedState.Name, prevState, action.linkedState);

            prevState = action.linkedState;
        }

        return fsm;
    }

    private IEnumerable<GOAPAction> CalculateGoap(IEnumerable<GOAPState> sequence)
    {
        foreach (var act in sequence.Skip(1))
        {
            UnityEngine.Debug.Log(act);
        }

        UnityEngine.Debug.Log("WATCHDOG " + _watchdog);

        return sequence.Skip(1).Select(x => x.generatingAction);
    }

    private static float GetHeuristic(GOAPState from, GOAPState goal) => goal.values.Count(kv => !kv.In(from.values));
    private static bool Satisfies(GOAPState state, GOAPState to) => to.values.All(kv => kv.In(state.values));

    private static IEnumerable<WeightedNode<GOAPState>> Explode(GOAPState node, IEnumerable<GOAPAction> actions,
                                                                ref int watchdog)
    {
        if (watchdog == 0) return Enumerable.Empty<WeightedNode<GOAPState>>();
        watchdog--;

        return actions.Where(action => action.preconditions.All(kv => kv.In(node.values)))
                      .Aggregate(new List<WeightedNode<GOAPState>>(), (possibleList, action) =>
                      {
                          var newState = new GOAPState(node);
                          newState.values.UpdateWith(action.effects);
                          newState.generatingAction = action;
                          newState.step = node.step + 1;

                          possibleList.Add(new WeightedNode<GOAPState>(newState, action.cost));
                          return possibleList;
                      });
    }
}
